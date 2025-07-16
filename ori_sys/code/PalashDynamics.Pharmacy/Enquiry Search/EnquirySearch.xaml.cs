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
using PalashDynamics.Animations;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using System.Windows.Browser;
using CIMS;
using System.Collections;
using PalashDynamics.Animations;
using System.Collections.ObjectModel;

using PalashDynamics.ValueObjects.Inventory.EnquirySearch;
using PalashDynamics.Collections;
namespace PalashDynamics.Pharmacy.Enquiry_Search
{
    public partial class EnquirySearch : ChildWindow
    {
        public event RoutedEventHandler onOKButton_Click;
        public long storeID { get; set; }
        public long supplierID { get; set; }
        public long EnquiryID { get; set; }
        private ObservableCollection<clsEnquiryDetailsVO> _SelectedItem;
        public ObservableCollection<clsEnquiryDetailsVO> SelectedItems { get { return _SelectedItem; } }
        public PagedSortableCollectionView<clsEnquirySearchVO> DataList { get; private set; }
        public EnquirySearch()
        {
            InitializeComponent();

            ////======================================================
            ////Paging
            //DataList = new PagedSortableCollectionView<clsEnquirySearchVO>();
            //DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            //DataListPageSize = 10;
            ////======================================================

        }



        //#region 'Paging'



        //public int DataListPageSize
        //{
        //    get
        //    {
        //        return DataList.PageSize;
        //    }
        //    set
        //    {
        //        if (value == DataList.PageSize) return;
        //        DataList.PageSize = value;
        //        // RaisePropertyChanged("DataListPageSize");
        //    }
        //}
        //WaitIndicator indicator = new WaitIndicator();

        ///// <summary>
        ///// Handles the OnRefresh event of the DataList control.
        ///// </summary>
        ///// <param name="sender">The source of the event.</param>
        ///// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        //void DataList_OnRefresh(object sender, RefreshEventArgs e)
        //{
        //    FillEnquiryGrid();

        //}



