using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PalashDynamics.Animations;
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
using System.Xml.Linq;
using PalashDynamics.Pharmacy.Inventory;

namespace PalashDynamics.Pharmacy
{
    public partial class ItemMaster : UserControl
    {
        Boolean blnBatchRequired = false;
        Boolean IsPageLoded = false;
        public List<clsItemMasterVO> objItems;
        private SwivelAnimation objAnimation;
        public Boolean ISEditMode = false;
        public Boolean IsSearchButtonClicked = false;
        bool IsCancel = true;
        bool IsNew = false;
        bool Edit = false;
        long CurrentItemId = 0;
        #region 'Paging'

        public PagedSortableCollectionView<clsItemMasterVO> DataList { get; private set; }
        public PagedSortableCollectionView<clsItemMasterVO> DataList1 { get; private set; }
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
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            BindItemListGrid();

        }
        #endregion
        public string ModuleName { get; set; }
        public string Action { get; set; }
        public ItemMaster()
        {
            InitializeComponent();
            this.DataContext = new clsItemMasterVO();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));


            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsItemMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;
            //======================================================

        }
        clsItemMasterVO objItemVO = new clsItemMasterVO();
        private void BindItemListGrid()
        {
            try
            {
                WaitIndicator w = new WaitIndicator();
                w.Show();
                clsGetItemListBizActionVO BizActionObj = new clsGetItemListBizActionVO();
                BizActionObj.ItemDetails = new clsItemMasterVO();
                BizActionObj.ItemDetails.RetrieveDataFlag = true;
                BizActionObj.PagingEnabled = true;
                BizActionObj.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                BizActionObj.MaximumRows = DataList.PageSize;
                BizActionObj.ItemDetails.ItemName = txtSearchItemName.Text;
                BizActionObj.ItemDetails.ItemCode = txtSearchItemCode.Text;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsGetItemListBizActionVO result = args.Result as clsGetItemListBizActionVO;
                        DataList.Clear();
                        DataList.TotalItemCount = result.TotalRowCount;
                        if (result.ItemList != null)
                        {
                            foreach (var item in result.ItemList)
                            {
                                DataList.Add(item);
                            }
                            dgItemList.ItemsSource = null;
                            dgItemList.ItemsSource = DataList;
                            dgDataPager.Source = null;
                            dgDataPager.PageSize = BizActionObj.MaximumRows;
                            dgDataPager.Source = DataList;
                        }
                    }
                    w.Close();
                    if (txtSearchItemCode.Text != "")
                        txtSearchItemCode.Focus();
                    else
                    { txtSearchItemName.Focus(); }

                    
                };
                client.ProcessAsync(BizActionObj, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        void msgW2_OnMessageBoxClosed(MessageBoxResult result)
        {
            try
            {
                if (result == MessageBoxResult.Yes)
                {
                    ItemStore_new win = new ItemStore_new();
                    win.Show();
                    win.IsFromNewItem = true;
                    win.objMasterVO = objItemVO;
                    win.GetItemDetails(objItemVO);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void CmdNew_Click(object sender, RoutedEventArgs e)
        {
           // checkValidation();
            txtItemBrandName.Focus();
            txtItemBrandName.UpdateLayout();
            txtItemCode.IsEnabled = false;
            txtConFact.Text = "1";
            txtConFactStockBase.Text = "1";
            txtConFactBaseSale.Text = "1";
            CmdApplyTax.IsEnabled = false;
            CmdApplySupplier.IsEnabled = false;
            CmdApplyStore.IsEnabled = false;
            CmdApplyItemTax.IsEnabled = false;//***//19
            CmdSave1.IsEnabled = true;
            CmdSave1.Content = "Save";
            CmdSetLocation.IsEnabled = false;
            IsCancel = false;
            CmdClose.IsEnabled = true;
            CmdNew.IsEnabled = false;
            CmdOtherDetail.IsEnabled = false;
            cboSUM.IsEnabled = true;
            cboPUM.IsEnabled = true;
            cboBaseUM.IsEnabled = true;
            cboSellingUM.IsEnabled = true;
            chkBatches.IsEnabled = true;      //Added By Umesh
            chkInclusive.IsEnabled = true;    //Added By Umesh
            CmdLocation.IsEnabled = false;    //Added By Umesh
            txtConFact.IsReadOnly = false;
            CmdConversions.IsEnabled = false;
            cboBaseUM.IsEnabled = true;
            txtExpiryAlert.Text = "0";
            if (txtItemBrandName.Text.Trim() == "")
            {
                txtItemBrandName.SetValidation("Brand Name is required");
                txtItemBrandName.RaiseValidationError();
                txtItemBrandName.Focus();
            }
            else
            {
                txtItemBrandName.ClearValidationError();
            }

            if (txtMRP.Text.Trim() == "0.00" || txtMRP.Text.Trim() == "")
            {
                txtMRP.SetValidation("MRP is required");
                txtMRP.RaiseValidationError();
                txtMRP.Focus();
            }
            else
            {
                txtMRP.ClearValidationError();
            }

            if (txtPurRate.Text.Trim() == "0.00" || txtPurRate.Text.Trim() == "")
            {
                txtPurRate.SetValidation("Cost Price is required");
                txtPurRate.RaiseValidationError();
                txtPurRate.Focus();
            }
            else
            {
                txtPurRate.ClearValidationError();
            }

            if (String.IsNullOrEmpty(txtItemName.Text.Trim()) || (txtItemName.Text.Trim() == ""))
            {
                txtItemName.SetValidation("Item Name is required");
                txtItemName.RaiseValidationError();
                txtItemName.Focus();
            }
            else
            {
                txtItemName.ClearValidationError();
            }

            this.DataContext = new clsItemMasterVO();
            try
            {
                objAnimation.Invoke(RotationType.Forward);
                clsItemMasterVO obj = new clsItemMasterVO();
                //if (obj != null)
                obj.EditMode = false;
                ((clsItemMasterVO)this.DataContext).EditMode = false;
                EmptyUI();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void EmptyUI()
        {
            clsItemMasterVO obj = new clsItemMasterVO();
            IsNew = false;
            Edit = false;
            txtItemBrandName.Text = "";
            txtItemCode.Text = "";
            txtItemName.Text = "";
            txtMRP.Text = "0.00";
            txtPurRate.Text = "0.00";
            txtReorderQuanty.Text = "";
            txtStrength.Text = "";
            txtVatper.Text = "0.00";
            txtDiscountOnSale.Text = "0.00";
            chkBatches.IsChecked = false;
            chkInclusive.IsChecked = false;
            txtHRP.Text = "0.00";
            txtStorageDegree.Text = "0.00";
            cboDispensingType.SelectedValue = obj.DispencingType;
            cboItemCategory.SelectedValue = obj.ItemCategory;
            cboItemGroup.SelectedValue = obj.ItemGroup;
            cboMfg.SelectedValue = obj.MfgBy;
            cboMoleculeName.SelectedValue = obj.MoleculeName;
            cboMrk.SelectedValue = obj.MrkBy;
            cbopregClass.SelectedValue = obj.PregClass;
            cboPUM.SelectedValue = obj.PUM;
            cboBaseUM.SelectedValue = obj.BaseUM;
            cboSellingUM.SelectedValue = obj.SellingUM;
            cboRoute.SelectedValue = obj.Route;
            cboStoreageType.SelectedValue = obj.StoreageType;
            cboSUM.SelectedValue = obj.SUM;
            cboTheraClass.SelectedValue = obj.TherClass;
            ChkABC.IsChecked = false;
            ChkFNS.IsChecked = false;
            ChkVED.IsChecked = false;
            cmbStrengthUnit.SelectedValue = obj.StrengthUnitTypeID;
        }

        private void CmdClose1_Click(object sender, RoutedEventArgs e)
        {

            objAnimation.Invoke(RotationType.Backward);
            CmdNew.IsEnabled = true;


        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                ((clsItemMasterVO)this.DataContext).EditMode = false;
                ((clsItemMasterVO)this.DataContext).ItemCode = "Auto Generated Code";
                txtItemCode.IsEnabled = false;
                CmdApplyTax.IsEnabled = false;
                CmdApplySupplier.IsEnabled = false;
                CmdApplyStore.IsEnabled = false;
                CmdApplyItemTax.IsEnabled = false; //***//19
                CmdSave1.IsEnabled = false;
                CmdSave1.Content = "Save";
                CmdClose.IsEnabled = true;
                CmdNew.IsEnabled = true;
                txtConFact.Text = "1";
                txtConFactStockBase.Text = "1";
                txtConFactBaseSale.Text = "1";
                CmdOtherDetail.IsEnabled = true;
                IsCancel = true;
                BindItemListGrid();
                FillMoleculeName();
                FillItemGroup();
                FillItemCategory();
                FillDispensingType();
                FillStoreageType();
                FillPreganancyClass();
                FillTheraputicClass();
                FillManufacturedBy();
                FillPUM();
                FillSUM();
                FillBaseUM();
                FillSellingUM();
                FillRoute();
                FillStrengthUnit();
                EmptyUI();
                FillHSNCodes(); //Added By Bhushanp For GST 19062017
                IsPageLoded = true;
            }
        }

        private void FillStrengthUnit()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_StrengthUnitMaster;
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
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        cmbStrengthUnit.ItemsSource = null;
                        cmbStrengthUnit.ItemsSource = objList;
                        cmbStrengthUnit.SelectedItem = objList[0];
                    }
                    if (this.DataContext != null)
                    {
                        cmbStrengthUnit.SelectedValue = ((clsItemMasterVO)this.DataContext).StrengthUnitTypeID;
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FillMoleculeName()
        {
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
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }

            catch (Exception ex)
            {

                throw ex;
            }


        }

        private void FillItemGroup()
        {
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

                }

                if (this.DataContext != null)
                {
                    cboItemGroup.SelectedValue = ((clsItemMasterVO)this.DataContext).ItemGroup;
                }

            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillItemCategory()
        {
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

                    }


                    if (this.DataContext != null)
                    {
                        cboItemCategory.SelectedValue = ((clsItemMasterVO)this.DataContext).ItemCategory;
                    }
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
            };

        }

        private void FillStoreageType()
        {
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

                }

                if (this.DataContext != null)
                {
                    cboStoreageType.SelectedValue = ((clsItemMasterVO)this.DataContext).StoreageType;
                }

                client.CloseAsync();
            };

        }

        private void FillPreganancyClass()
        {
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
                }
                if (this.DataContext != null)
                {
                    cbopregClass.SelectedValue = ((clsItemMasterVO)this.DataContext).PregClass;
                }
                client.CloseAsync();
            };
        }
        private void FillTheraputicClass()
        {
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
                }
                if (this.DataContext != null)
                {
                    cboTheraClass.SelectedValue = ((clsItemMasterVO)this.DataContext).TherClass;
                }

                client.CloseAsync();
            };
        }
        private void FillManufacturedBy()
        {
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
                }
                if (this.DataContext != null)
                {
                    cboMfg.SelectedValue = ((clsItemMasterVO)this.DataContext).MfgBy;
                }
                client.CloseAsync();
            };
        }
        private void FillMarketedBy()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ItemCompany;
            //BizAction.Parent = new KeyValue();
            //BizAction.Parent.Key = "0";
            //BizAction.Parent.Value = "MFlag";
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
                    objList.Add(new MasterListItem(0, " -- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    cboMrk.ItemsSource = null;
                    cboMrk.ItemsSource = objList;

                }

                if (this.DataContext != null)
                {
                    cboMrk.SelectedValue = ((clsItemMasterVO)this.DataContext).MrkBy;
                }
                client.CloseAsync();
            };

        }
        private void FillPUM()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_UnitOfMeasure;
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
                    MasterListItem _default = new MasterListItem(0, "-- Select --");

                    objList.Add(_default);
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    cboPUM.ItemsSource = null;
                    cboPUM.ItemsSource = objList;
                    cboPUM.SelectedItem = _default;
                }

                if (this.DataContext != null)
                {
                    cboPUM.SelectedValue = ((clsItemMasterVO)this.DataContext).PUM;
                }
                client.CloseAsync();
            };

        }
        private void FillSUM()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_UnitOfMeasure;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1"; //"1" for ON status
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
                    cboSUM.ItemsSource = null;
                    cboSUM.ItemsSource = objList;
                }
                if (this.DataContext != null)
                {
                    cboSUM.SelectedValue = ((clsItemMasterVO)this.DataContext).SUM;
                }
                client.CloseAsync();
            };
        }

        //** Added by AShish Z.
        private void FillBaseUM()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_UnitOfMeasure;
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
                    MasterListItem _default = new MasterListItem(0, "-- Select --");
                    objList.Add(_default);
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    cboBaseUM.ItemsSource = null;
                    cboBaseUM.ItemsSource = objList;
                    cboBaseUM.SelectedItem = _default;
                }

                if (this.DataContext != null)
                {
                    cboBaseUM.SelectedValue = ((clsItemMasterVO)this.DataContext).PUM;
                }
                client.CloseAsync();
            };
        }

        private void FillSellingUM()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_UnitOfMeasure;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1"; //"1" for ON status
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
                    cboSellingUM.ItemsSource = null;
                    cboSellingUM.ItemsSource = objList;
                }
                if (this.DataContext != null)
                {
                    cboSellingUM.SelectedValue = ((clsItemMasterVO)this.DataContext).SUM;
                }
                client.CloseAsync();
            };
        }
        //**


        private void FillRoute()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_Route;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1"; //"1" for ON status
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

                    cboRoute.ItemsSource = null;
                    cboRoute.ItemsSource = objList;

                }

                if (this.DataContext != null)
                {
                    cboRoute.SelectedValue = ((clsItemMasterVO)this.DataContext).Route;
                }
                client.CloseAsync();
            };
        }
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        public clsAddItemMasterBizActionVO BizActionobj;
        private void CmdSave1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtItemName.Focus();
                if (!Edit)
                    IsNew = true;
                bool isValidated = checkValidation();
                if (isValidated == true)
                {
                    BizActionobj = new clsAddItemMasterBizActionVO();
                    BizActionobj.ItemMatserDetails = new clsItemMasterVO();
                    BizActionobj.ItemMatserDetails.EditMode = ((clsItemMasterVO)this.DataContext).EditMode;
                    BizActionobj.ItemMatserDetails.ID = ((clsItemMasterVO)this.DataContext).ID;
                    BizActionobj.ItemMatserDetails.BrandName = txtItemBrandName.Text == "" ? "" : txtItemBrandName.Text.TrimStart();
                    BizActionobj.ItemMatserDetails.Strength = txtStrength.Text == "" ? "" : txtStrength.Text;
                    BizActionobj.ItemMatserDetails.ItemName = txtItemName.Text.TrimStart();
                    BizActionobj.ItemMatserDetails.MoleculeName = cboMoleculeName.SelectedItem == null ? 0 : ((MasterListItem)cboMoleculeName.SelectedItem).ID;
                    BizActionobj.ItemMatserDetails.ItemGroup = cboItemGroup.SelectedItem == null ? 0 : ((MasterListItem)cboItemGroup.SelectedItem).ID;
                    BizActionobj.ItemMatserDetails.ItemCategory = cboItemCategory.SelectedItem == null ? 0 : ((MasterListItem)cboItemCategory.SelectedItem).ID;
                    BizActionobj.ItemMatserDetails.DispencingType = cboDispensingType.SelectedItem == null ? 0 : ((MasterListItem)cboDispensingType.SelectedItem).ID;
                    BizActionobj.ItemMatserDetails.DispencingTypeString = cboDispensingType.SelectedItem.ToString() == null ? "" : cboDispensingType.SelectedItem.ToString();
                    BizActionobj.ItemMatserDetails.StoreageType = cboStoreageType.SelectedItem == null ? 0 : ((MasterListItem)cboStoreageType.SelectedItem).ID;
                    BizActionobj.ItemMatserDetails.PregClass = cbopregClass.SelectedItem == null ? 0 : ((MasterListItem)cbopregClass.SelectedItem).ID;
                    BizActionobj.ItemMatserDetails.TherClass = cboTheraClass.SelectedItem == null ? 0 : ((MasterListItem)cboTheraClass.SelectedItem).ID;
                    BizActionobj.ItemMatserDetails.MfgBy = cboMfg.SelectedItem == null ? 0 : ((MasterListItem)cboMfg.SelectedItem).ID;
                    BizActionobj.ItemMatserDetails.MrkBy = cboMrk.SelectedItem == null ? 0 : ((MasterListItem)cboMrk.SelectedItem).ID;
                    BizActionobj.ItemMatserDetails.PUM = cboPUM.SelectedItem == null ? 0 : ((MasterListItem)cboPUM.SelectedItem).ID;
                    BizActionobj.ItemMatserDetails.SUM = cboSUM.SelectedItem == null ? 0 : ((MasterListItem)cboSUM.SelectedItem).ID;
                    BizActionobj.ItemMatserDetails.ConversionFactor = txtConFact.Text == "" ? "" : txtConFact.Text;

                    BizActionobj.ItemMatserDetails.BaseUM = cboBaseUM.SelectedItem == null ? 0 : ((MasterListItem)cboBaseUM.SelectedItem).ID;
                    BizActionobj.ItemMatserDetails.SellingUM = cboSellingUM.SelectedItem == null ? 0 : ((MasterListItem)cboSellingUM.SelectedItem).ID;
                    BizActionobj.ItemMatserDetails.ConvFactStockBase = txtConFactStockBase.Text == "" ? "" : txtConFactStockBase.Text;
                    BizActionobj.ItemMatserDetails.ConvFactBaseSale = txtConFactBaseSale.Text == "" ? "" : txtConFactBaseSale.Text;
                    BizActionobj.ItemMatserDetails.ItemExpiredInDays = int.Parse(txtExpiryAlert.Text == "" ? "0" : txtExpiryAlert.Text);

                    BizActionobj.ItemMatserDetails.Route = cboRoute.SelectedItem == null ? 0 : ((MasterListItem)cboRoute.SelectedItem).ID;
                    BizActionobj.ItemMatserDetails.PurchaseRate = decimal.Parse(txtPurRate.Text == "" ? "0.0" : txtPurRate.Text);
                    BizActionobj.ItemMatserDetails.MRP = decimal.Parse(txtMRP.Text == "" ? "0.0" : txtMRP.Text);
                    BizActionobj.ItemMatserDetails.VatPer = decimal.Parse(txtVatper.Text == "" ? "0.0" : txtVatper.Text);
                    BizActionobj.ItemMatserDetails.DiscountOnSale = float.Parse(txtDiscountOnSale.Text == "" ? "0.0" : txtDiscountOnSale.Text);
                    BizActionobj.ItemMatserDetails.ReorderQnt = int.Parse(txtReorderQuanty.Text == "" ? "0" : txtReorderQuanty.Text);
                    BizActionobj.ItemMatserDetails.BatchesRequired = (Boolean)chkBatches.IsChecked ? true : false;
                    BizActionobj.ItemMatserDetails.InclusiveOfTax = (Boolean)chkInclusive.IsChecked ? true : false;
                    BizActionobj.ItemMatserDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    BizActionobj.ItemMatserDetails.Status = true;
                    BizActionobj.ItemMatserDetails.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    BizActionobj.ItemMatserDetails.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    BizActionobj.ItemMatserDetails.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    BizActionobj.ItemMatserDetails.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    BizActionobj.ItemMatserDetails.AddedDateTime = DateTime.Now;
                    BizActionobj.ItemMatserDetails.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    BizActionobj.ItemMatserDetails.UpdateddOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    BizActionobj.ItemMatserDetails.UpdatedDateTime = DateTime.Now;
                    BizActionobj.ItemMatserDetails.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    BizActionobj.ItemMatserDetails.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    BizActionobj.ItemMatserDetails.StorageDegree = decimal.Parse(txtStorageDegree.Text == "" ? "0.0" : txtStorageDegree.Text);
                    BizActionobj.ItemMatserDetails.HighestRetailPrice = decimal.Parse(txtHRP.Text == "" ? "0.0" : txtHRP.Text);
                    if (ChkABC.IsChecked == true)
                        BizActionobj.ItemMatserDetails.IsABC = true;
                    if (ChkFNS.IsChecked == true)
                        BizActionobj.ItemMatserDetails.IsFNS = true;
                    if (ChkVED.IsChecked == true)
                        BizActionobj.ItemMatserDetails.IsVED = true;
                    if ((MasterListItem)cmbStrengthUnit.SelectedItem != null)
                        BizActionobj.ItemMatserDetails.StrengthUnitTypeID = ((MasterListItem)cmbStrengthUnit.SelectedItem).ID;
                    //Added By Bhushanp For GST 19062017
                    if ((MasterListItem)cmbHSNCodes.SelectedItem != null)
                        BizActionobj.ItemMatserDetails.HSNCodesID = ((MasterListItem)cmbHSNCodes.SelectedItem).ID;

                    BizActionobj.ItemMatserDetails.StaffDiscount = float.Parse(txtStaff.Text == "" ? "0.0" : txtStaff.Text);
                    BizActionobj.ItemMatserDetails.WalkinDiscount = float.Parse(txtRegisteredPatients.Text == "" ? "0.0" : txtWalkinPatients.Text);
                    BizActionobj.ItemMatserDetails.RegisteredPatientsDiscount = float.Parse(txtWalkinPatients.Text == "" ? "0.0" : txtRegisteredPatients.Text);   

                    string msgTitle = "";
                    string msgText = "";
                    if (((clsItemMasterVO)this.DataContext).EditMode == true)
                    {
                        msgText = " Are you sure you want to update the item?";
                    }
                    else if (((clsItemMasterVO)this.DataContext).EditMode == false)
                    {
                        msgText = "Are you sure you want to Save the Details?";
                    }
                    MessageBoxControl.MessageBoxChildWindow msgW =
                          new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                    msgW.Show();
                    IsCancel = true;
                }
              //  else { checkValidation(); }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (flag == true)
            {
                BindItemListGrid();
                objItemVO = itemObject;
                if (objItemVO != null)
                {
                    String msgText = "";
                    if (((clsItemMasterVO)this.DataContext).EditMode == false)
                    {
                        msgText = "Do you want to apply store ?";
                        MessageBoxControl.MessageBoxChildWindow msgW2 =
                          new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        msgW2.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW2_OnMessageBoxClosed);
                        msgW2.Show();
                    }
                }
            }
        }
        bool flag = false;
        string msgTitle = "";
        clsItemMasterVO itemObject;
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            try
            {
                if (result == MessageBoxResult.Yes)
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            clsAddItemMasterBizActionVO result1 = arg.Result as clsAddItemMasterBizActionVO;
                            CurrentItemId = result1.ItemID;
                            itemObject = new clsItemMasterVO();
                            itemObject = result1.ItemMatserDetails;
                            if (result1.SuccessStatus == 2)
                            {
                                string msgText = "Item With Same Name Already Exists";
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW.Show();
                                IsCancel = false;
                                return;
                            }
                            if (((clsItemMasterVO)this.DataContext).EditMode == true && result1.SuccessStatus == -2)
                            {
                                //string msgText = txtItemName.Text + " " + "Item Updated Successfully";
                                string msgText = "Record Updated Successfully";
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);
                                msgW.Show();
                                BindItemListGrid();
                            }
                            else if (result1.SuccessStatus == -2 && blnBatchRequired != ((clsItemMasterVO)this.DataContext).BatchesRequired)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot change the batch status as transactions are done.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);
                                msgW.Show();
                                BindItemListGrid();
                            }
                            else
                            {
                                //string msgText = txtItemName.Text + " " + "Item Added Successfully";
                                string msgText = "Record Added Successfully";
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);
                                msgW.Show();
                                BindItemListGrid();
                                flag = true;
                            }
                            EmptyUI();
                            CmdApplyStore.IsEnabled = false;
                            CmdApplyItemTax.IsEnabled = false;//***//19
                            CmdApplySupplier.IsEnabled = false;
                            CmdApplyTax.IsEnabled = false;
                            CmdNew.IsEnabled = true;
                            CmdSave1.IsEnabled = false;
                            CmdClose.IsEnabled = true;
                            CmdOtherDetail.IsEnabled = true;
                            objAnimation.Invoke(RotationType.Backward);

                        }
                        else
                        {
                            string msgText = "Error Occurred while Processing";
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);
                            msgW.Show();
                        }
                    };
                    client.ProcessAsync(BizActionobj, new clsUserVO());
                    client.CloseAsync();
                }
                else
                {
                    IsCancel = false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool checkValidation()
        {
            try
            {
                bool result = true;
                if (txtItemBrandName.Text.Trim() == "")
                {
                    txtItemBrandName.SetValidation("Brand Name is required");
                    txtItemBrandName.RaiseValidationError();
                    txtItemBrandName.Focus();
                    result = false;
                }
                else
                {
                    txtItemBrandName.ClearValidationError();
                }

                if (txtMRP.Text.Trim() == "0.00" || txtMRP.Text.Trim() == "")
                {
                    txtMRP.SetValidation("MRP is required");
                    txtMRP.RaiseValidationError();
                    txtMRP.Focus();
                    result = false;
                }
                else
                {
                    txtMRP.ClearValidationError();
                }

                if (txtPurRate.Text.Trim() == "0.00" || txtPurRate.Text.Trim() == "")
                {
                    txtPurRate.SetValidation("Cost Price is required");
                    txtPurRate.RaiseValidationError();
                    txtPurRate.Focus();
                    result = false;
                }
                else
                {
                    txtPurRate.ClearValidationError();
                }

                if (cboItemCategory.SelectedItem == null)
                {
                    cboItemCategory.TextBox.SetValidation("Item Category is required");
                    cboItemCategory.TextBox.RaiseValidationError();
                    cboItemCategory.Focus();
                    result = false;
                }
                else if (((MasterListItem)cboItemCategory.SelectedItem).ID == 0)
                {
                    cboItemCategory.TextBox.SetValidation("Item Category is required");
                    cboItemCategory.TextBox.RaiseValidationError();
                    cboItemCategory.Focus();
                    result = false;
                }
                else
                {
                    cboItemCategory.TextBox.ClearValidationError();
                }

                if (cboMfg.SelectedItem == null)
                {
                    cboMfg.TextBox.SetValidation("Item Manufacturing Company is required");
                    cboMfg.TextBox.RaiseValidationError();
                    cboMfg.Focus();
                    result = false;
                }

                else if (((MasterListItem)cboMfg.SelectedItem).ID == 0)
                {
                    cboMfg.TextBox.SetValidation("Item Manufacturing Company is required");
                    cboMfg.TextBox.RaiseValidationError();
                    cboMfg.Focus();
                    result = false;
                }
                else
                {
                    cboMfg.TextBox.ClearValidationError();
                }
                if (cboPUM.SelectedItem == null)
                {
                    cboPUM.TextBox.SetValidation("Item Purchase Unit Of Measure is required");
                    cboPUM.TextBox.RaiseValidationError();
                    cboPUM.Focus();
                    result = false;
                }
                else if (((MasterListItem)cboPUM.SelectedItem).ID == 0)
                {
                    cboPUM.TextBox.SetValidation("Item Purchase Unit Of Measure is required");
                    cboPUM.TextBox.RaiseValidationError();
                    cboPUM.Focus();
                    result = false;
                }
                else
                {
                    cboPUM.TextBox.ClearValidationError();
                }

                if (cboBaseUM.SelectedItem == null)
                {
                    cboBaseUM.TextBox.SetValidation("Item Base Unit Of Measure is required");
                    cboBaseUM.TextBox.RaiseValidationError();
                    cboBaseUM.Focus();
                    result = false;
                }
                else if (((MasterListItem)cboBaseUM.SelectedItem).ID == 0)
                {
                    cboBaseUM.TextBox.SetValidation("Item Base Unit Of Measure is required");
                    cboBaseUM.TextBox.RaiseValidationError();
                    cboBaseUM.Focus();
                    result = false;
                }
                else
                {
                    cboBaseUM.TextBox.ClearValidationError();
                }

                if (String.IsNullOrEmpty(txtItemName.Text.Trim()) || (txtItemName.Text.Trim() == ""))
                {
                    txtItemName.SetValidation("Item Name is required");
                    txtItemName.RaiseValidationError();
                    txtItemName.Focus();
                    result = false;
                }
                else
                {
                    txtItemName.ClearValidationError();
                }
                if (cboSUM.SelectedItem == null)
                {
                    cboSUM.TextBox.SetValidation("Item Stocking Unit Of Measure is required");
                    cboSUM.TextBox.RaiseValidationError();
                    cboSUM.Focus();
                    result = false;
                }
                else if (((MasterListItem)cboSUM.SelectedItem).ID == 0)
                {
                    cboSUM.TextBox.SetValidation("Item Stocking Unit Of Measure is required");
                    cboSUM.TextBox.RaiseValidationError();
                    cboSUM.Focus();
                    result = false;
                }
                else
                {
                    cboSUM.TextBox.ClearValidationError();
                }

                if (cboSellingUM.SelectedItem == null)
                {
                    cboSellingUM.TextBox.SetValidation("Item Selling Unit Of Measure is required");
                    cboSellingUM.TextBox.RaiseValidationError();
                    cboSellingUM.Focus();
                    result = false;
                }
                else if (((MasterListItem)cboSellingUM.SelectedItem).ID == 0)
                {
                    cboSellingUM.TextBox.SetValidation("Item Selling Unit Of Measure is required");
                    cboSellingUM.TextBox.RaiseValidationError();
                    cboSellingUM.Focus();
                    result = false;
                }
                else
                {
                    cboSellingUM.TextBox.ClearValidationError();
                }
                //Added By Bhushanp For GST 19062017
                //if (cmbHSNCodes.SelectedItem == null)
                //{
                //    cmbHSNCodes.TextBox.SetValidation("HSN Code is required");
                //    cmbHSNCodes.TextBox.RaiseValidationError();
                //    cmbHSNCodes.Focus();
                //    result = false;
                //}
                //else if (((MasterListItem)cmbHSNCodes.SelectedItem).ID == 0)
                //{
                //    cmbHSNCodes.TextBox.SetValidation("HSN Code is required");
                //    cmbHSNCodes.TextBox.RaiseValidationError();
                //    cmbHSNCodes.Focus();
                //    result = false;
                //}
                //else
                //{
                //    cmbHSNCodes.TextBox.ClearValidationError();
                //}

                if (cboItemGroup.SelectedItem != null && ((MasterListItem)cboItemGroup.SelectedItem).ID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.InventoryCatagoryID)
                {
                    if (cboDispensingType.SelectedItem == null)
                    {
                        cboDispensingType.TextBox.SetValidation("Dispensing Type is required");
                        cboDispensingType.TextBox.RaiseValidationError();
                        cboDispensingType.Focus();
                        result = false;
                    }
                    else if (((MasterListItem)cboDispensingType.SelectedItem).ID == 0)
                    {
                        cboDispensingType.TextBox.SetValidation("Dispensing Type is required");
                        cboDispensingType.TextBox.RaiseValidationError();
                        cboDispensingType.Focus();
                        result = false;
                    }
                    else
                    {
                        cboDispensingType.TextBox.ClearValidationError();
                    }
                    if (txtMRP.Text.Trim() == "")
                    {

                        txtMRP.SetValidation("MRP is required");
                        txtMRP.RaiseValidationError();
                        txtMRP.Focus();
                        result = false;
                    }
                    else if (CIMS.Extensions.IsValueDouble(txtMRP.Text) == false)
                    {

                        txtMRP.SetValidation("MRP should be number");
                        txtMRP.RaiseValidationError();
                        txtMRP.Focus();
                        result = false;

                    }
                    else if (double.Parse(txtMRP.Text) < 0)
                    {
                        txtMRP.SetValidation("MRP should not be negative");
                        txtMRP.RaiseValidationError();
                        txtMRP.Focus();
                        result = false;
                    }
                    else
                    {
                        txtMRP.ClearValidationError();
                    }


                    if (txtHRP.Text.Trim() == "")
                    {
                        txtHRP.SetValidation("Highest Retail Price is required");
                        txtHRP.RaiseValidationError();
                        txtHRP.Focus();
                        result = false;
                    }
                    else if (CIMS.Extensions.IsValueDouble(txtHRP.Text) == false)
                    {

                        txtHRP.SetValidation("Highest Retail Price should be number");
                        txtHRP.RaiseValidationError();
                        txtHRP.Focus();
                        result = false;

                    }
                    else if (double.Parse(txtHRP.Text) < 0)
                    {
                        txtHRP.SetValidation("Highest Retail Price should not be negative");
                        txtHRP.RaiseValidationError();
                        txtHRP.Focus();
                        result = false;
                    }
                    else
                    {
                        txtHRP.ClearValidationError();
                    }
                    if (cboMoleculeName.SelectedItem == null)
                    {
                        cboMoleculeName.TextBox.SetValidation("Molecule Name is required");
                        cboMoleculeName.TextBox.RaiseValidationError();
                        cboMoleculeName.Focus();
                        result = false;
                    }
                    else if (((MasterListItem)cboMoleculeName.SelectedItem).ID == 0)
                    {
                        cboMoleculeName.TextBox.SetValidation("Molecule Name is required");
                        cboMoleculeName.TextBox.RaiseValidationError();
                        cboMoleculeName.Focus();
                        result = false;
                    }
                    else
                    {
                        cboMoleculeName.TextBox.ClearValidationError();
                    }

                    if (CIMS.Extensions.IsValueDouble(txtHRP.Text) == true && CIMS.Extensions.IsValueDouble(txtMRP.Text) == true && CIMS.Extensions.IsValueDouble(txtPurRate.Text) == true)
                    {
                        //if (decimal.Parse(txtMRP.Text) > decimal.Parse(txtHRP.Text))
                        //{
                        //    txtMRP.SetValidation("MRP Should not be greater than Highest Retail Price");
                        //    txtMRP.RaiseValidationError();
                        //    txtMRP.Focus();
                        //    result = false;
                        //}
                        if (decimal.Parse(txtMRP.Text) < decimal.Parse(txtPurRate.Text))
                        {
                            txtMRP.SetValidation("MRP Should be greater than Purchase Price");
                            txtMRP.RaiseValidationError();
                            txtMRP.Focus();
                            result = false;
                        }
                        else
                        {
                            txtMRP.ClearValidationError();
                        }
                    }
                }
                else if (cboMoleculeName != null && cboDispensingType != null && cboMoleculeName.SelectedItem != null && cboDispensingType.SelectedItem != null)
                {
                    cboDispensingType.TextBox.ClearValidationError();
                    cboMoleculeName.TextBox.ClearValidationError();
                }
                if (txtPurRate.Text.Trim() == "" || txtPurRate.Text.Trim() == "0.00")
                {

                    txtPurRate.SetValidation("Purchase Rate is required");
                    txtPurRate.RaiseValidationError();
                    txtPurRate.Focus();
                    result = false;
                }
                else if (CIMS.Extensions.IsValueDouble(txtPurRate.Text) == false)
                {

                    txtPurRate.SetValidation("Purchase Rate should be number");
                    txtPurRate.RaiseValidationError();
                    txtPurRate.Focus();
                    result = false;

                }
                else if (double.Parse(txtPurRate.Text) < 0)
                {
                    txtPurRate.SetValidation("Purchase Rate should not be negative");
                    txtPurRate.RaiseValidationError();
                    txtPurRate.Focus();
                    result = false;
                }
                else
                {
                    txtPurRate.ClearValidationError();
                }
                if (CIMS.Extensions.IsValueDouble(txtVatper.Text) == false)
                {
                    txtVatper.SetValidation("VAT  should be number");
                    txtVatper.RaiseValidationError();
                    txtVatper.Focus();
                    result = false;

                }
                else if (decimal.Parse(txtVatper.Text) < 0)
                {
                    txtVatper.SetValidation("VAT  should not be negative");
                    txtVatper.RaiseValidationError();
                    txtVatper.Focus();
                    result = false;
                }
                else if (decimal.Parse(txtVatper.Text) > 100)
                {
                    txtVatper.SetValidation(" VAT % should not be greater than 100%");
                    txtVatper.RaiseValidationError();
                    txtVatper.Focus();
                    result = false;
                }
                else
                {
                    txtVatper.ClearValidationError();
                }
                if (CIMS.Extensions.IsValueDouble(txtDiscountOnSale.Text) == false)
                {
                    txtDiscountOnSale.SetValidation("Discount on sale should be number");
                    txtDiscountOnSale.RaiseValidationError();
                    txtDiscountOnSale.Focus();
                    result = false;
                }
                else if (decimal.Parse(txtDiscountOnSale.Text) > 100)
                {
                    txtDiscountOnSale.SetValidation("Discount on sale should not be greater than 100%");
                    txtDiscountOnSale.RaiseValidationError();
                    txtDiscountOnSale.Focus();
                    result = false;
                }
                else if (decimal.Parse(txtDiscountOnSale.Text) < 0)
                {
                    txtDiscountOnSale.SetValidation("Discount on sale should not be negative");
                    txtDiscountOnSale.RaiseValidationError();
                    txtDiscountOnSale.Focus();
                    result = false;
                }
                else
                {
                    txtDiscountOnSale.ClearValidationError();
                }
                if (txtReorderQuanty.Text != string.Empty && decimal.Parse(txtReorderQuanty.Text) < 0)
                {
                    txtReorderQuanty.SetValidation("Reorder Quantity should not be negative");
                    txtReorderQuanty.RaiseValidationError();
                    txtReorderQuanty.Focus();
                    result = false;
                }
                else
                {
                    txtReorderQuanty.ClearValidationError();
                }

                if (Convert.ToInt64(txtExpiryAlert.Text.Trim()) > 365)
                {
                    txtExpiryAlert.SetValidation("Expiration Days Shouls Not Greater Than 365 Days");
                    txtExpiryAlert.RaiseValidationError();
                    txtExpiryAlert.Focus();
                    result = false;
                }
                else
                {
                    txtExpiryAlert.ClearValidationError();
                }
                return result;

                
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void dgItemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgItemList.SelectedIndex != -1)
            {

                clsItemMasterVO obj = ((clsItemMasterVO)dgItemList.SelectedItem);
                if (obj != null)
                {
                    //((clsItemMasterVO)this.DataContext).EditMode = true;
                    ((clsItemMasterVO)this.DataContext).ID = obj.ID;
                }

                CmdApplyTax.IsEnabled = true;
                CmdApplySupplier.IsEnabled = true;
                CmdApplyStore.IsEnabled = true;
                CmdApplyItemTax.IsEnabled = true;//***//19
                CmdLocation.IsEnabled = true;    //Added By Umesh
                CmdConversions.IsEnabled = true;
                CmdNew.IsEnabled = true;
                CmdOtherDetail.IsEnabled = true;
                CmdSave1.IsEnabled = false;
                CmdClose.IsEnabled = true;
            }





        }
        private void lnkClinic_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lnkTax_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lnkSupplier_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CmdSearch_Click(object sender, RoutedEventArgs e)
        {
            dgDataPager.PageIndex = 0;
            IsSearchButtonClicked = true;
            BindItemListGrid();

            //try
            //{
            //    //clsItemMasterVO obj = new clsItemMasterVO();
            //    //obj.RetrieveDataFlag = true;

            //    clsGetItemListBizActionVO BizActionObj = new clsGetItemListBizActionVO();
            //    BizActionObj.ItemDetails = new clsItemMasterVO();
            //    //RetrieveDataFlag True when we want to search item
            //    BizActionObj.ItemDetails.RetrieveDataFlag = true;
            //    BizActionObj.ItemDetails.ItemName = txtSearchItemName.Text==""?"":txtSearchItemName.Text;
            //    BizActionObj.ItemDetails.ItemCode = txtSearchItemCode.Text == "" ? "" : txtSearchItemCode.Text;
            //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //    client.ProcessCompleted += (s, args) =>
            //    {
            //        if (args.Error == null && args.Result != null)
            //        {
            //            dgItemList.ItemsSource = null;
            //            dgItemList.ItemsSource = ((clsGetItemListBizActionVO)args.Result).ItemList;
            //            //objItems = new List<clsItemMasterVO>();
            //            //objItems = ((clsGetItemListBizActionVO)args.Result).ItemList;
            //            //txtSearchItemName.ItemsSource = ((clsGetItemListBizActionVO)args.Result).ItemList;

            //        }

            //    };

            //    client.ProcessAsync(BizActionObj, new clsUserVO());
            //    client.CloseAsync();




            //}
            //catch (Exception)
            //{

            //    throw;
            //}
        }

        private void CmdShow_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                clsGetItemListBizActionVO BizActionObj = new clsGetItemListBizActionVO();
                //False when we want to fetch all items
                //clsItemMasterVO obj = new clsItemMasterVO();
                //obj.RetrieveDataFlag = false;
                BizActionObj.ItemDetails = new clsItemMasterVO();
                BizActionObj.ItemDetails.RetrieveDataFlag = false;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        dgItemList.ItemsSource = null;
                        dgItemList.ItemsSource = ((clsGetItemListBizActionVO)args.Result).ItemList;


                    }

                };

                client.ProcessAsync(BizActionObj, new clsUserVO());
                client.CloseAsync();




            }
            catch (Exception)
            {

                throw;
            }
        }

        private void txtItemBrandName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtItemBrandName.Text.ToTitleCase();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            IsNew = false;
            Edit = true;
            clsItemMasterVO objItemVO = new clsItemMasterVO();
            objItemVO = (clsItemMasterVO)dgItemList.SelectedItem;
            this.DataContext = objItemVO;
            //cboPUM.IsEnabled = false;
            //cboSUM.IsEnabled = false;
            if (objItemVO != null)
            {
                blnBatchRequired = ((clsItemMasterVO)this.DataContext).BatchesRequired;
                ((clsItemMasterVO)this.DataContext).EditMode = true;
                ((clsItemMasterVO)this.DataContext).ID = objItemVO.ID;
                txtItemName.Text = ((clsItemMasterVO)this.DataContext).ItemName;
                cboMoleculeName.SelectedValue = objItemVO.MoleculeName;
                cboItemGroup.SelectedValue = objItemVO.ItemGroup;
                cboItemCategory.SelectedValue = objItemVO.ItemCategory;
                cboDispensingType.SelectedValue = objItemVO.DispencingType;
                cboMfg.SelectedValue = objItemVO.MfgBy;
                cboMrk.SelectedValue = objItemVO.MrkBy;
                cbopregClass.SelectedValue = objItemVO.PregClass;
                cboPUM.SelectedValue = objItemVO.PUM;
                cboRoute.SelectedValue = objItemVO.Route;
                cboSUM.SelectedValue = objItemVO.SUM;

                cboBaseUM.SelectedValue = objItemVO.BaseUM;
                cboSellingUM.SelectedValue = objItemVO.SellingUM;
                
                cboTheraClass.SelectedValue = objItemVO.TherClass;
                cboStoreageType.SelectedValue = objItemVO.StoreageType;
                cmbStrengthUnit.SelectedValue = objItemVO.StrengthUnitTypeID;
                ChkABC.IsChecked = objItemVO.IsABC;
                ChkFNS.IsChecked = objItemVO.IsFNS;
                ChkVED.IsChecked = objItemVO.IsVED;
                CmdApplyTax.IsEnabled = false;
                CmdApplySupplier.IsEnabled = false;
                CmdApplyStore.IsEnabled = false;
                CmdApplyItemTax.IsEnabled = false;//***//19
                CmdSave1.IsEnabled = true;
                CmdSave1.Content = "Modify";
                CmdClose.IsEnabled = true;
                CmdNew.IsEnabled = false;
                CmdSetLocation.IsEnabled = false;
                CmdOtherDetail.IsEnabled = false;
                chkBatches.IsEnabled = false;      //Added By Umesh
                chkInclusive.IsEnabled = false;    //Added By Umesh
                txtConFact.IsReadOnly = true;      //Added By Umesh
                CmdLocation.IsEnabled = false;    //Added By Umesh
                CmdConversions.IsEnabled = false;
                cmbHSNCodes.SelectedValue = objItemVO.HSNCodesID; //Added By Bhushanp For GST 19062017
                IsCancel = false;
                if (objItemVO.BaseUM > 0) cboBaseUM.IsEnabled = false;
                objAnimation.Invoke(RotationType.Forward);
            }
        }

        private void CmdApplyTax_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsItemMasterVO objItemVO = new clsItemMasterVO();
                objItemVO = (clsItemMasterVO)dgItemList.SelectedItem;
                if (objItemVO != null)
                {
                    ItemTax win = new ItemTax();
                    win.Show();
                    win.GetItemDetails(objItemVO);
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        private void CmdApplySupplier_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                clsItemMasterVO objItemVO = new clsItemMasterVO();
                objItemVO = (clsItemMasterVO)dgItemList.SelectedItem;
                if (objItemVO != null)
                {
                    ItemSupplier win = new ItemSupplier();
                    win.Show();
                    win.GetItemDetails(objItemVO);

                }


            }
            catch (Exception)
            {

                throw;
            }

        }


        private void CmdApplyStore_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsItemMasterVO objItemVO = new clsItemMasterVO();
                objItemVO = (clsItemMasterVO)dgItemList.SelectedItem;
                if (objItemVO != null)
                {
                    ItemStore_new win = new ItemStore_new();
                    win.flagClickStore = true;
                    win.Show();
                    win.objMasterVO = objItemVO;
                    win.GetItemDetails(objItemVO);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            objAnimation.Invoke(RotationType.Backward);
            //Added By Pallavi
            EmptyUI();
            if (((clsItemMasterVO)this.DataContext).EditMode == true)
                ((clsItemMasterVO)this.DataContext).EditMode = false;


            BindItemListGrid();
            /////////////////////////////////////


            CmdApplyStore.IsEnabled = false;
            CmdApplyItemTax.IsEnabled = false;//***//19
            CmdApplySupplier.IsEnabled = false;
            CmdApplyTax.IsEnabled = false;
            CmdNew.IsEnabled = true;
            CmdSave1.IsEnabled = false;
            CmdClose.IsEnabled = true;
            CmdOtherDetail.IsEnabled = true;
            CmdSetLocation.IsEnabled = true;
            txtExpiryAlert.Text = "0";
            if (IsCancel == true)
            {
                ModuleName = "PalashDynamics.Administration";
                Action = "PalashDynamics.Administration.frmInventoryConfiguration";
                UserControl rootPage = Application.Current.RootVisual as UserControl;

                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Inventory Configuration";

                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                IsCancel = true;
                //BindItemListGrid();
            }
        }

        void c_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {

                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);

                myData = asm.CreateInstance(Action) as UIElement;

                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);




            }
            catch (Exception ex)
            {
                throw;
            }



        }

        private void CmdOtherDetail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgItemList.SelectedItem != null)
                {
                    clsItemMasterOtherDetailsVO objItemVO = new clsItemMasterOtherDetailsVO();
                    //objItemVO.ID = ((clsItemMasterVO)dgItemList.SelectedItem).ID;
                    objItemVO.UnitID = ((clsItemMasterVO)dgItemList.SelectedItem).UnitID;
                    objItemVO.ItemID = ((clsItemMasterVO)dgItemList.SelectedItem).ID;
                    objItemVO.ItemName = ((clsItemMasterVO)dgItemList.SelectedItem).ItemName;


                    if (objItemVO != null)
                    {
                        ContraIndicationSideEffects win = new ContraIndicationSideEffects();
                        win.objItemOtherDetailVO = objItemVO;
                        win.Show();

                    }
                }
                else
                {
                    MessageBoxChildWindow mgbox = new MessageBoxChildWindow("Item is not Selected.", "Item is not Selected.\n Please Select a Item then click on button.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    mgbox.Show();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool CheckDuplicasy()
        {
            clsItemMasterVO objItem;
            clsItemMasterVO objItem1 = null; ;
            if (IsNew)
            {
                //ServiceItem = ((PagedSortableCollectionView<clsServiceMasterVO>)grdServices.ItemsSource).FirstOrDefault(p => p.Code.ToString().ToUpper().Equals(txtCode.Text.ToUpper()));
                if (dgItemList.ItemsSource != null)
                {
                    objItem1 = ((PagedSortableCollectionView<clsItemMasterVO>)dgItemList.ItemsSource).FirstOrDefault(p => p.ItemName.ToUpper().Trim().Equals(txtItemName.Text.ToUpper()));
                }
            }
            else
            {
                // ServiceItem = ((PagedSortableCollectionView<clsServiceMasterVO>)grdServices.ItemsSource).FirstOrDefault(p => p.Code.ToString().ToUpper().Equals(txtCode.Text.ToUpper()) && p.ID != ((clsServiceMasterVO)grdServices.SelectedItem).ID);
                if (dgItemList.ItemsSource != null)
                {
                    objItem1 = ((PagedSortableCollectionView<clsItemMasterVO>)dgItemList.ItemsSource).FirstOrDefault(p => p.ItemName.ToUpper().Trim().Equals(txtItemName.Text.ToUpper()) && p.ID != ((clsItemMasterVO)dgItemList.SelectedItem).ID);
                    Edit = false;
                }
            }

            //if (ServiceItem != null)
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //               new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because CODE already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //    msgW1.Show();
            //    return false;
            //}
            if (objItem1 != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because item name name already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return false;
            }
            else
            {
                return true;
            }
        }

        private void cboPUM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboPUM.SelectedItem != null)
            {
                cboSUM.SelectedValue = ((MasterListItem)cboPUM.SelectedItem).ID;
            }
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
                    ((clsItemMasterVO)this.DataContext).DispencingType = 0;

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

        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValueDouble() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }
        private void txtConversionFactor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValueDouble() || Convert.ToDouble(((TextBox)sender).Text) <= 0 && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }
        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void cmbStrengthUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((MasterListItem)cmbStrengthUnit.SelectedItem).ID != 0)
            {
                string ItemName = txtItemBrandName.Text + ' ' + txtStrength.Text + ' ' + ((MasterListItem)cmbStrengthUnit.SelectedItem).Description;
                if (ItemName.Length > 120)
                {
                    ItemName=ItemName.Substring(0,120);
                    txtItemName.Text = ItemName;
                }
                else
                {
                    txtItemName.Text = txtItemBrandName.Text + ' ' + txtStrength.Text + ' ' + ((MasterListItem)cmbStrengthUnit.SelectedItem).Description;
                }   
            }
            else
            {
                txtItemName.Text = txtItemBrandName.Text + ' ' + txtStrength.Text;
            }
        }

        private void txtBox_KeyUp(object sender, KeyEventArgs e)
        {
          //  if (e.Key == Key.Enter || e.Key == Key.Back || e.Key == Key.Delete)
            if (e.Key == Key.Enter )
            {
                BindItemListGrid();
            }
        }

        private void txtItemName_GotFocus(object sender, RoutedEventArgs e)
        {
            //if (IsNew == true)
            //{
            string ItemName =  txtItemBrandName.Text + ' ' + txtStrength.Text;
            if (ItemName.Length > 120)
            {
                ItemName=ItemName.Substring(0, 120);
                txtItemName.Text = ItemName;
            }
            else
            {
                txtItemName.Text = txtItemBrandName.Text + ' ' + txtStrength.Text;
            }
            
            //}
            //else
            //{
            //    txtItemName.Text =
            //}
        }

        private void CmdApplyLocation_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CmdSetLocation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsItemMasterVO objItemVO = new clsItemMasterVO();
                objItemVO = (clsItemMasterVO)dgItemList.SelectedItem;
                if (objItemVO != null)
                {
                    ApplyLocationToItem win = new ApplyLocationToItem();
                    win.flagClickStore = true;
                    win.Show();
                    win.GetItemDetails(objItemVO);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void txtSearchItemCode_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter || e.Key == Key.Back || e.Key == Key.Delete)
            if (e.Key == Key.Enter )
            {
                BindItemListGrid();
            }
        }

        private void cboBaseUM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboBaseUM.SelectedItem != null)
            {
                cboSellingUM.SelectedValue = ((MasterListItem)cboBaseUM.SelectedItem).ID;
            }

        }

        private void CmdLocation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsItemMasterVO objItemVO = new clsItemMasterVO();
                objItemVO = (clsItemMasterVO)dgItemList.SelectedItem;
                if (objItemVO != null)
                {
                    ItemStoreLocationDetails win = new ItemStoreLocationDetails();
                    win.flagClickStore = true;                    
                    win.Show();
                    win.objMasterVO = objItemVO;
                    win.GetItemDetails(objItemVO);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void txtExpirationdays_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid() || Convert.ToInt64(((TextBox)sender).Text) <= 0 && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void CmdConversions_Click(object sender, RoutedEventArgs e)
        {
            if (dgItemList.SelectedItem != null)
            {
                ItemConversions form = new ItemConversions();
                form.objItemVO = (clsItemMasterVO)dgItemList.SelectedItem;
                form.Show();
            }
        }

        private void txtCostPrice_GotFocus(object sender, RoutedEventArgs e)
        {
            textBefore = "";
            selectionStart = 0;
            selectionLength = 0;
        }


        //Added By Bhushanp For GST 19062017
        private void FillHSNCodes()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_HSNCodes;
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
                        foreach (var item in objList)
                        {
                            item.Description = item.Code;
                        }
                        cmbHSNCodes.ItemsSource = null;
                        cmbHSNCodes.ItemsSource = objList;
                        cmbHSNCodes.SelectedItem = objList[0];

                    }

                    if (this.DataContext != null)
                    {
                        cmbHSNCodes.SelectedValue = ((clsItemMasterVO)this.DataContext).MoleculeName;
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }

            catch (Exception ex)
            {

                throw ex;
            }


        }

        private void CmdApplyItemTax_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsItemMasterVO objItemVO = new clsItemMasterVO();
                objItemVO = (clsItemMasterVO)dgItemList.SelectedItem;
                if (objItemVO != null)
                {
                    ItemTaxDetails win = new ItemTaxDetails();

                    win.Show();
                    win.objMasterVO = objItemVO;
                    //win.GetItemDetails(objItemVO);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

