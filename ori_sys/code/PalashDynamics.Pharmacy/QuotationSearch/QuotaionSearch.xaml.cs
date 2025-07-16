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
using PalashDynamics.ValueObjects.Inventory.Quotation;
namespace PalashDynamics.Pharmacy
{
    public partial class QuotaionSearch : ChildWindow
    {
        private ObservableCollection<clsQuotationDetailsVO> _selectedItems;
        public ObservableCollection<clsQuotationDetailsVO> SelectedItems { get { return _selectedItems; } }
        public event RoutedEventHandler onOKButton_Click;
        public QuotaionSearch()
        {
            InitializeComponent();
            this.DataContext = new clsQuotaionVO();
        }

       
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillSupplier();
            FillStore();
            dtpFromDate.SelectedDate = DateTime.Now.Date;
            dtpToDate.SelectedDate = DateTime.Now.Date;
        }

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



                        clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--select--" };
                        BizActionObj.ItemMatserDetails.Insert(0, Default);

                        List<clsStoreVO> objList = BizActionObj.ItemMatserDetails;

                        if (objList != null)
                        {
                            cmbstore.ItemsSource = objList;
                            if (objList.Count > 1)
                            {
                                cmbstore.SelectedItem = objList[1];
                            }
                            else
                            {
                                cmbstore.SelectedItem = objList[0];
                            }

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
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                        //cmbBloodGroup.ItemsSource = null;
                        //cmbBloodGroup.ItemsSource = objList;
                        cmbSupplier.ItemsSource = null;
                        cmbSupplier.ItemsSource = objList;

                        if (objList.Count > 1)
                            cmbSupplier.SelectedItem = objList[1];
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

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsGetQuotationBizActionVO objBizAction = new clsGetQuotationBizActionVO();

                //clsGetItemEnquiryBizActionVO objBizAction = new clsGetItemEnquiryBizActionVO();
              
                objBizAction.SearchFromDate = dtpFromDate.SelectedDate.Value.Date;
                objBizAction.SearchToDate = dtpToDate.SelectedDate.Value.Date;
                objBizAction.SearchSupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;
                objBizAction.SearchStoreID = ((clsStoreVO)cmbstore.SelectedItem).StoreId;
                objBizAction.IsPagingEnabled = true;
                objBizAction.MaxRows = 20;
                objBizAction.StartIndex = 0;
               


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


               
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<clsQuotaionVO> objList = ((clsGetQuotationBizActionVO)args.Result).QuotaionList;
                        if (objList != null)
                        {
                            dgQuotations.ItemsSource = null;
                            dgQuotations.ItemsSource = objList;

                        }


                    }

                };
                client.ProcessAsync(objBizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

                client.CloseAsync();


            }
            catch (Exception)
            {

                throw;
            }
        }

        private void dgQuotations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

           
            if (dgQuotations.SelectedIndex!=-1)
            {
               clsQuotaionVO objList = (clsQuotaionVO)dgQuotations.SelectedItem;
                if (objList!=null)
                {
                    clsGetQuotationDetailsBizActionVO objBizActionVO = new clsGetQuotationDetailsBizActionVO();
                    objBizActionVO.SearchQuotationID = objList.ID;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);



                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            clsGetQuotationDetailsBizActionVO objQuotationList = ((clsGetQuotationDetailsBizActionVO)args.Result);
                            if (objList != null)
                            {
                                dgQuotationItems.ItemsSource = null;
                                dgQuotationItems.ItemsSource = objQuotationList.QuotaionList;

                            }


                        }

                    };
                    client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);

                    client.CloseAsync();

                }
            }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void chkQuotationStatus_Click(object sender, RoutedEventArgs e)
        {
            if (dgQuotationItems.SelectedItem!=null)
            {
                if (_selectedItems==null)
                
                    _selectedItems = new ObservableCollection<clsQuotationDetailsVO>();
                
                
            
                CheckBox obj = (CheckBox)sender;
                if (obj!=null)
                {
                    if (obj.IsChecked == true)
                        _selectedItems.Add((clsQuotationDetailsVO)dgQuotationItems.SelectedItem);
                    else
                        _selectedItems.Remove((clsQuotationDetailsVO)dgQuotationItems.SelectedItem);
                    

                    
                   
                }
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            if (onOKButton_Click!=null)
            {
                onOKButton_Click(this, new RoutedEventArgs());
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            dgQuotations.ItemsSource = null;
            dgQuotationItems.ItemsSource = null;
        }

        private void cmdCancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }


     
    }
}

