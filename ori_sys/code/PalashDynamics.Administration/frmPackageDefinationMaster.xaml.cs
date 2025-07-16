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
using System.Windows.Data;
using System.Globalization;
using System.Windows.Markup;
using PalashDynamics.Animations;
using PalashDynamics.CRM;
using CIMS;
using PalashDynamics.ValueObjects;
using System.Collections.ObjectModel;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Reflection;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Administration;
using System.Windows.Browser;
using System.Text;
using PalashDynamics.Pharmacy;
using PalashDynamics.Controls;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Administration.PackageNew;


namespace PalashDynamics.Administration
{
    public partial class frmPackageDefinationMaster : ChildWindow
    {
        #region Variable Declaration
        SwivelAnimation objAnimation;
        bool IsNew = false;
        bool IsCancel = true;
        bool Flag = false;

        public bool IsView = false;
        public bool IsFromPackageMainMaster = false;
        bool IsPageLoded = false;

        public ObservableCollection<clsPackageRelationsVO> ItemList1 { get; set; }
        public ObservableCollection<clsPackageItemMasterVO> ItemList12 { get; set; }

        public long PackageServiceID { get; set; }
        public long PackageServiceUnitID { get; set; }
        public long PackageID { get; set; }
        public bool IsFreeze { get; set; }

        public double PackageRate { get; set; }


        public bool IsCategoryOn { get; set; }
        public bool IsGroupOn { get; set; }

        string msgText = "";

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        clsPackageServiceVO objPrevPackMasterNew;

        public event RoutedEventHandler OnAddButton_Click;

        #endregion

        public frmPackageDefinationMaster()
        {
            InitializeComponent();
        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {


            if (!IsPageLoded)
            {
                rdbFixedNew.IsChecked = true;
                rdbPercentage.IsChecked = false;

                SetCommandButtonState("Load");
                ServiceList = new ObservableCollection<clsPackageServiceDetailsVO>();
                ItemList1 = new ObservableCollection<clsPackageRelationsVO>();
                ItemList12 = new ObservableCollection<clsPackageItemMasterVO>();

                this.DataContext = new clsPackageServiceVO();

                FillService();
                FillValidity();

                if (IsFreeze == true)
                {
                    cmdService.IsEnabled = false;
                    cmdItems.IsEnabled = false;
                    cmdSavePharmacyItems.IsEnabled = false;
                }
                else
                {
                    cmdService.IsEnabled = true;
                    cmdItems.IsEnabled = true;
                    cmdSavePharmacyItems.IsEnabled = true;
                }

                txtServiceFixedRate.Visibility = Visibility.Visible;
                txtPharmacyFixedRate.Visibility = Visibility.Visible;

                txtServicePercentage.Visibility = Visibility.Collapsed;
                txtPharmacyPercentage.Visibility = Visibility.Collapsed;
                txtServicePercentage.Text = "0";
                txtPharmacyPercentage.Text = "0";
            }

        }

        # region Fill Combos & Events

        private void FillValidity()
        {
            List<MasterListItem> lst = new List<MasterListItem>();
            lst.Add(new MasterListItem() { ID = 0, Description = "--Select--", Status = true });
            lst.Add(new MasterListItem() { ID = 1, Description = "Month", Status = true });
            lst.Add(new MasterListItem() { ID = 2, Description = "Days", Status = true });
            cmbValidity.ItemsSource = lst;
            cmbValidity.SelectedItem = lst[0];
        }

        private void FillService()
        {

            clsGetPackageServiceBizActionVO BizAction = new clsGetPackageServiceBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetPackageServiceBizActionVO)e.Result).MasterList);
                    cmbService.ItemsSource = null;
                    cmbService.ItemsSource = objList;
                    cmbService.SelectedItem = objList[0];

                    var result = from r in (objList)
                                 where r.ID == PackageServiceID
                                 select r;

                    if (result != null)
                    {
                        cmbService.SelectedItem = ((MasterListItem)result.FirstOrDefault());
                    }

                    if (IsFromPackageMainMaster == true)
                        cmbService.IsEnabled = false;

