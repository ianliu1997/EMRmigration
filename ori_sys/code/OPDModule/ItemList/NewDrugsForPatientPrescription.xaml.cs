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
using PalashDynamics.ValueObjects.EMR;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.Pharmacy.ViewModels;
using OPDModule;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using PalashDynamics.Collections;
using OPDModule.Forms;
using System.Windows.Browser;
using PalashDynamics.UserControls;
using System.Text;


namespace OPDModule.ItemList
{
    public partial class NewDrugsForPatientPrescription : ChildWindow
    {
        public event RoutedEventHandler OnSaveButton_Click;

        public NewDrugsForPatientPrescription()
        {
            InitializeComponent();
            this.DataContext = new ItemSearchViewModel();
            this.Loaded += new RoutedEventHandler(DrugsForPatientPrescription_Loaded);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }


        void DrugsForPatientPrescription_Loaded(object sender, RoutedEventArgs e)
        {
            // 
            //if(VisitID!=0)
            //{ 
            //    clsGetPatientPrescriptionDetailByVisitIDBizActionVO BizAction=new clsGetPatientPrescriptionDetailByVisitIDBizActionVO();
            //    BizAction.VisitID = this.VisitID;

            //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //    Client.ProcessCompleted += (s, args) =>
            //    {
            //        if (args.Error == null && args.Result != null)
            //        {
            //            dataGrid2.ItemsSource = ((clsGetPatientPrescriptionDetailByVisitIDBizActionVO)args.Result).PatientPrescriptionDetail;
            //        }
            //    };

            //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //    Client.CloseAsync();
            //}
            FillStores(ClinicID);
            if (((IApplicationConfiguration)App.Current).SelectedPatient.UnitId != null && ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId > 0)
            {
                GetVisitDetails(((IApplicationConfiguration)App.Current).SelectedPatient.MRNo);
            }


        }

        //added by neena