        //#endregion
        protected override void OnClosed(EventArgs e)
        {
          //  ClickedFlag = 0;
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        int ClickedFlag = 0;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag++;
            if (ClickedFlag == 1)
            {
                this.DialogResult = true;
                if (((clsStoreVO)cmbstore.SelectedItem).StoreId != null)
                    storeID = ((clsStoreVO)cmbstore.SelectedItem).StoreId == null ? 0 : ((clsStoreVO)cmbstore.SelectedItem).StoreId;
                supplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID == null ? 0 : ((MasterListItem)cmbSupplier.SelectedItem).ID;
                if (onOKButton_Click != null)
                {
                    onOKButton_Click(this, new RoutedEventArgs());
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            dgEnquiries.ItemsSource = null;
            dgEnquiryItems.ItemsSource = null;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillStore();
            FillSupplier();
            dtpFromDate.SelectedDate = DateTime.Now.Date;
            dtpToDate.SelectedDate = DateTime.Now.Date;


        }

        #region Fill  Supplier dropdown
        private void FillSupplier()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_Supplier;
                BizAction.MasterList = new List<MasterListItem>();

                //if (pClinicID > 0)
                //{
                //    BizAction.Parent = new KeyValue { Value = "ClinicID", Key = pClinicID.ToString() };
                //}

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();

                        //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                        objList.Add(new MasterListItem(0, "--Select--"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                        //cmbBloodGroup.ItemsSource = null;
                        //cmbBloodGroup.ItemsSource = objList;
                        cmbSupplier.ItemsSource = null;
                        cmbSupplier.ItemsSource = objList;



                        if(supplierID > 0)
                            cmbSupplier.SelectedValue = supplierID;
                        else
                        cmbSupplier.SelectedItem = objList[0];


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
        #endregion

        #region Fill  store dropdown
        private void FillStore()
        {
            try
            {
                clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
                //False when we want to fetch all items
                clsItemMasterVO obj = new clsItemMasterVO();
                obj.RetrieveDataFlag = false;
                BizActionObj.ItemMatserDetails = new List<clsStoreVO>();


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


                client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;



                        clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true };
                        BizActionObj.ItemMatserDetails.Insert(0, Default);

                        List<clsStoreVO> objList = BizActionObj.ItemMatserDetails;
                        if (!((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                        {
                            var result = from item in BizActionObj.ItemMatserDetails
                                         where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
                                         select item;
                            objList = (List<clsStoreVO>)result.ToList();
                        }
                        else
                        {
                            var result1 = from item in BizActionObj.ItemMatserDetails
                                          where item.Status == true
                                          select item;
                            //objList = BizActionObj.ItemMatserDetails;
                            objList = result1.ToList();
                        }


                        if (objList != null)
                        {
                            cmbstore.ItemsSource = null;
                            cmbstore.ItemsSource = objList;

                            if (storeID > 0)
                                cmbstore.SelectedValue = storeID;
                            else
                            cmbstore.SelectedItem = objList[0];
                            

                        }




                        //var result = from item in BizActionObj.ItemMatserDetails
                        //             where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                        //             select item;

                        //List<clsStoreVO> objList = (List<clsStoreVO>)result.ToList();
                        //objList.Insert(0, new clsStoreVO { StoreName = " --Select-- " });


                        //cmbstore.ItemsSource = objList;




                        //    cmbstore.SelectedItem = objList[0];









                    }

                };

                client.CloseAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

            FillEnquiryGrid();
            
        }

        private void FillEnquiryGrid()
        {
            try
            {
                clsGetItemEnquiryBizActionVO objBizAction = new clsGetItemEnquiryBizActionVO();

                DateTime FromDate = dtpFromDate.SelectedDate.Value.Date.Date;
                DateTime ToDate = dtpToDate.SelectedDate.Value.Date.Date;
                
                supplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;
                objBizAction.SupplierId = supplierID;
                storeID = ((clsStoreVO)cmbstore.SelectedItem).StoreId;

               

                string FilterExpression = "";

                if (FromDate != null)
                {
                    if (ToDate != null)
                    {
                        if (ToDate.Equals(FromDate))
                        {
                            ToDate = ToDate.Date.AddDays(1);
                        }

                    }
                }
                //Added By Somnath on 05/12/2011 for Search Criteria as Pass in the DAL
                objBizAction.FromDate = FromDate;
                objBizAction.ToDate = ToDate;
                objBizAction.SupplierId = supplierID;
                objBizAction.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
              //  objBizAction.searchStoreID = storeID;

                if (FromDate != null && ToDate != null)
                {
                    if (FilterExpression == "" && FilterExpression.Length == 0)
                    {
                        FilterExpression = FilterExpression + " Date between '" + FromDate + "' and '" + ToDate + "'";
                    }

                }
                if (supplierID != null && supplierID > 0)
                {
                    if (FilterExpression != "" && FilterExpression.Length > 0)
                    {
                        FilterExpression = FilterExpression + " AND SupplierID=" + supplierID;
                    }

                }
                if (storeID != null && storeID > 0)
                {
                    if (FilterExpression != "" && FilterExpression.Length > 0)
                    {
                        FilterExpression = FilterExpression + " AND StoreID=" + storeID;
                    }

                }
                objBizAction.FilterExpression = FilterExpression;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


                client.ProcessAsync(objBizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        //clsGetItemEnquiryBizActionVO objSearchList = ((clsGetItemEnquiryBizActionVO)args.Result);

                        List<clsItemEnquiryVO> objList = ((clsGetItemEnquiryBizActionVO)args.Result).ItemMatserDetail;
                        if (objList != null)
                        {
                            dgEnquiries.ItemsSource = null;
                            dgEnquiries.ItemsSource = objList;

                        }


                    }

                };

                client.CloseAsync();


            }
            catch (Exception)
            {

                throw;
            }
        }

        private void dgEnquiries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                if (dgEnquiries.SelectedIndex != -1)
                {


                    clsItemEnquiryVO objList = ((clsItemEnquiryVO)dgEnquiries.SelectedItem);
                    clsGetItemEnquiryDetailsBizActionVO objBizActionVO = new clsGetItemEnquiryDetailsBizActionVO();

                    if (objList != null)
                    {
                        objBizActionVO.ItemEnquiryId = objList.EnquiryId;
                        EnquiryID = objList.EnquiryId;
                        objBizActionVO.UnitID = objList.UnitId;
                    }

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


                    

                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            clsGetItemEnquiryDetailsBizActionVO objItemList = ((clsGetItemEnquiryDetailsBizActionVO)args.Result);
                            List<clsEnquiryDetailsVO> lstItems = objItemList.ItemMatserDetail;
                            if (objItemList != null)
                            {
                                dgEnquiryItems.ItemsSource = null;
                                dgEnquiryItems.ItemsSource = lstItems;

                            }


                        }

                    };
                    client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }




        private void AddItems_Click(object sender, RoutedEventArgs e)
        {
            if (dgEnquiries.SelectedItem != null)
            {
                if (_SelectedItem == null)
                    _SelectedItem = new ObservableCollection<clsEnquiryDetailsVO>();

                CheckBox chkEnquiry = (CheckBox)sender;
                if (chkEnquiry.IsChecked == true)

                    _SelectedItem.Add((clsEnquiryDetailsVO)dgEnquiryItems.SelectedItem);
                else
                    _SelectedItem.Remove((clsEnquiryDetailsVO)dgEnquiryItems.SelectedItem);

            }
        }

        private void cmdCancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            ClickedFlag = 0;
        }

    }
}

