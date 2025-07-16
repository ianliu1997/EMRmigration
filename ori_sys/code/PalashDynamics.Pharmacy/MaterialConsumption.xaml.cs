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
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.Windows.Data;
using PalashDynamics.Collections;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Inventory.MaterialConsumption;
using System.Collections.ObjectModel;
using System.Windows.Browser;
using PalashDynamics.Pharmacy.ItemSearch;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Inventory.BarCode;
using System.Text;
using PalashDynamics.Pharmacy.Inventory;
using OPDModule.Forms;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects.IPD;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using System.Windows.Resources;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using CIMS.Forms;


namespace PalashDynamics.Pharmacy
{
    public partial class MaterialConsumption : UserControl, IInitiateMaterialConsumption
    {
        private SwivelAnimation objAnimation;
        PagedCollectionView pcvItemList;
        #region 'Paging'
        public PagedSortableCollectionView<clsMaterialConsumptionVO> ConsumptionDataList { get; private set; }
        public ObservableCollection<clsMaterialConsumptionItemDetailsVO> MaterialConsumptionAddedItems { get; set; }
        //***//
        public ObservableCollection<clsItemSalesDetailsVO> PharmacyItems { get; set; }
        double vatamt;
        clsBillVO SelectedBill { get; set; }
        //---------
        public int DataListPageSize
        {
            get
            {
                return ConsumptionDataList.PageSize;
            }
            set
            {
                if (value == ConsumptionDataList.PageSize) return;
                ConsumptionDataList.PageSize = value;
            }
        }

        #endregion
        WaitIndicator Indicatior;
        private long PatientUnitID = 0;
        private long PatientID = 0;
        private string MRNO = "";
        private string PatientName = "";
        private bool IsAgainstPatient = false;
        private long Opd_Ipd_External_Id = 0;
        private long Opd_Ipd_External_UnitId = 0;
        private long Opd_Ipd_External = 0;
        //Added by AJ Date 5/1/2017
        public string ModuleName { get; set; }
        public string Action { get; set; }
        private long PatientCategoryID = 0;
        private long CompanyID = 0;
        private long TariffID = 0;
        private long PatientSourceID = 0;
        private long DoctorID = 0;
        private string DoctorName = null;
        private string PharmacyTotal { get; set; }
        private string PharmacyConcession { get; set; }
        private string PharmacyNetAmount { get; set; }
        private string RountOffAmount { get; set; }
        private string PayAmount { get; set; }
        private string TotalBill { get; set; }
        private string TotalConcession { get; set; }
        private string NetAmount1 { get; set; }
        private bool ISFreeze { get; set; }
        private double TotalMRPAmount { get; set; }
        bool IsAgainstIndentReceiveStock { get; set; }

        public bool AgainstDonor = false;
        public long LinkPatientID;
        public long LinkPatientUnitID;
        public long LinkCompanyID;
        public long LinkPatientSourceID;
        public long LinkTariffID;

        public MaterialConsumption()
        {
            PharmacyItems = new ObservableCollection<clsItemSalesDetailsVO>();
            Indicatior = new WaitIndicator();
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            MaterialConsumptionAddedItems = new ObservableCollection<clsMaterialConsumptionItemDetailsVO>();
            ConsumptionDataList = new PagedSortableCollectionView<clsMaterialConsumptionVO>();
            ConsumptionDataList.OnRefresh += new EventHandler<RefreshEventArgs>(ConsumptionDataList_OnRefresh);
            DataListPageSize = 15;
            dpdgMaterailConsumptionList.PageSize = DataListPageSize;
            dpdgMaterailConsumptionList.Source = ConsumptionDataList;
            SetCommandButtonState("New");
            // FillPackage(PatientID, PatientUnitID);

        }

        //Added by AJ Date 5/1/2017
        public void InitiateMaterialConsumption(clsIPDAdmissionVO objAdmission)
        {
            Opd_Ipd_External = 1;
            cmdNew_Click(null, null);
            IsAgainstPatient = true;
            chkAgainstPatient.IsChecked = true;
            chkAgainstPatient.IsEnabled = false;
            btnSearchCriteria.IsEnabled = false;
            cmdCloseM.Visibility = Visibility.Visible;
            PatientID = objAdmission.PatientId;
            PatientUnitID = objAdmission.PatientUnitID;
            Opd_Ipd_External_Id = objAdmission.AdmID;
            Opd_Ipd_External_UnitId = objAdmission.AdmissionUnitID;
            Opd_Ipd_External = 1;
            txtPatientName.Text = objAdmission.PatientName;
            txtMRNo.Text = objAdmission.MRNo;
            TariffID = Convert.ToInt64(objAdmission.TariffID);
            CompanyID = objAdmission.CompanyID;
            PatientSourceID = objAdmission.PatientSourceID;
            DoctorID = objAdmission.DoctorID;
            DoctorName = objAdmission.DoctorName;
            PatientCategoryID = objAdmission.PatientCategoryID;
            FillPackage(PatientID, PatientUnitID);
            FillBillSearchList();
        }
        //***//----------------
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("ClickNew");
            ResetControls();
            if (String.IsNullOrEmpty(txtRemark.Text.Trim()))
            {
                txtRemark.SetValidation("Please, Enter Remark.");
                txtRemark.RaiseValidationError();
                txtRemark.Focus();
                ClickedFlag1 = 0;
            }
            objAnimation.Invoke(RotationType.Forward);
            if (Opd_Ipd_External == 0)
            {
                FillPackage(PatientID, PatientUnitID);
            }
        }
        void ConsumptionDataList_OnRefresh(object sender, RefreshEventArgs e)
        {

            FillMatarialConsumptionList();
        }