                    //FillGender();
                    FillProcedure();     // For Package New Changes Added on 16042018
                }

            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        List<MasterListItem> ProcedureList;
        // For Package New Changes Added on 16042018
        private void FillProcedure()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_ProcessMaster;    // For Package New Changes Added on 18042019
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    ProcedureList = new List<MasterListItem>();

                    ProcedureList.Add(new MasterListItem(0, "- Select -"));
                    ProcedureList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    FillGender();    // For Package New Changes Added on 16042018
                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        List<MasterListItem> GenderList;

        private void FillGender()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_ApplicableToGenderMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    GenderList = new List<MasterListItem>();

                    GenderList.Add(new MasterListItem(0, "- Select -"));
                    GenderList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    if (PackageID > 0)
                        GetPackageDetails(PackageID);

                    //if (PackageID > 0)
                    //    GetPackagePharmacyItemDetails(PackageID);
                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        private void cmbService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbService.SelectedItem != null)
            {
                if (((MasterListItem)cmbService.SelectedItem).ID > 0)
                {
                    GetAmount(((MasterListItem)cmbService.SelectedItem).ID);
                }
                else
                {
                    txtAmount.Text = "0";
                }
            }
        }


        # endregion

        # region Methods

        private void GetAmount(long serviceID)
        {
            try
            {
                clsGetTariffServiceListBizActionVO BizAction = new clsGetTariffServiceListBizActionVO();
                BizAction.ServiceList = new List<clsServiceMasterVO>();

                //BizAction.ServiceName = ((MasterListItem)cmbService.SelectedItem).Description;
                //BizAction.TariffID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID;

                BizAction.ServiceID = ((MasterListItem)cmbService.SelectedItem).ID;
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizAction.IsPackage = true;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList != null)
                        {
                            List<clsServiceMasterVO> ObjList = new List<clsServiceMasterVO>();
                            ObjList = ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList;

                            var item1 = from r in ObjList
                                        where (r.ID == serviceID)
                                        select new clsServiceMasterVO
                                        {
                                            Rate = r.Rate
                                        };
                            if (item1.ToList().Count > 0)
                            {
                                ObjList = item1.ToList();
                                //foreach (var item in ObjList)  //Commented by AJ Date 18/11/2016
                                //{
                                //    txtAmount.Text = item.Rate.ToString();
                                //}

                            }
                            else // Added By CDS Becouse First Time Package Rate Text Box Is Blank So below COde is Written 
                            {
                                txtAmount.Text = PackageRate.ToString();
                            }
                        }
                    }

                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
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

        private void GetPackageDetails(long packageid)
        {
            clsGetPackageServiceDetailsListNewBizActionVO BizAction = new clsGetPackageServiceDetailsListNewBizActionVO();
            try
            {
                BizAction.PackageMasterList = new clsPackageServiceVO();
                BizAction.PackageDetailList = new List<clsPackageServiceDetailsVO>();

                BizAction.PackageID = PackageID;  //packageid;
                BizAction.UnitId = PackageServiceUnitID;  //((clsPackageServiceVO)dgPackage.SelectedItem).UnitID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetPackageServiceDetailsListNewBizActionVO)arg.Result).PackageDetailList != null)
                        {
                            clsGetPackageServiceDetailsListNewBizActionVO DetailsVO = new clsGetPackageServiceDetailsListNewBizActionVO();
                            DetailsVO = (clsGetPackageServiceDetailsListNewBizActionVO)arg.Result;

                            List<clsPackageServiceDetailsVO> ObjItem;
                            ObjItem = DetailsVO.PackageDetailList;

                            ServiceList = new ObservableCollection<clsPackageServiceDetailsVO>();

                            foreach (var item4 in ObjItem)
                            {
                                ServiceList.Add(item4);

                            }

                            SetCommandButtonState("View");

                            if (PackageID > 0)  //(dgPackage.SelectedItem != null)
                            {
                                IsNew = false;
                                this.DataContext = ((clsGetPackageServiceDetailsListNewBizActionVO)arg.Result).PackageMasterList;   //((clsPackageServiceVO)dgPackage.SelectedItem);
                                cmbService.SelectedValue = ((clsPackageServiceVO)this.DataContext).ServiceID;
                                cmbValidity.SelectedValue = ((clsPackageServiceVO)this.DataContext).ValidityUnit;

                                chkSelectAll.IsChecked = ((clsPackageServiceVO)this.DataContext).ApplicableToAll;
                                txtDiscountOnAllItems.Text = Convert.ToString(((clsPackageServiceVO)this.DataContext).ApplicableToAllDiscount);
                                txtTotalBudget.Text = Convert.ToString(((clsPackageServiceVO)this.DataContext).TotalBudget);


                                if (((clsGetPackageServiceDetailsListNewBizActionVO)arg.Result).PackageMasterList.IsFixedRate == true)
                                {
                                    rdbFixedNew.IsChecked = true;
                                    txtServiceFixedRate.Visibility = Visibility.Visible;
                                    txtPharmacyFixedRate.Visibility = Visibility.Visible;
                                    txtServicePercentage.Visibility = Visibility.Collapsed;
                                    txtPharmacyPercentage.Visibility = Visibility.Collapsed;

                                    txtServiceFixedRate.Text = ((clsGetPackageServiceDetailsListNewBizActionVO)arg.Result).PackageMasterList.ServiceFixedRate.ToString();
                                    txtPharmacyFixedRate.Text = ((clsGetPackageServiceDetailsListNewBizActionVO)arg.Result).PackageMasterList.PharmacyFixedRate.ToString();
                                    txtServicePercentage.Text = "0";
                                    txtPharmacyPercentage.Text = "0";
                                }
                                else
                                {
                                    rdbPercentage.IsChecked = true;
                                    txtServiceFixedRate.Visibility = Visibility.Collapsed;
                                    txtPharmacyFixedRate.Visibility = Visibility.Collapsed;
                                    txtServicePercentage.Visibility = Visibility.Visible;
                                    txtPharmacyPercentage.Visibility = Visibility.Visible;
                                    txtServiceFixedRate.Text = "0";
                                    txtPharmacyFixedRate.Text = "0";
                                    txtServicePercentage.Text = ((clsGetPackageServiceDetailsListNewBizActionVO)arg.Result).PackageMasterList.ServicePercentage.ToString();
                                    txtPharmacyPercentage.Text = ((clsGetPackageServiceDetailsListNewBizActionVO)arg.Result).PackageMasterList.PharmacyPercentage.ToString();

                                    double PharmacyRate = (((clsGetPackageServiceDetailsListNewBizActionVO)arg.Result).PackageMasterList.PackageAmount * ((((clsGetPackageServiceDetailsListNewBizActionVO)arg.Result).PackageMasterList.PharmacyPercentage) / 100));

                                    ((clsPackageServiceVO)this.DataContext).TotalBudget = Convert.ToDouble(((((clsPackageServiceVO)this.DataContext).TotalBudget) / PharmacyRate) * 100);

                                    txtTotalBudget.Text = Convert.ToString(((clsPackageServiceVO)this.DataContext).TotalBudget); ///((clsGetPackageServiceDetailsListNewBizActionVO)arg.Result).PackageMasterList.PharmacyPercentage);


                                }

                                //chkFreezePackage.IsChecked = ((clsPackageServiceVO)this.DataContext).IsFreezed;

                                //if (chkFreezePackage.IsChecked == true)
                                //{
                                //    cmdModify.IsEnabled = false;
                                //    chkFreezePackage.IsEnabled = false;
                                //}
                                //else
                                //{
                                //    cmdModify.IsEnabled = true;
                                //    chkFreezePackage.IsEnabled = true;
                                //}

                                //if (chkSelectAll.IsChecked == true)
                                //{
                                //    txtDiscountOnAllItems.Visibility = Visibility.Visible;
                                //    txtBlkDiscount.Visibility = Visibility.Visible;

                                //    cmdItems.IsEnabled = false;
                                //}
                                //else
                                //{
                                //    txtDiscountOnAllItems.Visibility = Visibility.Collapsed;
                                //    txtBlkDiscount.Visibility = Visibility.Collapsed;

                                //    cmdItems.IsEnabled = true;
                                //}

                                //GetPackageDetails(((clsPackageServiceVO)dgPackage.SelectedItem).ID);
                                //dgCalender.IsReadOnly = false;

                                cmbService.IsEnabled = false;
                                //objAnimation.Invoke(RotationType.Forward);
                                ////cmdService.Visibility = System.Windows.Visibility.Collapsed;
                                cmbValidity.IsEnabled = false;
                                txtValidity.IsReadOnly = true;
                                IsView = true;

                            }

                            List<clsPackageServiceDetailsVO> objPackServlist = new List<clsPackageServiceDetailsVO>();
                            objPackServlist = ServiceList.ToList();

                            clsPackageServiceVO objPackMaster = new clsPackageServiceVO();
                            objPackMaster = ((clsGetPackageServiceDetailsListNewBizActionVO)arg.Result).PackageMasterList;
                            objPackMaster.PackageDetails = objPackServlist;

                            FillGridText(objPackMaster, true);  //objPackServlist

                            //List<clsPackageServiceDetailsVO> SortedList = ServiceList.OrderBy(o => o.ServiceName).ToList();
                            //var record = SortedList.Select(c => c.Month).Max().ToString();

                            //ObservableCollection<PackageList> PatientList2 = new ObservableCollection<PackageList>();
                            ////List<PackageList> PatientList1 = new List<PackageList>();

                            //PackageMonths list2item;

                            //System.Collections.ObjectModel.ObservableCollection<PackageMonths> list1item;
                            //var Cnt = "";
                            //int Cnt1 = 0;
                            //String SerName = "";
                            //list1item = new System.Collections.ObjectModel.ObservableCollection<PackageMonths>();

                            //foreach (var item in SortedList)
                            //{
                            //    if (Cnt1 == 0)
                            //        list1item = new System.Collections.ObjectModel.ObservableCollection<PackageMonths>();

                            //    list2item = new PackageMonths();

                            //    if (SerName == "" || SerName != item.ServiceName)
                            //        SerName = item.ServiceName;

                            //    list2item.Month = item.Month;

                            //    list2item.MonthStatus = item.MonthStatus;

                            //    if (SerName == item.ServiceName)
                            //    {
                            //        list1item.Add(list2item);
                            //        Cnt1++;
                            //    }
                            //    else
                            //    {
                            //        SerName = item.ServiceName;
                            //        Cnt1 = 0;
                            //    }

                            //    Cnt = SortedList.Count(c => c.ServiceName == SerName).ToString();

                            //    var result = from r in (GenderList)
                            //                 where r.ID == item.ApplicableTo
                            //                 select r;

                            //    if (result != null)
                            //    {
                            //        item.SelectedGender = ((MasterListItem)result.First());
                            //    }

                            //    if (Cnt1 == Convert.ToInt32(Cnt))
                            //    {
                            //        PatientList2.Add(new PackageList { ServiceID = item.ServiceID, ServiceName = item.ServiceName, Rate = item.Rate, Amount = item.Amount, Discount = item.Discount, GenderList = GenderList, SelectedGender = item.SelectedGender, Infinite = item.Infinite, Quantity = item.Quantity, FreeAtFollowUp = item.FreeAtFollowUp, Delete = item.Delete, Months = list1item, DepartmentID = item.DepartmentID, IsSpecilizationGroup = item.IsSpecilizationGroup });
                            //        Cnt1 = 0;
                            //    }

                            //}

                            //if (((clsGetPackageServiceDetailsListBizActionVO)arg.Result).ItemDetails != null)
                            //{
                            //    ItemList1 = new ObservableCollection<clsPackageItemMasterVO>(((clsGetPackageServiceDetailsListBizActionVO)arg.Result).ItemDetails);
                            //    dgPharmaItems.ItemsSource = ItemList1;
                            //}

                            //StringTable sampleTable = new StringTable();
                            //this.StringTable = sampleTable;
                            //for (int i = 0; i < PatientList2.Count; i++)
                            //{
                            //    StringRow newRowValues = new StringRow();

                            //    if (i == 0)
                            //    {
                            //        this.StringTable.ColumnNames.Add("Service Name");
                            //        this.StringTable.ColumnNames.Add("Rate");
                            //        this.StringTable.ColumnNames.Add("Unlimited Quantity");
                            //        this.StringTable.ColumnNames.Add("Quantity");
                            //        this.StringTable.ColumnNames.Add("Free At FollowUp");
                            //        this.StringTable.ColumnNames.Add("Delete");
                            //    }


                            //    newRowValues["Service Name"] = PatientList2[i].ServiceName;
                            //    newRowValues["Rate"] = PatientList2[i].Rate;
                            //    newRowValues["Unlimited Quantity"] = PatientList2[i].Infinite;
                            //    newRowValues["Quantity"] = PatientList2[i].Quantity;
                            //    newRowValues["Free At FollowUp"] = PatientList2[i].FreeAtFollowUp;
                            //    newRowValues["Delete"] = PatientList2[i].Delete;

                            //    for (int j = 0; j < PatientList2[i].Months.Count; j++)
                            //    {

                            //        if (i == 0)
                            //        {
                            //            this.StringTable.ColumnNames.Add(PatientList2[i].Months[j].Month);
                            //        }

                            //        newRowValues[PatientList2[i].Months[j].Month] = PatientList2[i].Months[j].MonthStatus;

                            //    }

                            //    this.StringTable.Add(this.StringTable.Count, newRowValues);
                            //}


                            //RefreshDataGrid(PatientList2);




                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }

                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }

        }

        # endregion

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void AddService_Click(object sender, RoutedEventArgs e)
        {
            if (ChkValidationToAddServiceGroup())
            {
                PackageServices objservice = new PackageServices();
                objservice.CmdAddFreeService.Visibility = System.Windows.Visibility.Collapsed;
                objservice.CmdconcessionService.Visibility = System.Windows.Visibility.Collapsed;
                objservice.TariffID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID;
                objservice.SelectedServicesVO = objPackMaster;
                objservice.PackageServiceID = PackageServiceID;

                if (txtValidity.Text.Trim() != String.Empty)
                    objservice.ValidityDuration = Convert.ToInt64(txtValidity.Text);

                objservice.OnAddButton_Click += new RoutedEventHandler(objservice_OnAddButton_Click);
                objservice.Show();
            }
        }

        private bool ChkValidationToAddServiceGroup()
        {
            bool result = true;

            if ((MasterListItem)cmbService.SelectedItem == null)
            {

                cmbService.TextBox.SetValidation("Please Select Service");
                cmbService.TextBox.RaiseValidationError();
                cmbService.Focus();
                result = false;


            }
            else if (((MasterListItem)cmbService.SelectedItem).ID == 0)
            {
                cmbService.TextBox.SetValidation("Please Select Service");
                cmbService.TextBox.RaiseValidationError();
                cmbService.Focus();
                result = false;

            }
            else
                cmbService.TextBox.ClearValidationError();

            if ((MasterListItem)cmbValidity.SelectedItem == null)
            {

                cmbValidity.TextBox.SetValidation("Please Select Duration Unit");
                cmbValidity.TextBox.RaiseValidationError();
                cmbValidity.Focus();
                result = false;


            }
            else if (((MasterListItem)cmbValidity.SelectedItem).ID == 0)
            {
                cmbValidity.TextBox.SetValidation("Please Select Duration Unit");
                cmbValidity.TextBox.RaiseValidationError();
                cmbValidity.Focus();
                result = false;

            }
            else
                cmbValidity.TextBox.ClearValidationError();

            if (txtValidity.Text == "0" || txtValidity.Text == "")
            {
                txtValidity.SetValidation("Package Duration can not be zero");
                txtValidity.RaiseValidationError();
                txtValidity.Focus();
                result = false;
            }
            else
                txtValidity.ClearValidationError();
            //Added By Bhushanp 17022017
            if (rdbFixedNew.IsChecked == true)
            {
                double TAmount = Convert.ToDouble(txtServiceFixedRate.Text) + Convert.ToDouble(txtPharmacyFixedRate.Text);
                if (Convert.ToDouble(txtAmount.Text) < TAmount || Convert.ToDouble(txtAmount.Text) > TAmount)
                {
                    txtPharmacyFixedRate.SetValidation("Please Enter valid Amount");
                    txtPharmacyFixedRate.RaiseValidationError();
                    txtServiceFixedRate.SetValidation("Please Enter valid Amount");
                    txtServiceFixedRate.RaiseValidationError();
                    txtServiceFixedRate.Focus();
                    result = false;
                }
                else if (0 >= TAmount)
                {
                    txtPharmacyFixedRate.SetValidation("Please Enter valid Amount");
                    txtPharmacyFixedRate.RaiseValidationError();
                    txtServiceFixedRate.SetValidation("Please Enter valid Amount");
                    txtServiceFixedRate.RaiseValidationError();
                    txtServiceFixedRate.Focus();
                    result = false;
                }
                else
                {
                    txtPharmacyFixedRate.ClearValidationError();
                    txtServiceFixedRate.ClearValidationError();
                }
            }

            //Added By Bhushanp 17022017
            if (rdbPercentage.IsChecked == true)
            {

                double TPercentage = Convert.ToDouble(txtServicePercentage.Text) + Convert.ToDouble(txtPharmacyPercentage.Text);
                if (100 < TPercentage || TPercentage < 100)
                {
                    txtServicePercentage.SetValidation("Please Enter valid Amount");
                    txtServicePercentage.RaiseValidationError();
                    txtPharmacyPercentage.SetValidation("Please Enter valid Amount");
                    txtPharmacyPercentage.RaiseValidationError();
                    result = false;
                    txtServicePercentage.Focus();
                }
                else if (0 >= TPercentage)
                {
                    txtServicePercentage.SetValidation("Please Enter valid Percentage");
                    txtServicePercentage.RaiseValidationError();
                    txtPharmacyPercentage.SetValidation("Please Enter valid Percentage");
                    txtPharmacyPercentage.RaiseValidationError();
                    result = false;
                    txtServicePercentage.Focus();
                }
                else
                {
                    txtServicePercentage.ClearValidationError();
                    txtPharmacyPercentage.ClearValidationError();
                }
            }

            return result;
        }

        ObservableCollection<clsPackageServiceDetailsVO> ServiceList { get; set; }
        clsPackageServiceVO objPackMaster = new clsPackageServiceVO();
        void objservice_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            //int Cnt = 0;

            if (((PackageServices)sender).DialogResult == true)
            {
                if (((PackageServices)sender).ServiceItemSource.Count != 0)
                {
                    for (int i = 0; i < ((PackageServices)sender).ServiceItemSource.Count; i++)
                    {
                        var item1 = from r in ServiceList
                                    where (r.ServiceID == (((PackageServices)sender).ServiceItemSource[i]).ID && r.DepartmentID == (((PackageServices)sender).ServiceItemSource[i]).Specialization
                                    && r.IsSpecilizationGroup == false
                                    && r.ProcessID == (((PackageServices)sender).ServiceItemSource[i]).ProcessID     // Package New Changes for Procedure Added on 17042018
                                    )
                                    select new clsPackageServiceDetailsVO
                                    {
                                        ServiceID = r.ServiceID,
                                        ServiceCode = r.ServiceCode,
                                        Status = r.Status

                                    };


                        if (item1.ToList().Count == 0)
                        {
                            if ((((PackageServices)sender).ServiceItemSource[i]).FromPackage == false)
                            {
                                clsPackageServiceDetailsVO ObjTemp = new clsPackageServiceDetailsVO();

                                ObjTemp.ServiceID = (((PackageServices)sender).ServiceItemSource[i]).ID;
                                ObjTemp.ServiceName = (((PackageServices)sender).ServiceItemSource[i]).ServiceName;
                                ObjTemp.ServiceCode = (((PackageServices)sender).ServiceItemSource[i]).ServiceCode;
                                ObjTemp.Rate = (double)((PackageServices)sender).ServiceItemSource[i].Rate;
                                ObjTemp.DepartmentID = (long)((PackageServices)sender).ServiceItemSource[i].Specialization;
                                ObjTemp.Department = ((PackageServices)sender).ServiceItemSource[i].SpecializationString;
                                ObjTemp.ProcessID = ((PackageServices)sender).ServiceItemSource[i].ProcessID;           // Package New Changes for Procedure Added on 17042018
                                ObjTemp.Quantity = 0;
                                ServiceList.Add(ObjTemp);

                                if (objPrevPackMasterNew != null && objPrevPackMasterNew.PackageDetails != null)
                                    objPrevPackMasterNew.PackageDetails.Add(ObjTemp);

                            }
                            else if ((((PackageServices)sender).ServiceItemSource[i]).FromPackage == true)
                            {
                                for (int j = 0; j < ((PackageServices)sender).ServiceItemSource[i].PackageInPackageItemList.Count; j++)
                                {
                                    clsPackageServiceDetailsVO ObjTemp = new clsPackageServiceDetailsVO();

                                    ObjTemp.ServiceID = (((PackageServices)sender).ServiceItemSource[i]).PackageInPackageItemList[j].ServiceID;
                                    ObjTemp.ServiceName = (((PackageServices)sender).ServiceItemSource[i]).PackageInPackageItemList[j].ServiceName;
                                    ObjTemp.ServiceCode = (((PackageServices)sender).ServiceItemSource[i]).PackageInPackageItemList[j].ServiceCode;
                                    ObjTemp.Rate = (double)((PackageServices)sender).ServiceItemSource[i].PackageInPackageItemList[j].Rate;
                                    ObjTemp.DepartmentID = (long)((PackageServices)sender).ServiceItemSource[i].PackageInPackageItemList[j].DepartmentID;
                                    ObjTemp.Department = ((PackageServices)sender).ServiceItemSource[i].PackageInPackageItemList[j].Department;
                                    ObjTemp.Quantity = ((PackageServices)sender).ServiceItemSource[i].PackageInPackageItemList[j].Quantity;

                                    ObjTemp.Amount = ((PackageServices)sender).ServiceItemSource[i].PackageInPackageItemList[j].Amount;
                                    ObjTemp.Discount = ((PackageServices)sender).ServiceItemSource[i].PackageInPackageItemList[j].Discount;
                                    ObjTemp.SelectedGender = ((PackageServices)sender).ServiceItemSource[i].PackageInPackageItemList[j].SelectedGender;
                                    ObjTemp.Infinite = ((PackageServices)sender).ServiceItemSource[i].PackageInPackageItemList[j].Infinite;
                                    ObjTemp.FreeAtFollowUp = ((PackageServices)sender).ServiceItemSource[i].PackageInPackageItemList[j].FreeAtFollowUp;
                                    ObjTemp.IsSpecilizationGroup = ((PackageServices)sender).ServiceItemSource[i].PackageInPackageItemList[j].IsSpecilizationGroup;
                                    ObjTemp.Month = ((PackageServices)sender).ServiceItemSource[i].PackageInPackageItemList[j].Month;
                                    ObjTemp.MonthStatus = ((PackageServices)sender).ServiceItemSource[i].PackageInPackageItemList[j].MonthStatus;
                                    ObjTemp.FromPackage = ((PackageServices)sender).ServiceItemSource[i].FromPackage;

                                    ObjTemp.ApplicableTo = ((PackageServices)sender).ServiceItemSource[i].PackageInPackageItemList[j].ApplicableTo;

                                    ObjTemp.Validity = ((PackageServices)sender).ServiceItemSource[i].PackageInPackageItemList[j].Validity;
                                    ObjTemp.ValidityUnit = ((PackageServices)sender).ServiceItemSource[i].PackageInPackageItemList[j].ValidityUnit;

                                    ObjTemp.ProcessID = ((PackageServices)sender).ServiceItemSource[i].PackageInPackageItemList[j].ProcessID;     // Package New Changes for Procedure Added on 17042018

                                    ServiceList.Add(ObjTemp);

                                    if (objPrevPackMasterNew != null && objPrevPackMasterNew.PackageDetails != null)
                                        objPrevPackMasterNew.PackageDetails.Add(ObjTemp);
                                }

                            }
                        }


                    }
                }

                if (((PackageServices)sender).SpecilizationItemSource.Count != 0)
                {
                    for (int i = 0; i < ((PackageServices)sender).SpecilizationItemSource.Count; i++)
                    {
                        //var item1 = from r in ServiceList
                        //            where (r.GroupID == (((PackageServices)sender).SpecilizationItemSource[i]).ID && r.DepartmentID == (((PackageServices)sender).ServiceItemSource[i]).Specialization
                        //            )
                        //            select new clsPackageServiceDetailsVO
                        //            {
                        //                GroupID = r.GroupID,
                        //                Status = r.Status

                        //            };

                        var item1 = from r in ServiceList
                                    where (r.ServiceID == (((PackageServices)sender).SpecilizationItemSource[i]).ID && r.DepartmentID == (((PackageServices)sender).SpecilizationItemSource[i]).ID
                                    && r.IsSpecilizationGroup == true
                                    && r.ProcessID == (((PackageServices)sender).SpecilizationItemSource[i]).FilterID  // use FilterID as ProcedureID  // Package New Changes for Procedure Added on 17042018
                                    )
                                    select new clsPackageServiceDetailsVO
                                    {
                                        GroupID = r.GroupID,
                                        Status = r.Status

                                    };

                        if (item1.ToList().Count == 0)
                        {
                            if ((((PackageServices)sender).SpecilizationItemSource[i]).FromPackage == false)
                            {
                                clsPackageServiceDetailsVO ObjTemp = new clsPackageServiceDetailsVO();
                                ObjTemp.GroupID = (((PackageServices)sender).SpecilizationItemSource[i]).ID;
                                ObjTemp.DepartmentID = (((PackageServices)sender).SpecilizationItemSource[i]).ID;
                                ObjTemp.ServiceID = (((PackageServices)sender).SpecilizationItemSource[i]).ID;
                                ObjTemp.ServiceName = (((PackageServices)sender).SpecilizationItemSource[i]).Description;
                                ObjTemp.Department = ((PackageServices)sender).SpecilizationItemSource[i].Description;
                                ObjTemp.IsSpecilizationGroup = true;
                                ObjTemp.ProcessID = (((PackageServices)sender).SpecilizationItemSource[i]).FilterID;  // use FilterID as ProcedureID // Package New Changes for Procedure Added on 17042018
                                ObjTemp.Quantity = 0;
                                ServiceList.Add(ObjTemp);

                                if (objPrevPackMasterNew != null && objPrevPackMasterNew.PackageDetails != null)
                                    objPrevPackMasterNew.PackageDetails.Add(ObjTemp);

                            }
                            else if ((((PackageServices)sender).SpecilizationItemSource[i]).FromPackage == true)
                            {
                                for (int j = 0; j < ((PackageServices)sender).SpecilizationItemSource[i].PackageInPackageItemList.Count; j++)
                                {
                                    clsPackageServiceDetailsVO ObjTemp = new clsPackageServiceDetailsVO();

                                    ObjTemp.ServiceID = (((PackageServices)sender).SpecilizationItemSource[i]).PackageInPackageItemList[j].ServiceID;
                                    ObjTemp.ServiceName = (((PackageServices)sender).SpecilizationItemSource[i]).PackageInPackageItemList[j].ServiceName;
                                    ObjTemp.ServiceCode = (((PackageServices)sender).SpecilizationItemSource[i]).PackageInPackageItemList[j].ServiceCode;
                                    ObjTemp.Rate = (double)((PackageServices)sender).SpecilizationItemSource[i].PackageInPackageItemList[j].Rate;
                                    ObjTemp.DepartmentID = (long)((PackageServices)sender).SpecilizationItemSource[i].PackageInPackageItemList[j].DepartmentID;
                                    ObjTemp.Department = ((PackageServices)sender).SpecilizationItemSource[i].PackageInPackageItemList[j].Department;
                                    ObjTemp.Quantity = ((PackageServices)sender).SpecilizationItemSource[i].PackageInPackageItemList[j].Quantity;

                                    ObjTemp.Amount = ((PackageServices)sender).SpecilizationItemSource[i].PackageInPackageItemList[j].Amount;
                                    ObjTemp.Discount = ((PackageServices)sender).SpecilizationItemSource[i].PackageInPackageItemList[j].Discount;
                                    ObjTemp.SelectedGender = ((PackageServices)sender).SpecilizationItemSource[i].PackageInPackageItemList[j].SelectedGender;
                                    ObjTemp.Infinite = ((PackageServices)sender).SpecilizationItemSource[i].PackageInPackageItemList[j].Infinite;
                                    ObjTemp.FreeAtFollowUp = ((PackageServices)sender).SpecilizationItemSource[i].PackageInPackageItemList[j].FreeAtFollowUp;
                                    ObjTemp.IsSpecilizationGroup = ((PackageServices)sender).SpecilizationItemSource[i].PackageInPackageItemList[j].IsSpecilizationGroup;
                                    ObjTemp.Month = ((PackageServices)sender).SpecilizationItemSource[i].PackageInPackageItemList[j].Month;
                                    ObjTemp.MonthStatus = ((PackageServices)sender).SpecilizationItemSource[i].PackageInPackageItemList[j].MonthStatus;
                                    ObjTemp.FromPackage = ((PackageServices)sender).SpecilizationItemSource[i].FromPackage;

                                    //Cnt = ((PackageServices)sender).SpecilizationItemSource[i].PackageInPackageItemList.Count(c => c.ServiceID == ((PackageServices)sender).SpecilizationItemSource[i].PackageInPackageItemList[j].ServiceID);

                                    ObjTemp.ApplicableTo = ((PackageServices)sender).SpecilizationItemSource[i].PackageInPackageItemList[j].ApplicableTo;

                                    ObjTemp.Validity = ((PackageServices)sender).SpecilizationItemSource[i].PackageInPackageItemList[j].Validity;
                                    ObjTemp.ValidityUnit = ((PackageServices)sender).SpecilizationItemSource[i].PackageInPackageItemList[j].ValidityUnit;

                                    // use FilterID as ProcedureID // Package New Changes for Procedure Added on 17042018
                                    ObjTemp.ProcessID = ((PackageServices)sender).SpecilizationItemSource[i].PackageInPackageItemList[j].ProcessID;

                                    ServiceList.Add(ObjTemp);

                                    if (objPrevPackMasterNew != null && objPrevPackMasterNew.PackageDetails != null)
                                        objPrevPackMasterNew.PackageDetails.Add(ObjTemp);

                                }

                            }
                        }

                    }
                }





                objPackMaster = ((clsPackageServiceVO)this.DataContext);
                objPackMaster.PackageDetails = ServiceList.ToList();


                if (objPrevPackMasterNew == null)
                    objPrevPackMasterNew = objPackMaster.DeepCopy();

                FillGridText(objPackMaster, false);


                //CreateGrid();


            }
        }


        void CreateGrid()
        {
            var rw = new RowDefinition { Height = new GridLength(25) };
            ServiceGrid.RowDefinitions.Add(rw);
            int C = 0;
            int R = 0;


            foreach (var item in ServiceList)
            {
                var col = new ColumnDefinition { Width = new GridLength() };
                var col2 = new ColumnDefinition { Width = new GridLength() };
                var col3 = new ColumnDefinition { Width = new GridLength() };

                ServiceGrid.ColumnDefinitions.Add(col);
                ServiceGrid.ColumnDefinitions.Add(col2);
                ServiceGrid.ColumnDefinitions.Add(col3);


                var myBlock = new TextBlock
                {
                    Text = item.ServiceName,
                    Margin = new Thickness(2),
                    DataContext = item,
                };

                var myBlock2 = new HyperlinkButton
                {
                    Content = "Define Rule",
                    Margin = new Thickness(2),
                    DataContext = item,
                };

                // Added BY  CDS For Package Freeze 
                //if (IsFreeze == true)
                //{
                //    myBlock2.IsEnabled = false;
                //}
                //else
                //{
                //    myBlock2.IsEnabled = true;
                //}

                myBlock2.Click += new RoutedEventHandler(myBlock2_Click);
                myBlock.SetValue(Grid.RowProperty, R);
                myBlock.SetValue(Grid.ColumnProperty, C);
                C = C + 1;

                myBlock2.SetValue(Grid.RowProperty, R);
                myBlock2.SetValue(Grid.ColumnProperty, C);

                ServiceGrid.Children.Add(myBlock);
                ServiceGrid.Children.Add(myBlock2);

                R = R + 1;

            }

        }

        void myBlock2_Click(object sender, RoutedEventArgs e)
        {
            frmPackageDefineRuleMaster Win = new frmPackageDefineRuleMaster();
            Win.OnAddButton_Click += new RoutedEventHandler(Win_OnAddButton_Click);
            //Win.ServiceListFromParent.Add(((List<clsPackageServiceDetailsVO>)(((HyperlinkButton)sender).DataContext)));

            ((List<clsPackageServiceDetailsVO>)(((HyperlinkButton)sender).DataContext)).ToList().DeepCopy().ForEach(Win.ServiceListFromParent.Add);

            Win.PackageServiceID = PackageServiceID;
            Win.PackageServiceUnitID = PackageServiceUnitID;
            Win.PackageID = PackageID;
            Win.IsFreeze = this.IsFreeze;

            if (txtValidity.Text.Trim() != String.Empty)
                Win.PackageDuration = Convert.ToInt64(txtValidity.Text);

            Win.objPackMasterCopy = this.objPackMaster.DeepCopy();     // Package New Changes for Procedure Added on 17042018

            Win.objPackMasterCopy.PackageDetails = new List<clsPackageServiceDetailsVO>();
            //Win.objPackMasterCopy.PackageDetails = this.objPackMaster.PackageDetails.Except(Win.ServiceListFromParent.ToList()).ToList();

            foreach (clsPackageServiceDetailsVO itemPD in this.objPackMaster.PackageDetails.ToList())
            {

                if (itemPD.ServiceID == Win.ServiceListFromParent[0].ServiceID && itemPD.DepartmentID == Win.ServiceListFromParent[0].DepartmentID
                    && itemPD.IsSpecilizationGroup == Win.ServiceListFromParent[0].IsSpecilizationGroup && itemPD.ProcessID == Win.ServiceListFromParent[0].ProcessID && itemPD.ID > 0)
                {
                    // Nothing
                }
                else
                {
                    if (itemPD.ID > 0)
                    {
                        Win.objPackMasterCopy.PackageDetails.Add(itemPD.DeepCopy());
                    }
                }
            }

            Win.Show();

        }

        WaitIndicator Indicatior = null;

        private bool ChkValidation()
        {
            bool result = true;

            if ((MasterListItem)cmbService.SelectedItem == null)
            {

                cmbService.TextBox.SetValidation("Please Select Package Service");
                cmbService.TextBox.RaiseValidationError();
                cmbService.Focus();
                result = false;


            }
            else if (((MasterListItem)cmbService.SelectedItem).ID == 0)
            {
                cmbService.TextBox.SetValidation("Please Select Package Service");
                cmbService.TextBox.RaiseValidationError();
                cmbService.Focus();
                result = false;

            }
            else
                cmbService.TextBox.ClearValidationError();

            if (txtValidity.Text == "0" || txtValidity.Text == "")
            {
                txtValidity.SetValidation("Package Duration can not be zero");
                txtValidity.RaiseValidationError();
                txtValidity.Focus();
                result = false;
                return result;
            }
            else
                txtValidity.ClearValidationError();

            if ((MasterListItem)cmbValidity.SelectedItem == null)
            {

                cmbValidity.TextBox.SetValidation("Please Select Duration Unit");
                cmbValidity.TextBox.RaiseValidationError();
                cmbValidity.Focus();
                result = false;


            }
            else if (((MasterListItem)cmbValidity.SelectedItem).ID == 0)
            {
                cmbValidity.TextBox.SetValidation("Please Select Duration Unit");
                cmbValidity.TextBox.RaiseValidationError();
                cmbValidity.Focus();
                result = false;

            }
            else
                cmbValidity.TextBox.ClearValidationError();


            if (BizActionSave.Details.PackageServiceRelationDetails == null)  //dgCalender.ItemsSource
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "You Can not save without service list.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                //dgCalender.Focus();
                result = false;
                return result;

            }

            if (BizActionSave.Details.PackageServiceRelationDetails != null)  //dgCalender.ItemsSource
            {
                if (BizActionSave.Details.PackageServiceRelationDetails.Count == 0)  //((ObservableCollection<PackageList>)dgCalender.ItemsSource).Count
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", "You Can not save without service list.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                    //dgCalender.Focus();
                    result = false;
                    return result;

                }
            }

            if (txtValidity.Text == "0" || txtValidity.Text == "")
            {
                txtValidity.SetValidation("Package Duration can not be zero");
                txtValidity.RaiseValidationError();
                txtValidity.Focus();
                result = false;
            }
            else
                txtValidity.ClearValidationError();

            #region Commented Package New Changes for Procedure Added on 19042018

            //if (chkSelectAll.IsChecked == true && (txtDiscountOnAllItems.Text == "0" || txtDiscountOnAllItems.Text == ""))
            //{
            //    txtDiscountOnAllItems.SetValidation("Please Enter Discount");
            //    txtDiscountOnAllItems.RaiseValidationError();
            //    txtDiscountOnAllItems.Focus();
            //    result = false;
            //}
            ////Added By Bhushanp

            #endregion


            double TAmount = Convert.ToDouble(txtServiceFixedRate.Text) + Convert.ToDouble(txtPharmacyFixedRate.Text);
            if (TAmount > Convert.ToDouble(txtAmount.Text))
            {
                txtServiceFixedRate.SetValidation("Please Enter Discount");
                txtServiceFixedRate.RaiseValidationError();
                txtServiceFixedRate.Focus();
                result = false;
            }


            return result;
        }
        WaitIndicator wait = new WaitIndicator();
        clsAddPackageServiceNewBizActionVO BizActionSave;

        void Win_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            wait.Show();
            try
            {
                BizActionSave = new clsAddPackageServiceNewBizActionVO();

                BizActionSave.Details = new clsPackageServiceVO();

                BizActionSave.Details.PackageDetails = ((frmPackageDefineRuleMaster)sender).ServiceListFromParent.ToList();   //lstPackage;

                BizActionSave.Details.PackageServiceRelationDetails = ((frmPackageDefineRuleMaster)sender).PackageServiceRelationList;
                BizActionSave.Details.PackageServiceRelationDetailsDelete = ((frmPackageDefineRuleMaster)sender).PackageServiceRelationListDelete;

                if (((frmPackageDefineRuleMaster)sender).ConditionANDOR != null & ((frmPackageDefineRuleMaster)sender).ConditionANDOR.Count > 0 && ((frmPackageDefineRuleMaster)sender).ConditionANDOR[0].SelectedService != null && ((frmPackageDefineRuleMaster)sender).ConditionANDOR[0].SelectedService.ID != 0)
                    BizActionSave.Details.ServiceConditionDetails = ((frmPackageDefineRuleMaster)sender).ConditionANDOR.ToList();
                BizActionSave.Details.ServiceConditionDetailsDelete = ((frmPackageDefineRuleMaster)sender).ConditionANDORDelete.ToList();


                //if (rdbFixedNew.IsChecked == true)
                //    BizActionSave.IsFixedRate = true;
                //else
                //    BizActionSave.IsFixedRate = false;

                //if (rdbPercentage.IsChecked == true)
                //    BizActionSave.IsFixedRate = false;
                //else
                //    BizActionSave.IsFixedRate = true;

                //if (!string.IsNullOrEmpty(txtServiceFixedRate.Text.Trim()))
                //    BizActionSave.ServiceFixedRate = float.Parse(txtServiceFixedRate.Text.Trim());

                //if (!string.IsNullOrEmpty(txtPharmacyFixedRate.Text.Trim()))
                //    BizActionSave.PharmacyFixedRate = float.Parse(txtPharmacyFixedRate.Text.Trim());

                //if (!string.IsNullOrEmpty(txtServicePercentage.Text.Trim()))
                //    BizActionSave.ServicePercentage = float.Parse(txtServicePercentage.Text.Trim());

                //if (!string.IsNullOrEmpty(txtPharmacyPercentage.Text.Trim()))
                //    BizActionSave.PharmacyPercentage = float.Parse(txtPharmacyPercentage.Text.Trim());

                if (rdbFixedNew.IsChecked == true)
                    BizActionSave.Details.IsFixedRate = true;
                else
                    BizActionSave.Details.IsFixedRate = false;

                if (rdbPercentage.IsChecked == true)
                    BizActionSave.Details.IsFixedRate = false;
                else
                    BizActionSave.Details.IsFixedRate = true;

                if (!string.IsNullOrEmpty(txtServiceFixedRate.Text.Trim()))
                    BizActionSave.Details.ServiceFixedRate = float.Parse(txtServiceFixedRate.Text.Trim());

                if (!string.IsNullOrEmpty(txtPharmacyFixedRate.Text.Trim()))
                    BizActionSave.Details.PharmacyFixedRate = float.Parse(txtPharmacyFixedRate.Text.Trim());

                if (!string.IsNullOrEmpty(txtServicePercentage.Text.Trim()))
                    BizActionSave.Details.ServicePercentage = Math.Round(float.Parse(txtServicePercentage.Text.Trim()), 2);
                //BizActionSave.Details.ServicePercentage = float.Parse(txtServicePercentage.Text.Trim());

                if (!string.IsNullOrEmpty(txtPharmacyPercentage.Text.Trim()))
                    BizActionSave.Details.PharmacyPercentage = Math.Round(float.Parse(txtPharmacyPercentage.Text.Trim()), 2);
                //BizActionSave.Details.PharmacyPercentage = float.Parse(txtPharmacyPercentage.Text.Trim());


                if (ChkValidation())
                {
                    msgText = "Are you sure you want to save the Package Details ?";

                    bool IsSavePatientData = false;

                    //MessageBoxControl.MessageBoxChildWindow msgW =
                    //    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    //msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                    //msgW.Show();

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {

                        ClickedFlag += 1;

                        if (ClickedFlag == 1)
                        {
                            if (res == MessageBoxResult.Yes)
                            {

                                if ((Convert.ToBoolean(chkFreezePackage.IsChecked)) == true)
                                {
                                    //msgTitle = "Palash";
                                    msgText = "Are you sure you want to Save the selected Service/Group for already Registered Patient ?";

                                    MessageBoxControl.MessageBoxChildWindow msgWD2 =
                                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);


                                    msgWD2.OnMessageBoxClosed += (MessageBoxResult res2) =>
                                    {
                                        if (res2 == MessageBoxResult.Yes)
                                        {
                                            IsSavePatientData = true;
                                        }
                                        else
                                        {
                                            IsSavePatientData = false;
                                        }

                                        msgW_OnMessageBoxClosed(IsSavePatientData);

                                    };
                                    msgWD2.Show();

                                }
                                else
                                {
                                    msgW_OnMessageBoxClosed(false);
                                }

                                wait.Close();
                            }
                            else
                            {
                                ClickedFlag = 0;
                                wait.Close();
                            }
                        }
                        else
                        {
                            msgText = "Please Wait, Request To Save Package Detils Is Been Already Processed ?";

                            MessageBoxControl.MessageBoxChildWindow msgWD1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                            msgWD1.Show();

                            wait.Close();
                        }

                    };
                    msgWD.Show();

                }
            }
            catch (Exception es)
            {
                wait.Close();
            }
            //finally
            //{ wait.Close(); }
        }

        private bool CheckDuplicasy()
        {
            bool IsPackageExist = false;

            if (IsNew)
            {
                if (((MasterListItem)cmbService.SelectedItem).Value > 0)
                {
                    IsPackageExist = true;
                }
                else
                {
                    IsPackageExist = false;
                }

                if (IsPackageExist == true)
                {
                    msgText = "Record cannot be save as Package already exist!";

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                    return false;
                }
                else
                {
                    return true;
                }

            }
            else
            {
                return true;
            }

        }

        void msgW_OnMessageBoxClosed(bool IsSavePatientData)  //MessageBoxResult result
        {
            //ClickedFlag += 1;

            //if (ClickedFlag == 1)
            //{
            //    if (result == MessageBoxResult.Yes)
            if (CheckDuplicasy())
            {
                Save(IsSavePatientData);  //Save();
            }
            else
            {
                ClickedFlag = 0;
            }
            //}
            //else
            //{
            //    string msgText = "Please Wait, Request To Save Package Is Been Already Processed ?";

            //    MessageBoxControl.MessageBoxChildWindow msgWD =
            //        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
            //    msgWD.Show();
            //}


        }

        # region Comment msgW_OnMessageBoxClosed
        //void msgW_OnMessageBoxClosed(MessageBoxResult result)
        //{
        //    ClickedFlag += 1;

        //    if (ClickedFlag == 1)
        //    {
        //        if (result == MessageBoxResult.Yes)
        //            if (CheckDuplicasy())
        //            {
        //                Save();
        //            }

        //            else
        //            {
        //                ClickedFlag = 0;
        //            }
        //    }
        //    else
        //    {
        //        string msgText = "Please Wait, Request To Save Package Is Been Already Processed ?";

        //        MessageBoxControl.MessageBoxChildWindow msgWD =
        //            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
        //        msgWD.Show();
        //    }


        //}
        # endregion

        private void Save(bool IsSavePatientData)  // private void Save()
        {
            clsAddPackageServiceNewBizActionVO BizAction = new clsAddPackageServiceNewBizActionVO();


            try
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();

                BizAction.Details = (clsPackageServiceVO)this.DataContext;

                if (cmbService.SelectedItem != null)
                    BizAction.Details.ServiceID = ((MasterListItem)cmbService.SelectedItem).ID;

                if (cmbValidity.SelectedItem != null)
                    BizAction.Details.ValidityUnit = ((MasterListItem)cmbValidity.SelectedItem).ID;

                BizAction.Details.ApplicableToAll = Convert.ToBoolean(chkSelectAll.IsChecked);
                BizAction.Details.ApplicableToAllDiscount = ((clsPackageServiceVO)this.DataContext).ApplicableToAllDiscount;

                BizAction.Details.IsFreezed = Convert.ToBoolean(chkFreezePackage.IsChecked);

                //if (rdbFixedNew.IsChecked == true)
                //    BizActionSave.IsFixedRate = true;
                //else
                //    BizActionSave.IsFixedRate = false;

                //if (rdbPercentage.IsChecked == true)
                //    BizActionSave.IsFixedRate = false;
                //else
                //    BizActionSave.IsFixedRate = true;

                //if (!string.IsNullOrEmpty(txtServiceFixedRate.Text.Trim()))
                //    BizActionSave.ServiceFixedRate = float.Parse(txtServiceFixedRate.Text.Trim());

                //if (!string.IsNullOrEmpty(txtPharmacyFixedRate.Text.Trim()))
                //    BizActionSave.PharmacyFixedRate = float.Parse(txtPharmacyFixedRate.Text.Trim());

                //if (!string.IsNullOrEmpty(txtServicePercentage.Text.Trim()))
                //    BizActionSave.ServicePercentage = float.Parse(txtServicePercentage.Text.Trim());

                //if (!string.IsNullOrEmpty(txtPharmacyPercentage.Text.Trim()))
                //    BizActionSave.PharmacyPercentage = float.Parse(txtPharmacyPercentage.Text.Trim());

                if (rdbFixedNew.IsChecked == true)
                {
                    BizAction.Details.IsFixedRate = true;

                    txtServicePercentage.Text = "0";
                    txtPharmacyPercentage.Text = "0";
                }
                else
                {
                    BizAction.Details.IsFixedRate = false;

                    txtServiceFixedRate.Text = "0";
                    txtPharmacyFixedRate.Text = "0";
                }

                if (rdbPercentage.IsChecked == true)
                {
                    BizAction.Details.IsFixedRate = false;

                    txtServiceFixedRate.Text = "0";
                    txtPharmacyFixedRate.Text = "0";
                }
                else
                {
                    BizAction.Details.IsFixedRate = true;

                    txtServicePercentage.Text = "0";
                    txtPharmacyPercentage.Text = "0";
                }

                if (!string.IsNullOrEmpty(txtServiceFixedRate.Text.Trim()))
                    BizAction.Details.ServiceFixedRate = float.Parse(txtServiceFixedRate.Text.Trim());

                if (!string.IsNullOrEmpty(txtPharmacyFixedRate.Text.Trim()))
                    BizAction.Details.PharmacyFixedRate = float.Parse(txtPharmacyFixedRate.Text.Trim());

                // For Package New Changes Commented on 02052018
                //if (!string.IsNullOrEmpty(txtServicePercentage.Text.Trim()))
                //    BizAction.Details.ServicePercentage = float.Parse(txtServicePercentage.Text.Trim());

                if (!string.IsNullOrEmpty(txtServicePercentage.Text.Trim()))
                    BizAction.Details.ServicePercentage = Math.Round(float.Parse(txtServicePercentage.Text.Trim()), 2);      // For Package New Changes modified on 02052018

                // For Package New Changes Commented on 02052018
                //if (!string.IsNullOrEmpty(txtPharmacyPercentage.Text.Trim()))
                //    BizAction.Details.PharmacyPercentage = float.Parse(txtPharmacyPercentage.Text.Trim());

                if (!string.IsNullOrEmpty(txtPharmacyPercentage.Text.Trim()))
                    BizAction.Details.PharmacyPercentage = Math.Round(float.Parse(txtPharmacyPercentage.Text.Trim()), 2);     // For Package New Changes modified on 02052018

                ////BizAction.Details.PackageDetails = ServiceList.ToList();

                //ObservableCollection<PackageList> PackageServiceList = new ObservableCollection<PackageList>();
                //PackageServiceList = ((ObservableCollection<PackageList>)dgCalender.ItemsSource);

                //bool FreeAtFollowup = false;

                //foreach (var item2 in PackageServiceList)   //(var item2 in ServiceList)
                //{
                //    foreach (var item in dgCalender.Columns)  //(var item in item2.Months)
                //    {
                //        if (item.Header.ToString() == "Free At FollowUp")
                //        {
                //            FreeAtFollowup = ((CheckBox)item.GetCellContent(item2)).IsChecked.Value;
                //        }

                //        if (item.Header.ToString() != "Service Name" && item.Header.ToString() != "Rate" && item.Header.ToString() != "Amount" && item.Header.ToString() != "Discount" && item.Header.ToString() != "Applicable To" && item.Header.ToString() != "Unlimited Quantity" && item.Header.ToString() != "Quantity" && item.Header.ToString() != "Free At FollowUp" && item.Header.ToString() != "Delete")
                //        {
                //            clsPackageServiceDetailsVO obj = new clsPackageServiceDetailsVO();
                //            obj.Month = item.Header.ToString(); //item.Month;
                //            obj.DepartmentID = item2.DepartmentID;
                //            obj.ServiceID = item2.ServiceID;
                //            obj.Rate = item2.Rate;
                //            obj.Amount = item2.Amount;
                //            obj.Discount = item2.Discount;
                //            obj.SelectedGender = item2.SelectedGender;
                //            obj.Infinite = item2.Infinite;
                //            obj.Quantity = item2.Quantity;
                //            obj.FreeAtFollowUp = FreeAtFollowup;  // item2.FreeAtFollowUp;
                //            obj.IsSpecilizationGroup = item2.IsSpecilizationGroup;
                //            obj.MonthStatus = ((CheckBox)item.GetCellContent(item2)).IsChecked.Value;  //item.MonthStatus; 
                //            lstPackage.Add(obj);
                //        }
                //    }
                //}

                //BizAction.Details = BizActionSave.Details;

                BizAction.Details.PackageDetails = BizActionSave.Details.PackageDetails;  //  ((frmPackageDefineRuleMaster)sender).ServiceListFromParent.ToList();   //lstPackage;
                //BizAction.Details.ItemDetails = ItemList1.ToList();

                BizAction.Details.PackageServiceRelationDetails = BizActionSave.Details.PackageServiceRelationDetails;  //((frmPackageDefineRuleMaster)sender).PackageServiceRelationList;
                BizAction.Details.PackageServiceRelationDetailsDelete = BizActionSave.Details.PackageServiceRelationDetailsDelete;

                BizAction.Details.ServiceConditionDetails = BizActionSave.Details.ServiceConditionDetails;    //((frmPackageDefineRuleMaster)sender).ConditionANDOR.ToList();
                BizAction.Details.ServiceConditionDetailsDelete = BizActionSave.Details.ServiceConditionDetailsDelete;

                BizAction.IsSavePatientData = IsSavePatientData;  //Set to check whether Add Patient Package Details after Package Freezed for particular Service for already Registered Patient or not

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    SetCommandButtonState("Save");
                    if (arg.Error == null)
                    {
                        if (((clsAddPackageServiceNewBizActionVO)arg.Result).Details != null)
                        {
                            //ClearData();
                            //FetchData();

                            //List<clsPackageServiceDetailsVO> objPackServlist = new List<clsPackageServiceDetailsVO>();
                            //objPackServlist = BizActionSave.Details.PackageDetails;    //((frmPackageDefineRuleMaster)sender).ServiceListFromParent.ToList();

                            clsPackageServiceVO objPackMaster = new clsPackageServiceVO();
                            objPackMaster = ((clsAddPackageServiceNewBizActionVO)arg.Result).Details;
                            objPackMaster.PackageDetails = ((clsAddPackageServiceNewBizActionVO)arg.Result).Details.PackageDetails;

                            PackageID = objPackMaster.ID;

                            if (PackageID > 0)
                                GetPackageDetails(PackageID);

                            //FillGridText(objPackMaster);  //objPackServlist

                            //objAnimation.Invoke(RotationType.Backward);
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Package Service/Group added successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosedFlag);
                            msgW1.Show();

                            ClickedFlag = 0;
                        }
                        Indicatior.Close();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosedFlag);

                        msgW1.Show();

                        ClickedFlag = 0;
                    }
                    Indicatior.Close();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
                Indicatior.Close();
            }
            //finally
            //{
            //    Indicatior.Close();

            //}
        }

        int ClickedFlag = 0;

        void msgW_OnMessageBoxClosedFlag(MessageBoxResult result)
        {
            ClickedFlag = 0;
            //clickflg = 0;
        }

        private void FillGridText(clsPackageServiceVO objPackMasterNew, bool IsCallFromSave) //List<clsPackageServiceDetailsVO> objPackServlist
        {
            //clsPackageServiceVO objPrevPackMasterNew = new clsPackageServiceVO();
            //if (IsCallFromSave == false)
            //{
            //objPrevPackMasterNew = objPackMasterNew.DeepCopy();

            if (objPrevPackMasterNew != null && objPrevPackMasterNew.PackageDetails != null)
            {
                foreach (clsPackageServiceDetailsVO itemPrevPackServ in objPrevPackMasterNew.PackageDetails)  //previous list
                {

                    var item1 = from r in objPackMasterNew.PackageDetails
                                where (r.ServiceID == itemPrevPackServ.ServiceID && r.DepartmentID == itemPrevPackServ.DepartmentID
                                && r.IsSpecilizationGroup == itemPrevPackServ.IsSpecilizationGroup
                                && r.ProcessID == itemPrevPackServ.ProcessID    // Package New Changes for Procedure Added on 17042018
                                )
                                select r;

                    //if (item1.ToList().Count == 0)
                    if (item1.ToList().Count == 0 && itemPrevPackServ.ProcessID > 0)      // Package New Changes for Procedure Added on 18042018      
                    {
                        objPackMasterNew.PackageDetails.Add(itemPrevPackServ);
                    }

                }
            }

            //}

            ServiceGrid.Children.Clear();

            int C = 2;
            int R = -1;  //int R = 0;

            long SerId = 0;
            long SerCnt = 0;
            long SpecializationId = 0;
            bool IsSpecialization = false;
            long ProcID = 0;

            //var grd = new Grid();--

            //clsPackageServiceDetailsVO itemPackServ = new clsPackageServiceDetailsVO();

            //if (objPrevPackMasterNew != null)
            //{
            //foreach (clsPackageServiceDetailsVO itemPrevPackServ in objPrevPackMasterNew.PackageDetails)  //objPackServlist
            //{
            foreach (clsPackageServiceDetailsVO itemPackServ in objPackMasterNew.PackageDetails)  //objPackServlist
            {
                //if (objPrevPackMasterNew.ServiceID == objPackMasterNew.ServiceID && itemPrevPackServ.ServiceID == itemPackServ2.ServiceID && itemPrevPackServ.DepartmentID == itemPackServ2.DepartmentID && itemPrevPackServ.IsSpecilizationGroup == itemPackServ2.IsSpecilizationGroup)
                //{
                //    itemPackServ = itemPackServ2;
                //}
                //else if (objPrevPackMasterNew.ServiceID == objPackMasterNew.ServiceID)
                //{
                //    itemPackServ = itemPrevPackServ;
                //}

                //if (objPrevPackMasterNew.ServiceID == objPackMasterNew.ServiceID && itemPrevPackServ.ServiceID == itemPackServ.ServiceID && itemPrevPackServ.DepartmentID == itemPackServ.DepartmentID && itemPrevPackServ.IsSpecilizationGroup == itemPackServ.IsSpecilizationGroup)
                //{

                //var col = new ColumnDefinition { Width = new GridLength() };
                //var col2 = new ColumnDefinition { Width = new GridLength() };
                //var col3 = new ColumnDefinition { Width = new GridLength() };
                //var col4 = new ColumnDefinition { Width = new GridLength() };
                //var col5 = new ColumnDefinition { Width = new GridLength() };

                //ServiceGrid.ColumnDefinitions.Add(col.DeepCopy());
                //ServiceGrid.ColumnDefinitions.Add(col2);
                //ServiceGrid.ColumnDefinitions.Add(col3);
                //ServiceGrid.ColumnDefinitions.Add(col4);
                //ServiceGrid.ColumnDefinitions.Add(col5);



                //if (SpecializationId != itemPackServ.DepartmentID || SerId != itemPackServ.ServiceID) //SerId == 0 || //&& IsSpecialization != itemPackServ.IsSpecilizationGroup
                if (SpecializationId != itemPackServ.DepartmentID || SerId != itemPackServ.ServiceID || ProcID != itemPackServ.ProcessID)     // Package New Changes for Procedure Added on 17042018
                {
                    //if (SpecializationId != itemPackServ.DepartmentID || SerId != itemPackServ.ServiceID)  //if (R > 0)  //&& IsSpecialization != itemPackServ.IsSpecilizationGroup
                    if (SpecializationId != itemPackServ.DepartmentID || SerId != itemPackServ.ServiceID || ProcID != itemPackServ.ProcessID)
                        R = R + 1;

                    SerId = itemPackServ.ServiceID;
                    SpecializationId = itemPackServ.DepartmentID;
                    IsSpecialization = itemPackServ.IsSpecilizationGroup;
                    ProcID = itemPackServ.ProcessID;      // Package New Changes for Procedure Added on 17042018

                    //SerCnt = objPackMasterNew.PackageDetails.Count(c => c.ServiceID == SerId && c.DepartmentID == itemPackServ.DepartmentID && c.IsSpecilizationGroup == itemPackServ.IsSpecilizationGroup);
                    SerCnt = objPackMasterNew.PackageDetails.Count(c => c.ServiceID == SerId && c.DepartmentID == itemPackServ.DepartmentID && c.IsSpecilizationGroup == itemPackServ.IsSpecilizationGroup
                        && c.ProcessID == ProcID);    // Package New Changes for Procedure Added on 17042018

                    var rw = new RowDefinition { Height = new GridLength(25) };
                    ServiceGrid.RowDefinitions.Add(rw);

                    var col = new ColumnDefinition { Width = new GridLength() };//--
                    ServiceGrid.ColumnDefinitions.Add(col);//--
                    var col1 = new ColumnDefinition { Width = new GridLength() };//--
                    ServiceGrid.ColumnDefinitions.Add(col1);//--

                    SolidColorBrush brushPaint1 = new SolidColorBrush(Color.FromArgb(255, 112, 128, 144));     // (255, 255, 69, 0)

                    var myBlockService = new TextBlock
                    {
                        Text = itemPackServ.ServiceName,
                        Margin = new Thickness(2),
                        Foreground = brushPaint1,
                        DataContext = itemPackServ,
                    };

                    myBlockService.SetValue(Grid.RowProperty, R);
                    myBlockService.SetValue(Grid.ColumnProperty, 0);

                    ServiceGrid.Children.Add(myBlockService);

                    List<clsPackageServiceDetailsVO> lstPackageDetils = new List<clsPackageServiceDetailsVO>();
                    //lstPackageDetils = objPackMasterNew.PackageDetails.Where(objPackServ => objPackServ.ServiceID == itemPackServ.ServiceID && objPackServ.DepartmentID == itemPackServ.DepartmentID && objPackServ.IsSpecilizationGroup == itemPackServ.IsSpecilizationGroup).ToList();
                    lstPackageDetils = objPackMasterNew.PackageDetails.Where(objPackServ => objPackServ.ServiceID == itemPackServ.ServiceID && objPackServ.DepartmentID == itemPackServ.DepartmentID && objPackServ.IsSpecilizationGroup == itemPackServ.IsSpecilizationGroup
                         && objPackServ.ProcessID == itemPackServ.ProcessID).ToList();      // Package New Changes for Procedure Added on 17042018

                    var myBlockServiceLabel = new HyperlinkButton
                    {
                        Content = "Define Rule",
                        Margin = new Thickness(2),
                        DataContext = lstPackageDetils, //objPackMasterNew
                    };

                    // Added BY  CDS For Package Freeze 
                    //if (IsFreeze == true)
                    //{                        
                    //    myBlockServiceLabel.IsEnabled = false;
                    //}
                    //else
                    //{
                    //    myBlockServiceLabel.IsEnabled = true;
                    //}

                    myBlockServiceLabel.Click += new RoutedEventHandler(myBlock2_Click);

                    //C = C + 1;

                    myBlockServiceLabel.SetValue(Grid.RowProperty, R);
                    myBlockServiceLabel.SetValue(Grid.ColumnProperty, 1);


                    ServiceGrid.Children.Add(myBlockServiceLabel);

                    if (SerCnt > 0 && ((Convert.ToInt64(itemPackServ.DisplayQuantity) > 0) || (Convert.ToBoolean(itemPackServ.AdjustableHead) == true)))  //if (SerCnt > 1)  //if (SerCnt > 1)   IsCallFromSave==true
                    {
                        string DefineRate = string.Empty;

                        if (itemPackServ.AdjustableHead == true)
                        {
                            DefineRate = "Adjustable Head";

                            //Package New Changes for Procedure Added on 18042018
                            if (itemPackServ.AdjustableHeadType == 1)    // 1 = Clinical
                            {
                                DefineRate = DefineRate + " (S)";
                            }
                            else if (itemPackServ.AdjustableHeadType == 2)    // 2 = Pharmacy
                            {
                                DefineRate = DefineRate + " (P)";
                            }
                        }
                        else if (itemPackServ.IsFixed == true)
                        {
                            DefineRate = itemPackServ.Rate.ToString();
                        }
                        else if (itemPackServ.IsFixed == false)
                        {
                            if (itemPackServ.IsDoctorSharePercentage == true)
                            {
                                DefineRate = "Doctor Share";
                            }
                            else
                            {
                                DefineRate = itemPackServ.RatePercentage.ToString() + "%";
                            }
                        }

                        var myBlock = new TextBlock
                        {

                            //Text = "| Rate  - " + itemPackServ.Rate.ToString(),  //"| Rate  -500 |",
                            //Margin = new Thickness(2),
                            Text = "| Rate  - " + DefineRate.ToString(),  //"| Rate  -500 |",
                            Margin = new Thickness(2),

                        };
                        //By Anjali/CDS
                        //var myBlock2 = new TextBlock
                        //{
                        //    Text = "| Quantity - " + itemPackServ.Quantity.ToString(),  // + ((frmPackageDefineRuleMaster)sender).txtQuantity.Text + " | ",
                        //    Margin = new Thickness(2),

                        //};
                        //string VarQty = "";
                        //if (itemPackServ.IsSpecilizationGroup == true && itemPackServ.Infinite==true)
                        //{
                        //    VarQty = "0";
                        //}
                        //else if (itemPackServ.IsSpecilizationGroup == false && itemPackServ.Infinite == true)
                        //{

                        //    VarQty = itemPackServ.Month.ToString();
                        //}
                        //else
                        //{

                        //    VarQty = itemPackServ.Quantity.ToString();

                        //}
                        var myBlock2 = new TextBlock
                        {
                            Text = "| Quantity - " + itemPackServ.DisplayQuantity,  // + ((frmPackageDefineRuleMaster)sender).txtQuantity.Text + " | ",
                            Margin = new Thickness(2),

                        };
                        //.........................................
                        //GenderList 
                        MasterListItem objGender = new MasterListItem();

                        var result = from r in (GenderList)
                                     where r.ID == itemPackServ.ApplicableTo
                                     select r;

                        if (result != null)
                        {
                            objGender = ((MasterListItem)result.FirstOrDefault());
                        }

                        var myBlock3 = new TextBlock
                        {
                            Text = "| Gender - " + objGender.Description,  // "Gender -Male |"
                            Margin = new Thickness(2),

                        };

                        var myBlock4 = new TextBlock
                        {
                            Text = "| Discount - " + itemPackServ.Discount.ToString() + "% ",  //"Discount - 30% |"
                            Margin = new Thickness(2),

                        };

                        var myBlock5 = new HyperlinkButton
                        {
                            Content = " | x",
                            Margin = new Thickness(2),
                            DataContext = lstPackageDetils,
                        };

                        myBlock5.Click += new RoutedEventHandler(myBlock5_Click);

                        var col3 = new ColumnDefinition { Width = new GridLength() };//--
                        ServiceGrid.ColumnDefinitions.Add(col3);

                        var col4 = new ColumnDefinition { Width = new GridLength() };//--
                        ServiceGrid.ColumnDefinitions.Add(col4);

                        var col5 = new ColumnDefinition { Width = new GridLength() };//--
                        ServiceGrid.ColumnDefinitions.Add(col5);

                        myBlock.SetValue(Grid.RowProperty, R);
                        myBlock.SetValue(Grid.ColumnProperty, 2);  //3
                        //C = C + 1;

                        myBlock2.SetValue(Grid.RowProperty, R);
                        myBlock2.SetValue(Grid.ColumnProperty, 3);  //4
                        //C = C + 1;

                        myBlock3.SetValue(Grid.RowProperty, R);
                        myBlock3.SetValue(Grid.ColumnProperty, 4);   //5
                        //C = C + 1;


                        myBlock4.SetValue(Grid.RowProperty, R);
                        myBlock4.SetValue(Grid.ColumnProperty, 5);   //6
                        //C = C + 1;

                        myBlock5.SetValue(Grid.RowProperty, R);
                        myBlock5.SetValue(Grid.ColumnProperty, 6);   //7

                        ServiceGrid.Children.Add(myBlock);
                        ServiceGrid.Children.Add(myBlock2);
                        ServiceGrid.Children.Add(myBlock3);
                        ServiceGrid.Children.Add(myBlock4);
                        ServiceGrid.Children.Add(myBlock5);

                        #region Package New Changes for Procedure Added on 16042018

                        MasterListItem objProcedure = new MasterListItem();

                        var result2 = from r in (ProcedureList)
                                      where r.ID == itemPackServ.ProcessID
                                      select r;

                        if (result2 != null)
                        {
                            objProcedure = ((MasterListItem)result2.FirstOrDefault());
                        }

                        SolidColorBrush brushPaint = new SolidColorBrush(Color.FromArgb(255, 199, 21, 133));

                        var myBlock5A = new TextBlock
                        {
                            Text = "Procedure : " + objProcedure.Description,
                            Margin = new Thickness(2),
                            //Foreground = brushPaint,
                        };

                        var rw2 = new RowDefinition { Height = new GridLength(25) };
                        ServiceGrid.RowDefinitions.Add(rw2);

                        R = R + 1;

                        myBlock5A.SetValue(Grid.RowProperty, R);
                        myBlock5A.SetValue(Grid.ColumnProperty, 0);

                        ServiceGrid.Children.Add(myBlock5A);//--

                        #endregion

                        var myBlock6 = new TextBlock
                        {
                            Text = "Schedule -",
                            Margin = new Thickness(2),

                        };

                        //var rw2 = new RowDefinition { Height = new GridLength(25) };//--
                        //grd = new Grid();//--
                        //grd.RowDefinitions.Add(rw2);//--

                        var rw1 = new RowDefinition { Height = new GridLength(25) };
                        ServiceGrid.RowDefinitions.Add(rw1);

                        R = R + 1;

                        myBlock6.SetValue(Grid.RowProperty, R);
                        myBlock6.SetValue(Grid.ColumnProperty, 0);

                        ServiceGrid.Children.Add(myBlock6);//--
                        //grd.Children.Add(myBlock6);//--

                        //grd.SetValue(Grid.RowProperty, R);//--
                        //grd.SetValue(Grid.ColumnProperty, 0);//--

                        //ServiceGrid.Children.Add(grd);//--

                        C = 1;
                    }


                }
                else
                {
                    //if (SpecializationId != itemPackServ.DepartmentID || SerId != itemPackServ.ServiceID)
                    if (SpecializationId != itemPackServ.DepartmentID || SerId != itemPackServ.ServiceID || ProcID != itemPackServ.ProcessID)     // Package New Changes for Procedure Added on 17042018
                    {

                        SerId = itemPackServ.ServiceID;
                        C = 1;
                        SerCnt = 0;
                        R = R + 1;

                        //SerId = itemPackServ.ServiceID;
                        SpecializationId = itemPackServ.DepartmentID;
                        IsSpecialization = itemPackServ.IsSpecilizationGroup;
                        ProcID = itemPackServ.ProcessID;  // Package New Changes for Procedure Added on 17042018
                    }
                }


                if (SerCnt > 0 && Convert.ToInt64(itemPackServ.DisplayQuantity) > 0) // if (SerCnt > 1)   //if (SerCnt > 1)        IsCallFromSave==true 
                {
                    var col2 = new ColumnDefinition { Width = new GridLength() };
                    ServiceGrid.ColumnDefinitions.Add(col2);


                    ////if (SerId != itemPackServ.ServiceID) //SerId == 0 ||
                    ////{
                    ////    var myBlock6 = new TextBlock
                    ////    {
                    ////        Text = "|Schedule -",
                    ////        Margin = new Thickness(2),

                    ////    };

                    ////    myBlock6.SetValue(Grid.RowProperty, R);
                    ////    myBlock6.SetValue(Grid.ColumnProperty, 0);

                    ////    ServiceGrid.Children.Add(myBlock6);
                    ////}

                    var myBlock7 = new CheckBox
                    {
                        IsChecked = itemPackServ.MonthStatus,   //true
                        Margin = new Thickness(2),
                        Content = itemPackServ.Month,
                        IsEnabled = false,
                    };

                    ////var myBlock8 = new CheckBox
                    ////{
                    ////    IsChecked = true,
                    ////    Margin = new Thickness(2),

                    ////};
                    ////var myBlock9 = new CheckBox
                    ////{
                    ////    IsChecked = true,
                    ////    Margin = new Thickness(2),

                    ////};
                    ////var myBlock10 = new CheckBox
                    ////{
                    ////    IsChecked = true,
                    ////    Margin = new Thickness(2),

                    ////};

                    myBlock7.SetValue(Grid.RowProperty, R);
                    myBlock7.SetValue(Grid.ColumnProperty, C);

                    ////myBlock8.SetValue(Grid.RowProperty, 1);
                    ////myBlock8.SetValue(Grid.ColumnProperty, 5);

                    ////myBlock9.SetValue(Grid.RowProperty, 1);
                    ////myBlock9.SetValue(Grid.ColumnProperty, 6);

                    ////myBlock10.SetValue(Grid.RowProperty, 1);
                    ////myBlock10.SetValue(Grid.ColumnProperty, 7);

                    //grd = new Grid();
                    //grd.Children.Add(myBlock7);//--

                    //grd.SetValue(Grid.RowProperty, R);
                    //grd.SetValue(Grid.ColumnProperty, C);

                    //ServiceGrid.Children.Add(grd);
                    //grd.Children.Add(myBlock7);
                    ServiceGrid.Children.Add(myBlock7);//--

                    ////ServiceGrid.Children.Add(myBlock8);
                    ////ServiceGrid.Children.Add(myBlock9);
                    ////ServiceGrid.Children.Add(myBlock10);

                    C = C + 1;

                }
                //}



            }

            if (PackageID > 0)
                GetPackagePharmacyItemDetails(PackageID);

            #region Comment

            //}

            //}
            //else
            //{
            //    foreach (clsPackageServiceDetailsVO itemPackServ in objPackMasterNew.PackageDetails)  //objPackServlist
            //    {
            //        //if (objPrevPackMasterNew.ServiceID == objPackMasterNew.ServiceID && itemPrevPackServ.ServiceID == itemPackServ2.ServiceID && itemPrevPackServ.DepartmentID == itemPackServ2.DepartmentID && itemPrevPackServ.IsSpecilizationGroup == itemPackServ2.IsSpecilizationGroup)
            //        //{
            //        //    itemPackServ = itemPackServ2;
            //        //}
            //        //else if (objPrevPackMasterNew.ServiceID == objPackMasterNew.ServiceID)
            //        //{
            //        //    itemPackServ = itemPrevPackServ;
            //        //}

            //        //if (objPrevPackMasterNew.ServiceID == objPackMasterNew.ServiceID && itemPrevPackServ.ServiceID == itemPackServ.ServiceID && itemPrevPackServ.DepartmentID == itemPackServ.DepartmentID && itemPrevPackServ.IsSpecilizationGroup == itemPackServ.IsSpecilizationGroup)
            //        //{

            //            //var col = new ColumnDefinition { Width = new GridLength() };
            //            //var col2 = new ColumnDefinition { Width = new GridLength() };
            //            //var col3 = new ColumnDefinition { Width = new GridLength() };
            //            //var col4 = new ColumnDefinition { Width = new GridLength() };
            //            //var col5 = new ColumnDefinition { Width = new GridLength() };

            //            //ServiceGrid.ColumnDefinitions.Add(col.DeepCopy());
            //            //ServiceGrid.ColumnDefinitions.Add(col2);
            //            //ServiceGrid.ColumnDefinitions.Add(col3);
            //            //ServiceGrid.ColumnDefinitions.Add(col4);
            //            //ServiceGrid.ColumnDefinitions.Add(col5);



            //            if (SerId != itemPackServ.ServiceID) //SerId == 0 ||
            //            {
            //                if (SerId != itemPackServ.ServiceID)  //if (R > 0)
            //                    R = R + 1;

            //                SerId = itemPackServ.ServiceID;

            //                SerCnt = objPackMasterNew.PackageDetails.Count(c => c.ServiceID == SerId && c.DepartmentID == itemPackServ.DepartmentID && c.IsSpecilizationGroup == itemPackServ.IsSpecilizationGroup);

            //                var rw = new RowDefinition { Height = new GridLength(25) };
            //                ServiceGrid.RowDefinitions.Add(rw);

            //                var col = new ColumnDefinition { Width = new GridLength() };//--
            //                ServiceGrid.ColumnDefinitions.Add(col);//--
            //                var col1 = new ColumnDefinition { Width = new GridLength() };//--
            //                ServiceGrid.ColumnDefinitions.Add(col1);//--

            //                var myBlockService = new TextBlock
            //                {
            //                    Text = itemPackServ.ServiceName,
            //                    Margin = new Thickness(2),
            //                    DataContext = itemPackServ,
            //                };

            //                myBlockService.SetValue(Grid.RowProperty, R);
            //                myBlockService.SetValue(Grid.ColumnProperty, 0);

            //                ServiceGrid.Children.Add(myBlockService);

            //                List<clsPackageServiceDetailsVO> lstPackageDetils = new List<clsPackageServiceDetailsVO>();
            //                lstPackageDetils = objPackMasterNew.PackageDetails.Where(objPackServ => objPackServ.ServiceID == itemPackServ.ServiceID && objPackServ.DepartmentID == itemPackServ.DepartmentID && objPackServ.IsSpecilizationGroup == itemPackServ.IsSpecilizationGroup).ToList();

            //                var myBlockServiceLabel = new HyperlinkButton
            //                {
            //                    Content = "Define Rule",
            //                    Margin = new Thickness(2),
            //                    DataContext = lstPackageDetils, //objPackMasterNew
            //                };

            //                myBlockServiceLabel.Click += new RoutedEventHandler(myBlock2_Click);

            //                //C = C + 1;

            //                myBlockServiceLabel.SetValue(Grid.RowProperty, R);
            //                myBlockServiceLabel.SetValue(Grid.ColumnProperty, 1);


            //                ServiceGrid.Children.Add(myBlockServiceLabel);

            //                if (SerCnt > 1)
            //                {
            //                    var myBlock = new TextBlock
            //                    {
            //                        Text = "| Rate  - " + itemPackServ.Rate.ToString(),  //"| Rate  -500 |",
            //                        Margin = new Thickness(2),

            //                    };

            //                    var myBlock2 = new TextBlock
            //                    {
            //                        Text = "| Quantity - " + itemPackServ.Quantity.ToString(),  // + ((frmPackageDefineRuleMaster)sender).txtQuantity.Text + " | ",
            //                        Margin = new Thickness(2),

            //                    };

            //                    //GenderList 
            //                    MasterListItem objGender = new MasterListItem();

            //                    var result = from r in (GenderList)
            //                                 where r.ID == itemPackServ.ApplicableTo
            //                                 select r;

            //                    if (result != null)
            //                    {
            //                        objGender = ((MasterListItem)result.FirstOrDefault());
            //                    }

            //                    var myBlock3 = new TextBlock
            //                    {
            //                        Text = "| Gender - " + objGender.Description,  // "Gender -Male |"
            //                        Margin = new Thickness(2),

            //                    };

            //                    var myBlock4 = new TextBlock
            //                    {
            //                        Text = "| Discount - " + itemPackServ.Discount.ToString() + "% ",  //"Discount - 30% |"
            //                        Margin = new Thickness(2),

            //                    };

            //                    var myBlock5 = new HyperlinkButton
            //                    {
            //                        Content = " | x",
            //                        Margin = new Thickness(2),
            //                        DataContext = lstPackageDetils,
            //                    };

            //                    myBlock5.Click += new RoutedEventHandler(myBlock5_Click);

            //                    var col3 = new ColumnDefinition { Width = new GridLength() };//--
            //                    ServiceGrid.ColumnDefinitions.Add(col3);

            //                    var col4 = new ColumnDefinition { Width = new GridLength() };//--
            //                    ServiceGrid.ColumnDefinitions.Add(col4);

            //                    var col5 = new ColumnDefinition { Width = new GridLength() };//--
            //                    ServiceGrid.ColumnDefinitions.Add(col5);

            //                    myBlock.SetValue(Grid.RowProperty, R);
            //                    myBlock.SetValue(Grid.ColumnProperty, 2);  //3
            //                    //C = C + 1;

            //                    myBlock2.SetValue(Grid.RowProperty, R);
            //                    myBlock2.SetValue(Grid.ColumnProperty, 3);  //4
            //                    //C = C + 1;

            //                    myBlock3.SetValue(Grid.RowProperty, R);
            //                    myBlock3.SetValue(Grid.ColumnProperty, 4);   //5
            //                    //C = C + 1;


            //                    myBlock4.SetValue(Grid.RowProperty, R);
            //                    myBlock4.SetValue(Grid.ColumnProperty, 5);   //6
            //                    //C = C + 1;

            //                    myBlock5.SetValue(Grid.RowProperty, R);
            //                    myBlock5.SetValue(Grid.ColumnProperty, 6);   //7

            //                    ServiceGrid.Children.Add(myBlock);
            //                    ServiceGrid.Children.Add(myBlock2);
            //                    ServiceGrid.Children.Add(myBlock3);
            //                    ServiceGrid.Children.Add(myBlock4);
            //                    ServiceGrid.Children.Add(myBlock5);

            //                    var myBlock6 = new TextBlock
            //                    {
            //                        Text = "Schedule -",
            //                        Margin = new Thickness(2),

            //                    };



            //                    //var rw2 = new RowDefinition { Height = new GridLength(25) };//--
            //                    //grd = new Grid();//--
            //                    //grd.RowDefinitions.Add(rw2);//--

            //                    var rw1 = new RowDefinition { Height = new GridLength(25) };
            //                    ServiceGrid.RowDefinitions.Add(rw1);

            //                    R = R + 1;

            //                    myBlock6.SetValue(Grid.RowProperty, R);
            //                    myBlock6.SetValue(Grid.ColumnProperty, 0);

            //                    ServiceGrid.Children.Add(myBlock6);//--
            //                    //grd.Children.Add(myBlock6);//--

            //                    //grd.SetValue(Grid.RowProperty, R);//--
            //                    //grd.SetValue(Grid.ColumnProperty, 0);//--

            //                    //ServiceGrid.Children.Add(grd);//--

            //                    C = 1;
            //                }


            //            }
            //            else
            //            {
            //                if (SerId != itemPackServ.ServiceID)
            //                {
            //                    SerId = itemPackServ.ServiceID;
            //                    C = 1;
            //                    SerCnt = 0;
            //                    R = R + 1;
            //                }
            //            }


            //            if (SerCnt > 1)
            //            {
            //                var col2 = new ColumnDefinition { Width = new GridLength() };
            //                ServiceGrid.ColumnDefinitions.Add(col2);


            //                ////if (SerId != itemPackServ.ServiceID) //SerId == 0 ||
            //                ////{
            //                ////    var myBlock6 = new TextBlock
            //                ////    {
            //                ////        Text = "|Schedule -",
            //                ////        Margin = new Thickness(2),

            //                ////    };

            //                ////    myBlock6.SetValue(Grid.RowProperty, R);
            //                ////    myBlock6.SetValue(Grid.ColumnProperty, 0);

            //                ////    ServiceGrid.Children.Add(myBlock6);
            //                ////}

            //                var myBlock7 = new CheckBox
            //                {
            //                    IsChecked = itemPackServ.MonthStatus,   //true
            //                    Margin = new Thickness(2),
            //                    Content = itemPackServ.Month,
            //                    IsEnabled = false,
            //                };

            //                ////var myBlock8 = new CheckBox
            //                ////{
            //                ////    IsChecked = true,
            //                ////    Margin = new Thickness(2),

            //                ////};
            //                ////var myBlock9 = new CheckBox
            //                ////{
            //                ////    IsChecked = true,
            //                ////    Margin = new Thickness(2),

            //                ////};
            //                ////var myBlock10 = new CheckBox
            //                ////{
            //                ////    IsChecked = true,
            //                ////    Margin = new Thickness(2),

            //                ////};

            //                myBlock7.SetValue(Grid.RowProperty, R);
            //                myBlock7.SetValue(Grid.ColumnProperty, C);

            //                ////myBlock8.SetValue(Grid.RowProperty, 1);
            //                ////myBlock8.SetValue(Grid.ColumnProperty, 5);

            //                ////myBlock9.SetValue(Grid.RowProperty, 1);
            //                ////myBlock9.SetValue(Grid.ColumnProperty, 6);

            //                ////myBlock10.SetValue(Grid.RowProperty, 1);
            //                ////myBlock10.SetValue(Grid.ColumnProperty, 7);

            //                //grd = new Grid();
            //                //grd.Children.Add(myBlock7);//--

            //                //grd.SetValue(Grid.RowProperty, R);
            //                //grd.SetValue(Grid.ColumnProperty, C);

            //                //ServiceGrid.Children.Add(grd);
            //                //grd.Children.Add(myBlock7);
            //                ServiceGrid.Children.Add(myBlock7);//--

            //                ////ServiceGrid.Children.Add(myBlock8);
            //                ////ServiceGrid.Children.Add(myBlock9);
            //                ////ServiceGrid.Children.Add(myBlock10);

            //                C = C + 1;

            //            }
            //        //}

            //    }
            //}

            # endregion

            if (objPackMasterNew.PackageDetails.Count > 0)
            {
                txtServicePercentage.IsReadOnly = true;
                txtPharmacyPercentage.IsReadOnly = true;
                txtServicePercentage.IsEnabled = false;
                txtPharmacyPercentage.IsEnabled = false;
                txtServiceFixedRate.IsReadOnly = true;
                txtPharmacyFixedRate.IsReadOnly = true;
                txtServiceFixedRate.IsEnabled = false;
                txtPharmacyFixedRate.IsEnabled = false;

            }
            else
            {
                txtServicePercentage.IsReadOnly = false;
                txtPharmacyPercentage.IsReadOnly = false;
                txtServicePercentage.IsEnabled = true;
                txtPharmacyPercentage.IsEnabled = true;
                txtServiceFixedRate.IsReadOnly = true;
                txtPharmacyFixedRate.IsReadOnly = true;
                txtServiceFixedRate.IsEnabled = true;
                txtPharmacyFixedRate.IsEnabled = true;
            }

        }

        #region Comment
        //ClickedFlag += 1;

        //if (ClickedFlag == 1)
        //{
        //    if (result == MessageBoxResult.Yes)
        //        if (CheckDuplicasy())
        //        {
        //            Save();
        //        }

        //        else
        //        {
        //            ClickedFlag = 0;
        //        }
        //}
        //else
        //{
        //    string msgText = "Please Wait, Request To Save Package Is Been Already Processed ?";

        //    MessageBoxControl.MessageBoxChildWindow msgWD =
        //        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
        //    msgWD.Show();
        //}
        # endregion

        void myBlock5_Click(object sender, RoutedEventArgs e)
        {
            if (((HyperlinkButton)sender).DataContext != null)
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to Delete the selected Service/Group ?";

                bool IsDeletePatientData = false;

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {

                    ClickedFlag += 1;

                    if (ClickedFlag == 1)
                    {
                        if (res == MessageBoxResult.Yes)
                        {

                            msgTitle = "Palash";
                            msgText = "Are you sure you want to Delete the selected Service/Group for already Registered Patient ?";

                            MessageBoxControl.MessageBoxChildWindow msgWD2 =
                                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);


                            msgWD2.OnMessageBoxClosed += (MessageBoxResult res2) =>
                            {
                                if (res2 == MessageBoxResult.Yes)
                                {
                                    IsDeletePatientData = true;
                                }
                                else
                                {
                                    IsDeletePatientData = false;
                                }

                                /////////////////////////////////////////////////////

                                List<clsPackageServiceDetailsVO> PackageServiceDetils = new List<clsPackageServiceDetailsVO>();
                                PackageServiceDetils = ((List<clsPackageServiceDetailsVO>)(((HyperlinkButton)sender).DataContext)).ToList();

                                if (PackageServiceDetils.Count > 0)
                                {
                                    clsDeletePackageServiceDetilsListNewBizActionVO BizAction = new clsDeletePackageServiceDetilsListNewBizActionVO();

                                    try
                                    {
                                        Indicatior = new WaitIndicator();
                                        Indicatior.Show();

                                        BizAction.PackageID = PackageServiceDetils[0].PackageID;
                                        BizAction.PackageUnitID = PackageServiceDetils[0].UnitID;
                                        BizAction.ServiceID = PackageServiceDetils[0].ServiceID;
                                        BizAction.SpecilizationID = PackageServiceDetils[0].DepartmentID;
                                        BizAction.IsSpecilizationGroup = PackageServiceDetils[0].IsSpecilizationGroup;
                                        BizAction.UnitID = PackageServiceDetils[0].UnitID;
                                        BizAction.Status = PackageServiceDetils[0].Status;

                                        BizAction.IsDeletePatientData = IsDeletePatientData;

                                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                                        client.ProcessCompleted += (s, arg) =>
                                        {
                                            SetCommandButtonState("Save");
                                            if (arg.Error == null)
                                            {
                                                if (((clsDeletePackageServiceDetilsListNewBizActionVO)arg.Result).SuccessStatus == 0)
                                                {

                                                    if (PackageID > 0)
                                                        GetPackageDetails(PackageID);

                                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                        new MessageBoxControl.MessageBoxChildWindow("", "Package Service/Group deleted successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                                    msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosedFlag);
                                                    msgW1.Show();

                                                    ClickedFlag = 0;
                                                }
                                            }
                                            else
                                            {
                                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                       new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosedFlag);

                                                msgW1.Show();

                                                ClickedFlag = 0;
                                            }

                                            Indicatior.Close();

                                        };
                                        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                        client.CloseAsync();
                                    }
                                    catch (Exception)
                                    {
                                        throw;
                                    }
                                    finally
                                    {
                                        Indicatior.Close();

                                    }
                                }

                                /////////////////////////////////////////////////////

                            };
                            msgWD2.Show();


                        }
                        else
                        {
                            ClickedFlag = 0;
                        }
                    }
                    else
                    {
                        msgText = "Please Wait, Request To Delete Package Service Is Been Already Processed ?";

                        MessageBoxControl.MessageBoxChildWindow msgWD1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                        msgWD1.Show();
                    }

                };
                msgWD.Show();
            }
        }

        private void dgCalender_CellEditEnded_1(object sender, DataGridCellEditEndedEventArgs e)
        {

        }

        private void dgCalender_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }

        private void cmdDeleteService_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddItems_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // PalashDynamics.Pharmacy


                //ItemList ItemsWin = new ItemList();

                ItemListWithCategory ItemsWin = new ItemListWithCategory();

                //ItemsWin.txtBlkStore.Visibility = Visibility.Collapsed;
                //ItemsWin.cmbStore.Visibility = Visibility.Collapsed;

                IsCategoryOn = false;
                IsGroupOn = false;

                if (ItemList12 != null && ItemList12.Where(Items => Items.IsCategory == true).Any() == true)
                {
                    IsCategoryOn = true;
                }

                if (ItemList12 != null && ItemList12.Where(Items => Items.IsGroup == true).Any() == true)
                {
                    IsGroupOn = true;
                }

                ItemsWin.StoreID = 0;  //((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathologyStoreID;
                ItemsWin.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
                ItemsWin.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                ItemsWin.ShowBatches = false;
                ItemsWin.cmbStore.IsEnabled = false;
                ItemsWin.ShowZeroStockBatches = false;

                ItemsWin.IsCategoryOn = IsCategoryOn;
                ItemsWin.IsGroupOn = IsGroupOn;

                ItemsWin.OnSaveButton_Click += new RoutedEventHandler(ItemsWin_OnSaveButton_Click);
                ItemsWin.Show();


            }
            catch (Exception)
            {
                throw;
            }
        }

        void ItemsWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            //ItemList Itemswin = (ItemList)sender;
            ItemListWithCategory Itemswin = (ItemListWithCategory)sender;

            IsCategoryOn = false;
            IsGroupOn = false;

            if ((ItemList12.Where(Items => Items.IsCategory == true).Any() == true) || (Itemswin.SelectedCategories != null && Itemswin.SelectedCategories.Count > 0))
            {
                IsCategoryOn = true;
            }

            if ((ItemList12.Where(Items => Items.IsGroup == true).Any() == true) || (Itemswin.SelectedGroups != null && Itemswin.SelectedGroups.Count > 0))
            {
                IsGroupOn = true;
            }

            if (Itemswin.SelectedItems != null)
            {

                foreach (var item in Itemswin.SelectedItems)
                {
                    //bool isExist = CheckForItemExistance(item.ID);
                    //if (!isExist)
                    //{

                    //if (IsGroupOn == false && IsCategoryOn == false)
                    //{
                    if (ItemList12.Where(Items => Items.ItemCategory == item.ItemCategory && Items.IsCategory == true).Any() == false)
                    {
                        if (ItemList12.Where(Items => Items.ItemGroup == item.ItemGroup && Items.IsGroup == true).Any() == false)
                        {


                            if (ItemList12.Where(Items => Items.ItemID == item.ID && Items.ItemCategory == item.ItemCategory && Items.ItemGroup == item.ItemGroup && Items.IsCategory == false).Any() == false)
                            {
                                ItemList12.Add(
                                new clsPackageItemMasterVO()
                                {
                                    ItemID = item.ID,
                                    ItemCode = item.ItemCode,
                                    ItemGroupName = item.ItemGroupString,
                                    ItemCategoryName = item.ItemCategoryString,
                                    ItemName = item.ItemName,
                                    ItemCategory = item.ItemCategory,
                                    IsCategory = false,
                                    ItemGroup = item.ItemGroup,
                                    IsGroup = false,
                                    Discount = 0.00,
                                    Quantity = 0.00,
                                    Budget = 100,
                                    Status = item.Status
                                });
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW2 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Item cannot be add as Item already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                msgW2.Show();
                            }

                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW2 =
                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Item cannot be add as Group of selected Item already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW2.Show();
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW2 =
                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Item cannot be add as Category of selected Item already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW2.Show();

                    }
                }

                dgPharmaItems.ItemsSource = null;
                dgPharmaItems.ItemsSource = ItemList12;
                //dgPharmaItems.Columns[5].IsReadOnly = true;
            }

            if (IsGroupOn == false)
            {
                if (Itemswin.SelectedCategories != null && Itemswin.SelectedCategories.Count > 0)
                {

                    foreach (var item in Itemswin.SelectedCategories)
                    {
                        //bool isExist = CheckForItemExistance(item.ID);
                        //if (!isExist)
                        //{

                        if (ItemList12.Where(Items => Items.ItemCategory == item.ID && Items.IsCategory == false).Any() == false)
                        {
                            if (ItemList12.Where(Items => Items.ItemID == item.ID && Items.ItemCategory == item.ID && Items.IsCategory == true).Any() == false)
                            {
                                ItemList12.Add(
                                new clsPackageItemMasterVO()
                                {
                                    ItemID = item.ID,
                                    ItemCode = item.Code, //item.ItemCode,
                                    ItemGroupName = "",  //item.ItemGroupString,
                                    ItemCategoryName = "",  //item.ItemCategoryString,
                                    ItemName = item.Description,  // item.ItemName,
                                    ItemCategory = item.ID,
                                    IsCategory = true,
                                    Discount = 0.00,
                                    Quantity = 0.00,
                                    Status = true  //item.Status
                                });
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW2 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Caegory cannot be add as selected Category already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                msgW2.Show();
                            }

                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW2 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Category cannot be add as Item for same Category already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW2.Show();
                        }

                        //}
                    }
                    dgPharmaItems.ItemsSource = null;
                    dgPharmaItems.ItemsSource = ItemList12;
                    //dgPharmaItems.Columns[5].IsReadOnly = true;
                }
            }
            else
            {
                if (Itemswin.SelectedCategories != null && Itemswin.SelectedCategories.Count > 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Category cannot be add as Category & Group list can not be added at a time!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW3.Show();
                }
            }

            if ((ItemList12.Where(Items => Items.IsGroup == true).Any() == true) || (Itemswin.SelectedGroups != null && Itemswin.SelectedGroups.Count > 0))
            {
                IsGroupOn = true;
            }

            if (IsCategoryOn == false)
            {
                if (Itemswin.SelectedGroups != null && Itemswin.SelectedGroups.Count > 0)
                {


                    foreach (var item in Itemswin.SelectedGroups)
                    {
                        //bool isExist = CheckForItemExistance(item.ID);
                        //if (!isExist)
                        //{

                        if (ItemList12.Where(Items => Items.ItemGroup == item.ID && Items.IsGroup == false).Any() == false)
                        {
                            if (ItemList12.Where(Items => Items.ItemID == item.ID && Items.ItemGroup == item.ID && Items.IsGroup == true).Any() == false)
                            {
                                ItemList12.Add(
                                new clsPackageItemMasterVO()
                                {
                                    ItemID = item.ID,
                                    ItemCode = item.Code, //item.ItemCode,
                                    ItemGroupName = "",  //item.ItemGroupString,
                                    ItemCategoryName = "",  //item.ItemCategoryString,
                                    ItemName = item.Description,  // item.ItemName,
                                    ItemGroup = item.ID,
                                    IsGroup = true,
                                    Discount = 0.00,
                                    Quantity = 0.00,
                                    Status = true  //item.Status
                                });
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW4 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Group cannot be add as selected Group already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                msgW4.Show();
                            }

                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW4 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Group cannot be add as Item for same Group already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW4.Show();
                        }

                        //}
                    }
                    dgPharmaItems.ItemsSource = null;
                    dgPharmaItems.ItemsSource = ItemList12;
                    //dgPharmaItems.Columns[5].IsReadOnly = true;
                }
            }
            else
            {
                if (Itemswin.SelectedGroups != null && Itemswin.SelectedGroups.Count > 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW5 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Group cannot be add as Category & Group list can not be added at a time!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW5.Show();
                }
            }


        }

        private void chkSelectAll_Click(object sender, RoutedEventArgs e)
        {
            if (chkSelectAll.IsChecked == true)
            {
                cmdItems.IsEnabled = false;
                txtBlkDiscount.Visibility = Visibility.Visible;
                txtDiscountOnAllItems.Visibility = Visibility.Visible;
                txtBlkDiscount.Visibility = Visibility.Visible;

                DisAppTB.Visibility = Visibility.Collapsed;
                txtTotalBudget.Visibility = Visibility.Collapsed;

                txtDiscountOnAllItems.Text = "0";
                txtTotalBudget.Text = "100";

                cmdItems.IsEnabled = false;

                dgPharmaItems.ItemsSource = null;
                dgPharmaItems.UpdateLayout();

                ItemList12 = new ObservableCollection<clsPackageItemMasterVO>(); //ItemList1 = new ObservableCollection<clsPackageItemMasterVO>();
                //ItemList12 = new ObservableCollection<clsPackageItemMasterVO>(((clsGetPackagePharmacyItemListNewBizActionVO)arg.Result).ItemDetails);
            }
            else
            {
                cmdItems.IsEnabled = true;
                txtBlkDiscount.Visibility = Visibility.Collapsed;
                txtDiscountOnAllItems.Visibility = Visibility.Collapsed;
                txtBlkDiscount.Visibility = Visibility.Collapsed;

                DisAppTB.Visibility = Visibility.Collapsed;
                txtTotalBudget.Visibility = Visibility.Collapsed;
                txtTotalBudget.Text = "100"; // Added By Bhushanp For New Perecntage Wise Package Changes.
                txtDiscountOnAllItems.Text = "0";

                cmdItems.IsEnabled = true;
            }
        }

        private void txtDiscountOnAllItems_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (!((TextBox)sender).Text.IsPositiveNumber())//!((TextBox)sender).Text.IsValueDouble() &&
            //{
            //    if (textBefore != null)
            //    {
            //        ((TextBox)sender).Text = textBefore;
            //        ((TextBox)sender).SelectionStart = selectionStart;
            //        ((TextBox)sender).SelectionLength = selectionLength;
            //        textBefore = "";
            //        selectionStart = 0;
            //        selectionLength = 0;
            //    }
            //}
            //Commented By Bhushanp For New Package Changes 04092017
            //if ((!((TextBox)sender).Text.IsPositiveDoubleValid() && textBefore != null))//&& 
            //{
            //    ((TextBox)sender).Text = textBefore;
            //    ((TextBox)sender).SelectionStart = selectionStart;
            //    ((TextBox)sender).SelectionLength = selectionLength;
            //    textBefore = "";
            //    selectionStart = 0;
            //    selectionLength = 0;
            //}
            /////////////////////////

            if (!(((TextBox)sender).Text == "100") && !((TextBox)sender).Text.IsValidTwoDigitWithTwoDecimal() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void cmdDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (dgPharmaItems.SelectedItem != null)
                //{
                //    //string msgTitle = "";
                //    string msgText = "Are you sure you want to Delete the selected Item ?";
                //    int index = dgPharmaItems.SelectedIndex;

                //    MessageBoxControl.MessageBoxChildWindow msgWD =
                //        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                //    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                //    {
                //        if (res == MessageBoxResult.Yes)
                //        {
                //            ItemList12.RemoveAt(index);  // ItemList1.RemoveAt(index);

                //        }
                //    };

                //    msgWD.Show();
                //    dgPharmaItems.ItemsSource = null;
                //    dgPharmaItems.ItemsSource = ItemList1;
                //}

                if (dgPharmaItems.SelectedItem != null)
                {
                    string msgTitle = "Palash";
                    string msgText = "Are you sure you want to Delete the selected Item ?";

                    bool IsDeletePatientData = false;

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {

                        ClickedFlag += 1;

                        if (ClickedFlag == 1)
                        {
                            if (res == MessageBoxResult.Yes)
                            {
                                if (((clsPackageServiceVO)this.DataContext).IsFreezed == true)
                                {
                                    msgTitle = "Palash";
                                    msgText = "Are you sure you want to Delete the selected Item for already Registered Patient ?";

                                    MessageBoxControl.MessageBoxChildWindow msgWD2 =
                                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);


                                    msgWD2.OnMessageBoxClosed += (MessageBoxResult res2) =>
                                    {
                                        if (res2 == MessageBoxResult.Yes)
                                        {
                                            IsDeletePatientData = true;
                                        }
                                        else
                                        {
                                            IsDeletePatientData = false;
                                        }

                                        /////////////////////////////////////////////////////

                                        clsPackageItemMasterVO objPackItem = new clsPackageItemMasterVO();
                                        objPackItem = ((clsPackageItemMasterVO)dgPharmaItems.SelectedItem);

                                        //List<clsPackageServiceDetailsVO> PackageServiceDetils = new List<clsPackageServiceDetailsVO>();
                                        //PackageServiceDetils = ((List<clsPackageServiceDetailsVO>)(((HyperlinkButton)sender).DataContext)).ToList();

                                        if (objPackItem != null)
                                        {

                                            //by anjali
                                            if (objPackItem.PackageID != 0 && objPackItem.PackageID != null)
                                            {

                                                clsDeletePackageItemDetilsListNewBizActionVO BizAction = new clsDeletePackageItemDetilsListNewBizActionVO();

                                                try
                                                {
                                                    Indicatior = new WaitIndicator();
                                                    Indicatior.Show();

                                                    BizAction.PackageID = objPackItem.PackageID;
                                                    BizAction.PackageUnitID = objPackItem.UnitID;
                                                    BizAction.ItemID = objPackItem.ItemID;
                                                    BizAction.ItemCategoryID = objPackItem.ItemCategory;
                                                    BizAction.IsCategory = objPackItem.IsCategory;
                                                    BizAction.ItemGroupID = objPackItem.ItemGroup;
                                                    BizAction.IsGroup = objPackItem.IsGroup;
                                                    BizAction.UnitID = objPackItem.UnitID;
                                                    BizAction.Status = objPackItem.Status;

                                                    BizAction.IsDeletePatientData = IsDeletePatientData;

                                                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                                                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                                                    client.ProcessCompleted += (s, arg) =>
                                                    {
                                                        SetCommandButtonState("Save");
                                                        if (arg.Error == null)
                                                        {
                                                            if (((clsDeletePackageItemDetilsListNewBizActionVO)arg.Result).SuccessStatus == 0)
                                                            {

                                                                if (PackageID > 0)
                                                                    GetPackagePharmacyItemDetails(PackageID);

                                                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                                    new MessageBoxControl.MessageBoxChildWindow("", "Package Item deleted successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                                                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosedFlag);
                                                                msgW1.Show();

                                                                ClickedFlag = 0;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                                   new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                                            msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosedFlag);

                                                            msgW1.Show();

                                                            ClickedFlag = 0;
                                                        }

                                                        Indicatior.Close();

                                                    };
                                                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                                    client.CloseAsync();
                                                }
                                                catch (Exception)
                                                {
                                                    throw;
                                                }
                                                finally
                                                {
                                                    Indicatior.Close();

                                                }
                                            }
                                            else
                                            {

                                                int index = dgPharmaItems.SelectedIndex;
                                                ItemList12.RemoveAt(index);
                                                dgPharmaItems.UpdateLayout();
                                                ClickedFlag = 0;
                                            }
                                        }

                                        /////////////////////////////////////////////////////

                                    };
                                    msgWD2.Show();

                                }
                                else
                                {
                                    int index = dgPharmaItems.SelectedIndex;
                                    ItemList12.RemoveAt(index);
                                    dgPharmaItems.UpdateLayout();
                                    ClickedFlag = 0;
                                }
                            }
                            else
                            {
                                ClickedFlag = 0;
                            }
                        }
                        else
                        {
                            msgText = "Please Wait, Request To Delete Package Item Is Been Already Processed ?";

                            MessageBoxControl.MessageBoxChildWindow msgWD1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                            msgWD1.Show();
                        }

                    };
                    msgWD.Show();
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    //cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    //cmdNew.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "New":
                    //cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    //cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Save":
                    //cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Modify":
                    //cmdNew.IsEnabled = true;
                    //cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Cancel":
                    //cmdNew.IsEnabled = true;
                    //cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;

                case "View":
                    //cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    //cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                default:
                    break;
            }
        }



        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(((TextBox)sender).Text.IsValueDouble()))
            {
                if (textBefore != null)
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        private void TextNameV_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtValidity_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private bool ChkValidationPharmacy()
        {
            bool result = true;

            if (ItemList12 == null && chkSelectAll.IsChecked == false)  //dgCalender.ItemsSource
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "You Can not save without Item list.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                //dgCalender.Focus();
                result = false;
                return result;

            }

            if (ItemList12 != null)  //dgCalender.ItemsSource
            {
                if (ItemList12.Count == 0 && chkSelectAll.IsChecked == false)  //((ObservableCollection<PackageList>)dgCalender.ItemsSource).Count
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", "You Can not save without Item list.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                    //dgCalender.Focus();
                    result = false;
                    return result;

                }
            }

            if (ItemList12.Count == 0 && chkSelectAll.IsChecked == true && (((clsPackageServiceVO)this.DataContext).ApplicableToAllDiscount == 0) && (((clsPackageServiceVO)this.DataContext).TotalBudget == 0))  //dgCalender.ItemsSource
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "You Can not save Discount And Budget For All Items to Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                //dgCalender.Focus();
                result = false;
                return result;

            }
            //Commented By Bhushanp For Package Percentage Changes 28082017
            //if (chkSelectAll.IsChecked == true && Convert.ToDouble(txtDiscountOnAllItems.Text)==0)  //dgCalender.ItemsSource
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                       new MessageBoxControl.MessageBoxChildWindow("", "You Can not save Discount For All Items to Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //    msgW1.Show();
            //    //dgCalender.Focus();
            //    result = false;
            //    return result;

            //}    

            if (Convert.ToDouble(txtTotalBudget.Text) > Convert.ToDouble(txtPharmacyFixedRate.Text) && Convert.ToDouble(txtPharmacyFixedRate.Text) > 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "You Can not save? TotalBudget Is Greater Then Pharamacy Component .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                result = false;
                return result;
            }

            if (Convert.ToDouble(txtTotalBudget.Text) > Convert.ToDouble(100) && Convert.ToDouble(txtPharmacyPercentage.Text) > 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "You Can not save? TotalBudget % Is Greater Than 100 .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                result = false;
                return result;
            }

            int count = 0;
            int Bcount = 0;
            string ItemName = string.Empty;
            string BItemName = string.Empty;
            if (ItemList12.Count > 0)
            {
                foreach (var item in ItemList12)
                {
                    if (item.Discount == 0)
                    {
                        count = count + 1;
                        if (ItemName.Length == 0)
                            ItemName = ItemName + item.ItemName;
                        else
                            ItemName = ItemName + ", " + item.ItemName;
                        //result = false;
                    }

                    if (item.Budget == 0)
                    {
                        Bcount = count + 1;
                        if (BItemName.Length == 0)
                            BItemName = BItemName + item.ItemName;
                        else
                            BItemName = BItemName + ", " + item.ItemName;
                        //result = false;}
                    }
                    //Commented By Bhushanp For Package Percentage Changes 28082017
                    //if (item.Discount == 0 && item.Budget == 0)
                    //{
                    //    string msgText1 = "For";

                    //    msgText = "For " + ItemName + " Discount and budget can not be Zero.";

                    //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    //                           new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //    msgW1.Show();
                    //    result = false;
                    //    return result;
                    //}

                }
            }

            //if (count > 0)
            //{
            //    string msgText1 = "For";

            //    msgText = "For " + ItemName + " Discount can not be Zero.";

            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                           new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    msgW1.Show();
            //    result = false;
            //    return result;
            //}
            //if (Bcount > 0)
            //{
            //    string msgText1 = "For";                
            //    msgText = "For " + BItemName + " budget can not be Zero.";

            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                           new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    msgW1.Show();
            //    result = false;
            //    return result;
            //}

            //if (count > 0 && Bcount > 0)
            //{
            //    string msgText1 = "For";

            //    msgText = "For " + ItemName + " Discount and budget can not be Zero.";

            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                           new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    msgW1.Show();
            //    result = false;
            //    return result;
            //}            

            return result;
        }

        private void cmdSavePharmacyItems_Click(object sender, RoutedEventArgs e)
        {
            if (ChkValidationPharmacy())
            {

                //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                //{
                //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("msgPackageService");
                //}
                //else
                //{
                msgText = "Are you sure you want to save the Package Pharmacy Items ?";
                //}
                MessageBoxControl.MessageBoxChildWindow msgWPharmacy =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWPharmacy.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWPharmacy_OnMessageBoxClosed);

                msgWPharmacy.Show();
            }
        }

        void msgWPharmacy_OnMessageBoxClosed(MessageBoxResult result)
        {
            ClickedFlag += 1;

            if (ClickedFlag == 1)
            {
                if (result == MessageBoxResult.Yes)
                //if (CheckDuplicasy())
                {
                    SavePharmacyItems();
                }

                else
                {
                    ClickedFlag = 0;
                }
            }
            else
            {
                string msgText = "Please Wait, Request To Save Package Pharmacy Items Is Been Already Processed ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                msgWD.Show();
            }
        }

        private void SavePharmacyItems()
        {
            clsAddPackagePharmacyItemsNewBizActionVO BizAction = new clsAddPackagePharmacyItemsNewBizActionVO();


            try
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();

                BizAction.PackageID = PackageID;
                BizAction.PackageUnitID = PackageServiceUnitID;

                BizAction.ItemDetails = new List<clsPackageItemMasterVO>();

                BizAction.ItemDetails = ItemList12.ToList();

                BizAction.ApplicableToAll = Convert.ToBoolean(chkSelectAll.IsChecked);
                BizAction.ApplicableToAllDiscount = ((clsPackageServiceVO)this.DataContext).ApplicableToAllDiscount; //txtDiscountOnAllItems.Text 

                if (Convert.ToDouble(txtPharmacyPercentage.Text) > 0)
                {
                    //BizAction.TotalBudget = ((((clsPackageServiceVO)this.DataContext).PackageAmount) * ((((clsPackageServiceVO)this.DataContext).PharmacyPercentage) / 100)) ;

                    BizAction.TotalBudget = ((((clsPackageServiceVO)this.DataContext).PackageAmount) * ((((clsPackageServiceVO)this.DataContext).PharmacyPercentage) / 100)) * 1; //Commented By Bhushan For Package Changes 30082017 //(((clsPackageServiceVO)this.DataContext).TotalBudget / 100);
                    //((clsPackageServiceVO)this.DataContext).TotalBudget;                     
                }
                else
                {
                    BizAction.TotalBudget = ((clsPackageServiceVO)this.DataContext).TotalBudget;
                }



                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    //SetCommandButtonState("Save");
                    if (arg.Error == null)
                    {
                        if (((clsAddPackagePharmacyItemsNewBizActionVO)arg.Result).ItemDetails != null)
                        {
                            //ClearData();
                            //FetchData();

                            PackageID = ((clsAddPackagePharmacyItemsNewBizActionVO)arg.Result).PackageID;

                            if (PackageID > 0)  //&& ((clsAddPackagePharmacyItemsNewBizActionVO)arg.Result).ApplicableToAll == false
                                GetPackagePharmacyItemDetails(PackageID);

                            if (PackageID > 0 && ((clsAddPackagePharmacyItemsNewBizActionVO)arg.Result).ApplicableToAll == true)
                                GetPackageDetails(PackageID);

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Package Pharmacy Items added successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosedFlag);
                            msgW1.Show();

                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosedFlag);

                        msgW1.Show();

                    }

                    Indicatior.Close();

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Indicatior.Close();

            }
        }

        private void GetPackagePharmacyItemDetails(long packageid)
        {
            clsGetPackagePharmacyItemListNewBizActionVO BizAction = new clsGetPackagePharmacyItemListNewBizActionVO();
            try
            {
                BizAction.ItemDetails = new List<clsPackageItemMasterVO>();
                BizAction.PackageID = PackageID;
                BizAction.UnitId = PackageServiceUnitID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetPackagePharmacyItemListNewBizActionVO)arg.Result).ItemDetails != null)
                        {
                            ItemList12 = new ObservableCollection<clsPackageItemMasterVO>(((clsGetPackagePharmacyItemListNewBizActionVO)arg.Result).ItemDetails);
                            dgPharmaItems.ItemsSource = ItemList12;
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }

                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }

        }

        private void ChildWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            OnAddButton_Click(this, new RoutedEventArgs());
        }

        private void txtDiscountOnAllItems_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        //private void txtDiscountOnAllItems_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    if (Convert.ToDouble(txtDiscountOnAllItems.Text) < 0)
        //    {
        //        txtDiscountOnAllItems.Text = "0";
        //    }
        //}

        private void txtDiscountOnAllItems_LostFocus_1(object sender, RoutedEventArgs e)
        {
            if (txtDiscountOnAllItems.Text != string.Empty)
            {
                if (Convert.ToDouble(txtDiscountOnAllItems.Text) > 100)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                  new MessageBoxControl.MessageBoxChildWindow("", "Discount should be less than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                    txtDiscountOnAllItems.Text = "100";
                }
            }
            else
            {
                txtDiscountOnAllItems.Text = "0";
            }
        }

        private void txtTotalBudget_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((!((TextBox)sender).Text.IsPositiveDoubleValid() && textBefore != null))//&& 
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtTotalBudget_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtBudget_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValidDigintWithTwoDecimalPlaces() && textBefore != null)
            {

                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtBudget_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        //private void rdbFixed_Click(object sender, RoutedEventArgs e)
        //{

        //    if (rdbFixed.IsChecked == true)
        //    {
        //        //chkAll.Visibility = Visibility.Visible;

        //        //chkAll.IsChecked = false;
        //        //FillSpecilizationGrid();
        //        //dgServiceList.ItemsSource = null;
        //        //DocSpecilization.Visibility = System.Windows.Visibility.Visible;
        //        //DocService.Visibility = System.Windows.Visibility.Collapsed;
        //        //cmbSpecialization.IsEnabled = false;
        //        //cmbSubSpecialization.IsEnabled = false;
        //        //txtServiceName.IsReadOnly = true;
        //    }
        //    //else
        //    //{
        //    //    dgSpecilization.ItemsSource = null;

        //    //}
        //}


        //private void rdbPercentage_Click(object sender, RoutedEventArgs e)
        //{
        //    if (rdbPercentage.IsChecked == true)
        //    {
        //        //chkAll.Visibility = Visibility.Visible;

        //        //chkAll.IsChecked = false;
        //        //FillSpecilizationGrid();
        //        //dgServiceList.ItemsSource = null;
        //        //DocSpecilization.Visibility = System.Windows.Visibility.Visible;
        //        //DocService.Visibility = System.Windows.Visibility.Collapsed;
        //        //cmbSpecialization.IsEnabled = false;
        //        //cmbSubSpecialization.IsEnabled = false;
        //        //txtServiceName.IsReadOnly = true;
        //    }
        //    //else
        //    //{
        //    //    dgSpecilization.ItemsSource = null;

        //    //}
        //}

        private void rdbFixed_Checked(object sender, RoutedEventArgs e)
        {
            //if (rdbFixed.IsChecked == true)
            //    {
            //        txtServiceFixedRate.Visibility = Visibility.Visible;
            //        txtPharmacyFixedRate.Visibility = Visibility.Visible;

            //        txtServicePercentage.Visibility = Visibility.Collapsed;
            //        txtPharmacyPercentage.Visibility = Visibility.Collapsed;
            //    }           
        }

        private void rdbPercentage_Checked(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //if (rdbPercentage.IsChecked == true)
            //{
            //    txtServiceFixedRate.Visibility = Visibility.Collapsed;
            //    txtPharmacyFixedRate.Visibility = Visibility.Collapsed;
            //    txtServicePercentage.Visibility = Visibility.Visible;
            //    txtPharmacyPercentage.Visibility = Visibility.Visible;
            //}
            //}
            //catch (Exception)
            //{

            //    //throw;
            //}

        }

        private void rdbFixed_Click(object sender, RoutedEventArgs e)
        {
            if (rdbFixedNew.IsChecked == true)
            {
                txtServiceFixedRate.Visibility = Visibility.Visible;
                txtPharmacyFixedRate.Visibility = Visibility.Visible;

                txtServicePercentage.Visibility = Visibility.Collapsed;
                txtPharmacyPercentage.Visibility = Visibility.Collapsed;
                if (PackageID == 0 && objPackMaster.PackageDetails.Count() == 0)
                {
                    txtServicePercentage.Text = "0";
                    txtPharmacyPercentage.Text = "0";
                }
            }
        }

        private void rdbPercentage_Click(object sender, RoutedEventArgs e)
        {
            if (rdbPercentage.IsChecked == true)
            {
                txtServiceFixedRate.Visibility = Visibility.Collapsed;
                txtPharmacyFixedRate.Visibility = Visibility.Collapsed;
                txtServicePercentage.Visibility = Visibility.Visible;
                txtPharmacyPercentage.Visibility = Visibility.Visible;

                if (PackageID == 0 && objPackMaster.PackageDetails.Count() == 0)
                {
                    txtServiceFixedRate.Text = "0";
                    txtPharmacyFixedRate.Text = "0";
                }
            }
        }

        private void txtDiscount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValueDouble())
            {
                if (textBefore != null)
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        private void txtDiscount_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;

        }

        private void txtServiceFixedRate_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Convert.ToDouble(txtServiceFixedRate.Text) > Convert.ToDouble(txtAmount.Text))
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("", "Please Enter valid Amount.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();

                txtServiceFixedRate.Text = "0";
            }
            else if (Convert.ToDouble(txtServiceFixedRate.Text) > 0)
            {
                txtPharmacyFixedRate.Text = (Convert.ToDouble(txtAmount.Text) - Convert.ToDouble(txtServiceFixedRate.Text)).ToString();
            }
        }

        private void txtPharmacyFixedRate_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Convert.ToDouble(txtPharmacyFixedRate.Text) > Convert.ToDouble(txtAmount.Text))
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("", "Please Enter valid Amount.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();

                txtPharmacyFixedRate.Text = "0";
            }
            else if (Convert.ToDouble(txtPharmacyFixedRate.Text) > 0)
            {
                txtServiceFixedRate.Text = (Convert.ToDouble(txtAmount.Text) - Convert.ToDouble(txtPharmacyFixedRate.Text)).ToString();
            }

        }


        private void txtServicePercentage_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Convert.ToDouble(txtServicePercentage.Text) > 100)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Please Enter valid Percentage.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();

                txtServicePercentage.Text = "0";
            }
            else if (Convert.ToDouble(txtServicePercentage.Text) > 0)
            {
                txtPharmacyPercentage.Text = (100 - Convert.ToDouble(txtServicePercentage.Text)).ToString();
            }

            //double TPercentage = Convert.ToDouble(txtServicePercentage.Text) + Convert.ToDouble(txtPharmacyPercentage.Text);
            //if (100 < TPercentage)
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                   new MessageBoxControl.MessageBoxChildWindow("", "Please Enter valid Percentage.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    msgW1.Show();
            //    //txtServicePercentage.Focus();
            //}
        }

        private void txtPharmacyPercentage_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Convert.ToDouble(txtPharmacyPercentage.Text) > 100)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                              new MessageBoxControl.MessageBoxChildWindow("", "Please Enter valid Percentage.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();

                txtPharmacyPercentage.Text = "0";
            }
            else if (Convert.ToDouble(txtPharmacyPercentage.Text) > 0)
            {
                txtServicePercentage.Text = (100 - Convert.ToDouble(txtPharmacyPercentage.Text)).ToString();
            }

            //double TPercentage = Convert.ToDouble(txtServicePercentage.Text) + Convert.ToDouble(txtPharmacyPercentage.Text);
            //if (100 < TPercentage)
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                 new MessageBoxControl.MessageBoxChildWindow("", "Please Enter valid Percentage.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    msgW1.Show();
            //   // txtPharmacyPercentage.Focus();
            //}
        }

        private void txtServicePercentage_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(((TextBox)sender).Text == "100") && !((TextBox)sender).Text.IsValidTwoDigitWithTwoDecimal() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtServicePercentage_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }
    }
}