        List<clsPatientPrescriptionDetailVO> PrescIdlist = null;
        bool flag = false;
        long PresID = 0;
        long PrescriptionUnitID = 0;
        double totalQuantity = 0;
        //
        private void GetDrugInfo(long VisitID)
        {

            if (VisitID > 0)
            {
                clsGetPatientPrescriptionDetailByVisitIDBizActionVO BizAction = new clsGetPatientPrescriptionDetailByVisitIDBizActionVO();
                BizAction.VisitID = VisitID;
                //BizAction.IsFrom = (int)PrescriptionFrom.Consultation;
                //By Anjali.................
                BizAction.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                //........................

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {

                        dataGrid2.ItemsSource = ((clsGetPatientPrescriptionDetailByVisitIDBizActionVO)args.Result).PatientPrescriptionDetail;
                        totalQuantity = ((clsGetPatientPrescriptionDetailByVisitIDBizActionVO)args.Result).TotalNewPendingQuantity;

                        if (dataGrid2.ItemsSource != null)
                        {
                            foreach (var item in ((clsGetPatientPrescriptionDetailByVisitIDBizActionVO)args.Result).PatientPrescriptionDetail)
                            {
                                if (item.Billed)
                                {
                                    item.VisibleBill = "Visible";
                                    item.collapseBill = "Collapsed";
                                    //item.SelectService = true;
                                    //item.IsBillEnabled = false;
                                }
                                else
                                {
                                    item.VisibleBill = "Collapsed";
                                    item.collapseBill = "Visible";
                                    //item.SelectService = false;
                                    //item.IsBillEnabled = true;
                                }
                            }
                        }
                                



                        //added by neena
                        if (dgPastVisitList.CurrentColumn.DisplayIndex == 0)
                        {
                            if (((clsGetPatientPrescriptionDetailByVisitIDBizActionVO)args.Result).PatientPrescriptionDetail != null && ((clsGetPatientPrescriptionDetailByVisitIDBizActionVO)args.Result).PatientPrescriptionDetail.Count > 0)
                            {
                                PrescIdlist = new List<clsPatientPrescriptionDetailVO>();
                                foreach (var item in ((clsGetPatientPrescriptionDetailByVisitIDBizActionVO)args.Result).PatientPrescriptionDetail)
                                {
                                    clsPatientPrescriptionDetailVO obj = new clsPatientPrescriptionDetailVO();
                                    obj.ID = item.ID;
                                    obj.UnitID = item.UnitID;
                                    obj.Quantity = item.Quantity;
                                    obj.PrescriptionID = item.PrescriptionID;
                                    obj.NewPendingQuantity = item.NewPendingQuantity;
                                    obj.ActualPrescribedBaseQuantity = Convert.ToSingle(item.NewPendingQuantity);
                                    //obj.ActualPrescribedBaseQuantity = Convert.ToSingle(item.NewPendingQuantity * item.SellingCF);
                                    //obj.Quantity = Math.Ceiling(obj.ActualPrescribedBaseQuantity / item.SellingCF);
                                    obj.SellingCF = item.SellingCF;
                                    PresID = item.PrescriptionID;
                                    PrescriptionUnitID = item.UnitID;
                                    PrescIdlist.Add(obj);
                                }

                                if (PrescIdlist != null)
                                {
                                    string SendPrescriptionID = string.Empty;
                                    long PrescriptionID = 0;
                                    StringBuilder builder = new StringBuilder();
                                    foreach (var item in PrescIdlist)
                                    {
                                        PrescriptionID = item.ID;

                                        builder.Append(PrescriptionID).Append(",");

                                    }

                                    SendPrescriptionID = builder.ToString();

                                    if (SendPrescriptionID.Length != 0)
                                    {
                                        SendPrescriptionID = SendPrescriptionID.TrimEnd(',');
                                    }



                                    clsGetPatientPrescriptionDetailByVisitIDForPrintBizActionVO BizAction1 = new clsGetPatientPrescriptionDetailByVisitIDForPrintBizActionVO();
                                    BizAction1.PatientPrescriptionDetailObj = new clsPatientPrescriptionDetailVO();
                                    BizAction1.SendPrescriptionID = SendPrescriptionID;
                                    BizAction1.PatientPrescriptionDetailObj.UnitID = PrescIdlist[0].UnitID;
                                    Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                                    PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                                    Client1.ProcessCompleted += (s1, args1) =>
                                    {
                                        if (args1.Error == null && ((clsGetPatientPrescriptionDetailByVisitIDForPrintBizActionVO)args1.Result).PatientPrescriptionDetail != null)
                                        {
                                            if (((clsGetPatientPrescriptionDetailByVisitIDForPrintBizActionVO)args1.Result).PatientPrescriptionDetail.Count == 0)
                                            {
                                                flag = true;
                                            }

                                            double calQuantity = 0.0;
                                            //if (((clsGetPatientPrescriptionDetailByVisitIDForPrintBizActionVO)args1.Result).PatientPrescriptionDetail != null)
                                            //{

                                            if (PrescIdlist.Count > ((clsGetPatientPrescriptionDetailByVisitIDForPrintBizActionVO)args1.Result).PatientPrescriptionDetail.Count)
                                                flag = true;

                                            foreach (var item in PrescIdlist)
                                            {
                                                calQuantity = item.Quantity * item.SellingCF;
                                                foreach (var item1 in ((clsGetPatientPrescriptionDetailByVisitIDForPrintBizActionVO)args1.Result).PatientPrescriptionDetail)
                                                {
                                                    if (item.ID == item1.ID && item.UnitID == item1.UnitID)
                                                    {
                                                        if (calQuantity > item1.SaleQuantity)
                                                        {
                                                            flag = true;
                                                        }
                                                    }
                                                }
                                            }

                                            if (flag == true)
                                            {
                                                //page open 

                                                if (dgPastVisitList.CurrentColumn.DisplayIndex == 0)
                                                {
                                                    CounterSalePrescriptionReason WinReason = new CounterSalePrescriptionReason();
                                                    WinReason.PrescriptionID = PresID;
                                                    WinReason.PrescriptionUnitID = PrescriptionUnitID;
                                                    WinReason.OnSaveButton_Click += new RoutedEventHandler(WinReason_OnSaveButton_Click);
                                                    //WinReason.FillReason();
                                                    WinReason.Show();
                                                }
                                            }
                                            else
                                            {
                                                //print
                                                string msgText = "";
                                                msgText = "Are You Sure \n Do You Want Patient Drug Report?";
                                                MessageBoxControl.MessageBoxChildWindow msgW =
                                                      new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(rptDrugDetails);
                                                msgW.Show();
                                            }
                                            //}
                                        }
                                        else
                                        {
                                            //HtmlPage.Window.Alert("Error occured while processing.");
                                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                            msgW1.Show();
                                        }
                                    };

                                    Client1.ProcessAsync(BizAction1, ((IApplicationConfiguration)App.Current).CurrentUser);
                                    Client1.CloseAsync();
                                }
                            }
                            else if (dgPastVisitList.CurrentColumn.DisplayIndex == 0)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                         new MessageBoxControl.MessageBoxChildWindow("Palash", "No Prescriptions Are Available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }
                        }
                        //
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
        }


        //added by neena
        void WinReason_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Reason Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);
            msgW1.Show();
        }

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            //print
            string msgText = "";
            msgText = "Are You Sure \n Do You Want Patient Drug Report?";
            MessageBoxControl.MessageBoxChildWindow msgW =
                  new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(rptDrugDetails);
            msgW.Show();
        }
        //

