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
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.UserControls;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Reflection;
using PalashDynamics.ValueObjects.Administration;
using CIMS.Forms;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects.Billing;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using PalashDynamics.IPD.Forms;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using OPDModule.Forms;
using PalashDynamics.Animations;
using PalashDynamics.Collections;
using System.Windows.Browser;
using System.Globalization;
using PalashDynamics.ValueObjects.Radiology;
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.ValueObjects.Administration.IPD;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Administration.UserRights;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.Log;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects.Administration.PackageNew;
using MessageBoxControl;
using PalashDynamics.Pharmacy.ItemSearch;
using PalashDynamics.Pharmacy;
using OPDModule;
using PalashDynamics.ValueObjects.Master.DoctorMaster;

namespace PalashDynamics.IPD
{
    public partial class frmIPDBills : UserControl, IPassData
    {
        public enum RequestType
        {
            Concession = 1,
            Refund = 2
        }
        int RowNum = 0; //Added By Bhushanp 17022017 use for delete service
        bool flagFreezFromSearch = false;
        double TotalConcession = 0;
        double PatAdvanceAmt = 0;
        double PatBalAdvaceAmt = 0;
        double PatPaybleAmt = 0;
        double concession_Reasonid = 0;
        bool IsPackageConsumption = false;
        #region "Paging"
        public PagedSortableCollectionView<clsBillVO> DataList { get; private set; }
        public ObservableCollection<clsChargeVO> BillAddedItems { get; set; }
        public ObservableCollection<clsChargeVO> DeleteBillAddedItems { get; set; }

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
            FillBillSearchList();

        }
        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        /// 
        private void FillBillSearchList()
        {
            indicator.Show();
            clsGetBillSearchListBizActionVO BizAction = new clsGetBillSearchListBizActionVO();
            BizAction.RequestTypeID = (int)RequestType.Concession;
            BizAction.IsRequest = true;
            if (dtpFromDate.SelectedDate != null)
                BizAction.FromDate = dtpFromDate.SelectedDate.Value.Date;
            if (dtpToDate.SelectedDate != null)
                BizAction.ToDate = dtpToDate.SelectedDate.Value.Date;
            BizAction.PatientID = PatientID;
            BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
            {
                BizAction.UnitID = BizAction.PatientUnitID.Value;//((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            else
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
            else
                BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IPDCounterID;

            BizAction.BillType = PalashDynamics.ValueObjects.BillTypes.Clinical; //Added by AJ Date6/1/2017 Only Display Clinical Bill
            BizAction.Opd_Ipd_External = 1;
            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    clsGetBillSearchListBizActionVO result = e.Result as clsGetBillSearchListBizActionVO;
                    DataList.TotalItemCount = result.TotalRows;
                    DataList.Clear();
                    if (result.List != null)
                    {
                        foreach (var item in result.List)
                        {
                            //if (!item.IsFreezed)
                            //{
                            //    cmdNew.IsEnabled = false;
                            //}
                            if (!item.IsPackageConsumption)
                            {
                                DataList.Add(item);
                                PatPaybleAmt = PatPaybleAmt + item.NetBillAmount;
                            }
                        }
                        dgBillList.ItemsSource = null;
                        dgBillList.ItemsSource = DataList;

                        dgDataPager.Source = null;
                        dgDataPager.PageSize = BizAction.MaximumRows;
                        dgDataPager.Source = DataList;
                        //chkBillFreeze();

                        // Added on 07Feb2019 for Package Flow in IPD
                        //var items4 = from r in result.List
                        //             where r.IsPackageServiceInclude == true && r.IsFreezed == false
                        //             select r;
                        //if (items4.ToList().Count() > 0)
                        //{
                        //    IsPackageFreezed = true;
                        //}

                    }
                }
                indicator.Close();
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        #endregion

        public bool IsPatientExist = false;
        public long PatientID = 0;
        long BillID = 0;
        long UnitID = 0;
        long PackageTariffID = 0;
        long ServiceTariffID = 0;
        bool FromVisit = false;
        long AdmissionID = 0;
        long AdmissionUnitID = 0;
        bool IsFromViewClick = false;
        long PRID = 0;
        long CashCounterID = 0;
        String CashCounterName = "";
        public bool IsFromRequestApproval = false;
        public bool IsFromSaveAndBill = false;
        bool IsMsgServiceConsumption = false;
        bool IsFromOtherServices = false;
        public decimal DiscRateOnForm = 0;
        //List<string> SpecID = new List<string>();
        bool AssignForOnlyOnce = false;

        List<MasterListItem> DoctorList = new List<MasterListItem>();
        PalashDynamics.Animations.SwivelAnimation _flip = null;
        public ObservableCollection<clsItemSalesDetailsVO> PharmacyItems { get; set; }
        public ObservableCollection<PalashDynamics.ValueObjects.clsPackageServiceDetailsVO> PackageServiceList { get; set; }
        ObservableCollection<clsChargeVO> ChargeList { get; set; }
        ObservableCollection<clsChargeVO> ChargeListForFrontGrid { get; set; }
        List<clsPatientFollowUpVO> FollowUpDetails { get; set; }
        #region IInitiateCIMS Members

        /// <summary>
        /// Function is for initializing the form based on Mode Passes as Paramater.
        /// </summary>
        /// <param name="Mode"></param>
        public void Initiate(string Mode)
        {

            switch (Mode.ToUpper())
            {
                case "NEW":

                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        return;
                    }
                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        return;
                    }
                    IsPatientExist = true;
                    PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;

                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                    mElement.Text = " : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                        " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                        ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;
                    mElement.Text += " - " + ((IApplicationConfiguration)App.Current).SelectedIPDPatient.MRNo;

                    break;

                case "VISIT":

                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        return;
                    }
                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        return;
                    }
                    IsPatientExist = true;
                    PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    UserControl rootPage2 = Application.Current.RootVisual as UserControl;
                    TextBlock mElement3 = (TextBlock)rootPage2.FindName("SampleSubHeader");

                    mElement3.Text = " : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                        " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                        ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;

                    mElement3.Text += " - " + ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                    FromVisit = true;
                    break;
                default:
                    break;
            }
        }

        #endregion

        bool IsPageLoaded = false;
        bool IsAuditTrail = false;
        public clsBillVO SelectedBill { get; set; }
        clsPatientVO myPatient { get; set; }
        bool IsEditMode = false;
        bool IsPackagePatient = false;
        long? PatientCategoryID { get; set; }

        long PatientSourceID { get; set; }
        long PatientTariffID { get; set; }
        long PatientCompanyID { get; set; }
        int lineNumber = 0;
        List<LogInfo> LogInfoList = new List<LogInfo>();  // For the Activity Log List
        LogInfo LogInformation;
        Guid objGUID;
        //
        /// <summary>
        /// Function is for fetching Patient Details
        /// </summary>
        private void GetPatientDetails()
        {
            clsGetPatientBizActionVO BizAction1 = new clsGetPatientBizActionVO();
            BizAction1.PatientDetails = new clsPatientVO();
            BizAction1.PatientDetails.GeneralDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction1.PatientDetails.GeneralDetails.UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
            Client1.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        myPatient = new clsPatientVO();
                        myPatient = ((clsGetPatientBizActionVO)arg.Result).PatientDetails;
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured While Adding Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            };

            Client1.ProcessAsync(BizAction1, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client1.CloseAsync();

        }

        /// <summary>
        /// Function is for checking active visit for patient is present or not.
        ///If active visit is present then it calls the functions GetPatientDetails and GetVisitDetails.
        /// </summary>
        private void CheckAdmission(bool IsDefaultService)
        {

            WaitIndicator ind = new WaitIndicator();
            ind.Show();
            clsGetVisitBizActionVO BizAction = new clsGetVisitBizActionVO();
            BizAction.ObjAdmission = new PalashDynamics.ValueObjects.IPD.clsGetIPDAdmissionBizActionVO();
            BizAction.ObjAdmission.Details = new PalashDynamics.ValueObjects.IPD.clsIPDAdmissionVO();
            BizAction.ObjAdmission.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.ObjAdmission.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
            BizAction.GetLatestVisit = true;
            BizAction.OPD_IPD_External = 1;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetVisitBizActionVO)arg.Result).ObjAdmission.Details != null && ((clsGetVisitBizActionVO)arg.Result).ObjAdmission.Details.ID > 0)
                    {
                        AdmissionID = ((clsGetVisitBizActionVO)arg.Result).ObjAdmission.Details.ID;
                        AdmissionUnitID = ((clsGetVisitBizActionVO)arg.Result).ObjAdmission.Details.UnitId;
                        VisitDoctorID = ((clsGetVisitBizActionVO)arg.Result).ObjAdmission.Details.DoctorID;
                        //VisitDoctor = ((clsGetVisitBizActionVO)arg.Result).ObjAdmission.Details.ReferredDoctor;
                        InitialiseForm();
                        SetCommandButtonState("ClickNew");
                        //GetPatientDetails(); Commented By Bhushan
                        //GetVisitDetails();
                        if (AdmissionID > 0)
                        {
                            long lUnitID;
                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                            {
                                lUnitID = 0;
                            }
                            else
                            {
                                lUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                            }
                            //FillChargeList(0, lUnitID, false, AdmissionID, AdmissionUnitID, IsDefaultService);
                        }
                        if (IsPackagePatient == true)
                            lnkAddPackageService.IsEnabled = true;
                        _flip.Invoke(RotationType.Forward);
                        FillPatientAdvance(true);
                        ind.Close();
                    }
                    else
                    {
                        ind.Close();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient is not Admitted", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        lnkAddService.IsEnabled = false;
                        lnkAddServicesFromPrescription.IsEnabled = false;
                        lnkAddItemsFromPrescription.IsEnabled = false;
                        lnkAddItems.IsEnabled = false;
                        cmdSave.IsEnabled = false;
                        return;
                    }
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        public frmIPDBills()
        {

            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmBill_Loaded);
            _flip = new PalashDynamics.Animations.SwivelAnimation(LayoutRoot1, LayoutRoot2, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            dtpFromDate.SelectedDate = null;
            dtpToDate.SelectedDate = null;
            fillDoctor();
            dgCharges.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgCharges_CellEditEnded);
            dgPharmacyItems.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgPharmacyItems_CellEditEnded);
            IsAuditTrail = ((IApplicationConfiguration)App.Current).CurrentUser.IsAuditTrail;
            //Paging
            DataList = new PagedSortableCollectionView<clsBillVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;
            fillreason();
            DeleteBillAddedItems = new ObservableCollection<clsChargeVO>();
        }
        List<MasterListItem> objList = new List<MasterListItem>();
        private void fillDoctor()
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            BizAction.IsNonReferralDoctor = true;
            BizAction.ReferralID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DoctorTypeForReferral;
            BizAction.UnitId = 0;
            BizAction.DepartmentId = 0;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    objList.Clear();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);
                    DoctorList = objList;
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        void dgPharmacyItems_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            if (e.Column.DisplayIndex == 6)
            {
                if (((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity > ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableQuantity)
                {
                    double availQty = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableQuantity;
                    ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity = availQty;
                    string msgText = "Quantity Must Be Less Than Or Equal To Available Quantity " + availQty;
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD.Show();
                }
            }
            else if (e.Column.DisplayIndex == 11)
            {
                if (((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).VATPercent > 100)
                {
                    ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).VATPercent = 100;
                    string msgText = "Percentage Should Not Be Greater Than 100";
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD.Show();
                }
            }
            else if (e.Column.DisplayIndex == 8)
            {
                if (((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConcessionPercentage > 100)
                {
                    ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConcessionPercentage = 100;
                    string msgText = "Percentage Should Not Be Greater Than 100";
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD.Show();
                }
            }
            else if (e.Column.DisplayIndex == 9)
            {
                if (((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConcessionAmount > ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Amount)
                {
                    ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConcessionAmount = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Amount;
                    string msgText = "Concession Amount Should Not Be Greater Than Amount " + ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Amount;
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD.Show();
                }
            }

            else if (e.Column.DisplayIndex == 7)
            {
                if (((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).MRP < ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).PurchaseRate)
                {
                    double PurchaseRate = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).PurchaseRate;
                    double MRP = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).OriginalMRP;
                    ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).MRP = MRP;
                    string msgText = "MRP Must Be Greater Than Or Equal To Purchase Rate :" + PurchaseRate;

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD.Show();
                }
            }
            CalculatePharmacySummary();
            fillDoctor();
        }

        void dgCharges_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            // Display Indesx change Ny Bhushanp 20042017
            if (e.Column.Header.ToString().Equals("Rate"))//(e.Column.DisplayIndex == 4)
            {
                if (dgCharges.SelectedItem != null)
                {

                    if (((clsChargeVO)dgCharges.SelectedItem).RateEditable)
                    {
                        #region GST Details added by Ashish Z. on dated 24062017
                        double TotalTaxamount = 0;
                        if (((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsList != null && ((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsList.Count() > 0)
                        {
                            ((clsChargeVO)dgCharges.SelectedItem).TaxType = ((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsList[0].TaxType;
                            foreach (var item in ((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsList.ToList())
                            {
                                item.Concession = ((clsChargeVO)dgCharges.SelectedItem).Concession;
                                item.TotalAmount = ((clsChargeVO)dgCharges.SelectedItem).TotalAmount;
                                item.Rate = ((clsChargeVO)dgCharges.SelectedItem).Rate;
                                TotalTaxamount += item.ServiceTaxAmount;
                            }
                            ((clsChargeVO)dgCharges.SelectedItem).TotalServiceTaxAmount = TotalTaxamount;
                            CalculateClinicalSummary();
                        }
                        else
                        {
                            if (((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsVO != null && ((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsVO.TaxLinkingDetailsList != null && ((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsVO.TaxLinkingDetailsList.Count() > 0)
                            {
                                ((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsList = GetServiceTaxList(((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsVO.TaxLinkingDetailsList);
                            }
                        }
                        #endregion

                        if ((((clsChargeVO)dgCharges.SelectedItem).Rate < ((clsChargeVO)dgCharges.SelectedItem).MinRate)
                            || (((clsChargeVO)dgCharges.SelectedItem).Rate > ((clsChargeVO)dgCharges.SelectedItem).MaxRate))
                        {
                            ((clsChargeVO)dgCharges.SelectedItem).Rate = ((clsChargeVO)dgCharges.SelectedItem).MaxRate;
                            string msgText = "Rate Must Be In Between  " + ((clsChargeVO)dgCharges.SelectedItem).MinRate + " and " + ((clsChargeVO)dgCharges.SelectedItem).MaxRate;

                            MessageBoxControl.MessageBoxChildWindow msgWD =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWD.Show();
                        }
                    }
                }
            }
            else if (e.Column.Header.ToString().Equals("Concession %") || (e.Column.Header.ToString().Equals("Concession Amt.")))//(e.Column.DisplayIndex == 6 || e.Column.DisplayIndex == 7)
            {
                if (dgCharges.SelectedItem != null)
                {
                    #region GST Details added by Ashish Z. on dated 24062017
                    double TotalTaxamount = 0;
                    if (((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsList != null && ((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsList.Count() > 0)
                    {
                        ((clsChargeVO)dgCharges.SelectedItem).TaxType = ((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsList[0].TaxType;
                        foreach (var item in ((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsList.ToList())
                        {
                            item.Concession = ((clsChargeVO)dgCharges.SelectedItem).Concession;
                            item.TotalAmount = ((clsChargeVO)dgCharges.SelectedItem).TotalAmount;
                            item.Rate = ((clsChargeVO)dgCharges.SelectedItem).Rate;
                            TotalTaxamount += item.ServiceTaxAmount;
                        }
                        ((clsChargeVO)dgCharges.SelectedItem).TotalServiceTaxAmount = TotalTaxamount;
                        CalculateClinicalSummary();
                    }
                    else
                    {
                        if (((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsVO != null && ((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsVO.TaxLinkingDetailsList != null && ((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsVO.TaxLinkingDetailsList.Count() > 0)
                        {
                            ((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsList = GetServiceTaxList(((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsVO.TaxLinkingDetailsList);
                        }
                    }
                    #endregion

                    if (((clsChargeVO)dgCharges.SelectedItem).Concession > 0)
                    {
                        if (objUser.IpdBillAuthLvlID == 0 && IsEditMode == false)
                        {
                            cmbConcessionReason.IsEnabled = true;
                            // txtApprovalRemark.IsEnabled = true;
                        }
                        else
                        {
                            if (IsEditMode == true && dgBillList.SelectedItem != null)
                            {
                                if ((dgBillList.SelectedItem as clsBillVO).LevelID > 0)
                                {
                                    cmbConcessionReason.IsEnabled = true;
                                    // txtApprovalRemark.IsEnabled = true;
                                }
                                else if ((dgBillList.SelectedItem as clsBillVO).LevelID == 0 && objUser.IpdBillAuthLvlID == 0 && IsEditMode == true && (dgBillList.SelectedItem as clsBillVO).IsApproved == false)
                                {
                                    cmbConcessionReason.IsEnabled = true;
                                    //  txtApprovalRemark.IsEnabled = true;
                                }
                                else if ((dgBillList.SelectedItem as clsBillVO).LevelID == 0 && (dgBillList.SelectedItem as clsBillVO).IsApproved == false)
                                {
                                    cmbConcessionReason.IsEnabled = false;
                                    // txtApprovalRemark.IsEnabled = false;
                                }
                            }
                        }
                        if ((MasterListItem)cmbConcessionReason.SelectedItem == null)
                        {
                            cmbConcessionReason.TextBox.SetValidation("Select Concession Reason");
                            cmbConcessionReason.TextBox.RaiseValidationError();
                            cmbConcessionReason.Focus();
                        }
                        else if (((MasterListItem)cmbConcessionReason.SelectedItem).ID == 0)
                        {
                            cmbConcessionReason.TextBox.SetValidation("Select Concession Reason");
                            cmbConcessionReason.TextBox.RaiseValidationError();
                            cmbConcessionReason.Focus();
                        }
                        else
                            cmbConcessionReason.TextBox.ClearValidationError();

                        // Addded By Bhushanp 15032017

                        if (String.IsNullOrEmpty(txtApprovalRemark.Text))
                        {
                            txtApprovalRemark.SetValidation("Please Select Remark");
                            txtApprovalRemark.RaiseValidationError();
                            txtApprovalRemark.Focus();
                        }
                        else
                            txtApprovalRemark.ClearValidationError();

                    }
                    else
                    {
                        cmbConcessionReason.TextBox.ClearValidationError();
                        txtApprovalRemark.ClearValidationError();
                        if (((clsChargeVO)dgCharges.SelectedItem).Concession <= 0)
                        {
                            cmbConcessionReason.SelectedValue = (long)0;
                            cmbConcessionReason.IsEnabled = false;
                            //  txtApprovalRemark.IsEnabled = false;
                        }
                    }

                    if (((clsChargeVO)dgCharges.SelectedItem).Concession > ((clsChargeVO)dgCharges.SelectedItem).TotalAmount)
                    {
                        ((clsChargeVO)dgCharges.SelectedItem).Concession = ((clsChargeVO)dgCharges.SelectedItem).TotalAmount;
                        string msgText = "Concession Amount Should Not Be Greater Than Total Amount " + ((clsChargeVO)dgCharges.SelectedItem).TotalAmount;
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD.Show();
                    }
                    if (IsAuditTrail)
                    {
                        LogInformation = new LogInfo();
                        LogInformation.guid = objGUID;
                        LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        LogInformation.TimeStamp = DateTime.Now;
                        LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                        LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                        LogInformation.Message = " 1 : Line Number : " //+ Convert.ToString(lineNumber)
                                                                + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                                                                + " , Concession % : " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).ConcessionPercent)
                                                                + " , Concession Amount : " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).Concession);
                        LogInfoList.Add(LogInformation);
                    }
                }
            }
            else if (e.Column.Header.ToString().Equals("Quantity"))//(e.Column.DisplayIndex == 5)//Quantity
            {
                if (dgCharges.SelectedItem != null)
                {
                    if (!((clsChargeVO)dgCharges.SelectedItem).Quantity.ToString().IsNumberValid())
                    {
                        string msgText = "Decimal Value Not Allowed In Quantity Field ";
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD.Show();
                        ((clsChargeVO)dgCharges.SelectedItem).Quantity = Math.Ceiling(((clsChargeVO)dgCharges.SelectedItem).Quantity);
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 2 : Line Number : "
                                                                    + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                                                                    + " , Service Code : " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).ServiceId)
                                                                    + " , Service Name : " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).ServiceName)
                                                                    + " , Quantity : " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).Quantity)
                                                                    ;
                            LogInfoList.Add(LogInformation);
                        }
                    }
                }
            }
            if (e.Column.Header.ToString().Equals("Rate") | e.Column.Header.ToString().Equals("Quantity") | e.Column.Header.ToString().Equals("Concession %") | e.Column.Header.ToString().Equals("Concession Amt."))
                //(e.Column.DisplayIndex == 3 | e.Column.DisplayIndex == 4 | e.Column.DisplayIndex == 5 | e.Column.DisplayIndex == 6)
                CalculateClinicalSummary();
            fillDoctor();
            if (IsAuditTrail)
            {
                LogInformation = new LogInfo();
                LogInformation.guid = objGUID;
                LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                LogInformation.TimeStamp = DateTime.Now;
                LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                LogInformation.Message = " 3 : Line Number : "
                            + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                            + " , Service Code : " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).ServiceId)
                            + " , Service Name : " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).ServiceName)
                            + " , Rate : " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).Rate)
                            + " , Quantity : " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).Quantity)
                            + " , Concession % : " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).ConcessionPercent)
                            + " , Concession Amount : " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).Concession)
                            + " , Service Tax %: " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).ServiceTaxPercent)
                            + " , Service Tax Amount : " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).ServiceTaxAmount)
                            + " , Total Amount : " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).TotalAmount)
                            + " , Net Amount : " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).NetAmount)
                            + " , Doctor ID : " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).DoctorId);
                LogInfoList.Add(LogInformation);
            }
        }

        void fillreason()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ReasonMaster;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbConcessionReason.ItemsSource = null;
                    cmbConcessionReason.ItemsSource = objList;
                    cmbConcessionReason.SelectedValue = (long)0;
                    if (IsFromRequestApproval == true && SelectedBill != null)
                    {
                        cmbConcessionReason.SelectedValue = SelectedBill.ConcessionReasonId;
                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        void frmBill_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoaded)
            {
                if (IsPatientExist == false)
                {
                    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                    //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                    frmAdmissionList _AdmissionListObject = new frmAdmissionList();
                    ((IApplicationConfiguration)App.Current).OpenMainContent(_AdmissionListObject);
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                    mElement.Text = "Admission List";
                    ////ChargeList.Clear();
                }
                SetCommandButtonState("New");
                _flip.Invoke(RotationType.Backward);
                if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
                {
                    PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                }

                if (PatientID <= 0)
                {
                    lnkAddService.IsEnabled = false;
                    lnkAddServicesFromPrescription.IsEnabled = false;
                    lnkAddItemsFromPrescription.IsEnabled = false;
                    lnkAddItems.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdNew.IsEnabled = false;

                }
                else
                {
                    InitialiseForm();
                    SetCommandButtonState("New");
                    FillPatientAdvance(false);
                    FillBillSearchList();

                }
                IsPageLoaded = true;
                if (FromVisit == true)
                {
                    SelectedBill = null;
                    IsEditMode = false;
                    rdbAgainstBill.IsEnabled = false;
                    rdbAgainstBill.IsChecked = true;
                    rdbAgainstServices.IsEnabled = false;
                    CheckAdmission(true);
                }
                FillPrintFormat();
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                {
                    CashCounterID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
                    CashCounterName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterName;
                    cmbCostingDivision.SelectedItem = CashCounterName.ToString();
                }
                else
                    CashCounterID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IPDCounterID;

                cmdApprove.IsEnabled = false;
                GetUserRights(((IApplicationConfiguration)App.Current).CurrentUser.ID);
            }
            cmbConcessionReason.IsEnabled = false;
            // txtApprovalRemark.IsEnabled = false;
            if (IsFromRequestApproval == true)
            {
                IsEditMode = true;
                InitialiseForm();
                SetCommandButtonState("Modify");
                rdbAgainstBill.IsChecked = true;
                rdbAgainstBill.IsEnabled = false;
                rdbAgainstServices.IsEnabled = false;
                // txtApprovalRemark.IsReadOnly = false;
                //txtApprovalRemark.IsEnabled = false;
                if (SelectedBill != null)
                {
                    BillID = SelectedBill.ID;
                    PatientSourceID = SelectedBill.PatientSourceId;
                    PatientTariffID = SelectedBill.TariffId;
                    FillChargeList(SelectedBill.ID, SelectedBill.UnitID, false, SelectedBill.Opd_Ipd_External_Id, SelectedBill.Opd_Ipd_External_UnitId, false);
                    FillPharmacyItemsList(SelectedBill.ID, SelectedBill.UnitID, false);
                    chkFreezBill.IsChecked = SelectedBill.IsFreezed;
                }
                _flip.Invoke(RotationType.Forward);

                cmbConcessionReason.SelectedValue = SelectedBill.ConcessionReasonId;
                txtApprovalRemark.Text = Convert.ToString(SelectedBill.ConcessionRemark.Trim());
                if (objUser.IpdBillAuthLvlID == 0 && IsEditMode == false)
                {
                    cmbConcessionReason.IsEnabled = true;
                    // txtApprovalRemark.IsEnabled = true;
                }
                else
                {
                    if (IsEditMode == true && dgBillList.SelectedItem != null)
                    {
                        if ((dgBillList.SelectedItem as clsBillVO).LevelID > 0)
                        {
                            cmbConcessionReason.IsEnabled = true;
                            // txtApprovalRemark.IsEnabled = true;
                        }
                        else if ((dgBillList.SelectedItem as clsBillVO).LevelID == 0 && (dgBillList.SelectedItem as clsBillVO).IsApproved == false)
                        {
                            cmbConcessionReason.IsEnabled = false;
                            //txtApprovalRemark.IsEnabled = false;
                        }
                    }
                }
            }
            else
                //txtApprovalRemark.IsReadOnly = true;
                // txtApprovalRemark.IsEnabled = false;

                rdbDraftBill.IsChecked = false;
            if (((IApplicationConfiguration)App.Current).SelectedIPDPatient != null)
            {
                UserControl rootPage1 = Application.Current.RootVisual as UserControl;
                TextBlock mElement1 = (TextBlock)rootPage1.FindName("SampleSubHeader");

                mElement1.Text = " : " + ((IApplicationConfiguration)App.Current).SelectedIPDPatient.FirstName +
                    " " + ((IApplicationConfiguration)App.Current).SelectedIPDPatient.LastName;
                mElement1.Text += " - " + ((IApplicationConfiguration)App.Current).SelectedIPDPatient.MRNo;
            }
        }
        clsUserRightsVO objUser = new clsUserRightsVO();
        public void GetUserRights(long UserId)
        {
            try
            {
                clsGetUserRightsBizActionVO objBizVO = new clsGetUserRightsBizActionVO();
                objBizVO.objUserRight = new clsUserRightsVO();
                objBizVO.objUserRight.UserID = UserId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Error == null && ea.Result != null)
                    {
                        objUser = ((clsGetUserRightsBizActionVO)ea.Result).objUserRight;
                    }
                };
                client.ProcessAsync(objBizVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                client = null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #region
        private void FillPrintFormat()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ReportPrintFormat;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    }
                    cmbPrintFormat.ItemsSource = null;
                    cmbPrintFormat.ItemsSource = objList;

                    cmbPrintFormat1.ItemsSource = null;
                    cmbPrintFormat1.ItemsSource = objList;

                    cmbPrintFormat.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID;
                    cmbPrintFormat1.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID;
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion


        /// <summary>
        /// Function is for Fetching Charge list of bill/patient based on parameters sent to the function
        /// You Change this Function will be same change on another duplicate function that name is FillChargeList_Duplicate()
        /// </summary>
        /// <param name="PBillID"></param>
        /// <param name="pUnitID"></param>
        /// <param name="pIsBilled"></param>
        /// <param name="pVisitID"></param>
        private void FillChargeList(long PBillID, long pUnitID, bool pIsBilled, long pAdmissionID, long pAdmissionUnitID, bool IsDefaultService)
        {
            try
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();
                clsGetChargeListBizActionVO BizAction = new clsGetChargeListBizActionVO();
                BizAction.ID = 0;
                BizAction.Opd_Ipd_External_Id = pAdmissionID;
                BizAction.Opd_Ipd_External_UnitId = pAdmissionUnitID;
                BizAction.Opd_Ipd_External = 1;
                BizAction.IsBilled = pIsBilled;
                BizAction.BillID = PBillID;
                BizAction.UnitID = pUnitID;
                BizAction.RequestTypeID = (int)RequestType.Concession;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsGetChargeListBizActionVO)arg.Result).List != null)
                        {
                            List<clsChargeVO> objList;
                            objList = ((clsGetChargeListBizActionVO)arg.Result).List;
                            PackageServiceList = new ObservableCollection<PalashDynamics.ValueObjects.clsPackageServiceDetailsVO>();
                            foreach (var item in objList)
                            {
                                if (!this.IsFromViewClick)
                                    item.InitialRate = item.Rate;
                                item.Doctor = DoctorList;
                                item.IsDefaultService = IsDefaultService;
                                if (!string.IsNullOrEmpty(Convert.ToString(item.DoctorId)))
                                {
                                    //Added By Bhushanp 17022017 use for delete service
                                    RowNum += 1;
                                    item.RowID = RowNum;
                                    if (item.DoctorId > 0)
                                    {
                                        item.SelectedDoctor = DoctorList.FirstOrDefault(p => p.ID == item.DoctorId);
                                    }
                                    else
                                    {
                                        item.SelectedDoctor = DoctorList.FirstOrDefault(p => p.ID == 0);
                                    }
                                    if (item.IsPackageConsumption == true)
                                    {
                                        item.RateEditable = false;
                                        item.IsEnable = false;
                                    }
                                }
                                ChargeList.Add(item);
                            }
                            dgCharges.Focus();
                            dgCharges.UpdateLayout();
                            CalculateClinicalSummary();
                            if (SelectedBill != null)
                            {
                                SelectedBill.ChargeDetails = objList;
                            }
                        }
                    }
                    Indicatior.Close();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {

            }
            finally
            {
                Indicatior.Close();
            }
        }

        /// <summary>
        /// Duplicate Function for freez the bill of outside in Bill List 
        /// </summary>        
        private void FillChargeList_Duplicate(long PBillID, long pUnitID, bool pIsBilled, long pAdmissionID, long pAdmissionUnitID, bool IsDefaultService)
        {
            WaitIndicator Indic = new WaitIndicator();
            try
            {

                Indic.Show();
                clsGetChargeListBizActionVO BizAction = new clsGetChargeListBizActionVO();
                BizAction.ID = 0;
                BizAction.Opd_Ipd_External_Id = pAdmissionID;
                BizAction.Opd_Ipd_External_UnitId = pAdmissionUnitID;
                BizAction.Opd_Ipd_External = 1;
                BizAction.IsBilled = pIsBilled;
                BizAction.BillID = PBillID;
                BizAction.UnitID = pUnitID;
                BizAction.RequestTypeID = (int)RequestType.Concession;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsGetChargeListBizActionVO)arg.Result).List != null)
                        {
                            List<clsChargeVO> objList;
                            objList = ((clsGetChargeListBizActionVO)arg.Result).List;
                            PackageServiceList = new ObservableCollection<PalashDynamics.ValueObjects.clsPackageServiceDetailsVO>();
                            foreach (var item in objList)
                            {
                                if (!this.IsFromViewClick)
                                    item.InitialRate = item.Rate;
                                item.Doctor = DoctorList;
                                item.IsDefaultService = IsDefaultService;
                                if (!string.IsNullOrEmpty(Convert.ToString(item.DoctorId)))
                                {
                                    //Added By Bhushanp 17022017 use for delete service
                                    RowNum += 1;
                                    item.RowID = RowNum;
                                    if (item.DoctorId > 0)
                                    {
                                        item.SelectedDoctor = DoctorList.FirstOrDefault(p => p.ID == item.DoctorId);
                                    }
                                    else
                                    {
                                        item.SelectedDoctor = DoctorList.FirstOrDefault(p => p.ID == 0);
                                    }
                                }
                                ChargeList.Add(item);
                            }
                            dgCharges.Focus();
                            dgCharges.UpdateLayout();
                            CalculateClinicalSummary();
                            if (SelectedBill != null)
                            {
                                SelectedBill.ChargeDetails = objList;
                            }
                        }
                    }
                    Indic.Close();
                    //Added By Bhushanp 22032017
                    string msgText = "Are You Sure \n  You Want To Freeze The Bill ?";
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWD.OnMessageBoxClosed += (args) =>
                    {
                        if (args == MessageBoxResult.Yes)
                        {
                            bool isValid;
                            isValid = CheckValidations();
                            if (isValid)
                            {
                                chkFreezBill.IsChecked = true;
                                flagFreezFromSearch = true;
                                PaymentWindow paymentWin = new PaymentWindow();
                                if (!(txtPayAmount.Text == null) && !(txtPayAmount.Text.Trim().Length == 0))
                                    paymentWin.TotalAmount = double.Parse(txtPayAmount.Text);
                                paymentWin.Initiate("Bill");
                                paymentWin.txtPayTotalAmount.Text = this.txtNetAmount.Text;
                                paymentWin.txtDiscAmt.Text = this.txtTotalConcession.Text;
                                paymentWin.txtPayableAmt.Text = this.txtPayAmount.Text;
                                paymentWin.conID = ((clsBillVO)dgBillList.SelectedItem).ConcessionAuthorizedBy;
                                paymentWin.PatientCategoryID = PatientCategoryID;
                                paymentWin.Opd_Ipd_External = 1;
                                paymentWin.PackageConcenssionAmt = PackageConcenssion;
                                if ((Convert.ToDouble(this.txtTotalConcession.Text) - PackageConcenssion) > 0.0)
                                {
                                    paymentWin.ConcessionFromPlan = false;
                                }
                                else
                                {
                                    paymentWin.ConcessionFromPlan = true;
                                }
                                if (rdbAgainstBill.IsChecked == true)
                                {
                                    paymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstBill;
                                }
                                else
                                    paymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstServices;
                                paymentWin.OnSaveButton_Click += new RoutedEventHandler(paymentWin_OnSaveButton_Click);
                                paymentWin.OnCancelButton_Click += new RoutedEventHandler(paymentWin_OnCancelButton2_Click);
                                paymentWin.Show();
                            }
                            else
                            {
                                InitialiseForm();
                                ((clsBillVO)dgBillList.SelectedItem).IsFreezed = false;
                            }
                        }
                        else
                        {
                            InitialiseForm();
                            ((clsBillVO)dgBillList.SelectedItem).IsFreezed = false;
                        }
                    };
                    msgWD.Show();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                Indic.Close();
            }
            finally
            {
                Indicatior.Close();
            }
        }

        private void GetPackageConditionalServicesAndRelations(List<clsChargeVO> objList)
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            try
            {
                long MainTariffID = 0;
                long MainServiceID = 0;
                double TotalORUsedQuantity = 0;
                foreach (clsChargeVO itemObj in objList)
                {
                    MainTariffID = itemObj.TariffId;
                    MainServiceID = itemObj.ServiceId;
                }
                clsGetPackageConditionalServicesNewBizActionVO BizAction = new clsGetPackageConditionalServicesNewBizActionVO();
                BizAction.ServiceConditionList = new List<clsServiceMasterVO>();
                BizAction.TariffID = MainTariffID;
                BizAction.ServiceID = MainServiceID;
                BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                BizAction.PatientDateOfBirth = ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth;
                BizAction.MemberRelationID = ((IApplicationConfiguration)App.Current).SelectedPatient.MemberRelationID; ;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((clsGetPackageConditionalServicesNewBizActionVO)arg.Result).ServiceConditionList != null)
                    {
                        clsGetPackageConditionalServicesNewBizActionVO result = arg.Result as clsGetPackageConditionalServicesNewBizActionVO;
                        DataList.TotalItemCount = result.ServiceConditionList.Count;  // result.TotalRows;
                        DataList.Clear();
                        if (result.ServiceConditionList != null)
                        {
                            foreach (var item in result.ServiceConditionList)
                            {
                                if (MainServiceID == item.PackageServiceID && item.ConditionType == "OR")
                                {
                                    item.TotalORUsedQuantity += item.ConditionalUsedQuantity;
                                    TotalORUsedQuantity = item.TotalORUsedQuantity;
                                }
                            }
                            FillDefaultServiceRateAndConcession(objList, TotalORUsedQuantity);
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                    Indicatior.Close();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
            }
            catch (Exception)
            {
                Indicatior.Close();
            }
            finally
            {
                Indicatior.Close();
            }
        }

        private void FillDefaultServiceRateAndConcession(List<clsChargeVO> objList, double TotalORUsedQuantity)
        {
            List<clsServiceMasterVO> lServices = new List<clsServiceMasterVO>();
            #region clsApplyPackageDiscountRateOnServiceBizActionVO
            clsApplyPackageDiscountRateOnServiceBizActionVO objApplyNewRate = new clsApplyPackageDiscountRateOnServiceBizActionVO();
            objApplyNewRate.objApplyPackageDiscountRate = new List<clsApplyPackageDiscountRateOnServiceVO>();
            objApplyNewRate.ipServiceList = new List<clsServiceMasterVO>();
            objApplyNewRate.ipLoginUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            objApplyNewRate.ipPatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            objApplyNewRate.ipPatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            objApplyNewRate.ipVisitID = AdmissionID;
            objApplyNewRate.ipPatientGenderID = ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID;
            objApplyNewRate.ipPatientDateOfBirth = ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth;
            objApplyNewRate.MemberRelationID = ((IApplicationConfiguration)App.Current).SelectedPatient.MemberRelationID;
            foreach (clsChargeVO itemObj in objList)
            {
                clsServiceMasterVO itemlServices = new clsServiceMasterVO();
                itemlServices.TariffID = itemObj.TariffId;
                itemlServices.ID = itemObj.ServiceId;
                lServices.Add(itemlServices);
            }
            objApplyNewRate.ipServiceList = lServices;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    objApplyNewRate = (clsApplyPackageDiscountRateOnServiceBizActionVO)arg.Result;
                    foreach (clsChargeVO itemCharge in ChargeList)
                    {
                        foreach (clsApplyPackageDiscountRateOnServiceVO item1 in objApplyNewRate.objApplyPackageDiscountRate)
                        {
                            if (itemCharge.ServiceId == item1.ServiceID && itemCharge.TariffId == item1.TariffID)
                            {
                                if (item1.IsApplyOn_Rate_Percentage == 1 || item1.IsApplyOn_Rate_Percentage == 3)
                                {
                                    itemCharge.Rate = Convert.ToDouble(item1.DiscountedRate);
                                    if ((item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0) && item1.TariffID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID)
                                    {
                                        itemCharge.Concession = 0;
                                        itemCharge.ConcessionPercent = item1.ConcessionPercentage;
                                    }
                                    if (item1.IsDiscountOnQuantity == true || item1.ServiceMemberRelationID == 0)
                                    {
                                        itemCharge.ConcessionPercent = 0;
                                        itemCharge.Concession = 0;
                                    }
                                }
                                if (item1.IsApplyOn_Rate_Percentage == 2 || item1.IsApplyOn_Rate_Percentage == 3)
                                {
                                    if (item1.IsDiscountOnQuantity == false)
                                    {
                                        if (item1.ActualQuantity > (item1.UsedQuantity + TotalORUsedQuantity) && item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0)
                                        {
                                            itemCharge.ConcessionPercent = Convert.ToDouble(item1.DiscountedPercentage);

                                            ConcessionFromPlan = true;
                                        }
                                        else
                                        {
                                            if ((item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0) && item1.TariffID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID)
                                            {
                                                itemCharge.Concession = 0;
                                                itemCharge.ConcessionPercent = item1.ConcessionPercentage;
                                            }
                                            if ((item1.IsAgeApplicable == false || item1.ServiceMemberRelationID == 0))  //item1.ActualQuantity > 0 &&
                                            {
                                                itemCharge.ConcessionPercent = 0;
                                                itemCharge.Concession = 0;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (item1.ActualQuantity > (item1.UsedQuantity + TotalORUsedQuantity) && item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0)
                                        {
                                            if ((item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0) && item1.TariffID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID)
                                            {
                                                itemCharge.Concession = 0;
                                                itemCharge.ConcessionPercent = item1.ConcessionPercentage;
                                            }
                                        }
                                        else
                                        {
                                            itemCharge.ConcessionPercent = 0;
                                            itemCharge.Concession = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    dgCharges.Focus();
                    dgCharges.UpdateLayout();
                    CalculateClinicalSummary();
                    if (SelectedBill != null)
                    {
                        SelectedBill.ChargeDetails = objList;
                    }
                }
            };
            client.ProcessAsync(objApplyNewRate, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
            #endregion
        }


        /// <summary>
        /// Function is for Fetching Pharmacy Items list of bill/patient based on parameters sent to the function
        /// </summary>
        /// <param name="pBillID"></param>
        /// <param name="pBillUnitID"></param>
        /// <param name="pIsBilled"></param>
        private void FillPharmacyItemsList(long pBillID, long pBillUnitID, bool pIsBilled)
        {
            try
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();
                clsGetItemSalesCompleteBizActionVO BizAction = new clsGetItemSalesCompleteBizActionVO();
                BizAction.BillID = pBillID;
                BizAction.IsBilled = pIsBilled;
                BizAction.BillUnitID = pBillUnitID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsGetItemSalesCompleteBizActionVO)arg.Result).Details != null)
                        {
                            clsItemSalesVO mobj = ((clsGetItemSalesCompleteBizActionVO)arg.Result).Details;
                            StoreID = mobj.StoreID;
                            foreach (var item in mobj.Items)
                            {
                                PharmacyItems.Add(item);
                            }
                            dgPharmacyItems.Focus();
                            dgPharmacyItems.UpdateLayout();
                            CalculatePharmacySummary();
                            if (SelectedBill != null) SelectedBill.PharmacyItems = mobj;
                        }
                    }
                    Indicatior.Close();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Indicatior.Close();
            }
        }

        public long VisitDoctorID { get; set; }
        public string VisitDoctor { get; set; }
        long CurrentVisitTypeID = 0;

        /// <summary>
        /// Function is for fetching Visit Details.
        /// </summary>
        private void GetVisitDetails()
        {
            clsUpdateCurrentVisitStatusBizActionVO updateVisit = new clsUpdateCurrentVisitStatusBizActionVO();
            updateVisit.VisitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmID;
            updateVisit.CurrentVisitStatus = PalashDynamics.ValueObjects.VisitCurrentStatus.Billing;
            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
            Client1.ProcessCompleted += (s1, arg1) =>
            {
                if (arg1.Error == null && arg1.Result != null)
                {
                }
            };
            Client1.ProcessAsync(updateVisit, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client1.CloseAsync();
            clsGetVisitBizActionVO BizAction = new clsGetVisitBizActionVO();
            BizAction.Details.ID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetVisitBizActionVO)arg.Result).Details != null)
                    {
                        VisitDoctorID = ((clsGetVisitBizActionVO)arg.Result).Details.DoctorID;
                        VisitDoctor = ((clsGetVisitBizActionVO)arg.Result).Details.ReferredDoctor;
                        CurrentVisitTypeID = ((clsGetVisitBizActionVO)arg.Result).Details.VisitTypeID;
                        CheckVisitType(CurrentVisitTypeID);
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 12 : Visit Details " //+ Convert.ToString(lineNumber)
                                                                    + "Visit Doctor ID : " + Convert.ToString(VisitDoctorID)
                                                                    + " , Visit Doctor : " + Convert.ToString(VisitDoctor)
                                                                    + " , Current Visit Type ID : " + Convert.ToString(CurrentVisitTypeID);
                            LogInfoList.Add(LogInformation);
                        }
                    }
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void CheckVisitType(long pVisitTypeID)
        {
            clsGetVisitTypeBizActionVO BizAction = new clsGetVisitTypeBizActionVO();
            BizAction.ID = pVisitTypeID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    if (((clsGetVisitTypeBizActionVO)e.Result).List != null)
                    {
                        clsVisitTypeVO objVO = new clsVisitTypeVO();
                        objVO = ((clsGetVisitTypeBizActionVO)e.Result).List[0];
                        if (objVO.IsClinical == false)
                        {
                            tabClinicalInfo.IsEnabled = false;
                            tabPharmacyInfo.IsSelected = true;
                        }
                    }
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        #region Costing Divisions for Clinical & Pharmacy Billing

        //private void FillCostingDivisions()
        //{
        //    try
        //    {
        //        //Indicatior = new WaitIndicator();
        //        //Indicatior.Show();

        //        clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //        //BizAction.IsActive = true;
        //        BizAction.MasterTable = MasterTableNameList.M_IVFCostingDivision;
        //        BizAction.MasterList = new List<MasterListItem>();

        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        Client.ProcessCompleted += (s, e) =>
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();

        //            objList.Add(new MasterListItem(0, "- Select -"));

        //            if (e.Error == null && e.Result != null)
        //            {

        //                objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);


        //            }

        //            cmbCostingDivision.ItemsSource = null;
        //            cmbCostingDivision.ItemsSource = objList.DeepCopy();

        //            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations != null)
        //            {
        //                cmbCostingDivision.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.ClinicalCostingDivisionID;
        //            }

        //            //Indicatior.Close();
        //        };

        //        Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //        Client.CloseAsync();

        //    }
        //    catch (Exception)
        //    {

        //        // throw;
        //        Indicatior.Close();
        //        throw;
        //    }

        //}

        # endregion

        private void cmdPayment_Click(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Checks & assigns validations for the controls on the form
        /// </summary>
        /// <returns></returns>
        private bool CheckValidations()
        {
            bool isValid = true;
            if (PharmacyItems.Count == 0 && ChargeList.Count <= 0) // Only When Clinic Bill Then  BillTypes.Clinical == BillTypes.Clinical
            {
                isValid = false;
                string msgText = "";
                if (IsEditMode == false)
                {
                    msgText = "You Can Not Save The Bill Without Services";
                }
                else
                {
                    msgText = "You Can Not Update The Bill Without Services";
                }
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWD.Show();
            }

            #region
            //Added By Bhushanp 20042017 

            if (Convert.ToDecimal(txtClinicalConcession.Text) > 0)
            {
                var _ObjChargeList = ChargeList.Where(s => s.PackageID == 0 && s.Concession > 0).ToList();
                if (_ObjChargeList.Count > 0)
                {
                    if (((MasterListItem)cmbConcessionReason.SelectedItem).ID == 0)
                    {
                        if (objUser.IpdBillAuthLvlID == 0 && IsEditMode == false)
                        {
                            cmbConcessionReason.IsEnabled = true;
                            //txtApprovalRemark.IsEnabled = true;
                        }
                        else
                        {
                            if (IsEditMode == true && dgBillList.SelectedItem != null)
                            {
                                if ((dgBillList.SelectedItem as clsBillVO).LevelID > 0)
                                {
                                    cmbConcessionReason.IsEnabled = true;
                                    // txtApprovalRemark.IsEnabled = true;
                                }
                                else if ((dgBillList.SelectedItem as clsBillVO).LevelID == 0 && (dgBillList.SelectedItem as clsBillVO).IsApproved == false && IsEditMode == false)//Addeed yogesh K-->&& IsEditMode == true
                                {
                                    cmbConcessionReason.IsEnabled = false;
                                    // txtApprovalRemark.IsEnabled = false;
                                }

                                else if ((dgBillList.SelectedItem as clsBillVO).LevelID == 0 && (dgBillList.SelectedItem as clsBillVO).IsApproved == false && IsEditMode == true)//Added yogesh
                                {
                                    cmbConcessionReason.IsEnabled = true;         //Added Yogesh
                                    // txtApprovalRemark.IsEnabled = true;
                                }
                            }
                        }

                        if ((MasterListItem)cmbConcessionReason.SelectedItem == null)
                        {
                            isValid = false;
                            cmbConcessionReason.TextBox.SetValidation("Concession Reason Is Required");
                            cmbConcessionReason.TextBox.RaiseValidationError();
                            cmbConcessionReason.Focus();
                            return isValid;
                        }
                        else if (((MasterListItem)cmbConcessionReason.SelectedItem).ID == 0)
                        {
                            isValid = false;
                            cmbConcessionReason.TextBox.SetValidation("Concession Reason  Is Required");
                            cmbConcessionReason.TextBox.RaiseValidationError();
                            cmbConcessionReason.Focus();
                            return isValid;
                        }
                        else
                        {
                            cmbConcessionReason.TextBox.ClearValidationError();
                            isValid = true;
                        }
                    }
                }
                // Addded By bhushanp 15032017
                if (String.IsNullOrEmpty(txtApprovalRemark.Text.Trim()))
                {
                    txtApprovalRemark.SetValidation("Please Select Remark");
                    txtApprovalRemark.RaiseValidationError();
                    txtApprovalRemark.Focus();
                    isValid = false;
                    return isValid;
                }
                else if ((txtApprovalRemark.Text.Trim()).Length < 3)
                {
                    txtApprovalRemark.SetValidation("Minimum Remark Length Should be 60 Character");
                    txtApprovalRemark.RaiseValidationError();
                    txtApprovalRemark.Focus();
                    isValid = false;
                }
                else
                {
                    txtApprovalRemark.ClearValidationError();
                    isValid = true;
                }

            }


            #endregion

            foreach (var item in PharmacyItems)
            {
                if (item.Quantity > item.AvailableQuantity)
                {
                    isValid = false;
                    string msgText = "Available Quantity For " + item.ItemName + " Is Less Than Specified Quantity. Specified Quantiry Is " + item.Quantity.ToString() + " And Available Quantity Is " + item.AvailableQuantity.ToString();

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD.Show();
                    break;
                }
                if (item.Quantity <= 0)
                {
                    isValid = false;
                    string msgText = "Can Not Save Item " + item.ItemName + " With Zero Quantity ";
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD.Show();
                    break;
                }

                if (item.MRP <= 0)
                {
                    isValid = false;
                    string msgText = "Can Not Save Item " + item.ItemName + " With Zero MRP. ";
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD.Show();
                    break;
                }
            }
            //Added By Bhushanp 
            var _chargeList = ChargeList.Where(x => x.DoctorId == 0).ToList();
            if (_chargeList.Count > 0)
            {
                isValid = false;
                string msgText = "Please Select Doctor.";
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWD.Show();
            }

            return isValid;
        }


        /// <summary>
        /// This method check the mandatory field and validate the page.
        /// If page is validated then it Opens PaymentWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        double BillWiseCon = 0;
        bool ConcessionFromPlan = false;

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;
            cmdSave.IsEnabled = false;
            isValid = CheckValidations();
            if (isValid)
            {
                if (chkFreezBill.IsChecked == true)
                {
                    isValid = false;
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForConcessionID > 0)
                    {

                        if (ApprovalConcenssion > 0 && ApprovalTotal > 0)
                        {
                            if ((objUser.IpdBillingAmmount == 0 ? true : objUser.IpdBillingAmmount < Convert.ToDecimal(ApprovalConcenssion)) || (objUser.IpdBillingPercentage == 0 ? true : (((Convert.ToDecimal(ApprovalConcenssion) / Convert.ToDecimal(ApprovalTotal)) * 100)) > objUser.IpdBillingPercentage))//objUser.OpdBillingAmmount < Convert.ToDecimal(txtClinicalConcession.Text)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Concession Exceeds The Authorised Amount,Are You Sure You Want To Send For Approval?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                msgWD_1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWD_1_OnMessageBoxClosed);
                                msgWD_1.Show();
                            }
                        }
                        else
                        {
                            SaveFreezBills();
                        }
                    }
                    else
                    {
                        SaveFreezBills();
                    }
                }
                else
                {
                    SaveBill(null, false);
                }
            }
            else
                cmdSave.IsEnabled = true;
        }
        private void msgWD_1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                SendRequest(null, false);
            }
            else
            {
                SaveBill(null, false);
            }
        }
        private void SendRequest(clsPaymentVO pPayDetails, bool pFreezBill)
        {
            if (chkFreezBill.IsChecked == true)
            {

                var results2 = from r in ChargeList
                               orderby r.NetAmount ascending
                               select r;

                ChargeList = new ObservableCollection<clsChargeVO>();
                foreach (var item in results2.ToList())
                {
                    item.Doctor = null;//added By Bhushanp 25042017
                    ChargeList.Add(item);
                }
                double Con = BillWiseCon;
                foreach (var item in ChargeList)
                {

                    double Net = item.NetAmount;
                    double TotalConcession = item.Concession;

                    if (item.ConcessionPercent < 100)
                    {
                        if (Con > 0)
                        {
                            if (Con >= Net)
                            {
                                item.Concession = item.Concession + item.NetAmount;
                                item.NetAmount = 0;
                                Con = Con - Net;
                            }
                            else
                            {
                                double usedCon = 0;
                                usedCon = Con;
                                item.Concession = item.Concession + Con;
                                item.NetAmount = Net - item.Concession;
                                Con = Con - usedCon;
                            }
                        }
                    }
                }
                double TotalAmt = 0;
                if (pPayDetails != null)
                    TotalAmt = pPayDetails.PaidAmount;
                double ConsumeAmt = 0;

                var results = from r in ChargeList
                              orderby r.NetAmount descending
                              select r;

                ChargeList = new ObservableCollection<clsChargeVO>();

                foreach (var item in results.ToList())
                {
                    if (item.ChildPackageService == false)
                    {
                        if (TotalAmt > 0)
                        {
                            ConsumeAmt = item.NetAmount;
                            if (TotalAmt >= ConsumeAmt)
                            {
                                TotalAmt = TotalAmt - ConsumeAmt;
                            }
                            else
                            {
                                ConsumeAmt = TotalAmt;
                                TotalAmt = TotalAmt - ConsumeAmt;
                            }

                            item.ServicePaidAmount = ConsumeAmt;
                            item.BalanceAmount = item.NetAmount - item.ServicePaidAmount;
                            item.ChkID = true;
                        }
                        else
                        {
                            item.BalanceAmount = item.NetAmount;
                        }

                        var _List = from obj in results.ToList()
                                    where (obj.PackageID.Equals(item.PackageID) && obj.ChildPackageService == true)
                                    select obj;

                        double PaidAmount = item.ServicePaidAmount;
                        foreach (var Obj in _List)
                        {
                            clsChargeVO clschargeObject = (clsChargeVO)Obj;
                            if (PaidAmount > 0)
                            {
                                ConsumeAmt = Obj.NetAmount;
                                if (PaidAmount >= ConsumeAmt)
                                {
                                    PaidAmount = PaidAmount - ConsumeAmt;
                                }
                                else
                                {
                                    ConsumeAmt = PaidAmount;
                                    PaidAmount = PaidAmount - ConsumeAmt;
                                }

                                Obj.ServicePaidAmount = ConsumeAmt;
                                Obj.BalanceAmount = Obj.NetAmount - Obj.ServicePaidAmount;
                                Obj.ChkID = true;
                            }
                            else
                            {
                                Obj.BalanceAmount = Obj.NetAmount;
                            }
                        }
                    }
                    ChargeList.Add(item);
                }
            }
            double TotCon = 0;
            double TotAmt = 0;
            double TotPaid = 0;

            foreach (var item in ChargeList)
            {
                TotCon = TotCon + item.Concession;
                TotAmt = TotAmt + item.NetAmount;
            }

            Indicatior = new WaitIndicator();
            Indicatior.Show();
            try
            {
                clsBillVO objBill = new clsBillVO();

                if (SelectedBill == null)
                {
                    objBill.Date = DateTime.Now;
                    objBill.Time = DateTime.Now;
                    IsEditMode = false;
                    objBill.Opd_Ipd_External_Id = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmID;
                    objBill.Opd_Ipd_External_UnitId = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmissionUnitID;
                    objBill.Opd_Ipd_External = 1;
                    objBill.CompanyId = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
                    objBill.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    objBill.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                }
                else
                {
                    objBill = SelectedBill.DeepCopy();
                    IsEditMode = true;
                }

                if (pFreezBill)
                {
                    objBill.Date = DateTime.Now;
                    objBill.Time = DateTime.Now;
                    if (pPayDetails != null)
                    {
                        objBill.BalanceAmountSelf = TotAmt - pPayDetails.PaidAmount;
                        objBill.BillPaymentType = pPayDetails.BillPaymentType;
                    }

                    if (!string.IsNullOrEmpty(txtPayAmount.Text))
                        objBill.SelfAmount = Convert.ToDouble(txtPayAmount.Text);
                }

                if (PatientSourceID > 0)
                {
                    objBill.PatientSourceId = PatientSourceID;
                }
                else
                {
                    objBill.PatientSourceId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;

                }
                if (PatientTariffID > 0)
                {
                    objBill.TariffId = PatientTariffID;
                }
                else
                {
                    objBill.TariffId = ((IApplicationConfiguration)App.Current).SelectedPatient.TariffID;
                }

                if (PatientCategoryID > 0)
                {
                    objBill.PatientCategoryId = Convert.ToInt64(PatientCategoryID);
                }
                else
                {
                    objBill.PatientCategoryId = ((IApplicationConfiguration)App.Current).SelectedPatient.NewPatientCategoryID;
                }
                if (PatientCompanyID > 0)
                {
                    objBill.CompanyId = Convert.ToInt64(PatientCompanyID);
                }
                else
                {
                    objBill.CompanyId = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
                }
                objBill.IsFreezed = pFreezBill;
                objBill.PaymentDetails = pPayDetails;
                if (!string.IsNullOrEmpty(txtTotalBill.Text))
                    objBill.TotalBillAmount = Convert.ToDouble(txtTotalBill.Text);

                if (!string.IsNullOrEmpty(txtTotalConcession.Text))
                    objBill.TotalConcessionAmount = Convert.ToDouble(txtTotalConcession.Text);
                TotalConcession = objBill.TotalConcessionAmount;

                if (!string.IsNullOrEmpty(txtNetAmount.Text))
                    objBill.NetBillAmount = Convert.ToDouble(txtNetAmount.Text);

                if (cmbConcessionReason.SelectedItem != null)
                    objBill.ConcessionReasonId = ((MasterListItem)cmbConcessionReason.SelectedItem).ID;//Added By Yogesh K

                objBill.TotalConcessionAmount = TotCon;
                TotalConcession = objBill.TotalConcessionAmount;
                objBill.NetBillAmount = TotAmt;
                //Added by Bhushanp 09032017
                objBill.ConcessionRemark = Convert.ToString(txtApprovalRemark.Text.Trim());

                if (ChargeList != null & ChargeList.Count > 0)
                {
                    objBill.ChargeDetails = ChargeList.ToList();
                    if (pFreezBill || !pFreezBill)
                    {
                        for (int i = 0; i < ChargeList.Count; i++)
                        {
                            clsChargeVO objCharge = new clsChargeVO();
                            objCharge = ChargeList[i];
                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                                objCharge.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                            else
                                objCharge.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IPDCounterID;
                            if (objCharge.ServiceSpecilizationID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathoSpecializationID)
                            {
                                objBill.PathoWorkOrder = new clsPathOrderBookingVO();
                                objBill.PathoWorkOrder.DoctorID = objCharge.SelectedDoctor.ID;
                                objBill.PathoWorkOrder.PathoSpecilizationID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathoSpecializationID;
                                objBill.PathoWorkOrder.Items = new List<clsPathOrderBookingDetailVO>();
                            }
                            else if (objCharge.ServiceSpecilizationID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RadiologySpecializationID)
                            {
                                objBill.RadiologyWorkOrder.OrderBookingDetails.Add(new clsRadOrderBookingDetailsVO()
                                {
                                    TariffServiceID = objCharge.TariffServiceId,
                                    ServiceID = objCharge.ServiceId,
                                    DoctorID = objCharge.SelectedDoctor.ID,
                                    IsApproved = false
                                });
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < ChargeList.Count; i++)
                        {
                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                                ChargeList[i].CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                            else
                                ChargeList[i].CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IPDCounterID;
                        }
                    }
                }

                if (PharmacyItems.Count > 0)
                {
                    objBill.PharmacyItems.Items = new List<clsItemSalesDetailsVO>();
                    objBill.PharmacyItems.VisitID = objBill.Opd_Ipd_External_Id;
                    objBill.PharmacyItems.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    objBill.PharmacyItems.PatientUnitID = objBill.Opd_Ipd_External_UnitId;
                    objBill.PharmacyItems.Date = objBill.Date;
                    objBill.PharmacyItems.Time = objBill.Time;
                    objBill.PharmacyItems.StoreID = StoreID;

                    if (!string.IsNullOrEmpty(txtPharmacyTotal.Text))
                        objBill.PharmacyItems.TotalAmount = Convert.ToDouble(txtPharmacyTotal.Text);

                    if (!string.IsNullOrEmpty(txtPharmacyConcession.Text))
                        objBill.PharmacyItems.ConcessionAmount = Convert.ToDouble(txtPharmacyConcession.Text);

                    if (!string.IsNullOrEmpty(txtPharmacyNetAmount.Text))
                        objBill.PharmacyItems.NetAmount = Convert.ToDouble(txtPharmacyNetAmount.Text);

                    objBill.PharmacyItems.Items = PharmacyItems.ToList();
                }
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                {
                    if (objBill.ChargeDetails != null && objBill.ChargeDetails.Count > 0 && objBill.PharmacyItems != null && objBill.PharmacyItems.Items != null && objBill.PharmacyItems.Items.Count > 0)
                    {
                        objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                    }
                    else if (objBill.ChargeDetails != null && objBill.ChargeDetails.Count > 0)
                    {
                        objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                    }
                    else if (objBill.PharmacyItems != null && objBill.PharmacyItems.Items != null && objBill.PharmacyItems.Items.Count > 0)
                    {
                        objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                    }
                }
                else
                {
                    objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IPDCounterID;
                }

                if (objBill.PaymentDetails != null)
                {
                    objBill.PaymentDetails.CostingDivisionID = objBill.CostingDivisionID;
                }
                clsSendRequestForApprovalVO obj = new clsSendRequestForApprovalVO();
                obj.BillDetails = new clsAddBillBizActionVO();
                obj.Details = new clsRefundVO();
                obj.ChargeList = new List<clsChargeVO>();
                foreach (var item in ChargeList)
                {
                    if (item.isPackageService == true)
                    {
                        obj.BillDetails.IsPackageBill = true;
                        obj.BillDetails.PackageID = item.PackageID;
                        break;
                    }
                    else
                    {
                        obj.BillDetails.IsPackageBill = false;
                        obj.BillDetails.PackageID = item.PackageID;
                    }
                }
                obj.Details.IsOPDBill = false;
                obj.Details.RequestTypeID = (int)RequestType.Concession;
                obj.Details.RequestType = Enum.GetName(typeof(RequestType), RequestType.Concession);
                obj.ChargeList = ChargeList.ToList().DeepCopy();
                obj.BillDetails.Details = new clsBillVO();
                obj.BillDetails.Details = objBill;
                obj.BillDetails.Details.IsIPDBill = true; //Added By Bhushanp 20012017
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        Indicatior.Close();
                        SetCommandButtonState("Save");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Request Send Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        _flip.Invoke(RotationType.Backward);
                        FillBillSearchList();
                    }
                    else
                    {
                        Indicatior.Close();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("NT8", "Error occured while adding Bill details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        cmdSave.IsEnabled = true;
                    }
                };
                Client.ProcessAsync(obj, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                cmdSave.IsEnabled = true;
                string err = ex.Message;
                throw;
            }
            finally
            {
                Indicatior.Close();
            }
        }
        private void SaveFreezBills()
        {
            string msgText = string.Empty;

            if (Convert.ToDouble(txtAdvance.Text) < double.Parse(txtPayAmount.Text)) msgText = "You do not have sufficient Advance, Are you sure you want to Freeze the Bill?";
            else msgText = "Are you sure you want to Freeze the Bill ?";
            MessageBoxControl.MessageBoxChildWindow msgWD =
            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgWD.OnMessageBoxClosed += (arg) =>
            {
                if (arg == MessageBoxResult.Yes)
                {
                    PaymentWindow paymentWin = new PaymentWindow();
                    paymentWin.TotalAmount = double.Parse(txtPayAmount.Text);
                    paymentWin.Initiate("Bill");
                    paymentWin.txtPayTotalAmount.Text = this.txtClinicalTotal.Text;
                    paymentWin.txtDiscAmt.Text = this.txtTotalConcession.Text;
                    paymentWin.txtPayableAmt.Text = this.txtPayAmount.Text;
                    paymentWin.PackageConcenssionAmt = PackageConcenssion;
                    paymentWin.PatientCategoryID = PatientCategoryID;
                    paymentWin.Opd_Ipd_External = 1;
                    if ((Convert.ToDouble(this.txtTotalConcession.Text) - PackageConcenssion) > 0.0)
                    {
                        paymentWin.ConcessionFromPlan = false;
                    }
                    else
                    {
                        paymentWin.ConcessionFromPlan = true;
                    }
                    if (rdbAgainstBill.IsChecked == true)
                        paymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstBill;
                    else
                        paymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstServices;

                    # region Added on 07Feb2019 for Package Flow in IPD

                    if (ChargeList != null && ChargeList.Count > 0)
                    {
                        paymentWin.PackageID = ChargeList.Select(z => z.PackageID).FirstOrDefault();
                        paymentWin.PackageBillID = ChargeList.Select(z => z.PackageBillID).FirstOrDefault();
                        paymentWin.PackageBillUnitID = ChargeList.Select(z => z.PackageBillUnitID).FirstOrDefault();

                        if (paymentWin.PackageID > 0)
                        {
                            paymentWin.txtPackagePayableAmt.Text = PackageConcenssion.ToString();
                            paymentWin.lblPackagePayableAmt.Visibility = Visibility.Visible;
                            paymentWin.txtPackagePayableAmt.Visibility = Visibility.Visible;
                        }
                    }

                    if (ChargeList != null && ChargeList.Count > 0)
                    {
                        var items12 = from r in ChargeList
                                      where r.isPackageService == true
                                      select r;

                        if (items12.ToList().Count > 0)
                        {
                            paymentWin.IsNoAutoAdvanceConsume = true;       // Set to call auto collect advance or not : added on 22062018
                        }
                    }

                    #endregion

                    paymentWin.OnSaveButton_Click += new RoutedEventHandler(paymentWin_OnSaveButton_Click);
                    paymentWin.OnCancelButton_Click += new RoutedEventHandler(paymentWin_OnCancelButton_Click);
                    paymentWin.Show();
                }
                else
                    cmdSave.IsEnabled = true;
            };
            msgWD.Show();
        }

        /// <summary>
        /// Creates instance of clsAddBillBizActionVO class,assign the details to it 
        /// and call service ProcessAsync() method for adding Bill Details. 
        /// </summary>
        /// <param name="pPayDetails"></param>
        /// <param name="pFreezBill"></param>
        private void SaveBill(clsPaymentVO pPayDetails, bool pFreezBill)
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            #region Log
            if (pPayDetails != null)
            {
                if (IsAuditTrail)
                {
                    LogInformation = new LogInfo();   // Origional data from payment window
                    LogInformation.guid = objGUID;
                    LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    LogInformation.TimeStamp = DateTime.Now;
                    LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                    LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                    LogInformation.Message = " 4 : Origional Data From Payment Window " //+ Convert.ToString(lineNumber)
                                                            + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                                                            + "Is Freez :" + Convert.ToString(pFreezBill)
                                                            + " , Advanced Amount : " + Convert.ToInt32(pPayDetails.AdvanceAmount)
                                                            + " , Advanced ID : " + Convert.ToString(pPayDetails.AdvanceID)
                                                            + " , Advanced Used : " + Convert.ToString(pPayDetails.AdvanceUsed)
                                                            + " , Bill Amount : " + Convert.ToString(pPayDetails.BillAmount)
                                                            + " , Bill Balance Amount : " + Convert.ToString(pPayDetails.BillBalanceAmount)
                                                            + " , Bill Payment Type : " + Convert.ToString(pPayDetails.BillPaymentType)
                                                            + " , Cheque Authorised By: " + Convert.ToString(pPayDetails.ChequeAuthorizedBy)
                                                            + " , Inovice ID : " + Convert.ToString(pPayDetails.InvoiceID)
                                                            + " , Inovice Unit ID : " + Convert.ToString(pPayDetails.InvoiceUnitID)
                                                            + " , Is Bill Settltment : " + Convert.ToString(pPayDetails.IsBillSettlement)
                                                            + " , Paid Amount : " + Convert.ToString(pPayDetails.PaidAmount)
                                                            + " , Refund Amount : " + Convert.ToString(pPayDetails.RefundAmount)
                                                            + " , Settltment Concession Amount : " + Convert.ToString(pPayDetails.SettlementConcessionAmount);
                    foreach (clsPaymentDetailsVO item in pPayDetails.PaymentDetails)
                    {
                        LogInformation.Message = LogInformation.Message + ",Bank ID :" + Convert.ToString(item.BankID)
                                                                        + ",Cheq Cleared :" + Convert.ToString(item.ChequeCleared)
                                                                        + ",Date :" + Convert.ToString(item.Date)
                                                                        + ",Number :" + Convert.ToString(item.Number)
                                                                        + ",Paid Amount :" + Convert.ToString(item.PaidAmount)
                                                                        + ",Payment Mode ID :" + Convert.ToString(item.PaymentModeID) + "\r\n"
                        ;
                    }
                    LogInfoList.Add(LogInformation);
                }
            }
            #endregion

            if (chkFreezBill.IsChecked == true)
            {

                var results2 = from r in ChargeList
                               orderby r.NetAmount ascending
                               select r;

                ChargeList = new ObservableCollection<clsChargeVO>();
                foreach (var item in results2.ToList())
                {
                    item.Doctor = null;//Added By Bhushanp 25042017
                    ChargeList.Add(item);
                }
                double Con = BillWiseCon;
                foreach (var item in ChargeList)
                {

                    double Net = item.NetAmount;
                    double TotalConcession = item.Concession;

                    if (item.ConcessionPercent < 100)
                    {
                        if (Con > 0)
                        {
                            if (Con >= Net)
                            {
                                item.Concession = item.Concession + item.NetAmount;
                                item.NetAmount = 0;
                                Con = Con - Net;
                            }
                            else
                            {
                                double usedCon = 0;
                                usedCon = Con;
                                item.Concession = item.Concession + Con;
                                item.NetAmount = Net - item.Concession;
                                Con = Con - usedCon;
                            }
                        }
                    }
                }

                double TotalAmt = 0;
                if (pPayDetails != null)
                    TotalAmt = pPayDetails.PaidAmount;

                double ConsumeAmt = 0;

                var results = from r in ChargeList
                              orderby r.NetAmount descending
                              select r;

                ChargeList = new ObservableCollection<clsChargeVO>();

                foreach (var item in results.ToList())
                {
                    if (item.ChildPackageService == false)
                    {
                        if (TotalAmt > 0)
                        {
                            ConsumeAmt = item.NetAmount;
                            if (TotalAmt >= ConsumeAmt)
                            {
                                TotalAmt = TotalAmt - ConsumeAmt;
                            }
                            else
                            {
                                ConsumeAmt = TotalAmt;
                                TotalAmt = TotalAmt - ConsumeAmt;
                            }
                            //item.ServicePaidAmount = ConsumeAmt; // Commented on on 07Feb2019 for Package Flow in IPD
                            item.ServicePaidAmount = Math.Round(ConsumeAmt, 2); // modify on 07Feb2019 for Package Flow in IPD :: for balance amount issue while pay the bill 
                            item.BalanceAmount = item.NetAmount - item.ServicePaidAmount;
                            item.ChkID = true;
                        }
                        else
                        {
                            item.BalanceAmount = item.NetAmount;
                        }
                        var _List = from obj in results.ToList()
                                    where (obj.PackageID.Equals(item.PackageID) && obj.ChildPackageService == true)
                                    select obj;


                        double PaidAmount = item.ServicePaidAmount;
                        foreach (var Obj in _List)
                        {
                            clsChargeVO clschargeObject = (clsChargeVO)Obj;

                            if (PaidAmount > 0)
                            {
                                ConsumeAmt = Obj.NetAmount;
                                if (PaidAmount >= ConsumeAmt)
                                {
                                    PaidAmount = PaidAmount - ConsumeAmt;
                                }
                                else
                                {
                                    ConsumeAmt = PaidAmount;
                                    PaidAmount = PaidAmount - ConsumeAmt;
                                }
                                //Obj.ServicePaidAmount = ConsumeAmt; // Commented on on 07Feb2019 for Package Flow in IPD
                                Obj.ServicePaidAmount = Math.Round(ConsumeAmt, 2);   // modify on 07Feb2019 for Package Flow in IPD :: for balance amount issue while pay the bill 
                                Obj.BalanceAmount = Obj.NetAmount - Obj.ServicePaidAmount;
                                Obj.ChkID = true;
                            }
                            else
                            {
                                Obj.BalanceAmount = Obj.NetAmount;
                            }
                        }
                    }
                    ChargeList.Add(item);
                }

                // Added By CDS For Adjustable Service Rate 

                //double SumTotalServriceRate = 0;
                //double AdjustableServiceRate=0;

                //var result = ChargeList.GroupBy(test => test.ServiceId)
                //   .Select(grp => grp.First())
                //   .ToList();

                //foreach (var item in result)
                //{
                //    if (item.PackageID > 0 && item.ConcessionPercent > 0 && item.IsConsiderAdjustable == true)
                //    {
                //        SumTotalServriceRate = SumTotalServriceRate + item.Rate;
                //    }
                //}

                double SumConsiderInAdjustableHead = 0;
                double SumNotInAdjHead = 0;
                double AdjustableServiceRate = 0;

                foreach (var item in ChargeList)
                {
                    if (item.PackageID > 0 && item.ConcessionPercent > 0 && item.IsConsiderAdjustable == true && item.IsInGroupSpecialization == false) // Not In Group Services
                    {
                        SumConsiderInAdjustableHead = SumConsiderInAdjustableHead + item.Rate;
                    }

                    if (item.PackageID > 0 && item.ConcessionPercent > 0 && item.IsConsiderAdjustable == true && item.IsInGroupSpecialization == true) //For Group In Services
                    {
                        SumConsiderInAdjustableHead = SumConsiderInAdjustableHead + item.ConcessionAmount;
                    }

                    if (item.PackageID > 0 && item.ConcessionPercent > 0 && item.IsConsiderAdjustable == false)
                    {
                        SumNotInAdjHead = SumNotInAdjHead + item.Rate;
                    }
                }

                foreach (var item in ChargeList)
                {
                    if (item.PackageID > 0 && item.IsAdjustableHead == true)
                    {

                        item.Rate = (Convert.ToDouble(item.ServiceComponentRate) - SumConsiderInAdjustableHead - SumNotInAdjHead - item.OPDConsumption) - (Convert.ToDouble(item.SumOfExludedServices) - (item.OpdExcludeServiceConsumption + SumNotInAdjHead));//Convert.ToDouble(item.SumOfExludedServices) - SumConsiderInAdjustableHead;
                        item.TotalAmount = Convert.ToDouble(item.ServiceComponentRate) - Convert.ToDouble(item.SumOfExludedServices) - SumConsiderInAdjustableHead;
                        item.Concession = Convert.ToDouble(item.ServiceComponentRate) - Convert.ToDouble(item.SumOfExludedServices) - SumConsiderInAdjustableHead;
                        item.ConcessionPercent = 100;
                        // AdjustableServiceRate = Convert.ToDouble(item.ServiceComponentRate) - Convert.ToDouble(item.SumOfExludedServices) - SumConsiderInAdjustableHead;
                        AdjustableServiceRate = Convert.ToDouble(item.ServiceComponentRate) - SumConsiderInAdjustableHead - SumNotInAdjHead - item.OPDConsumption - (Convert.ToDouble(item.SumOfExludedServices) - (item.OpdExcludeServiceConsumption + SumNotInAdjHead));//Convert.ToDouble(item.SumOfExludedServices) - SumConsiderInAdjustableHead;
                    }
                }
            }

            // Added By CDS 
            if (IsFromOtherServices == true)
            {
                var res = from r in ChargeList
                          where r.ID == 0
                          select r;

                foreach (var item in res.ToList())
                {
                    item.IsTemp = true;
                }
            }
            else
            {
                foreach (var item in ChargeList)
                {
                    item.IsTemp = false;
                }
            }
            //END

            double TotCon = 0;
            double TotAmt = 0;
            double TotPaid = 0;
            double TotalAmount = 0; // Added By bhushanp 18012016

            StringBuilder StrPrescriptionDetailsID = new StringBuilder();   // Added on 07Feb2019 for Package Flow in IPD
            StringBuilder StrInvestigationDetailsID = new StringBuilder();  // Added on 07Feb2019 for Package Flow in IPD

            foreach (var item in ChargeList)
            {
                item.Doctor = null;
                TotCon = TotCon + item.Concession;
                TotAmt = TotAmt + item.NetAmount;
                TotalAmount = TotalAmount + item.TotalAmount;// Added By bhushanp 18012016

                #region Added on 07Feb2019 for Package Flow in IPD
                if (StrPrescriptionDetailsID.ToString().Length > 0)
                    StrPrescriptionDetailsID.Append(",");
                StrPrescriptionDetailsID.Append(item.PrescriptionDetailsID);

                if (StrInvestigationDetailsID.ToString().Length > 0)
                    StrInvestigationDetailsID.Append(",");
                StrInvestigationDetailsID.Append(item.InvestigationDetailsID);
                #endregion
            }

            try
            {
                clsBillVO objBill = new clsBillVO();

                if (SelectedBill == null)
                {
                    objBill.Date = DateTime.Now;
                    objBill.Time = DateTime.Now;
                    IsEditMode = false;
                    objBill.Opd_Ipd_External_Id = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmID;
                    objBill.Opd_Ipd_External_UnitId = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmissionUnitID;
                    objBill.Opd_Ipd_External = 1;
                    objBill.IsDraftBill = Convert.ToBoolean(rdbDraftBill.IsChecked);
                    objBill.CompanyId = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
                    objBill.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    objBill.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                }
                else
                {
                    objBill = SelectedBill.DeepCopy();
                    IsEditMode = true;
                }

                if (pFreezBill)
                {
                    objBill.Date = DateTime.Now;
                    objBill.Time = DateTime.Now;
                    if (pPayDetails != null)
                    {
                        //objBill.BalanceAmountSelf = TotAmt - pPayDetails.PaidAmount;     // Commented on on 07Feb2019 for Package Flow in IPD
                        objBill.BalanceAmountSelf = Math.Round(TotAmt, 2) - pPayDetails.PaidAmount;    // modify on 07Feb2019 for Package Flow in IPD :: for balance amount issue while pay the bill 
                        objBill.BillPaymentType = pPayDetails.BillPaymentType;
                    }

                    if (!string.IsNullOrEmpty(txtPayAmount.Text))
                        objBill.SelfAmount = Convert.ToDouble(txtPayAmount.Text);
                }

                if (PatientSourceID > 0)
                {
                    objBill.PatientSourceId = PatientSourceID;
                }
                else
                {
                    objBill.PatientSourceId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;
                }
                if (PatientTariffID > 0)
                {
                    objBill.TariffId = PatientTariffID;
                }
                else
                {
                    objBill.TariffId = ((IApplicationConfiguration)App.Current).SelectedPatient.TariffID;
                }
                // This IS valid In Health Spring So Commented 
                if (PatientCategoryID > 0)
                {
                    objBill.PatientCategoryId = Convert.ToInt64(PatientCategoryID);
                }
                else
                {
                    objBill.PatientCategoryId = ((IApplicationConfiguration)App.Current).SelectedPatient.NewPatientCategoryID;
                }
                if (PatientCompanyID > 0)
                {
                    objBill.CompanyId = Convert.ToInt64(PatientCompanyID);
                }
                else
                {
                    objBill.CompanyId = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
                }
                //Added By Bhushanp 12/01/2017 For Draft bill cant save properly
                if (pFreezBill)
                {
                    objBill.Date = DateTime.Now;
                    objBill.Time = DateTime.Now;
                    if (pPayDetails != null)
                    {
                        objBill.BalanceAmountSelf = TotAmt - pPayDetails.PaidAmount;
                        objBill.BillPaymentType = pPayDetails.BillPaymentType;
                    }
                    if (!string.IsNullOrEmpty(txtPayAmount.Text))
                        objBill.SelfAmount = Convert.ToDouble(txtPayAmount.Text);
                }
                else
                {
                    objBill.SelfAmount = TotAmt;
                    objBill.BalanceAmountSelf = TotAmt;

                }
                objBill.IsFreezed = pFreezBill;
                objBill.PaymentDetails = pPayDetails;
                if (!string.IsNullOrEmpty(txtTotalBill.Text))
                    objBill.TotalBillAmount = Convert.ToDouble(txtTotalBill.Text);

                if (!string.IsNullOrEmpty(txtTotalConcession.Text))
                    objBill.TotalConcessionAmount = Convert.ToDouble(txtTotalConcession.Text);
                TotalConcession = objBill.TotalConcessionAmount;

                if (!string.IsNullOrEmpty(txtNetAmount.Text))
                    objBill.NetBillAmount = Convert.ToDouble(txtNetAmount.Text);

                if (cmbConcessionReason.SelectedItem != null)
                    objBill.ConcessionReasonId = ((MasterListItem)cmbConcessionReason.SelectedItem).ID;

                objBill.TotalConcessionAmount = TotCon;
                objBill.TotalBillAmount = TotalAmount;
                TotalConcession = objBill.TotalConcessionAmount;
                objBill.NetBillAmount = TotAmt;
                if (ChargeList != null & ChargeList.Count > 0)
                {
                    objBill.ChargeDetails = ChargeList.ToList();

                    if (pFreezBill || ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RadiologySpecializationID > 0 && pFreezBill == false)
                    {
                        for (int i = 0; i < ChargeList.Count; i++)
                        {
                            clsChargeVO objCharge = new clsChargeVO();
                            objCharge = ChargeList[i];
                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                                objCharge.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                            else
                                objCharge.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IPDCounterID;

                            if (objCharge.ServiceSpecilizationID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathoSpecializationID)
                            {
                                objBill.PathoWorkOrder = new clsPathOrderBookingVO();
                                objBill.PathoWorkOrder.DoctorID = objCharge.SelectedDoctor.ID;
                                objBill.PathoWorkOrder.IsExternalPatient = false;
                                objBill.PathoWorkOrder.PathoSpecilizationID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathoSpecializationID;
                                objBill.PathoWorkOrder.Items = new List<clsPathOrderBookingDetailVO>();
                            }
                            else if (objCharge.ServiceSpecilizationID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RadiologySpecializationID)
                            {
                                objBill.RadiologyWorkOrder.OrderBookingDetails.Add(new clsRadOrderBookingDetailsVO()
                                {
                                    TariffServiceID = objCharge.TariffServiceId,
                                    ServiceID = objCharge.ServiceId,
                                    ID = objCharge.ROBDID,
                                    DoctorID = objCharge.SelectedDoctor.ID,
                                    IsApproved = false
                                });
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < ChargeList.Count; i++)
                        {
                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                                ChargeList[i].CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                            else
                                ChargeList[i].CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IPDCounterID;
                        }
                    }
                    if (IsAuditTrail)
                    {
                        LogInformation = new LogInfo();
                        LogInformation.guid = objGUID;
                        LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        LogInformation.TimeStamp = DateTime.Now;
                        LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                        LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                        LogInformation.Message = " 5 : T_charge "
                                    + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId);
                        foreach (clsChargeVO obj in ChargeList.ToList())
                        {
                            LogInformation.Message = LogInformation.Message
                                            + " , Tarriff Service ID : " + Convert.ToString(obj.TariffServiceId)
                                            + " , Service ID : " + Convert.ToString(obj.ServiceId)
                                            + " , Doctor ID : " + Convert.ToString(obj.DoctorId)
                                            + " , Rate : " + Convert.ToString(obj.Rate)
                                            + " , Quantity : " + Convert.ToString(obj.Quantity)
                                            + " , Total Amount : " + Convert.ToString(obj.TotalAmount)
                                            + " , Concession % : " + Convert.ToString(obj.ConcessionPercent)
                                            + " , Concession Amount : " + Convert.ToString(obj.ConcessionAmount)
                                            + " , Service Tax %: " + Convert.ToString(obj.ServiceTaxPercent)
                                            + " , Service Tax Amount : " + Convert.ToString(obj.ServiceTaxAmount)
                                            + " , Net Amount : " + Convert.ToString(obj.NetAmount)
                                            + " , Package ID : " + Convert.ToString(obj.PackageID)
                                            + " , Paid Amount : " + Convert.ToString(obj.PaidAmount)
                                            + " , Tarriff ID : " + Convert.ToString(obj.TariffId)
                                            + " , Costing Division ID : " + Convert.ToString(obj.CostingDivisionID)
                                            + " , Service Code : " + Convert.ToString(obj.ServiceCode)
                                            + " , Service Name New: " + Convert.ToString(obj.ServiceName)
                                            + " , Initial Rate : " + Convert.ToString(obj.InitialRate)
                                            + " , Is Billed : " + Convert.ToString(obj.IsBilled)
                                            + " , Is Package : " + Convert.ToString(obj.IsPackaged)
                                            + " , Parent ID : " + Convert.ToString(obj.ParentID)
                                            + " , Condition Type ID : " + Convert.ToString(obj.ConditionTypeID)
                                            + " , Opd_ipd_external_ID : " + Convert.ToString(obj.Opd_Ipd_External_Id)
                                            + " , Opd_ipd_external_UnitID : " + Convert.ToString(obj.Opd_Ipd_External_UnitId) + "\r\n"
                                                                    ;
                        }
                        LogInfoList.Add(LogInformation);
                    }
                }
                if (PharmacyItems.Count > 0)
                {
                    objBill.PharmacyItems.Items = new List<clsItemSalesDetailsVO>();
                    objBill.PharmacyItems.VisitID = objBill.Opd_Ipd_External_Id;
                    objBill.PharmacyItems.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    objBill.PharmacyItems.PatientUnitID = objBill.Opd_Ipd_External_UnitId;
                    objBill.PharmacyItems.Date = objBill.Date;
                    objBill.PharmacyItems.Time = objBill.Time;
                    objBill.PharmacyItems.StoreID = StoreID;
                    if (!string.IsNullOrEmpty(txtPharmacyTotal.Text))
                        objBill.PharmacyItems.TotalAmount = Convert.ToDouble(txtPharmacyTotal.Text);

                    if (!string.IsNullOrEmpty(txtPharmacyConcession.Text))
                        objBill.PharmacyItems.ConcessionAmount = Convert.ToDouble(txtPharmacyConcession.Text);

                    if (!string.IsNullOrEmpty(txtPharmacyNetAmount.Text))
                        objBill.PharmacyItems.NetAmount = Convert.ToDouble(txtPharmacyNetAmount.Text);

                    objBill.PharmacyItems.Items = PharmacyItems.ToList();
                }
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                {
                    if (objBill.ChargeDetails != null && objBill.ChargeDetails.Count > 0 && objBill.PharmacyItems != null && objBill.PharmacyItems.Items != null && objBill.PharmacyItems.Items.Count > 0)
                    {
                        objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                    }
                    else if (objBill.ChargeDetails != null && objBill.ChargeDetails.Count > 0)
                    {
                        objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                    }
                    else if (objBill.PharmacyItems != null && objBill.PharmacyItems.Items != null && objBill.PharmacyItems.Items.Count > 0)
                    {
                        objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                    }
                }
                else
                {
                    objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IPDCounterID;
                }

                if (objBill.PaymentDetails != null)
                {
                    objBill.PaymentDetails.CostingDivisionID = objBill.CostingDivisionID;
                }
                clsAddBillBizActionVO BizAction = new clsAddBillBizActionVO();
                foreach (var item in ChargeList)
                {
                    if (item.isPackageService == true)
                    {
                        BizAction.IsPackageBill = true;
                        BizAction.PackageID = item.PackageID;
                        break;
                    }
                    else
                    {
                        BizAction.IsPackageBill = false;
                        BizAction.PackageID = item.PackageID;
                    }
                }
                //Added By Bhushanp 09032017
                objBill.ConcessionRemark = Convert.ToString(txtApprovalRemark.Text.Trim().ToString());

                BizAction.Details = new clsBillVO();

                BizAction.PrescriptionDetailsID = StrPrescriptionDetailsID.ToString();      // Added on 07Feb2019 for Package Flow in IPD
                BizAction.InvestigationDetailsID = StrInvestigationDetailsID.ToString();    // Added on 07Feb2019 for Package Flow in IPD

                BizAction.Details = objBill;
                BizAction.Details.IsIPDBill = true;// added By Bhushanp
                BizAction.DeletedRadSerDetailsList = (List<clsChargeVO>)DeleteBillAddedItems.ToList<clsChargeVO>();
                BizAction.LogInfoList = new List<LogInfo>();  // For the Activity Log List
                BizAction.LogInfoList = LogInfoList.DeepCopy();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((clsAddBillBizActionVO)arg.Result).Details != null)
                    {
                        if (((clsAddBillBizActionVO)arg.Result).Details != null && IsFromOtherServices == false)
                        {
                            BillID = (((clsAddBillBizActionVO)arg.Result).Details).ID;
                            UnitID = (((clsAddBillBizActionVO)arg.Result).Details).UnitID;
                            PRID = ((MasterListItem)cmbPrintFormat.SelectedItem).ID;
                            Indicatior.Close();
                            SetCommandButtonState("Save");
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Bill details saved successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.OnMessageBoxClosed += (re) =>
                            {
                                if (re == MessageBoxResult.OK)
                                {
                                    if (pFreezBill == true)
                                    {
                                        if (IsPackageConsumption)
                                        {
                                        }
                                        else if ((((clsAddBillBizActionVO)arg.Result).Details).BillType == PalashDynamics.ValueObjects.BillTypes.Clinical)
                                        {
                                            PrintBill(BillID, UnitID, 4);
                                        }
                                        else if ((((clsAddBillBizActionVO)arg.Result).Details).BillType == PalashDynamics.ValueObjects.BillTypes.Pharmacy)
                                        {
                                            if ((((clsAddBillBizActionVO)arg.Result).Details).BalanceAmountNonSelf == 0 && (((clsAddBillBizActionVO)arg.Result).Details).BalanceAmountSelf == 0)
                                                PrintPharmacyBill(BillID, UnitID, 4);
                                        }
                                        else
                                        {
                                            if ((((clsAddBillBizActionVO)arg.Result).Details).BalanceAmountNonSelf == 0 && (((clsAddBillBizActionVO)arg.Result).Details).BalanceAmountSelf == 0)
                                            {
                                                PrintBill(BillID, UnitID, 4);
                                                PrintPharmacyBill(BillID, UnitID, PRID);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //if (IsFromOtherServices == false)
                                        //{
                                        PrintBill(BillID, UnitID, 3);
                                        //}
                                    }
                                }
                            };
                            msgW1.Show();
                            _flip.Invoke(RotationType.Backward);
                            FillBillSearchList();
                        }
                        else if (((clsAddBillBizActionVO)arg.Result).Details != null && IsFromOtherServices == true)
                        {
                            IsFromOtherServices = false;
                        }
                    }
                    else
                    {
                        Indicatior.Close();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("NT9", "Error occured while adding Bill details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        cmdSave.IsEnabled = true;
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
                if (IsAuditTrail)
                {
                    LogInformation = new LogInfo();   //final data for T_Bill
                    LogInformation.guid = objGUID;
                    LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    LogInformation.TimeStamp = DateTime.Now;
                    LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                    LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                    LogInformation.Message = " 6 : T_Bill " //+ Convert.ToString(lineNumber)
                                        + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                                        + "Is Freez :" + Convert.ToString(pFreezBill)
                                        + " , Bill Date : " + Convert.ToString(objBill.Date)
                                        + " , Company ID : " + Convert.ToString(objBill.CompanyId)
                                        + " , Patient Source ID : " + Convert.ToString(objBill.PatientSourceId)
                                        + " , Patient Category ID : " + Convert.ToString(objBill.PatientCategoryId)
                                        + " , Tarrif ID : " + Convert.ToString(objBill.TariffId)
                                        + " , Total Bill Amount : " + Convert.ToString(objBill.TotalBillAmount)
                                        + " , Total Concession Amount: " + Convert.ToString(objBill.TotalConcessionAmount)
                                        + " , Net Bill Amount : " + Convert.ToString(objBill.NetBillAmount)
                                        + " , Costing Division ID : " + Convert.ToString(objBill.CostingDivisionID)
                                        + " , Self Amount : " + Convert.ToString(objBill.SelfAmount)
                                        + " , Balance Amount Self : " + Convert.ToString(objBill.BalanceAmountSelf)
                                        + " , Is Freez : " + Convert.ToString(objBill.IsFreezed)
                                        + " , Concession Reason ID: " + Convert.ToString(objBill.ConcessionReasonId)
                                        + " , Opd_Ipd_ExternalID: " + Convert.ToString(objBill.Opd_Ipd_External_Id)
                                        + " , Opd_Ipd_ExternalUnitID: " + Convert.ToString(objBill.Opd_Ipd_External_UnitId)
                                        ;
                    LogInfoList.Add(LogInformation);
                }
            }
            catch (Exception ex)
            {
                cmdSave.IsEnabled = true;
                string err = ex.Message;
                throw;
            }
            finally
            {
                Indicatior.Close();
            }
        }
        bool? IsCorporate = false;
        WaitIndicator Indicatior = null;
        void paymentWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (((CIMS.Forms.PaymentWindow)sender).DialogResult == true)
            {
                if (((PaymentWindow)sender).Payment != null)
                {
                    if (IsAuditTrail && ((PaymentWindow)sender).Payment.LogInfoList != null && ((PaymentWindow)sender).Payment.LogInfoList.Count > 0)   // By Umesh For Activity Log
                    {
                        foreach (LogInfo item in ((PaymentWindow)sender).Payment.LogInfoList)
                        {
                            LogInfoList.Add(item);
                        }
                    }
                    if (((PaymentWindow)sender).TxtConAmt.Text != null)
                    {
                        BillWiseCon = ((PaymentWindow)sender).TotalConAmount;
                    }
                    if (((PaymentWindow)sender).IsBillAsDraft == true)
                    {
                        chkFreezBill.IsChecked = false;
                        SaveBill(((PaymentWindow)sender).Payment, false);
                    }
                    else
                    {
                        rdbAgainstBill.IsChecked = true;
                        SaveBill(((PaymentWindow)sender).Payment, true);
                    }
                }
            }
            else
            {
                if (!flagFreezFromSearch)
                {
                    cmdSave.IsEnabled = true;
                }
                if (flagFreezFromSearch)
                {
                    ((clsBillVO)dgBillList.SelectedItem).IsFreezed = false;

                    chkFreezBill.IsChecked = false;
                    flagFreezFromSearch = false;
                }
            }
        }

        void paymentWin_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedBill != null)
                cmdSave.IsEnabled = false;
            else
                cmdSave.IsEnabled = true;
        }

        void paymentWin_OnCancelButton2_Click(object sender, RoutedEventArgs e)
        {
            if ((clsBillVO)dgBillList.SelectedItem != null)
            {
                ((clsBillVO)dgBillList.SelectedItem).IsFreezed = false;
            }
        }

        public bool IsCancel = true;
        public string Action { get; set; }
        public string ModuleName { get; set; }
        UIElement myData = null;
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {

            if (DeleteBillAddedItems != null)
            {
                DeleteBillAddedItems = new ObservableCollection<clsChargeVO>();
                DeleteBillAddedItems.Clear();
            }
            txtNetAmount.Text = "0";
            txtClinicalNetAmount.Text = "0";
            txtPharmacyNetAmount.Text = "0";
            cmbConcessionReason.SelectedValue = null;
            InitialiseForm();
            if (IsFromRequestApproval == true)
            {
                ModuleName = "OPDModule";
                Action = "CIMS.Forms.frmApprovalRequests";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c2 = new WebClient();
                c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                if (IsCancel == true && !IsFromViewClick)
                {
                    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                    frmAdmissionList _AdmissionListObject = new frmAdmissionList();
                    ((IApplicationConfiguration)App.Current).OpenMainContent(_AdmissionListObject);
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                    mElement.Text = "Admission List";
                }
                else
                {

                    clsBillVO objBill = new clsBillVO();
                    if (dgBillList.SelectedItem != null)
                    {
                        objBill.ID = ((clsBillVO)dgBillList.SelectedItem).ID;
                        objBill.UnitID = ((clsBillVO)dgBillList.SelectedItem).UnitID;
                    }
                    else
                    {
                        objBill.ID = BillID;
                        objBill.UnitID = UnitID;
                    }
                    objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IPDCounterID;

                    clsAddBillBizActionVO BizAction = new clsAddBillBizActionVO();

                    BizAction.Details = new clsBillVO();
                    BizAction.Details = objBill;
                    BizAction.IsFromIPDCancel = true;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null && ((clsAddBillBizActionVO)arg.Result).Details != null)
                        {
                            FillBillSearchList();
                        }
                    };
                    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client.CloseAsync();

                    SetCommandButtonState("New");
                    _flip.Invoke(RotationType.Backward);
                    IsCancel = true;
                    IsFromViewClick = false;
                }
            }
            FillBillSearchList();
        }
        void c2_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
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
                ((IApplicationConfiguration)App.Current).SelectedPatient = null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Method is for opening Services Search Window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkAddService_Click(object sender, RoutedEventArgs e)
        {
            if (cmdModify.IsEnabled == true)
            {
                IsFromOtherServices = true;
                //cmdModify_Click(null, null);
                if (PharmacyItems.Count == 0 && ChargeList.Count <= 0)
                {
                    // Then We Can not Save the Services In Charge Table 
                }
                else
                {
                    SaveBill(null, false);
                }
            }

            DiscRateOnForm = 0;
            AssignForOnlyOnce = false;

            ServiceSearchForPackage serviceSearch = null;   //ServiceSearch serviceSearch = null;
            serviceSearch = new ServiceSearchForPackage();   //serviceSearch = new ServiceSearch();
            serviceSearch.ClassID = ((IApplicationConfiguration)App.Current).SelectedPatient.ClassID;
            serviceSearch.OnAddButton_Click += new RoutedEventHandler(serviceSearch_OnAddButton_Click);
            serviceSearch.Show();

        }

        /// <summary>
        /// This is method is for adding selected services on service search window to the patients bill details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public List<clsUnavailableItemStockServiceId> UnavailableItemStockService { get; set; }

        public class clsUnavailableItemStockServiceId
        {
            public long ServiceID { get; set; }
        }
        void serviceSearch_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (((ServiceSearchForPackage)sender).DialogResult == true) //if (((ServiceSearch)sender).DialogResult == true)
            {
                //List<string> SpecID = new List<string>();
                List<clsSpecialSpecializationVO> SpecializationList = new List<clsSpecialSpecializationVO>();

                List<clsServiceMasterVO> lServices = new List<clsServiceMasterVO>();
                lServices = ((ServiceSearchForPackage)sender).SelectedOtherServices.ToList();   //lServices = ((ServiceSearch)sender).SelectedOtherServices.ToList();
                bool IsMultiSponser = false;
                bool IscomboPackage = false;    // Added on 07Feb2019 for Package Flow in IPD

                if (IsAuditTrail)
                {
                    LogInformation = new LogInfo();
                    LogInformation.guid = objGUID;
                    LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    LogInformation.TimeStamp = DateTime.Now;
                    LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                    LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                    LogInformation.Message = " 13 : Selected Services For Bill " //+ Convert.ToString(lineNumber)
                                                            + "Is MultiSponser : " + Convert.ToString(IsMultiSponser);
                }

                var item5 = from p in ChargeList
                            where p.IsDefaultService == true
                            select p;
                if (item5.ToList().Count != ChargeList.Count)
                {
                    foreach (var item in lServices)
                    {
                        var item1 = from r in ChargeList
                                    where r.CompanyID == item.CompanyID && r.PatientSourceID == item.PatientSourceID && r.TariffId == item.TariffID && r.IsDefaultService == false
                                    select r;

                        if (item1.ToList().Count > 0)
                        {
                            IsMultiSponser = false;
                        }
                        else
                        {
                            IsMultiSponser = true;
                        }
                        break;
                    }

                    # region
                    // Begin :: Added on 07Feb2019 for Package Flow in IPD

                    var item52 = from p in lServices
                                 where p.PackageID > 0
                                 select p;

                    if (item52.ToList().Count > 0)      // if service selected under package : added on 13082018
                    {
                        foreach (var item in lServices)
                        {
                            var item12 = from r in ChargeList
                                         where r.CompanyID == item.CompanyID && r.PatientSourceID == item.PatientSourceID && r.TariffId == item.TariffID && r.PackageID == item.PackageID && r.IsDefaultService == false
                                         select r;

                            if (item12.ToList().Count > 0)
                            {
                                IscomboPackage = false;
                            }
                            else
                            {
                                IscomboPackage = true;      // Sets true when non-package & package ; or multi - package services find while add
                            }
                            break;
                        }
                    }
                    // End :: Added on 07Feb2019 for Package Flow in IPD
                    #endregion

                }
                //Added By Bhushanp 03082017 FOR New Package Changes
                var items1 = from r in ChargeList
                             where r.isPackageService == true
                             select r;

                var items2 = from r in ChargeList
                             where r.isPackageService == false && r.PackageID == 0
                             select r;


                var items3 = from r in ChargeList
                             where r.isPackageService == false && r.PackageID > 0
                             select r;

                var items4 = from r in lServices
                             where r.IsPackage == false && r.PackageID == 0
                             select r;


                var items5 = from r in lServices
                             where r.IsPackage == true
                             select r;

                var items6 = from r in lServices
                             where r.IsPackage == false && r.PackageID > 0
                             select r;

                # region
                // Begin :: Added on 07Feb2019 for Package Flow in IPD

                // For Package New changes Added on 07Feb2019
                var items7 = from r in lServices
                             where r.IsPackage == false && r.PackageID > 0 && r.IsAdjustableHead == false // Check for Package Consumption Services
                             select r;

                // For Package New changes Added on 07Feb2019
                var items8 = from r in lServices
                             where r.IsPackage == false && r.PackageID > 0 && r.IsAdjustableHead == true  // Check for Package Consumption Services as a adjustable head
                             select r;

                // For Package New changes Added on 07Feb2019
                var items9 = from r in ChargeList
                             where r.isPackageService == false && r.PackageID > 0 && r.IsAdjustableHead == true // Check for Package Consumption Services as a adjustable head 
                             select r;                                                                          // already added in grid   

                // For Package New changes Added on 07Feb2019
                var items10 = from r in ChargeList
                              where r.isPackageService == false && r.PackageID > 0 && r.IsAdjustableHead == false // Check for Package Consumption Services already added in grid
                              select r;

                // End :: Added on 07Feb2019 for Package Flow in IPD
                #endregion

                string MSG = "";
                bool IsServiceCheck = false;

                if (items1.ToList().Count > 0)
                {
                    IsServiceCheck = true;
                    MSG = "You cannot add more than one package.";
                }
                else if (items5.ToList().Count > 1)
                {
                    IsServiceCheck = true;
                    MSG = "You cannot add more than one package.";
                }
                else if (items2.ToList().Count > 0 && (items5.ToList().Count > 0 || items6.ToList().Count > 0))
                {
                    IsServiceCheck = true;
                    MSG = "You cannot add package or package consumption with service.";
                }
                else if (items3.ToList().Count > 0 && (items4.ToList().Count > 0 || items5.ToList().Count > 0))
                {
                    IsServiceCheck = true;
                    MSG = "You cannot add package or srevices with package consumption.";
                }
                else if (items4.ToList().Count > 0 && items5.ToList().Count > 0 || items4.ToList().Count > 0 && items6.ToList().Count > 0 || items5.ToList().Count > 0 && items6.ToList().Count > 0)
                {
                    IsServiceCheck = true;
                    MSG = "You cannot add package, package consumption, and services in same Bill.";
                }
                else if (items1.ToList().Count == 0 && items5.ToList().Count > 0)   // Begin :: Added on 07Feb2019 for Package Flow in IPD
                {
                    //if (IsPackageFreezed == true) // need to check condition for IPD : 07Feb2019
                    //{
                    //    IsServiceCheck = true;
                    //    MSG = "Please freeze the previous package bill.";
                    //}
                }
                else if (items7.ToList().Count > 0 && items8.ToList().Count > 0)    // if Package Consumption & Package Consumption Adjustable both add
                {
                    IsServiceCheck = true;
                    MSG = "You cannot add package consumption, and package consumption Adjustable head in same Bill.";
                }
                else if (items10.ToList().Count > 0 && items8.ToList().Count > 0)    // if Package Consumption already added in grid & Package Consumption Adjustable add
                {
                    IsServiceCheck = true;
                    MSG = "You cannot add package consumption, and package consumption Adjustable head in same Bill.";
                }
                else if (items9.ToList().Count > 0 && items7.ToList().Count > 0)    // if Package Consumption Adjustable already added in grid & Package Consumption add   
                {
                    IsServiceCheck = true;
                    MSG = "You cannot add package consumption, and package consumption Adjustable head in same Bill.";
                }
                // End :: Added on 07Feb2019 for Package Flow in IPD

                //if (IsMultiSponser == false && IsServiceCheck == false)                               // commented on 07Feb2019
                if (IsMultiSponser == false && IsServiceCheck == false && IscomboPackage == false)      // modified on 07Feb2019
                {
                    #region clsApplyPackageDiscountRateOnServiceBizActionVO
                    clsApplyPackageDiscountRateOnServiceBizActionVO objApplyNewRate = new clsApplyPackageDiscountRateOnServiceBizActionVO();
                    objApplyNewRate.objApplyPackageDiscountRate = new List<clsApplyPackageDiscountRateOnServiceVO>();
                    objApplyNewRate.ipServiceList = new List<clsServiceMasterVO>();
                    objApplyNewRate.ipLoginUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    objApplyNewRate.ipPatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    objApplyNewRate.ipPatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                    objApplyNewRate.ipVisitID = AdmissionID;
                    objApplyNewRate.IsIPD = true;
                    objApplyNewRate.ipPatientGenderID = ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID;

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.IsAge == true)
                    {
                        objApplyNewRate.ipPatientDateOfBirth = ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirthFromAge;
                    }
                    else
                    {
                        objApplyNewRate.ipPatientDateOfBirth = ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth;
                    }

                    objApplyNewRate.ipServiceList = lServices;

                    if ((ServiceSearchForPackage)sender != null && ((ServiceSearchForPackage)sender).cmbPatientSource.SelectedItem != null && ((clsPatientSponsorVO)((ServiceSearchForPackage)sender).cmbPatientSource.SelectedItem).MemberRelationID > 0)
                        objApplyNewRate.MemberRelationID = ((clsPatientSponsorVO)((ServiceSearchForPackage)sender).cmbPatientSource.SelectedItem).MemberRelationID;

                    objApplyNewRate.SponsorID = ((MasterListItem)((ServiceSearchForPackage)sender).cmbTariff.SelectedItem).SponsorID;
                    objApplyNewRate.SponsorUnitID = ((MasterListItem)((ServiceSearchForPackage)sender).cmbTariff.SelectedItem).SponsorUnitID;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            objApplyNewRate = (clsApplyPackageDiscountRateOnServiceBizActionVO)arg.Result;

                            UnavailableItemStockService = new List<clsUnavailableItemStockServiceId>();

                            foreach (clsServiceMasterVO item in lServices)
                            {
                                if (item.ConditionID > 0)
                                {
                                    if (item.ConditionalQuantity > item.ConditionalUsedQuantity && item.ConditionType == "AND" && item.IsAgeApplicable == true && item.ServiceMemberRelationID > 0)  //if (item.ConditionalQuantity > item.ConditionalUsedQuantity)
                                    {
                                        item.ConcessionPercent = 100;   //Convert.ToDecimal(item1.DiscountedPercentage);
                                        ConcessionFromPlan = true;
                                    }
                                    else if (item.ConditionalQuantity > (item.ConditionalUsedQuantity + item.MainServiceUsedQuantity) && item.ConditionType == "OR" && item.IsAgeApplicable == true && item.ServiceMemberRelationID > 0)  //if (item.ConditionalQuantity > item.ConditionalUsedQuantity)
                                    {
                                        item.ConcessionPercent = 100;   //Convert.ToDecimal(item1.DiscountedPercentage);
                                        ConcessionFromPlan = true;
                                    }
                                    else
                                    {
                                        if ((item.IsAgeApplicable == true && item.ServiceMemberRelationID > 0))  //&& item.TariffID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID
                                        {
                                            item.ConcessionAmount = 0;
                                            item.ConcessionPercent = Convert.ToDecimal(item.ConditionalDiscount);
                                            ConcessionFromPlan = true;
                                        }
                                        if ((item.IsAgeApplicable == false || item.ServiceMemberRelationID == 0))  //item1.ActualQuantity > 0 &&
                                        {
                                            item.ConcessionPercent = 0;
                                            item.ConcessionAmount = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (clsApplyPackageDiscountRateOnServiceVO item1 in objApplyNewRate.objApplyPackageDiscountRate)
                                    {
                                        //if (item.ID == item1.ServiceID && item.TariffID == item1.TariffID && item.ConditionID == 0)
                                        if (item.ID == item1.ServiceID && item.TariffID == item1.TariffID && item.ConditionID == 0 && item.ProcessID == item1.ProcessID)    // Added on 07Feb2019 for Package Flow in IPD
                                        {

                                            if (item1.IsApplyOn_Rate_Percentage == 1 || item1.IsApplyOn_Rate_Percentage == 3 && item.ConditionID == 0)
                                            {
                                                if (item1.ActualQuantity == 0 && item1.ServiceMemberRelationID == 0)
                                                {
                                                    // Rate Not Assign Here 
                                                }
                                                else
                                                {
                                                    // Commented By CDS On 26/04/2017 Becouse Of Percentage Case Rate Is Dependent On Tariff For That Service So No Rate is going to Change 
                                                    item.Rate = Convert.ToDecimal(item1.DiscountedRate);
                                                }

                                                if ((item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0) && item1.PackageID > 0)  //By CDS   // item1.TariffID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID
                                                {
                                                    item.ConcessionAmount = 0;
                                                    item.ConcessionPercent = Convert.ToDecimal(item1.ConcessionPercentage);
                                                }

                                                if (item1.IsDiscountOnQuantity == true || item1.ServiceMemberRelationID == 0 || item1.IsAgeApplicable == false)
                                                {
                                                    item.ConcessionPercent = 0;
                                                    item.ConcessionAmount = 0;
                                                }
                                            }

                                            if (item1.IsApplyOn_Rate_Percentage == 2 || item1.IsApplyOn_Rate_Percentage == 3 && item.ConditionID == 0)
                                            {
                                                if (item1.IsDiscountOnQuantity == false)
                                                {
                                                    if (item.PackageServiceConditionID == 0 && item.IsPackage == false)
                                                    {
                                                        // Added By CDS For Group Specialization Rate 
                                                        if (item1.ActualQuantity == 0 && item1.UsedQuantity == 0 && item1.IsAgeApplicable == true && item1.ServiceMemberRelationID == 0)
                                                        {
                                                            // For Specialistion Services 
                                                            item.IsInGroupSpecialization = true;

                                                            var result1 = from r in SpecializationList
                                                                          where r.SpecializationId == item1.ServiceID_AsPackageID
                                                                          select r.SpecializationRate;

                                                            //var result1 = from r in SpecID
                                                            //              where r.ToString() == item1.ServiceID_AsPackageID.ToString()
                                                            //              select r.ToList();

                                                            //if (result1.ToList().Count() == 0)
                                                            //{
                                                            //    DiscRateOnForm = Convert.ToDecimal(item1.DiscountedRate);
                                                            //    AssignForOnlyOnce = true;
                                                            //    SpecID.Add(item1.ServiceID_AsPackageID.ToString());
                                                            //}

                                                            //if (item.Rate > DiscRateOnForm && DiscRateOnForm > 0)
                                                            //{

                                                            //    item.ConcessionPercent = 0;
                                                            //    item.ConcessionAmount = DiscRateOnForm;

                                                            //}
                                                            //else if (item.Rate < DiscRateOnForm && DiscRateOnForm > 0)
                                                            //{

                                                            //    item.ConcessionAmount = item.Rate;
                                                            //    DiscRateOnForm = DiscRateOnForm - item.Rate;
                                                            //}
                                                            //else if (item.Rate == DiscRateOnForm && DiscRateOnForm > 0)
                                                            //{
                                                            //    item.ConcessionAmount = item.Rate;
                                                            //    item.ConcessionPercent = DiscRateOnForm;
                                                            //    DiscRateOnForm = DiscRateOnForm - item.Rate;
                                                            //}

                                                            if (result1.ToList().Count() == 0)
                                                            {
                                                                DiscRateOnForm = Convert.ToDecimal(item1.DiscountedRate);
                                                                AssignForOnlyOnce = true;

                                                                clsSpecialSpecializationVO obj1 = new clsSpecialSpecializationVO();

                                                                obj1.SpecializationId = item1.ServiceID_AsPackageID;
                                                                obj1.SpecializationRate = item1.DiscountedRate;
                                                                SpecializationList.Add(obj1);
                                                            }


                                                            double result11 = (from r in SpecializationList
                                                                               where r.SpecializationId == item1.ServiceID_AsPackageID
                                                                               select r.SpecializationRate).SingleOrDefault();

                                                            if (item.Rate > Convert.ToDecimal(result11) && Convert.ToDecimal(result11) > 0)
                                                            {

                                                                item.ConcessionPercent = 0;
                                                                item.ConcessionAmount = Convert.ToDecimal(result11);
                                                                foreach (var it in SpecializationList)
                                                                {
                                                                    if (it.SpecializationId == item1.ServiceID_AsPackageID)
                                                                    {
                                                                        it.SpecializationRate = Convert.ToDouble(result11) - Convert.ToDouble(result11);
                                                                    }
                                                                }
                                                            }
                                                            else if (item.Rate < Convert.ToDecimal(result11) && Convert.ToDecimal(result11) > 0)
                                                            {

                                                                item.ConcessionAmount = item.Rate;
                                                                foreach (var it in SpecializationList)
                                                                {
                                                                    if (it.SpecializationId == item1.ServiceID_AsPackageID)
                                                                    {
                                                                        it.SpecializationRate = Convert.ToDouble(result11) - Convert.ToDouble(item.Rate);
                                                                    }
                                                                }
                                                            }
                                                            else if (item.Rate == Convert.ToDecimal(result11) && Convert.ToDecimal(result11) > 0)
                                                            {
                                                                item.ConcessionAmount = item.Rate;
                                                                item.ConcessionPercent = Convert.ToDecimal(result11);
                                                                foreach (var it in SpecializationList)
                                                                {
                                                                    if (it.SpecializationId == item1.ServiceID_AsPackageID)
                                                                    {
                                                                        it.SpecializationRate = Convert.ToDouble(result11) - Convert.ToDouble(item.Rate);
                                                                    }
                                                                }
                                                            }

                                                        } // Added By CDS For Group Specialization Rate 
                                                        else if (item1.ActualQuantity > item1.UsedQuantity && item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0)
                                                        {
                                                            item.ConcessionPercent = Convert.ToDecimal(item1.DiscountedPercentage);

                                                            ConcessionFromPlan = true; //Added by priyanka-to set validation on concession
                                                        }
                                                        else
                                                        {
                                                            if ((item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0) && item1.PackageID > 0)   //By CDS   item1.TariffID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID
                                                            {
                                                                item.ConcessionAmount = 0;
                                                                item.ConcessionPercent = Convert.ToDecimal(item1.ConcessionPercentage);
                                                            }

                                                            if ((item1.IsAgeApplicable == false || item1.ServiceMemberRelationID == 0))  //item1.ActualQuantity > 0 &&
                                                            {
                                                                item.ConcessionPercent = 0;
                                                                item.ConcessionAmount = 0;
                                                            }
                                                        }
                                                    }
                                                    else if (item.PackageServiceConditionID > 0)
                                                    {
                                                        if (item1.ActualQuantity > (item1.UsedQuantity + item.TotalORUsedQuantity) && item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0)
                                                        {
                                                            item.ConcessionPercent = Convert.ToDecimal(item1.DiscountedPercentage);

                                                            ConcessionFromPlan = true; //Added by priyanka-to set validation on concession
                                                        }
                                                        else
                                                        {
                                                            if ((item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0) && item1.PackageID > 0)    //By CDS // item1.TariffID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID
                                                            {
                                                                item.ConcessionAmount = 0;
                                                                item.ConcessionPercent = Convert.ToDecimal(item1.ConcessionPercentage);
                                                            }

                                                            if ((item1.IsAgeApplicable == false || item1.ServiceMemberRelationID == 0))  //item1.ActualQuantity > 0 && 
                                                            {
                                                                item.ConcessionPercent = 0;
                                                                item.ConcessionAmount = 0;
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (item1.ActualQuantity > (item1.UsedQuantity + item.TotalORUsedQuantity) && item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0)
                                                    {
                                                        if ((item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0) && item1.PackageID > 0)  //By CDS // item1.TariffID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID
                                                        {
                                                            item.ConcessionAmount = 0;
                                                            item.ConcessionPercent = Convert.ToDecimal(item1.ConcessionPercentage);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        item.ConcessionPercent = 0;
                                                        item.ConcessionAmount = 0;
                                                    }
                                                }
                                            }

                                            if (item.IsMarkUp == true && item1.IsServiceItemStockAvailable != true && item.ConditionID == 0)
                                            {
                                                UnavailableItemStockService.Add(new clsUnavailableItemStockServiceId { ServiceID = item.ServiceID });
                                            }

                                        }
                                    }
                                }
                            }
                            StringBuilder sbServiceName = new StringBuilder();
                            foreach (clsUnavailableItemStockServiceId item in UnavailableItemStockService)
                            {
                                clsServiceMasterVO obj = new clsServiceMasterVO();
                                obj = (clsServiceMasterVO)lServices.First(z => z.ServiceID == item.ServiceID && z.IsMarkUp == true);
                                sbServiceName.Append(obj.ServiceName + ",");
                                lServices.Remove(obj);
                            }
                            if (sbServiceName.Length > 0)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgWD =
                                                new MessageBoxControl.MessageBoxChildWindow("", "Items are not available for Service : " + sbServiceName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWD.OnMessageBoxClosed += (re) =>
                                {
                                    if (re == MessageBoxResult.OK)
                                    {
                                        PatientCategoryID = ((ServiceSearchForPackage)sender).PatientCategoryL1Id_Retail; //((ServiceSearch)sender).PatientCategoryL1Id_Retail;
                                        PatientSourceID = ((clsPatientSponsorVO)((ServiceSearchForPackage)sender).cmbPatientSource.SelectedItem).PatientSourceID;   //((clsPatientSponsorVO)((ServiceSearch)sender).cmbPatientSource.SelectedItem).PatientSourceID;
                                        PatientTariffID = ((MasterListItem)((ServiceSearchForPackage)sender).cmbTariff.SelectedItem).ID;  //((MasterListItem)((ServiceSearch)sender).cmbTariff.SelectedItem).ID;
                                        PatientCompanyID = ((MasterListItem)((ServiceSearchForPackage)sender).cmbCompany.SelectedItem).ID;  //((MasterListItem)((ServiceSearch)sender).cmbTariff.SelectedItem).ID;
                                        PackageTariffID = ((ServiceSearchForPackage)sender).PackageTariffID; //((ServiceSearch)sender).PackageTariffID;
                                        ServiceTariffID = ((ServiceSearchForPackage)sender).ServiceTariffID;  //((ServiceSearch)sender).ServiceTariffID;
                                        AddCharges(lServices, ServiceTariffID);
                                        ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID = PatientCompanyID;// added by Bhushanp
                                    }
                                };
                                msgWD.Show();
                            }
                            else
                            {
                                PatientCategoryID = ((ServiceSearchForPackage)sender).PatientCategoryL1Id_Retail; //((ServiceSearch)sender).PatientCategoryL1Id_Retail;
                                PatientSourceID = ((clsPatientSponsorVO)((ServiceSearchForPackage)sender).cmbPatientSource.SelectedItem).PatientSourceID;  //((clsPatientSponsorVO)((ServiceSearch)sender).cmbPatientSource.SelectedItem).PatientSourceID;
                                PatientTariffID = ((MasterListItem)((ServiceSearchForPackage)sender).cmbTariff.SelectedItem).ID;  //((MasterListItem)((ServiceSearch)sender).cmbTariff.SelectedItem).ID;
                                PatientCompanyID = ((MasterListItem)((ServiceSearchForPackage)sender).cmbCompany.SelectedItem).ID;
                                PackageTariffID = ((ServiceSearchForPackage)sender).PackageTariffID;  //((ServiceSearch)sender).PackageTariffID;
                                ServiceTariffID = ((ServiceSearchForPackage)sender).ServiceTariffID;  //((ServiceSearch)sender).ServiceTariffID;
                                AddCharges(lServices, ServiceTariffID);
                                ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID = PatientCompanyID;// added by Bhushanp
                            }
                            if (IsAuditTrail)
                            {
                                LogInformation.Message = LogInformation.Message +
                                                                      " , Patient Category ID : " + Convert.ToString(PatientCategoryID)
                                                                    + " , Patient Source ID : " + Convert.ToString(PatientSourceID)
                                                                    + " , Patient Tariff ID : " + Convert.ToString(PatientTariffID)
                                                                    + " , Patient Company ID : " + Convert.ToString(PatientCompanyID)
                                                                    + " , Package Tariff ID : " + Convert.ToString(PackageTariffID)
                                                                    + " , Service Tariff ID : " + Convert.ToString(ServiceTariffID);
                                foreach (clsServiceMasterVO item in lServices)
                                {
                                    LogInformation.Message = LogInformation.Message +
                                                                      " , Service ID : " + Convert.ToString(item.ID)
                                                                    + " , Service Code : " + Convert.ToString(item.ServiceCode)
                                                                    + " , Service Name : " + Convert.ToString(item.ServiceName)
                                                                    + " , Long Description : " + Convert.ToString(item.LongDescription)
                                                                    + " , Patient Category L3 : " + Convert.ToString(item.PatientCategoryL3)
                                                                    + " , Patient Source ID : " + Convert.ToString(item.PatientSourceID)
                                                                    + " , Rate : " + Convert.ToString(item.Rate)
                                                                    + " , Specialization ID : " + Convert.ToString(item.Specialization)
                                                                    + " , Specialization : " + Convert.ToString(item.SpecializationString)
                                                                    + " , SubSpecialization ID : " + Convert.ToString(item.SubSpecialization)
                                                                    + " , SubSpecialization : " + Convert.ToString(item.SubSpecializationString)
                                                                    + " , Tariff ID : " + Convert.ToString(item.TariffID)
                                                                    + " , Tariff Service Master ID : " + Convert.ToString(item.TariffServiceMasterID) + "\r\n"
                                                                    ;
                                }
                                LogInfoList.Add(LogInformation);
                            }
                        }
                        FillPatientAdvance(true);
                        if (IsMsgServiceConsumption)
                        {
                            IsMsgServiceConsumption = false;
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Some services already consume under this package, Create new bill for already consume services.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    };
                    client.ProcessAsync(objApplyNewRate, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();

                    #endregion
                }
                else if (IsServiceCheck == true)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW2 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", MSG.ToString(), MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW2.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW2 =
                                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Multiple Sponsor Billing is not Allowed", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW2.Show();
                }
            }
        }

        private void AddCharges(List<clsServiceMasterVO> mServices, long TariffID)
        {
            StringBuilder strError = new StringBuilder();

            for (int i = 0; i < mServices.Count; i++)
            {
                bool Addcharge = true;

                if (ChargeList != null && ChargeList.Count > 0)
                {
                    //var item = from r in ChargeList
                    //           where r.ServiceId == mServices[i].ID
                    ////&& r.ProcessID == mServices[i].ProcessID     // Added on 07Feb2019 for Package Flow in IPD
                    //           select new clsChargeVO
                    //           {
                    //               Status = r.Status,
                    //               ID = r.ID,
                    //               ServiceName = r.ServiceName
                    //           };

                    //if (item.ToList().Count > 0)
                    //{
                    //    if (strError.ToString().Length > 0)
                    //        strError.Append(",");
                    //    strError.Append(item.ToList()[0].ServiceName);
                    //    Addcharge = false;

                    //}
                }
                if (Addcharge)
                {
                    RowNum += 1;//Added By Bhushanp 17022017 use for delete service
                    clsChargeVO itemC = new clsChargeVO();

                    itemC.RowID = RowNum;//Added By Bhushanp 17022017 use for delete service
                    itemC.Service = mServices[i].ServiceName;
                    itemC.TariffId = mServices[i].TariffID;
                    itemC.ServiceSpecilizationID = mServices[i].Specialization;
                    itemC.TariffServiceId = mServices[i].TariffServiceMasterID;
                    itemC.ServiceId = mServices[i].ID;
                    itemC.ServiceName = mServices[i].ServiceName;
                    itemC.Quantity = 1;
                    itemC.Date = DateTime.Now;
                    itemC.RateEditable = mServices[i].RateEditable;
                    itemC.MinRate = Convert.ToDouble(mServices[i].MinRate);
                    itemC.MaxRate = Convert.ToDouble(mServices[i].MaxRate);
                    itemC.Rate = Convert.ToDouble(mServices[i].Rate);
                    itemC.InitialRate = itemC.Rate;  // to maintain the Initial Rate of Service after changing Doctor Service Rate.
                    itemC.PatientSourceID = mServices[i].PatientSourceID;
                    itemC.CompanyID = mServices[i].CompanyID;
                    itemC.TotalAmount = itemC.Rate * itemC.Quantity;
                    itemC.isPackageService = mServices[i].IsPackage;
                    itemC.PackageID = mServices[i].PackageID;

                    itemC.ProcessID = mServices[i].ProcessID;       // For Package New Changes Added on 07Feb2019

                    // Begin : Added on 07Feb2019 for Package Flow in IPD
                    //if (itemC.isPackageService == false && itemC.PackageID > 0)
                    //{
                    //    itemC.ParentID = mServices[i].ChargeID;//For package bill service  charge id as PArent ID 180417 added by yk
                    //    chkFreezBill.IsChecked = true;
                    //    chkFreezBill.IsEnabled = false;
                    //    IsPackageConsumption = true;
                    //}
                    //else
                    //{
                    //    itemC.ParentID = 0;
                    //    chkFreezBill.IsChecked = false;
                    //    chkFreezBill.IsEnabled = true;
                    //    IsPackageConsumption = false;
                    //}
                    // End : Added on 07Feb2019 for Package Flow in IPD

                    itemC.PackageBillID = mServices[i].PackageBillID;               // Added on 07Feb2019 for Package Flow in IPD
                    itemC.PackageBillUnitID = mServices[i].PackageBillUnitID;       // Added on 07Feb2019 for Package Flow in IPD

                    itemC.IsAdjustableHead = mServices[i].IsAdjustableHead;         // Added on 07Feb2019 for Package Flow in IPD
                    itemC.AdjustableHeadType = mServices[i].AdjustableHeadType;     // Added on 07Feb2019 for Package Flow in IPD

                    itemC.ServiceCode = mServices[i].ServiceCode;
                    // Added By CDS For Adjustable Service
                    itemC.ServiceComponentRate = mServices[i].ServiceComponentRate;
                    itemC.IsAdjustableHead = mServices[i].IsAdjustableHead;
                    itemC.IsConsiderAdjustable = mServices[i].IsConsiderAdjustable;

                    itemC.SumOfExludedServices = mServices[i].SumOfExludedServices;
                    itemC.IsInGroupSpecialization = mServices[i].IsInGroupSpecialization;
                    itemC.OPDConsumption = mServices[i].OPDConsumption;
                    itemC.OpdExcludeServiceConsumption = mServices[i].OpdExcludeServiceConsumption;
                    if (mServices[i].IsPackage == false && mServices[i].PackageID > 0)
                    {
                        if (mServices[i].ConcessionPercent > 0 || mServices[i].ConcessionAmount > 0)
                        {
                            itemC.ParentID = mServices[i].ChargeID;  //For package bill service  charge id as PArent ID 180417 added by yk
                            chkFreezBill.IsChecked = true;
                            chkFreezBill.IsEnabled = false;
                            IsPackageConsumption = true;
                        }
                        else if (mServices[i].IsAdjustableHead == true)
                        {
                            itemC.ParentID = mServices[i].ChargeID;  //For package bill service  charge id as PArent ID 
                            chkFreezBill.IsChecked = true;
                            chkFreezBill.IsEnabled = false;
                            IsPackageConsumption = true;
                        }
                        else
                        {
                            itemC.ParentID = 0;
                        }
                    }
                    else
                    {
                        itemC.ParentID = 0;
                        chkFreezBill.IsChecked = false;
                        chkFreezBill.IsEnabled = true;
                        IsPackageConsumption = false;
                    }

                    //if (itemC.isPackageService == false && itemC.PackageID > 0)
                    //{
                    //    itemC.ParentID = mServices[i].ChargeID;//For package bill service  charge id as PArent ID 180417 added by yk
                    //}
                    //else
                    //{
                    //    itemC.ParentID = 0;
                    //}
                    //END

                    //***// Begin :: Added on 07Feb2019 for Package Flow in IPD
                    itemC.VisitID = mServices[i].VisitID;
                    itemC.PrescriptionDetailsID = mServices[i].PrescriptionDetailsID;
                    itemC.InvestigationDetailsID = mServices[i].InvestigationDetailsID;
                    //----- End :: Added on 07Feb2019 for Package Flow in IPD

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 3 || ((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 6)
                    {
                        if (mServices[i].StaffDiscountPercent > 0)
                            itemC.StaffDiscountPercent = Convert.ToDouble(mServices[i].StaffDiscountPercent);
                        else
                            itemC.StaffDiscountAmount = Convert.ToDouble(mServices[i].StaffDiscountAmount);

                        if (mServices[i].StaffDependantDiscountPercent > 0)
                            itemC.StaffParentDiscountPercent = Convert.ToDouble(mServices[i].StaffDependantDiscountPercent);
                        else
                            itemC.StaffParentDiscountAmount = Convert.ToDouble(mServices[i].StaffDependantDiscountAmount);

                        if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.ApplyConcessionToStaff == true)
                        {
                            if (mServices[i].ConcessionPercent > 0)
                            {
                                itemC.ConcessionPercent = Convert.ToDouble(mServices[i].ConcessionPercent);
                            }
                            else
                                itemC.Concession = Convert.ToDouble(mServices[i].ConcessionAmount);
                        }
                    }
                    else
                    {
                        if (itemC.isPackageService == false && itemC.PackageID > 0)     // Added on 07Feb2019 for Package Flow in IPD
                        {
                            if (mServices[i].ConcessionPercent > 0)
                            {
                                itemC.PackageConcessionPercent = Convert.ToDouble(mServices[i].ConcessionPercent);
                            }
                            else
                                itemC.PackageConcession = Convert.ToDouble(mServices[i].ConcessionAmount);
                        }
                        else
                        {
                            if (mServices[i].ConcessionPercent > 0)
                            {
                                itemC.ConcessionPercent = Convert.ToDouble(mServices[i].ConcessionPercent);
                            }
                            else
                                itemC.Concession = Convert.ToDouble(mServices[i].ConcessionAmount);
                        }
                    }

                    if (mServices[i].ServiceTaxPercent > 0)
                        itemC.ServiceTaxPercent = Convert.ToDouble(mServices[i].ServiceTaxPercent);
                    else
                        itemC.ServiceTaxAmount = Convert.ToDouble(mServices[i].ServiceTaxAmount);

                    if (mServices[i].ConditionTypeID > 0)
                    {
                        itemC.ConditionTypeID = mServices[i].ConditionTypeID;
                        itemC.ConditionType = mServices[i].ConditionType;
                    }
                    itemC.Doctor = DoctorList;
                    itemC.DoctorId = VisitDoctorID;
                    itemC.SelectedDoctor = DoctorList.FirstOrDefault(p => p.ID == VisitDoctorID);

                    #region GST Details added by Ashish Z. on dated 24062017
                    try
                    {
                        clsGetServiceTaxDetailsBizActionVO BizAction = new clsGetServiceTaxDetailsBizActionVO();
                        BizAction.ServiceTaxDetailsVO = new clsServiceTaxVO();
                        BizAction.ServiceTaxDetailsVO.ServiceId = itemC.ServiceId;
                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, args) =>
                        {
                            if (args.Error == null && args.Result != null)
                            {
                                clsGetServiceTaxDetailsBizActionVO objVO = new clsGetServiceTaxDetailsBizActionVO();
                                objVO = (args.Result as clsGetServiceTaxDetailsBizActionVO);

                                if (objVO.ServiceTaxDetailsVOList != null && objVO.ServiceTaxDetailsVOList.Count > 0)
                                {
                                    if (itemC.ChargeTaxDetailsList == null)
                                        itemC.ChargeTaxDetailsList = new List<clsChargeTaxDetailsVO>();

                                    foreach (var item in objVO.ServiceTaxDetailsVOList.ToList())
                                    {
                                        clsChargeTaxDetailsVO ChargeTaxDetailsVO = new clsChargeTaxDetailsVO();
                                        ChargeTaxDetailsVO.ServiceId = item.ServiceId;
                                        ChargeTaxDetailsVO.TariffId = item.TariffId;
                                        ChargeTaxDetailsVO.ClassId = item.ClassId;
                                        ChargeTaxDetailsVO.TaxID = item.TaxID;
                                        ChargeTaxDetailsVO.Percentage = item.Percentage;
                                        ChargeTaxDetailsVO.ServiceTaxPercent = Convert.ToDouble(item.Percentage);
                                        ChargeTaxDetailsVO.TaxType = item.TaxType;
                                        ChargeTaxDetailsVO.IsTaxLimitApplicable = item.IsTaxLimitApplicable;
                                        ChargeTaxDetailsVO.TaxLimit = item.TaxLimit;

                                        ChargeTaxDetailsVO.ServiceName = itemC.ServiceName;
                                        ChargeTaxDetailsVO.TariffName = item.TariffName;
                                        ChargeTaxDetailsVO.TariffName = item.TariffName;
                                        ChargeTaxDetailsVO.TaxName = item.TaxName;

                                        ChargeTaxDetailsVO.Quantity = 1;
                                        ChargeTaxDetailsVO.Rate = Convert.ToDouble(itemC.Rate);
                                        ChargeTaxDetailsVO.TotalAmount = itemC.Rate * itemC.Quantity;

                                        itemC.ChargeTaxDetailsList.Add(ChargeTaxDetailsVO);
                                    }

                                    double TotalTaxamount = 0;
                                    itemC.TaxType = itemC.ChargeTaxDetailsList.ToList()[0].TaxType;
                                    foreach (var item in itemC.ChargeTaxDetailsList.ToList())
                                    {
                                        item.Concession = itemC.Concession;
                                        item.TotalAmount = itemC.TotalAmount;
                                        item.Rate = itemC.Rate;
                                        TotalTaxamount += item.ServiceTaxAmount;
                                    }
                                    itemC.TotalServiceTaxAmount = TotalTaxamount;
                                }
                            }
                            CalculateClinicalSummary(); //added by Ashish Z. for Taxation Details on dated 18052017
                        };
                        client.ProcessAsync(BizAction, null); //((IApplicationConfiguration)App.Current).CurrentUser
                        client.CloseAsync();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    #endregion

                    //ChargeList.Add(itemC);
                    if (itemC.isPackageService == false && itemC.PackageID > 0)
                    {
                        //if (itemC.ConcessionPercent == 100)           // Commented on 07Feb2019 for Package Flow in IPD
                        if (itemC.PackageConcessionPercent == 100)      // Added on 07Feb2019 for Package Flow in IPD
                        {
                            ChargeList.Add(itemC);
                        }
                        else
                        {
                            IsMsgServiceConsumption = true;
                        }
                    }
                    else
                    {
                        ChargeList.Add(itemC);
                    }
                }
            }
            CalculateClinicalSummary();

            dgCharges.Focus();
            dgCharges.UpdateLayout();

            dgCharges.SelectedIndex = ChargeList.Count - 1;

            # region Added on 07Feb2019 for Package Flow in IPD

            long PackageBillID2 = 0;
            long PackageBillUnitID2 = 0;
            bool IsAdjustableHead = false;

            if (ChargeList != null && ChargeList.Count > 0)         // Added on 07Feb2019 for Package Flow in IPD
            {
                if (ChargeList.Where(p => p.PackageBillID > 0).Any())
                    PackageBillID2 = ChargeList.FirstOrDefault(p => p.PackageBillID > 0).PackageBillID;
                if (ChargeList.Where(p => p.PackageBillUnitID > 0).Any())
                    PackageBillUnitID2 = ChargeList.FirstOrDefault(p => p.PackageBillUnitID > 0).PackageBillUnitID;
                if (ChargeList.Where(p => p.IsAdjustableHead == true).Any())
                    IsAdjustableHead = ChargeList.FirstOrDefault(p => p.IsAdjustableHead == true).IsAdjustableHead;
            }

            if (PackageBillID2 > 0 && PackageBillUnitID2 > 0 && IsAdjustableHead == true)       // Added on 07Feb2019 for Package Flow in IPD
                FillPackage(PackageBillID2, PackageBillUnitID2);

            #endregion

            if (!string.IsNullOrEmpty(strError.ToString()))
            {
                string strMsg = "Services already added : " + strError.ToString();

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        /// <summary>
        /// Function is for calculating and displaying Services amount summary.
        /// </summary>
        /// 
        double ApprovalConcenssion = 0;
        double ApprovalTotal = 0;
        double PackageConcenssion = 0;
        private void CalculateClinicalSummary()
        {
            bool IsPackageServices = false;
            double clinicalTotal, clinicalConcession, clinicalNetAmount;
            double clinicalTotalForPackService = 0;
            clinicalTotal = clinicalConcession = clinicalNetAmount = ApprovalConcenssion = ApprovalTotal = PackageConcenssion = 0;
            for (int i = 0; i < ChargeList.Count; i++)
            {
                clinicalTotal += (ChargeList[i].TotalAmount);
                clinicalConcession += ChargeList[i].Concession;
                clinicalNetAmount += ChargeList[i].NetAmount;

                if (ChargeList[i].PackageID == 0 || ChargeList[i].isPackageService == true)
                {
                    ApprovalConcenssion += ChargeList[i].Concession;
                    ApprovalTotal += ChargeList[i].TotalAmount;
                }
                if (ChargeList[i].PackageID > 0)
                {
                    //PackageConcenssion += ChargeList[i].Concession;          // modified on 07Feb2019 for Package Flow in IPD
                    PackageConcenssion += ChargeList[i].PackageConcession;     // Added on 07Feb2019 for Package Flow in IPD

                }

                /////////////Added By YK

                if (ChargeList[i].isPackageService == true)
                {
                    IsPackageServices = true;
                }

                if (IsPackageServices == true)
                {
                    if (ChargeList[i].isPackageService == false)
                    {
                        clinicalTotalForPackService += (ChargeList[i].TotalAmount);
                    }
                }

                ///////////END///////////

            }

            txtClinicalTotal.Text = String.Format("{0:0.00}", clinicalTotal);


            //if (IsPackageServices == true)
            //{
            //    txtClinicalTotal.Text = String.Format("{0:0.00}", clinicalTotalForPackService);//Added BY YK
            //}


            if (clinicalConcession <= 0)
            {
                cmbConcessionReason.SelectedValue = 0;
            }
            txtClinicalConcession.Text = String.Format("{0:0.00}", clinicalConcession);
            txtClinicalNetAmount.Text = String.Format("{0:0.00}", clinicalNetAmount);
            CalculateTotalSummary();
        }

        /// <summary>
        /// Function is for calculating and displaying Pharmacy amount summary.
        /// </summary>
        private void CalculatePharmacySummary()
        {
            double Total, Concession, NetAmount, TotalVat;
            Total = Concession = NetAmount = TotalVat = 0;
            for (int i = 0; i < PharmacyItems.Count; i++)
            {
                Total += (PharmacyItems[i].Amount);
                Concession += PharmacyItems[i].ConcessionAmount;
                TotalVat += PharmacyItems[i].VATAmount;
            }
            NetAmount = (Total - Concession) + TotalVat;
            txtPharmacyTotal.Text = String.Format("{0:0.00}", Total);
            txtPharmacyConcession.Text = String.Format("{0:0.00}", Concession);
            txtPharmacyVatAmount.Text = String.Format("{0:0.00}", TotalVat);
            txtPharmacyNetAmount.Text = String.Format("{0:0.00}", NetAmount);
            CalculateTotalSummary();
        }

        /// <summary>
        ///  Function is for calculating and displaying Total Bill amount summary
        /// </summary>
        private void CalculateTotalSummary()
        {
            if (string.IsNullOrEmpty(txtPharmacyNetAmount.Text))
                txtPharmacyNetAmount.Text = "0";
            if (string.IsNullOrEmpty(txtClinicalNetAmount.Text))
                txtClinicalNetAmount.Text = "0";

            if (string.IsNullOrEmpty(txtPharmacyTotal.Text))
                txtPharmacyTotal.Text = "0";
            if (string.IsNullOrEmpty(txtClinicalTotal.Text))
                txtClinicalTotal.Text = "0";

            if (string.IsNullOrEmpty(txtPharmacyConcession.Text))
                txtPharmacyConcession.Text = "0";
            if (string.IsNullOrEmpty(txtClinicalConcession.Text))
                txtClinicalConcession.Text = "0";

            txtTotalClinicalBill.Text = txtClinicalNetAmount.Text;
            txtTotalPharmacyBill.Text = txtPharmacyNetAmount.Text;

            double lNetAmt, lTotalBill, lTotalConcession = 0;

            lNetAmt = (Convert.ToDouble(txtPharmacyNetAmount.Text) + Convert.ToDouble(txtClinicalNetAmount.Text));
            txtNetAmount.Text = String.Format("{0:0.00}", lNetAmt);

            lTotalBill = Convert.ToDouble(txtPharmacyTotal.Text) + Convert.ToDouble(txtClinicalTotal.Text);
            txtTotalBill.Text = String.Format("{0:0.00}", lTotalBill);

            lTotalConcession = Convert.ToDouble(txtPharmacyConcession.Text) + Convert.ToDouble(txtClinicalConcession.Text);
            txtTotalConcession.Text = String.Format("{0:0.00}", lTotalConcession);

            if (rdbAgainstBill.IsChecked == true)
                txtPayAmount.Text = txtNetAmount.Text;
            else
            {
                var results = from r in this.ChargeList
                              where r.Status == true
                              select r;
                double? lPayAmount = results.Sum(cnt => cnt.NetAmount);

                if (lPayAmount.HasValue)
                    txtPayAmount.Text = String.Format("{0:0.00}", lPayAmount);
                else
                    txtPayAmount.Text = "0.00";
            }
        }

        # region Added on 07Feb2019 for Package Flow in IPD

        private void FillPackage(long PackageBillID1, long PackageBillUnitID1)
        {
            clsGetPatientPackageInfoListBizActionVO BizAction = new clsGetPatientPackageInfoListBizActionVO();

            BizAction.PackageBillID = PackageBillID1;
            BizAction.PackageBillUnitID = PackageBillUnitID1;

            BizAction.IsForPackage = true;   // For Package New Changes Added on 19062018 : to Get details for Selected Package

            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    //objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetPatientPackageInfoListBizActionVO)arg.Result).MasterList);

                    if (objList.Count > 0 && ChargeList != null && ChargeList.Count > 0)
                    {
                        foreach (clsChargeVO itemCh in ChargeList)
                        {
                            if (itemCh.IsAdjustableHead == true && itemCh.AdjustableHeadType == 1)
                            {
                                if (Math.Round(objList[0].OPDConsumption, 2) < Math.Round(objList[0].PackageClinicalTotal, 2))
                                {
                                    itemCh.Rate = Math.Round((objList[0].PackageClinicalTotal - objList[0].OPDConsumption), 2);
                                    itemCh.PackageConcession = Math.Round((objList[0].PackageClinicalTotal - objList[0].OPDConsumption), 2);
                                }
                            }

                            if (itemCh.IsAdjustableHead == true && itemCh.AdjustableHeadType == 2)
                            {
                                if (Math.Round(objList[0].PharmacyConsumeAmount, 2) < Math.Round(objList[0].PharmacyFixedRate, 2))
                                {
                                    itemCh.Rate = Math.Round((objList[0].PharmacyFixedRate - objList[0].PharmacyConsumeAmount), 2);
                                    itemCh.PackageConcession = Math.Round((objList[0].PharmacyFixedRate - objList[0].PharmacyConsumeAmount), 2);
                                }
                            }
                        }

                        CalculateClinicalSummary();
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        #endregion

        private void FillPatientAdvance(bool checkFromNewClick)
        {
            try
            {
                PatAdvanceAmt = 0;   // Added By CDS 
                PatBalAdvaceAmt = 0; // Added By CDS 

                Indicatior = new WaitIndicator();
                Indicatior.Show();
                clsGetPatientAdvanceListBizActionVO BizAction = new clsGetPatientAdvanceListBizActionVO();
                BizAction.ID = 0;
                BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction.CompanyID = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
                BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;

                BizAction.IsShowBothAdvance = true;  //Added by Prashant Channe on 11/26/2018 to get patient advance on new IPD Bill

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    PatAdvanceAmt = 0;   // Added By CDS 
                    PatBalAdvaceAmt = 0; // Added By CDS 

                    if (arg.Error == null && arg.Result != null)
                    {
                        List<clsAdvanceVO> objList = new List<clsAdvanceVO>();
                        objList = ((clsGetPatientAdvanceListBizActionVO)arg.Result).Details;
                        if (objList != null)
                        {
                            foreach (var item in objList)
                            {
                                item.Used = 0;
                                PatAdvanceAmt += item.Total;
                                PatBalAdvaceAmt += item.Balance;
                            }

                        }
                        //Added By Bhushanp 04/01/17 For Advance Amount is "" String
                        if (!(string.IsNullOrEmpty(PatBalAdvaceAmt.ToString())))
                        {
                            //txtAdvance.Text = PatBalAdvaceAmt.ToString();                      // commented on 07Feb2019 for Package Flow in IPD
                            txtAdvance.Text = (Math.Round(PatBalAdvaceAmt, 2)).ToString();       // Added on 07Feb2019 for Package Flow in IPD
                        }
                        else
                        {
                            txtAdvance.Text = "0";
                        }
                        if (checkFromNewClick == true)
                        {
                            if (((IApplicationConfiguration)App.Current).SelectedPatient.BillBalanceAmountSelf > 0)
                            {
                                string msgTitle = "";
                                string msgText = "Previous Bill Balance Amount is Pending are you sure you want to continue?";

                                MessageBoxControl.MessageBoxChildWindow msgbox =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                msgbox.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(Msgbox_OnMessageBoxClosed);
                                msgbox.Show();
                            }
                        }
                        Indicatior.Close();
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {

            }
            finally
            {
                Indicatior.Close();
            }
        }

        void Msgbox_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                //do continue to Selected Patient Bill
            }
            else
            {
                ModuleName = "PalashDynamics";
                Action = "CIMS.Forms.PatientList";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c2 = new WebClient();
                c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
        }

        /// <summary>
        /// Method is for deleting added service from the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdDeleteCharges_Click(object sender, RoutedEventArgs e)
        {
            clsChargeVO objVO = (clsChargeVO)dgCharges.SelectedItem;
            if (objVO != null)
            {
                if (objVO.IsSampleCollected == false)  //else      //Added By Bhushanp 17012017                 
                {
                    if (IsFromRequestApproval == false)
                    {
                        if (dgCharges.SelectedItem != null)
                        {
                            List<clsChargeVO> tempList = new List<clsChargeVO>();
                            tempList = ChargeList.ToList();
                            if (((clsChargeVO)dgCharges.SelectedItem).IsPackageConsumption == true)
                            {
                                string msgText = "Package Consumption Already done !";
                                MessageBoxControl.MessageBoxChildWindow msgWD =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWD.Show();
                            }
                            else
                            {
                                if (tempList != null) //&& tempList.Count > 1 Commented By Bhushanp 20072017
                                {
                                    string msgText = "Are You Sure \n  You Want To Delete The Selected Service Charge ?";
                                    MessageBoxControl.MessageBoxChildWindow msgWD =
                                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                                    {
                                        if (res == MessageBoxResult.Yes)
                                        {

                                            if (objVO.IsResultEntry == false) //Added By Yogesh Isupload,IsResultEntry 2 7 16
                                            {
                                                if (objVO.Isupload == false)
                                                {
                                                    long chargID = objVO.ID;
                                                    long tserID = objVO.TariffServiceId;
                                                    clsChargeVO obj;
                                                    if (objVO != null)
                                                    {
                                                        obj = ChargeList.Where(z => z.RowID == objVO.RowID).FirstOrDefault();
                                                        ChargeList.Remove(obj);
                                                        DeleteBillAddedItems.Add(obj);
                                                    }
                                                    dgCharges.Focus();
                                                    dgCharges.UpdateLayout();
                                                    dgCharges.SelectedIndex = ChargeList.Count - 1;
                                                    CalculateClinicalSummary();
                                                }
                                                else
                                                {
                                                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Service not Deleted Upload Report Done!", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                                                    mgbx.Show();
                                                }
                                            }
                                            else
                                            {
                                                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Service not Deleted Result Entry Done!", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                                                mgbx.Show();
                                            }
                                        }
                                    };
                                    msgWD.Show();
                                }
                                else
                                {
                                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Atleast one Service should be added!", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                                    mgbx.Show();
                                }
                            }
                        }
                    }
                }
                else
                {
                    string msgText = "Service cannot be deleted Sample Collection already done for this Test";
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWD.Show();
                }
            }
        }
        private void checkresultentryforbBill()
        {
        }



        /// <summary>
        /// Method is for opening Items Search Window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkAddItems_Click(object sender, RoutedEventArgs e)
        {
            ItemListNew ItemSearch = new ItemListNew();
            ItemSearch.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            ItemSearch.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
            ItemSearch.StoreID = StoreID;
            if (StoreID == 0)
                ItemSearch.AllowStoreSelection = true;
            else
                ItemSearch.AllowStoreSelection = false;

            ItemSearch.OnSaveButton_Click += new RoutedEventHandler(ItemSearch_OnSaveButton_Click);
            ItemSearch.Show();
        }
        public long StoreID { get; set; }

        /// <summary>
        /// This is method is for adding selected Item on Item search window into the patients bill details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ItemSearch_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ItemListNew Itemswin = (ItemListNew)sender;
            if (Itemswin.DialogResult == true)
            {
                if (Itemswin.ItemBatchList != null)
                {
                    StringBuilder strError = new StringBuilder();
                    StoreID = Itemswin.StoreID;
                    foreach (var item in Itemswin.ItemBatchList)
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
                            ObjAddItem.ItemID = item.ItemID;
                            ObjAddItem.ItemName = item.ItemName;// Itemswin.SelectedItems[0].ItemName;
                            ObjAddItem.Manufacture = item.Manufacturer;// Itemswin.SelectedItems[0].Manufacturer;
                            ObjAddItem.PregnancyClass = item.PreganancyClass; // Itemswin.SelectedItems[0].PreganancyClass;
                            ObjAddItem.BatchID = item.BatchID;
                            ObjAddItem.BatchCode = item.BatchCode;
                            ObjAddItem.ExpiryDate = item.ExpiryDate;
                            ObjAddItem.Quantity = 1;
                            if (Itemswin.SelectedItems[0].InclusiveOfTax == false)
                                ObjAddItem.MRP = (((item.MRP * item.TotalSalesTaxPercent) / 100) + item.MRP);
                            else
                                ObjAddItem.MRP = item.MRP;
                            ObjAddItem.OriginalMRP = ObjAddItem.MRP;
                            ObjAddItem.AvailableQuantity = item.AvailableStock;
                            ObjAddItem.PurchaseRate = item.PurchaseRate;
                            ObjAddItem.ConcessionPercentage = item.DiscountOnSale;
                            ObjAddItem.ConcessionAmount = ObjAddItem.ConcessionAmount;
                            ObjAddItem.Amount = ObjAddItem.Amount;
                            ObjAddItem.VATPercent = item.VATPerc;
                            ObjAddItem.VATAmount = ObjAddItem.VATAmount;
                            ObjAddItem.NetAmount = ObjAddItem.NetAmount;
                            PharmacyItems.Add(ObjAddItem);
                        }
                    }
                    CalculatePharmacySummary();
                    dgPharmacyItems.Focus();
                    dgPharmacyItems.UpdateLayout();
                    dgPharmacyItems.SelectedIndex = PharmacyItems.Count - 1;
                    if (!string.IsNullOrEmpty(strError.ToString()))
                    {
                        string strMsg = "Items Already Added : " + strError.ToString();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                }
            }
        }

        /// <summary>
        /// Method is for deleting added Items from the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdDeletePharmacyItems_Click(object sender, RoutedEventArgs e)
        {
            if (dgPharmacyItems.SelectedItem != null)
            {
                string msgText = "Are You Sure \n  You Want To Delete The Selected Item ?";
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        PharmacyItems.RemoveAt(dgPharmacyItems.SelectedIndex);
                        CalculatePharmacySummary();
                    }
                };
                msgWD.Show();
            }
        }

        /// <summary>
        /// Method is for clearing all the previous data and opens the bill form (i.e backpanel) in new mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            dgBillList.SelectedItem = null;
            dgCharges.IsEnabled = true;
            IsFromViewClick = false;
            SelectedBill = null;
            IsEditMode = false;
            rdbAgainstBill.IsEnabled = false;
            rdbAgainstBill.IsChecked = true;
            rdbAgainstServices.IsEnabled = false;
            rdbDraftBill.IsChecked = true;
            CheckAdmission(true);
            cmbConcessionReason.ItemsSource = null;
            fillreason();
            cmbConcessionReason.SelectedValue = null;
            cmbConcessionReason.IsEnabled = false;
            //Added By Bhushanp 09032017
            //  txtApprovalRemark.IsEnabled = false;
            txtApprovalRemark.Text = string.Empty;
            txtApprovalRemark.ClearValidationError();
            cmbConcessionReason.TextBox.ClearValidationError();
        }

        /// <summary>
        /// Method is for searching and displaying the patient’s bill list in the grid based on specified criteria.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            bool res = true;

            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            {
                if (dtpFromDate.SelectedDate > dtpToDate.SelectedDate)
                {
                    dtpFromDate.SetValidation("From Date Should Be Less Than To Date");
                    dtpFromDate.RaiseValidationError();
                    string strMsg = "From Date Should Be Less Than To Date";
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    dtpFromDate.Focus();
                    res = false;
                }
                else
                {
                    dtpFromDate.ClearValidationError();
                }
            }

            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate == null)
            {
                dtpToDate.SetValidation("Plase Select To Date");
                dtpToDate.RaiseValidationError();
                string strMsg = "Plase Select To Date";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                dtpToDate.Focus();
                res = false;
            }
            else
            {
                dtpToDate.ClearValidationError();
            }

            if (dtpToDate.SelectedDate != null && dtpFromDate.SelectedDate == null)
            {
                dtpFromDate.SetValidation("Plase Select From Date");
                dtpFromDate.RaiseValidationError();
                string strMsg = "Plase Select From Date";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                dtpFromDate.Focus();
                res = false;
            }
            else
            {
                dtpFromDate.ClearValidationError();
            }
            if (res)
            {
                dgDataPager.PageIndex = 0;
                FillBillSearchList();
            }
        }

        /// <summary>
        /// Method is for Initializing all the control and data on the form
        /// </summary>
        private void InitialiseForm()
        {
            tabBillingSubInfo.SelectedIndex = 0;
            IsEditMode = false;
            PharmacyItems = new ObservableCollection<clsItemSalesDetailsVO>();
            dgPharmacyItems.ItemsSource = PharmacyItems;
            dgPharmacyItems.UpdateLayout();

            ChargeList = new ObservableCollection<clsChargeVO>();

            FollowUpDetails = new List<clsPatientFollowUpVO>();

            dgCharges.ItemsSource = ChargeList;
            dgCharges.Focus();
            dgCharges.UpdateLayout();

            StoreID = 0;
            txtClinicalTotal.Text = "";
            txtClinicalConcession.Text = "";
            txtClinicalNetAmount.Text = "";
            txtPharmacyTotal.Text = "";
            txtPharmacyConcession.Text = "";
            txtPharmacyNetAmount.Text = "";
            txtPharmacyVatAmount.Text = "";
            txtTotalBill.Text = "";
            txtTotalConcession.Text = "";
            txtNetAmount.Text = "";
            txtTotalClinicalBill.Text = "";
            txtTotalPharmacyBill.Text = "";
            txtPayAmount.Text = "";

            chkFreezBill.IsChecked = false;
            if (IsFromRequestApproval == false)
            {
                SelectedBill = null;
                BillID = 0;
                CurrentVisitTypeID = 0;
                if (dgBillList.SelectedItem != null)
                {
                    if (((clsBillVO)dgBillList.SelectedItem).IsRequestSend == true && ((clsBillVO)dgBillList.SelectedItem).IsApproved == true)
                    {
                        lnkAddItems.IsEnabled = false;
                        lnkAddItemsFromPrescription.IsEnabled = false;
                        lnkAddService.IsEnabled = false;
                        lnkAddServicesFromPrescription.IsEnabled = false;
                    }
                }
                else
                {
                    lnkAddItems.IsEnabled = true;
                    lnkAddItemsFromPrescription.IsEnabled = true;
                    lnkAddService.IsEnabled = true;
                    lnkAddServicesFromPrescription.IsEnabled = true;
                }
                chkFreezBill.IsEnabled = true;
                //txtApprovalRemark.IsReadOnly = true;
                //  txtApprovalRemark.IsEnabled = false;
            }
            else
            {
                lnkAddItems.IsEnabled = false;
                lnkAddItemsFromPrescription.IsEnabled = false;
                lnkAddService.IsEnabled = false;
                lnkAddServicesFromPrescription.IsEnabled = false;
                chkFreezBill.IsEnabled = false;
                //txtApprovalRemark.IsReadOnly = false;
                //txtApprovalRemark.IsEnabled = false;
            }
            PatientTariffID = 0;
            PatientSourceID = 0;
            PackageServiceList = new ObservableCollection<PalashDynamics.ValueObjects.clsPackageServiceDetailsVO>();
        }

        #region Set Command Button State New/Save/Modify/Print
        /// <summary>
        /// Method is for setting Command Button States based on strFormMode New/Save/Modify/Print
        /// </summary>
        /// <param name="strFormMode"></param>
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    IsCancel = true;
                    break;

                case "Save":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    IsCancel = true;
                    IsFromViewClick = false;
                    break;

                case "Modify":
                    if (IsFromRequestApproval == true)
                    {
                        cmdApprove.IsEnabled = true;
                        cmdModify.IsEnabled = false;
                        cmdNew.IsEnabled = false;
                        cmdSave.IsEnabled = false;
                    }
                    else
                    {
                        cmdNew.IsEnabled = false;
                        cmdSave.IsEnabled = false;
                        cmdModify.IsEnabled = true;
                    }
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    IsFromViewClick = false;
                    break;

                case "ClickNew":
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    IsCancel = false;
                    break;

                default:
                    break;
            }

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                  && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AllowClinicalTransaction == true)
                {
                    //do nothing
                }
                else
                {
                    cmdSave.IsEnabled = false;
                    cmdModify.IsEnabled = false;
                }
            }
        }

        #endregion

        private void dgBillList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        /// <summary>
        /// Method is for opening Prescription Items Search Window
        /// (Items which are specified in the prescription by doctor at the time of EMR).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkAddItemsFromPrescription_Click(object sender, RoutedEventArgs e)
        {
            DrugsForPatientPrescription DrugSearch = new DrugsForPatientPrescription();
            DrugSearch.StoreID = StoreID;
            if (StoreID == 0)
                DrugSearch.cmbStore.IsEnabled = true;
            else
                DrugSearch.cmbStore.IsEnabled = false;

            DrugSearch.VisitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmID;

            DrugSearch.OnSaveButton_Click += new RoutedEventHandler(DrugSearch_OnSaveButton_Click);
            DrugSearch.Show();
        }

        /// <summary>
        /// Method is for adding selected Items from Prescription Item search window to the patient’s bill details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DrugSearch_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            DrugsForPatientPrescription Itemswin = (DrugsForPatientPrescription)sender;
            if (Itemswin.DialogResult == true)
            {
                if (Itemswin.SelectedDrug != null && Itemswin.SelectedBatches != null)
                {
                    StringBuilder strError = new StringBuilder();
                    StoreID = Itemswin.StoreID;
                    //StoreID = Itemswin.StoreID;
                    foreach (var item in Itemswin.SelectedBatches)
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
                            PharmacyItems.Add(new clsItemSalesDetailsVO()
                            {
                                ItemID = Itemswin.SelectedDrug.DrugID,
                                ItemName = Itemswin.SelectedDrug.DrugName,

                                BatchID = item.BatchID,
                                BatchCode = item.BatchCode,
                                ExpiryDate = item.ExpiryDate,
                                Quantity = Convert.ToDouble(Itemswin.SelectedDrug.Quantity),
                                MRP = item.MRP,
                                Amount = (item.MRP * Convert.ToDouble(Itemswin.SelectedDrug.Quantity)),
                                AvailableQuantity = item.AvailableStock,
                                ConcessionPercentage = item.DiscountOnSale,
                                VATPercent = item.VATPerc
                            });
                        }
                    }
                    CalculatePharmacySummary();
                    dgPharmacyItems.Focus();
                    dgPharmacyItems.UpdateLayout();
                    dgPharmacyItems.SelectedIndex = PharmacyItems.Count - 1;

                    if (!string.IsNullOrEmpty(strError.ToString()))
                    {
                        string strMsg = "Items Already Added : " + strError.ToString();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                }
            }
        }

        /// <summary>
        /// Method is for opening bill form in edit mode for the selected bill in the grid from front panel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (DeleteBillAddedItems != null)
                {
                    DeleteBillAddedItems = new ObservableCollection<clsChargeVO>();
                    DeleteBillAddedItems.Clear();
                }
                if (dgBillList.SelectedItem != null)
                {
                    IsEditMode = true;
                    RowNum = 0;//Added By Bhushanp 17022017 use for delete service
                    InitialiseForm();
                    SetCommandButtonState("Modify");
                    IsFromViewClick = true;
                    rdbAgainstBill.IsChecked = true;
                    rdbAgainstBill.IsEnabled = false;
                    rdbAgainstServices.IsEnabled = false;
                    rdbDetail.IsChecked = true;
                    rdbInline.IsChecked = false;
                    SelectedBill = new clsBillVO();
                    SelectedBill = (clsBillVO)dgBillList.SelectedItem;
                    BillID = ((clsBillVO)dgBillList.SelectedItem).ID;
                    PatientSourceID = SelectedBill.PatientSourceId;
                    PatientTariffID = SelectedBill.TariffId;
                    cmbConcessionReason.SelectedValue = ((clsBillVO)dgBillList.SelectedItem).ConcessionReasonId;//Added By Yogesh K 21 Apr 16
                    txtApprovalRemark.Text = SelectedBill.ConcessionRemark.Trim().ToString(); //Added By Bhushanp 09032017
                    ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID = ((clsBillVO)dgBillList.SelectedItem).CompanyId;
                    PatientCategoryID = ((clsBillVO)dgBillList.SelectedItem).PatientCategoryId;
                    cmbConcessionReason.TextBox.ClearValidationError(); // Added by Bhushanp 20042017
                    cmbConcessionReason.IsEnabled = false;
                    if (((clsBillVO)dgBillList.SelectedItem).IsFreezed == true)
                    {
                        cmdModify.IsEnabled = false;
                        lnkAddItems.IsEnabled = false;
                        lnkAddItemsFromPrescription.IsEnabled = false;
                        lnkAddService.IsEnabled = false;
                        lnkAddServicesFromPrescription.IsEnabled = false;
                        lnkAddPackageService.IsEnabled = false;
                        chkFreezBill.IsEnabled = false;
                        cmbConcessionReason.IsEnabled = false;
                        if (SelectedBill.BillPaymentType == PalashDynamics.ValueObjects.BillPaymentTypes.AgainstBill)
                        {
                            rdbAgainstBill.IsChecked = true;
                            dgCharges.Columns[0].Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else if (SelectedBill.BillPaymentType == PalashDynamics.ValueObjects.BillPaymentTypes.AgainstServices)
                        {
                            rdbAgainstServices.IsChecked = true;
                            dgCharges.Columns[0].Visibility = System.Windows.Visibility.Visible;
                        }
                        FillChargeList(SelectedBill.ID, SelectedBill.UnitID, true, SelectedBill.Opd_Ipd_External_Id, SelectedBill.Opd_Ipd_External_UnitId, false);
                        FillPharmacyItemsList(SelectedBill.ID, SelectedBill.UnitID, true);
                        if (((clsBillVO)dgBillList.SelectedItem).BillPaymentType == PalashDynamics.ValueObjects.BillPaymentTypes.AgainstServices)
                            rdbAgainstServices.IsChecked = true;
                    }
                    else
                    {
                        rdbDraftBill.IsChecked = true;
                        FillChargeList(SelectedBill.ID, SelectedBill.UnitID, false, SelectedBill.Opd_Ipd_External_Id, SelectedBill.Opd_Ipd_External_UnitId, false);
                        FillPharmacyItemsList(SelectedBill.ID, SelectedBill.UnitID, false);
                    }
                    IsEditMode = true;
                    FillPatientAdvance(false);
                    chkFreezBill.IsChecked = ((clsBillVO)dgBillList.SelectedItem).IsFreezed;
                    chkFreezBill_Click(sender, e);
                    if (objUser.IpdBillAuthLvlID == 0 && IsEditMode == false)
                    {
                        cmbConcessionReason.IsEnabled = true;
                        //txtApprovalRemark.IsEnabled = true;
                    }
                    else
                    {
                        if (IsEditMode == true && dgBillList.SelectedItem != null)
                        {
                            if ((dgBillList.SelectedItem as clsBillVO).LevelID > 0 && objUser.IpdBillAuthLvlID != 0)
                            {
                                cmbConcessionReason.IsEnabled = true;
                                //txtApprovalRemark.IsEnabled = true;
                            }
                            else if (((dgBillList.SelectedItem as clsBillVO).LevelID == 1 || (dgBillList.SelectedItem as clsBillVO).LevelID == 2) && objUser.IpdBillAuthLvlID == 0)
                            {
                                cmbConcessionReason.IsEnabled = false;
                                // txtApprovalRemark.IsEnabled = false;
                            }
                        }
                    }

                    _flip.Invoke(RotationType.Forward);
                }
                GetAdmittedDoctor();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
        }

        /// <summary>
        /// Method is for opening the payment window it checks the dialog result of the payment window 
        /// if it is true call save bill method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            IsFromOtherServices = false;
            bool isValid = true;
            isValid = CheckValidations();
            if (isValid)
            {
                if (chkFreezBill.IsChecked == true)
                {
                    isValid = false;

                    if (((clsBillVO)dgBillList.SelectedItem).IsRequestSend == true && ((clsBillVO)dgBillList.SelectedItem).IsApproved == false)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Request for approval is already send", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                        msgWD_1.Show();
                    }
                    else
                    {
                        if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForConcessionID > 0)
                        {

                            //if (ApprovalConcenssion > 0 && ApprovalTotal > 0)     // commented on 21Feb2019 for Package Flow in IPD
                            //{
                            if ((ApprovalConcenssion > 0 && ApprovalTotal > 0) && ((objUser.IpdBillingAmmount == 0 ? true : objUser.IpdBillingAmmount < Convert.ToDecimal(ApprovalConcenssion)) || (objUser.IpdBillingPercentage == 0 ? true : (((Convert.ToDecimal(ApprovalConcenssion) / Convert.ToDecimal(ApprovalTotal)) * 100)) > objUser.IpdBillingPercentage)) && ((clsBillVO)dgBillList.SelectedItem).IsApproved == false) // Change By Bhushanp 09032017
                            {

                                MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Concession Exceeds The Authorised Amount,Are You Sure You Want To Send For Approval?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                msgWD_1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWD_1_OnMessageBoxClosed);
                                msgWD_1.Show();
                                //}
                            }
                            else
                            {


                                string msgText = "Are You Sure \n  You Want To Freeze The Bill ?";
                                MessageBoxControl.MessageBoxChildWindow msgWD =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                                msgWD.OnMessageBoxClosed += (arg) =>
                                {
                                    if (arg == MessageBoxResult.Yes)
                                    {
                                        PaymentWindow paymentWin = new PaymentWindow();
                                        paymentWin.TotalAmount = double.Parse(txtPayAmount.Text);
                                        paymentWin.Initiate("Bill");
                                        paymentWin.conID = ((clsBillVO)dgBillList.SelectedItem).ConcessionAuthorizedBy;
                                        paymentWin.txtPayTotalAmount.Text = this.txtNetAmount.Text;
                                        paymentWin.txtDiscAmt.Text = this.txtTotalConcession.Text;
                                        paymentWin.txtPayableAmt.Text = this.txtPayAmount.Text;
                                        paymentWin.Opd_Ipd_External = 1;
                                        paymentWin.PackageConcenssionAmt = PackageConcenssion;
                                        paymentWin.PatientCategoryID = PatientCategoryID;   // ((clsBillVO)dgBillList.SelectedItem).PatientCategoryId;
                                        if ((Convert.ToDouble(this.txtTotalConcession.Text) - PackageConcenssion) > 0.0)
                                        {
                                            paymentWin.ConcessionFromPlan = false;
                                        }
                                        else
                                        {
                                            paymentWin.ConcessionFromPlan = true;
                                        }

                                        if (rdbAgainstBill.IsChecked == true)
                                            paymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstBill;
                                        else
                                            paymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstServices;

                                        #region Added on 07Feb2019 for Package Flow in IPD

                                        if (ChargeList != null && ChargeList.Count > 0)
                                        {
                                            paymentWin.PackageID = ChargeList.Select(z => z.PackageID).FirstOrDefault();
                                            paymentWin.PackageBillID = ((clsBillVO)dgBillList.SelectedItem).ID;
                                            paymentWin.PackageBillUnitID = ((clsBillVO)dgBillList.SelectedItem).UnitID;
                                        }

                                        if (ChargeList != null && ChargeList.Count > 0)
                                        {
                                            var items12 = from r in ChargeList
                                                          where r.isPackageService == true
                                                          select r;

                                            if (items12.ToList().Count > 0)
                                            {
                                                paymentWin.IsNoAutoAdvanceConsume = true;       // Set to call auto collect advance or not : added on 22062018
                                            }
                                        }

                                        #endregion

                                        paymentWin.OnSaveButton_Click += new RoutedEventHandler(paymentWin_OnSaveButton_Click);
                                        paymentWin.Show();
                                    }
                                };
                                msgWD.Show();
                            }
                        }
                        else
                        {
                            string msgText = "Are You Sure \n  You Want To Freeze The Bill ?";
                            MessageBoxControl.MessageBoxChildWindow msgWD =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                            msgWD.OnMessageBoxClosed += (arg) =>
                            {
                                if (arg == MessageBoxResult.Yes)
                                {
                                    PaymentWindow paymentWin = new PaymentWindow();
                                    paymentWin.TotalAmount = double.Parse(txtPayAmount.Text);
                                    paymentWin.Initiate("Bill");
                                    paymentWin.conID = ((clsBillVO)dgBillList.SelectedItem).ConcessionAuthorizedBy;
                                    paymentWin.txtPayTotalAmount.Text = this.txtNetAmount.Text;
                                    paymentWin.txtDiscAmt.Text = this.txtTotalConcession.Text;
                                    paymentWin.txtPayableAmt.Text = this.txtPayAmount.Text;
                                    paymentWin.Opd_Ipd_External = 1;
                                    paymentWin.PackageConcenssionAmt = PackageConcenssion;
                                    paymentWin.PatientCategoryID = PatientCategoryID;       // ((clsBillVO)dgBillList.SelectedItem).PatientCategoryId;
                                    if ((Convert.ToDouble(this.txtTotalConcession.Text) - PackageConcenssion) > 0.0)
                                    {
                                        paymentWin.ConcessionFromPlan = false;
                                    }
                                    else
                                    {
                                        paymentWin.ConcessionFromPlan = true;
                                    }

                                    if (rdbAgainstBill.IsChecked == true)
                                        paymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstBill;
                                    else
                                        paymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstServices;

                                    if (ChargeList != null && ChargeList.Count > 0)     // Added on 07Feb2019 for Package Flow in IPD
                                    {
                                        paymentWin.PackageID = ChargeList.Select(z => z.PackageID).FirstOrDefault();
                                        paymentWin.PackageBillID = ((clsBillVO)dgBillList.SelectedItem).ID;
                                        paymentWin.PackageBillUnitID = ((clsBillVO)dgBillList.SelectedItem).UnitID;
                                    }

                                    paymentWin.OnSaveButton_Click += new RoutedEventHandler(paymentWin_OnSaveButton_Click);
                                    paymentWin.Show();
                                }
                            };
                            msgWD.Show();
                        }
                    }
                }
                else
                {
                    if (((clsBillVO)dgBillList.SelectedItem).IsRequestSend == true && ((clsBillVO)dgBillList.SelectedItem).IsApproved == false)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Request for approval is already send", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                        msgWD_1.Show();
                    }
                    else
                    {
                        SaveBill(null, false);
                    }
                }
            }
        }

        /// <summary>
        /// Facility is given for freezing the bill from front panel itself.
        /// Method is for opening the payment window it checks the dialog result of the payment window
        /// if it is true call save bill method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {

            if (dgBillList.SelectedItem != null && ((clsBillVO)dgBillList.SelectedItem).IsFreezed == true)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID = ((clsBillVO)dgBillList.SelectedItem).CompanyId;
                PatientCategoryID = ((clsBillVO)dgBillList.SelectedItem).PatientCategoryId;
                FillChargeListForFrontGrid(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, false, ((clsBillVO)dgBillList.SelectedItem).Opd_Ipd_External_Id, ((clsBillVO)dgBillList.SelectedItem).Opd_Ipd_External_UnitId, false);
            }
        }

        private void FreezBill()
        {
            InitialiseForm();
            SelectedBill = new clsBillVO();
            SelectedBill = (clsBillVO)dgBillList.SelectedItem;
            if ((MasterListItem)cmbConcessionReason.SelectedItem != null) //Added By Bhushan 27012017
            {
                ((MasterListItem)cmbConcessionReason.SelectedItem).ID = SelectedBill.ConcessionReasonId;
            }
            txtApprovalRemark.Text = Convert.ToString(SelectedBill.ConcessionRemark); //Added By Bhushanp 2052017
            //FillChargeList(SelectedBill.ID, SelectedBill.UnitID, false, SelectedBill.Opd_Ipd_External_Id, SelectedBill.Opd_Ipd_External_UnitId, false); //Commented By Bhushanp 
            FillChargeList_Duplicate(SelectedBill.ID, SelectedBill.UnitID, false, SelectedBill.Opd_Ipd_External_Id, SelectedBill.Opd_Ipd_External_UnitId, false);// Added By Bhushanp for freeze the bill of outside of Bill
            FillPharmacyItemsList(SelectedBill.ID, SelectedBill.UnitID, false);
            //string msgText = "Are You Sure \n  You Want To Freeze The Bill ?";
            //MessageBoxControl.MessageBoxChildWindow msgWD =
            //    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            //msgWD.OnMessageBoxClosed += (arg) =>
            //{
            //    if (arg == MessageBoxResult.Yes)
            //    {
            //        bool isValid;
            //        isValid = CheckValidations();
            //        if (isValid)
            //        {
            //            chkFreezBill.IsChecked = true;
            //            flagFreezFromSearch = true;
            //            PaymentWindow paymentWin = new PaymentWindow();
            //            if (!(txtPayAmount.Text == null) && !(txtPayAmount.Text.Trim().Length == 0))
            //                paymentWin.TotalAmount = double.Parse(txtPayAmount.Text);
            //            paymentWin.Initiate("Bill");
            //            paymentWin.txtPayTotalAmount.Text = this.txtNetAmount.Text;
            //            paymentWin.txtDiscAmt.Text = this.txtTotalConcession.Text;
            //            paymentWin.txtPayableAmt.Text = this.txtPayAmount.Text;
            //            paymentWin.conID = ((clsBillVO)dgBillList.SelectedItem).ConcessionAuthorizedBy;
            //            paymentWin.PatientCategoryID = PatientCategoryID;
            //            paymentWin.Opd_Ipd_External = 1;
            //            paymentWin.PackageConcenssionAmt = PackageConcenssion;
            //            if ((Convert.ToDouble(this.txtTotalConcession.Text) - PackageConcenssion) > 0.0)
            //            {
            //                paymentWin.ConcessionFromPlan = false;
            //            }
            //            else
            //            {
            //                paymentWin.ConcessionFromPlan = true;
            //            }
            //            if (rdbAgainstBill.IsChecked == true)
            //            {
            //                paymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstBill;
            //            }
            //            else
            //                paymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstServices;
            //            paymentWin.OnSaveButton_Click += new RoutedEventHandler(paymentWin_OnSaveButton_Click);
            //            paymentWin.OnCancelButton_Click += new RoutedEventHandler(paymentWin_OnCancelButton2_Click);
            //            paymentWin.Show();
            //        }
            //        else
            //        {
            //            InitialiseForm();
            //            ((clsBillVO)dgBillList.SelectedItem).IsFreezed = false;
            //        }
            //    }
            //    else
            //    {
            //        InitialiseForm();
            //        ((clsBillVO)dgBillList.SelectedItem).IsFreezed = false;
            //    }
            //};
            //msgWD.Show();
        }
        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Method is for printing the Clinical bill.
        /// </summary>
        /// <param name="iBillId"></param>
        /// <param name="iUnitID"></param>
        ///    

        private void PrintBill(long iBillId, long iUnitID, long PrintID)
        {
            bool IsBilled = true;
            if (PrintID == 3)
            {
                IsBilled = false;
            }
            if (iBillId > 0 && rdbInline.IsChecked == false)
            {
                long UnitID = iUnitID;
                string URL = "../Reports/OPD/ClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&PrintFomatID=" + PrintID + "&IsBilled=" + IsBilled;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
            else if (iBillId > 0 && rdbInline.IsChecked == true)
            {
                long UnitID = iUnitID;
                long isInline = 1;
                string URL = "../Reports/OPD/ClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&PrintFomatID=" + PrintID + "&IsBilled=" + IsBilled + "&isInline=" + isInline;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

            }
        }
        /// <summary>
        /// Method calls the PrintBill and PrintPharmacyBill method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgBillList.SelectedItem != null)
            {
                SelectedBill = new clsBillVO();
                SelectedBill = (clsBillVO)dgBillList.SelectedItem;
                if (SelectedBill.IsFreezed == true)
                {

                    if (SelectedBill.BillType == PalashDynamics.ValueObjects.BillTypes.Clinical)
                    {
                        if (((clsBillVO)dgBillList.SelectedItem).IsPackageServiceInclude == true)
                        {
                            string msgText = "Are You Sure  Want To Print Details Bill ?";
                            MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgWD.OnMessageBoxClosed += (arg) =>
                            {
                                if (arg == MessageBoxResult.Yes)
                                {
                                    rdbInline.IsChecked = false;
                                    PrintBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, 4);
                                }
                                else
                                {
                                    rdbInline.IsChecked = true;
                                    PrintBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, 4);
                                }
                            };
                            msgWD.Show();
                        }
                        else
                        {
                            PrintBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, 4);
                        }
                    }
                    else if (SelectedBill.BillType == PalashDynamics.ValueObjects.BillTypes.Pharmacy)
                    {
                        PrintPharmacyBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, 4);
                    }
                    else
                    {
                        PrintBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, 4);
                        PrintPharmacyBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, 4);
                    }
                }
                else
                {
                    PrintBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, 3);
                }
            }
        }

        /// <summary>
        /// Method is for printing the Pharmacy bill.
        /// </summary>
        /// <param name="iBillId"></param>
        /// <param name="IUnitID"></param>
        private void PrintPharmacyBill(long iBillId, long IUnitID, long PrintID)
        {
            if (iBillId > 0)
            {
                long UnitID = IUnitID;
                string URL = "../Reports/OPD/PharmacyBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&ReportID=" + 2 + "&PrintFomatID=" + PrintID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        /// <summary>
        /// Method is for opening Prescription Services Search Window 
        /// (Services which are specified in the prescription by doctor at the time of EMR).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkAddServicesFromPrescription_Click(object sender, RoutedEventArgs e)
        {
            PrescriptionServicesForPatient PrescriptionService = new PrescriptionServicesForPatient();
            PrescriptionService.IsOPDIPD = true;
            PrescriptionService.OnAddButton_Click += new RoutedEventHandler(PrescriptionService_OnAddButton_Click);
            PrescriptionService.Show();
        }

        /// <summary>
        /// Method is for adding selected services from Prescription Service search window to the patient’s bill details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PrescriptionService_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (((PrescriptionServicesForPatient)sender).DialogResult == true)  //if (((PrescriptionServicesForPatient)sender).DialogResult == true)
            {
                List<clsServiceMasterVO> lServices = new List<clsServiceMasterVO>();
                lServices = ((PrescriptionServicesForPatient)sender).SelectedServices;  //((PrescriptionServicesForPatient)sender).SelectedServices; 

                bool IsMultiSponser = false;
                bool IscomboPackage = false;    // Added on 07Feb2019 for Package Flow in IPD

                if (IsAuditTrail)
                {
                    LogInformation = new LogInfo();   // first time data in grid
                    LogInformation.guid = objGUID;
                    LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    LogInformation.TimeStamp = DateTime.Now;
                    LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                    LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                    LogInformation.Message = " 11 : Selected Services From Prescription " //+ Convert.ToString(lineNumber)
                                                            + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                                                            + " , IsMultiSponser : " + IsMultiSponser
                                                            ;
                }

                var item5 = from p in ChargeList
                            where p.IsDefaultService == true
                            select p;
                if (item5.ToList().Count != ChargeList.Count)
                {
                    foreach (var item in lServices)
                    {
                        var item1 = from r in ChargeList
                                    where r.CompanyID == item.CompanyID && r.PatientSourceID == item.PatientSourceID && r.TariffId == item.TariffID && r.IsDefaultService == false
                                    select r;

                        if (item1.ToList().Count > 0)
                        {
                            IsMultiSponser = false;
                        }
                        else
                        {
                            IsMultiSponser = true;
                        }
                        LogInformation.Message = LogInformation.Message + " , IsMultiSponser : " + IsMultiSponser;
                        LogInfoList.Add(LogInformation);
                        break;
                    }

                    #region Added on 07Feb2019 for Package Flow in IPD

                    var item52 = from p in lServices
                                 where p.PackageID > 0
                                 select p;

                    if (item52.ToList().Count > 0)      // if service selected under package : added on 13082018
                    {
                        foreach (var item in lServices)
                        {
                            var item12 = from r in ChargeList
                                         where r.CompanyID == item.CompanyID && r.PatientSourceID == item.PatientSourceID && r.TariffId == item.TariffID && r.PackageID == item.PackageID && r.IsDefaultService == false
                                         select r;

                            if (item12.ToList().Count > 0)
                            {
                                IscomboPackage = false;
                            }
                            else
                            {
                                IscomboPackage = true;      // Sets true when non-package & package ; or multi - package services find while add
                            }
                            break;
                        }
                    }

                    #endregion
                }

                //if (IsMultiSponser == false)      // commented on 07Feb2019 for Package Flow in IPD
                if (IsMultiSponser == false && IscomboPackage == false)        // modified on 07Feb2019 for Package Flow in IPD
                {

                    #region clsApplyPackageDiscountRateOnServiceBizActionVO II

                    clsApplyPackageDiscountRateOnServiceBizActionVO objApplyNewRate = new clsApplyPackageDiscountRateOnServiceBizActionVO();

                    objApplyNewRate.objApplyPackageDiscountRate = new List<clsApplyPackageDiscountRateOnServiceVO>();
                    objApplyNewRate.ipServiceList = new List<clsServiceMasterVO>();

                    objApplyNewRate.ipLoginUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    objApplyNewRate.ipPatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    objApplyNewRate.ipPatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;

                    objApplyNewRate.ipVisitID = AdmissionID;

                    objApplyNewRate.ipPatientGenderID = ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID;

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.IsAge == true)
                    {
                        objApplyNewRate.ipPatientDateOfBirth = ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirthFromAge;
                    }
                    else
                    {
                        objApplyNewRate.ipPatientDateOfBirth = ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth;
                    }

                    objApplyNewRate.ipServiceList = lServices;

                    if ((PrescriptionServicesForPatient)sender != null && ((PrescriptionServicesForPatient)sender).cmbPatientSource.SelectedItem != null && ((clsPatientSponsorVO)((PrescriptionServicesForPatient)sender).cmbPatientSource.SelectedItem).MemberRelationID > 0)
                        objApplyNewRate.MemberRelationID = ((clsPatientSponsorVO)((PrescriptionServicesForPatient)sender).cmbPatientSource.SelectedItem).MemberRelationID;

                    objApplyNewRate.SponsorID = ((MasterListItem)((PrescriptionServicesForPatient)sender).cmbTariff.SelectedItem).SponsorID;
                    objApplyNewRate.SponsorUnitID = ((MasterListItem)((PrescriptionServicesForPatient)sender).cmbTariff.SelectedItem).SponsorUnitID;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            objApplyNewRate = (clsApplyPackageDiscountRateOnServiceBizActionVO)arg.Result;

                            UnavailableItemStockService = new List<clsUnavailableItemStockServiceId>();

                            foreach (clsServiceMasterVO item in lServices)
                            {
                                if (item.ConditionID > 0)
                                {
                                    if (item.ConditionalQuantity > item.ConditionalUsedQuantity && item.ConditionType == "AND" && item.IsAgeApplicable == true && item.ServiceMemberRelationID > 0)  //if (item.ConditionalQuantity > item.ConditionalUsedQuantity)
                                    {
                                        item.ConcessionPercent = 100;   //Convert.ToDecimal(item1.DiscountedPercentage);
                                        ConcessionFromPlan = true;
                                    }
                                    else if (item.ConditionalQuantity > (item.ConditionalUsedQuantity + item.MainServiceUsedQuantity) && item.ConditionType == "OR" && item.IsAgeApplicable == true && item.ServiceMemberRelationID > 0)  //if (item.ConditionalQuantity > item.ConditionalUsedQuantity)
                                    {
                                        item.ConcessionPercent = 100;   //Convert.ToDecimal(item1.DiscountedPercentage);
                                        ConcessionFromPlan = true;
                                    }
                                    else
                                    {
                                        if ((item.IsAgeApplicable == true && item.ServiceMemberRelationID > 0))  //&& item.TariffID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID
                                        {
                                            item.ConcessionAmount = 0;
                                            item.ConcessionPercent = Convert.ToDecimal(item.ConditionalDiscount);
                                            ConcessionFromPlan = true;
                                        }
                                        if ((item.IsAgeApplicable == false || item.ServiceMemberRelationID == 0))  //item1.ActualQuantity > 0 &&
                                        {
                                            item.ConcessionPercent = 0;
                                            item.ConcessionAmount = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (clsApplyPackageDiscountRateOnServiceVO item1 in objApplyNewRate.objApplyPackageDiscountRate)
                                    {
                                        //if (item.ID == item1.ServiceID && item.TariffID == item1.TariffID && item.ConditionID == 0) // Commented on 07Feb2019 for Package Flow in IPD
                                        if (item.ID == item1.ServiceID && item.TariffID == item1.TariffID && item.ConditionID == 0 && item.ProcessID == item1.ProcessID)    // Added on 07Feb2019 for Package Flow in IPD
                                        {

                                            if (item1.IsApplyOn_Rate_Percentage == 1 || item1.IsApplyOn_Rate_Percentage == 3 && item.ConditionID == 0)
                                            {
                                                item.Rate = Convert.ToDecimal(item1.DiscountedRate);

                                                if ((item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0) && item1.PackageID > 0)  //By CDS   // item1.TariffID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID
                                                {
                                                    item.ConcessionAmount = 0;
                                                    item.ConcessionPercent = Convert.ToDecimal(item1.ConcessionPercentage);
                                                }
                                                if (item1.IsDiscountOnQuantity == true || item1.ServiceMemberRelationID == 0 || item1.IsAgeApplicable == false)
                                                {
                                                    item.ConcessionPercent = 0;
                                                    item.ConcessionAmount = 0;
                                                }
                                            }

                                            if (item1.IsApplyOn_Rate_Percentage == 2 || item1.IsApplyOn_Rate_Percentage == 3 && item.ConditionID == 0)
                                            {
                                                if (item1.IsDiscountOnQuantity == false)
                                                {
                                                    if (item.PackageServiceConditionID == 0)
                                                    {
                                                        if (item1.ActualQuantity > item1.UsedQuantity && item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0)
                                                        {
                                                            item.ConcessionPercent = Convert.ToDecimal(item1.DiscountedPercentage);

                                                            ConcessionFromPlan = true; //Added by priyanka-to set validation on concession
                                                        }
                                                        else
                                                        {
                                                            if ((item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0) && item1.PackageID > 0)   //By CDS   item1.TariffID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID
                                                            {
                                                                item.ConcessionAmount = 0;
                                                                item.ConcessionPercent = Convert.ToDecimal(item1.ConcessionPercentage);
                                                            }

                                                            if ((item1.IsAgeApplicable == false || item1.ServiceMemberRelationID == 0))  //item1.ActualQuantity > 0 &&
                                                            {
                                                                item.ConcessionPercent = 0;
                                                                item.ConcessionAmount = 0;
                                                            }
                                                        }
                                                    }
                                                    else if (item.PackageServiceConditionID > 0)
                                                    {
                                                        if (item1.ActualQuantity > (item1.UsedQuantity + item.TotalORUsedQuantity) && item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0)
                                                        {
                                                            item.ConcessionPercent = Convert.ToDecimal(item1.DiscountedPercentage);

                                                            ConcessionFromPlan = true; //Added by priyanka-to set validation on concession
                                                        }
                                                        else
                                                        {
                                                            if ((item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0) && item1.PackageID > 0)    //By CDS // item1.TariffID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID
                                                            {
                                                                item.ConcessionAmount = 0;
                                                                item.ConcessionPercent = Convert.ToDecimal(item1.ConcessionPercentage);
                                                            }

                                                            if ((item1.IsAgeApplicable == false || item1.ServiceMemberRelationID == 0))  //item1.ActualQuantity > 0 && 
                                                            {
                                                                item.ConcessionPercent = 0;
                                                                item.ConcessionAmount = 0;
                                                            }
                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    if (item1.ActualQuantity > (item1.UsedQuantity + item.TotalORUsedQuantity) && item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0)
                                                    {
                                                        if ((item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0) && item1.PackageID > 0)  //By CDS // item1.TariffID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID
                                                        {
                                                            item.ConcessionAmount = 0;
                                                            item.ConcessionPercent = Convert.ToDecimal(item1.ConcessionPercentage);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        item.ConcessionPercent = 0;
                                                        item.ConcessionAmount = 0;
                                                    }
                                                }
                                            }

                                            if (item.IsMarkUp == true && item1.IsServiceItemStockAvailable != true && item.ConditionID == 0)
                                            {
                                                UnavailableItemStockService.Add(new clsUnavailableItemStockServiceId { ServiceID = item.ServiceID });
                                            }

                                        }
                                    }

                                }
                            }

                            StringBuilder sbServiceName = new StringBuilder();

                            foreach (clsUnavailableItemStockServiceId item in UnavailableItemStockService)
                            {
                                clsServiceMasterVO obj = new clsServiceMasterVO();
                                obj = (clsServiceMasterVO)lServices.First(z => z.ServiceID == item.ServiceID && z.IsMarkUp == true);

                                sbServiceName.Append(obj.ServiceName + ",");

                                lServices.Remove(obj);
                            }
                            if (sbServiceName.Length > 0)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgWD =
                                                new MessageBoxControl.MessageBoxChildWindow("", "Items are not available for Service : " + sbServiceName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                msgWD.OnMessageBoxClosed += (re) =>
                                {
                                    if (re == MessageBoxResult.OK)
                                    {
                                        PatientCategoryID = ((PrescriptionServicesForPatient)sender).PatientCategoryL1Id_Retail; //((ServiceSearch)sender).PatientCategoryL1Id_Retail;
                                        PatientSourceID = ((clsPatientSponsorVO)((PrescriptionServicesForPatient)sender).cmbPatientSource.SelectedItem).PatientSourceID;   //((clsPatientSponsorVO)((ServiceSearch)sender).cmbPatientSource.SelectedItem).PatientSourceID;
                                        PatientTariffID = ((MasterListItem)((PrescriptionServicesForPatient)sender).cmbTariff.SelectedItem).ID;  //((MasterListItem)((ServiceSearch)sender).cmbTariff.SelectedItem).ID;
                                        PatientCompanyID = ((MasterListItem)((PrescriptionServicesForPatient)sender).cmbCompany.SelectedItem).ID;  //((MasterListItem)((ServiceSearch)sender).cmbTariff.SelectedItem).ID;
                                        PackageTariffID = ((PrescriptionServicesForPatient)sender).PackageTariffID; //((ServiceSearch)sender).PackageTariffID;
                                        ServiceTariffID = ((PrescriptionServicesForPatient)sender).ServiceTariffID;  //((ServiceSearch)sender).ServiceTariffID;
                                        AddCharges(lServices, ServiceTariffID);
                                    }
                                };
                                msgWD.Show();
                            }
                            else
                            {

                                PatientCategoryID = ((PrescriptionServicesForPatient)sender).PatientCategoryL1Id_Retail; //((ServiceSearch)sender).PatientCategoryL1Id_Retail;
                                PatientSourceID = ((clsPatientSponsorVO)((PrescriptionServicesForPatient)sender).cmbPatientSource.SelectedItem).PatientSourceID;  //((clsPatientSponsorVO)((ServiceSearch)sender).cmbPatientSource.SelectedItem).PatientSourceID;
                                PatientTariffID = ((MasterListItem)((PrescriptionServicesForPatient)sender).cmbTariff.SelectedItem).ID;  //((MasterListItem)((ServiceSearch)sender).cmbTariff.SelectedItem).ID;

                                PatientCompanyID = ((MasterListItem)((PrescriptionServicesForPatient)sender).cmbCompany.SelectedItem).ID;

                                PackageTariffID = ((PrescriptionServicesForPatient)sender).PackageTariffID;  //((ServiceSearch)sender).PackageTariffID;
                                ServiceTariffID = ((PrescriptionServicesForPatient)sender).ServiceTariffID;  //((ServiceSearch)sender).ServiceTariffID;

                                AddCharges(lServices, ServiceTariffID);
                            }

                            if (IsAuditTrail)
                            {
                                LogInformation.Message = LogInformation.Message +
                                                                      " , Patient Category ID : " + Convert.ToString(PatientCategoryID)
                                                                    + " , Patient Source ID : " + Convert.ToString(PatientSourceID)
                                                                    + " , Patient Tariff ID : " + Convert.ToString(PatientTariffID)
                                                                    + " , Patient Company ID : " + Convert.ToString(PatientCompanyID)
                                                                    + " , Package Tariff ID : " + Convert.ToString(PackageTariffID)
                                                                    + " , Service Tariff ID : " + Convert.ToString(ServiceTariffID);
                                foreach (clsServiceMasterVO item in lServices)
                                {
                                    LogInformation.Message = LogInformation.Message +
                                                                      " , Service ID : " + Convert.ToString(item.ID)
                                                                    + " , Service Code : " + Convert.ToString(item.ServiceCode)
                                                                    + " , Service Name : " + Convert.ToString(item.ServiceName)
                                                                    + " , Long Description : " + Convert.ToString(item.LongDescription)
                                                                    + " , Patient Category L3 : " + Convert.ToString(item.PatientCategoryL3)
                                                                    + " , Patient Source ID : " + Convert.ToString(item.PatientSourceID)
                                                                    + " , Rate : " + Convert.ToString(item.Rate)
                                                                    + " , Specialization ID : " + Convert.ToString(item.Specialization)
                                                                    + " , Specialization : " + Convert.ToString(item.SpecializationString)
                                                                    + " , SubSpecialization ID : " + Convert.ToString(item.SubSpecialization)
                                                                    + " , SubSpecialization : " + Convert.ToString(item.SubSpecializationString)
                                                                    + " , Tariff ID : " + Convert.ToString(item.TariffID)
                                                                    + " , Tariff Service Master ID : " + Convert.ToString(item.TariffServiceMasterID) + "\r\n"
                                                                    ;
                                }

                                LogInfoList.Add(LogInformation);
                            }
                        }

                        if (IsMsgServiceConsumption)        // Added on 07Feb2019 for Package Flow in IPD
                        {
                            IsMsgServiceConsumption = false;
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Some services already consume under this package, Create new bill for already consume services.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }

                    };
                    client.ProcessAsync(objApplyNewRate, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();

                    #endregion
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW2 =
                                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Multiple Sponsor Billing is not Allowed", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW2.Show();
                }
            }


        }

        /// <summary>
        /// Method is for adding service details in the bill
        /// </summary>
        /// <param name="mServices"></param>
        private void AddCharges(List<clsServiceMasterVO> mServices)
        {
            StringBuilder strError = new StringBuilder();
            for (int i = 0; i < mServices.Count; i++)
            {
                bool Addcharge = true;
                if (ChargeList != null && ChargeList.Count > 0)
                {
                    var item = from r in ChargeList
                               where r.ServiceId == mServices[i].ID
                               select new clsChargeVO
                               {
                                   Status = r.Status,
                                   ID = r.ID,
                                   ServiceName = r.ServiceName
                               };

                    if (item.ToList().Count > 0)
                    {
                        if (strError.ToString().Length > 0)
                            strError.Append(",");
                        strError.Append(item.ToList()[0].ServiceName);
                        Addcharge = false;
                    }
                }

                if (Addcharge)
                {
                    clsChargeVO itemC = new clsChargeVO();

                    itemC.ServiceSpecilizationID = mServices[i].Specialization;
                    itemC.TariffServiceId = mServices[i].TariffServiceMasterID;
                    itemC.ServiceId = mServices[i].ID;
                    itemC.ServiceName = mServices[i].ServiceName;
                    itemC.Quantity = 1;
                    itemC.Date = DateTime.Now;
                    itemC.RateEditable = mServices[i].RateEditable;
                    itemC.MinRate = Convert.ToDouble(mServices[i].MinRate);
                    itemC.MaxRate = Convert.ToDouble(mServices[i].MaxRate);
                    itemC.Rate = Convert.ToDouble(mServices[i].Rate);
                    itemC.InitialRate = itemC.Rate;  // to maintain the Initial Rate of Service after changing Doctor Service Rate.
                    itemC.TariffId = mServices[i].TariffID;
                    itemC.isPackageService = mServices[i].IsPackage;
                    itemC.IsPrescribedService = mServices[i].IsPrescribedService;
                    itemC.TotalAmount = itemC.Rate * itemC.Quantity;
                    itemC.ServiceCode = mServices[i].ServiceCode;

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 3 || ((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 6)
                    {    //Staff Or Staff Dependent
                        if (mServices[i].StaffDiscountPercent > 0)
                            itemC.StaffDiscountPercent = Convert.ToDouble(mServices[i].StaffDiscountPercent);
                        else
                            itemC.StaffDiscountAmount = Convert.ToDouble(mServices[i].StaffDiscountAmount);

                        if (mServices[i].StaffDependantDiscountPercent > 0)
                            itemC.StaffParentDiscountPercent = Convert.ToDouble(mServices[i].StaffDependantDiscountPercent);
                        else
                            itemC.StaffParentDiscountAmount = Convert.ToDouble(mServices[i].StaffDependantDiscountAmount);

                        if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.ApplyConcessionToStaff == true)
                        {
                            if (mServices[i].ConcessionPercent > 0)
                            {
                                itemC.ConcessionPercent = Convert.ToDouble(mServices[i].ConcessionPercent);
                            }
                            else
                                itemC.Concession = Convert.ToDouble(mServices[i].ConcessionAmount);
                        }
                    }
                    else
                    {
                        if (mServices[i].ConcessionPercent > 0)
                        {
                            itemC.ConcessionPercent = Convert.ToDouble(mServices[i].ConcessionPercent);
                        }
                        else
                            itemC.Concession = Convert.ToDouble(mServices[i].ConcessionAmount);
                    }

                    if (mServices[i].ServiceTaxPercent > 0)
                        itemC.ServiceTaxPercent = Convert.ToDouble(mServices[i].ServiceTaxPercent);
                    else
                        itemC.ServiceTaxAmount = Convert.ToDouble(mServices[i].ServiceTaxAmount);
                    itemC.Doctor = DoctorList;
                    ChargeList.Add(itemC);
                }
            }
            CalculateClinicalSummary();
            dgCharges.Focus();
            dgCharges.UpdateLayout();
            dgCharges.SelectedIndex = ChargeList.Count - 1;
            if (!string.IsNullOrEmpty(strError.ToString()))
            {
                string strMsg = "Services Already Added : " + strError.ToString();
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        private void rdbAgainstBill_Click(object sender, RoutedEventArgs e)
        {
            if (rdbAgainstBill.IsChecked == true)

                dgCharges.Columns[0].Visibility = System.Windows.Visibility.Collapsed;

            else if (rdbAgainstServices.IsChecked == true)
                dgCharges.Columns[0].Visibility = System.Windows.Visibility.Visible;
        }

        private void chkFreezBill_Click(object sender, RoutedEventArgs e)
        {
            switch (chkFreezBill.IsChecked)
            {
                case true:
                    rdbAgainstServices.IsEnabled = rdbAgainstBill.IsEnabled = true;
                    rdbFinalBill.IsChecked = true;
                    rdbInline.IsEnabled = true;
                    break;
                case false:
                    rdbAgainstServices.IsEnabled = rdbAgainstBill.IsEnabled = false;
                    rdbAgainstBill.IsChecked = true;
                    rdbDraftBill.IsChecked = true;
                    rdbInline.IsEnabled = false;
                    break;
                default:
                    rdbAgainstServices.IsEnabled = rdbAgainstBill.IsEnabled = false;
                    rdbAgainstBill.IsChecked = true;
                    rdbDraftBill.IsChecked = true;
                    break;
            }
        }

        private void chkPayService_Click(object sender, RoutedEventArgs e)
        {
            CalculateTotalSummary();
        }

        /// <summary>
        /// Method calls the function settlebill for the selected bill in the grid from front panel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSettle_Click(object sender, RoutedEventArgs e)
        {
            if (dgBillList.SelectedItem != null)
            {
                if (((clsBillVO)dgBillList.SelectedItem).IsFreezed == true)
                    SettleBill();
                else
                {
                    string msgText = "Only Freezed Bills Can Be Settled ?";
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWD.Show();
                }
            }
        }

        /// <summary>
        /// Method is for opening the Payment Window for Bill settlement.
        /// </summary>
        void SettleBill()
        {
            if (dgBillList.SelectedItem != null && ((clsBillVO)dgBillList.SelectedItem).IsFreezed == true)
            {
                InitialiseForm();
                SelectedBill = new clsBillVO();
                SelectedBill = (clsBillVO)dgBillList.SelectedItem;
                string msgText = "Are You Sure \n You Want To Settle The Bill ?";
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWD.OnMessageBoxClosed += (arg) =>
                {
                    if (arg == MessageBoxResult.Yes)
                    {
                        bool isValid = true;
                        if (isValid)
                        {
                            chkFreezBill.IsChecked = true;
                            flagFreezFromSearch = true;
                            rdbAgainstBill.IsChecked = true;
                            PaymentWindow SettlePaymentWin = new PaymentWindow();
                            if (SelectedBill.BalanceAmountSelf > 0)
                                SettlePaymentWin.TotalAmount = SelectedBill.BalanceAmountSelf;

                            SettlePaymentWin.Initiate("Bill");

                            SettlePaymentWin.txtPayTotalAmount.Text = SelectedBill.NetBillAmount.ToString();
                            SettlePaymentWin.txtDiscAmt.Text = SelectedBill.TotalConcessionAmount.ToString();
                            SettlePaymentWin.txtPayableAmt.Text = SelectedBill.BalanceAmountSelf.ToString();

                            SettlePaymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstBill;

                            if (ChargeList != null && ChargeList.Count > 0)     // Added on 11Feb2019 for Package Flow in IPD
                            {
                                SettlePaymentWin.PackageID = ChargeList.Select(z => z.PackageID).FirstOrDefault();
                                SettlePaymentWin.PackageBillID = ((clsBillVO)dgBillList.SelectedItem).ID;
                                SettlePaymentWin.PackageBillUnitID = ((clsBillVO)dgBillList.SelectedItem).UnitID;
                            }

                            SettlePaymentWin.OnSaveButton_Click += new RoutedEventHandler(SettlePaymentWin_OnSaveButton_Click);
                            SettlePaymentWin.Show();
                        }
                        else
                        {
                            InitialiseForm();
                        }
                    }
                    else
                    {
                        InitialiseForm();
                    }
                };
                msgWD.Show();
            }
        }

        long PaymentID = 0;

        /// <summary>
        /// This method check the dialog result of Payment Window if it is true
        /// then it creates instance of type clsUpdateBillPaymentDtlsBizActionVO
        /// class,assign the details to it and call service ProcessAsync() method for updating Bill Payment Details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SettlePaymentWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((CIMS.Forms.PaymentWindow)sender).DialogResult == true)
                {
                    if (((PaymentWindow)sender).Payment != null)
                    {
                        Indicatior = new WaitIndicator();
                        Indicatior.Show();
                        clsAddPaymentBizActionVO BizAction = new clsAddPaymentBizActionVO();
                        clsPaymentVO pPayDetails = new clsPaymentVO();

                        pPayDetails = ((PaymentWindow)sender).Payment;
                        BizAction.Details = pPayDetails;
                        pPayDetails.IsBillSettlement = true;

                        BizAction.Details.BillID = SelectedBill.ID;
                        BizAction.Details.BillAmount = SelectedBill.NetBillAmount;
                        BizAction.Details.Date = DateTime.Now;
                        BizAction.Details.BillBalanceAmount = pPayDetails.PaidAmount;

                        if (BizAction.Details != null && SelectedBill.BillType == BillTypes.Pharmacy)
                        {
                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                                BizAction.Details.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID; //Costing Divisions for Pharmacy Billing
                            else
                                BizAction.Details.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyCostingDivisionID;
                        }

                        if (BizAction.Details != null && SelectedBill.BillType == BillTypes.Clinical)
                        {
                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                                BizAction.Details.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID; //Costing Divisions for Pharmacy Billing
                            else
                                BizAction.Details.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IPDCounterID;
                        }

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        Client.ProcessCompleted += (s, arg) =>
                        {

                            if (arg.Error == null && arg.Result != null)
                            {
                                if (((clsAddPaymentBizActionVO)arg.Result).Details != null)
                                    PaymentID = ((clsAddPaymentBizActionVO)arg.Result).Details.ID;
                                clsUpdateBillPaymentDtlsBizActionVO mybillPayDetails = new clsUpdateBillPaymentDtlsBizActionVO();
                                mybillPayDetails.Details = SelectedBill;
                                mybillPayDetails.Details.BalanceAmountSelf = mybillPayDetails.Details.BalanceAmountSelf - pPayDetails.PaidAmount;
                                if (mybillPayDetails.Details.BalanceAmountSelf < 0) mybillPayDetails.Details.BalanceAmountSelf = 0;
                                PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                Client1.ProcessCompleted += (s1, arg1) =>
                                {
                                    Indicatior.Close();
                                    if (arg1.Error == null && arg1.Result != null)
                                    {
                                        if (dgBillList.ItemsSource != null)
                                        {
                                            dgBillList.ItemsSource = null;
                                            dgBillList.ItemsSource = DataList;
                                        }
                                        MessageBoxControl.MessageBoxChildWindow msgWD =
                                           new MessageBoxControl.MessageBoxChildWindow("", "Payment Details Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                        msgWD.OnMessageBoxClosed += (re) =>
                                        {
                                            if (re == MessageBoxResult.OK)
                                            {
                                                if (((clsBillVO)dgBillList.SelectedItem).BalanceAmountSelf == 0 && ((clsBillVO)dgBillList.SelectedItem).BalanceAmountNonSelf == 0)
                                                {
                                                    PrintSettleBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, PaymentID);
                                                }
                                            }
                                        };
                                        msgWD.Show();
                                    }
                                    else
                                    {
                                        MessageBoxControl.MessageBoxChildWindow msgWD =
                                        new MessageBoxControl.MessageBoxChildWindow("", "Error while updating Payment Details", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgWD.Show();
                                    }
                                };
                                Client1.ProcessAsync(mybillPayDetails, ((IApplicationConfiguration)App.Current).CurrentUser);
                                Client1.CloseAsync();
                            }
                            else
                            {
                                Indicatior.Close();
                                MessageBoxControl.MessageBoxChildWindow msgWD =
                                new MessageBoxControl.MessageBoxChildWindow("", "Error while Saving Payment Details", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWD.Show();
                            }
                        };
                        Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        Client.CloseAsync();
                    }
                }
                else
                {
                    if (!flagFreezFromSearch)
                    {
                        cmdSave.IsEnabled = true;
                    }
                    if (flagFreezFromSearch)
                    {
                        flagFreezFromSearch = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Indicatior.Close();
                throw ex;
            }
        }

        /// <summary>
        /// This method is for printing the receipt against the settlled bill
        /// </summary>
        /// <param name="iBillId"></param>
        /// <param name="iUnitID"></param>
        /// <param name="iPaymentID"></param>
        private void PrintSettleBill(long iBillId, long iUnitID, long iPaymentID)
        {
            if (iBillId > 0 && iPaymentID > 0)
            {
                long UnitID = iUnitID;
                string URL = "../Reports/OPD/SettleClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&PaymentID=" + iPaymentID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

        /// <summary>
        /// Method is for opening ReceiptList Window.
        /// This shows list of all the settled bill receipts against the bill selected in the front panel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceipt_Click(object sender, RoutedEventArgs e)
        {
            if (dgBillList.SelectedItem != null)
            {
                frmReceiptList receiptWin = new frmReceiptList();
                receiptWin.BillUnitID = ((clsBillVO)dgBillList.SelectedItem).UnitID;
                receiptWin.BillID = ((clsBillVO)dgBillList.SelectedItem).ID;
                receiptWin.BillNo = ((clsBillVO)dgBillList.SelectedItem).BillNo;

                receiptWin.Show();
            }
        }

        private void lnkAddPackageService_Click(object sender, RoutedEventArgs e)
        {
            PackageServiceSearchForPackage serviceSearch = null;
            serviceSearch = new PackageServiceSearchForPackage();
            serviceSearch.OnAddButton_Click += new RoutedEventHandler(servicePackageSearch_OnAddButton_Click);
            serviceSearch.Show();
        }

        void servicePackageSearch_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (((PackageServiceSearchForPackage)sender).DialogResult == true)   //if (((PackageServiceSearch)sender).DialogResult == true)
            {
                List<clsServiceMasterVO> lServices = new List<clsServiceMasterVO>();
                lServices = ((PackageServiceSearchForPackage)sender).SelectedOtherServices.ToList();

                #region clsApplyPackageDiscountRateOnServiceBizActionVO

                clsApplyPackageDiscountRateOnServiceBizActionVO objApplyNewRate = new clsApplyPackageDiscountRateOnServiceBizActionVO();

                objApplyNewRate.objApplyPackageDiscountRate = new List<clsApplyPackageDiscountRateOnServiceVO>();
                objApplyNewRate.ipServiceList = new List<clsServiceMasterVO>();

                objApplyNewRate.ipLoginUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                objApplyNewRate.ipPatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                objApplyNewRate.ipPatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;

                objApplyNewRate.ipVisitID = AdmissionID;

                objApplyNewRate.ipPatientGenderID = ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID;

                if (((IApplicationConfiguration)App.Current).SelectedPatient.IsAge == true)
                {
                    objApplyNewRate.ipPatientDateOfBirth = ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirthFromAge;
                }
                else
                {
                    objApplyNewRate.ipPatientDateOfBirth = ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth;
                }

                objApplyNewRate.ipServiceList = lServices;

                if ((PackageServiceSearchForPackage)sender != null && ((PackageServiceSearchForPackage)sender).cmbPatientSource.SelectedItem != null && ((clsPatientSponsorVO)((PackageServiceSearchForPackage)sender).cmbPatientSource.SelectedItem).MemberRelationID > 0)
                    objApplyNewRate.MemberRelationID = ((clsPatientSponsorVO)((PackageServiceSearchForPackage)sender).cmbPatientSource.SelectedItem).MemberRelationID;

                objApplyNewRate.SponsorID = ((MasterListItem)((PackageServiceSearchForPackage)sender).cmbTariff.SelectedItem).SponsorID;
                objApplyNewRate.SponsorUnitID = ((MasterListItem)((PackageServiceSearchForPackage)sender).cmbTariff.SelectedItem).SponsorUnitID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        objApplyNewRate = (clsApplyPackageDiscountRateOnServiceBizActionVO)arg.Result;

                        UnavailableItemStockService = new List<clsUnavailableItemStockServiceId>();

                        foreach (clsServiceMasterVO item in lServices)
                        {
                            if (item.ConditionID > 0)
                            {
                                if (item.ConditionalQuantity > item.ConditionalUsedQuantity && item.ConditionType == "AND" && item.IsAgeApplicable == true && item.ServiceMemberRelationID > 0)   //if (item.ConditionalQuantity > item.ConditionalUsedQuantity)
                                {
                                    item.ConcessionPercent = 100;   //Convert.ToDecimal(item1.DiscountedPercentage);
                                    ConcessionFromPlan = true;
                                }
                                else if (item.ConditionalQuantity > (item.ConditionalUsedQuantity + item.MainServiceUsedQuantity) && item.ConditionType == "OR" && item.IsAgeApplicable == true && item.ServiceMemberRelationID > 0)  //if (item.ConditionalQuantity > item.ConditionalUsedQuantity)
                                {
                                    item.ConcessionPercent = 100;   //Convert.ToDecimal(item1.DiscountedPercentage);
                                    ConcessionFromPlan = true;
                                }
                                else
                                {
                                    if ((item.IsAgeApplicable == true && item.ServiceMemberRelationID > 0))  //&& item.TariffID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID
                                    {
                                        item.ConcessionAmount = 0;
                                        item.ConcessionPercent = Convert.ToDecimal(item.ConditionalDiscount);
                                        ConcessionFromPlan = true;
                                    }
                                    if ((item.IsAgeApplicable == false || item.ServiceMemberRelationID == 0))  //item1.ActualQuantity > 0 &&
                                    {
                                        item.ConcessionPercent = 0;
                                        item.ConcessionAmount = 0;
                                    }
                                }
                            }
                            else
                            {
                                foreach (clsApplyPackageDiscountRateOnServiceVO item1 in objApplyNewRate.objApplyPackageDiscountRate)
                                {
                                    if (item.ID == item1.ServiceID && item.TariffID == item1.TariffID && item.ConditionID == 0)
                                    {
                                        if (item1.IsApplyOn_Rate_Percentage == 1 || item1.IsApplyOn_Rate_Percentage == 3 && item.ConditionID == 0)
                                        {
                                            item.Rate = Convert.ToDecimal(item1.DiscountedRate);

                                            if ((item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0) && item1.TariffID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID)
                                            {
                                                item.ConcessionAmount = 0;
                                                item.ConcessionPercent = Convert.ToDecimal(item1.ConcessionPercentage);
                                            }
                                            if (item1.IsDiscountOnQuantity == true || item1.ServiceMemberRelationID == 0 || item1.IsAgeApplicable == false)
                                            {
                                                item.ConcessionPercent = 0;
                                                item.ConcessionAmount = 0;
                                            }
                                        }

                                        if (item1.IsApplyOn_Rate_Percentage == 2 || item1.IsApplyOn_Rate_Percentage == 3 && item.ConditionID == 0)
                                        {
                                            if (item1.IsDiscountOnQuantity == false)
                                            {
                                                if (item.PackageServiceConditionID == 0)
                                                {
                                                    if (item1.ActualQuantity > item1.UsedQuantity && item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0)
                                                    {
                                                        item.ConcessionPercent = Convert.ToDecimal(item1.DiscountedPercentage);
                                                        ConcessionFromPlan = true; //Added by priyanka-to set validation on concession
                                                    }
                                                    else
                                                    {
                                                        if ((item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0) && item1.TariffID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID)
                                                        {
                                                            item.ConcessionAmount = 0;
                                                            item.ConcessionPercent = Convert.ToDecimal(item1.ConcessionPercentage);
                                                        }
                                                        if ((item1.IsAgeApplicable == false || item1.ServiceMemberRelationID == 0))  //item1.ActualQuantity > 0 &&
                                                        {
                                                            item.ConcessionPercent = 0;
                                                            item.ConcessionAmount = 0;
                                                        }
                                                    }
                                                }
                                                else if (item.PackageServiceConditionID > 0)
                                                {
                                                    if (item1.ActualQuantity > (item1.UsedQuantity + item.TotalORUsedQuantity) && item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0)
                                                    {
                                                        item.ConcessionPercent = Convert.ToDecimal(item1.DiscountedPercentage);
                                                        ConcessionFromPlan = true; //Added by priyanka-to set validation on concession
                                                    }
                                                    else
                                                    {
                                                        if ((item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0) && item1.TariffID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID)
                                                        {
                                                            item.ConcessionAmount = 0;
                                                            item.ConcessionPercent = Convert.ToDecimal(item1.ConcessionPercentage);
                                                        }
                                                        if ((item1.IsAgeApplicable == false || item1.ServiceMemberRelationID == 0))  //item1.ActualQuantity > 0 && 
                                                        {
                                                            item.ConcessionPercent = 0;
                                                            item.ConcessionAmount = 0;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (item1.ActualQuantity > (item1.UsedQuantity + item.TotalORUsedQuantity) && item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0)
                                                {
                                                    if ((item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0) && item1.TariffID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID)
                                                    {
                                                        item.ConcessionAmount = 0;
                                                        item.ConcessionPercent = Convert.ToDecimal(item1.ConcessionPercentage);
                                                    }
                                                }
                                                else
                                                {
                                                    item.ConcessionPercent = 0;
                                                    item.ConcessionAmount = 0;
                                                }
                                            }
                                        }
                                        if (item.IsMarkUp == true && item1.IsServiceItemStockAvailable != true)
                                        {
                                            UnavailableItemStockService.Add(new clsUnavailableItemStockServiceId { ServiceID = item.ServiceID });
                                        }
                                    }
                                }
                            }
                        }

                        StringBuilder sbServiceName = new StringBuilder();

                        foreach (clsUnavailableItemStockServiceId item in UnavailableItemStockService)
                        {
                            clsServiceMasterVO obj = new clsServiceMasterVO();
                            obj = (clsServiceMasterVO)lServices.First(z => z.ServiceID == item.ServiceID && z.IsMarkUp == true);
                            sbServiceName.Append(obj.ServiceName + ",");
                            lServices.Remove(obj);
                        }
                        if (sbServiceName.Length > 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWD =
                                            new MessageBoxControl.MessageBoxChildWindow("", "Items are not available for Service : " + sbServiceName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgWD.OnMessageBoxClosed += (re) =>
                            {
                                if (re == MessageBoxResult.OK)
                                {
                                    PatientCategoryID = ((PackageServiceSearchForPackage)sender).PatientCategoryL1Id_Retail;  //PatientCategoryID = ((PackageServiceSearch)sender).PatientCategoryL1Id_Retail;
                                    PatientSourceID = ((clsPatientSponsorVO)((PackageServiceSearchForPackage)sender).cmbPatientSource.SelectedItem).PatientSourceID;  //PatientSourceID = ((clsPatientSponsorVO)((PackageServiceSearch)sender).cmbPatientSource.SelectedItem).PatientSourceID;
                                    PatientTariffID = ((MasterListItem)((PackageServiceSearchForPackage)sender).cmbTariff.SelectedItem).ID;  //PatientTariffID = ((MasterListItem)((PackageServiceSearch)sender).cmbTariff.SelectedItem).ID;
                                    PackageTariffID = ((PackageServiceSearchForPackage)sender).PackageTariffID;  //PackageTariffID = ((PackageServiceSearch)sender).PackageTariffID;
                                    ServiceTariffID = ((PackageServiceSearchForPackage)sender).ServiceTariffID;    //ServiceTariffID = ((PackageServiceSearch)sender).ServiceTariffID;
                                    //PatientCatID = ((clsPatientSponsorVO)((PackageServiceSearchForPackage)sender).cmbPatientSource.SelectedItem).PatientCategoryID;
                                    AddCharges(lServices, ServiceTariffID);
                                }
                            };
                            msgWD.Show();
                        }
                        else
                        {
                            PatientCategoryID = ((PackageServiceSearchForPackage)sender).PatientCategoryL1Id_Retail;   //PatientCategoryID = ((PackageServiceSearch)sender).PatientCategoryL1Id_Retail;
                            PatientSourceID = ((clsPatientSponsorVO)((PackageServiceSearchForPackage)sender).cmbPatientSource.SelectedItem).PatientSourceID;    //PatientSourceID = ((clsPatientSponsorVO)((PackageServiceSearch)sender).cmbPatientSource.SelectedItem).PatientSourceID;
                            PatientTariffID = ((MasterListItem)((PackageServiceSearchForPackage)sender).cmbTariff.SelectedItem).ID;   //PatientTariffID = ((MasterListItem)((PackageServiceSearch)sender).cmbTariff.SelectedItem).ID;
                            PackageTariffID = ((PackageServiceSearchForPackage)sender).PackageTariffID;   //PackageTariffID = ((PackageServiceSearch)sender).PackageTariffID;
                            ServiceTariffID = ((PackageServiceSearchForPackage)sender).ServiceTariffID;     //ServiceTariffID = ((PackageServiceSearch)sender).ServiceTariffID;
                            AddCharges(lServices, ServiceTariffID);
                        }
                    }
                };
                client.ProcessAsync(objApplyNewRate, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                #endregion
            }
        }
        int ApproveFlag = 0;
        private void cmdApprove_Click(object sender, RoutedEventArgs e)
        {
            if (objUser.IpdBillAuthLvlID != null)
            {
                ApproveFlag += 1;
                if (ApproveFlag == 1)
                {
                    if (objUser.IpdBillAuthLvlID == SelectedBill.LevelID)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Bill is already approved with " + objUser.AuthLevelForConcenssionIPD + " rights ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                        msgWindowUpdate.Show();
                        ApproveFlag = 0;
                    }
                    else if (objUser.IpdBillAuthLvlID < SelectedBill.LevelID)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Bill is already approved with higher rights ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                        msgWindowUpdate.Show();
                        ApproveFlag = 0;
                    }
                    else if ((objUser.IpdBillingAmmount == 0 ? false : objUser.IpdBillingAmmount < Convert.ToDecimal(ApprovalConcenssion)) || (objUser.IpdBillingPercentage == 0 ? false : (((Convert.ToDecimal(ApprovalConcenssion) / Convert.ToDecimal(ApprovalTotal)) * 100)) > objUser.IpdBillingPercentage))
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Given concenssion amount exceeds authorised amount", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                        msgWindowUpdate.Show();
                        ApproveFlag = 0;
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to Approve the request with " + objUser.AuthLevelForConcenssionIPD + " rights", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        msgWindowUpdate.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindowUpdate_OnMessageBoxClosed);

                        msgWindowUpdate.Show();
                    }
                }
            }
        }
        private void msgWindowUpdate_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                ApproveRequest(null, false);
            }
            else
            {
                ApproveFlag = 0;
            }
        }
        private void ApproveRequest(clsPaymentVO pPayDetails, bool pFreezBill)
        {

            clsApproveSendRequestVO BizAction = new clsApproveSendRequestVO();
            BizAction.BillDetails = new clsAddBillBizActionVO();
            //For Bill

            var results2 = from r in ChargeList
                           orderby r.NetAmount ascending
                           select r;

            ChargeList = new ObservableCollection<clsChargeVO>();
            foreach (var item in results2.ToList())
            {
                item.Doctor = null;
                ChargeList.Add(item);
            }
            double Con = BillWiseCon;
            foreach (var item in ChargeList)
            {

                double Net = item.NetAmount;
                double TotalConcession = item.Concession;

                if (item.ConcessionPercent < 100)
                {
                    if (Con > 0)
                    {
                        if (Con >= Net)
                        {
                            item.Concession = item.Concession + item.NetAmount;
                            item.NetAmount = 0;
                            Con = Con - Net;
                        }
                        else
                        {
                            double usedCon = 0;
                            usedCon = Con;
                            item.Concession = item.Concession + Con;
                            item.NetAmount = Net - item.Concession;
                            Con = Con - usedCon;
                        }
                    }
                }
            }
            double TotalAmt = 0;
            if (pPayDetails != null)
                TotalAmt = pPayDetails.PaidAmount;
            double ConsumeAmt = 0;

            var results = from r in ChargeList
                          orderby r.NetAmount descending
                          select r;

            ChargeList = new ObservableCollection<clsChargeVO>();

            foreach (var item in results.ToList())
            {
                if (item.ChildPackageService == false)
                {
                    if (TotalAmt > 0)
                    {
                        ConsumeAmt = item.NetAmount;
                        if (TotalAmt >= ConsumeAmt)
                        {
                            TotalAmt = TotalAmt - ConsumeAmt;
                        }
                        else
                        {
                            ConsumeAmt = TotalAmt;
                            TotalAmt = TotalAmt - ConsumeAmt;
                        }

                        item.ServicePaidAmount = ConsumeAmt;
                        item.BalanceAmount = item.NetAmount - item.ServicePaidAmount;
                        item.ChkID = true;
                    }
                    else
                    {
                        item.BalanceAmount = item.NetAmount;
                    }


                    var _List = from obj in results.ToList()
                                where (obj.PackageID.Equals(item.PackageID) && obj.ChildPackageService == true)
                                select obj;


                    double PaidAmount = item.ServicePaidAmount;
                    foreach (var Obj in _List)
                    {
                        clsChargeVO clschargeObject = (clsChargeVO)Obj;

                        if (PaidAmount > 0)
                        {
                            ConsumeAmt = Obj.NetAmount;
                            if (PaidAmount >= ConsumeAmt)
                            {
                                PaidAmount = PaidAmount - ConsumeAmt;
                            }
                            else
                            {
                                ConsumeAmt = PaidAmount;
                                PaidAmount = PaidAmount - ConsumeAmt;
                            }

                            Obj.ServicePaidAmount = ConsumeAmt;
                            Obj.BalanceAmount = Obj.NetAmount - Obj.ServicePaidAmount;
                            Obj.ChkID = true;
                        }
                        else
                        {
                            Obj.BalanceAmount = Obj.NetAmount;
                        }
                    }
                }
                ChargeList.Add(item);
            }

            double TotCon = 0;
            double TotAmt = 0;
            double TotPaid = 0;

            foreach (var item in ChargeList)
            {
                TotCon = TotCon + item.Concession;
                TotAmt = TotAmt + item.NetAmount;
            }

            Indicatior = new WaitIndicator();
            Indicatior.Show();
            try
            {
                clsBillVO objBill = new clsBillVO();

                if (SelectedBill == null)
                {
                    objBill.Date = DateTime.Now;
                    objBill.Time = DateTime.Now;
                    IsEditMode = false;

                    objBill.Opd_Ipd_External_Id = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmID;
                    objBill.Opd_Ipd_External_UnitId = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmissionUnitID;

                    objBill.CompanyId = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
                    objBill.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    objBill.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                }
                else
                {
                    objBill = SelectedBill.DeepCopy();
                    IsEditMode = true;
                }

                if (cmbConcessionReason.SelectedItem != null)
                    objBill.ConcessionReasonId = ((MasterListItem)cmbConcessionReason.SelectedItem).ID;

                if (pFreezBill)
                {
                    objBill.Date = DateTime.Now;
                    objBill.Time = DateTime.Now;
                    if (pPayDetails != null)
                    {
                        objBill.BalanceAmountSelf = TotAmt - pPayDetails.PaidAmount;
                        objBill.BillPaymentType = pPayDetails.BillPaymentType;
                    }

                    if (!string.IsNullOrEmpty(txtPayAmount.Text))
                        objBill.SelfAmount = Convert.ToDouble(txtPayAmount.Text);
                }

                if (PatientSourceID > 0)
                {
                    objBill.PatientSourceId = PatientSourceID;
                }
                else
                {
                    objBill.PatientSourceId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;

                }
                if (PatientTariffID > 0)
                {
                    objBill.TariffId = PatientTariffID;
                }
                else
                {
                    objBill.TariffId = ((IApplicationConfiguration)App.Current).SelectedPatient.TariffID;
                }

                if (PatientCategoryID > 0)
                {
                    objBill.PatientCategoryId = Convert.ToInt64(PatientCategoryID);
                }
                else
                {
                    objBill.PatientCategoryId = ((IApplicationConfiguration)App.Current).SelectedPatient.NewPatientCategoryID;
                }


                if (PatientCompanyID > 0)
                {
                    objBill.CompanyId = Convert.ToInt64(PatientCompanyID);
                }
                else
                {
                    objBill.CompanyId = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
                }

                objBill.IsFreezed = pFreezBill;
                objBill.PaymentDetails = pPayDetails;

                if (!string.IsNullOrEmpty(txtTotalBill.Text))
                    objBill.TotalBillAmount = Convert.ToDouble(txtTotalBill.Text);

                if (!string.IsNullOrEmpty(txtTotalConcession.Text))
                    objBill.TotalConcessionAmount = Convert.ToDouble(txtTotalConcession.Text);
                TotalConcession = objBill.TotalConcessionAmount;

                if (!string.IsNullOrEmpty(txtNetAmount.Text))
                    objBill.NetBillAmount = Convert.ToDouble(txtNetAmount.Text);

                objBill.TotalConcessionAmount = TotCon;
                TotalConcession = objBill.TotalConcessionAmount;
                objBill.NetBillAmount = TotAmt;
                //added By Bhushanp 09032017
                objBill.ConcessionRemark = Convert.ToString(txtApprovalRemark.Text.Trim());

                if (ChargeList != null & ChargeList.Count > 0)
                {
                    objBill.ChargeDetails = ChargeList.ToList();

                    if (pFreezBill)
                    {
                        for (int i = 0; i < ChargeList.Count; i++)
                        {
                            clsChargeVO objCharge = new clsChargeVO();
                            objCharge = ChargeList[i];
                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                                objCharge.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                            else
                                objCharge.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IPDCounterID;

                            if (objCharge.ServiceSpecilizationID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathoSpecializationID)
                            {
                                objBill.PathoWorkOrder = new clsPathOrderBookingVO();
                                objBill.PathoWorkOrder.DoctorID = objCharge.SelectedDoctor.ID;
                                objBill.PathoWorkOrder.PathoSpecilizationID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathoSpecializationID;
                                objBill.PathoWorkOrder.Items = new List<clsPathOrderBookingDetailVO>();
                            }
                            else if (objCharge.ServiceSpecilizationID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RadiologySpecializationID)
                            {
                                objBill.RadiologyWorkOrder.OrderBookingDetails.Add(new clsRadOrderBookingDetailsVO()
                                {
                                    TariffServiceID = objCharge.TariffServiceId,
                                    ServiceID = objCharge.ServiceId,
                                    RadOrderID = objBill.RadiologyWorkOrder.ID,
                                    DoctorID = objCharge.SelectedDoctor.ID,
                                    IsApproved = false
                                });
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < ChargeList.Count; i++)
                        {
                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                                ChargeList[i].CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                            else
                                ChargeList[i].CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IPDCounterID;
                        }
                    }
                }

                if (PharmacyItems.Count > 0)
                {
                    objBill.PharmacyItems.Items = new List<clsItemSalesDetailsVO>();
                    objBill.PharmacyItems.VisitID = objBill.Opd_Ipd_External_Id;
                    objBill.PharmacyItems.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    objBill.PharmacyItems.PatientUnitID = objBill.Opd_Ipd_External_UnitId;
                    objBill.PharmacyItems.Date = objBill.Date;
                    objBill.PharmacyItems.Time = objBill.Time;
                    objBill.PharmacyItems.StoreID = StoreID;

                    if (!string.IsNullOrEmpty(txtPharmacyTotal.Text))
                        objBill.PharmacyItems.TotalAmount = Convert.ToDouble(txtPharmacyTotal.Text);

                    if (!string.IsNullOrEmpty(txtPharmacyConcession.Text))
                        objBill.PharmacyItems.ConcessionAmount = Convert.ToDouble(txtPharmacyConcession.Text);

                    if (!string.IsNullOrEmpty(txtPharmacyNetAmount.Text))
                        objBill.PharmacyItems.NetAmount = Convert.ToDouble(txtPharmacyNetAmount.Text);

                    objBill.PharmacyItems.Items = PharmacyItems.ToList();

                }
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                {
                    if (objBill.ChargeDetails != null && objBill.ChargeDetails.Count > 0 && objBill.PharmacyItems != null && objBill.PharmacyItems.Items != null && objBill.PharmacyItems.Items.Count > 0)
                    {
                        objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                    }
                    else if (objBill.ChargeDetails != null && objBill.ChargeDetails.Count > 0)
                    {
                        objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                    }
                    else if (objBill.PharmacyItems != null && objBill.PharmacyItems.Items != null && objBill.PharmacyItems.Items.Count > 0)
                    {
                        objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                    }
                }
                else
                    objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IPDCounterID;

                if (objBill.PaymentDetails != null)
                {
                    objBill.PaymentDetails.CostingDivisionID = objBill.CostingDivisionID;
                }

                foreach (var item in ChargeList)
                {
                    if (item.isPackageService == true)
                    {
                        BizAction.BillDetails.IsPackageBill = true;
                        BizAction.BillDetails.PackageID = item.PackageID;
                        break;
                    }
                    else
                    {
                        BizAction.BillDetails.IsPackageBill = false;
                        BizAction.BillDetails.PackageID = item.PackageID;
                    }
                }

                BizAction.BillDetails.Details = new clsBillVO();
                BizAction.BillDetails.Details = objBill;

                BizAction.IsForApproval = true;
                if (BizAction.ApprovalChargeList == null)
                    BizAction.ApprovalChargeList = new List<clsChargeVO>();
                BizAction.ApprovalRequestID = SelectedBill.ApprovalRequestID;
                BizAction.ApprovalRequestUnitID = SelectedBill.ApprovalRequestUnitID;
                BizAction.ApprovedByID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                BizAction.ApprovedByName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.Name;
                if (objUser.IpdBillAuthLvlID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForConcessionID)
                    BizAction.IsApproved = true;
                else
                    BizAction.IsApproved = false;
                BizAction.LevelID = objUser.IpdBillAuthLvlID;
                BizAction.ApprovalRemark = txtApprovalRemark.Text;
                BizAction.ApprovedDateTime = DateTime.Now;
                foreach (var item in ChargeList)
                {
                    if (item.IsSendForApproval && item.IsCancelled == false)
                        BizAction.ApprovalChargeList.Add(item);
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (sa, args) =>
                {
                    if (args.Error == null)
                    {
                        if (objUser.IpdAuthLvl == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForRefundID)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Request Approved Successfully,you can freeze the bill and make a payment", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                            msgW.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Request Approved Successfully, still higher level approval is required", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                            msgW.Show();
                        }
                        if (IsFromRequestApproval == true)
                        {
                            ModuleName = "OPDModule";
                            Action = "CIMS.Forms.frmApprovalRequests";
                            UserControl rootPage = Application.Current.RootVisual as UserControl;
                            WebClient c2 = new WebClient();
                            c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                            c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Error occured while Approving request.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                    ApproveFlag = 0;
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                Indicatior.Close();
            }
            catch (Exception)
            {
                Indicatior.Close();
                throw;
            }
        }
        private void cmdSendApprove_Click(object sender, RoutedEventArgs e)
        {

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

        private void dgCharges_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (((clsChargeVO)e.Row.DataContext).DoctorId != null)
            {
                ((clsChargeVO)e.Row.DataContext).SelectedDoctor = DoctorList.FirstOrDefault(p => p.ID == ((clsChargeVO)e.Row.DataContext).DoctorId);
            }

            #region For Doctor Selection Chage Get Rate For Service
            if (((clsChargeVO)e.Row.DataContext).DoctorId != null && ((clsChargeVO)e.Row.DataContext).DoctorId > 0 && ((clsChargeVO)e.Row.DataContext).PackageID == 0)
            {
                clsGetDoctorServiceLinkingByCategoryAndServiceBizActionVO BizActionVO = new clsGetDoctorServiceLinkingByCategoryAndServiceBizActionVO();
                BizActionVO.ServiceMasterDetails = new clsServiceMasterVO();

                BizActionVO.ServiceMasterDetails.DoctorID = ((clsChargeVO)e.Row.DataContext).DoctorId;
                BizActionVO.ServiceMasterDetails.ServiceID = ((clsChargeVO)e.Row.DataContext).ServiceId;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    BizActionVO.ServiceMasterDetails = new clsServiceMasterVO();
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (Convert.ToDouble(((clsGetDoctorServiceLinkingByCategoryAndServiceBizActionVO)arg.Result).ServiceMasterDetails.Rate) > 0)
                        {
                            ((clsChargeVO)e.Row.DataContext).Rate = Convert.ToDouble(((clsGetDoctorServiceLinkingByCategoryAndServiceBizActionVO)arg.Result).ServiceMasterDetails.Rate);
                        }
                        else
                        {
                            ((clsChargeVO)e.Row.DataContext).Rate = ((clsChargeVO)e.Row.DataContext).InitialRate;
                        }
                        CalculateClinicalSummary();
                    }
                };
                client.ProcessAsync(BizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            #endregion
            for (int i = 0; i < dgCharges.Columns.Count(); i++)
            {
                if (((clsChargeVO)e.Row.DataContext).PackageID != null)
                {
                    if (((clsChargeVO)e.Row.DataContext).PackageID > 0 && ((clsChargeVO)e.Row.DataContext).isPackageService == false)
                    {
                        if (i == 5 || i == 6)
                        {
                            DataGridColumn column = dgCharges.Columns[i];
                            FrameworkElement fe = column.GetCellContent(e.Row);
                            FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                            if (result != null)
                            {
                                DataGridCell cell = (DataGridCell)result;
                                cell.IsEnabled = false;
                            }
                        }
                    }
                }
            }
            if (IsFromRequestApproval == true)
            {
                for (int i = 0; i < dgCharges.Columns.Count(); i++)
                {
                    if (i != 5 && i != 6)
                        dgCharges.Columns[i].IsReadOnly = true;
                }
            }
            else if (dgBillList.SelectedItem != null)
            {
                if (((clsBillVO)dgBillList.SelectedItem).IsRequestSend == true && ((clsBillVO)dgBillList.SelectedItem).IsApproved == true)
                {
                    dgCharges.IsEnabled = false;
                }
            }

            if (IsAuditTrail)
            {
                LogInformation = new LogInfo();   // first time data in grid
                LogInformation.guid = objGUID;
                LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                LogInformation.TimeStamp = DateTime.Now;
                LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                LogInformation.Message = " 7 : First Time Data In Grid " //+ Convert.ToString(lineNumber)
                                                        + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                                                        + " , Service Code : " + Convert.ToString(((clsChargeVO)e.Row.DataContext).ServiceId)
                                                        + " , Service Name : " + Convert.ToString(((clsChargeVO)e.Row.DataContext).ServiceName)
                                                        + " , Rate : " + Convert.ToString(((clsChargeVO)e.Row.DataContext).Rate)
                                                        + " , Quantity : " + Convert.ToString(((clsChargeVO)e.Row.DataContext).Quantity)
                                                        + " , Concession % : " + Convert.ToString(((clsChargeVO)e.Row.DataContext).ConcessionPercent)
                                                        + " , Concession Amount : " + Convert.ToString(((clsChargeVO)e.Row.DataContext).Concession)
                                                        + " , Service Tax %: " + Convert.ToString(((clsChargeVO)e.Row.DataContext).ServiceTaxPercent)
                                                        + " , Service Tax Amount : " + Convert.ToString(((clsChargeVO)e.Row.DataContext).ServiceTaxAmount)
                                                        + " , Total Amount : " + Convert.ToString(((clsChargeVO)e.Row.DataContext).TotalAmount)
                                                        + " , Net Amount : " + Convert.ToString(((clsChargeVO)e.Row.DataContext).NetAmount)
                                                        + " , Doctor ID : " + Convert.ToString(((clsChargeVO)e.Row.DataContext).DoctorId);
                LogInfoList.Add(LogInformation);
            }
        }

        private void FillChargeListForFrontGrid(long PBillID, long pUnitID, bool pIsBilled, long pVisitID, long pVisitUnitID, bool IsDefaultService)
        {
            try
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();
                clsGetChargeListBizActionVO BizAction = new clsGetChargeListBizActionVO();
                BizAction.ID = 0;
                BizAction.Opd_Ipd_External_Id = pVisitID;
                BizAction.Opd_Ipd_External_UnitId = pVisitUnitID;
                BizAction.IsBilled = pIsBilled;
                BizAction.BillID = PBillID;
                BizAction.UnitID = pUnitID;
                BizAction.RequestTypeID = (int)RequestType.Concession;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsGetChargeListBizActionVO)arg.Result).List != null)
                        {
                            List<clsChargeVO> objList;
                            ChargeListForFrontGrid = new ObservableCollection<clsChargeVO>();
                            objList = ((clsGetChargeListBizActionVO)arg.Result).List;
                            foreach (var item in objList)
                            {
                                ChargeListForFrontGrid.Add(item);
                            }
                            CalculateClinicalSummaryForFrontGrid();
                        }
                    }
                    Indicatior.Close();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {

            }
            finally
            {
                Indicatior.Close();
            }
        }

        double ApprovalConcenssionFrontGrid, ApprovalTotalFrontGrid;
        private void CalculateClinicalSummaryForFrontGrid()
        {
            ApprovalConcenssionFrontGrid = ApprovalTotalFrontGrid = 0;
            for (int i = 0; i < ChargeListForFrontGrid.Count; i++)
            {
                if (ChargeListForFrontGrid[i].PackageID == 0 || ChargeListForFrontGrid[i].isPackageService == true)
                {
                    ApprovalConcenssionFrontGrid += ChargeListForFrontGrid[i].Concession;
                    ApprovalTotalFrontGrid += ChargeListForFrontGrid[i].TotalAmount;
                }
            }
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForConcessionID > 0)
            {
                if ((double)objUser.IpdBillingAmmount < ApprovalConcenssionFrontGrid && (((Convert.ToDecimal(ApprovalConcenssionFrontGrid) / Convert.ToDecimal(ApprovalTotalFrontGrid)) * 100) > objUser.IpdBillingPercentage) && ((clsBillVO)dgBillList.SelectedItem).IsRequestSend == false)
                {
                    MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Concenssion exceeds authorised amount, approval is needed", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                    msgWD_1.Show();
                    InitialiseForm();
                    ((clsBillVO)dgBillList.SelectedItem).IsFreezed = false;
                }
                else if (((clsBillVO)dgBillList.SelectedItem).IsRequestSend == true && ((clsBillVO)dgBillList.SelectedItem).IsApproved == false)
                {
                    MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Request is already Send", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                    msgWD_1.Show();
                    InitialiseForm();
                    ((clsBillVO)dgBillList.SelectedItem).IsFreezed = false;
                }
                else
                {
                    FreezBill();
                }
            }
            else
            {
                FreezBill();
            }
        }

        #region Doctor Selection Change
        private void cmbDoctor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCharges.SelectedItem != null)
            {
                FrameworkElement result = GetParent((FrameworkElement)sender, typeof(DataGridCell));
                if (result != null)
                {
                    var comboBox = (AutoCompleteBox)sender;
                    Int64 DoctorID;
                    if (comboBox.SelectedItem != null)
                    {
                        //Added By Bhushanp 03022017                    
                        if ((clsChargeVO)dgCharges.SelectedItem != null)
                        {
                            ((clsChargeVO)dgCharges.SelectedItem).DoctorId = ((MasterListItem)comboBox.SelectedItem).ID;
                        }
                        DoctorID = ((MasterListItem)comboBox.SelectedItem).ID;
                        if (DoctorID != null && DoctorID > 0 && ((clsChargeVO)(result.DataContext)).PackageID == 0)
                        {
                            clsGetDoctorServiceLinkingByCategoryAndServiceBizActionVO BizActionVO = new clsGetDoctorServiceLinkingByCategoryAndServiceBizActionVO();
                            BizActionVO.ServiceMasterDetails = new clsServiceMasterVO();
                            BizActionVO.ServiceMasterDetails.DoctorID = DoctorID; //ID;
                            BizActionVO.ServiceMasterDetails.ServiceID = ((clsChargeVO)(result.DataContext)).ServiceId;
                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            client.ProcessCompleted += (s, arg) =>
                            {
                                BizActionVO.ServiceMasterDetails = new clsServiceMasterVO();
                                if (arg.Error == null && arg.Result != null)
                                {
                                    if (Convert.ToDouble(((clsGetDoctorServiceLinkingByCategoryAndServiceBizActionVO)arg.Result).ServiceMasterDetails.Rate) > 0)
                                    {
                                        ((clsChargeVO)dgCharges.SelectedItem).Rate = Convert.ToDouble(((clsGetDoctorServiceLinkingByCategoryAndServiceBizActionVO)arg.Result).ServiceMasterDetails.Rate);
                                    }
                                    else
                                    {
                                        ((clsChargeVO)dgCharges.SelectedItem).Rate = ((clsChargeVO)dgCharges.SelectedItem).InitialRate;
                                    }

                                    #region GST Details added by Ashish Z. on dated 24062017

                                    double TotalTaxamount = 0;
                                    if (((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsList != null && ((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsList.Count() > 0)
                                    {
                                        ((clsChargeVO)dgCharges.SelectedItem).TaxType = ((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsList[0].TaxType;
                                        foreach (var item in ((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsList.ToList())
                                        {
                                            item.Concession = ((clsChargeVO)dgCharges.SelectedItem).Concession;
                                            item.TotalAmount = ((clsChargeVO)dgCharges.SelectedItem).TotalAmount;
                                            item.Rate = ((clsChargeVO)dgCharges.SelectedItem).Rate;
                                            TotalTaxamount += item.ServiceTaxAmount;
                                        }
                                        ((clsChargeVO)dgCharges.SelectedItem).TotalServiceTaxAmount = TotalTaxamount;
                                        CalculateClinicalSummary();
                                    }
                                    else
                                    {
                                        if (((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsVO != null && ((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsVO.TaxLinkingDetailsList != null && ((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsVO.TaxLinkingDetailsList.Count() > 0)
                                            ((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsList = GetServiceTaxList(((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsVO.TaxLinkingDetailsList);
                                    }
                                    #endregion

                                    CalculateClinicalSummary();
                                    if (IsAuditTrail)
                                    {
                                        LogInformation = new LogInfo();       // data on doctor change
                                        LogInformation.guid = objGUID;
                                        LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                                        LogInformation.TimeStamp = DateTime.Now;
                                        LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                                        LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                                        LogInformation.Message = " 8 : Data On Doctor Change" //+ Convert.ToString(lineNumber)
                                                                                + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                                                                                + " , Service Code : " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).ServiceId)
                                                                                + " , Service Name : " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).ServiceName)
                                                                                + " , Rate : " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).Rate)
                                                                                + " , Quantity : " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).Quantity)
                                                                                + " , Concession % : " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).ConcessionPercent)
                                                                                + " , Concession Amount : " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).Concession)
                                                                                + " , Service Tax %: " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).ServiceTaxPercent)
                                                                                + " , Service Tax Amount : " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).ServiceTaxAmount)
                                                                                + " , Total Amount : " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).TotalAmount)
                                                                                + " , Net Amount : " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).NetAmount)
                                                                                + " , Doctor ID : " + Convert.ToString(((MasterListItem)comboBox.SelectedItem).ID);
                                        LogInfoList.Add(LogInformation);
                                    }
                                }
                            };
                            client.ProcessAsync(BizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                            client.CloseAsync();
                        }
                    }
                }
            }
        }
        #endregion

        public void PassDataToForm(clsBillVO value, bool fromForm)
        {
            try
            {
                if (value != null)
                {
                    IsFromRequestApproval = true;
                    IsPatientExist = true;
                    dgBillList.SelectedItem = value;
                    SelectedBill = value;
                }
                //if (DeleteBillAddedItems != null)
                //{
                //    DeleteBillAddedItems = new ObservableCollection<clsChargeVO>();
                //    DeleteBillAddedItems.Clear();
                //}
                //if (dgBillList.SelectedItem != null)
                //{
                //    IsEditMode = true;
                //    InitialiseForm();
                //    SetCommandButtonState("Modify");
                //    IsFromViewClick = true;
                //    rdbAgainstBill.IsChecked = true;
                //    rdbAgainstBill.IsEnabled = false;
                //    rdbAgainstServices.IsEnabled = false;
                //    SelectedBill = new clsBillVO();
                //    SelectedBill = (clsBillVO)dgBillList.SelectedItem;
                //    BillID = ((clsBillVO)dgBillList.SelectedItem).ID;
                //    PatientSourceID = SelectedBill.PatientSourceId;
                //    PatientTariffID = SelectedBill.TariffId;
                //    cmbConcessionReason.SelectedValue = ((clsBillVO)dgBillList.SelectedItem).ConcessionReasonId;//Added By Yogesh K 21 Apr 16
                //    ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID = ((clsBillVO)dgBillList.SelectedItem).CompanyId;
                //    if (((clsBillVO)dgBillList.SelectedItem).IsFreezed == true)
                //    {
                //        cmdModify.IsEnabled = false;
                //        lnkAddItems.IsEnabled = false;
                //        lnkAddItemsFromPrescription.IsEnabled = false;
                //        lnkAddService.IsEnabled = false;
                //        lnkAddServicesFromPrescription.IsEnabled = false;
                //        lnkAddPackageService.IsEnabled = false;
                //        chkFreezBill.IsEnabled = false;

                //        if (SelectedBill.BillPaymentType == PalashDynamics.ValueObjects.BillPaymentTypes.AgainstBill)
                //        {
                //            rdbAgainstBill.IsChecked = true;
                //            dgCharges.Columns[0].Visibility = System.Windows.Visibility.Collapsed;
                //        }
                //        else if (SelectedBill.BillPaymentType == PalashDynamics.ValueObjects.BillPaymentTypes.AgainstServices)
                //        {
                //            rdbAgainstServices.IsChecked = true;
                //            dgCharges.Columns[0].Visibility = System.Windows.Visibility.Visible;
                //        }
                //        FillChargeList(SelectedBill.ID, SelectedBill.UnitID, true, SelectedBill.Opd_Ipd_External_Id, SelectedBill.Opd_Ipd_External_UnitId, false);
                //        FillPharmacyItemsList(SelectedBill.ID, SelectedBill.UnitID, true);
                //        if (((clsBillVO)dgBillList.SelectedItem).BillPaymentType == PalashDynamics.ValueObjects.BillPaymentTypes.AgainstServices)
                //            rdbAgainstServices.IsChecked = true;
                //    }
                //    else
                //    {
                //        rdbDraftBill.IsChecked = true;
                //        FillChargeList(SelectedBill.ID, SelectedBill.UnitID, false, SelectedBill.Opd_Ipd_External_Id, SelectedBill.Opd_Ipd_External_UnitId, false);
                //        FillPharmacyItemsList(SelectedBill.ID, SelectedBill.UnitID, false);
                //    }
                //    IsEditMode = true;
                //    FillPatientAdvance(false);
                //    chkFreezBill.IsChecked = ((clsBillVO)dgBillList.SelectedItem).IsFreezed;
                //    //chkFreezBill_Click(sender, e);
                //    if (objUser.OPDAuthLvtForConcessionID == 0 && IsEditMode == false)
                //    {
                //        cmbConcessionReason.IsEnabled = true;
                //    }
                //    else
                //    {
                //        if (IsEditMode == true && dgBillList.SelectedItem != null)
                //        {
                //            if ((dgBillList.SelectedItem as clsBillVO).LevelID > 0 && objUser.OPDAuthLvtForConcessionID != 0)
                //            {
                //                cmbConcessionReason.IsEnabled = true;
                //            }
                //            else if (((dgBillList.SelectedItem as clsBillVO).LevelID == 1 || (dgBillList.SelectedItem as clsBillVO).LevelID == 2) && objUser.OPDAuthLvtForConcessionID == 0)
                //                cmbConcessionReason.IsEnabled = false;
                //        }
                //    }

                //    _flip.Invoke(RotationType.Forward);
                //}
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
        }


        public void chkBillFreeze()
        {
            clsGetBillSearchListBizActionVO BizAction = new clsGetBillSearchListBizActionVO();
            BizAction.RequestTypeID = 1;
            BizAction.IsRequest = true;
            BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
            {
                BizAction.UnitID = BizAction.PatientUnitID.Value;//((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            else
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
            else
                BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IPDCounterID;

            BizAction.Opd_Ipd_External = 1;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, i) =>
            {
                if (i.Error == null && i.Result != null)
                {
                    clsGetBillSearchListBizActionVO result = i.Result as clsGetBillSearchListBizActionVO;
                    List<clsBillVO> objList = new List<clsBillVO>();
                    objList = result.List;
                    DataList.TotalItemCount = result.TotalRows;
                    DataList.Clear();
                    if (result.List != null)
                    {
                        objList = (from item in objList
                                   where item.IsFreezed == false && item.Opd_Ipd_External_Id == ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmID &&
                                   item.Opd_Ipd_External_UnitId == ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmissionUnitID
                                   select item).ToList();
                        if (objList != null && objList.Count > 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "You are not able to Discharge Without final the Bill.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("", "You are not able to Discharge Without creating the Bill.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();

                    }
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        public void GetAdmittedDoctor()
        {
            clsGetVisitBizActionVO BizAction = new clsGetVisitBizActionVO();
            BizAction.ObjAdmission = new PalashDynamics.ValueObjects.IPD.clsGetIPDAdmissionBizActionVO();
            BizAction.ObjAdmission.Details = new PalashDynamics.ValueObjects.IPD.clsIPDAdmissionVO();
            BizAction.ObjAdmission.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.ObjAdmission.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
            BizAction.GetLatestVisit = true;
            BizAction.OPD_IPD_External = 1;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetVisitBizActionVO)arg.Result).ObjAdmission.Details != null && ((clsGetVisitBizActionVO)arg.Result).ObjAdmission.Details.ID > 0)
                    {
                        AdmissionID = ((clsGetVisitBizActionVO)arg.Result).ObjAdmission.Details.ID;
                        AdmissionUnitID = ((clsGetVisitBizActionVO)arg.Result).ObjAdmission.Details.UnitId;
                        VisitDoctorID = ((clsGetVisitBizActionVO)arg.Result).ObjAdmission.Details.DoctorID;
                    }
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void rdbDetail_Click(object sender, RoutedEventArgs e)
        {
            rdbDetail.IsChecked = true;
        }

        private void rdbInline_Click(object sender, RoutedEventArgs e)
        {
            rdbInline.IsChecked = true;
        }

        #region GST Details added by Ashish Z. on dated 24062017
        private void hlbServiceTaxAmt_Click(object sender, RoutedEventArgs e)
        {
            if (dgCharges.SelectedItem != null && (dgCharges.SelectedItem as clsChargeVO).ChargeTaxDetailsList != null && !(dgCharges.SelectedItem as clsChargeVO).IsEditTax)
            {
                frmServiceTaxTransactionDetails frmTaxDetails = new frmServiceTaxTransactionDetails();
                frmTaxDetails.objChargeVO = new clsChargeVO();
                frmTaxDetails.objChargeVO = dgCharges.SelectedItem as clsChargeVO;
                frmTaxDetails.IsTaxReadOnly = false;
                frmTaxDetails.IsBillFreezed = IsEditMode == false ? false : (dgBillList.SelectedItem as clsBillVO).IsFreezed;
                frmTaxDetails.OnSaveButton_Click += new RoutedEventHandler(win_OnTotalTaxSaveButton_Click);
                frmTaxDetails.OnCancelButton_Click += new RoutedEventHandler(win_OnCancelButton_Click);
                frmTaxDetails.Show();
            }
            else if (dgCharges.SelectedItem != null && (dgCharges.SelectedItem as clsChargeVO).ChargeTaxDetailsList == null && !(dgCharges.SelectedItem as clsChargeVO).IsEditTax)
            {
                FillTaxChargeList((dgCharges.SelectedItem as clsChargeVO).ID, (dgCharges.SelectedItem as clsChargeVO).UnitID);
            }
            else if ((dgCharges.SelectedItem as clsChargeVO).IsEditTax)
            {
                frmServiceTaxTransactionDetails frmTaxDetails = new frmServiceTaxTransactionDetails();
                frmTaxDetails.objChargeVO = new clsChargeVO();
                frmTaxDetails.objChargeVO = dgCharges.SelectedItem as clsChargeVO;
                frmTaxDetails.IsTaxReadOnly = false;
                frmTaxDetails.IsBillFreezed = IsEditMode == false ? false : (dgBillList.SelectedItem as clsBillVO).IsFreezed;
                frmTaxDetails.OnSaveButton_Click += new RoutedEventHandler(win_OnTotalTaxSaveButton_Click);
                frmTaxDetails.OnCancelButton_Click += new RoutedEventHandler(win_OnCancelButton_Click);
                frmTaxDetails.Show();
            }
        }

        void win_OnTotalTaxSaveButton_Click(object sender, RoutedEventArgs e)
        {
            frmServiceTaxTransactionDetails Itemswin = (frmServiceTaxTransactionDetails)sender;
            (dgCharges.SelectedItem as clsChargeVO).ChargeTaxDetailsList = (Itemswin as frmServiceTaxTransactionDetails).objChargeVO.ChargeTaxDetailsList;
            (dgCharges.SelectedItem as clsChargeVO).TotalServiceTaxAmount = (Itemswin as frmServiceTaxTransactionDetails).objChargeVO.TotalServiceTaxAmount;
            CalculateClinicalSummary();
        }

        private void FillTaxChargeList(long ChargeID, long ChargeUnitID)
        {
            try
            {
                clsGetChargeListBizActionVO BizAction = new clsGetChargeListBizActionVO();
                BizAction.ID = ChargeID;
                BizAction.UnitID = ChargeUnitID;
                BizAction.IsForTaxationDetails = true;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        clsChargeVO ChargeVO = new clsChargeVO();
                        if (((clsGetChargeListBizActionVO)arg.Result).ChargeVO.ChargeTaxDetailsList != null)
                        {
                            List<clsChargeTaxDetailsVO> objList = ((clsGetChargeListBizActionVO)arg.Result).ChargeVO.ChargeTaxDetailsList;

                            //if (dgCharges.SelectedItem != null)
                            //{
                            //    ChargeVO.TotalServiceTaxAmount = (dgCharges.SelectedItem as clsChargeVO).TotalServiceTaxAmount;
                            //    ChargeVO.ServiceName = (dgCharges.SelectedItem as clsChargeVO).ServiceName;
                            //}
                            ChargeVO = (dgCharges.SelectedItem as clsChargeVO);
                            ChargeVO.ChargeTaxDetailsList = objList.ToList();
                        }

                        if (ChargeVO != null && ChargeVO.ChargeTaxDetailsList != null)
                        {
                            frmServiceTaxTransactionDetails frmTaxDetails = new frmServiceTaxTransactionDetails();
                            frmTaxDetails.objChargeVO = ChargeVO;
                            frmTaxDetails.IsTaxReadOnly = false;
                            frmTaxDetails.IsBillFreezed = IsEditMode == false ? false : (dgBillList.SelectedItem as clsBillVO).IsFreezed;
                            frmTaxDetails.OnCancelButton_Click += new RoutedEventHandler(win_OnCancelButton_Click);
                            frmTaxDetails.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Tax is not applied for this service.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }
                };
                Client.ProcessAsync(BizAction, null);
                Client.CloseAsync();
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

        void win_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            frmServiceTaxTransactionDetails Itemswin = (frmServiceTaxTransactionDetails)sender;
            (dgCharges.SelectedItem as clsChargeVO).ChargeTaxDetailsList = (Itemswin as frmServiceTaxTransactionDetails).objChargeVO.ChargeTaxDetailsList;
            (dgCharges.SelectedItem as clsChargeVO).TotalServiceTaxAmount = (Itemswin as frmServiceTaxTransactionDetails).objChargeVO.TotalServiceTaxAmount;
            CalculateClinicalSummary();
        }

        public List<clsChargeTaxDetailsVO> GetServiceTaxList(List<clsServiceTaxVO> objList)
        {
            List<clsChargeTaxDetailsVO> ServiceTaxList = new List<clsChargeTaxDetailsVO>();
            foreach (var item in ((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsVO.TaxLinkingDetailsList.ToList())
            {
                clsChargeTaxDetailsVO ChargeTaxDetailsVO = new clsChargeTaxDetailsVO();
                ChargeTaxDetailsVO.ServiceId = item.ServiceId;
                ChargeTaxDetailsVO.TariffId = item.TariffId;
                ChargeTaxDetailsVO.ClassId = item.ClassId;
                ChargeTaxDetailsVO.TaxID = item.TaxID;
                ChargeTaxDetailsVO.Percentage = item.Percentage;
                ChargeTaxDetailsVO.ServiceTaxPercent = Convert.ToDouble(item.Percentage);
                ChargeTaxDetailsVO.TaxType = item.TaxType;
                ChargeTaxDetailsVO.IsTaxLimitApplicable = item.IsTaxLimitApplicable;
                ChargeTaxDetailsVO.TaxLimit = item.TaxLimit;
                ChargeTaxDetailsVO.ServiceName = item.ServiceName;
                ChargeTaxDetailsVO.TaxName = item.TaxName;
                ChargeTaxDetailsVO.Quantity = 1;
                ChargeTaxDetailsVO.Rate = ((clsChargeVO)dgCharges.SelectedItem).Rate;
                ChargeTaxDetailsVO.TotalAmount = 0;
                ServiceTaxList.Add(ChargeTaxDetailsVO);
            }

            double TotalTaxamount = 0;
            if (((clsChargeVO)dgCharges.SelectedItem) != null && ServiceTaxList != null && ServiceTaxList.Count() > 0)
            {
                ((clsChargeVO)dgCharges.SelectedItem).TaxType = ((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsVO.TaxLinkingDetailsList.ToList()[0].TaxType;
                foreach (var item in ServiceTaxList.ToList())
                {
                    item.Concession = ((clsChargeVO)dgCharges.SelectedItem).Concession;
                    item.TotalAmount = ((clsChargeVO)dgCharges.SelectedItem).TotalAmount;
                    item.Rate = ((clsChargeVO)dgCharges.SelectedItem).Rate;
                    TotalTaxamount += item.ServiceTaxAmount;
                }
                ((clsChargeVO)dgCharges.SelectedItem).TotalServiceTaxAmount = TotalTaxamount;

                CalculateClinicalSummary();
            }
            return ServiceTaxList;
        }
        #endregion

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {

        }
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (dgCharges.SelectedItem != null)
            {
                if (((clsChargeVO)dgCharges.SelectedItem).IsPackageConsumption == true)
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

       
       
    }

    public class clsSpecialSpecializationVO
    {
        public long SpecializationId { get; set; }
        public double SpecializationRate { get; set; }
        //public string Code { get; set; }
    }

}