        private void FillStore()
        {
            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            clsItemMasterVO obj = new clsItemMasterVO();
            obj.RetrieveDataFlag = false;
            BizActionObj.IsUserwiseStores = true;
            BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizActionObj.ToStoreList = new List<clsStoreVO>();
            BizActionObj.ItemMatserDetails = new List<clsStoreVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;
                    BizActionObj.ToStoreList = ((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList;
                    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true };
                    clsStoreVO select = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true };
                    BizActionObj.ItemMatserDetails.Insert(0, select);
                    BizActionObj.ToStoreList.Insert(0, Default);
                    var result = from item in BizActionObj.ItemMatserDetails
                                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true && item.IsQuarantineStore == false
                                 select item;
                    var NonQSAndUserDefinedStores = from item in BizActionObj.ToStoreList.ToList()
                                                    where item.IsQuarantineStore == false
                                                    select item;

                    NonQSAndUserDefinedStores.ToList().Insert(0, select);

                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        if (result.ToList().Count > 0)
                        {
                            cmbSearchStore.ItemsSource = result.ToList();
                            cmbSearchStore.SelectedItem = result.ToList()[0];
                            cmbStore.ItemsSource = result.ToList();
                            cmbStore.SelectedItem = result.ToList()[0];
                        }
                    }
                    else
                    {
                        if (NonQSAndUserDefinedStores != null)
                        {
                            cmbSearchStore.ItemsSource = NonQSAndUserDefinedStores.ToList();
                            cmbSearchStore.SelectedItem = NonQSAndUserDefinedStores.ToList()[0];
                            cmbStore.ItemsSource = NonQSAndUserDefinedStores.ToList();
                            cmbStore.SelectedItem = NonQSAndUserDefinedStores.ToList()[0];
                        }
                    }
                }
            };
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dpConsumptionDate.SelectedDate = DateTime.Now.Date;
            dpSearchFromDate.SelectedDate = DateTime.Now;
            dpSearchToDate.SelectedDate = DateTime.Now;
            dpConsumptionDate.IsEnabled = false;
            FillStore();
            FillMatarialConsumptionList();

            if (Opd_Ipd_External == 1)
            {
                dpConsumptionDate.SelectedDate = DateTime.Now;
            }
        }

        private void FillMatarialConsumptionList()
        {
            Indicatior.Show();
            try
            {
                clsGetMatarialConsumptionListBizActionVO BizAction = new clsGetMatarialConsumptionListBizActionVO();
                BizAction.ConsumptionList = new List<clsMaterialConsumptionVO>();
                BizAction.FromDate = dpSearchFromDate.SelectedDate;
                BizAction.ToDate = dpSearchToDate.SelectedDate;
                if (BizAction.ToDate != null)
                    BizAction.ToDate = dpSearchToDate.SelectedDate.Value.AddDays(1);
                if (cmbSearchStore.SelectedItem != null)
                {
                    BizAction.StoreId = ((clsStoreVO)cmbSearchStore.SelectedItem).StoreId;
                }
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId; //0;
                }
                else
                {
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }
                #region Paging
                BizAction.IsPagingEnabled = true;
                BizAction.StartIndex = ConsumptionDataList.PageIndex * ConsumptionDataList.PageSize;
                BizAction.MaximumRows = ConsumptionDataList.PageSize;
                #endregion
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        BizAction = (clsGetMatarialConsumptionListBizActionVO)e.Result;
                        BizAction.ConsumptionList = ((clsGetMatarialConsumptionListBizActionVO)e.Result).ConsumptionList;
                        ConsumptionDataList.TotalItemCount = BizAction.TotalRows;
                        ConsumptionDataList.Clear();
                        foreach (var item in BizAction.ConsumptionList)
                        {
                            ConsumptionDataList.Add(item);
                        }
                        dgMaterailConsumptionList.ItemsSource = null;
                        dgMaterailConsumptionList.ItemsSource = ConsumptionDataList;
                        dpdgMaterailConsumptionList.Source = null;
                        dpdgMaterailConsumptionList.PageSize = BizAction.MaximumRows;
                        dpdgMaterailConsumptionList.Source = ConsumptionDataList;
                    }
                    Indicatior.Close();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                Indicatior.Close();
                throw;
            }
        }

        private void ResetControls()
        {
            dpConsumptionDate.SelectedDate = DateTime.Now;
            cmbSearchStore.SelectedValue = (long)0;
            cmbStore.SelectedValue = (long)0;
            dgItemList.ItemsSource = null;
            txtNoOfItems.Text = String.Empty;
            txtTotalAmount.Text = String.Empty;
            txtRemark.Text = String.Empty;
            txtBarCode.Text = String.Empty;
            //Added by AJ Date 9/2/2017
            Opd_Ipd_External_Id = 0;
            Opd_Ipd_External_UnitId = 0;
            Opd_Ipd_External = 0;
            cmbApplicabelPackage.IsEnabled = true; // Added By Bhushanp For New Package Flow 04092017
            //-----------//
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = null;
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            }
        }

        string msgText = "";
        string msgTitle = "PALASHDYNAMICS";
        private void cmdAddItem_Click(object sender, RoutedEventArgs e)
        {
            ItemListNew Itemswin = new ItemListNew();
            Itemswin.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
            Itemswin.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            Itemswin.ShowExpiredBatches = false;
            if (cmbStore.SelectedItem == null)
            {
                msgText = "Please Select Store";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
                cmbStore.Focus();

            }
            else if (((clsStoreVO)cmbStore.SelectedItem).StoreId == 0)
            {
                msgText = "Please Select Store";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
                cmbStore.Focus();
            }
            else
            {
                Itemswin.ShowBatches = true;
                Itemswin.StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
                if ((MasterListItem)cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)
                {
                    if (((MasterListItem)cmbApplicabelPackage.SelectedItem).ApplicableToAll == true) //&& ((MasterListItem)cmbApplicabelPackage.SelectedItem).ApplicableToAllDiscount > 0 Commented By Bhushanp For New Package Changes 04092017
                    {
                        Itemswin.PackageID = 0;
                    }
                    else
                    {
                        Itemswin.PackageID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID;
                    }
                }
                if (this.IsAgainstIndentReceiveStock)
                {
                    Itemswin.PatientID = PatientID;
                    Itemswin.PatientUnitID = PatientUnitID;
                }
                else
                {
                    Itemswin.PatientID = 0;
                    Itemswin.PatientUnitID = 0;
                }
                Itemswin.cmbStore.IsEnabled = false;
                Itemswin.OnSaveButton_Click += new RoutedEventHandler(Itemswin_OnSaveButton_Click);
                Itemswin.Show();
            }
        }
        double vatper;
        void Itemswin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.IsAgainstIndentReceiveStock == false && cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0 && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ConsumableServicesBilled == 0)
                {
                    msgText = "Please Add Media And Consumable Service From Billing.";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                    return;
                }

                String strItemIDs = string.Empty;

                if (dgItemList.ItemsSource == null)
                    dgItemList.ItemsSource = this.MaterialConsumptionAddedItems;

                foreach (clsItembatchSearchVO item in (((ItemListNew)sender).ItemBatchList))
                {
                    String ItemCode = String.Empty, ItemName = String.Empty;
                    ItemCode = item.ItemCode;
                    ItemName = item.ItemName;
                    //clsMaterialConsumptionItemDetailsVO ConsumptedItem = new clsMaterialConsumptionItemDetailsVO()
                    //{
                    //    AvailableStock = (item.AvailableStock),
                    //    BatchCode = item.BatchCode,
                    //    BatchId = item.BatchID,
                    //    ExpiryDate = item.ExpiryDate,
                    //    ItemId = item.ItemID,
                    //    Rate = Convert.ToDecimal(item.PurchaseRate), //Rate = Convert.ToDecimal(item.PurchaseRate) * (decimal)item.StockingToBaseCF,
                    //    MainRate = Convert.ToSingle(item.PurchaseRate) / item.StockingToBaseCF,//item.StockingToBaseCF,
                    //    ItemName = ItemName,
                    //    UsedQty = 0,
                    //    //  ConversionFactor=1,
                    //    //  Amount = Convert.ToDecimal(item.PurchaseRate),
                    //    SUOM = item.SUOM,
                    //    SUOMID = item.SUM,
                    //    BaseUOMID = item.BaseUM,
                    //    // SelectedUOM = new MasterListItem { ID = item.SUM, Description = item.SUOM }
                    //    // SelectedUOM = new MasterListItem { ID = 0, Description = "--Select--" }
                    //    SelectedUOM = new MasterListItem { ID = item.SUM, Description = item.SUOM },

                    //    ConversionFactor = item.StockingToBaseCF / item.StockingToBaseCF, //item.PurchaseToBaseCF / item.StockingToBaseCF,
                    //    BaseConversionFactor = item.StockingToBaseCF,//item.PurchaseToBaseCF
                    //    StockToBase = item.StockingToBaseCF,
                    //    MRP = Math.Round(item.MRP) //Added by AJ Date 18/1/2017
                    //};

                    clsMaterialConsumptionItemDetailsVO ConsumptedItem = new clsMaterialConsumptionItemDetailsVO(); //Added by AJ Date 18/1/2017
                    ConsumptedItem.AvailableStock = (item.AvailableStock);
                    ConsumptedItem.BatchCode = item.BatchCode;
                    ConsumptedItem.BatchId = item.BatchID;
                    ConsumptedItem.ExpiryDate = item.ExpiryDate;
                    ConsumptedItem.ItemId = item.ItemID;
                    ConsumptedItem.Rate = Convert.ToDecimal(item.PurchaseRate);//Rate = Convert.ToDecimal(item.PurchaseRate) * (decimal)item.StockingToBaseCF,
                    ConsumptedItem.MainRate = Convert.ToSingle(item.PurchaseRate) / item.StockingToBaseCF;//item.StockingToBaseCF,
                    ConsumptedItem.ItemName = ItemName;
                    ConsumptedItem.UsedQty = 0;
                    ConsumptedItem.SUOM = item.SUOM;
                    ConsumptedItem.SUOMID = item.SUM;
                    ConsumptedItem.BaseUOMID = item.BaseUM;
                    ConsumptedItem.SelectedUOM = new MasterListItem { ID = item.SUM, Description = item.SUOM };
                    ConsumptedItem.ConversionFactor = item.StockingToBaseCF / item.StockingToBaseCF; //item.PurchaseToBaseCF / item.StockingToBaseCF,
                    ConsumptedItem.BaseConversionFactor = item.StockingToBaseCF;//item.PurchaseToBaseCF
                    ConsumptedItem.StockToBase = item.StockingToBaseCF;
                    //Added By Bhushanp For New Package Flow 31082017                   

                    //ConsumptedItem.MRP = Math.Round(item.MRP); //Commented on Date 29042017
                    ConsumptedItem.MRP = item.MRP; //Added on Date 29042017
                   if (cmbApplicabelPackage.SelectedItem != null) 
                    {
                    if (Opd_Ipd_External == 1 || (((MasterListItem)cmbApplicabelPackage.SelectedItem).ID) > 0 && Opd_Ipd_External == 0)
                    {
                        if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsSellBySellingUnit == true)
                        {
                            ConsumptedItem.SelectedUOM = new MasterListItem(item.SellingUM, item.SellingUMString);

                            float CalculatedFromCF = item.SellingCF / item.StockingCF;
                            ConsumptedItem.ConversionFactor = CalculatedFromCF;
                            ConsumptedItem.BaseConversionFactor = item.SellingCF;
                            ConsumptedItem.SelectedUOM.ID = item.SellingUM;
                            ConsumptedItem.BaseQuantity = 0 * item.SellingCF;
                            ConsumptedItem.MainRate = Convert.ToSingle(item.PurchaseRate) / Convert.ToSingle(item.StockingCF);
                            ConsumptedItem.BaseRate = Convert.ToSingle(item.PurchaseRate) / Convert.ToSingle(item.StockingCF);
                            ConsumptedItem.PurchaseRate = ConsumptedItem.BaseRate * item.SellingCF;
                            ConsumptedItem.MainMRP = Convert.ToSingle(item.MRP) / Convert.ToSingle(item.StockingCF);
                            ConsumptedItem.BaseMRP = Convert.ToSingle(item.MRP) / Convert.ToSingle(item.StockingCF);
                            ConsumptedItem.MRP = ConsumptedItem.BaseMRP * item.SellingCF;
                            ConsumptedItem.Rate = Convert.ToDecimal(ConsumptedItem.BaseRate * item.SellingCF);

                        }
                    }
                    }

                    if (MaterialConsumptionAddedItems.Where(ConsumptedItems => ConsumptedItems.ItemId == item.ItemID).Any() == true)
                    {
                        if (MaterialConsumptionAddedItems.Where(ConsumptedItems => ConsumptedItems.BatchId == item.BatchID).Any() == false)
                            this.MaterialConsumptionAddedItems.Add(ConsumptedItem);
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Item Already Added.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWin.Show();
                        }
                    }
                    else
                        this.MaterialConsumptionAddedItems.Add(ConsumptedItem);
                }
                SumOfTotals();

                foreach (var item2 in MaterialConsumptionAddedItems)
                {
                    strItemIDs = String.Format(strItemIDs + Convert.ToString(item2.ItemId) + ",");

                }
                CheckIndentReceiveQuantityItems(strItemIDs.Trim(','));



                //------------------------------
                if (cmbApplicabelPackage.SelectedItem != null) 
                {
                if (Opd_Ipd_External == 1 || (((MasterListItem)cmbApplicabelPackage.SelectedItem).ID) > 0 && Opd_Ipd_External == 0)
                {
                    if ((((ItemListNew)sender).ItemBatchList) != null)
                    {
                        StringBuilder strError = new StringBuilder();
                        //StoreID = Itemswin.StoreID;
                        foreach (var item in (((ItemListNew)sender).ItemBatchList))
                        {
                            bool Additem = true;
                            if (PharmacyItems != null && PharmacyItems.Count > 0)
                            {
                                var item1 = from r in PharmacyItems
                                            where (r.BatchID == item.BatchID)
                                            select new clsItemSalesDetailsVO
                                            {
                                                Status = r.Status,
                                                ID = r.ID,
                                                ItemName = r.ItemName
                                            };

                                if (item1.ToList().Count > 0)
                                {
                                    if (strError.ToString().Length > 0)
                                        strError.Append(",");
                                    strError.Append(item1.ToList()[0].ItemName);
                                    Additem = false;
                                }
                            }

                            if (Additem)
                            {
                                clsItemSalesDetailsVO ObjAddItem = new clsItemSalesDetailsVO();
                                ObjAddItem.ItemCode = item.ItemCode;
                                ObjAddItem.ItemID = item.ItemID;
                                ObjAddItem.ItemName = item.ItemName;
                                ObjAddItem.Manufacture = item.Manufacturer;
                                ObjAddItem.PregnancyClass = item.PreganancyClass;
                                ObjAddItem.BatchID = item.BatchID;
                                ObjAddItem.BatchCode = item.BatchCode;
                                ObjAddItem.ExpiryDate = item.ExpiryDate;
                                ObjAddItem.Quantity = 1;
                                ObjAddItem.InclusiveOfTax = item.InclusiveOfTax;

                                ObjAddItem.OriginalMRP = ObjAddItem.MRP;
                                ObjAddItem.AvailableQuantity = item.AvailableStock;
                                ObjAddItem.PurchaseRate = item.PurchaseRate;
                                ObjAddItem.ConcessionPercentage = item.DiscountOnSale;

                                ObjAddItem.ConcessionAmount = ObjAddItem.ConcessionAmount;
                                ObjAddItem.Amount = ObjAddItem.Amount;
                                // ObjAddItem.VATPercent = item.VATPerc; //Commented By Bhushanp For vat is not required for GST 
                                vatper = item.VATPerc;
                                ObjAddItem.MRP = item.MRP;
                                ObjAddItem.OriginalMRP = ObjAddItem.MRP;
                                ObjAddItem.ItemVatType = item.ItemVatType;
                                ObjAddItem.AvailableQuantity = item.AvailableStock;
                                ObjAddItem.PurchaseRate = item.PurchaseRate;
                                ObjAddItem.ConcessionPercentage = item.DiscountOnSale;
                                ObjAddItem.ConcessionAmount = ObjAddItem.ConcessionAmount;
                                ObjAddItem.Amount = ObjAddItem.Amount;
                                //Added By Bhushanp For GST 27062017
                                ObjAddItem.SGSTtaxtype = item.SGSTtaxtype;
                                ObjAddItem.SGSTapplicableon = item.SGSTapplicableon;
                                ObjAddItem.CGSTtaxtype = item.CGSTtaxtype;
                                ObjAddItem.CGSTapplicableon = item.CGSTapplicableon;
                                ObjAddItem.IGSTtaxtype = item.IGSTtaxtype;
                                ObjAddItem.IGSTapplicableon = item.IGSTapplicableon;
                                /////
                                //Commented By Bhushanp 
                                //ObjAddItem.SGSTPercent = Convert.ToDouble(item.SGSTPercent);
                                //ObjAddItem.CGSTPercent = Convert.ToDouble(item.CGSTPercent);
                                //ObjAddItem.IGSTPercent = 0;
                                ///////////////////////////////////////////////


                                //ObjAddItem.VATPercent = item.TotalSalesTaxPercent;    
                                ObjAddItem.NetAmount = ObjAddItem.NetAmount;
                                ObjAddItem.Shelfname = item.Shelfname;
                                ObjAddItem.Containername = item.Containername;
                                ObjAddItem.Rackname = item.Rackname;

                                ObjAddItem.AvailableStockInBase = item.AvailableStockInBase;

                                ObjAddItem.StockUOM = item.SUOM;
                                ObjAddItem.PurchaseUOM = item.PUOM;
                                ObjAddItem.PUOM = item.PUOM;
                                ObjAddItem.MainPUOM = item.PUOM;
                                ObjAddItem.SUOM = item.SUOM;

                                ObjAddItem.ConversionFactor = Convert.ToSingle(item.ConversionFactor);
                                ObjAddItem.PUOMID = item.PUM;
                                ObjAddItem.SUOMID = item.SUM;
                                ObjAddItem.BaseUOMID = item.BaseUM;
                                ObjAddItem.BaseUOM = item.BaseUMString;
                                ObjAddItem.SellingUOMID = item.SellingUM;
                                ObjAddItem.SellingUOM = item.SellingUMString;
                                ObjAddItem.MainMRP = Convert.ToSingle(item.MRP);
                                ObjAddItem.MainRate = Convert.ToSingle(item.PurchaseRate);
                                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsSellBySellingUnit == true)
                                {
                                    ObjAddItem.SelectedUOM = new MasterListItem(ObjAddItem.SellingUOMID, ObjAddItem.SellingUOM);

                                    float CalculatedFromCF = item.SellingCF / item.StockingCF;

                                    ObjAddItem.ConversionFactor = CalculatedFromCF;
                                    ObjAddItem.BaseConversionFactor = item.SellingCF;
                                    ObjAddItem.SelectedUOM.ID = item.SellingUM;
                                    ObjAddItem.BaseQuantity = 1 * item.SellingCF;
                                    ObjAddItem.MainRate = Convert.ToSingle(item.PurchaseRate) / Convert.ToSingle(item.StockingCF);
                                    ObjAddItem.BaseRate = Convert.ToSingle(item.PurchaseRate) / Convert.ToSingle(item.StockingCF);
                                    ObjAddItem.PurchaseRate = ObjAddItem.BaseRate * item.SellingCF;
                                    ObjAddItem.MainMRP = Convert.ToSingle(item.MRP) / Convert.ToSingle(item.StockingCF);
                                    ObjAddItem.BaseMRP = Convert.ToSingle(item.MRP) / Convert.ToSingle(item.StockingCF);
                                    ObjAddItem.MRP = ObjAddItem.BaseMRP * item.SellingCF;

                                }
                                else
                                {
                                    ObjAddItem.SelectedUOM = new MasterListItem(0, "--Select--");
                                }
                                //Added By Bhushanp For New Package Flow 31082017
                                if (cmbApplicabelPackage.SelectedItem != null)
                                {
                                    if (((MasterListItem)cmbApplicabelPackage.SelectedItem).ApplicableToAll)
                                    {
                                        ObjAddItem.DiscountOnPackageItem = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ApplicableToAllDiscount;
                                    }
                                    else
                                    {
                                        ObjAddItem.DiscountOnPackageItem = Convert.ToDouble(item.DiscountOnPackageItem);
                                    }
                                }
                                PharmacyItems.Add(ObjAddItem);
                            }
                        }
                        CalculatePharmacySummary();
                        //cmbApplicabelPackage.IsEnabled = false;
                    }
                }
            }
                if (!this.IsAgainstIndentReceiveStock)
                    cmbApplicabelPackage.IsEnabled = false;
            }
            catch (Exception Ex)
            {
                throw;
            }
        }

        public void CheckIndentReceiveQuantityItems(string strItemIDs)
        {
            WaitIndicator indicator = new WaitIndicator();
            indicator.Show();
            clsGetMatarialConsumptionItemListBizActionVO objBizAction = new clsGetMatarialConsumptionItemListBizActionVO();
            objBizAction.objConsumptionVO = new clsMaterialConsumptionVO();
            objBizAction.ItemList = new List<clsMaterialConsumptionItemDetailsVO>();
            objBizAction.IsForPatientIndentReceiveStock = true;
            if (this.IsAgainstIndentReceiveStock == true)
            {
                objBizAction.objConsumptionVO.PatientId = this.PatientID;
                objBizAction.objConsumptionVO.PatientUnitID = this.PatientUnitID;
            }
            else
            {
                objBizAction.objConsumptionVO.PatientId = 0;
                objBizAction.objConsumptionVO.PatientUnitID = 0;
            }


            objBizAction.objConsumptionVO.ItemIDs = strItemIDs;
            objBizAction.objConsumptionVO.StoreID = cmbStore.SelectedItem == null ? 0 : (cmbStore.SelectedItem as clsStoreVO).StoreId;
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        objBizAction = args.Result as clsGetMatarialConsumptionItemListBizActionVO;
                        foreach (var item in MaterialConsumptionAddedItems)
                        {
                            foreach (var itemIndentReceive in objBizAction.ItemList)
                            {
                                if (item.ItemId == itemIndentReceive.ItemId)
                                {
                                    item.TotalPatientIndentReceiveQty = itemIndentReceive.TotalPatientIndentReceiveQty;
                                    item.TotalPatientIndentConsumptionQty = itemIndentReceive.TotalPatientIndentConsumptionQty;
                                }
                            }
                        }
                    }
                };
                client.ProcessAsync(objBizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception Ex)
            {
                indicator.Close();
                throw Ex;
            }
            finally
            {
                indicator.Close();
            }
        }

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    cmdPrint.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    cmdClose.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmbStore.SelectedValue = 0;
                    break;
                case "Save":
                    cmdPrint.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = true;
                    break;
                case "Modify":
                    cmdPrint.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    break;
                case "ClickNew":
                    cmdPrint.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    break;
                default:
                    break;
            }
        }

        int ClickedFlag1 = 0;
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {


            ClickedFlag1 = ClickedFlag1 + 1;
            if (ClickedFlag1 == 1)
            {
                bool isvalidate = true;
                try
                {
                    if (dpConsumptionDate.SelectedDate == null)
                    {
                        dpConsumptionDate.SetValidation("Consumption Date can not be blank.");
                        dpConsumptionDate.RaiseValidationError();
                        dpConsumptionDate.Focus();
                        isvalidate = false;
                        ClickedFlag1 = 0;
                        return;
                    }
                    else if (dpConsumptionDate.SelectedDate < DateTime.Now.Date)
                    {
                        dpConsumptionDate.SetValidation("Consumption date can not be less than today's date.");
                        dpConsumptionDate.RaiseValidationError();
                        dpConsumptionDate.Focus();
                        isvalidate = false;
                        ClickedFlag1 = 0;
                        return;
                    }
                    else
                        dpConsumptionDate.ClearValidationError();

                    if (String.IsNullOrEmpty(txtRemark.Text.Trim()))
                    {
                        txtRemark.SetValidation("Please, Enter Remark.");
                        txtRemark.RaiseValidationError();
                        txtRemark.Focus();
                        isvalidate = false;
                        ClickedFlag1 = 0;
                        return;
                    }
                    else
                        txtRemark.ClearValidationError();

                    if (((clsStoreVO)cmbStore.SelectedItem).StoreId == 0)
                    {
                        cmbStore.TextBox.SetValidation("Please Select The Store");
                        cmbStore.TextBox.RaiseValidationError();
                        cmbStore.Focus();
                        isvalidate = false;
                        ClickedFlag1 = 0;
                        return;
                    }
                    else if (((clsStoreVO)cmbStore.SelectedItem) == null)
                    {
                        cmbStore.TextBox.SetValidation("Please Select The Store");
                        cmbStore.TextBox.RaiseValidationError();
                        cmbStore.Focus();
                        isvalidate = false;
                        ClickedFlag1 = 0;
                        return;
                    }
                    else
                        cmbStore.TextBox.ClearValidationError();

                    if (MaterialConsumptionAddedItems.Count == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Please Select Items.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWin.Show();
                        isvalidate = false;
                        ClickedFlag1 = 0;
                        return;
                    }
                    else
                    {
                        foreach (var item in MaterialConsumptionAddedItems)
                        {
                            if (item.UsedQty == 0)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Used Quantity Can't Be Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWin.Show();
                                isvalidate = false;
                                ClickedFlag1 = 0;
                                return;
                            }

                            if (item.SelectedUOM.ID == 0)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Please Select UOM.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWin.Show();
                                isvalidate = false;
                                ClickedFlag1 = 0;
                                return;
                            }

                            if (item.StockToBase <= 0 || item.ConversionFactor <= 0)
                            {
                                isvalidate = false;
                                MessageBoxControl.MessageBoxChildWindow msgW1;
                                msgW1 = new MessageBoxControl.MessageBoxChildWindow("Validation Error", "Can Not Save, Please Assign Conversion Factor For Item " + "'" + item.ItemName + "'", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                                ClickedFlag1 = 0;
                                return;
                            }

                            if (ISFreeze == true)
                            {
                                isvalidate = false;
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "You are not able to Material Consumption? Pharmacy Bill Is Generated  .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                                ClickedFlag1 = 0;
                                return;
                            }



                            //if (String.IsNullOrEmpty(item.Remark))
                            //{
                            //    FrameworkElement fe = dgItemList.Columns[7].GetCellContent(item);
                            //    FrameworkElement parent = GetParent(fe, typeof(DataGridCell));
                            //    var thisCell = (DataGridCell)parent;
                            //    TextBox txt = thisCell.Content as TextBox;
                            //    txt.SetValidation("Please, Enter Remark.");
                            //    txt.RaiseValidationError();
                            //    txt.Focus();
                            //    ClickedFlag1 = 0;
                            //    Indicatior.Close();
                            //    return;
                            //}
                        }

                        var List = from itemMC in MaterialConsumptionAddedItems
                                   let z = new { zItemID = itemMC.ItemId, zItemName = itemMC.ItemName, zIndentPendingQty = (itemMC.TotalPatientIndentReceiveQty - itemMC.TotalPatientIndentConsumptionQty) }
                                   group itemMC by z into ConsumptionItem
                                   select new { zItemID = ConsumptionItem.Key.zItemID, zItemName = ConsumptionItem.Key.zItemName, zIndentPendingQty = ConsumptionItem.Key.zIndentPendingQty, SumUsedQty = ConsumptionItem.Sum(a => a.BaseOty) };



                        if (this.IsAgainstIndentReceiveStock)
                        {
                            foreach (var item in List.ToList())
                            {
                                if (item.SumUsedQty > item.zIndentPendingQty)
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Sum of Used Qty should not be Greater than Indent Received Qty! Remaining Qty is " + item.zIndentPendingQty + " ,for Item " + item.zItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    msgWin.Show();
                                    isvalidate = false;
                                    break;
                                }
                            }
                        }
                        //else if (this.IsAgainstIndentReceiveStock == false)
                        //{
                        //    foreach (var item in List.ToList())
                        //    {
                        //        if (item.SumUsedQty > item.zIndentPendingQty)
                        //        {
                        //            MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("",
                        //                "Sum of Used Qty should not be Greater than Patient Reserved Indent Qty! Available Qty is " + item.zIndentPendingQty + " ,for Item " + item.zItemName + item.zItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //            msgWin.Show();
                        //            isvalidate = false;
                        //            break;
                        //        }
                        //    }
                        //}

                        if ((((MasterListItem)cmbApplicabelPackage.SelectedItem).PharmacyFixedRate > 0) && Opd_Ipd_External == 0)
                        {
                            if ((((MasterListItem)cmbApplicabelPackage.SelectedItem).TotalPackageAdvance) > 0)
                            {
                                decimal ItemAmount = 0;
                                foreach (var item1 in MaterialConsumptionAddedItems)
                                {
                                    //ItemAmount += item1.Rate * item1.UsedQty;
                                    ItemAmount += Convert.ToDecimal(item1.MRP) * item1.UsedQty;
                                }

                                decimal PackageConsumptionAmount = Convert.ToDecimal(((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageConsumptionAmount);     // Package against Media Consumption
                                decimal TotalPackageAdvance = Convert.ToDecimal(((MasterListItem)cmbApplicabelPackage.SelectedItem).TotalPackageAdvance);
                                decimal OPDConsumption = Convert.ToDecimal(((MasterListItem)cmbApplicabelPackage.SelectedItem).OPDConsumption);                         // Package against Service Billing

                                decimal PharmacyConsumeAmount = Convert.ToDecimal(((MasterListItem)cmbApplicabelPackage.SelectedItem).PharmacyConsumeAmount);   // Package New Changes Added on 30042018

                                decimal PharmacyLimit = Convert.ToDecimal(((MasterListItem)cmbApplicabelPackage.SelectedItem).PharmacyFixedRate);   // Package Pharmacy Component   17052018

                                //double TotalConsumption = Convert.ToDouble(ItemAmount + PackageConsumptionAmount + OPDConsumption);   // For Package New Changes Commented on 30042018
                                //double TotalMaterialConsumption = Convert.ToDouble(ItemAmount + PackageConsumptionAmount);            // For Package New Changes Commented on 30042018

                                double TotalConsumption = 0;
                                double TotalMaterialConsumption = 0;

                                //double PharmacyCheckAmount = 0;

                                if (this.IsAgainstIndentReceiveStock == true)   // For Package New Changes Added on 30042018
                                {
                                    PharmacyLimit = PharmacyLimit - PharmacyConsumeAmount;

                                    if (PharmacyLimit < ItemAmount)     // 67000 < 70000
                                    {
                                        ItemAmount = PharmacyLimit;     // ItemAmount = 67000
                                    }

                                }

                                TotalConsumption = Convert.ToDouble(ItemAmount + PackageConsumptionAmount + OPDConsumption + PharmacyConsumeAmount);    // For Package New Changes Added on 30042018

                                if (this.IsAgainstIndentReceiveStock == true)   // For Package New Changes Added on 30042018
                                {
                                    TotalMaterialConsumption = Convert.ToDouble(ItemAmount + PharmacyConsumeAmount);
                                }
                                else
                                {
                                    TotalMaterialConsumption = Convert.ToDouble(ItemAmount + PackageConsumptionAmount);
                                }

                                if (TotalConsumption < ((MasterListItem)cmbApplicabelPackage.SelectedItem).TotalPackageAdvance)
                                {
                                    if (this.IsAgainstIndentReceiveStock == true)   // For Package New Changes Added on 30042018
                                    {
                                        //if (TotalMaterialConsumption > ((MasterListItem)cmbApplicabelPackage.SelectedItem).PharmacyFixedRate)
                                        //{
                                        //    isvalidate = false;
                                        //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        //    new MessageBoxControl.MessageBoxChildWindow("", " Package Limit Exceeded, You Can Not Add Material Consumption?", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        //    msgW1.Show();
                                        //    ClickedFlag1 = 0;
                                        //    return;
                                        //}
                                    }
                                    else
                                    {
                                        //if (TotalMaterialConsumption > ((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageConsumableLimit)  // For Package New Changes Added on 30042018
                                        //{
                                        //    isvalidate = false;
                                        //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        //    new MessageBoxControl.MessageBoxChildWindow("", " Package Limit Exceeded, You Can Not Add Material Consumption?", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        //    msgW1.Show();
                                        //    ClickedFlag1 = 0;
                                        //    return;
                                        //}
                                    }

                                    // For Package New Changes Commented on 30042018
                                    //if (TotalMaterialConsumption > ((MasterListItem)cmbApplicabelPackage.SelectedItem).PharmacyFixedRate)
                                    //{
                                    //    isvalidate = false;
                                    //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    //    new MessageBoxControl.MessageBoxChildWindow("", " Package Limit Exceeded, You Can Not Add Material Consumption?", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    //    msgW1.Show();
                                    //    ClickedFlag1 = 0;
                                    //    return;
                                    //}

                                }
                                else
                                {
                                    // Commented as per discussion with Abaso and Anant on 14052018
                                    // Uncommented as per discussion with Abaso and Anant on 17052018
                                    if (this.IsAgainstIndentReceiveStock == true)   // For Package New Changes Added on 30042018
                                    {
                                        isvalidate = false;
                                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("", " Patient doesn’t have enough advance. Kindly collect the Advance first", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgW1.Show();
                                        ClickedFlag1 = 0;
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                // Commented as per discussion with Abaso and Anant on 14052018
                                // Uncommented as per discussion with Abaso and Anant on 17052018
                                if (this.IsAgainstIndentReceiveStock == true)   // For Package New Changes Added on 30042018
                                {
                                    isvalidate = false;
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", " Patient doesn’t have enough advance. Kindly collect the Advance first", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                    ClickedFlag1 = 0;
                                    return;
                                }
                            }
                        }




                    }
                    if (isvalidate)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW =
                                new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Are you sure you want to save Consumption details ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                        msgW.OnMessageBoxClosed += (res) =>
                        {
                            if (res == MessageBoxResult.Yes)
                            {
                                Indicatior.Show();
                                var resultSelectedItem = from r in this.MaterialConsumptionAddedItems
                                                         where r.UsedQty > 0
                                                         select r;

                                clsAddMaterialConsumptionBizActionVO BizAction = new clsAddMaterialConsumptionBizActionVO();
                                BizAction.ConsumptionDetails = new clsMaterialConsumptionVO();
                                BizAction.ConsumptionDetails.Date = (DateTime)dpConsumptionDate.SelectedDate;
                                BizAction.ConsumptionDetails.StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
                                BizAction.ConsumptionDetails.PatientId = PatientID;
                                BizAction.ConsumptionDetails.PatientUnitID = PatientUnitID;
                                BizAction.ConsumptionDetails.IsAgainstPatient = IsAgainstPatient;
                                BizAction.ConsumptionDetails.TotalAmount = Convert.ToDecimal(txtTotalAmount.Text);
                                BizAction.ConsumptionDetails.Remark = txtRemark.Text;
                                BizAction.ConsumptionDetails.TotalItems = Convert.ToDecimal(txtNoOfItems.Text);
                                //Added by AJ Date 2/1/2017
                                BizAction.ConsumptionDetails.Opd_Ipd_External_Id = Opd_Ipd_External_Id;
                                BizAction.ConsumptionDetails.Opd_Ipd_External_UnitId = Opd_Ipd_External_UnitId;
                                BizAction.ConsumptionDetails.Opd_Ipd_External = Opd_Ipd_External;
                                BizAction.ConsumptionDetails.LinkPatientID = LinkPatientID;
                                BizAction.ConsumptionDetails.LinkPatientUnitID = LinkPatientUnitID;
                               

                                if (cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)
                                {
                                    BizAction.ConsumptionDetails.PackageID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID;
                                    BizAction.ConsumptionDetails.PharmacyFixedRate = (((MasterListItem)cmbApplicabelPackage.SelectedItem).PharmacyFixedRate);
                                    BizAction.ConsumptionDetails.PackageBillID = (((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageBillID);
                                    BizAction.ConsumptionDetails.PackageBillUnitID = (((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageBillUnitID);

                                    BizAction.ConsumptionDetails.PackageConsumableLimit = (((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageConsumableLimit);
                                }

                                BizAction.ConsumptionDetails.TotalMRPAmount = Convert.ToDecimal(TotalMRPAmount);


                                BizAction.ConsumptionDetails.IsAgainstPatientIndent = this.IsAgainstIndentReceiveStock == true ? true : false;

                                if (this.IsAgainstIndentReceiveStock == false && cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)   // For Package New Changes Added on 30042018
                                {
                                    BizAction.ConsumptionDetails.IsPackageConsumable = true;
                                }

                                # region For Save in Bill And ItemSale

                                clsPaymentVO pPayDetails = new clsPaymentVO();
                                pPayDetails.BillAmount = Convert.ToDouble(PharmacyNetAmount);
                                if (SelectedBill == null)
                                {
                                    //pPayDetails.BillBalanceAmount = Convert.ToDouble(RountOffAmount);
                                    pPayDetails.BillBalanceAmount = Math.Round(Convert.ToDouble(RountOffAmount));
                                }
                                else
                                {
                                    //pPayDetails.BillBalanceAmount = SelectedBill.NetBillAmount + Convert.ToDouble(RountOffAmount);
                                    pPayDetails.BillBalanceAmount = SelectedBill.NetBillAmount + Math.Round(Convert.ToDouble(RountOffAmount));
                                }
                                pPayDetails.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstBill;
                                clsBillVO objBill = new clsBillVO();
                                if (SelectedBill == null)
                                {
                                    objBill.Date = DateTime.Now;
                                    objBill.Time = DateTime.Now;
                                }
                                else
                                    objBill = SelectedBill.DeepCopy();

                                //objBill.IsFreezed = false;        // For Package New Changes Commented on 02052018
                                //objBill.IsFreezed = true;         // For Package New Changes modified on 02052018      // Commented on 25Feb2019 for Package Flow in IPD 

                                if (this.Opd_Ipd_External == 1)     // Added on 25Feb2019 for Package Flow in IPD
                                {
                                    objBill.IsFreezed = false;  
                                }
                                else
                                {
                                    objBill.IsFreezed = true;  
                                }

                                objBill.PatientID = PatientID;
                                objBill.Opd_Ipd_External_Id = Opd_Ipd_External_Id;
                                objBill.Opd_Ipd_External_UnitId = Opd_Ipd_External_UnitId;

                                //objBill.Opd_Ipd_External = 1;     // For Package New Changes Commented on 30042018

                                if (this.Opd_Ipd_External == 1)     // For Package New Changes Added on 30042018
                                {
                                    objBill.Opd_Ipd_External = 1;
                                }
                                else
                                {
                                    objBill.Opd_Ipd_External = 0;
                                }

                                objBill.LinkPatientID = LinkPatientID;
                                objBill.LinkPatientUnitID = LinkPatientUnitID;
                                objBill.AgainstDonor = AgainstDonor;
                                objBill.DoctorID = DoctorID;
                                objBill.TariffId = TariffID;
                                objBill.CompanyId = CompanyID;
                                objBill.PatientSourceId = PatientSourceID;
                                objBill.PatientCategoryId = PatientCategoryID;
                                //if (pFreezBill)
                                //{
                                //objBill.Date = DateTime.Now;
                                // objBill.Time = DateTime.Now;

                                if (pPayDetails != null)
                                {
                                    objBill.BalanceAmountSelf = pPayDetails.BillBalanceAmount;
                                    objBill.BillPaymentType = pPayDetails.BillPaymentType;
                                }
                                objBill.SelfAmount = Convert.ToDouble(PayAmount);
                                //}

                                if (SelectedBill == null)
                                {
                                    objBill.TotalBillAmount = Convert.ToDouble(TotalBill);
                                    objBill.TotalConcessionAmount = Convert.ToDouble(TotalConcession);
                                    objBill.CalculatedNetBillAmount = Convert.ToDouble(NetAmount1);
                                    //objBill.NetBillAmount = Convert.ToDouble(RountOffAmount);
                                    objBill.NetBillAmount = Math.Round(Convert.ToDouble(RountOffAmount));
                                }
                                else
                                {

                                    objBill.TotalBillAmount = Convert.ToDouble(TotalBill) + SelectedBill.TotalBillAmount;
                                    objBill.TotalConcessionAmount = Convert.ToDouble(TotalConcession) + SelectedBill.TotalConcessionAmount;
                                    objBill.CalculatedNetBillAmount = Convert.ToDouble(NetAmount1) + SelectedBill.NetBillAmount;
                                    // objBill.NetBillAmount = Convert.ToDouble(RountOffAmount) + SelectedBill.NetBillAmount;
                                    objBill.NetBillAmount = Math.Round(Convert.ToDouble(RountOffAmount)) + SelectedBill.NetBillAmount;
                                }                                

                                if (PatientSourceID > 0)
                                {
                                    objBill.PatientSourceId = PatientSourceID;
                                }
                                else
                                {
                                    if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
                                        objBill.PatientSourceId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;

                                }

                                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                                    objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;  //Costing Divisions for Pharmacy Billing
                                else
                                    objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyCounterID;

                                if (objBill.PaymentDetails != null)
                                {
                                    objBill.PaymentDetails.CostingDivisionID = objBill.CostingDivisionID;   //Costing Divisions for Pharmacy Billing
                                }


                                if (PharmacyItems.Count > 0)
                                {
                                    foreach (var itemP in PharmacyItems)
                                    {
                                        if (cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)
                                        {
                                            itemP.PackageBillID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageBillID;
                                            itemP.PackageBillUnitID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageBillUnitID;
                                            itemP.PackageID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID;
                                        }
                                    }

                                    objBill.PharmacyItems.Items = new List<clsItemSalesDetailsVO>();
                                    objBill.PharmacyItems.ISForMaterialConsumption = true;
                                    objBill.PharmacyItems.VisitID = Opd_Ipd_External_Id;
                                    objBill.PharmacyItems.PatientID = PatientID;
                                    objBill.PharmacyItems.PatientUnitID = PatientUnitID;
                                    objBill.PharmacyItems.Date = DateTime.Now;
                                    objBill.PharmacyItems.Time = DateTime.Now;
                                    objBill.PharmacyItems.StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
                                    objBill.PharmacyItems.CostingDivisionID = objBill.CostingDivisionID;
                                    objBill.PharmacyItems.VATAmount = vatamt;
                                    objBill.PharmacyItems.VATPercentage = vatper;
                                    objBill.PharmacyItems.TotalAmount = Convert.ToDouble(PharmacyTotal);
                                    objBill.PharmacyItems.ConcessionAmount = Convert.ToDouble(PharmacyConcession);
                                    //objBill.PharmacyItems.NetAmount = Convert.ToDouble(RountOffAmount);
                                    //objBill.PharmacyItems.
                                    objBill.PharmacyItems.NetAmount = Math.Round(Convert.ToDouble(RountOffAmount));
                                    objBill.PharmacyItems.ReferenceDoctorID = DoctorID;
                                    objBill.PharmacyItems.ReferenceDoctor = DoctorName;
                                    objBill.BillType = PalashDynamics.ValueObjects.BillTypes.Pharmacy;
                                    objBill.PharmacyItems.PatientID = PatientID;
                                    objBill.PharmacyItems.PatientUnitID = PatientUnitID;
                                    objBill.PharmacyItems.Opd_Ipd_External_Id = Opd_Ipd_External_Id;
                                    objBill.PharmacyItems.Opd_Ipd_External_UnitId = Opd_Ipd_External_UnitId;
                                    objBill.PharmacyItems.Items = PharmacyItems.ToList();
                                }

                                # endregion

                                BizAction.ConsumptionDetails.ConsumptionItemDetailsList = new List<clsMaterialConsumptionItemDetailsVO>();
                                BizAction.ConsumptionDetails.ConsumptionItemDetailsList = resultSelectedItem.ToList<clsMaterialConsumptionItemDetailsVO>();
                                BizAction.ObjBillDetails = new clsAddBillBizActionVO();
                                BizAction.ObjBillDetails.Details = new clsBillVO();
                                BizAction.ObjBillDetails.IsFromCounterSale = true;
                                BizAction.ObjBillDetails.Details = objBill;

                                if (cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)
                                {
                                    BizAction.ObjBillDetails.IsCouterSalesPackage = true;
                                }

                                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                Client.ProcessCompleted += (s, e1) =>
                                {
                                    ClickedFlag1 = 0;
                                    if (e1.Error == null && e1.Result != null && ((clsAddMaterialConsumptionBizActionVO)e1.Result).ConsumptionDetails != null)
                                    {
                                        Indicatior.Close();
                                        MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Consumption details saved successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgWin.Show();
                                        chkAgainstPatient.IsChecked = false;
                                        btnSearchCriteria.IsEnabled = false;
                                        PatientUnitID = 0;
                                        PatientID = 0;
                                        Opd_Ipd_External_Id = 0;
                                        Opd_Ipd_External_UnitId = 0;
                                        Opd_Ipd_External = 0;
                                        IsAgainstPatient = false;
                                        txtMRNo.Text = "";
                                        txtPatientName.Text = "";
                                        FillMatarialConsumptionList();
                                        SetCommandButtonState("New");
                                    }
                                    else
                                    {
                                        Indicatior.Close();
                                        MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Some error occurred while saving", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgWin.Show();
                                    }
                                };
                                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                Client.CloseAsync();
                                objAnimation.Invoke(RotationType.Backward);
                            }
                            else
                                ClickedFlag1 = 0;
                        };
                        msgW.Show();
                    }
                    else
                    {
                        ClickedFlag1 = 0;
                        Indicatior.Close();
                    }

                }
                catch (Exception ex)
                {
                    Indicatior.Close();
                    ClickedFlag1 = 0;
                    throw;
                }
            }
        }

        private void dgMaterailConsumptionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dgMaterailConsumptionItemList.ItemsSource = null;
            FillItemListByConsumptionId();
        }
        private void FillItemListByConsumptionId()
        {
            if (dgMaterailConsumptionList.SelectedItem != null)
            {
                clsGetMatarialConsumptionItemListBizActionVO BizAction = new clsGetMatarialConsumptionItemListBizActionVO();
                BizAction.ConsumptionID = ((clsMaterialConsumptionVO)dgMaterailConsumptionList.SelectedItem).ID;
                BizAction.UnitID = ((clsMaterialConsumptionVO)dgMaterailConsumptionList.SelectedItem).UnitID;
                //    clsMaterialConsumptionItemDetailsVO Detail = new clsMaterialConsumptionItemDetailsVO();
                //    Detail.Flag = true;
                BizAction.ItemList = new List<clsMaterialConsumptionItemDetailsVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        BizAction.ItemList = ((clsGetMatarialConsumptionItemListBizActionVO)e.Result).ItemList;
                        dgMaterailConsumptionItemList.ItemsSource = null;
                        dgMaterailConsumptionItemList.ItemsSource = BizAction.ItemList;
                        //  Detail.Flag = false;
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            else
            {
                dgMaterailConsumptionItemList.ItemsSource = null;
            }
        }

        private void dgItemList_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            try
            {
                if (e.Column.DisplayIndex == 4)//Used Quantity
                {
                    if (((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UsedQty == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                          new MessageBoxControl.MessageBoxChildWindow("", "Please Enter Used Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                    }

                    else if (((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).SelectedUOM != null && ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).SelectedUOM.ID == ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).BaseUOMID && (((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UsedQty % 1) != 0)
                    {
                        ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UsedQty = 0;
                        string msgText = "Quantity Cannot be in fraction";

                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD.Show();

                    }
                    if (((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).SelectedUOM != null && ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).SelectedUOM.ID > 0)
                    {
                        if ((((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UsedQty * Convert.ToDecimal(((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).ConversionFactor) * Convert.ToDecimal(((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).StockToBase))
                            > (Math.Round((decimal)((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).AvailableStock, 3) * (decimal)((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).StockToBase))
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                              new MessageBoxControl.MessageBoxChildWindow("", "Please Enter Used Quantity Less Than Or Equal To Available Stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            //((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UsedQty = (decimal)((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).AvailableStock / (decimal)((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).BaseConversionFactor;
                            ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UsedQty = (decimal)Convert.ToSingle(Math.Floor(Convert.ToDouble(((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).AvailableStock / ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).BaseConversionFactor)));
                        }
                        else
                        {
                            ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).StockOty = Convert.ToDecimal(Convert.ToSingle(((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UsedQty) * ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).ConversionFactor);
                            ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).BaseOty = Convert.ToSingle(Convert.ToSingle(((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UsedQty) * ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).BaseConversionFactor);

                            if (this.IsAgainstIndentReceiveStock == true)
                            {
                                ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).Amount = Convert.ToDecimal(((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UsedQty) * Convert.ToDecimal(((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).MRP);   //***//
                            }
                            else
                            {
                            ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).Amount = Convert.ToDecimal(((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UsedQty) * ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).Rate;
                            }

                            for (int i = 0; i < PharmacyItems.Count; i++) //Added by AJ Date 30/1/2017
                            {
                                if ((PharmacyItems[i].BatchID) == ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).BatchId && (PharmacyItems[i].ItemID) == ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).ItemId)
                                {
                                    (PharmacyItems[i].BaseQuantity) = Convert.ToSingle(((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UsedQty) * ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).BaseConversionFactor;
                                    (PharmacyItems[i].Quantity) = Convert.ToSingle(((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UsedQty);

                                }
                            }


                        }

                    }
                    else
                    {
                        CalculateConversionFactorCentral(((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).SelectedUOM.ID, ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).SUOMID);//By Umesh

                        // CalculateConversionFactorCentral2(((clsItemSalesDetailsVO)dgItemList.SelectedItem).SelectedUOM.ID, ((clsItemSalesDetailsVO)dgItemList.SelectedItem).SUOMID); //Added by AJ Date 30/1/2017 

                        if (((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UsedQty > (decimal)((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).AvailableStock)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                              new MessageBoxControl.MessageBoxChildWindow("", "Please Enter Used Quantity Less Than Or Equal To Available Stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            // ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UsedQty = (decimal)((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).AvailableStock;
                        }
                    }

                    clsMaterialConsumptionItemDetailsVO objCD = new clsMaterialConsumptionItemDetailsVO();
                    objCD = (clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem;

                    if (objCD.TotalPatientIndentReceiveQty > 0)
                    {
                        if (this.IsAgainstIndentReceiveStock)
                        {
                            if (objCD.BaseOty > (objCD.TotalPatientIndentReceiveQty - objCD.TotalPatientIndentConsumptionQty))
                            {
                                MessageBoxControl.MessageBoxChildWindow msgWD =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Used Qty should not be Greater than Indent Received Qty! Remaining Qty is " + (objCD.TotalPatientIndentReceiveQty - objCD.TotalPatientIndentConsumptionQty), MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWD.Show();
                                objCD.UsedQty = 0;
                            }
                        }
                        else if (this.IsAgainstIndentReceiveStock == false)
                        {
                            if (objCD.AvailableStock > (objCD.TotalPatientIndentReceiveQty - objCD.TotalPatientIndentConsumptionQty))
                            {
                                if (objCD.BaseOty > (objCD.AvailableStock - (objCD.TotalPatientIndentReceiveQty - objCD.TotalPatientIndentConsumptionQty)))
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgWD =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Used Qty should not be Greater than Patient Reserved Indent Qty! Reserved Qty is " + (objCD.TotalPatientIndentReceiveQty - objCD.TotalPatientIndentConsumptionQty), MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgWD.Show();
                                    objCD.UsedQty = 0;
                                }
                            }
                        }
                    }

                    SumOfTotals();
                    CalculatePharmacySummary(); //Added by AJ Date 30/1/2017
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void SumOfTotals()
        {
            decimal TotalAmount = 0;
            decimal TotalQuantity = 0;
            double TotalMRPAmt = 0;
            foreach (var item in MaterialConsumptionAddedItems)
            {
                TotalAmount = TotalAmount + item.Amount;
                TotalQuantity = TotalQuantity + item.UsedQty;
                TotalMRPAmt = TotalMRPAmt + item.MRP * Convert.ToDouble(item.UsedQty);

            }
            txtTotalAmount.Text = String.Format("{0:0.00}", TotalAmount);
            txtNoOfItems.Text = String.Format("{0:0.00}", TotalQuantity);
            TotalMRPAmount = TotalMRPAmt;
        }

        private void cmdDeleteItemList_Click(object sender, RoutedEventArgs e)
        {
            if (dgItemList.SelectedItem != null)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Delete the selected Item ?";
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        if (Opd_Ipd_External == 1 || (Opd_Ipd_External == 0 && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0))
                        {
                            PharmacyItems.RemoveAt(dgItemList.SelectedIndex);
                            CalculatePharmacySummary();
                        }
                        MaterialConsumptionAddedItems.RemoveAt(dgItemList.SelectedIndex);
                        SumOfTotals();

                        if (PharmacyItems.Count == 0)
                        {
                            cmbApplicabelPackage.IsEnabled = true;
                        }
                    }
                };

                msgWD.Show();
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FillMatarialConsumptionList();
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("New");
            objAnimation.Invoke(RotationType.Backward);
            //Added by AJ Date 6/1/2017// 
            IsAgainstPatient = false;
            chkAgainstPatient.IsChecked = false;
            btnSearchCriteria.IsEnabled = false;
            chkAgainstPatient.IsEnabled = true;
            PatientID = 0;
            PatientUnitID = 0;
            Opd_Ipd_External_Id = 0;
            Opd_Ipd_External_UnitId = 0;
            Opd_Ipd_External = 0;
            txtPatientName.Text = "";
            txtMRNo.Text = "";
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgMaterailConsumptionList.SelectedItem != null)
            {
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/InventoryPharmacy/MaterialConsumptionReport.aspx?ConsumptionId=" + ((clsMaterialConsumptionVO)dgMaterailConsumptionList.SelectedItem).ID + "&UnitID=" + (((clsMaterialConsumptionVO)dgMaterailConsumptionList.SelectedItem).UnitID)), "_blank");
            }
        }
        public long StoreId { get; set; }
        private void cmbStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbStore.SelectedItem != null)
                {
                    if (MaterialConsumptionAddedItems != null)
                        MaterialConsumptionAddedItems.Clear();
                    StoreId = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
                }
            }
            catch (Exception Ex)
            {
                throw;
            }
            txtBarCode.Focus();
        }

        private void txtBarCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (StoreId != 0)
            {
                if (e.Key == Key.Enter)
                {
                    //fillItemGridOnBar();
                    FillItemGrid();
                    SumOfTotals();
                    //  txtBarCode.Focus();
                    // txtBarCode.Text = "";
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = null;
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select store", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                mgbx.Show();
                txtBarCode.Focus();
                txtBarCode.Text = "";
            }
        }
        // By Anjali.............
        public void FillItemGrid()
        {
            clsCounterSaleBarCodeBizActionVO BizActionObj = new clsCounterSaleBarCodeBizActionVO();
            BizActionObj.IssueList = new List<clsItemSalesDetailsVO>();
            WaitIndicator w = new WaitIndicator();
            w.Show();
            try
            {
                BizActionObj.BarCode = txtBarCode.Text;
                BizActionObj.StoreId = StoreId;
                BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {

                        BizActionObj = (clsCounterSaleBarCodeBizActionVO)e.Result;
                        if (((clsCounterSaleBarCodeBizActionVO)e.Result).IssueList != null)
                            if (((clsCounterSaleBarCodeBizActionVO)e.Result).IssueList.Count > 0)
                            {
                                BizActionObj.IssueList = ((clsCounterSaleBarCodeBizActionVO)e.Result).IssueList;
                                foreach (clsItemSalesDetailsVO item in BizActionObj.IssueList)
                                {

                                    if (item.AvailableQuantity == 0)
                                    {
                                        MessageBoxControl.MessageBoxChildWindow mgbx = null;
                                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Item is out of stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        mgbx.Show();
                                        mgbx.OnMessageBoxClosed += (MessageBoxResult res) =>
                                        {
                                            if (res == MessageBoxResult.OK)
                                            {
                                                txtBarCode.Text = string.Empty;
                                                txtBarCode.Focus();
                                            }
                                        };
                                    }
                                    else
                                    {
                                        String ItemCode = String.Empty, ItemName = String.Empty;
                                        ItemCode = item.ItemCode;
                                        ItemName = item.ItemName;
                                        clsMaterialConsumptionItemDetailsVO ConsumptedItem = new clsMaterialConsumptionItemDetailsVO()
                                        {
                                            AvailableStock = (item.AvailableQuantity),
                                            BatchCode = item.BatchCode,
                                            BatchId = item.BatchID,
                                            ExpiryDate = item.ExpiryDate,
                                            ItemId = item.ItemID,
                                            Rate = Convert.ToDecimal(item.PurchaseRate),
                                            ItemName = ItemName,
                                            UsedQty = 1,
                                            Amount = Convert.ToDecimal(item.PurchaseRate),
                                            BaseUOMID = item.BaseUOMID,
                                            SUOMID = item.SUOMID,
                                            SUOM = item.SUOM,
                                            SelectedUOM = new MasterListItem { ID = item.SUOMID, Description = item.SUOM }
                                        };
                                        if (MaterialConsumptionAddedItems.Where(ConsumptedItems => ConsumptedItems.ItemId == item.ItemID).Any() == true)
                                        {
                                            if (MaterialConsumptionAddedItems.Where(ConsumptedItems => ConsumptedItems.BatchId == item.BatchID).Any() == false)
                                                this.MaterialConsumptionAddedItems.Add(ConsumptedItem);
                                            else
                                            {
                                                foreach (var item1 in MaterialConsumptionAddedItems.Where(x => x.ItemId == ConsumptedItem.ItemId & x.BatchId == ConsumptedItem.BatchId))
                                                {
                                                    if (item1.AvailableStock > Convert.ToDouble(item1.UsedQty))
                                                    {
                                                        item1.UsedQty = item1.UsedQty + 1;
                                                    }
                                                    else
                                                    {
                                                        MessageBoxControl.MessageBoxChildWindow mgbx = null;
                                                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Item is out of stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                                        mgbx.Show();
                                                    }
                                                }
                                            }

                                        }
                                        else
                                            this.MaterialConsumptionAddedItems.Add(ConsumptedItem);
                                    }
                                    txtBarCode.Focus();
                                    txtBarCode.Text = "";

                                }
                                dgItemList.ItemsSource = this.MaterialConsumptionAddedItems;
                                SumOfTotals();
                                CalculatePharmacySummary(); //Added by AJ Date 30/1/2017
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow mgbx = null;
                                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Item is not in stock", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                mgbx.OnMessageBoxClosed += (MessageBoxResult res) =>
                                {
                                    if (res == MessageBoxResult.OK)
                                        txtBarCode.Focus();
                                    txtBarCode.Text = "";
                                };
                                mgbx.Show();

                            }

                    }


                };
                client.ProcessAsync(BizActionObj, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            w.Close();
        }
        // ............................
        public void fillItemGridOnBar()
        {
            clsCounterSaleBarCodeBizActionVO BizActionObj = new clsCounterSaleBarCodeBizActionVO();
            BizActionObj.IssueList = new List<clsItemSalesDetailsVO>();
            WaitIndicator w = new WaitIndicator();
            w.Show();
            try
            {
                string[] str = txtBarCode.Text.Split('-');
                if (str.Length > 1)
                {
                    BizActionObj.ItemID = Convert.ToInt64(str[0]);
                    string inputString = null;
                    string[] str1 = null;
                    bool blnFlag = false;
                    if (str[1].Contains("/"))
                    {
                        str1 = str[1].Split('/');
                        inputString = str1[1];
                    }
                    else
                    {
                        str1 = str[1].Split();
                        inputString = str1[0];
                    }
                    string BatchID = null;
                    string lastCharacter = inputString.Substring(inputString.Length - 1);
                    string lastCharacter1 = inputString.Substring(inputString.Length - 2);
                    if (lastCharacter == "B")
                    {
                        BatchID = inputString.Substring(0, inputString.Length - 1);
                        BizActionObj.BatchCode = str1[0];
                        BizActionObj.BatchID = Convert.ToInt64(BatchID);
                        foreach (var item in MaterialConsumptionAddedItems.Where(x => x.ItemId == Convert.ToInt64(str[0]) & x.BatchId == Convert.ToInt64(BatchID)))
                        {
                            if (item.AvailableStock > Convert.ToDouble(item.UsedQty))
                            {
                                item.UsedQty = item.UsedQty + 1;
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow mgbx = null;
                                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Item is out of stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                mgbx.Show();
                            }
                            blnFlag = true;
                        }
                    }
                    else if (lastCharacter == "I")
                    {
                        if (lastCharacter1 == "BI")
                        {
                            str[1] = inputString.Substring(0, inputString.Length - 2);
                            BizActionObj.BatchID = Convert.ToInt64(str[1]);

                            foreach (var item in MaterialConsumptionAddedItems.Where(x => x.ItemId == Convert.ToInt64(str[0]) & x.BatchId == Convert.ToInt64(str[1])))
                            {
                                if (item.AvailableStock > Convert.ToDouble(item.UsedQty))
                                {
                                    item.UsedQty = item.UsedQty + 1;
                                }
                                else
                                {
                                    MessageBoxControl.MessageBoxChildWindow mgbx = null;
                                    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Item is out of stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    mgbx.Show();
                                }
                                blnFlag = true;
                            }
                        }
                        else
                        {
                            str[1] = inputString.Substring(0, inputString.Length - 1);
                            BizActionObj.ItemCode = str[1];
                            foreach (var item in MaterialConsumptionAddedItems.Where(x => x.ItemId == Convert.ToInt64(str[0])))
                            {
                                if (item.AvailableStock > Convert.ToDouble(item.UsedQty))
                                {
                                    item.UsedQty = item.UsedQty + 1;
                                }
                                else
                                {
                                    MessageBoxControl.MessageBoxChildWindow mgbx = null;
                                    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Item is out of stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    mgbx.Show();
                                }
                                blnFlag = true;
                            }
                        }
                    }
                    dgItemList.ItemsSource = null;
                    dgItemList.ItemsSource = this.MaterialConsumptionAddedItems;
                    dgItemList.UpdateLayout();

                    if (blnFlag == false)
                    {
                        BizActionObj.StoreId = StoreId;
                        BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, e) =>
                        {
                            if (e.Error == null && e.Result != null)
                            {
                                BizActionObj = (clsCounterSaleBarCodeBizActionVO)e.Result;
                                BizActionObj.IssueList = ((clsCounterSaleBarCodeBizActionVO)e.Result).IssueList;
                                foreach (clsItemSalesDetailsVO item in BizActionObj.IssueList)
                                {
                                    String ItemCode = String.Empty, ItemName = String.Empty;
                                    ItemCode = item.ItemCode;
                                    ItemName = item.ItemName;
                                    clsMaterialConsumptionItemDetailsVO ConsumptedItem = new clsMaterialConsumptionItemDetailsVO()
                                    {
                                        AvailableStock = (item.AvailableQuantity),
                                        BatchCode = item.BatchCode,
                                        BatchId = item.BatchID,
                                        ExpiryDate = item.ExpiryDate,
                                        ItemId = item.ItemID,
                                        Rate = Convert.ToDecimal(item.PurchaseRate),
                                        ItemName = ItemName,
                                        UsedQty = 1,
                                        Amount = Convert.ToDecimal(item.PurchaseRate)
                                    };
                                    if (MaterialConsumptionAddedItems.Where(ConsumptedItems => ConsumptedItems.ItemId == item.ItemID).Any() == true)
                                    {
                                        if (MaterialConsumptionAddedItems.Where(ConsumptedItems => ConsumptedItems.BatchId == item.BatchID).Any() == false)
                                            this.MaterialConsumptionAddedItems.Add(ConsumptedItem);
                                    }
                                    else
                                        this.MaterialConsumptionAddedItems.Add(ConsumptedItem);
                                }
                                dgItemList.ItemsSource = this.MaterialConsumptionAddedItems;
                            }
                        };
                        client.ProcessAsync(BizActionObj, new clsUserVO());
                        client.CloseAsync();
                    }
                }
                //By Anjali.......................
                else
                {
                    BizActionObj.BarCode = txtBarCode.Text;
                    BizActionObj.StoreId = StoreId;
                    BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;


                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, e) =>
                    {
                        if (e.Error == null && e.Result != null)
                        {
                            BizActionObj = (clsCounterSaleBarCodeBizActionVO)e.Result;
                            BizActionObj.IssueList = ((clsCounterSaleBarCodeBizActionVO)e.Result).IssueList;
                            foreach (clsItemSalesDetailsVO item in BizActionObj.IssueList)
                            {
                                String ItemCode = String.Empty, ItemName = String.Empty;
                                ItemCode = item.ItemCode;
                                ItemName = item.ItemName;
                                clsMaterialConsumptionItemDetailsVO ConsumptedItem = new clsMaterialConsumptionItemDetailsVO()
                                {
                                    AvailableStock = (item.AvailableQuantity),
                                    BatchCode = item.BatchCode,
                                    BatchId = item.BatchID,
                                    ExpiryDate = item.ExpiryDate,
                                    ItemId = item.ItemID,
                                    Rate = Convert.ToDecimal(item.PurchaseRate),
                                    ItemName = ItemName,
                                    UsedQty = 1,
                                    Amount = Convert.ToDecimal(item.PurchaseRate)
                                };
                                if (MaterialConsumptionAddedItems.Where(ConsumptedItems => ConsumptedItems.ItemId == item.ItemID).Any() == true)
                                {
                                    if (MaterialConsumptionAddedItems.Where(ConsumptedItems => ConsumptedItems.BatchId == item.BatchID).Any() == false)
                                        this.MaterialConsumptionAddedItems.Add(ConsumptedItem);
                                }
                                else
                                    this.MaterialConsumptionAddedItems.Add(ConsumptedItem);
                            }
                            dgItemList.ItemsSource = this.MaterialConsumptionAddedItems;
                        }
                    };
                    client.ProcessAsync(BizActionObj, new clsUserVO());
                    client.CloseAsync();
                }
                //..................................................................................................................................


            }
            catch (Exception)
            {
                throw;
            }
            w.Close();
        }

        private FrameworkElement GetParent(FrameworkElement child, Type targetType)
        {
            object parent = null;
            if (child != null)
            {
                parent = child.Parent;
            }
            if (parent != null)
            {
                if (parent.GetType() == targetType)
                {
                    return (FrameworkElement)parent;
                }
                else
                {
                    return GetParent((FrameworkElement)parent, targetType);
                }
            }
            return null;

        }

        //By Umesh

        private void cmbPUOM_GotFocus(object sender, RoutedEventArgs e)
        {
            AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;

            if (cmbConversions.ItemsSource == null || (cmbConversions.ItemsSource != null && ((List<MasterListItem>)cmbConversions.ItemsSource).ToList().Count == 0) || (((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UOMConversionList == null || ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UOMConversionList.Count == 0))
            {
                FillUOMConversions(cmbConversions);
            }
        }

        private void cmbPUOM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgItemList.SelectedItem != null)
            {
                clsConversionsVO objConversion = new clsConversionsVO();

                AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;
                long SelectedUomId = 0;

                //  ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UsedQty = 0;

                if (cmbConversions.SelectedItem != null && ((MasterListItem)cmbConversions.SelectedItem).ID > 0)
                {
                    ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).SelectedUOM = ((MasterListItem)cmbConversions.SelectedItem);

                    CalculateConversionFactorCentral(((MasterListItem)cmbConversions.SelectedItem).ID, ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).SUOMID);
                }
                else
                {
                    ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).ConversionFactor = 0;
                    ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).BaseConversionFactor = 0;

                    //    ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).MRP = ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).MainMRP;
                    ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).Rate = (decimal)((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).Rate;
                }

                // CalculateOpeningBalanceSummary(); 
                SumOfTotals();
            }
        }

        // Method To Fill Unit Of Mesurements with Conversion Factors for Selected Item
        private void FillUOMConversions(AutoCompleteBox cmbConversions)
        {
            try
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();

                clsGetItemConversionFactorListBizActionVO BizAction = new clsGetItemConversionFactorListBizActionVO();

                BizAction.ItemID = ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).ItemId;
                BizAction.UOMConversionList = new List<clsConversionsVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        Indicatior.Close();

                        List<MasterListItem> UOMConvertLIst = new List<MasterListItem>();
                        MasterListItem objConversion = new MasterListItem();
                        objConversion.ID = 0;
                        objConversion.Description = "- Select -";
                        UOMConvertLIst.Add(objConversion);

                        if (((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList != null)
                            UOMConvertLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList);
                        cmbConversions.ItemsSource = UOMConvertLIst.DeepCopy();

                        if (UOMConvertLIst != null)
                            cmbConversions.SelectedItem = UOMConvertLIst[0];
                        List<clsConversionsVO> UOMConversionLIst = new List<clsConversionsVO>();
                        UOMConversionLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConversionList);

                        if (dgItemList.SelectedItem != null)
                        {
                            ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UOMConversionList = UOMConversionLIst.DeepCopy();
                            ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UOMList = UOMConvertLIst.DeepCopy();
                        }
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                Indicatior.Close();
                throw;
            }
        }

        private void CalculateConversionFactorCentral(long FromUOMID, long ToUOMID)
        {
            clsConversions objConversionVO = new clsConversions();

            try
            {
                List<clsConversionsVO> UOMConvertLIst = new List<clsConversionsVO>();

                if (dgItemList.SelectedItem != null)
                {
                    UOMConvertLIst = ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UOMConversionList;



                    objConversionVO.UOMConvertLIst = UOMConvertLIst;

                    objConversionVO.MainMRP = ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).MainMRP;
                    objConversionVO.MainRate = (float)((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).MainRate;
                    objConversionVO.SingleQuantity = (float)((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UsedQty;

                    long BaseUOMID = ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).BaseUOMID;

                    objConversionVO.CalculateConversionFactor(FromUOMID, ToUOMID, BaseUOMID);

                    //   ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).Rate =(long) objConversionVO.Rate;
                    //   ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).MRP = objConversionVO.MRP;


                    //    ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).Quantity = objConversionVO.Quantity;
                    //    ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).BaseQuantity = objConversionVO.BaseQuantity;

                    //      ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).BaseRate = objConversionVO.BaseRate;
                    //      ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).BaseMRP = objConversionVO.BaseMRP;

                    ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).ConversionFactor = objConversionVO.ConversionFactor;
                    ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).BaseConversionFactor = objConversionVO.BaseConversionFactor;

                    ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).StockOty = (decimal)objConversionVO.BaseQuantity;
                    ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).BaseOty = (double)objConversionVO.BaseQuantity;
                    ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).Rate = Convert.ToDecimal(objConversionVO.Rate);
                    ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).Amount = (Convert.ToDecimal(objConversionVO.Rate) * (decimal)((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UsedQty);
                    ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).MRP = Convert.ToSingle(objConversionVO.MRP);
                }

            }
            catch (Exception ex)
            {
                objConversionVO = null;
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgItemList.SelectedItem != null)
            {
                Conversion win = new Conversion();

                win.FillUOMConversions(((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).ItemId, ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).SelectedUOM.ID);
                win.OnSaveButton_Click += new RoutedEventHandler(win_OnSaveButton_Click);
                win.OnCancelButton_Click += new RoutedEventHandler(win_OnCancelButton_Click);
                win.Show();
            }

        }

        void win_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {

        }



        void win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            Conversion Itemswin = (Conversion)sender;

            ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).SelectedUOM = (MasterListItem)Itemswin.cmbConversion.SelectedItem;
            ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UOMList = Itemswin.UOMConvertLIst;
            ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UOMConversionList = Itemswin.UOMConversionLIst;

            //Added by AJ Date 4/2/2017----
            if (Opd_Ipd_External == 1 || Opd_Ipd_External == 0)
            {
                PharmacyItems[dgItemList.SelectedIndex].SelectedUOM = (MasterListItem)Itemswin.cmbConversion.SelectedItem;
                PharmacyItems[dgItemList.SelectedIndex].UOMList = Itemswin.UOMConvertLIst;
                PharmacyItems[dgItemList.SelectedIndex].UOMConversionList = Itemswin.UOMConversionLIst;
            }

            //***//---------------------------

            CalculateConversionFactorCentral(((MasterListItem)Itemswin.cmbConversion.SelectedItem).ID, ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).SUOMID);
            SumOfTotals();
            if (Opd_Ipd_External == 1 || Opd_Ipd_External == 0)  //Added by AJ Date 30/1/2017
            {
                CalculateConversionFactorCentral2(((MasterListItem)Itemswin.cmbConversion.SelectedItem).ID, PharmacyItems[dgItemList.SelectedIndex].SUOMID); //Added by AJ Date 30/1/2017                         
                CalculatePharmacySummary(); //Added by AJ Date 30/1/2017
            }


            if (((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UsedQty == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW3 =
                                  new MessageBoxControl.MessageBoxChildWindow("", "Please Enter Used Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW3.Show();
            }

            else if (((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).SelectedUOM != null && ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).SelectedUOM.ID == ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).BaseUOMID && (((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UsedQty % 1) != 0)
            {
                ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UsedQty = 0;
                string msgText = "Quantity Cannot be in fraction";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWD.Show();

            }

            if (((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).SelectedUOM != null && ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).SelectedUOM.ID > 0)
            {
                if ((((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UsedQty * Convert.ToDecimal(((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).ConversionFactor) * Convert.ToDecimal(((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).StockToBase))
                    > (Math.Round((decimal)((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).AvailableStock, 3) * (decimal)((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).StockToBase))
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                      new MessageBoxControl.MessageBoxChildWindow("", "Please Enter Used Quantity Less Than Or Equal To Available Stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW3.Show();
                    //((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UsedQty = (decimal)((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).AvailableStock / (decimal)((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).BaseConversionFactor;
                    ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UsedQty = (decimal)Convert.ToSingle(Math.Floor(Convert.ToDouble(((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).AvailableStock / ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).BaseConversionFactor)));
                }

            }
            else
            {
                CalculateConversionFactorCentral(((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).SelectedUOM.ID, ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).SUOMID);

                if (((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UsedQty > (decimal)((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).AvailableStock)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                      new MessageBoxControl.MessageBoxChildWindow("", "Please Enter Used Quantity Less Than Or Equal To Available Stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW3.Show();

                }
            }
            if (this.IsAgainstIndentReceiveStock)
            {
                clsMaterialConsumptionItemDetailsVO objCD = new clsMaterialConsumptionItemDetailsVO();
                objCD = (clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem;
                if (objCD.BaseOty > (objCD.TotalPatientIndentReceiveQty - objCD.TotalPatientIndentConsumptionQty))
                {
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Used Qty should not be Greater than Indent Received Qty! Remaining Qty is " + (objCD.TotalPatientIndentReceiveQty - objCD.TotalPatientIndentConsumptionQty), MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD.Show();
                    objCD.UsedQty = 0;
                }
            }
        }

        // END

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
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

            //if (!((TextBox)sender).Text.IsValidDigintWithTwoDecimalPlaces() && textBefore != null)
            //{

            //    ((TextBox)sender).Text = textBefore;
            //    ((TextBox)sender).SelectionStart = selectionStart;
            //    ((TextBox)sender).SelectionLength = selectionLength;
            //    textBefore = "";
            //    selectionStart = 0;
            //    selectionLength = 0;
            //}
        }

        private void txtNumber_KeyDown1(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void btnSearchCriteria_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch_Child Win = new PatientSearch_Child();
            Win.isfromMaterialConsumpation = true;           //Added by AJ Date 29/12/2016
            Win.StoreID = cmbStore.SelectedItem == null ? 0 : ((clsStoreVO)cmbStore.SelectedItem).StoreId;
            Win.OnSaveButton_Click += new RoutedEventHandler(PatientSearch_OnSaveButton_Click);
            Win.Show();
        }

        void PatientSearch_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.IsAgainstIndentReceiveStock = false;
            WaitIndicator Indicatior = new WaitIndicator();           
            try
            {
                if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
                {
                      AgainstDonor = false;
                      LinkPatientID = 0;
                      LinkPatientUnitID = 0;
                      LinkCompanyID = 0;
                      LinkPatientSourceID = 0;
                      LinkTariffID = 0;
                      if ((((IApplicationConfiguration)App.Current).SelectedPatient.RelationID == 15) && (((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 8 || ((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 9))
                      {
                          DonorCoupleLinkedList Win = new DonorCoupleLinkedList();
                          Win.DonorID = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                          Win.DonorUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
                          Win.DonorName = ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName + " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " + ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;

                          Win.OnSaveButton_Click += new RoutedEventHandler(DonorLinWin_OnSaveButton_Click);
                          Win.OnCancelButton_Click += new RoutedEventHandler(DonorLinWin_OnCancelButton_Click);
                          Win.Show();
                      }
                    else
                      {
                          Indicatior.Show();

                    //  MRNO = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                    //PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                    PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID; //Added by AJ Date 2/1/2017
                    Opd_Ipd_External_Id = ((IApplicationConfiguration)App.Current).SelectedPatient.Opd_Ipd_External_Id;
                    Opd_Ipd_External_UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.Opd_Ipd_External_UnitId;
                    Opd_Ipd_External = ((IApplicationConfiguration)App.Current).SelectedPatient.Opd_Ipd_External;
                    PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    TariffID = ((IApplicationConfiguration)App.Current).SelectedPatient.TariffID;
                    CompanyID = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
                    PatientCategoryID = ((IApplicationConfiguration)App.Current).SelectedPatient.NewPatientCategoryID;
                    PatientSourceID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;
                    DoctorID = ((IApplicationConfiguration)App.Current).SelectedPatient.ReferralDoctorID;
                    txtPatientName.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName + " " + ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;
                    txtMRNo.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                    if (Opd_Ipd_External == 1 || Opd_Ipd_External == 0) //Added by AJ Date 23/1/2017
                    {
                        FillPackage(PatientID, PatientUnitID);
                        FillBillSearchList();
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.IsPatientIndentReceiveExists == 1)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW =
                               new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Patient Indent Stock Exists! Are you sure you want to add Items from Patient Indent Stock ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                        msgW.OnMessageBoxClosed += (res) =>
                        {
                            if (res == MessageBoxResult.Yes)
                            {
                                this.IsAgainstIndentReceiveStock = true;

                                //ItemListNew Itemswin = new ItemListNew();
                                //Itemswin.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
                                //Itemswin.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                                //Itemswin.ShowExpiredBatches = false;
                                //if (cmbStore.SelectedItem == null)
                                //{
                                //    msgText = "Please Select Store";
                                //    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                //    msgWindow.Show();
                                //    cmbStore.Focus();

                                //}
                                //else if (((clsStoreVO)cmbStore.SelectedItem).StoreId == 0)
                                //{
                                //    msgText = "Please Select Store";
                                //    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                //    msgWindow.Show();
                                //    cmbStore.Focus();
                                //}
                                //else
                                //{
                                //    this.IsAgainstIndentReceiveStock = true;
                                //    Itemswin.ShowBatches = true;
                                //    Itemswin.StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
                                //    if ((MasterListItem)cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)
                                //    {
                                //        if (((MasterListItem)cmbApplicabelPackage.SelectedItem).ApplicableToAll == true) //&& ((MasterListItem)cmbApplicabelPackage.SelectedItem).ApplicableToAllDiscount > 0 Commented By Bhushanp For New Package Changes 04092017
                                //        {
                                //            Itemswin.PackageID = 0;
                                //        }
                                //        else
                                //        {
                                //            Itemswin.PackageID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID;
                                //        }
                                //    }
                                //    Itemswin.cmbStore.IsEnabled = false;
                                //    Itemswin.PatientID = PatientID;
                                //    Itemswin.PatientUnitID = PatientUnitID;
                                //    Itemswin.OnSaveButton_Click += new RoutedEventHandler(Itemswin_OnSaveButton_Click);
                                //    Itemswin.Show();
                                //}
                            }
                        };
                        msgW.Show();
                    }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Indicatior.Close();
        }


        public void DonorLinWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();
            if (((DonorCoupleLinkedList)sender).DialogResult == true && ((DonorCoupleLinkedList)sender).DonorLink.IsAgainstDonor == true)
            {

                AgainstDonor = ((DonorCoupleLinkedList)sender).DonorLink.IsAgainstDonor;
                LinkPatientID = ((DonorCoupleLinkedList)sender).DonorLink.PatientID;
                LinkPatientUnitID = ((DonorCoupleLinkedList)sender).DonorLink.PatientUnitID;
                LinkCompanyID = ((DonorCoupleLinkedList)sender).DonorLink.CompanyID;
                LinkPatientSourceID = ((DonorCoupleLinkedList)sender).DonorLink.PatientSourceID;
                LinkTariffID = ((DonorCoupleLinkedList)sender).DonorLink.TariffID;
            }

            if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
            {
                PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID; //Added by AJ Date 2/1/2017
                Opd_Ipd_External_Id = ((IApplicationConfiguration)App.Current).SelectedPatient.Opd_Ipd_External_Id;
                Opd_Ipd_External_UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.Opd_Ipd_External_UnitId;
                Opd_Ipd_External = ((IApplicationConfiguration)App.Current).SelectedPatient.Opd_Ipd_External;
                PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                TariffID = ((IApplicationConfiguration)App.Current).SelectedPatient.TariffID;
                CompanyID = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
                PatientCategoryID = ((IApplicationConfiguration)App.Current).SelectedPatient.NewPatientCategoryID;
                PatientSourceID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;
                DoctorID = ((IApplicationConfiguration)App.Current).SelectedPatient.ReferralDoctorID;
                txtPatientName.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName + " " + ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;
                txtMRNo.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                if (Opd_Ipd_External == 1 || Opd_Ipd_External == 0) //Added by AJ Date 23/1/2017
                {
                    if (AgainstDonor == true)
                    {
                        FillPackage(LinkPatientID, LinkPatientUnitID);
                    }
                    else
                    {
                        FillPackage(PatientID, PatientUnitID);
                    }

                    FillBillSearchList();
                }

                if (((IApplicationConfiguration)App.Current).SelectedPatient.IsPatientIndentReceiveExists == 1)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                           new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Patient Indent Stock Exists! Are you sure you want to add Items from Patient Indent Stock ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgW.OnMessageBoxClosed += (res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            this.IsAgainstIndentReceiveStock = true;
                        }
                    };
                    msgW.Show();
                }

            }
            Indicatior.Close();
        }

        void DonorLinWin_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            //if (SelectedBill != null)
            //    //cmdSave.IsEnabled = false;
            //else
            //cmdSave.IsEnabled = true;
        }



        private void FillPackage(long PatientID1, long UnitId1)
        {
            clsGetPatientPackageInfoListBizActionVO BizAction = new clsGetPatientPackageInfoListBizActionVO();
            if (PatientID1 > 0 && UnitId1 > 0)
            {
                BizAction.PatientID1 = PatientID1;
                BizAction.PatientUnitID1 = UnitId1;
            }
            else if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
            {
                BizAction.PatientID1 = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction.PatientUnitID1 = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
            }
            BizAction.CheckDate = DateTime.Now.Date.Date;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                List<MasterListItem> objList = new List<MasterListItem>();
                if (arg.Error == null && arg.Result != null)
                {
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetPatientPackageInfoListBizActionVO)arg.Result).MasterList);

                    cmbApplicabelPackage.ItemsSource = null;
                    cmbApplicabelPackage.ItemsSource = objList;

                    if (objList.Count > 1)
                    {
                        cmbApplicabelPackage.Visibility = Visibility.Visible;

                        //var list1 = from ls in objList
                        //            orderby ls.ID descending
                        //            select ls.ID;

                        var list1 = from ls in objList
                                    orderby ls.FilterID descending
                                    select ls.ID;

                        cmbApplicabelPackage.SelectedValue = list1.ToList()[0];


                    }
                    else
                    {
                        cmbApplicabelPackage.ItemsSource = null;
                        cmbApplicabelPackage.SelectedItem = objList[0];
                    }
                }
                else
                {
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    cmbApplicabelPackage.SelectedItem = objList[0];
                    cmbApplicabelPackage.IsEnabled = false;

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void chkAgainstPatient_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (cmbStore.SelectedItem != null && ((clsStoreVO)cmbStore.SelectedItem).StoreId == 0)
            {
                chk.IsChecked = false;

                msgText = "Please Select Store";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
                cmbStore.Focus();
            }
            else
            {
                if (chk.IsChecked == true)
                {
                    btnSearchCriteria.IsEnabled = true;
                    IsAgainstPatient = true;
                }
                else
                {
                    btnSearchCriteria.IsEnabled = false;
                    PatientUnitID = 0;
                    PatientID = 0;
                    IsAgainstPatient = false;
                    txtMRNo.Text = "";
                    txtPatientName.Text = "";

                    if (MaterialConsumptionAddedItems != null && MaterialConsumptionAddedItems.Count > 0)
                    {
                        MaterialConsumptionAddedItems.Clear();
                        MaterialConsumptionAddedItems = new ObservableCollection<clsMaterialConsumptionItemDetailsVO>();
                        dgItemList.ItemsSource = null;
                        dgItemList.ItemsSource = MaterialConsumptionAddedItems;
                        this.IsAgainstIndentReceiveStock = false;
                    }
                }
            }

            //else
            //{
            //    chk.IsChecked = true;
            //    MessageBoxControl.MessageBoxChildWindow msgW =
            //    new MessageBoxControl.MessageBoxChildWindow("", "Atleast One Store Should Be Selected", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    msgW.Show();
            //}
        }

        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Subtract || e.Key == Key.Decimal && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = CIMS.Comman.HandleDecimal(sender, e);
            }
        }

        //Added by AJ Date 5/1/2017

        private void cmdCloseM_Click(object sender, RoutedEventArgs e)
        {
            ModuleName = "PalashDynamics.IPD";
            Action = "PalashDynamics.IPD.PatientAgainstMaterialConsumptionDetails";
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            WebClient c = new WebClient();
            c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_AdmissionListOpenReadCompleted);
            c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
        }

        void c_AdmissionListOpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
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


        double PackageConcenssion = 0;
        private void SaveDetails(clsPaymentVO pPayDetails, bool pFreezBill, long pPatientID, long pPatientUnitID)
        {
            try
            {
                pPayDetails.BillAmount = Convert.ToDouble(PharmacyNetAmount);
                if (SelectedBill == null)
                {
                    pPayDetails.BillBalanceAmount = Convert.ToDouble(RountOffAmount);
                }
                else
                {
                    pPayDetails.BillBalanceAmount = SelectedBill.NetBillAmount + Convert.ToDouble(RountOffAmount);
                }

                pPayDetails.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstBill;


                clsBillVO objBill = new clsBillVO();
                if (SelectedBill == null)
                {
                    objBill.Date = DateTime.Now;
                    objBill.Time = DateTime.Now;
                }
                else
                    objBill = SelectedBill.DeepCopy();

                objBill.IsFreezed = false;
                objBill.PatientID = PatientID;
                objBill.Opd_Ipd_External_Id = Opd_Ipd_External_Id;
                objBill.Opd_Ipd_External_UnitId = Opd_Ipd_External_UnitId;
                objBill.Opd_Ipd_External = 1;
                objBill.DoctorID = DoctorID;
                objBill.TariffId = TariffID;
                objBill.CompanyId = CompanyID;
                objBill.PatientSourceId = PatientSourceID;
                if (pFreezBill)
                {
                    objBill.Date = DateTime.Now;
                    objBill.Time = DateTime.Now;

                    if (pPayDetails != null)
                    {
                        objBill.BalanceAmountSelf = pPayDetails.BillBalanceAmount;
                        objBill.BillPaymentType = pPayDetails.BillPaymentType;
                    }
                    objBill.SelfAmount = Convert.ToDouble(PayAmount);
                }

                if (SelectedBill == null)
                {
                    objBill.TotalBillAmount = Convert.ToDouble(TotalBill);
                    objBill.TotalConcessionAmount = Convert.ToDouble(TotalConcession);
                    objBill.CalculatedNetBillAmount = Convert.ToDouble(NetAmount1);
                    objBill.NetBillAmount = Convert.ToDouble(RountOffAmount);
                }
                else
                {

                    objBill.TotalBillAmount = Convert.ToDouble(TotalBill) + SelectedBill.TotalBillAmount;
                    objBill.TotalConcessionAmount = Convert.ToDouble(TotalConcession) + SelectedBill.TotalConcessionAmount;
                    objBill.CalculatedNetBillAmount = Convert.ToDouble(NetAmount1) + SelectedBill.NetBillAmount;
                    objBill.NetBillAmount = Convert.ToDouble(RountOffAmount) + SelectedBill.NetBillAmount;
                }
                if (PatientSourceID > 0)
                {
                    objBill.PatientSourceId = PatientSourceID;
                }
                else
                {
                    if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
                        objBill.PatientSourceId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;

                }

                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                    objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;  //Costing Divisions for Pharmacy Billing
                else
                    objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyCounterID;

                if (objBill.PaymentDetails != null)
                {
                    objBill.PaymentDetails.CostingDivisionID = objBill.CostingDivisionID;   //Costing Divisions for Pharmacy Billing
                }

                if (PharmacyItems.Count > 0)
                {
                    objBill.PharmacyItems.Items = new List<clsItemSalesDetailsVO>();
                    objBill.PharmacyItems.ISForMaterialConsumption = true;
                    objBill.PharmacyItems.VisitID = Opd_Ipd_External_Id;
                    objBill.PharmacyItems.PatientID = PatientID;
                    objBill.PharmacyItems.PatientUnitID = PatientUnitID;
                    objBill.PharmacyItems.Date = objBill.Date;
                    objBill.PharmacyItems.Time = objBill.Time;
                    objBill.PharmacyItems.StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
                    objBill.PharmacyItems.CostingDivisionID = objBill.CostingDivisionID;
                    objBill.PharmacyItems.VATAmount = vatamt;
                    objBill.PharmacyItems.VATPercentage = vatper;
                    objBill.PharmacyItems.TotalAmount = Convert.ToDouble(PharmacyTotal);
                    objBill.PharmacyItems.ConcessionAmount = Convert.ToDouble(PharmacyConcession);
                    objBill.PharmacyItems.NetAmount = Convert.ToDouble(RountOffAmount);
                    objBill.PharmacyItems.ReferenceDoctorID = DoctorID;
                    objBill.PharmacyItems.ReferenceDoctor = DoctorName;
                    objBill.BillType = PalashDynamics.ValueObjects.BillTypes.Pharmacy;
                    objBill.PharmacyItems.Opd_Ipd_External_Id = Opd_Ipd_External_Id;
                    objBill.PharmacyItems.Opd_Ipd_External_UnitId = Opd_Ipd_External_UnitId;
                    objBill.PharmacyItems.Items = PharmacyItems.ToList();
                }
                clsAddBillBizActionVO BizAction = new clsAddBillBizActionVO();
                BizAction.IsFromCounterSale = true;
                BizAction.Details = new clsBillVO();
                BizAction.Details = objBill;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    Indicatior.Close();
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {

                        }

                    }

                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                Indicatior.Close();
                throw ex;
            }
            finally
            {
            }
        }

        double roundAmt = 0;
        private void CalculatePharmacySummary()
        {
            double Total, Concession, NetAmount, TotalVat;
            Total = Concession = NetAmount = TotalVat = PackageConcenssion = 0;
            for (int i = 0; i < PharmacyItems.Count; i++)
            {
                Total += (PharmacyItems[i].Amount);
                Concession += PharmacyItems[i].ConcessionAmount;
                TotalVat += PharmacyItems[i].VATAmount;
                NetAmount += PharmacyItems[i].NetAmount;
            }

            //  NetAmount = (Total - Concession) + TotalVat;
            PharmacyTotal = String.Format("{0:0.00}", Total);
            PharmacyConcession = String.Format("{0:0.00}", Concession);
            PharmacyNetAmount = String.Format("{0:0.00}", NetAmount);

            NetAmount1 = String.Format("{0:0.00}", NetAmount);
            TotalBill = String.Format("{0:0.00}", Total);
            TotalConcession = String.Format("{0:0.00}", Concession);

            //RountOffAmount = Math.Round(NetAmount).ToString();
            RountOffAmount = String.Format("{0:0.00}", NetAmount);
            roundAmt = Math.Round(NetAmount);
            PayAmount = NetAmount1;

        }

        private void CalculateConversionFactorCentral2(long FromUOMID, long ToUOMID)
        {
            clsConversions objConversionVO = new clsConversions();

            try
            {
                List<clsConversionsVO> UOMConvertLIst = new List<clsConversionsVO>();

                if (dgItemList.SelectedItem != null)
                {
                    UOMConvertLIst = PharmacyItems[dgItemList.SelectedIndex].UOMConversionList;
                    objConversionVO.UOMConvertLIst = UOMConvertLIst;

                    objConversionVO.MainMRP = PharmacyItems[dgItemList.SelectedIndex].MainMRP;
                    objConversionVO.MainRate = PharmacyItems[dgItemList.SelectedIndex].MainRate;
                    objConversionVO.SingleQuantity = Convert.ToSingle(PharmacyItems[dgItemList.SelectedIndex].Quantity);

                    long BaseUOMID = PharmacyItems[dgItemList.SelectedIndex].BaseUOMID;
                    objConversionVO.CalculateConversionFactor(FromUOMID, ToUOMID, BaseUOMID);

                    PharmacyItems[dgItemList.SelectedIndex].PurchaseRate = objConversionVO.Rate;
                    PharmacyItems[dgItemList.SelectedIndex].MRP = objConversionVO.MRP;

                    PharmacyItems[dgItemList.SelectedIndex].Quantity = objConversionVO.SingleQuantity;
                    PharmacyItems[dgItemList.SelectedIndex].BaseQuantity = objConversionVO.BaseQuantity;

                    PharmacyItems[dgItemList.SelectedIndex].BaseRate = objConversionVO.BaseRate;
                    PharmacyItems[dgItemList.SelectedIndex].BaseMRP = objConversionVO.BaseMRP;

                    PharmacyItems[dgItemList.SelectedIndex].ConversionFactor = objConversionVO.ConversionFactor;
                    PharmacyItems[dgItemList.SelectedIndex].BaseConversionFactor = objConversionVO.BaseConversionFactor;

                }

            }
            catch (Exception ex)
            {
                objConversionVO = null;
            }
        }


        private void FillBillSearchList()
        {
            clsGetBillSearchListBizActionVO BizAction = new clsGetBillSearchListBizActionVO();
            BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            BizAction.Opd_Ipd_External_Id = Opd_Ipd_External_Id;
            BizAction.Opd_Ipd_External_UnitId = Opd_Ipd_External_UnitId;
            BizAction.Opd_Ipd_External = 1;
            BizAction.BillType = (BillTypes)(2);

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
            {
                BizAction.UnitID = BizAction.PatientUnitID.Value;
            }
            else
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
            else
                BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IPDCounterID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    clsGetBillSearchListBizActionVO result = e.Result as clsGetBillSearchListBizActionVO;
                    if (result.List != null)
                    {
                        foreach (var item in result.List)
                        {
                            if (item.IsFreezed == true && item.Opd_Ipd_External_Id == Opd_Ipd_External_Id && item.Opd_Ipd_External_UnitId == Opd_Ipd_External_UnitId && item.Opd_Ipd_External == 1 && item.BillType == PalashDynamics.ValueObjects.BillTypes.Pharmacy)
                            {
                                ISFreeze = true;
                            }
                            else
                            {
                                SelectedBill = item;
                            }
                        }
                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        //Package New Changes Added on 02052018
        private void cmbApplicabelPackage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbApplicabelPackage.SelectedItem != null)      // For Package New Changes Added on 02052018
            {
                if (MaterialConsumptionAddedItems != null && MaterialConsumptionAddedItems.Count > 0)
                {
                    MaterialConsumptionAddedItems = new ObservableCollection<clsMaterialConsumptionItemDetailsVO>();
                    dgItemList.ItemsSource = null;
                    dgItemList.ItemsSource = MaterialConsumptionAddedItems;
                    dgItemList.UpdateLayout();
                }

                if (PharmacyItems != null && PharmacyItems.Count > 0)
                {
                    PharmacyItems = new ObservableCollection<clsItemSalesDetailsVO>();
                }

                SumOfTotals();
                CalculatePharmacySummary();
            }
        }

    }
}