        public clsUserVO loggedinUser { get; set; }

        public long VisitID { get; set; }
        public long StoreID { get; set; }

        public long ClinicID
        {
            get
            {
                return ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
        }

        public clsPatientPrescriptionDetailVO SelectedDrug { get; set; }

        private ObservableCollection<clsItemStockVO> _BatchSelected;
        public ObservableCollection<clsItemStockVO> SelectedBatches { get { return _BatchSelected; } }


        private void FillStores(long pClinicID)
        {
            WaitIndicator wait = new WaitIndicator();
            wait.Show();
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_Store;
            BizAction.MasterList = new List<MasterListItem>();

            if (pClinicID > 0)
            {
                BizAction.Parent = new KeyValue { Value = "ClinicID", Key = pClinicID.ToString() };
            }


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
                    cmbStore.ItemsSource = null;
                    cmbStore.ItemsSource = objList;

                    if (StoreID > 0)
                    {
                        cmbStore.SelectedValue = StoreID;
                    }
                    else
                    {
                        if (objList.Count > 1)
                            cmbStore.SelectedItem = objList[1];
                        else
                            cmbStore.SelectedItem = objList[0];
                    }
                }
                wait.Close();

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
            if (OnSaveButton_Click != null)
                OnSaveButton_Click(this, new RoutedEventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedDrug = null;
            _BatchSelected = null;

            this.DialogResult = false;
        }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid2.SelectedItem != null)
            {
                if (((clsPatientPrescriptionDetailVO)dataGrid2.SelectedItem).IsItemBlock != true)
                {
                    if (_BatchSelected != null)
                        _BatchSelected.Clear();

                    if (dataGrid2.SelectedItem != null)
                    {
                        SelectedDrug = (clsPatientPrescriptionDetailVO)dataGrid2.SelectedItem;
                        if (SelectedDrug.DrugID == 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Item is not available ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            // ((clsServiceMasterVO)dgPrescriptionServiceList.SelectedItem).SelectService = false;
                            SelectedDrug = new clsPatientPrescriptionDetailVO();

                        }
                        else
                        {
                            ((ItemSearchViewModel)this.DataContext).SelectedItemID = ((clsPatientPrescriptionDetailVO)dataGrid2.SelectedItem).DrugID;
                            ((ItemSearchViewModel)this.DataContext).StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                            ((ItemSearchViewModel)this.DataContext).loggedinUser = loggedinUser;
                            ((ItemSearchViewModel)this.DataContext).GetBatches();
                        }
                    }
                }
            }

        }

        private ObservableCollection<clsItembatchSearchVO> _ItemBatchList;
        public ObservableCollection<clsItembatchSearchVO> ItemBatchList { get { return _ItemBatchList; } }

        private void AddBatch_Click(object sender, RoutedEventArgs e)
        {
            //if (dgItemBatches.SelectedItem != null)
            //{
            //    if (_BatchSelected == null)
            //        _BatchSelected = new ObservableCollection<clsItemStockVO>();

            //    CheckBox chk = (CheckBox)sender;

            //    if (chk.IsChecked == true)
            //        _BatchSelected.Add((clsItemStockVO)dgItemBatches.SelectedItem);
            //    else
            //        _BatchSelected.Remove((clsItemStockVO)dgItemBatches.SelectedItem);

            //}

            if (dgItemBatches.SelectedItem != null)
            {
                if (_BatchSelected == null)
                    _BatchSelected = new ObservableCollection<clsItemStockVO>();

                CheckBox chk = (CheckBox)sender;

                if (chk.IsChecked == true)
                    _BatchSelected.Add((clsItemStockVO)dgItemBatches.SelectedItem);
                else
                    _BatchSelected.Remove((clsItemStockVO)dgItemBatches.SelectedItem);

                if (_ItemBatchList == null)
                    _ItemBatchList = new ObservableCollection<clsItembatchSearchVO>();
                clsItembatchSearchVO objItem = new clsItembatchSearchVO();
                clsPatientPrescriptionDetailVO ObjPrescribeItem = new clsPatientPrescriptionDetailVO();
                if (dataGrid2.SelectedItem == null)
                {

                    objItem.PrescribeDrugs.DrugID = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].DrugID;
                    objItem.ItemCode = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].ItemCode;
                    objItem.PrescribeDrugs.DrugName = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].DrugName;
                    objItem.PrescribeDrugs.Status = false;
                    objItem.PrescribeDrugs.Frequency = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].Frequency;
                    objItem.PrescribeDrugs.IsBatchRequired = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].IsBatchRequired;
                    objItem.PrescribeDrugs.Quantity = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].Quantity;

                    objItem.PrescribeDrugs.IsInclusiveOfTax = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].IsInclusiveOfTax;
                    objItem.PrescribeDrugs.IsPrescribedMedicine = false;

                    objItem.CategoryId = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].ItemCategory;
                    objItem.GroupId = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].ItemGroup;

                    objItem.Manufacturer = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].Manufacture;
                    objItem.PreganancyClass = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].PregnancyClass;

                    // objItem.BrandName = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].BrandName;

                    //objItem.BatchesRequired = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].BatchesRequired;

                    //objItem.SUOM = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].SUOM;
                    // objItem.PUOM = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].PUOM;
                    //   objItem.InclusiveOfTax = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].InclusiveOfTax;
                    //   objItem.Manufacturer = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].Manufacturer;
                    //  objItem.PreganancyClass = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].PreganancyClass;


                    //By Anjali.................
                    objItem.ItemVatPer = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].SItemVatPer;
                    objItem.ItemVatType = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].SItemVatType;
                    objItem.ItemVatApplicationOn = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].SItemVatApplicationOn;
                    objItem.VATPerc = Convert.ToDouble(((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].SVatPer);

                    // Begin Properties for Sale 29062017 By Bhushanp

                    objItem.SGSTPercentSale = ((clsItemStockVO)(dgItemBatches.SelectedItem)).SGSTPercentage;
                    objItem.CGSTPercentSale = ((clsItemStockVO)(dgItemBatches.SelectedItem)).CGSTPercentage;
                    objItem.IGSTPercentSale = ((clsItemStockVO)(dgItemBatches.SelectedItem)).IGSTPercentage;

                    objItem.SGSTtaxtypeSale = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].SGSTtaxtypeSale;
                    objItem.SGSTapplicableonSale = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].SGSTapplicableonSale;

                    objItem.CGSTtaxtypeSale = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].CGSTtaxtypeSale;
                    objItem.CGSTapplicableonSale = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].CGSTapplicableonSale;

                    objItem.IGSTtaxtypeSale = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].IGSTtaxtypeSale;
                    objItem.IGSTapplicableonSale = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].IGSTapplicableonSale;

                    // End Properties for Sale 29062017

                    objItem.PUM = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].PUM;
                    objItem.SUM = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].SUM;
                    objItem.BaseUM = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].BaseUM;
                    objItem.BaseUMString = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].BaseUMString;
                    objItem.SellingUM = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].SellingUM;
                    objItem.SellingUMString = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].SellingUMString;
                    objItem.SUOM = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].SUOM;
                    objItem.PUOM = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].PUOM;
                    objItem.SUOMID = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].SUM;
                    objItem.BUOMID = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].BaseUM;

                    objItem.StockingCF = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].StockingCF;
                    objItem.SellingCF = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].SellingCF;
                    objItem.Rackname = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].Rackname;
                    objItem.Containername = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].Containername;
                    objItem.Shelfname = ((PagedSortableCollectionView<clsPatientPrescriptionDetailVO>)dataGrid2.ItemsSource)[0].Shelfname;
                    //.......................

                }
                else
                {
                    objItem.PrescribeDrugs.DrugID = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).DrugID;
                    objItem.ItemCode = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).ItemCode;
                    objItem.PrescribeDrugs.DrugName = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).DrugName;
                    objItem.PrescribeDrugs.Frequency = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).Frequency;
                    objItem.PrescribeDrugs.IsBatchRequired = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).IsBatchRequired;
                    objItem.PrescribeDrugs.Status = false;
                    objItem.PrescribeDrugs.Quantity = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).Quantity;

                    objItem.PrescribeDrugs.IsInclusiveOfTax = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).IsInclusiveOfTax;
                    objItem.PrescribeDrugs.IsPrescribedMedicine = true;

                    objItem.CategoryId = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).ItemCategory;
                    objItem.GroupId = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).ItemGroup;

                    objItem.Manufacturer = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).Manufacture;
                    objItem.PreganancyClass = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).PregnancyClass;

                    //objItem.PUOM = ((clsItemMasterVO)(dataGrid2.SelectedItem)).PUOM;
                    //objItem.InclusiveOfTax = ((clsItemMasterVO)(dataGrid2.SelectedItem)).InclusiveOfTax;
                    //objItem.Manufacturer = ((clsItemMasterVO)(dataGrid2.SelectedItem)).Manufacturer;
                    //objItem.PreganancyClass = ((clsItemMasterVO)(dataGrid2.SelectedItem)).PreganancyClass;
                    //objItem.TotalPerchaseTaxPercent = ((clsItemMasterVO)(dataGrid2.SelectedItem)).TotalPerchaseTaxPercent;
                    //objItem.TotalSalesTaxPercent = ((clsItemMasterVO)(dataGrid2.SelectedItem)).TotalSalesTaxPercent;
                    //objItem.AssignSupplier = ((clsItemMasterVO)(dataGrid2.SelectedItem)).AssignSupplier;

                    //by neena
                    objItem.PrescribeDrugs.TotalNewPendingQuantity = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).TotalNewPendingQuantity;
                    //

                    //By Anjali
                    objItem.PrescribeDrugs.ID = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).ID;
                    objItem.PrescribeDrugs.UnitID = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).UnitID;
                    objItem.PrescribeDrugs.PrescriptionID = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).PrescriptionID;
                    objItem.PrescribeDrugs.PendingQuantity = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).PendingQuantity;
                    objItem.PrescribeDrugs.NewPendingQuantity = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).NewPendingQuantity;


                    //By Anjali.................
                    objItem.ItemVatPer = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).SItemVatPer;
                    objItem.ItemVatType = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).SItemVatType;
                    objItem.ItemVatApplicationOn = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).SItemVatApplicationOn;
                    objItem.VATPerc = Convert.ToDouble(((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).SVatPer);
                    //.......................

                    // Begin Properties for Sale 29062017 By Bhushanp

                    objItem.SGSTPercentSale = ((clsItemStockVO)(dgItemBatches.SelectedItem)).SGSTPercentage;
                    objItem.CGSTPercentSale = ((clsItemStockVO)(dgItemBatches.SelectedItem)).CGSTPercentage;
                    objItem.IGSTPercentSale = ((clsItemStockVO)(dgItemBatches.SelectedItem)).IGSTPercentage;

                    objItem.SGSTtaxtypeSale = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).SGSTtaxtypeSale;
                    objItem.SGSTapplicableonSale = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).SGSTapplicableonSale;

                    objItem.CGSTtaxtypeSale = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).CGSTtaxtypeSale;
                    objItem.CGSTapplicableonSale = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).CGSTapplicableonSale;

                    objItem.IGSTtaxtypeSale = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).IGSTtaxtypeSale;
                    objItem.IGSTapplicableonSale = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).IGSTapplicableonSale;

                    // End Properties for Sale 29062017
                    objItem.PUM = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).PUM;
                    objItem.SUM = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).SUM;
                    objItem.BaseUM = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).BaseUM;
                    objItem.BaseUMString = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).BaseUMString;
                    objItem.SellingUM = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).SellingUM;
                    objItem.SellingUMString = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).SellingUMString;
                    objItem.SUOM = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).SUOM;
                    objItem.PUOM = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).PUOM;
                    objItem.SUOMID = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).SUM;
                    objItem.BUOMID = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).BaseUM;

                    objItem.StockingCF = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).StockingCF;
                    objItem.SellingCF = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).SellingCF;

                    objItem.Rackname = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).Rackname;
                    objItem.Containername = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).Containername;
                    objItem.Shelfname = ((clsPatientPrescriptionDetailVO)(dataGrid2.SelectedItem)).Shelfname;

                    //..........................................................................
                }

                if (dgItemBatches.SelectedItem != null)
                {
                    objItem.ID = ((clsItemStockVO)(dgItemBatches.SelectedItem)).ID;

                    objItem.Status = ((clsItemStockVO)(dgItemBatches.SelectedItem)).Status;
                    objItem.StoreID = ((clsItemStockVO)(dgItemBatches.SelectedItem)).StoreID;
                    objItem.ItemID = ((clsItemStockVO)(dgItemBatches.SelectedItem)).ItemID;
                    objItem.BatchID = ((clsItemStockVO)(dgItemBatches.SelectedItem)).BatchID;
                    objItem.AvailableStock = ((clsItemStockVO)(dgItemBatches.SelectedItem)).AvailableStock;
                    objItem.BatchCode = ((clsItemStockVO)(dgItemBatches.SelectedItem)).BatchCode;
                    objItem.ExpiryDate = ((clsItemStockVO)(dgItemBatches.SelectedItem)).ExpiryDate;
                    objItem.MRP = ((clsItemStockVO)(dgItemBatches.SelectedItem)).MRP;
                    objItem.PurchaseRate = ((clsItemStockVO)(dgItemBatches.SelectedItem)).PurchaseRate;

                    objItem.Date = ((clsItemStockVO)(dgItemBatches.SelectedItem)).Date;
                    objItem.VATAmt = ((clsItemStockVO)(dgItemBatches.SelectedItem)).VATAmt;
                    objItem.DiscountOnSale = ((clsItemStockVO)(dgItemBatches.SelectedItem)).DiscountOnSale;
                    //By Anjali.............................
                    // objItem.VATPerc = ((clsItemStockVO)(dgItemBatches.SelectedItem)).VATPerc;
                    objItem.Re_Order = ((clsItemStockVO)(dgItemBatches.SelectedItem)).Re_Order;
                    objItem.AvailableStockInBase = ((clsItemStockVO)(dgItemBatches.SelectedItem)).AvailableStockInBase;
                    //...................................
                }
                if (chk.IsChecked == true)
                {
                    // _ItemBatchList.Add(objItem);
                    bool Addflag = true;
                    if (_ItemBatchList.Count == 0)
                        _ItemBatchList.Add(objItem);
                    else
                    {
                        foreach (var item in _ItemBatchList)
                        {
                            if (item.ItemID == objItem.ItemID && item.BatchCode == objItem.BatchCode)
                            {
                                Addflag = false;
                                //_ItemBatchList.Remove(item);
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Item is already added ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                                break;
                            }
                            //else
                            //    _ItemBatchList.Add(objItem);
                        }
                        if (Addflag == true)
                            _ItemBatchList.Add(objItem);
                    }

                }
                else
                {
                    foreach (var item in _ItemBatchList)
                    {
                        if (item.ItemID == objItem.ItemID && item.BatchCode == objItem.BatchCode)
                        {
                            _ItemBatchList.Remove(item);
                            break;
                        }
                    }
                }

                foreach (var item in ItemBatchList)
                {
                    if (item.PrescribeDrugs.PrescriptionID == objItem.PrescribeDrugs.PrescriptionID)
                        objItem.PrescribeDrugs.TotalNewPendingQuantity = totalQuantity;
                }

                dgSelectedItemList.ItemsSource = null;
                dgSelectedItemList.ItemsSource = ItemBatchList;



            }
        }

        private void dgItemBatches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void GetVisitDetails(string MrNo)
        {
            clsGetPatientPastVisitBizActionVO BizAction = new clsGetPatientPastVisitBizActionVO();
            BizAction.MRNO = MrNo;
            //if (BizAction.UnitID != null || BizAction.UnitID != 0)
            //{
            //    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //}

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitID = 0;
            }
            else
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            BizAction.SpouseID = ((IApplicationConfiguration)App.Current).SelectedPatient.SpouseID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            dgPastVisitList.ItemsSource = null;

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null && ((clsGetPatientPastVisitBizActionVO)arg.Result).VisitList != null)
                    {
                        int ItemCount = 0;
                        List<clsVisitVO> VisitList = new List<clsVisitVO>();
                        //foreach (var item in ((clsGetPatientPastVisitBizActionVO)arg.Result).VisitList)
                        //{

                        foreach (var item2 in ((clsGetPatientPastVisitBizActionVO)arg.Result).VisitList.OrderByDescending(o => o.ID).ToList())
                        {
                            if (ItemCount < 5)
                            {
                                VisitList.Add(item2);

                            }
                            ItemCount++;
                        }



                        //}
                        dgPastVisitList.ItemsSource = VisitList;
                    }
                }
                else
                {
                    //HtmlPage.Window.Alert("Error occured while processing.");
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void dgVisitList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgPastVisitList.CurrentColumn.DisplayIndex != 0)
            {
                GetDrugInfo(((clsVisitVO)dgPastVisitList.SelectedItem).ID);
            }
        }

        private void CheckBox1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MoreVisit_Click(object sender, RoutedEventArgs e)
        {
            //frmGetPatientPastVisitChildWindow PatientPastVisit = new frmGetPatientPastVisitChildWindow();

            //PatientPastVisit.GetVisitDetails(((IApplicationConfiguration)App.Current).SelectedPatient.MRNo);
            //PatientPastVisit.OnSaveButton_Click += new RoutedEventHandler(VisitDetails_OnSaveButton_Click);
            //PatientPastVisit.Show();
        }

        void VisitDetails_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            //long VisitID = ((frmGetPatientPastVisitChildWindow)sender).VisitID;          
            //GetDrugInfo(VisitID);
        }
        //Added By AkshayS 
        private void printDrugDetails(object sender, RoutedEventArgs e)
        {

            GetDrugInfo(((clsVisitVO)dgPastVisitList.SelectedItem).ID);  //added by neena


            ////commented by neena
            //string msgText = "";
            //msgText = "Are You Sure \n Do You Want Patient Drug Report?";
            //MessageBoxControl.MessageBoxChildWindow msgW =
            //      new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            //msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(rptDrugDetails);
            //msgW.Show();
            ////
        }

        void rptDrugDetails(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                rptprintDrugDetails();
        }

        private void rptprintDrugDetails()
        {
            //long ID = ((clsPatientVO)this.DataContext).GeneralDetails.PatientID;
            //long UnitID = ((clsPatientVO)this.DataContext).GeneralDetails.UnitId;
            //long IsDoctorID ;
            //long IsEmployee;
            //long DoctorID ;
            //long EmployeeID;

            long VisitID = ((clsVisitVO)dgPastVisitList.SelectedItem).ID;
            long UnitID = ((clsVisitVO)dgPastVisitList.SelectedItem).UnitId;


            if (VisitID > 0 && UnitID > 0)
            {
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/EMRPatientPrescription.aspx?VisitID=" + VisitID + "&PatientID=" + ((clsVisitVO)dgPastVisitList.SelectedItem).PatientId + "&IsOPDIPD=0&UnitID=" + UnitID), "_blank");
                //string URL = "../Reports/Patient/rptPatientDrugReport.aspx?VisitID=" + VisitID + "&UnitID=" + UnitID;
                // HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }


        }

        private void dataGrid2_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (((clsPatientPrescriptionDetailVO)e.Row.DataContext).IsItemBlock == true)
            {
                e.Row.IsEnabled = false;
            }
            else
            {
                e.Row.IsEnabled = true;
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {

        }


        //added by neena
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (dgPastVisitList.CurrentColumn.DisplayIndex == 8)
            {
                long VisitID = ((clsVisitVO)dgPastVisitList.SelectedItem).ID;

                long UnitID = ((clsVisitVO)dgPastVisitList.SelectedItem).UnitId;

                GetCounterSalePrescriptionReason winReason = new GetCounterSalePrescriptionReason();
                winReason.VisitID = VisitID;
                winReason.UnitID = UnitID;
                winReason.GetPrescriptionID();
                winReason.Show();
            }
        }
        //
    }
}

