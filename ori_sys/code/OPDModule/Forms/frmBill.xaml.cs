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
using OPDModule;
using OPDModule.Forms;
using PalashDynamics.ValueObjects.Patient;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.Pharmacy;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using PalashDynamics.Collections;
using System.Windows.Browser;
using System.Text;
using PalashDynamics.Pharmacy.ItemSearch;
using PalashDynamics.ValueObjects.Radiology;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects.Administration.UserRights;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Reflection;
using PalashDynamics.ValueObjects.Administration.PackageNew;
using PalashDynamics.ValueObjects.Master.DoctorMaster;
using PalashDynamics.ValueObjects.Administration;
using MessageBoxControl;
using PalashDynamics.Service.EmailServiceReference;
using PalashDynamics.ValueObjects.CRM;
using PalashDynamics.ValueObjects.Log;

namespace CIMS.Forms
{
    public partial class frmBill : UserControl, IInitiateCIMS
    {
        public enum RequestType
        {
            Concession = 1,
            Refund = 2
        }

        bool flagFreezFromSearch = false;
        double TotalConcession = 0;
        double PatAdvanceAmt = 0;
        double PatBalAdvaceAmt = 0;
        double PatPaybleAmt = 0;
        double concession_Reasonid = 0;
        //* Commented by - Ajit Jadhav
        //* Added Date - 26/8/2016
        //* Comments - Set pan number
        string PanNumber = string.Empty;
        bool IsPackageConsumption = false;
        bool IsMsgServiceConsumption = false;

        public Boolean AgainstDonor = false;
        public long LinkPatientID;
        public long LinkPatientUnitID;
        public long LinkCompanyID;

        //***//-------------------

        // Added By CDS
        public decimal DiscRateOnForm = 0;
        bool AssignForOnlyOnce = false;
        // Added By CDS
        public long serviceID = 0;
        #region "Paging"
        public PagedSortableCollectionView<clsBillVO> DataList { get; private set; }
        public ObservableCollection<clsChargeVO> BillAddedItems { get; set; }
        public ObservableCollection<clsChargeVO> DeleteBillAddedItems { get; set; }
        public long _TariffID;
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
            FillBillSearchList();

        }

        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        /// 
        public bool IsPackageFreezed = false;
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
                BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;

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
                            DataList.Add(item);
                            PatPaybleAmt = PatPaybleAmt + item.NetBillAmount;
                        }

                        dgBillList.ItemsSource = null;
                        dgBillList.ItemsSource = DataList;

                        dgDataPager.Source = null;
                        dgDataPager.PageSize = BizAction.MaximumRows;
                        dgDataPager.Source = DataList;

                        var items4 = from r in result.List
                                     where r.IsPackageServiceInclude == true && r.IsFreezed == false
                                     select r;
                        if (items4.ToList().Count() > 0)
                        {
                            IsPackageFreezed = true;
                        }

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
        long VisitID = 0;
        long VisitUnitID = 0;
        bool IsFromViewClick = false;
        long PRID = 0;
        long CashCounterID = 0;
        String CashCounterName = "";

        //long ConcessionReasonID = 0;  //Added By Yogesh K

        //by Anjali.....................
        public bool IsFromRequestApproval = false;
        //...............................

        // Added By CDS...................
        public bool IsFromSaveAndBill = false;
        //END................................
        // Added By BHUSHANP    FOR Package...................
        public bool IsFromSaveAndPackageBill = false;
        public long PackageVisitDoctorID { get; set; }
        public long AdvanceID { get; set; }
        public long AdvanceUnitID { get; set; }
        //END................................

        public clsAddAdvanceBizActionVO AdvanceBizActionVO { get; set; }    //added on 16082018 For Package Advance & Bill Save in transaction

        bool IsFreeConcessionForFollowup = false;

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
                    if (((IApplicationConfiguration)App.Current).SelectedPatient.IsSpouse == true)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Donor Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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
                    mElement.Text += " - " + ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;

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
                    //PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;

                    UserControl rootPage2 = Application.Current.RootVisual as UserControl;
                    TextBlock mElement3 = (TextBlock)rootPage2.FindName("SampleSubHeader");



                    mElement3.Text = " : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                        " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                        ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;

                    mElement3.Text += " - " + ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                    FromVisit = true;

                    // Added By CDS
                    if (IsFromSaveAndBill == true)
                    {
                        //mElement3 = (TextBlock)rootPage2.FindName("Bill Details");
                        //mElement3.Text = "Bill Details";
                    }
                    if (IsFromSaveAndPackageBill)
                    {
                        PackageVisitDoctorID = ((IApplicationConfiguration)App.Current).SelectedPatient.DoctorID;
                    }
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
        // Added by PMG 
        long PatientCompanyID { get; set; }

        // For Activity Log By Umesh
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

        #region Commented By CDS CheckVisit() Function
        ///// <summary>
        ///// Function is for checking active visit for patient is present or not.
        /////If active visit is present then it calls the functions GetPatientDetails and GetVisitDetails.
        ///// </summary>
        //private void CheckVisit()
        //{
        //    WaitIndicator ind = new WaitIndicator();
        //    ind.Show();
        //    clsGetVisitBizActionVO BizAction = new clsGetVisitBizActionVO();
        //    //BizAction.IsActive = true;
        //    BizAction.Details.PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
        //    BizAction.Details.PatientUnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
        //    BizAction.GetLatestVisit = true;

        //    //BizAction.Details.VisitId = ((clsVisitVO)dgEncounterList.SelectedItem).VisitId;

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null && arg.Result != null)
        //        {
        //            if (((clsGetVisitBizActionVO)arg.Result).Details != null && ((clsGetVisitBizActionVO)arg.Result).Details.ID > 0 && ((clsGetVisitBizActionVO)arg.Result).Details.VisitStatus == true)
        //            {
        //                InitialiseForm();
        //                SetCommandButtonState("ClickNew");
        //                //InitialiseForm();
        //                GetPatientDetails();
        //                GetVisitDetails();
        //                if (((IApplicationConfiguration)App.Current).SelectedPatient.VisitID > 0)
        //                {
        //                    long lUnitID;
        //                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
        //                    {
        //                        lUnitID = 0;
        //                    }
        //                    else
        //                    {
        //                        lUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
        //                    }
        //                    FillChargeList(0, lUnitID, false, ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID);
        //                }
        //                _flip.Invoke(RotationType.Forward);
        //                ind.Close();
        //            }
        //            else
        //            {
        //                ind.Close();
        //                // System.Windows.Browser.HtmlPage.Window.Alert("Visit is not marked for the Patient");
        //                MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Visit Is Not Marked For The Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //                msgW1.Show();

        //                lnkAddService.IsEnabled = false;
        //                lnkAddServicesFromPrescription.IsEnabled = false;
        //                lnkAddItemsFromPrescription.IsEnabled = false;
        //                lnkAddItems.IsEnabled = false;
        //                cmdSave.IsEnabled = false;
        //                return;
        //            }
        //        }
        //    };
        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        //}
        #endregion

        // Added By CDS for VISIT And BILL Case 

        /// <summary>
        /// Function is for checking active visit for patient is present or not.
        ///If active visit is present then it calls the functions GetPatientDetails and GetVisitDetails.
        /// </summary>
        private void CheckVisit(bool IsDefaultService)
        {

            WaitIndicator ind = new WaitIndicator();
            ind.Show();

            clsGetVisitBizActionVO BizAction = new clsGetVisitBizActionVO();
            BizAction.Details.PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.Details.PatientUnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            BizAction.GetLatestVisit = true;



            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetVisitBizActionVO)arg.Result).Details != null && ((clsGetVisitBizActionVO)arg.Result).Details.ID > 0 && ((clsGetVisitBizActionVO)arg.Result).Details.VisitStatus == true)
                    {

                        VisitID = ((clsGetVisitBizActionVO)arg.Result).Details.ID;
                        VisitUnitID = ((clsGetVisitBizActionVO)arg.Result).Details.UnitId;

                        InitialiseForm();
                        SetCommandButtonState("ClickNew");

                        GetPatientDetails();
                        GetVisitDetails();

                        if (VisitID > 0)
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


                            FillChargeList(0, lUnitID, false, VisitID, VisitUnitID, IsDefaultService);

                        }
                        if (IsPackagePatient == true)
                            lnkAddPackageService.IsEnabled = true;
                        _flip.Invoke(RotationType.Forward);

                        FillPatientAdvance(true); // Added By CDS To Fill Proper Advanced Details 

                        ind.Close();

                    }
                    else
                    {
                        ind.Close();

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Visit is not marked for the Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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

        // END



        public frmBill()
        {

            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmBill_Loaded);
            _flip = new PalashDynamics.Animations.SwivelAnimation(LayoutRoot1, LayoutRoot2, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            dtpFromDate.SelectedDate = DateTime.Now.Date;
            dtpToDate.SelectedDate = DateTime.Now.Date;

            fillDoctor();
            dgCharges.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgCharges_CellEditEnded);
            dgPharmacyItems.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgPharmacyItems_CellEditEnded);
            IsAuditTrail = ((IApplicationConfiguration)App.Current).CurrentUser.IsAuditTrail;// By Umesh For Enable/Disable Audit Trail
            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsBillVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;
            //======================================================

            //By Yogesh Added combo box fill concession reason ==>fillreason();
            fillreason();
            //       IsAuditTrail = HMSConfigurationManager.GetValueFromApplicationConfig();
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

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    objList.Clear();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);

                    DoctorList = objList;

                    //cmbDoctor.ItemsSource = null;
                    //cmbDoctor.ItemsSource = objList;
                    //cmbDoctor.SelectedValue = (long)0;
                    if (IsFromSaveAndPackageBill)
                    {
                        ChargeList = new ObservableCollection<clsChargeVO>();
                        dgCharges.ItemsSource = ChargeList;
                        dgCharges.Focus();
                        dgCharges.UpdateLayout();
                        AddCharges(lServices, _TariffID);
                    }

                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        void dgPharmacyItems_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            if (e.Column.Header.ToString().Equals("Quantity"))//(e.Column.DisplayIndex == 6)
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
            if (e.Column.Header.ToString().Equals("Rate")) //(e.Column.DisplayIndex == 3)
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
            else if (e.Column.Header.ToString().Equals("Concession %") || (e.Column.Header.ToString().Equals("Concession Amt.")))//(e.Column.DisplayIndex == 5 || e.Column.DisplayIndex == 6)
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
                        if (objUser.OPDAuthLvtForConcessionID == 0 && IsEditMode == false)
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
                                else if ((dgBillList.SelectedItem as clsBillVO).LevelID == 0 && objUser.OPDAuthLvtForConcessionID == 0 && IsEditMode == true && (dgBillList.SelectedItem as clsBillVO).IsApproved == false)
                                {
                                    cmbConcessionReason.IsEnabled = true;
                                    //txtApprovalRemark.IsEnabled = true;
                                }
                                else if ((dgBillList.SelectedItem as clsBillVO).LevelID == 0 && (dgBillList.SelectedItem as clsBillVO).IsApproved == false)
                                {
                                    cmbConcessionReason.IsEnabled = false;
                                    // txtApprovalRemark.IsEnabled = false;
                                }
                            }
                        }
                        //  cmbConcessionReason.IsEnabled = true;
                        if ((MasterListItem)cmbConcessionReason.SelectedItem == null)
                        {
                            cmbConcessionReason.TextBox.SetValidation("Select Concession Reason");
                            cmbConcessionReason.TextBox.RaiseValidationError();
                            cmbConcessionReason.Focus();
                            // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                            //  result = false;
                        }
                        else if (((MasterListItem)cmbConcessionReason.SelectedItem).ID == 0)
                        {
                            cmbConcessionReason.TextBox.SetValidation("Select Concession Reason");
                            cmbConcessionReason.TextBox.RaiseValidationError();
                            cmbConcessionReason.Focus();
                            // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                            // result = false;
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
                        {
                            txtApprovalRemark.ClearValidationError();
                        }


                    }
                    else
                    {
                        cmbConcessionReason.TextBox.ClearValidationError();
                        txtApprovalRemark.ClearValidationError();
                        //if (Convert.ToDecimal(txtClinicalConcession.Text) <= 0)//by Yogesh Enable/disable reason cmb box
                        //{
                        //    cmbConcessionReason.SelectedValue = (long)0;
                        //    //cmbConcessionReason.SelectedValue = 0;
                        //    cmbConcessionReason.IsEnabled = false;
                        //}

                        if (((clsChargeVO)dgCharges.SelectedItem).Concession <= 0)
                        {
                            cmbConcessionReason.SelectedValue = (long)0;
                            //cmbConcessionReason.SelectedValue = 0;
                            cmbConcessionReason.IsEnabled = false;
                            //txtApprovalRemark.IsEnabled = false;
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
                    //else

                    //{
                    //    if (((clsChargeVO)dgCharges.SelectedItem).Concession > 0)
                    //    {
                    //        cmbConcessionReason.IsEnabled = true;
                    //    }
                    //}
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
            else if (e.Column.Header.ToString().Equals("Quantity"))//(e.Column.DisplayIndex == 4)//Quantity
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
                            LogInformation.Message = " 2 : Line Number : " //+ Convert.ToString(lineNumber)
                                                                    + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                                                                    + " , Service Code : " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).ServiceId)
                                                                    + " , Service Name : " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).ServiceName)
                                //   + " , Rate : " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).Rate)
                                                                    + " , Quantity : " + Convert.ToString(((clsChargeVO)dgCharges.SelectedItem).Quantity)
                                                                    ;
                            LogInfoList.Add(LogInformation);
                        }
                    }
                }
            }
            //else if (e.Column.DisplayIndex == 5 || e.Column.DisplayIndex == 6)
            //{
            //    if (dgCharges.SelectedItem != null)
            //    {
            //        if (((clsChargeVO)dgCharges.SelectedItem).ConcessionAmount>0)
            //        {
            //            cmbConcessionReason.IsEnabled = true;
            //        }
            //    }
            //}


            if (e.Column.Header.ToString().Equals("Rate") | e.Column.Header.ToString().Equals("Quantity") | e.Column.Header.ToString().Equals("Concession %") | e.Column.Header.ToString().Equals("Concession Amt."))
                //(e.Column.DisplayIndex == 3 | e.Column.DisplayIndex == 4 | e.Column.DisplayIndex == 5 | e.Column.DisplayIndex == 6)
                CalculateClinicalSummary();
            fillDoctor();
            if (IsAuditTrail)
            {
                LogInformation = new LogInfo();  // data after cell_editend
                LogInformation.guid = objGUID;
                LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                LogInformation.TimeStamp = DateTime.Now;
                LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                LogInformation.Message = " 3 : Line Number : " //+ Convert.ToString(lineNumber)
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
            //BizAction.IsActive = true;
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

                //if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientKind == PalashDynamics.ValueObjects.PatientsKind.IPD)
                //{
                //   // return;
                //    frmIPDBill IPDWin = new frmIPDBill();
                //    IPDWin.Initiate("NEW");
                //    ((IApplicationConfiguration)App.Current).OpenMainContent(IPDWin);
                //}
                //else
                if (IsPatientExist == false)
                {
                    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                    if (((IApplicationConfiguration)App.Current).SelectedPatient.IsSurrogate == true)
                        ((IApplicationConfiguration)App.Current).OpenMainContent("PalashDynamics.Forms.PatientView.PatientListForSurrogacy"); //
                    else
                        ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                    // ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                    //((IApplicationConfiguration)App.Current).FillMenu("Find Patient");
                }

                SetCommandButtonState("New");
                _flip.Invoke(RotationType.Backward);
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

                // Added by CDS 
                if (FromVisit == true)
                {
                    SelectedBill = null;
                    IsEditMode = false;
                    rdbAgainstBill.IsEnabled = false;
                    rdbAgainstBill.IsChecked = true;
                    rdbAgainstServices.IsEnabled = false;
                    CheckVisit(true);
                }

                FillPrintFormat();
                //FillCostingDivisions();  //Fill and Set Costing Divisions for Pharmacy Billing
                // Added by CDS
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                {
                    CashCounterID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
                    CashCounterName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterName;
                    cmbCostingDivision.SelectedItem = CashCounterName.ToString();
                }
                else
                    CashCounterID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;


                //By Anjali.....................
                cmdApprove.IsEnabled = false;


                GetUserRights(((IApplicationConfiguration)App.Current).CurrentUser.ID);
                //........................................

            }
            cmbConcessionReason.IsEnabled = false;
            //txtApprovalRemark.IsEnabled = false;
            if (IsFromRequestApproval == true)
            {
                IsEditMode = true;
                InitialiseForm();
                SetCommandButtonState("Modify");
                rdbAgainstBill.IsChecked = true;
                rdbAgainstBill.IsEnabled = false;
                rdbAgainstServices.IsEnabled = false;
                //txtApprovalRemark.IsReadOnly = false;
                txtApprovalRemark.IsEnabled = true;
                if (SelectedBill != null)
                {
                    BillID = SelectedBill.ID;
                    PatientSourceID = SelectedBill.PatientSourceId;
                    PatientTariffID = SelectedBill.TariffId;
                    //Added By CDS
                    // FillChargeList(SelectedBill.ID, SelectedBill.UnitID, false, SelectedBill.Opd_Ipd_External_Id);
                    FillChargeList(SelectedBill.ID, SelectedBill.UnitID, false, SelectedBill.Opd_Ipd_External_Id, SelectedBill.Opd_Ipd_External_UnitId, false);
                    FillPharmacyItemsList(SelectedBill.ID, SelectedBill.UnitID, false);
                    chkFreezBill.IsChecked = SelectedBill.IsFreezed;
                }
                _flip.Invoke(RotationType.Forward);

                cmbConcessionReason.SelectedValue = SelectedBill.ConcessionReasonId;
                txtApprovalRemark.Text = Convert.ToString(SelectedBill.ConcessionRemark);
                if (objUser.OPDAuthLvtForConcessionID == 0 && IsEditMode == false)
                {
                    cmbConcessionReason.IsEnabled = true;
                    //  txtApprovalRemark.IsEnabled = true;
                }
                else
                {
                    if (IsEditMode == true && dgBillList.SelectedItem != null)
                    {
                        if ((dgBillList.SelectedItem as clsBillVO).LevelID > 0)
                        {
                            cmbConcessionReason.IsEnabled = true;
                            //txtApprovalRemark.IsEnabled = true;
                        }
                        else if ((dgBillList.SelectedItem as clsBillVO).LevelID == 0 && (dgBillList.SelectedItem as clsBillVO).IsApproved == false)
                        {
                            cmbConcessionReason.IsEnabled = false;
                            // txtApprovalRemark.IsEnabled = false;
                        }
                    }
                }

            }
            else
            {
                //txtApprovalRemark.IsReadOnly = true;
                // txtApprovalRemark.IsEnabled = false;
            }


            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

            mElement.Text = " : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;
            mElement.Text += " - " + ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;

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
                        //if (objUser.OpdAuthLvl != null)
                        //{
                        //    if (objUser.OpdAuthLvl == SelectedBill.LevelID)
                        //    {
                        //        MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                        //        new MessageBoxControl.MessageBoxChildWindow("Palash", "Bill is already approved with " + objUser.AuthLevelForRefundOPD + " rights ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                        //        msgWindowUpdate.Show();
                        //    }
                        //    else
                        //    {
                        //        MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                        //          new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to Approve the Refund with " + objUser.AuthLevelForRefundOPD + " rights", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        //        msgWindowUpdate.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindowUpdate_OnMessageBoxClosed);

                        //        msgWindowUpdate.Show();
                        //    }
                        //}

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

        #region Commnted By CDS FillChargeList(long PBillID, long pUnitID, bool pIsBilled, long pVisitID) For Package
        ///// <summary>
        ///// Function is for Fetching Charge list of bill/patient based on parameters sent to the function
        ///// </summary>
        ///// <param name="PBillID"></param>
        ///// <param name="pUnitID"></param>
        ///// <param name="pIsBilled"></param>
        ///// <param name="pVisitID"></param>
        //private void FillChargeList(long PBillID, long pUnitID, bool pIsBilled, long pVisitID)
        //{
        //    try
        //    {
        //        Indicatior = new WaitIndicator();
        //        Indicatior.Show();f
        //        clsGetChargeListBizActionVO BizAction = new clsGetChargeListBizActionVO();
        //        BizAction.ID = 0;
        //        BizAction.Opd_Ipd_External_Id = pVisitID;
        //        BizAction.Opd_Ipd_External_UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
        //        BizAction.IsBilled = pIsBilled;
        //        BizAction.BillID = PBillID;
        //        BizAction.UnitID = pUnitID; // ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        Client.ProcessCompleted += (s, arg) =>
        //        {
        //            if (arg.Error == null && arg.Result != null)
        //            {
        //                if (((clsGetChargeListBizActionVO)arg.Result).List != null)
        //                {
        //                    List<clsChargeVO> objList;// = new List<clsChargeVO>();
        //                    objList = ((clsGetChargeListBizActionVO)arg.Result).List;

        //                    foreach (var item in objList)
        //                    {
        //                        item.Doctor = DoctorList;
        //                        ChargeList.Add(item);
        //                    }
        //                    dgCharges.Focus();
        //                    dgCharges.UpdateLayout();
        //                    dgCharges.ItemsSource = ChargeList;
        //                    CalculateClinicalSummary();
        //                    if (SelectedBill != null) SelectedBill.ChargeDetails = objList;
        //                }
        //            }
        //            Indicatior.Close();
        //        };
        //        Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //        Client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        Indicatior.Close();
        //    }
        //}
        #endregion


        /// <summary>
        /// Function is for Fetching Charge list of bill/patient based on parameters sent to the function
        /// </summary>
        /// <param name="PBillID"></param>
        /// <param name="pUnitID"></param>
        /// <param name="pIsBilled"></param>
        /// <param name="pVisitID"></param>
        private void FillChargeList(long PBillID, long pUnitID, bool pIsBilled, long pVisitID, long pVisitUnitID, bool IsDefaultService)
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
                            objList = ((clsGetChargeListBizActionVO)arg.Result).List;
                            PackageServiceList = new ObservableCollection<PalashDynamics.ValueObjects.clsPackageServiceDetailsVO>();
                            foreach (var itemObjList in objList)
                            {
                                if (!this.IsFromViewClick)
                                    itemObjList.InitialRate = itemObjList.Rate;  // to maintain the Initial Rate of Service after changing Doctor Service Rate.

                                itemObjList.Doctor = DoctorList;
                                itemObjList.IsDefaultService = IsDefaultService;
                                if (!string.IsNullOrEmpty(Convert.ToString(itemObjList.DoctorId)))
                                {
                                    if (itemObjList.DoctorId > 0)
                                    {
                                        itemObjList.SelectedDoctor = DoctorList.FirstOrDefault(p => p.ID == itemObjList.DoctorId);
                                    }
                                    else
                                    {
                                        itemObjList.SelectedDoctor = DoctorList.FirstOrDefault(p => p.ID == 0);
                                    }
                                }

                                if (!IsFromViewClick)
                                {
                                    #region GST Details added by Ashish Z. on dated 24062017
                                    try
                                    {
                                        clsGetServiceTaxDetailsBizActionVO BizActionObj = new clsGetServiceTaxDetailsBizActionVO();
                                        BizActionObj.ServiceTaxDetailsVO = new clsServiceTaxVO();
                                        BizActionObj.ServiceTaxDetailsVO.ServiceId = itemObjList.ServiceId;
                                        Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                                        PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                                        client1.ProcessCompleted += (s1, args) =>
                                        {
                                            if (args.Error == null && args.Result != null)
                                            {
                                                clsGetServiceTaxDetailsBizActionVO objVO = new clsGetServiceTaxDetailsBizActionVO();
                                                objVO = (args.Result as clsGetServiceTaxDetailsBizActionVO);

                                                if (objVO.ServiceTaxDetailsVOList != null && objVO.ServiceTaxDetailsVOList.Count > 0)
                                                {
                                                    if (itemObjList.ChargeTaxDetailsList == null)
                                                        itemObjList.ChargeTaxDetailsList = new List<clsChargeTaxDetailsVO>();

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

                                                        ChargeTaxDetailsVO.ServiceName = itemObjList.ServiceName;
                                                        ChargeTaxDetailsVO.TariffName = item.TariffName;
                                                        ChargeTaxDetailsVO.TariffName = item.TariffName;
                                                        ChargeTaxDetailsVO.TaxName = item.TaxName;

                                                        ChargeTaxDetailsVO.Quantity = 1;
                                                        ChargeTaxDetailsVO.Rate = Convert.ToDouble(itemObjList.Rate);
                                                        ChargeTaxDetailsVO.TotalAmount = itemObjList.Rate * itemObjList.Quantity;

                                                        itemObjList.ChargeTaxDetailsList.Add(ChargeTaxDetailsVO);
                                                    }

                                                    double TotalTaxamount = 0;
                                                    itemObjList.TaxType = itemObjList.ChargeTaxDetailsList.ToList()[0].TaxType;
                                                    foreach (var item in itemObjList.ChargeTaxDetailsList.ToList())
                                                    {
                                                        item.Concession = itemObjList.Concession;
                                                        item.TotalAmount = itemObjList.TotalAmount;
                                                        item.Rate = itemObjList.Rate;
                                                        TotalTaxamount += itemObjList.ServiceTaxAmount;
                                                    }
                                                    itemObjList.TotalServiceTaxAmount = TotalTaxamount;
                                                }
                                            }

                                            CalculateClinicalSummary(); //added by Ashish Z. for Taxation Details on dated 18052017
                                        };
                                        client1.ProcessAsync(BizActionObj, null); //((IApplicationConfiguration)App.Current).CurrentUser
                                        client1.CloseAsync();
                                    }
                                    catch (Exception)
                                    {
                                        throw;
                                    }
                                    #endregion

                                    if (IsFromSaveAndPackageBill != true)
                                    {
                                        ChargeList.Add(itemObjList);
                                    }
                                }
                                else
                                {
                                    ChargeList.Add(itemObjList);
                                }

                                if (IsFromViewClick == true && itemObjList.isPackageService == true)    // For Package New Changes Added on 20062018
                                {
                                    if (SelectedBill != null && SelectedBill.IsFreezed == false && SelectedBill.IsAdjustableBillDone == true)  // && SelectedBill.IsAllBillSettle == true
                                    {
                                        itemObjList.Rate = Math.Round(SelectedBill.PackageSettleRate, 0);
                                    }
                                }
                            }

                            if (IsDefaultService == true)
                            {

                                //GetPackageConditionalServicesAndRelations(objList);
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

        private void FillChargeList_New(long PBillID, long pUnitID, bool pIsBilled, long pVisitID, long pVisitUnitID, bool IsDefaultService)
        {
            // WaitIndicator Indicat = new WaitIndicator();
            try
            {
                //Indicat.Show();
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
                            objList = ((clsGetChargeListBizActionVO)arg.Result).List;
                            PackageServiceList = new ObservableCollection<PalashDynamics.ValueObjects.clsPackageServiceDetailsVO>();
                            foreach (var item in objList)
                            {
                                if (!this.IsFromViewClick)
                                    item.InitialRate = item.Rate;  // to maintain the Initial Rate of Service after changing Doctor Service Rate.

                                item.Doctor = DoctorList;
                                item.IsDefaultService = IsDefaultService;
                                if (!string.IsNullOrEmpty(Convert.ToString(item.DoctorId)))
                                {
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

                                if (IsFromViewClick == true && item.isPackageService == true)    // For Package New Changes Added on 21062018
                                {
                                    if (SelectedBill != null && SelectedBill.IsFreezed == false && SelectedBill.IsAdjustableBillDone == true)  // && SelectedBill.IsAllBillSettle == true
                                    {
                                        item.Rate = Math.Round(SelectedBill.PackageSettleRate, 0);
                                    }
                                }
                            }

                            if (IsDefaultService == true)
                            {
                                //GetPackageConditionalServicesAndRelations(objList);
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
                                paymentWin.PatientCategoryID = SelectedBill.PatientCategoryId;//this.PatientCategoryID;  //Ashish Z
                                if (!(txtPayAmount.Text == null) && !(txtPayAmount.Text.Trim().Length == 0))
                                    paymentWin.TotalAmount = double.Parse(txtPayAmount.Text);
                                paymentWin.Initiate("Bill");
                                paymentWin.txtPayTotalAmount.Text = this.txtNetAmount.Text;
                                paymentWin.txtDiscAmt.Text = this.txtTotalConcession.Text;
                                paymentWin.txtPayableAmt.Text = this.txtPayAmount.Text;

                                //* Added by - Ajit Jadhav
                                //* Added Date - 13/10/2016
                                //* Comments - mandatorily capture Pan No 
                                if (!String.IsNullOrEmpty(PanNumber) && PanNumber != "NULL")
                                {
                                    paymentWin.txtPanNo.Text = PanNumber;
                                    paymentWin.txtPanNo.IsEnabled = false;
                                }

                                //***//-------------------------

                                //Added By Parmeshwar
                                paymentWin.conID = ((clsBillVO)dgBillList.SelectedItem).ConcessionAuthorizedBy;

                                //Added By CDS 
                                paymentWin.PackageConcenssionAmt = PackageConcenssion;
                                if ((Convert.ToDouble(this.txtTotalConcession.Text) - PackageConcenssion) > 0.0)
                                {
                                    paymentWin.ConcessionFromPlan = false;
                                }
                                else
                                {
                                    paymentWin.ConcessionFromPlan = true;
                                }
                                //Added By CDS 

                                if (rdbAgainstBill.IsChecked == true)
                                {
                                    paymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstBill;
                                }
                                else
                                    paymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstServices;

                                if (ChargeList != null && ChargeList.Count > 0)
                                {
                                    paymentWin.PackageID = ChargeList.Select(z => z.PackageID).FirstOrDefault();
                                    paymentWin.PackageBillID = SelectedBill.ID;
                                    paymentWin.PackageBillUnitID = SelectedBill.UnitID;
                                }

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
                Indicatior.Close();
            }
            finally
            {
                //Indicatior.Close();
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

                // New Code by CDS
                //BizAction.SponsorID = ((MasterListItem)((ServiceSearchForPackage)sender).cmbTariff.SelectedItem).SponsorID;
                //BizAction.SponsorUnitID = ((MasterListItem)((ServiceSearchForPackage)sender).cmbTariff.SelectedItem).SponsorUnitID;

                //BizAction.SponsorID = ((MasterListItem)cmbTariff.SelectedItem).SponsorID;
                //BizAction.SponsorUnitID = ((MasterListItem)cmbTariff.SelectedItem).SponsorUnitID;                

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


            objApplyNewRate.ipVisitID = VisitID;

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
                Indicatior.Close();
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
            updateVisit.VisitID = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;
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

            BizAction.Details.ID = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;

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

                    if (IsFromSaveAndPackageBill)
                    {
                        //ChargeList = new ObservableCollection<clsChargeVO>();
                        //dgCharges.ItemsSource = ChargeList;
                        //dgCharges.Focus();
                        //dgCharges.UpdateLayout();
                        //AddCharges(lServices, _TariffID);
                        fillDoctor();
                    }
                }
                //Indicatior.Close();
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void CheckVisitType(long pVisitTypeID)
        {
            clsGetVisitTypeBizActionVO BizAction = new clsGetVisitTypeBizActionVO();
            //BizAction.IsActive = true;
            BizAction.ID = pVisitTypeID;
            //BizAction.MasterList = new List<MasterListItem>();
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
                        serviceID = objVO.ServiceID;
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
            //if (this.DataContext == null)
            //{
            //    System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
            //    return;
            //}

            //if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
            //{
            //    System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
            //    return;
            //}

        }

        /// <summary>
        /// Checks & assigns validations for the controls on the form
        /// </summary>
        /// <returns></returns>
        private bool CheckValidations()
        {
            bool isValid = true;
            //if (txtNetAmount.Text.Trim() == "")
            //{
            //    isValid = false;
            //    string msgTitle = "";
            //    string msgText = "You can not save the Bill with Zero amount";
            //    MessageBoxControl.MessageBoxChildWindow msgWD =
            //        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    msgWD.Show();
            //}
            //else if (Convert.ToDouble(txtNetAmount.Text) <= 0)
            //{
            //    isValid = false;
            //    string msgTitle = "";
            //    string msgText = "You can not save the Bill with Zero amount";
            //    MessageBoxControl.MessageBoxChildWindow msgWD =
            //        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    msgWD.Show();
            //}

            //if ((MasterListItem)cmbCostingDivision.SelectedItem == null || ((MasterListItem)cmbCostingDivision.SelectedItem).ID == 0)
            //{
            //    cmbCostingDivision.TextBox.SetValidation("Please Select Costing Division ");
            //    cmbCostingDivision.TextBox.RaiseValidationError();
            //    cmbCostingDivision.Focus();
            //    return false;
            //}
            //else
            //    cmbCostingDivision.ClearValidationError();

            //Commented By CDS 
            //if ((IsEditMode == false && ChargeList.Count <= 0) || (dgBillList.SelectedItem != null && ((clsBillVO)dgBillList.SelectedItem).BillType == BillTypes.Clinical && ChargeList.Count <= 0))
            //{
            //    isValid = false;
            //    string msgText = "You Can Not Save The Bill Without Services";
            //    MessageBoxControl.MessageBoxChildWindow msgWD =
            //        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    msgWD.Show();
            //}
            //Commented By CDS 

            //Added By CDS 
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
            //END

            #region
            if (PharmacyItems.Count > 0 || ChargeList.Count > 0)
            {
                //Added By Yogesh K 20 Apr 2016 validation condition 

                if (Convert.ToDecimal(txtClinicalConcession.Text) > 0)
                {
                    var _ObjChargeList = ChargeList.Where(s => s.PackageID == 0 && s.Concession > 0).ToList();
                    var _ObjChargeList_New = ChargeList.Where(s => s.ServiceId != serviceID && s.Concession > 0).ToList();
                    if (_ObjChargeList.Count > 0 && _ObjChargeList_New.Count > 0)  //|| _ObjChargeList_New.Count > 0
                    {
                        if (((MasterListItem)cmbConcessionReason.SelectedItem).ID == 0)
                        {
                            if (objUser.OPDAuthLvtForConcessionID == 0 && IsEditMode == false)
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
                                    else if ((dgBillList.SelectedItem as clsBillVO).LevelID == 0 && (dgBillList.SelectedItem as clsBillVO).IsApproved == false && IsEditMode == false)//Addeed yogesh K-->&& IsEditMode == true
                                    {
                                        cmbConcessionReason.IsEnabled = false;
                                        //   txtApprovalRemark.IsEnabled = false;
                                    }

                                    else if ((dgBillList.SelectedItem as clsBillVO).LevelID == 0 && (dgBillList.SelectedItem as clsBillVO).IsApproved == false && IsEditMode == true)//Added yogesh
                                    {
                                        cmbConcessionReason.IsEnabled = true;         //Added Yogesh
                                        //txtApprovalRemark.IsEnabled = true;
                                    }
                                }
                            }

                            if ((MasterListItem)cmbConcessionReason.SelectedItem == null)  //&& IsFreeConcessionForFollowup==false
                            {
                                isValid = false;
                                cmbConcessionReason.TextBox.SetValidation("Concession Reason Is Required");
                                cmbConcessionReason.TextBox.RaiseValidationError();
                                cmbConcessionReason.Focus();
                                //  PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                                return isValid;
                            }
                            else if (((MasterListItem)cmbConcessionReason.SelectedItem).ID == 0)  //&& IsFreeConcessionForFollowup == false
                            {
                                isValid = false;
                                cmbConcessionReason.TextBox.SetValidation("Concession Reason  Is Required");
                                cmbConcessionReason.TextBox.RaiseValidationError();
                                cmbConcessionReason.Focus();
                                // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                                // result = false;
                                return isValid;
                            }
                            else
                            {
                                cmbConcessionReason.TextBox.ClearValidationError();
                                isValid = true;
                            }
                        }
                    }
                    if (_ObjChargeList_New.Count > 0)
                    {
                        if (String.IsNullOrEmpty(txtApprovalRemark.Text.Trim()) && IsFreeConcessionForFollowup == false)
                        {
                            txtApprovalRemark.SetValidation("Please Select Remark");
                            txtApprovalRemark.RaiseValidationError();
                            txtApprovalRemark.Focus();
                            isValid = false;
                        }
                        else if ((txtApprovalRemark.Text.Trim()).Length < 3 && IsFreeConcessionForFollowup == false)
                        {
                            txtApprovalRemark.SetValidation("Minimum Remark Length Should be 3 Character");
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
            }

            if (chkFreezBill.IsChecked == true && isValid != false)
            {
                isValid = CheckPackageConsumption();
            }

            if ((chkFreezBill.IsChecked == true && isValid != false) || (SelectedBill != null && SelectedBill.IsFreezed == true && isValid != false))     // For Package New Changes Added on 20062018
            {
                var items12 = from r in ChargeList
                              where r.isPackageService == true
                              select r;

                if (items12.ToList().Count > 0)
                {
                    if (IsFromViewClick == true)
                    {
                        if (SelectedBill != null && SelectedBill.IsAdjustableBillDone == false)
                        {
                            isValid = false;
                            string msgText = "You Can Not Freeze Package Service Bill Without Adjustable Service Bill. ";
                            MessageBoxControl.MessageBoxChildWindow msgWD1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgWD1.Show();
                            //break;
                        }
                        //else if (SelectedBill != null && SelectedBill.IsAllBillSettle == false)
                        //{
                        //    isValid = false;
                        //    string msgText = "You Can Not Freeze Package Service Bill Without Settle Package Under Consume Bills. ";
                        //    MessageBoxControl.MessageBoxChildWindow msgWD2 =
                        //        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        //    msgWD2.Show();
                        //    //break;
                        //}
                    }
                }
            }

            // added on 13082018 for validation so user can not Freeze Package Service bill while save.
            if (IsFromSaveAndPackageBill == true && chkFreezBill.IsChecked == true && IsEditMode == false && IsFromViewClick == false)
            {
                var items15 = from r in ChargeList
                              where r.isPackageService == true
                              select r;

                if (items15.ToList().Count > 0)
                {
                    isValid = false;
                    string msgText2 = "You Can Not Freeze Package Service Bill Without avail Package Under Services.";
                    MessageBoxControl.MessageBoxChildWindow msgWD12 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText2, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgWD12.Show();
                }
            }

            return isValid;
        }
        public bool _IsConsumption = false;
        public string msgConsumption = "";
        public bool CheckPackageConsumption()
        {
            var items1 = from r in ChargeList
                         where r.IsConsumption == true
                         select r;
            if (items1.ToList().Count > 0)
            {
                //msgConsumption = "Consumption Pending for this package ";
            }
            return true;
        }

        void Msgbox_OnMessageBoxClosedForConsumption(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                _IsConsumption = true;
            }
            else
            {
                _IsConsumption = false;
            }
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
                msgConsumption = string.Empty;
                if (ChargeList.Select(z => z.PackageID).FirstOrDefault() > 0) //&& !ChargeList.Select(z => z.isPackageService).FirstOrDefault()
                {
                    clsAddBillBizActionVO BizAction = new clsAddBillBizActionVO();
                    BizAction.IsForPackageAdvanceCheck = true;
                    if (BizAction.Details == null) BizAction.Details = new clsBillVO();
                    BizAction.Details.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    BizAction.Details.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
                    BizAction.Details.TotalBillAmount = Convert.ToDouble(txtClinicalTotal.Text);

                    if (ChargeList != null && ChargeList.Count > 0)
                    {
                        BizAction.Details.PackageID = ChargeList.Select(z => z.PackageID).FirstOrDefault();
                        BizAction.Details.ParentID = ChargeList.Select(z => z.ParentID).FirstOrDefault();
                        BizAction.Details.BillID = ChargeList.Select(z => z.PackageBillID).FirstOrDefault();
                    }

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            if (((clsAddBillBizActionVO)arg.Result).SuccessStatus == 1 && !ChargeList.Select(z => z.isPackageService).FirstOrDefault())
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Insufficient Package Advance Balance!.. Remaining Package Advance is Rs. " + ((clsAddBillBizActionVO)arg.Result).BalancePackageAdvance, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                msgW1.Show();
                                cmdSave.IsEnabled = true;
                            }
                            else
                            {
                                if (((clsAddBillBizActionVO)arg.Result).ServiceConsumptionAmount == 0 && ((clsAddBillBizActionVO)arg.Result).PharmacyConsumptionAmount == 0 && ChargeList.Select(z => z.isPackageService).FirstOrDefault())
                                {
                                    msgConsumption = "Consumption Pending for this package,";
                                }
                                else if (((clsAddBillBizActionVO)arg.Result).ServiceConsumptionAmount == 0 && ChargeList.Select(z => z.isPackageService).FirstOrDefault())
                                {
                                    msgConsumption = "Service Consumption Pending for this package,";
                                }
                                else if (((clsAddBillBizActionVO)arg.Result).PharmacyConsumptionAmount == 0 && ChargeList.Select(z => z.isPackageService).FirstOrDefault())
                                {
                                    msgConsumption = "Pharmacy Consumption Pending for this package,";
                                }
                                else
                                    msgConsumption = "";

                                #region SaveBill
                                if (chkFreezBill.IsChecked == true)
                                {
                                    isValid = false;
                                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForConcessionID > 0)
                                    {
                                        if ((objUser.OpdBillingAmmount == 0 ? false : objUser.OpdBillingAmmount < Convert.ToDecimal(ApprovalConcenssion)) || (objUser.OpdBillingPercentage == 0 ? false : (((Convert.ToDecimal(ApprovalConcenssion) / Convert.ToDecimal(ApprovalTotal)) * 100)) > objUser.OpdBillingPercentage))//objUser.OpdBillingAmmount < Convert.ToDecimal(txtClinicalConcession.Text)
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Concession Exceeds The Authorised Amount,Are You Sure You Want To Send For Approval?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                            msgWD_1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWD_1_OnMessageBoxClosed);
                                            msgWD_1.Show();
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
                                //}
                                #endregion //SaveBill
                            }
                        }
                    };
                    Client.ProcessAsync(BizAction, null);
                    Client.CloseAsync();
                }
                else
                {
                    #region SaveBill
                    if (chkFreezBill.IsChecked == true)
                    {
                        isValid = false;
                        if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForConcessionID > 0)
                        {
                            if ((objUser.OpdBillingAmmount == 0 ? false : objUser.OpdBillingAmmount < Convert.ToDecimal(ApprovalConcenssion)) || (objUser.OpdBillingPercentage == 0 ? false : (((Convert.ToDecimal(ApprovalConcenssion) / Convert.ToDecimal(ApprovalTotal)) * 100)) > objUser.OpdBillingPercentage))//objUser.OpdBillingAmmount < Convert.ToDecimal(txtClinicalConcession.Text)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Concession Exceeds The Authorised Amount,Are You Sure You Want To Send For Approval?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                msgWD_1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWD_1_OnMessageBoxClosed);
                                msgWD_1.Show();
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
                    //}
                    #endregion //SaveBill
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
                // cmdSave.IsEnabled = true;
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

                    objBill.Opd_Ipd_External_Id = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;
                    objBill.Opd_Ipd_External_UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;

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
                    ////objBill.PatientCategoryId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID;
                    objBill.PatientCategoryId = ((IApplicationConfiguration)App.Current).SelectedPatient.NewPatientCategoryID;
                }


                if (PatientCompanyID > 0)
                {
                    objBill.CompanyId = Convert.ToInt64(PatientCompanyID);
                }
                else
                {
                    ////objBill.PatientCategoryId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID;
                    objBill.CompanyId = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
                }

                objBill.IsFreezed = pFreezBill;
                objBill.PaymentDetails = pPayDetails;

                // This IS valid In Health Spring So Commented 
                //if (!string.IsNullOrEmpty(txtGrossDiscount.Text))
                //    objBill.GrossDiscountPercentage = Convert.ToDouble(txtGrossDiscount.Text);

                //if (cmbGrossDiscountReason.SelectedItem != null)
                //    objBill.GrossDiscountReasonID = ((MasterListItem)cmbGrossDiscountReason.SelectedItem).ID;


                //if (!string.IsNullOrEmpty(txtTotalBill.Text))
                //    objBill.TotalBillAmount = Convert.ToDouble(txtTotalBill.Text);

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

                if (ChargeList != null & ChargeList.Count > 0)
                {
                    objBill.ChargeDetails = ChargeList.ToList();

                    if (pFreezBill)
                    {
                        for (int i = 0; i < ChargeList.Count; i++)
                        {
                            clsChargeVO objCharge = new clsChargeVO();
                            objCharge = ChargeList[i];
                            // Added By CDS
                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                                objCharge.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                            else
                                objCharge.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;
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

                            //foreach (var item in FollowUpDetails)
                            //{
                            //    if (item.ServiceId == objCharge.ServiceId)
                            //    {
                            //        if (FollowUpDetails.Count > 0)
                            //        {
                            //            objBill.FollowUpDetails.Add(item);
                            //        }
                            //    }
                            //}
                        }
                    }
                    else
                    {
                        for (int i = 0; i < ChargeList.Count; i++)
                        {
                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                                ChargeList[i].CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                            else
                                ChargeList[i].CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;
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
                        // Added By CDS
                        objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                    }
                    else if (objBill.ChargeDetails != null && objBill.ChargeDetails.Count > 0)
                    {
                        // Added By CDS
                        objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                    }
                    else if (objBill.PharmacyItems != null && objBill.PharmacyItems.Items != null && objBill.PharmacyItems.Items.Count > 0)
                    {
                        // Added By CDS
                        objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                    }
                }
                else
                {
                    objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;
                }

                if (objBill.PaymentDetails != null)
                {
                    //objBill.PaymentDetails.CostingDivisionID = objBill.CostingDivisionID;
                    // Added By CDS
                    objBill.PaymentDetails.CostingDivisionID = objBill.CostingDivisionID;
                }
                clsEmailSMSSentListVO objEmailTemp = new clsEmailSMSSentListVO(); //***//
                clsSendRequestForApprovalVO obj = new clsSendRequestForApprovalVO();
                obj.BillDetails = new clsAddBillBizActionVO();
                obj.Details = new clsRefundVO();
                obj.ChargeList = new List<clsChargeVO>();
                //obj.EmailSMSDetail = objEmailTemp;//***//
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
                obj.Details.IsOPDBill = true;
                obj.Details.RequestTypeID = (int)RequestType.Concession;
                obj.Details.RequestType = Enum.GetName(typeof(RequestType), RequestType.Concession);
                obj.ChargeList = ChargeList.ToList().DeepCopy();
                objBill.ConcessionRemark = Convert.ToString(txtApprovalRemark.Text.Trim()); //Added By Bhushanp 09032017
                obj.BillDetails.Details = new clsBillVO();
                obj.BillDetails.Details = objBill;

                //***//-----
                //obj.EmailSMSDetail.PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                //obj.EmailSMSDetail.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                //obj.EmailSMSDetail.TemplateID = 1;
                //obj.EmailSMSDetail.PatientMobileNo = ((IApplicationConfiguration)App.Current).SelectedPatient.ContactNO1;
                //obj.EmailSMSDetail.PatientEmailId = ((IApplicationConfiguration)App.Current).SelectedPatient.Email;
                //obj.EmailSMSDetail.LocalText ="Hi";
                //obj.EmailSMSDetail.FailureReason ="";
                //obj.EmailSMSDetail.EmailSubject = "RequestForApproval";
                //obj.EmailSMSDetail.EmailAttachment = "";
                //obj.EmailSMSDetail.Email_SMS = true;
                //obj.EmailSMSDetail.EmailText = "Approval";
                //obj.EmailSMSDetail.EnglishText = "";
                //obj.EmailSMSDetail.SuccessStatus = true;
                //***--------------------------------

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        Indicatior.Close();

                        SetCommandButtonState("Save");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Request Sent Successfully for Approval ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();

                        _flip.Invoke(RotationType.Backward);
                        FillBillSearchList();
                    }
                    else
                    {
                        Indicatior.Close();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("NT2", "Error occured while adding Bill details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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
            //* Commented by - Ajit Jadhav
            //* Added Date - 26/8/2016
            //* Comments - get pan number
            PanNumber = ((IApplicationConfiguration)App.Current).SelectedPatient.PanNumber;
            //***//--------------------

            //if ((((IApplicationConfiguration)App.Current).SelectedPatient.RelationID == 15) && (((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 8 || ((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 9))
            //{
            //    DonorCoupleLinkedList Win = new DonorCoupleLinkedList();
            //    //Win.DonorID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            //    Win.DonorID = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
            //    Win.DonorUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
            //    Win.DonorName = ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName + " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " + ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;

            // Win.OnSaveButton_Click += new RoutedEventHandler(DonorLinWin_OnSaveButton_Click);
            //    Win.OnCancelButton_Click += new RoutedEventHandler(DonorLinWin_OnCancelButton_Click);
            //    Win.Show();
            //}
            //else
            //{
            string msgText = string.Empty;
            if (PatBalAdvaceAmt < double.Parse(txtPayAmount.Text)) msgText = "You do not have sufficient Advance, Are you sure you want to Freeze the Bill?";
            else msgText = "Are you sure you want to Freeze the Bill ?";
            MessageBoxControl.MessageBoxChildWindow msgWD =
            new MessageBoxControl.MessageBoxChildWindow("Palash", msgConsumption + " Are you sure you want to Freeze the Bill ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgWD.OnMessageBoxClosed += (arg) =>
            {
                if (arg == MessageBoxResult.Yes)
                {
                    PaymentWindow paymentWin = new PaymentWindow();
                    paymentWin.TotalAmount = double.Parse(txtPayAmount.Text);
                    paymentWin.Initiate("Bill");
                    //paymentWin.ConcessionFromPlan = ConcessionFromPlan;
                    paymentWin.txtPayTotalAmount.Text = this.txtClinicalTotal.Text;
                    paymentWin.txtDiscAmt.Text = this.txtTotalConcession.Text;
                    paymentWin.txtPayableAmt.Text = this.txtPayAmount.Text;
                    paymentWin.PackageConcenssionAmt = PackageConcenssion;
                    //paymentWin.PatientCategoryID = PatientCategoryID;
                    if (PatientCategoryID > 0)
                    {
                        paymentWin.PatientCategoryID = Convert.ToInt64(PatientCategoryID);
                    }
                    else if (((IApplicationConfiguration)App.Current).SelectedPatient.NewPatientCategoryID != null)
                    {
                        paymentWin.PatientCategoryID = ((IApplicationConfiguration)App.Current).SelectedPatient.NewPatientCategoryID;
                    }


                    //* Added by - Ajit Jadhav
                    //* Added Date - 26/8/2016
                    //* Comments - mandatorily capture Pan No 
                    if (!String.IsNullOrEmpty(PanNumber) && PanNumber != "NULL")
                    {
                        paymentWin.txtPanNo.Text = PanNumber;
                        paymentWin.txtPanNo.IsEnabled = false;
                    }
                    if (LinkPatientID > 0)
                    {
                        paymentWin.LinkPatientID = LinkPatientID;
                        paymentWin.LinkPatientUnitID = LinkPatientUnitID;
                        paymentWin.LinkCompanyID = LinkCompanyID;
                    }

                    //***//-------------------------
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

                        paymentWin.IsAdjustableHead = ChargeList.Select(z => z.IsAdjustableHead).FirstOrDefault(); //***//
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

                    paymentWin.OnSaveButton_Click += new RoutedEventHandler(paymentWin_OnSaveButton_Click);
                    paymentWin.OnCancelButton_Click += new RoutedEventHandler(paymentWin_OnCancelButton_Click);
                    paymentWin.Show();
                }
                else
                    cmdSave.IsEnabled = true;
            };
            msgWD.Show();
            // }
        }


        //***//------------------------------------------------------
        public void DonorLinWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {

            ServiceSearchForPackage serviceSearch = null;
            serviceSearch = new ServiceSearchForPackage();

            if (((DonorCoupleLinkedList)sender).DialogResult == true && ((DonorCoupleLinkedList)sender).DonorLink.IsAgainstDonor == true)
            {
                serviceSearch.VisitID = VisitID;
                serviceSearch.LinkPatientID = ((DonorCoupleLinkedList)sender).DonorLink.PatientID;
                serviceSearch.LinkPatientUnitID = ((DonorCoupleLinkedList)sender).DonorLink.PatientUnitID;
                serviceSearch.LinkCompanyID = ((DonorCoupleLinkedList)sender).DonorLink.CompanyID;
                serviceSearch.LinkTariffID = ((DonorCoupleLinkedList)sender).DonorLink.TariffID;
                serviceSearch.LinkPatientSourceID = ((DonorCoupleLinkedList)sender).DonorLink.PatientSourceID;

                AgainstDonor = ((DonorCoupleLinkedList)sender).DonorLink.IsAgainstDonor;
                LinkPatientID = ((DonorCoupleLinkedList)sender).DonorLink.PatientID;
                LinkPatientUnitID = ((DonorCoupleLinkedList)sender).DonorLink.PatientUnitID;
                LinkCompanyID = ((DonorCoupleLinkedList)sender).DonorLink.CompanyID;

                serviceSearch.IsFromSaveAndPackageBill = IsFromSaveAndPackageBill;
                serviceSearch.OnAddButton_Click += new RoutedEventHandler(serviceSearch_OnAddButton_Click);
                serviceSearch.Show();
            }
            else
            {
                serviceSearch.VisitID = VisitID;
                serviceSearch.IsFromSaveAndPackageBill = IsFromSaveAndPackageBill;
                serviceSearch.OnAddButton_Click += new RoutedEventHandler(serviceSearch_OnAddButton_Click);
                serviceSearch.Show();
            }



            //if (((DonorCoupleLinkedList)sender).DialogResult == true)
            //{
            //    AgainstDonor = ((DonorCoupleLinkedList)sender).DonorLink.IsAgainstDonor;
            //    LinkPatientID = ((DonorCoupleLinkedList)sender).DonorLink.PatientID;
            //    LinkPatientUnitID = ((DonorCoupleLinkedList)sender).DonorLink.PatientUnitID;
            //    LinkCompanyID = ((DonorCoupleLinkedList)sender).DonorLink.CompanyID;
            //}



            //double _PackagePaidAmount = 0;

            //if ((from S in ChargeList where S.PackageID > 0 select S).ToList().Count > 0)
            //{
            //    var distinctList = (ChargeList.GroupBy(x => x.ParentID)
            //                 .Select(g => g.First())
            //                 .ToList()).Sum(x => x.PackagePaidAmount).ToString();

            //    _PackagePaidAmount = Convert.ToDouble(distinctList.ToString());
            //}

            //string msgText = string.Empty;

            //if ((PatBalAdvaceAmt + _PackagePaidAmount < Convert.ToDouble(txtClinicalTotal.Text.Trim())) && ((from S in ChargeList where S.PackageID > 0 && S.isPackageService == false select S).ToList().Count > 0))
            //{
            //    msgText = "Patient Account Shows insufficient Package Balance, Are you sure you want to Freeze the Bill?";
            //}
            //else if (PatBalAdvaceAmt < double.Parse(txtPayAmount.Text))
            //{
            //    msgText = "You do not have sufficient Advance, Are you sure you want to Freeze the Bill?";
            //}
            //else
            //{
            //    msgText = "Are you sure you want to Freeze the Bill ?";
            //}
            //MessageBoxControl.MessageBoxChildWindow msgWD =
            //new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            //msgWD.OnMessageBoxClosed += (arg) =>
            //{
            //    if (arg == MessageBoxResult.Yes)
            //    {
            //        PaymentWindow paymentWin = new PaymentWindow();
            //        paymentWin.TotalAmount = double.Parse(txtPayAmount.Text);
            //        paymentWin.Initiate("Bill");
            //        paymentWin.txtPayTotalAmount.Text = this.txtClinicalTotal.Text;
            //        paymentWin.txtDiscAmt.Text = this.txtTotalConcession.Text;
            //        paymentWin.txtPayableAmt.Text = this.txtPayAmount.Text;
            //        paymentWin.PackageConcenssionAmt = PackageConcenssion;

            //        paymentWin.LinkPatientID = LinkPatientID;
            //        paymentWin.LinkPatientUnitID = LinkPatientUnitID;
            //        paymentWin.LinkCompanyID = LinkCompanyID;


            //        if (PatientCategoryID > 0)
            //        {
            //            paymentWin.PatientCategoryID = Convert.ToInt64(PatientCategoryID);
            //        }
            //        else if (((IApplicationConfiguration)App.Current).SelectedPatient.NewPatientCategoryID != null)
            //        {
            //            paymentWin.PatientCategoryID = ((IApplicationConfiguration)App.Current).SelectedPatient.NewPatientCategoryID;
            //        }

            //        if (!String.IsNullOrEmpty(PanNumber) && PanNumber != "NULL")
            //        {
            //            paymentWin.txtPanNo.Text = PanNumber;
            //            paymentWin.txtPanNo.IsEnabled = false;
            //        }

            //        if ((Convert.ToDouble(this.txtTotalConcession.Text) - PackageConcenssion) > 0.0)
            //        {
            //            paymentWin.ConcessionFromPlan = false;
            //        }
            //        else
            //        {
            //            paymentWin.ConcessionFromPlan = true;
            //        }
            //        if (rdbAgainstBill.IsChecked == true)
            //            paymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstBill;
            //        else
            //            paymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstServices;
            //        paymentWin.OnSaveButton_Click += new RoutedEventHandler(paymentWin_OnSaveButton_Click);
            //        paymentWin.OnCancelButton_Click += new RoutedEventHandler(paymentWin_OnCancelButton_Click);
            //        paymentWin.Show();
            //    }
            //    else
            //        cmdSave.IsEnabled = true;
            //};
            //msgWD.Show();

        }


        void DonorLinWin_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedBill != null)
                cmdSave.IsEnabled = false;
            else
                cmdSave.IsEnabled = true;
        }


        public void ModifyDonorLinWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (((DonorCoupleLinkedList)sender).DialogResult == true)
            {
                AgainstDonor = ((DonorCoupleLinkedList)sender).DonorLink.IsAgainstDonor;
                LinkPatientID = ((DonorCoupleLinkedList)sender).DonorLink.PatientID;
                LinkPatientUnitID = ((DonorCoupleLinkedList)sender).DonorLink.PatientUnitID;
                LinkCompanyID = ((DonorCoupleLinkedList)sender).DonorLink.CompanyID;
            }

            string msgText = "Are You Sure \n  You Want To Freeze The Bill ?";
            MessageBoxControl.MessageBoxChildWindow msgWD =
                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

            msgWD.OnMessageBoxClosed += (arg) =>
            {
                if (arg == MessageBoxResult.Yes)
                {
                    PaymentWindow paymentWin = new PaymentWindow();
                    paymentWin.PatientCategoryID = ((clsBillVO)dgBillList.SelectedItem).PatientCategoryId; //this.PatientCategoryID;  //Ashish Z
                    paymentWin.TotalAmount = double.Parse(txtPayAmount.Text);
                    paymentWin.Initiate("Bill");
                    //By Parmeshwar.........................
                    paymentWin.conID = ((clsBillVO)dgBillList.SelectedItem).ConcessionAuthorizedBy;
                    //...............................................
                    paymentWin.txtPayTotalAmount.Text = this.txtNetAmount.Text;
                    paymentWin.txtDiscAmt.Text = this.txtTotalConcession.Text;
                    paymentWin.txtPayableAmt.Text = this.txtPayAmount.Text;
                    paymentWin.PackageConcenssionAmt = PackageConcenssion;


                    paymentWin.LinkPatientID = LinkPatientID;
                    paymentWin.LinkPatientUnitID = LinkPatientUnitID;
                    paymentWin.LinkCompanyID = LinkCompanyID;

                    if (!String.IsNullOrEmpty(PanNumber) && PanNumber != "NULL")
                    {
                        paymentWin.txtPanNo.Text = PanNumber;
                        paymentWin.txtPanNo.IsEnabled = false;
                    }

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
                    paymentWin.OnSaveButton_Click += new RoutedEventHandler(paymentWin_OnSaveButton_Click);
                    paymentWin.Show();
                }
            };
            msgWD.Show();
        }
        //-----------------------------------------------------------

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
            if (chkFreezBill.IsChecked == true)
            {

                var results2 = from r in ChargeList
                               orderby r.NetAmount ascending
                               select r;

                ChargeList = new ObservableCollection<clsChargeVO>();
                foreach (var item in results2.ToList())
                {
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

                            //item.ServicePaidAmount = ConsumeAmt;              // Commented on 26082018
                            item.ServicePaidAmount = Math.Round(ConsumeAmt, 2); // modify on 26082018 for balance amount issue while pay the bill
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

                                //Obj.ServicePaidAmount = ConsumeAmt;                // Commented on 26082018
                                Obj.ServicePaidAmount = Math.Round(ConsumeAmt, 2);   // modify on 26082018 for balance amount issue while pay the bill
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

            StringBuilder StrPrescriptionDetailsID = new StringBuilder();
            StringBuilder StrInvestigationDetailsID = new StringBuilder();

            foreach (var item in ChargeList)
            {
                TotCon = TotCon + item.Concession;
                TotAmt = TotAmt + item.NetAmount;

                if (StrPrescriptionDetailsID.ToString().Length > 0)
                    StrPrescriptionDetailsID.Append(",");
                StrPrescriptionDetailsID.Append(item.PrescriptionDetailsID);

                if (StrInvestigationDetailsID.ToString().Length > 0)
                    StrInvestigationDetailsID.Append(",");
                StrInvestigationDetailsID.Append(item.InvestigationDetailsID);
            }


            try
            {
                clsBillVO objBill = new clsBillVO();

                if (SelectedBill == null)
                {
                    objBill.Date = DateTime.Now;
                    objBill.Time = DateTime.Now;
                    IsEditMode = false;

                    objBill.Opd_Ipd_External_Id = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;
                    objBill.Opd_Ipd_External_UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;

                    objBill.CompanyId = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;

                    objBill.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    objBill.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;

                    objBill.AgainstDonor = AgainstDonor;
                    objBill.LinkPatientID = LinkPatientID;
                    objBill.LinkPatientUnitID = LinkPatientUnitID;
                }
                else
                {
                    objBill = SelectedBill.DeepCopy();
                    IsEditMode = true;

                    objBill.AgainstDonor = AgainstDonor;
                    objBill.LinkPatientID = LinkPatientID;
                    objBill.LinkPatientUnitID = LinkPatientUnitID;
                }

                if (pFreezBill)
                {
                    objBill.Date = DateTime.Now;
                    objBill.Time = DateTime.Now;
                    if (pPayDetails != null)
                    {
                        //objBill.BalanceAmountSelf = TotAmt - pPayDetails.PaidAmount;      // Commented on 01082018
                        objBill.BalanceAmountSelf = Math.Round(TotAmt, 2) - pPayDetails.PaidAmount;        // added on 01082018
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
                    ////objBill.PatientCategoryId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID;
                    objBill.PatientCategoryId = ((IApplicationConfiguration)App.Current).SelectedPatient.NewPatientCategoryID;
                }


                if (PatientCompanyID > 0)
                {
                    objBill.CompanyId = Convert.ToInt64(PatientCompanyID);
                }
                else
                {
                    ////objBill.PatientCategoryId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID;
                    objBill.CompanyId = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
                }

                objBill.IsFreezed = pFreezBill;
                objBill.PaymentDetails = pPayDetails;


                // This IS valid In Health Spring So Commented 
                //if (!string.IsNullOrEmpty(txtGrossDiscount.Text))
                //    objBill.GrossDiscountPercentage = Convert.ToDouble(txtGrossDiscount.Text);

                //if (cmbGrossDiscountReason.SelectedItem != null)
                //    objBill.GrossDiscountReasonID = ((MasterListItem)cmbGrossDiscountReason.SelectedItem).ID;


                //if (!string.IsNullOrEmpty(txtTotalBill.Text))
                //    objBill.TotalBillAmount = Convert.ToDouble(txtTotalBill.Text);

                if (!string.IsNullOrEmpty(txtTotalBill.Text))
                    objBill.TotalBillAmount = Convert.ToDouble(txtTotalBill.Text);

                if (!string.IsNullOrEmpty(txtTotalConcession.Text))
                    objBill.TotalConcessionAmount = Convert.ToDouble(txtTotalConcession.Text);
                TotalConcession = objBill.TotalConcessionAmount;

                if (!string.IsNullOrEmpty(txtNetAmount.Text))
                    objBill.NetBillAmount = Convert.ToDouble(txtNetAmount.Text);

                // objBill.ConcessionReasonId=cmbConcessionReason.se
                //Added by Yogesh K ConcessionReasonId 20 Apr 2016
                if (cmbConcessionReason.SelectedItem != null)
                    //  objBill.ConcessionReasonId = ;
                    objBill.ConcessionReasonId = ((MasterListItem)cmbConcessionReason.SelectedItem).ID;


                objBill.TotalConcessionAmount = TotCon;

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
                            // Added By CDS
                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                                objCharge.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                            else
                                objCharge.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;

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
                                //Added By Yogesh K 24 5 16
                                objBill.RadiologyWorkOrder.OrderBookingDetails.Add(new clsRadOrderBookingDetailsVO()
                                {
                                    TariffServiceID = objCharge.TariffServiceId,
                                    ServiceID = objCharge.ServiceId,
                                    ID = objCharge.ROBDID,
                                    DoctorID = objCharge.SelectedDoctor.ID,
                                    IsApproved = false
                                });

                                // BizAction.objIndent.DeletedIndentDetailsList = (List<clsIndentDetailVO>)DeleteIndentAddedItems.ToList<clsIndentDetailVO>();
                            }

                            //foreach (var item in FollowUpDetails)
                            //{
                            //    if (item.ServiceId == objCharge.ServiceId)
                            //    {
                            //        if (FollowUpDetails.Count > 0)
                            //        {
                            //            objBill.FollowUpDetails.Add(item);
                            //        }
                            //    }
                            //}
                        }
                    }
                    else
                    {
                        for (int i = 0; i < ChargeList.Count; i++)
                        {
                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                                ChargeList[i].CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                            else
                                ChargeList[i].CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;
                        }

                    }

                    if (IsAuditTrail)
                    {
                        LogInformation = new LogInfo();   // for T_charge
                        LogInformation.guid = objGUID;
                        LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        LogInformation.TimeStamp = DateTime.Now;
                        LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                        LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                        LogInformation.Message = " 5 : T_charge " //+ Convert.ToString(lineNumber)
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
                        // Added By CDS
                        objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                    }
                    else if (objBill.ChargeDetails != null && objBill.ChargeDetails.Count > 0)
                    {
                        // Added By CDS
                        objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                    }
                    else if (objBill.PharmacyItems != null && objBill.PharmacyItems.Items != null && objBill.PharmacyItems.Items.Count > 0)
                    {
                        // Added By CDS
                        objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                    }
                }
                else
                {
                    objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;
                }

                if (objBill.PaymentDetails != null)
                {
                    //objBill.PaymentDetails.CostingDivisionID = objBill.CostingDivisionID;
                    // Added By CDS
                    objBill.PaymentDetails.CostingDivisionID = objBill.CostingDivisionID;
                }

                clsAddBillBizActionVO BizAction = new clsAddBillBizActionVO();

                #region For Package Advance & Bill Save in transaction : added on 16082018

                if (IsFromSaveAndPackageBill == true && this.AdvanceBizActionVO != null)
                {
                    BizAction.objPackageAdvanceVODetails = this.AdvanceBizActionVO;
                    BizAction.IsFromSaveAndPackageBill = this.IsFromSaveAndPackageBill;
                }

                #endregion

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
                objBill.ConcessionRemark = Convert.ToString(txtApprovalRemark.Text.Trim()); //Added By Bhushanp 09032017

                BizAction.Details = new clsBillVO();

                BizAction.PrescriptionDetailsID = StrPrescriptionDetailsID.ToString();
                BizAction.InvestigationDetailsID = StrInvestigationDetailsID.ToString();

                BizAction.Details = objBill;
                BizAction.DeletedRadSerDetailsList = (List<clsChargeVO>)DeleteBillAddedItems.ToList<clsChargeVO>();
                //  BizAction.objIndent.DeletedIndentDetailsList = (List<clsIndentDetailVO>)DeleteIndentAddedItems.ToList<clsIndentDetailVO>();
                BizAction.LogInfoList = new List<LogInfo>();  // For the Activity Log List
                BizAction.LogInfoList = LogInfoList.DeepCopy();

                BizAction.Details.AdvanceID = this.AdvanceID;
                BizAction.Details.AdvanceUnitID = this.AdvanceUnitID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((clsAddBillBizActionVO)arg.Result).Details != null)
                    {

                        if (((clsAddBillBizActionVO)arg.Result).Details != null)
                        {

                            BillID = (((clsAddBillBizActionVO)arg.Result).Details).ID;
                            UnitID = (((clsAddBillBizActionVO)arg.Result).Details).UnitID;
                            PRID = ((MasterListItem)cmbPrintFormat.SelectedItem).ID;
                            Indicatior.Close();
                            SetCommandButtonState("Save");
                            MessageBoxControl.MessageBoxChildWindow msgW1 = null;
                            if (IsFromSaveAndPackageBill)
                            {
                                msgW1 = new MessageBoxControl.MessageBoxChildWindow("Palash", " Advance details and Package saved successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            }
                            else
                            {
                                if (((clsAddBillBizActionVO)arg.Result).Details.IsFreezed)
                                    msgW1 = new MessageBoxControl.MessageBoxChildWindow("Palash", "Bill details saved successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                else
                                    msgW1 = new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient bill is saved as Credit.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            }

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
                                            PrintBill(BillID, UnitID, PRID);
                                        }
                                        else if ((((clsAddBillBizActionVO)arg.Result).Details).BillType == PalashDynamics.ValueObjects.BillTypes.Pharmacy)
                                        {
                                            if ((((clsAddBillBizActionVO)arg.Result).Details).BalanceAmountNonSelf == 0 && (((clsAddBillBizActionVO)arg.Result).Details).BalanceAmountSelf == 0)
                                                PrintPharmacyBill(BillID, UnitID, PRID);
                                        }
                                        else
                                        {
                                            if ((((clsAddBillBizActionVO)arg.Result).Details).BalanceAmountNonSelf == 0 && (((clsAddBillBizActionVO)arg.Result).Details).BalanceAmountSelf == 0)
                                            {
                                                PrintBill(BillID, UnitID, PRID);
                                                PrintPharmacyBill(BillID, UnitID, PRID);
                                            }
                                        }
                                    }
                                    //if (IsFromSaveAndPackageBill)     // Commented on 20082018
                                    if (IsFromSaveAndPackageBill && this.AdvanceBizActionVO != null)    //modified on 20082018 For Package Advance & Bill Save in transaction
                                    {
                                        // Commented on 20082018
                                        //PrintAdvance(((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID, AdvanceID, AdvanceUnitID, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID);

                                        //modified on 20082018 For Package Advance & Bill Save in transaction
                                        PrintAdvance(((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID, (((clsAddBillBizActionVO)arg.Result).Details).AdvanceID, (((clsAddBillBizActionVO)arg.Result).Details).AdvanceUnitID, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID);
                                    }
                                }
                            };

                            msgW1.Show();
                            _flip.Invoke(RotationType.Backward);
                            FillBillSearchList();
                        }
                    }
                    else
                    {
                        Indicatior.Close();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("NT1", "Error occured while adding Bill details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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

            #region OLD Code Commented By CDS
            //Indicatior = new WaitIndicator();
            //Indicatior.Show();
            //try
            //{
            //    clsBillVO objBill = new clsBillVO();
            //    // if (IsEditMode==true)  objBill = SelectedBill;
            //    if (SelectedBill == null)
            //    {
            //        objBill.Date = DateTime.Now;
            //        objBill.Time = DateTime.Now;
            //        IsEditMode = false;
            //        objBill.Opd_Ipd_External_Id = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;
            //        objBill.Opd_Ipd_External_UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            //    }
            //    else
            //    {
            //        objBill = SelectedBill.DeepCopy();
            //        IsEditMode = true;
            //    }

            //    if (pFreezBill)
            //    {
            //        objBill.Date = DateTime.Now;
            //        objBill.Time = DateTime.Now;
            //        if (pPayDetails != null)
            //        {
            //            objBill.BalanceAmountSelf = pPayDetails.BillBalanceAmount;
            //            objBill.BillPaymentType = pPayDetails.BillPaymentType;
            //        }

            //        if (!string.IsNullOrEmpty(txtPayAmount.Text))
            //            objBill.SelfAmount = Convert.ToDouble(txtPayAmount.Text);
            //    }

            //    objBill.PatientSourceId = PatientSourceID;
            //    objBill.TariffId = PatientTariffID;

            //    objBill.IsFreezed = pFreezBill;
            //    objBill.PaymentDetails = pPayDetails;

            //    if (!string.IsNullOrEmpty(txtTotalBill.Text))
            //        objBill.TotalBillAmount = Convert.ToDouble(txtTotalBill.Text);

            //    if (!string.IsNullOrEmpty(txtTotalConcession.Text))
            //        objBill.TotalConcessionAmount = Convert.ToDouble(txtTotalConcession.Text);
            //    TotalConcession = objBill.TotalConcessionAmount;

            //    if (!string.IsNullOrEmpty(txtNetAmount.Text))
            //        objBill.NetBillAmount = Convert.ToDouble(txtNetAmount.Text);



            //    if (ChargeList != null & ChargeList.Count > 0)
            //    {
            //        objBill.ChargeDetails = ChargeList.ToList();

            //        if (pFreezBill)
            //        {
            //            for (int i = 0; i < ChargeList.Count; i++)
            //            {
            //                clsChargeVO objCharge = new clsChargeVO();
            //                objCharge = ChargeList[i];

            //                // Added By CDS
            //                objCharge.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;

            //                if (objCharge.ServiceSpecilizationID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathoSpecializationID)
            //                {
            //                    objBill.PathoWorkOrder = new clsPathOrderBookingVO();
            //                    //objBill.PathoWorkOrder.TariffServiceID = objCharge.TariffServiceId;
            //                    objBill.PathoWorkOrder.DoctorID = VisitDoctorID;
            //                    //objBill.PathoWorkOrder.ServiceID = objCharge.ServiceId;
            //                    objBill.PathoWorkOrder.PathoSpecilizationID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathoSpecializationID;
            //                    objBill.PathoWorkOrder.Items = new List<clsPathOrderBookingDetailVO>();
            //                    //objBill.PathoWorkOrder.Items.Add(new clsPathOrderBookingDetailVO()
            //                    //{
            //                    //    TariffServiceID = objCharge.TariffServiceId,
            //                    //    ServiceID = objCharge.ServiceId,
            //                    //    //DoctorID = VisitDoctorID,
            //                    //    TestCharges = objCharge.Rate,
            //                    //    IsSampleCollected = false
            //                    //});
            //                }
            //                else if (objCharge.ServiceSpecilizationID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RadiologySpecializationID)
            //                {

            //                    objBill.RadiologyWorkOrder.OrderBookingDetails.Add(new clsRadOrderBookingDetailsVO()
            //                    {
            //                        TariffServiceID = objCharge.TariffServiceId,
            //                        ServiceID = objCharge.ServiceId,

            //                        DoctorID = VisitDoctorID,
            //                        IsApproved = false
            //                    });
            //                }
            //            }
            //        }
            //    }

            //    if (PharmacyItems.Count > 0)
            //    {
            //        objBill.PharmacyItems.Items = new List<clsItemSalesDetailsVO>();
            //        objBill.PharmacyItems.VisitID = objBill.Opd_Ipd_External_Id;
            //        objBill.PharmacyItems.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            //        objBill.PharmacyItems.PatientUnitID = objBill.Opd_Ipd_External_UnitId;
            //        objBill.PharmacyItems.Date = objBill.Date;
            //        objBill.PharmacyItems.Time = objBill.Time;
            //        objBill.PharmacyItems.StoreID = StoreID;

            //        if (!string.IsNullOrEmpty(txtPharmacyTotal.Text))
            //            objBill.PharmacyItems.TotalAmount = Convert.ToDouble(txtPharmacyTotal.Text);

            //        if (!string.IsNullOrEmpty(txtPharmacyConcession.Text))
            //            objBill.PharmacyItems.ConcessionAmount = Convert.ToDouble(txtPharmacyConcession.Text);

            //        if (!string.IsNullOrEmpty(txtPharmacyNetAmount.Text))
            //            objBill.PharmacyItems.NetAmount = Convert.ToDouble(txtPharmacyNetAmount.Text);
            //        objBill.PharmacyItems.Items = PharmacyItems.ToList();
            //    }

            //    if (objBill.ChargeDetails != null && objBill.ChargeDetails.Count > 0 && objBill.PharmacyItems != null && objBill.PharmacyItems.Items != null && objBill.PharmacyItems.Items.Count > 0)
            //    {
            //        //objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyCostingDivisionID;  //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
            //        // Added By CDS
            //        objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
            //    }
            //    else if (objBill.ChargeDetails != null && objBill.ChargeDetails.Count > 0)
            //    {
            //        //objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.ClinicalCostingDivisionID;  //Costing Divisions for Clinical Billing BillTypes.Clinical;
            //        // Added By CDS
            //        objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
            //    }
            //    else if (objBill.PharmacyItems != null && objBill.PharmacyItems.Items != null && objBill.PharmacyItems.Items.Count > 0)
            //    {
            //        //objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyCostingDivisionID;  //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
            //        // Added By CDS
            //        objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
            //    }

            //    //if (objBill != null && objBill.BillType == BillTypes.Pharmacy)
            //    //    objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyCostingDivisionID;  //Costing Divisions for Pharmacy Billing 
            //    ////((MasterListItem)(cmbCostingDivision.SelectedItem)).ID;   //Costing Divisions for Pharmacy Billing

            //    //if (objBill != null && objBill.BillType == BillTypes.Clinical)
            //    //    objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.ClinicalCostingDivisionID;  //Costing Divisions for Clinical Billing

            //    if (objBill.PaymentDetails != null)
            //    {
            //        //if (objBill.BillType == BillTypes.Pharmacy)  //if (objBill.BillType == BillTypes.Pharmacy)
            //        //    objBill.PaymentDetails.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyCostingDivisionID;  //Costing Divisions for Pharmacy Billing 
            //        ////((MasterListItem)(cmbCostingDivision.SelectedItem)).ID;   //Costing Divisions for Clinical Billing

            //        //if (objBill.BillType == BillTypes.Clinical)
            //        //    objBill.PaymentDetails.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.ClinicalCostingDivisionID;   //Costing Divisions for Clinical Billing

            //        //objBill.PaymentDetails.CostingDivisionID = objBill.CostingDivisionID;

            //        // Added By CDS
            //        objBill.PaymentDetails.CostingDivisionID = objBill.CostingDivisionID;
            //    }

            //    clsAddBillBizActionVO BizAction = new clsAddBillBizActionVO();
            //    BizAction.Details = new clsBillVO();
            //    BizAction.Details = objBill;

            //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //    Client.ProcessCompleted += (s, arg) =>
            //    {
            //        if (arg.Error == null && arg.Result != null && ((clsAddBillBizActionVO)arg.Result).Details != null)
            //        {
            //            if (((clsAddBillBizActionVO)arg.Result).Details != null)
            //            {
            //                //CloseVisit(((clsAddBillBizActionVO)arg.Result).Details.Opd_Ipd_External_Id, ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId);
            //                BillID = (((clsAddBillBizActionVO)arg.Result).Details).ID;
            //                UnitID = (((clsAddBillBizActionVO)arg.Result).Details).UnitID;

            //                //PRID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID;
            //                PRID = ((MasterListItem)cmbPrintFormat.SelectedItem).ID;


            //                Indicatior.Close();

            //                SetCommandButtonState("Save");
            //                MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                new MessageBoxControl.MessageBoxChildWindow("Palash", "Bill Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //                msgW1.OnMessageBoxClosed += (re) =>
            //                {
            //                    if (re == MessageBoxResult.OK)
            //                    {
            //                        if (pFreezBill == true)
            //                        {                                    

            //                            if ((((clsAddBillBizActionVO)arg.Result).Details).BillType == PalashDynamics.ValueObjects.BillTypes.Clinical)
            //                            {
            //                                if ((((clsAddBillBizActionVO)arg.Result).Details).BalanceAmountNonSelf == 0 && (((clsAddBillBizActionVO)arg.Result).Details).BalanceAmountSelf == 0)
            //                                    PrintBill(BillID, UnitID, PRID);
            //                            }
            //                            else if ((((clsAddBillBizActionVO)arg.Result).Details).BillType == PalashDynamics.ValueObjects.BillTypes.Pharmacy)
            //                            {
            //                                if ((((clsAddBillBizActionVO)arg.Result).Details).BalanceAmountNonSelf == 0 && (((clsAddBillBizActionVO)arg.Result).Details).BalanceAmountSelf == 0)
            //                                    PrintPharmacyBill(BillID, UnitID, PRID);
            //                            }
            //                            else
            //                            {
            //                                if ((((clsAddBillBizActionVO)arg.Result).Details).BalanceAmountNonSelf == 0 && (((clsAddBillBizActionVO)arg.Result).Details).BalanceAmountSelf == 0)
            //                                {
            //                                    PrintBill(BillID, UnitID, PRID);
            //                                    PrintPharmacyBill(BillID, UnitID, PRID);
            //                                }
            //                            }
            //                        }
            //                    }
            //                };
            //                msgW1.Show();
            //                _flip.Invoke(RotationType.Backward);
            //                FillBillSearchList();
            //            }
            //        }
            //        else
            //        {
            //            Indicatior.Close();                        
            //            MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while Adding Bill Details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //            msgW1.Show();
            //            cmdSave.IsEnabled = true;
            //        }
            //    };
            //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //    Client.CloseAsync();
            //}
            //catch (Exception ex)
            //{
            //    cmdSave.IsEnabled = true;
            //    string err = ex.Message;
            //    throw;
            //}
            //finally
            //{
            //    Indicatior.Close();
            //}

            #endregion
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

                    //IsCorporate = ((PaymentWindow)sender).PrintAgainstCorporate.IsChecked;
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

            #region OLD Code Commented By CDS
            //if (((CIMS.Forms.PaymentWindow)sender).DialogResult == true)
            //{
            //    if (((PaymentWindow)sender).Payment != null)
            //    {
            //        rdbAgainstBill.IsChecked = true;
            //        SaveBill(((PaymentWindow)sender).Payment, true);
            //    }
            //}
            //else
            //{
            //    if (!flagFreezFromSearch)
            //    {
            //        cmdSave.IsEnabled = true;
            //    }
            //    if (flagFreezFromSearch)
            //    {
            //        ((clsBillVO)dgBillList.SelectedItem).IsFreezed = false;
            //        chkFreezBill.IsChecked = false;
            //        flagFreezFromSearch = false;
            //    }
            //}
            #endregion
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
            //  cmbConcessionReason.ItemsSource = null;
            cmbConcessionReason.SelectedValue = null;
            InitialiseForm();
            // BY BHUSHAN..
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
                    if (((IApplicationConfiguration)App.Current).SelectedPatient.IsSurrogate == true)
                        ((IApplicationConfiguration)App.Current).OpenMainContent("PalashDynamics.Forms.PatientView.PatientListForSurrogacy"); //
                    else
                        ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                    //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                }
                else
                {
                    SetCommandButtonState("New");
                    _flip.Invoke(RotationType.Backward);
                    IsCancel = true;
                    IsFromViewClick = false;

                }
            }

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

                //if (Menu != null && Menu.Parent == "Surrogacy")
                //    ((IInitiateCIMS)myData).Initiate("Surrogacy");
                //else
                //    ((IInitiateCIMS)myData).Initiate("REG");
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
            #region OLD Commented By CDS
            //ServiceSearch serviceSearch = null;
            //serviceSearch = new ServiceSearch();
            //serviceSearch.OnAddButton_Click += new RoutedEventHandler(serviceSearch_OnAddButton_Click);
            //serviceSearch.Show();
            #endregion

            if ((((IApplicationConfiguration)App.Current).SelectedPatient.RelationID == 15) && (((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 8 || ((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 9))
            {
                DonorCoupleLinkedList Win = new DonorCoupleLinkedList();
                //Win.DonorID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                Win.DonorID = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                Win.DonorUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
                Win.DonorName = ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName + " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " + ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;

                Win.OnSaveButton_Click += new RoutedEventHandler(DonorLinWin_OnSaveButton_Click);
                Win.OnCancelButton_Click += new RoutedEventHandler(DonorLinWin_OnCancelButton_Click);
                Win.Show();
            }
            else
            {

                ServiceSearchForPackage serviceSearch = null;   //ServiceSearch serviceSearch = null;
                serviceSearch = new ServiceSearchForPackage();   //serviceSearch = new ServiceSearch();
                serviceSearch.VisitID = VisitID;
                serviceSearch.IsFromSaveAndPackageBill = IsFromSaveAndPackageBill;
                serviceSearch.OnAddButton_Click += new RoutedEventHandler(serviceSearch_OnAddButton_Click);
                serviceSearch.Show();
            }
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

        //bool ConcessionFromPlan = false;

        //public List<string> SpecializationListItem(long Rate, string Description)
        //{
        //    Rate = Rate;
        //    Description = Description;
        //    //this.Date = Date;
        //}

        //public List<clsSpecializationVO> SpecializationList = new List<clsSpecializationVO>();
        //public List<clsPackageServiceDetailsVO> PackageInPackageItemList = new List<clsPackageServiceDetailsVO>();
        public List<clsServiceMasterVO> lServices = new List<clsServiceMasterVO>();
        void serviceSearch_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            #region OLD Commented By CDS
            //if (((ServiceSearch)sender).DialogResult == true)
            //{
            //    List<clsServiceMasterVO> lServices = new List<clsServiceMasterVO>();
            //    lServices = ((ServiceSearch)sender).SelectedOtherServices.ToList();
            //    PatientSourceID = ((clsPatientSponsorVO)((ServiceSearch)sender).cmbPatientSource.SelectedItem).PatientSourceID;
            //    PatientTariffID = ((MasterListItem)((ServiceSearch)sender).cmbTariff.SelectedItem).ID;

            //    ServiceTariffID = ((ServiceSearch)sender).ServiceTariffID;  //((ServiceSearch)sender).ServiceTariffID;
            //    AddCharges(lServices, ServiceTariffID);
            //}
            #endregion

            if (((ServiceSearchForPackage)sender).DialogResult == true) //if (((ServiceSearch)sender).DialogResult == true)
            {
                //List<string> SpecID = new List<string>(); // Added By CDS For Specialization Services 
                List<clsSpecialSpecializationVO> SpecializationList = new List<clsSpecialSpecializationVO>();



                lServices = ((ServiceSearchForPackage)sender).SelectedOtherServices.ToList();   //lServices = ((ServiceSearch)sender).SelectedOtherServices.ToList();
                bool IsMultiSponser = false;
                bool IscomboPackage = false;    // added on 13082016

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

                // For Package New changes Added on 25062018
                var items7 = from r in lServices
                             where r.IsPackage == false && r.PackageID > 0 && r.IsAdjustableHead == false // Check for Package Consumption Services
                             select r;

                // For Package New changes Added on 25062018
                var items8 = from r in lServices
                             where r.IsPackage == false && r.PackageID > 0 && r.IsAdjustableHead == true  // Check for Package Consumption Services as a adjustable head
                             select r;

                // For Package New changes Added on 25062018
                var items9 = from r in ChargeList
                             where r.isPackageService == false && r.PackageID > 0 && r.IsAdjustableHead == true // Check for Package Consumption Services as a adjustable head 
                             select r;                                                                          // already added in grid   

                // For Package New changes Added on 25062018
                var items10 = from r in ChargeList
                              where r.isPackageService == false && r.PackageID > 0 && r.IsAdjustableHead == false // Check for Package Consumption Services already added in grid
                              select r;                                                                            // excluding adjustable head services

                string MSG = "";
                bool IsServiceCheck = false;

                if (items1.ToList().Count > 0)
                {
                    IsServiceCheck = true;
                    MSG = "Package is Already Exists You can't add the service or Package.";
                }
                else if (items5.ToList().Count > 1)
                {
                    IsServiceCheck = true;
                    MSG = "Package is Already Exists You can't add the service or Package.";
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
                else if (items1.ToList().Count == 0 && items5.ToList().Count > 0)
                {
                    if (IsPackageFreezed == true)
                    {
                        IsServiceCheck = true;
                        MSG = "Please freeze the previous package bill.";
                    }
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

                ///////---------------------------------------------------------------------
                //if (IsMultiSponser == false && IsServiceCheck == false)                               // added on 13082018
                if (IsMultiSponser == false && IsServiceCheck == false && IscomboPackage == false)      // modified on 13082018
                {
                    #region clsApplyPackageDiscountRateOnServiceBizActionVO

                    clsApplyPackageDiscountRateOnServiceBizActionVO objApplyNewRate = new clsApplyPackageDiscountRateOnServiceBizActionVO();

                    objApplyNewRate.objApplyPackageDiscountRate = new List<clsApplyPackageDiscountRateOnServiceVO>();
                    objApplyNewRate.ipServiceList = new List<clsServiceMasterVO>();


                    objApplyNewRate.ipLoginUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                    if (LinkPatientID > 0 && LinkPatientUnitID > 0)
                    {
                        objApplyNewRate.ipPatientID = LinkPatientID;
                        objApplyNewRate.ipPatientUnitID = LinkPatientUnitID;
                    }
                    else
                    {
                        objApplyNewRate.ipPatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                        objApplyNewRate.ipPatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                    }

                    objApplyNewRate.ipVisitID = VisitID;
                    objApplyNewRate.IsIPD = false;

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

                    // Commnted By CDS 
                    //if ((ServiceSearchForPackage)sender != null && ((ServiceSearchForPackage)sender).cmbPatientSource.SelectedItem != null && ((clsPatientSponsorVO)((ServiceSearchForPackage)sender).cmbPatientSource.SelectedItem).MemberRelationID > 0)
                    //    objApplyNewRate.MemberRelationID = ((clsPatientSponsorVO)((ServiceSearchForPackage)sender).cmbPatientSource.SelectedItem).MemberRelationID;
                    //else
                    //    objApplyNewRate.MemberRelationID = 2;

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
                                        // Commented By CDS 
                                        item.ConcessionPercent = 100;   //Convert.ToDecimal(item1.DiscountedPercentage);
                                        ConcessionFromPlan = true;
                                        // Commented By CDS 

                                        // For AND Condition Uptp Condtion Quantity Conditional Discount is given 
                                        //item.ConcessionAmount = 0;
                                        //item.ConcessionPercent = Convert.ToDecimal(item.ConditionalDiscount);
                                        //ConcessionFromPlan = true;
                                        //END
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
                                        //if (item.ID == item1.ServiceID && item.TariffID == item1.TariffID && item.ConditionID == 0)   // For Package New changes commented on 25062018
                                        if (item.ID == item1.ServiceID && item.TariffID == item1.TariffID && item.ConditionID == 0 && item.ProcessID == item1.ProcessID)    // For Package New changes Added on 25062018
                                        {

                                            if (item1.IsApplyOn_Rate_Percentage == 1 || item1.IsApplyOn_Rate_Percentage == 3 && item.ConditionID == 0)
                                            {
                                                //item.Rate = Convert.ToDecimal(item1.DiscountedRate);
                                                if (item1.ActualQuantity == 0 && item1.ServiceMemberRelationID == 0)
                                                {
                                                    // Rate Not Assign Here 
                                                    // Only In Case Of Specialization Services 
                                                }
                                                else
                                                {
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
                                                            //    DiscRateOnForm = DiscRateOnForm - DiscRateOnForm;
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
                                                        }  // Added By CDS For Group Specialization Rate 

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

                                        VisitID = ((ServiceSearchForPackage)sender).VisitID; // Addded By CDS 

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

                                VisitID = ((ServiceSearchForPackage)sender).VisitID; // Addded By CDS 

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
                        FillPatientAdvance(true); //Added By Bhushanp
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

        //public void AddService()
        //{

        //    if (lServices != null) //if (((ServiceSearch)sender).DialogResult == true)
        //    {
        //        //List<string> SpecID = new List<string>(); // Added By CDS For Specialization Services 
        //        List<clsSpecialSpecializationVO> SpecializationList = new List<clsSpecialSpecializationVO>();
        //        lServices = lServices;   //lServices = ((ServiceSearch)sender).SelectedOtherServices.ToList();
        //        bool IsMultiSponser = false;
        //        if (IsAuditTrail)
        //        {
        //            LogInformation = new LogInfo();
        //            LogInformation.guid = objGUID;
        //            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
        //            LogInformation.TimeStamp = DateTime.Now;
        //            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
        //            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
        //            LogInformation.Message = " 13 : Selected Services For Bill " //+ Convert.ToString(lineNumber)
        //                                                    + "Is MultiSponser : " + Convert.ToString(IsMultiSponser);
        //        }

        //        var item5 = from p in ChargeList
        //                    where p.IsDefaultService == true
        //                    select p;
        //        if (item5.ToList().Count != ChargeList.Count)
        //        {
        //            foreach (var item in lServices)
        //            {
        //                var item1 = from r in ChargeList
        //                            where r.CompanyID == item.CompanyID && r.PatientSourceID == item.PatientSourceID && r.TariffId == item.TariffID && r.IsDefaultService == false
        //                            select r;

        //                if (item1.ToList().Count > 0)
        //                {
        //                    IsMultiSponser = false;
        //                }
        //                else
        //                {
        //                    IsMultiSponser = true;
        //                }
        //                break;
        //            }

        //        }
        //        //Added By Bhushanp 03082017 FOR New Package Changes
        //        var items1 = from r in ChargeList
        //                     where r.isPackageService == true
        //                     select r;

        //        var items2 = from r in ChargeList
        //                     where r.isPackageService == false && r.PackageID == 0
        //                     select r;


        //        var items3 = from r in ChargeList
        //                     where r.isPackageService == false && r.PackageID > 0
        //                     select r;

        //        var items4 = from r in lServices
        //                     where r.IsPackage == false && r.PackageID == 0
        //                     select r;


        //        var items5 = from r in lServices
        //                     where r.IsPackage == true
        //                     select r;

        //        var items6 = from r in lServices
        //                     where r.IsPackage == false && r.PackageID > 0
        //                     select r;


        //        string MSG = "";
        //        bool IsServiceCheck = false;
        //        if (items1.ToList().Count > 0)
        //        {
        //            IsServiceCheck = true;
        //            MSG = "You cannot add more than one package.";
        //        }
        //        else if (items5.ToList().Count > 1)
        //        {
        //            IsServiceCheck = true;
        //            MSG = "You cannot add more than one package.";
        //        }
        //        else if (items2.ToList().Count > 0 && (items5.ToList().Count > 0 || items6.ToList().Count > 0))
        //        {
        //            IsServiceCheck = true;
        //            MSG = "You cannot add package or package consumption with service.";
        //        }
        //        else if (items3.ToList().Count > 0 && (items4.ToList().Count > 0 || items5.ToList().Count > 0))
        //        {
        //            IsServiceCheck = true;
        //            MSG = "You cannot add package or srevices with package consumption.";
        //        }
        //        else if (items4.ToList().Count > 0 && items5.ToList().Count > 0 || items4.ToList().Count > 0 && items6.ToList().Count > 0 || items5.ToList().Count > 0 && items6.ToList().Count > 0)
        //        {
        //            IsServiceCheck = true;
        //            MSG = "You cannot add package, package consumption, and services in same Bill.";
        //        }
        //        ///////---------------------------------------------------------------------
        //        if (IsMultiSponser == false && IsServiceCheck == false)
        //        {
        //            #region clsApplyPackageDiscountRateOnServiceBizActionVO

        //            clsApplyPackageDiscountRateOnServiceBizActionVO objApplyNewRate = new clsApplyPackageDiscountRateOnServiceBizActionVO();

        //            objApplyNewRate.objApplyPackageDiscountRate = new List<clsApplyPackageDiscountRateOnServiceVO>();
        //            objApplyNewRate.ipServiceList = new List<clsServiceMasterVO>();


        //            objApplyNewRate.ipLoginUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
        //            objApplyNewRate.ipPatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
        //            objApplyNewRate.ipPatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;


        //            objApplyNewRate.ipVisitID = VisitID;
        //            objApplyNewRate.IsIPD = false;

        //            objApplyNewRate.ipPatientGenderID = ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID;

        //            if (((IApplicationConfiguration)App.Current).SelectedPatient.IsAge == true)
        //            {
        //                objApplyNewRate.ipPatientDateOfBirth = ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirthFromAge;
        //            }
        //            else
        //            {
        //                objApplyNewRate.ipPatientDateOfBirth = ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth;
        //            }

        //            objApplyNewRate.ipServiceList = lServices;


        //            if ((ServiceSearchForPackage)sender != null && ((ServiceSearchForPackage)sender).cmbPatientSource.SelectedItem != null && ((clsPatientSponsorVO)((ServiceSearchForPackage)sender).cmbPatientSource.SelectedItem).MemberRelationID > 0)
        //                objApplyNewRate.MemberRelationID = ((clsPatientSponsorVO)((ServiceSearchForPackage)sender).cmbPatientSource.SelectedItem).MemberRelationID;

        //            objApplyNewRate.SponsorID = ((MasterListItem)((ServiceSearchForPackage)sender).cmbTariff.SelectedItem).SponsorID;
        //            objApplyNewRate.SponsorUnitID = ((MasterListItem)((ServiceSearchForPackage)sender).cmbTariff.SelectedItem).SponsorUnitID;

        //            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //            client.ProcessCompleted += (s, arg) =>
        //            {
        //                if (arg.Error == null && arg.Result != null)
        //                {
        //                    objApplyNewRate = (clsApplyPackageDiscountRateOnServiceBizActionVO)arg.Result;
        //                    UnavailableItemStockService = new List<clsUnavailableItemStockServiceId>();                          

        //                    if (sbServiceName.Length > 0)
        //                    {
        //                        MessageBoxControl.MessageBoxChildWindow msgWD =
        //                                        new MessageBoxControl.MessageBoxChildWindow("", "Items are not available for Service : " + sbServiceName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

        //                        msgWD.OnMessageBoxClosed += (re) =>
        //                        {
        //                            if (re == MessageBoxResult.OK)
        //                            {

        //                                PatientCategoryID = ((ServiceSearchForPackage)sender).PatientCategoryL1Id_Retail; //((ServiceSearch)sender).PatientCategoryL1Id_Retail;
        //                                PatientSourceID = ((clsPatientSponsorVO)((ServiceSearchForPackage)sender).cmbPatientSource.SelectedItem).PatientSourceID;   //((clsPatientSponsorVO)((ServiceSearch)sender).cmbPatientSource.SelectedItem).PatientSourceID;
        //                                PatientTariffID = ((MasterListItem)((ServiceSearchForPackage)sender).cmbTariff.SelectedItem).ID;  //((MasterListItem)((ServiceSearch)sender).cmbTariff.SelectedItem).ID;

        //                                PatientCompanyID = ((MasterListItem)((ServiceSearchForPackage)sender).cmbCompany.SelectedItem).ID;  //((MasterListItem)((ServiceSearch)sender).cmbTariff.SelectedItem).ID;


        //                                PackageTariffID = ((ServiceSearchForPackage)sender).PackageTariffID; //((ServiceSearch)sender).PackageTariffID;
        //                                ServiceTariffID = ((ServiceSearchForPackage)sender).ServiceTariffID;  //((ServiceSearch)sender).ServiceTariffID;

        //                                VisitID = ((ServiceSearchForPackage)sender).VisitID; // Addded By CDS 

        //                                AddCharges(lServices, ServiceTariffID);
        //                                ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID = PatientCompanyID;// added by Bhushanp
        //                            }
        //                        };
        //                        msgWD.Show();
        //                    }
        //                    else
        //                    {

        //                        PatientCategoryID = ((ServiceSearchForPackage)sender).PatientCategoryL1Id_Retail; //((ServiceSearch)sender).PatientCategoryL1Id_Retail;
        //                        PatientSourceID = ((clsPatientSponsorVO)((ServiceSearchForPackage)sender).cmbPatientSource.SelectedItem).PatientSourceID;  //((clsPatientSponsorVO)((ServiceSearch)sender).cmbPatientSource.SelectedItem).PatientSourceID;
        //                        PatientTariffID = ((MasterListItem)((ServiceSearchForPackage)sender).cmbTariff.SelectedItem).ID;  //((MasterListItem)((ServiceSearch)sender).cmbTariff.SelectedItem).ID;

        //                        PatientCompanyID = ((MasterListItem)((ServiceSearchForPackage)sender).cmbCompany.SelectedItem).ID;

        //                        PackageTariffID = ((ServiceSearchForPackage)sender).PackageTariffID;  //((ServiceSearch)sender).PackageTariffID;
        //                        ServiceTariffID = ((ServiceSearchForPackage)sender).ServiceTariffID;  //((ServiceSearch)sender).ServiceTariffID;

        //                        VisitID = ((ServiceSearchForPackage)sender).VisitID; // Addded By CDS 

        //                        AddCharges(lServices, ServiceTariffID);
        //                        ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID = PatientCompanyID;// added by Bhushanp
        //                    }    
        //                }
        //                FillPatientAdvance(true); //Added By Bhushanp
        //            };
        //            client.ProcessAsync(objApplyNewRate, ((IApplicationConfiguration)App.Current).CurrentUser);
        //            client.CloseAsync();

        //            #endregion
        //        }
        //        else if (IsServiceCheck == true)
        //        {
        //            MessageBoxControl.MessageBoxChildWindow msgW2 =
        //                new MessageBoxControl.MessageBoxChildWindow("Palash", MSG.ToString(), MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //            msgW2.Show();
        //        }
        //        else
        //        {
        //            MessageBoxControl.MessageBoxChildWindow msgW2 =
        //                                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Multiple Sponsor Billing is not Allowed", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //            msgW2.Show();
        //        }
        //    }
        //}

        private void AddCharges(List<clsServiceMasterVO> mServices, long TariffID)
        {
            StringBuilder strError = new StringBuilder();

            for (int i = 0; i < mServices.Count; i++)
            {
                bool Addcharge = true;

                if (ChargeList != null && ChargeList.Count > 0)
                {
                    var item = from r in ChargeList
                               where r.ServiceId == mServices[i].ID
                               && r.ProcessID == mServices[i].ProcessID     // For Package New Changes Added on 14052018
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
                    itemC.Service = mServices[i].ServiceName;
                    itemC.TariffId = mServices[i].TariffID;
                    itemC.ServiceSpecilizationID = mServices[i].Specialization;
                    itemC.TariffServiceId = mServices[i].TariffServiceMasterID;
                    itemC.ServiceId = mServices[i].ID;
                    itemC.ServiceName = mServices[i].ServiceName;
                    itemC.Quantity = 1;
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

                    itemC.ProcessID = mServices[i].ProcessID;       // Package New Changes for Procedure Added on 21042018

                    if (itemC.isPackageService == false && itemC.PackageID > 0)
                    {
                        itemC.ParentID = mServices[i].ChargeID;//For package bill service  charge id as PArent ID 180417 added by yk
                        chkFreezBill.IsChecked = true;
                        chkFreezBill.IsEnabled = false;
                        IsPackageConsumption = true;
                    }
                    else
                    {
                        itemC.ParentID = 0;
                        chkFreezBill.IsChecked = false;
                        chkFreezBill.IsEnabled = true;
                        IsPackageConsumption = false;
                    }

                    itemC.PackageBillID = mServices[i].PackageBillID;
                    itemC.PackageBillUnitID = mServices[i].PackageBillUnitID;

                    itemC.IsAdjustableHead = mServices[i].IsAdjustableHead;         // For Package New Changes Added on 19062018
                    itemC.AdjustableHeadType = mServices[i].AdjustableHeadType;     // For Package New Changes Added on 19062018

                    //itemC.SelectedDoctor.ID = VisitDoctorID;
                    //itemC.SelectedDoctor.Description = VisitDoctor;

                    // Added By CDS                     
                    itemC.ServiceCode = mServices[i].ServiceCode;

                    //***//
                    itemC.VisitID = mServices[i].VisitID;
                    itemC.PrescriptionDetailsID = mServices[i].PrescriptionDetailsID;
                    itemC.InvestigationDetailsID = mServices[i].InvestigationDetailsID;
                    //-----
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
                        if (itemC.isPackageService == false && itemC.PackageID > 0)     // For Package New Changes added on 14062018
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
                    //ChargeList.Add(itemC);
                    //if (itemC.isPackageService == true)
                    //{
                    //    GetPackageServices(itemC.ServiceId, PackageTariffID);
                    //}

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
                    if (itemC.isPackageService == false && itemC.PackageID > 0)
                    {
                        //if (itemC.ConcessionPercent == 100)           // For Package New changes Commented on 14062018 
                        if (itemC.PackageConcessionPercent == 100)      // For Package New changes Added on 14062018 
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

            # region For Package New Changes Added on 19062018

            long PackageBillID2 = 0;
            long PackageBillUnitID2 = 0;
            bool IsAdjustableHead = false;

            if (ChargeList != null && ChargeList.Count > 0)         // For Package New Changes Added on 19062018
            {
                if (ChargeList.Where(p => p.PackageBillID > 0).Any())
                    PackageBillID2 = ChargeList.FirstOrDefault(p => p.PackageBillID > 0).PackageBillID;
                if (ChargeList.Where(p => p.PackageBillUnitID > 0).Any())
                    PackageBillUnitID2 = ChargeList.FirstOrDefault(p => p.PackageBillUnitID > 0).PackageBillUnitID;
                if (ChargeList.Where(p => p.IsAdjustableHead == true).Any())
                    IsAdjustableHead = ChargeList.FirstOrDefault(p => p.IsAdjustableHead == true).IsAdjustableHead;
            }

            if (PackageBillID2 > 0 && PackageBillUnitID2 > 0 && IsAdjustableHead == true)       // For Package New Changes Added on 19062018
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
            double clinicalTotal, clinicalConcession, clinicalNetAmount;

            clinicalTotal = clinicalConcession = clinicalNetAmount = ApprovalConcenssion = ApprovalTotal = PackageConcenssion = 0;

            for (int i = 0; i < ChargeList.Count; i++)
            {
                clinicalTotal += (ChargeList[i].TotalAmount);
                clinicalConcession += ChargeList[i].Concession;
                clinicalNetAmount += ChargeList[i].NetAmount;


                //By Anjali.........................
                if (ChargeList[i].PackageID == 0 || ChargeList[i].isPackageService == true)
                {
                    ApprovalConcenssion += ChargeList[i].Concession;
                    ApprovalTotal += ChargeList[i].TotalAmount;
                }
                //...................................
                // Added By CDS ......................
                if (ChargeList[i].PackageID > 0)
                {
                    //PackageConcenssion += ChargeList[i].Concession;          // Commented on 14062018 for Package New changes
                    PackageConcenssion += ChargeList[i].PackageConcession;     // Added on 14062018 for Package New changes
                }
                // Added By CDS ......................
            }
            //clinicalNetAmount = clinicalNetAmount;
            //dgCharges.ItemsSource = null;
            //dgCharges.ItemsSource = ChargeList;
            txtClinicalTotal.Text = String.Format("{0:0.00}", clinicalTotal);

            if (clinicalConcession <= 0)
            {
                cmbConcessionReason.SelectedValue = 0;
                // cmbConcessionReason.IsEnabled = false;
            }
            txtClinicalConcession.Text = String.Format("{0:0.00}", clinicalConcession);

            //if (Convert.ToDouble(txtClinicalConcession.Text) > 0)
            //{
            //    cmbConcessionReason.IsEnabled = true;
            //}
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
                //NetAmount += PharmacyItems[i].NetAmount;
            }
            //dgCharges.ItemsSource = null;
            //dgCharges.ItemsSource = ChargeList;
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
                //double lPayAmount = 0;
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
                if (BizAction.AdvanceDetails == null) BizAction.AdvanceDetails = new clsAdvanceVO();
                BizAction.AdvanceDetails.IsForTotalAdvance = true;

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
                            //txtAdvance.Text = PatBalAdvaceAmt.ToString();                      // For Package New Changes Commented on 20062018
                            txtAdvance.Text = (Math.Round(PatBalAdvaceAmt, 2)).ToString();       // For Package New Changes Added on 20062018
                        }
                        else //Added By Bhushanp
                        {
                            txtAdvance.Text = "0".ToString();
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
                if (((IApplicationConfiguration)App.Current).SelectedPatient.IsSurrogate == true)
                {
                    ModuleName = "PalashDynamics";
                    Action = "CIMS.Forms.PatientView.PatientListForSurrogacy";
                }
                else
                {
                    ModuleName = "PalashDynamics";
                    Action = "CIMS.Forms.PatientList";
                }

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
                        if (tempList != null) //&& tempList.Count > 1  Commented By devidas
                        {
                            string msgText = "Are You Sure \n  You Want To Delete The Selected Service Charge ?";
                            MessageBoxControl.MessageBoxChildWindow msgWD =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                            msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.Yes)
                                {
                                    clsChargeVO objVO = (clsChargeVO)dgCharges.SelectedItem;
                                    if (objVO.IsResultEntry == false) //Added By Yogesh Isupload,IsResultEntry 2 7 16
                                    {
                                        if (objVO.Isupload == false)
                                        {
                                            //long chargID = objVO.ID; // 
                                            long chargID = objVO.ServiceId;   //Added By Ajit Jadhav,Date 5/10/2016
                                            long tserID = objVO.TariffServiceId;
                                            //clsIndentDetailVO objVO = (clsIndentDetailVO)grdStoreIndent.SelectedItem;
                                            clsChargeVO obj;
                                            if (objVO != null)
                                            {
                                                //  obj = ChargeList.Where(z => z.ID == objVO.ID).FirstOrDefault();
                                                obj = ChargeList.Where(z => z.ServiceId == objVO.ServiceId).FirstOrDefault();//Added By Ajit Jadhav,Date 5/10/2016
                                                //BillAddedItems.Where(z => z.ID == objVO.ID).FirstOrDefault();
                                                ChargeList.Remove(obj);
                                                //DeletedChargeDetails
                                                DeleteBillAddedItems.Add(obj);
                                                // objj.DeletedChargeDetails.Add(objj);
                                            }
                                            dgCharges.Focus();
                                            dgCharges.UpdateLayout();
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
            //Itemswin.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
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
                    //if (GRNAddedItems == null)
                    //    GRNAddedItems = new ObservableCollection<clsGRNDetailsVO>();
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
                            //ObjAddItem.Amount = (((item.MRP * Itemswin.SelectedItems[0].TotalSalesTaxPercent) / 100) + item.MRP);
                            ObjAddItem.ConcessionPercentage = item.DiscountOnSale;

                            ObjAddItem.ConcessionAmount = ObjAddItem.ConcessionAmount;
                            //if(item.DiscountOnSale>0)
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
            CheckVisit(true);
            cmbConcessionReason.ItemsSource = null;
            IsFromSaveAndPackageBill = false; //Added By Bhushanp 03082017
            AdvanceBizActionVO = null;  // added on 20082018 For Package Advance & Bill Save in transaction
            fillreason();//Added By Yogesh K 21 Apr 2016
            cmbConcessionReason.SelectedValue = null;//Added By Yogesh K 21 Apr 2016
            cmbConcessionReason.IsEnabled = false;
            //txtApprovalRemark.IsEnabled = false;
            txtApprovalRemark.Text = string.Empty;// Change By Bhushanp 09032017
            txtApprovalRemark.ClearValidationError();
            cmbConcessionReason.ClearValidationError();
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

            //  cmbConcessionReason.SelectedValue = "";
            txtTotalBill.Text = "";
            txtTotalConcession.Text = "";
            txtNetAmount.Text = "";
            txtTotalClinicalBill.Text = "";
            txtTotalPharmacyBill.Text = "";

            //Added by priyanka
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
                //txtApprovalRemark.IsEnabled = false;
            }
            else
            {
                lnkAddItems.IsEnabled = false;
                lnkAddItemsFromPrescription.IsEnabled = false;
                lnkAddService.IsEnabled = false;
                lnkAddServicesFromPrescription.IsEnabled = false;
                chkFreezBill.IsEnabled = false;
                //txtApprovalRemark.IsReadOnly = false;
                // txtApprovalRemark.IsEnabled = true;
            }



            PatientTariffID = 0;
            PatientSourceID = 0;
            PackageServiceList = new ObservableCollection<PalashDynamics.ValueObjects.clsPackageServiceDetailsVO>();
            // SelectedBill = new clsBillVO();
            // this.DataContext = new clsGRNVO();
            //Commented By Bhushanp
            //if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForConcessionID == 0)
            //{
            //    //txtApprovalRemark.IsReadOnly = false;
            //    //txtApprovalRemark.IsEnabled = true;
            //    cmbConcessionReason.IsEnabled = true;
            //}
            if (IsFromSaveAndPackageBill)
            {
                chkFreezBill.IsEnabled = false;
                lnkAddService.IsEnabled = false;
                lnkAddItemsFromPrescription.IsEnabled = false;
                lnkAddServicesFromPrescription.IsEnabled = false;
            }
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
                    //cmdPrint.IsEnabled = false;
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    IsCancel = true;
                    break;

                case "Save":
                    //cmdPrint.IsEnabled = true; 
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    IsCancel = true;
                    IsFromViewClick = false;
                    break;

                case "Modify":
                    //cmdPrint.IsEnabled = false;
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
                    //cmdPrint.IsEnabled = true; 
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

            DrugSearch.VisitID = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;

            DrugSearch.OnSaveButton_Click += new RoutedEventHandler(DrugSearch_OnSaveButton_Click);
            DrugSearch.Show();
        }

        //void DrugSearch_OnSaveButton_Click(object sender, RoutedEventArgs e)
        //{
        //    //throw new NotImplementedException();
        //    DrugsForPatientPrescription Itemswin = (DrugsForPatientPrescription)sender;
        //    if (Itemswin.DialogResult == true)
        //    {

        //        if (Itemswin.SelectedDrug != null && Itemswin.SelectedBatches != null)
        //        {
        //            StringBuilder strError = new StringBuilder();
        //            StoreID = Itemswin.StoreID;
        //            //StoreID = Itemswin.StoreID;
        //            foreach (var item in Itemswin.SelectedBatches)
        //            {
        //                bool Additem = true;
        //                if (PharmacyItems != null && PharmacyItems.Count > 0)
        //                {
        //                    var item1 = from r in PharmacyItems
        //                                where (r.BatchID == item.BatchID)

        //                                select new clsItemSalesDetailsVO
        //                                {
        //                                    Status = r.Status,
        //                                    ID = r.ID,
        //                                    ItemName = r.ItemName
        //                                };

        //                    if (item1.ToList().Count > 0)
        //                    {
        //                        if (strError.ToString().Length > 0)
        //                            strError.Append(",");
        //                        strError.Append(item1.ToList()[0].ItemName);

        //                        Additem = false;

        //                    }
        //                }

        //                if (Additem)
        //                {
        //                    PharmacyItems.Add(new clsItemSalesDetailsVO()     
        //                    {
        //                        ItemID = Itemswin.SelectedDrug.DrugID,
        //                        ItemName = Itemswin.SelectedDrug.DrugName,
        //                        BatchID = item.BatchID,
        //                        BatchCode = item.BatchCode,
        //                        ExpiryDate = item.ExpiryDate,
        //                        Quantity = Convert.ToDouble(Itemswin.SelectedDrug.Quantity),
        //                        MRP = item.MRP,
        //                        AvailableQuantity = item.AvailableStock

        //                    });
        //            }
        //            }
        //            CalculatePharmacySummary();
        //            dgPharmacyItems.Focus();
        //            dgPharmacyItems.UpdateLayout();
        //            dgPharmacyItems.SelectedIndex = PharmacyItems.Count - 1;

        //            if (!string.IsNullOrEmpty(strError.ToString()))
        //            {
        //                string strMsg = "Items already added : " + strError.ToString();

        //                MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //                msgW1.Show();
        //            }
        //        }
        //    }
        //}



        //Event code sent by Harish on 20-Apr-2011

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
                                //ItemID = Itemswin.SelectedItems[0].ID,
                                //ItemName = Itemswin.SelectedItems[0].ItemName,
                                //Manufacture = Itemswin.SelectedItems[0].Manufacturer,
                                //PregnancyClass = Itemswin.SelectedItems[0].PreganancyClass,
                                //BatchID = item.BatchID,
                                //BatchCode = item.BatchCode,
                                //ExpiryDate = item.ExpiryDate,
                                //Quantity = 1,
                                //MRP = (((item.MRP * Itemswin.SelectedItems[0].TotalSalesTaxPercent) / 100) + item.MRP),
                                //AvailableQuantity = item.AvailableStock,
                                //Amount = (((item.MRP * Itemswin.SelectedItems[0].TotalSalesTaxPercent) / 100) + item.MRP),
                                //ConcessionPercentage = item.DiscountOnSale,
                                //VATPercent = item.VATPerc
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
                    InitialiseForm();
                    SetCommandButtonState("Modify");
                    IsFromViewClick = true;
                    //InitialiseForm();
                    rdbAgainstBill.IsChecked = true;
                    rdbAgainstBill.IsEnabled = false;
                    rdbAgainstServices.IsEnabled = false;
                    IsFromSaveAndPackageBill = false;// added By Bhushanp 16082017
                    AdvanceBizActionVO = null;       // added on 20082018 For Package Advance & Bill Save in transaction
                    lnkAddServicesFromPrescription.IsEnabled = true;
                    lnkAddService.IsEnabled = true;
                    chkFreezBill.IsEnabled = true;
                    rdbDetail.IsChecked = true;
                    rdbInline.IsChecked = false;

                    SelectedBill = new clsBillVO();
                    SelectedBill = (clsBillVO)dgBillList.SelectedItem;
                    BillID = ((clsBillVO)dgBillList.SelectedItem).ID;
                    PatientSourceID = SelectedBill.PatientSourceId;
                    PatientTariffID = SelectedBill.TariffId;
                    // txtApprovalRemark.Text = ((clsBillVO)dgBillList.SelectedItem).ApprovalRemark;
                    cmbConcessionReason.SelectedValue = ((clsBillVO)dgBillList.SelectedItem).ConcessionReasonId;//Added By Yogesh K 21 Apr 16
                    // cmbConcessionReason.SelectedValue = SelectedBill.ConcessionReasonId;
                    // cmbConcessionReason.SelectedValue = 
                    //   cmbConcessionReason.
                    //  FillPrintFormat();  //Fill Combo
                    if (SelectedBill.TotalConcessionAmount > 0)
                    {
                        cmbConcessionReason.IsEnabled = true;
                    }
                    else
                    {
                        cmbConcessionReason.IsEnabled = false;
                    }
                    // txtApprovalRemark.IsEnabled = true;
                    txtApprovalRemark.Text = SelectedBill.ConcessionRemark.Trim().ToString();
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
                        // txtApprovalRemark.IsEnabled = false;
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
                        //Added By CDS
                        //FillChargeList(SelectedBill.ID, SelectedBill.UnitID, true, SelectedBill.Opd_Ipd_External_Id);
                        FillChargeList(SelectedBill.ID, SelectedBill.UnitID, true, SelectedBill.Opd_Ipd_External_Id, SelectedBill.Opd_Ipd_External_UnitId, false);
                        FillPharmacyItemsList(SelectedBill.ID, SelectedBill.UnitID, true);
                        if (((clsBillVO)dgBillList.SelectedItem).BillPaymentType == PalashDynamics.ValueObjects.BillPaymentTypes.AgainstServices)
                            rdbAgainstServices.IsChecked = true;
                    }
                    else
                    {
                        if (((clsBillVO)dgBillList.SelectedItem).RequestTypeID == (int)RequestType.Concession && ((clsBillVO)dgBillList.SelectedItem).IsRequestSend)
                        {
                            lnkAddServicesFromPrescription.IsEnabled = false;
                            lnkAddService.IsEnabled = false;
                        }

                        //Added By CDS
                        //FillChargeList(SelectedBill.ID, SelectedBill.UnitID, false, SelectedBill.Opd_Ipd_External_Id);
                        FillChargeList(SelectedBill.ID, SelectedBill.UnitID, false, SelectedBill.Opd_Ipd_External_Id, SelectedBill.Opd_Ipd_External_UnitId, false);
                        FillPharmacyItemsList(SelectedBill.ID, SelectedBill.UnitID, false);
                    }
                    //long BillID;

                    IsEditMode = true; // Added By CDS 
                    FillPatientAdvance(false); // Added By CDS To Fill Proper Advanced Details 
                    chkFreezBill.IsChecked = ((clsBillVO)dgBillList.SelectedItem).IsFreezed;

                    if (objUser.OPDAuthLvtForConcessionID == 0 && IsEditMode == false)
                    {
                        cmbConcessionReason.IsEnabled = true;
                        //txtApprovalRemark.IsEnabled = true;
                    }
                    else
                    {
                        if (IsEditMode == true && dgBillList.SelectedItem != null)
                        {
                            if ((dgBillList.SelectedItem as clsBillVO).LevelID > 0 && objUser.OPDAuthLvtForConcessionID != 0)
                            {
                                cmbConcessionReason.IsEnabled = true;
                                // txtApprovalRemark.IsEnabled = true;
                            }
                            else if (((dgBillList.SelectedItem as clsBillVO).LevelID == 1 || (dgBillList.SelectedItem as clsBillVO).LevelID == 2) && objUser.OPDAuthLvtForConcessionID == 0)
                            {
                                cmbConcessionReason.IsEnabled = false;
                                //    txtApprovalRemark.IsEnabled = false;
                            }
                            //else if ((dgBillList.SelectedItem as clsBillVO).LevelID == 0 && (dgBillList.SelectedItem as clsBillVO).IsApproved == false)
                            //    cmbConcessionReason.IsEnabled = false;
                        }
                    }

                    _flip.Invoke(RotationType.Forward);
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                //throw;
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
            //* Commented by - Ajit Jadhav
            //* Added Date - 13/10/2016
            //* Comments - get pan number
            PanNumber = ((IApplicationConfiguration)App.Current).SelectedPatient.PanNumber;
            //***//--------------------
            bool isValid = true;
            isValid = CheckValidations();
            if (isValid)
            {
                msgConsumption = string.Empty;
                if (chkFreezBill.IsChecked == true)
                {
                    if (ChargeList.Select(z => z.PackageID).FirstOrDefault() > 0)  // only when package bill. && !ChargeList.Select(z => z.isPackageService).FirstOrDefault()
                    {
                        clsAddBillBizActionVO BizAction = new clsAddBillBizActionVO();
                        BizAction.IsForPackageAdvanceCheck = true;
                        if (BizAction.Details == null) BizAction.Details = new clsBillVO();
                        BizAction.Details.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                        BizAction.Details.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
                        BizAction.Details.TotalBillAmount = Convert.ToDouble(txtClinicalTotal.Text);
                        BizAction.Details.BillID = ((clsBillVO)dgBillList.SelectedItem).ID;
                        BizAction.Details.UnitID = ((clsBillVO)dgBillList.SelectedItem).UnitID;

                        if (ChargeList != null && ChargeList.Count > 0)
                            BizAction.Details.PackageID = ChargeList.Select(z => z.PackageID).FirstOrDefault();

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        Client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null && arg.Result != null)
                            {
                                decimal tmpTotalRate = 0;
                                foreach (var item in ChargeList)
                                {
                                    tmpTotalRate += Convert.ToDecimal(item.Rate);
                                }

                                if (((clsAddBillBizActionVO)arg.Result).ServiceConsumptionAmount == 0 && ((clsAddBillBizActionVO)arg.Result).PharmacyConsumptionAmount == 0 && ChargeList.Select(z => z.isPackageService).FirstOrDefault())
                                {
                                    msgConsumption = "Consumption Pending for this package,";
                                }
                                else if (((clsAddBillBizActionVO)arg.Result).ServiceConsumptionAmount == 0 && ChargeList.Select(z => z.isPackageService).FirstOrDefault())
                                {
                                    msgConsumption = "Service Consumption Pending for this package,";
                                }
                                else if (((clsAddBillBizActionVO)arg.Result).PharmacyConsumptionAmount == 0 && ChargeList.Select(z => z.isPackageService).FirstOrDefault())
                                {
                                    msgConsumption = "Pharmacy Consumption Pending for this package,";
                                }
                                else
                                    msgConsumption = "";

                                if (tmpTotalRate < (((clsAddBillBizActionVO)arg.Result).ServiceConsumptionAmount + ((clsAddBillBizActionVO)arg.Result).PharmacyConsumptionAmount))
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter total Package Amount. Total Package Amount is " + (((clsAddBillBizActionVO)arg.Result).ServiceConsumptionAmount + ((clsAddBillBizActionVO)arg.Result).PharmacyConsumptionAmount), MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgWD_1.Show();
                                }
                                else
                                {
                                    #region Modify
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

                                            if (((objUser.OpdBillingAmmount == 0 ? false : objUser.OpdBillingAmmount < Convert.ToDecimal(ApprovalConcenssion)) || (objUser.OpdBillingPercentage == 0 ? false : (((Convert.ToDecimal(ApprovalConcenssion) / Convert.ToDecimal(ApprovalTotal)) * 100)) > objUser.OpdBillingPercentage)) && ((clsBillVO)dgBillList.SelectedItem).IsApproved == false) // Change By Bhushanp 09032017
                                            {
                                                MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Concession Exceeds The Authorised Amount,Are You Sure You Want To Send For Approval?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                                msgWD_1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWD_1_OnMessageBoxClosed);
                                                msgWD_1.Show();
                                            }
                                            else
                                            {

                                                // Added By CDS In Case Of Donor Bill

                                                //if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 8 || ((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 9)
                                                //{
                                                //    DonorCoupleLinkedList Win = new DonorCoupleLinkedList();
                                                //    Win.DonorID = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                                                //    Win.DonorUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
                                                //    Win.DonorName = ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName + " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " + ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;

                                                //    Win.OnSaveButton_Click += new RoutedEventHandler(ModifyDonorLinWin_OnSaveButton_Click);
                                                //    Win.OnCancelButton_Click += new RoutedEventHandler(DonorLinWin_OnCancelButton_Click);
                                                //    Win.Show();
                                                //}
                                                //else
                                                //{
                                                string msgText = msgConsumption + " Are You Sure \n  You Want To Freeze The Bill ?";
                                                MessageBoxControl.MessageBoxChildWindow msgWD =
                                                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                                                msgWD.OnMessageBoxClosed += (arg1) =>
                                                {
                                                    if (arg1 == MessageBoxResult.Yes)
                                                    {
                                                        PaymentWindow paymentWin = new PaymentWindow();
                                                        paymentWin.PatientCategoryID = ((clsBillVO)dgBillList.SelectedItem).PatientCategoryId; //this.PatientCategoryID;  //Ashish Z
                                                        paymentWin.TotalAmount = double.Parse(txtPayAmount.Text);
                                                        paymentWin.Initiate("Bill");
                                                        paymentWin.conID = ((clsBillVO)dgBillList.SelectedItem).ConcessionAuthorizedBy;
                                                        paymentWin.txtPayTotalAmount.Text = this.txtNetAmount.Text;
                                                        paymentWin.txtDiscAmt.Text = this.txtTotalConcession.Text;
                                                        paymentWin.txtPayableAmt.Text = this.txtPayAmount.Text;
                                                        paymentWin.PackageConcenssionAmt = PackageConcenssion;
                                                        if (!String.IsNullOrEmpty(PanNumber) && PanNumber != "NULL")
                                                        {
                                                            paymentWin.txtPanNo.Text = PanNumber;
                                                            paymentWin.txtPanNo.IsEnabled = false;
                                                        }
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

                                                        if (ChargeList != null && ChargeList.Count > 0)
                                                        {
                                                            paymentWin.PackageID = ChargeList.Select(z => z.PackageID).FirstOrDefault();
                                                            paymentWin.PackageBillID = ((clsBillVO)dgBillList.SelectedItem).ID;
                                                            paymentWin.PackageBillUnitID = ((clsBillVO)dgBillList.SelectedItem).UnitID;
                                                        }
                                                        //***//19
                                                        paymentWin.BillNo = ChargeList.Select(z => z.BillNo).FirstOrDefault();
                                                        paymentWin.IsBilled = ChargeList.Select(z => z.IsBilled).FirstOrDefault();
                                                        paymentWin.isPackageService = ChargeList.Select(z => z.isPackageService).FirstOrDefault();
                                                        //------------
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

                                                        paymentWin.OnSaveButton_Click += new RoutedEventHandler(paymentWin_OnSaveButton_Click);
                                                        paymentWin.Show();
                                                    }
                                                };
                                                msgWD.Show();
                                                //} //
                                            }
                                        }

                                        else
                                        {
                                            //if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 8 || ((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 9)
                                            //{
                                            //    DonorCoupleLinkedList Win = new DonorCoupleLinkedList();
                                            //    Win.DonorID = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                                            //    Win.DonorUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
                                            //    Win.DonorName = ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName + " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " + ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;

                                            //    Win.OnSaveButton_Click += new RoutedEventHandler(ModifyDonorLinWin_OnSaveButton_Click);
                                            //    Win.OnCancelButton_Click += new RoutedEventHandler(DonorLinWin_OnCancelButton_Click);
                                            //    Win.Show();
                                            //}
                                            //else
                                            //{
                                            string msgText = msgConsumption + " Are You Sure \n  You Want To Freeze The Bill ?";
                                            MessageBoxControl.MessageBoxChildWindow msgWD =
                                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                                            msgWD.OnMessageBoxClosed += (arg2) =>
                                            {
                                                if (arg2 == MessageBoxResult.Yes)
                                                {
                                                    PaymentWindow paymentWin = new PaymentWindow();
                                                    paymentWin.PatientCategoryID = ((clsBillVO)dgBillList.SelectedItem).PatientCategoryId; //this.PatientCategoryID;  //Ashish Z
                                                    paymentWin.TotalAmount = double.Parse(txtPayAmount.Text);
                                                    paymentWin.Initiate("Bill");
                                                    paymentWin.conID = ((clsBillVO)dgBillList.SelectedItem).ConcessionAuthorizedBy;
                                                    //...............................................
                                                    paymentWin.txtPayTotalAmount.Text = this.txtNetAmount.Text;
                                                    paymentWin.txtDiscAmt.Text = this.txtTotalConcession.Text;
                                                    paymentWin.txtPayableAmt.Text = this.txtPayAmount.Text;
                                                    paymentWin.PackageConcenssionAmt = PackageConcenssion;
                                                    //* Added by - Ajit Jadhav
                                                    //* Added Date - 13/10/2016
                                                    //* Comments - mandatorily capture Pan No 
                                                    if (!String.IsNullOrEmpty(PanNumber) && PanNumber != "NULL")
                                                    {
                                                        paymentWin.txtPanNo.Text = PanNumber;
                                                        paymentWin.txtPanNo.IsEnabled = false;
                                                    }

                                                    //***//-------------------------

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

                                                    if (ChargeList != null && ChargeList.Count > 0)
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
                                            //}
                                        }
                                    }
                                    #endregion Modify
                                }
                            }
                        };
                        Client.ProcessAsync(BizAction, null);
                        Client.CloseAsync();
                    }
                    else
                    {
                        #region Modify
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

                                if (((objUser.OpdBillingAmmount == 0 ? false : objUser.OpdBillingAmmount < Convert.ToDecimal(ApprovalConcenssion)) || (objUser.OpdBillingPercentage == 0 ? false : (((Convert.ToDecimal(ApprovalConcenssion) / Convert.ToDecimal(ApprovalTotal)) * 100)) > objUser.OpdBillingPercentage)) && ((clsBillVO)dgBillList.SelectedItem).IsApproved == false) // Change By Bhushanp 09032017
                                {

                                    MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Concession Exceeds The Authorised Amount,Are You Sure You Want To Send For Approval?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                    msgWD_1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWD_1_OnMessageBoxClosed);
                                    msgWD_1.Show();

                                }

                                else
                                {

                                    // Added By CDS In Case Of Donor Bill

                                    //if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 8 || ((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 9)
                                    //{
                                    //    DonorCoupleLinkedList Win = new DonorCoupleLinkedList();
                                    //    Win.DonorID = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                                    //    Win.DonorUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
                                    //    Win.DonorName = ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName + " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " + ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;

                                    //    Win.OnSaveButton_Click += new RoutedEventHandler(ModifyDonorLinWin_OnSaveButton_Click);
                                    //    Win.OnCancelButton_Click += new RoutedEventHandler(DonorLinWin_OnCancelButton_Click);
                                    //    Win.Show();
                                    //}

                                    //else
                                    //{
                                    string msgText = msgConsumption + " Are You Sure \n  You Want To Freeze The Bill ?";
                                    MessageBoxControl.MessageBoxChildWindow msgWD =
                                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                                    msgWD.OnMessageBoxClosed += (arg1) =>
                                    {
                                        if (arg1 == MessageBoxResult.Yes)
                                        {
                                            PaymentWindow paymentWin = new PaymentWindow();
                                            paymentWin.PatientCategoryID = ((clsBillVO)dgBillList.SelectedItem).PatientCategoryId; //this.PatientCategoryID;  //Ashish Z
                                            paymentWin.TotalAmount = double.Parse(txtPayAmount.Text);
                                            paymentWin.Initiate("Bill");
                                            //By Parmeshwar.........................
                                            paymentWin.conID = ((clsBillVO)dgBillList.SelectedItem).ConcessionAuthorizedBy;
                                            //...............................................
                                            paymentWin.txtPayTotalAmount.Text = this.txtNetAmount.Text;
                                            paymentWin.txtDiscAmt.Text = this.txtTotalConcession.Text;
                                            paymentWin.txtPayableAmt.Text = this.txtPayAmount.Text;
                                            paymentWin.PackageConcenssionAmt = PackageConcenssion;

                                            //* Added by - Ajit Jadhav
                                            //* Added Date - 13/10/2016
                                            //* Comments - mandatorily capture Pan No 
                                            if (!String.IsNullOrEmpty(PanNumber) && PanNumber != "NULL")
                                            {
                                                paymentWin.txtPanNo.Text = PanNumber;
                                                paymentWin.txtPanNo.IsEnabled = false;
                                            }

                                            //***//-------------------------

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

                                            //PaymentWindow paymentWin = new PaymentWindow();
                                            //paymentWin.TotalAmount = double.Parse(txtNetAmount.Text);
                                            //paymentWin.Initiate("Bill");

                                            //paymentWin.txtPayTotalAmount.Text = this.txtTotalBill.Text;
                                            //paymentWin.txtDiscAmt.Text = this.txtTotalConcession.Text;
                                            //paymentWin.txtPayableAmt.Text = this.txtNetAmount.Text;

                                            if (ChargeList != null && ChargeList.Count > 0)
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
                                    //} //
                                }
                            }
                            else
                            {
                                string msgText = msgConsumption + " Are You Sure \n  You Want To Freeze The Bill ?";
                                MessageBoxControl.MessageBoxChildWindow msgWD =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                                msgWD.OnMessageBoxClosed += (arg2) =>
                                {
                                    if (arg2 == MessageBoxResult.Yes)
                                    {
                                        PaymentWindow paymentWin = new PaymentWindow();
                                        paymentWin.PatientCategoryID = ((clsBillVO)dgBillList.SelectedItem).PatientCategoryId; //this.PatientCategoryID;  //Ashish Z
                                        paymentWin.TotalAmount = double.Parse(txtPayAmount.Text);
                                        paymentWin.Initiate("Bill");
                                        //By Parmeshwar.........................
                                        paymentWin.conID = ((clsBillVO)dgBillList.SelectedItem).ConcessionAuthorizedBy;
                                        //...............................................
                                        paymentWin.txtPayTotalAmount.Text = this.txtNetAmount.Text;
                                        paymentWin.txtDiscAmt.Text = this.txtTotalConcession.Text;
                                        paymentWin.txtPayableAmt.Text = this.txtPayAmount.Text;
                                        paymentWin.PackageConcenssionAmt = PackageConcenssion;

                                        //* Added by - Ajit Jadhav
                                        //* Added Date - 13/10/2016
                                        //* Comments - mandatorily capture Pan No 
                                        if (!String.IsNullOrEmpty(PanNumber) && PanNumber != "NULL")
                                        {
                                            paymentWin.txtPanNo.Text = PanNumber;
                                            paymentWin.txtPanNo.IsEnabled = false;
                                        }

                                        //***//-------------------------

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

                                        //PaymentWindow paymentWin = new PaymentWindow();
                                        //paymentWin.TotalAmount = double.Parse(txtNetAmount.Text);
                                        //paymentWin.Initiate("Bill");

                                        //paymentWin.txtPayTotalAmount.Text = this.txtTotalBill.Text;
                                        //paymentWin.txtDiscAmt.Text = this.txtTotalConcession.Text;
                                        //paymentWin.txtPayableAmt.Text = this.txtNetAmount.Text;

                                        if (ChargeList != null && ChargeList.Count > 0)
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
                        #endregion Modify
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
                        //MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                        //new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient bill is saved as Credit.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                        //msgWD_1.Show();

                        SaveBill(null, false);
                    }
                }
                //if (chkFreezBill.IsChecked == true)
                //{
                //    PaymentWindow paymentWin = new PaymentWindow();
                //    paymentWin.TotalAmount = double.Parse(txtNetAmount.Text);
                //    paymentWin.Initiate("Bill");

                //    paymentWin.txtPayTotalAmount.Text = this.txtTotalBill.Text;
                //    paymentWin.txtDiscAmt.Text = this.txtTotalConcession.Text;
                //    paymentWin.txtPayableAmt.Text = this.txtNetAmount.Text;
                //    paymentWin.OnSaveButton_Click += new RoutedEventHandler(paymentWin_OnSaveButton_Click);
                //    paymentWin.Show();
                //}
                //else
                //{
                //    SaveBill(null, false);
                //}
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
                //if (PatAdvanceAmt < PatPaybleAmt)
                //{
                //    MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                //    new MessageBoxControl.MessageBoxChildWindow("Palash", "Bill amount is greater than advance amount are you sure you want to continue?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                //    msgWD_1.OnMessageBoxClosed += (arg_1) =>
                //    {
                //        if (arg_1 == MessageBoxResult.Yes)
                //        {
                //            FreezBill();
                //        }
                //        else
                //        {
                //            InitialiseForm();
                //            ((clsBillVO)dgBillList.SelectedItem).IsFreezed = false;
                //        }
                //    }; msgWD_1.Show();
                //}
                //else
                //{
                //    FreezBill();
                //}


                FillChargeListForFrontGrid(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, false, ((clsBillVO)dgBillList.SelectedItem).Opd_Ipd_External_Id, ((clsBillVO)dgBillList.SelectedItem).Opd_Ipd_External_UnitId, false);

                //if ((double)objUser.OpdBillingAmmount < ApprovalConcenssionFrontGrid && ((clsBillVO)dgBillList.SelectedItem).IsRequestSend == false)
                //{
                //    MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                // new MessageBoxControl.MessageBoxChildWindow("Palash", "Concenssion exceeds authorised amount, approval is needed", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                //    msgWD_1.Show();
                //    InitialiseForm();
                //    ((clsBillVO)dgBillList.SelectedItem).IsFreezed = false;

                //}
                //else if (((clsBillVO)dgBillList.SelectedItem).IsRequestSend == true && ((clsBillVO)dgBillList.SelectedItem).IsApproved == false)
                //{
                //    MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                // new MessageBoxControl.MessageBoxChildWindow("Palash", "Request is already Send", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                //    msgWD_1.Show();
                //    InitialiseForm();
                //    ((clsBillVO)dgBillList.SelectedItem).IsFreezed = false;

                //}
                //else
                //{
                //    FreezBill();
                //}
            }
        }

        private void FreezBill()
        {
            InitialiseForm();
            //* Commented by - Ajit Jadhav
            //* Added Date - 13/10/2016
            //* Comments - get pan number
            PanNumber = ((IApplicationConfiguration)App.Current).SelectedPatient.PanNumber;
            cmbConcessionReason.SelectedValue = ((clsBillVO)dgBillList.SelectedItem).ConcessionReasonId;
            //***//--------------------

            IsFromViewClick = true;     // For Package New Changes Added on 21062018

            SelectedBill = new clsBillVO();
            SelectedBill = (clsBillVO)dgBillList.SelectedItem;
            //Added By CDS 
            //FillChargeList(SelectedBill.ID, SelectedBill.UnitID, false, SelectedBill.Opd_Ipd_External_Id);
            if (SelectedBill.ConcessionReasonId != null)
            {
                ((MasterListItem)cmbConcessionReason.SelectedItem).ID = SelectedBill.ConcessionReasonId;
            }
            FillChargeList_New(SelectedBill.ID, SelectedBill.UnitID, false, SelectedBill.Opd_Ipd_External_Id, SelectedBill.Opd_Ipd_External_UnitId, false);// Change Function Name by Bhushanp 29052017
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
            //            paymentWin.PatientCategoryID = SelectedBill.PatientCategoryId;//this.PatientCategoryID;  //Ashish Z
            //            if (!(txtPayAmount.Text == null) && !(txtPayAmount.Text.Trim().Length == 0))
            //                paymentWin.TotalAmount = double.Parse(txtPayAmount.Text);
            //            paymentWin.Initiate("Bill");
            //            paymentWin.txtPayTotalAmount.Text = this.txtNetAmount.Text;
            //            paymentWin.txtDiscAmt.Text = this.txtTotalConcession.Text;
            //            paymentWin.txtPayableAmt.Text = this.txtPayAmount.Text;

            //            //* Added by - Ajit Jadhav
            //            //* Added Date - 13/10/2016
            //            //* Comments - mandatorily capture Pan No 
            //            if (!String.IsNullOrEmpty(PanNumber) && PanNumber != "NULL")
            //            {
            //                paymentWin.txtPanNo.Text = PanNumber;
            //                paymentWin.txtPanNo.IsEnabled = false;
            //            }

            //            //***//-------------------------

            //            //Added By Parmeshwar
            //            paymentWin.conID = ((clsBillVO)dgBillList.SelectedItem).ConcessionAuthorizedBy;

            //            //Added By CDS 
            //            paymentWin.PackageConcenssionAmt = PackageConcenssion;
            //            if ((Convert.ToDouble(this.txtTotalConcession.Text) - PackageConcenssion) > 0.0)
            //            {
            //                paymentWin.ConcessionFromPlan = false;
            //            }
            //            else
            //            {
            //                paymentWin.ConcessionFromPlan = true;
            //            }
            //            //Added By CDS 

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

        //comment on 17july for not printing concession report
        //private void PrintBill(long iBillId, long iUnitID)
        //{
        //    double ConnectionAmount = 0;
        //    if (dgBillList.SelectedItem != null)
        //        ConnectionAmount = ((clsBillVO)dgBillList.SelectedItem).TotalConcessionAmount;

        //    if ((ConnectionAmount > 0) || (TotalConcession > 0))
        //    {
        //        if (((clsBillVO)dgBillList.SelectedItem).BalanceAmountNonSelf == 0 && ((clsBillVO)dgBillList.SelectedItem).BalanceAmountSelf == 0)
        //        {
        //            if (iBillId > 0)
        //            {
        //                long UnitID = iUnitID;
        //                string URL = "../Reports/OPD/ClinicalBillConcessionReport.aspx?BillID=" + iBillId + "&UnitID=" + UnitID;
        //                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        //            }
        //        }
        //    }
        //    else if (iBillId > 0)
        //    {
        //        if (dgBillList.SelectedItem != null)
        //        {
        //            if (((clsBillVO)dgBillList.SelectedItem).BalanceAmountNonSelf == 0 && ((clsBillVO)dgBillList.SelectedItem).BalanceAmountSelf == 0)
        //            {
        //                long UnitID = iUnitID;
        //                string URL = "../Reports/OPD/ClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID;
        //                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        //            }
        //        }
        //        else
        //        {
        //            //long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
        //            long UnitID = iUnitID;
        //            string URL = "../Reports/OPD/ClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID;
        //            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        //        }
        //    }

        //}

        //end

        private void PrintBill(long iBillId, long iUnitID, long PrintID)
        {
            //////////////////OLD code Commented//////////////////
            //if (iBillId > 0)
            //{
            //    //long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //    long UnitID = iUnitID;
            //    string URL = "../Reports/OPD/ClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&PrintFomatID=" + PrintID;
            //    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            //}
            ///////////////////END///////////////////////////////////

            #region added by Prashant Channe to read reports config on 27thNov2019
            string StrConfigReportsDir = ((IApplicationConfiguration)App.Current).CurrentUser.ReportsFolder;
            string URL = null;
            #endregion

            if (iBillId > 0 && rdbInline.IsChecked == false)
            {
                //long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                long UnitID = iUnitID;

                //Modified by Prashant Channe on 27Nov2019 for ReportsFolder configuration
                if (!string.IsNullOrEmpty(StrConfigReportsDir))
                {
                    URL = "../" + StrConfigReportsDir + "/OPD/ClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&PrintFomatID=" + PrintID;
                }
                else
                {
                    URL = "../Reports/OPD/ClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&PrintFomatID=" + PrintID;
                }

                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }

            else if (iBillId > 0 && rdbInline.IsChecked == true)
            {
                long UnitID = iUnitID;
                long isInline = 1;

                ////Modified by Prashant Channe on 27Nov2019 for ReportsFolder configuration
                //if (!string.IsNullOrEmpty(StrConfigReportsDir))
                //{
                //    URL = "../" + StrConfigReportsDir + "/OPD/ClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&PrintFomatID=" + PrintID + "&isInline=" + isInline;
                //}
                //else
                //{
                    URL = "../Reports/OPD/ClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&PrintFomatID=" + PrintID + "&isInline=" + isInline;
                //}
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
                //  BillID = ((clsBillVO)dgBillList.SelectedItem).ID;
                if (SelectedBill.IsFreezed == true)
                {
                    //if(SelectedBill.VisitTypeID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyVisitTypeID)

                    if (((clsBillVO)dgBillList.SelectedItem).IsPackageServiceInclude == true)
                    {

                        if (SelectedBill.BillType == PalashDynamics.ValueObjects.BillTypes.Clinical)
                        {
                            // PrintBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, ((MasterListItem)cmbPrintFormat.SelectedItem).ID); //old code
                            string msgText = "Are You Sure  Want To Print Details Bill ?";
                            MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgWD.OnMessageBoxClosed += (arg) =>
                            {
                                if (arg == MessageBoxResult.Yes)
                                {
                                    rdbInline.IsChecked = false;
                                    PrintBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, ((MasterListItem)cmbPrintFormat.SelectedItem).ID);
                                }
                                else
                                {
                                    rdbInline.IsChecked = true;
                                    PrintBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, ((MasterListItem)cmbPrintFormat.SelectedItem).ID);
                                }
                            };
                            msgWD.Show();

                        }
                        else
                        {
                            if (SelectedBill.BillType == PalashDynamics.ValueObjects.BillTypes.Clinical)
                            {
                                PrintBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, ((MasterListItem)cmbPrintFormat.SelectedItem).ID);
                            }
                            else if (SelectedBill.BillType == PalashDynamics.ValueObjects.BillTypes.Pharmacy)
                            {
                                PrintPharmacyBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, ((MasterListItem)cmbPrintFormat.SelectedItem).ID);
                            }
                            else
                            {
                                PrintBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, ((MasterListItem)cmbPrintFormat.SelectedItem).ID);
                                PrintPharmacyBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, ((MasterListItem)cmbPrintFormat.SelectedItem).ID);
                            }
                        }
                    }


                    else
                    {
                        if (SelectedBill.BillType == PalashDynamics.ValueObjects.BillTypes.Clinical)
                        {
                            PrintBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, ((MasterListItem)cmbPrintFormat.SelectedItem).ID);
                        }
                        else if (SelectedBill.BillType == PalashDynamics.ValueObjects.BillTypes.Pharmacy)
                        {
                            PrintPharmacyBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, ((MasterListItem)cmbPrintFormat.SelectedItem).ID);
                        }
                        else
                        {
                            PrintBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, ((MasterListItem)cmbPrintFormat.SelectedItem).ID);
                            PrintPharmacyBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, ((MasterListItem)cmbPrintFormat.SelectedItem).ID);
                        }
                    }
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
                #region added by Prashant Channe to read reports config on 27thNov2019
                string StrConfigReportsDir = ((IApplicationConfiguration)App.Current).CurrentUser.ReportsFolder;
                string URL = null;
                #endregion

                //long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                long UnitID = IUnitID;
                //Modified by Prashant Channe on 27Nov2019 for ReportsFolder configuration
                if (!string.IsNullOrEmpty(StrConfigReportsDir))
                {
                    URL = "../" + StrConfigReportsDir + "/OPD/PharmacyBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&ReportID=" + 2 + "&PrintFomatID=" + PrintID;
                }
                else
                {
                    URL = "../Reports/OPD/PharmacyBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&ReportID=" + 2 + "&PrintFomatID=" + PrintID;
                }
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
            #region OLD Code Commented By CDS

            //if (((PrescriptionServicesForPatient)sender).DialogResult == true)
            //{
            //    List<clsServiceMasterVO> lServices = new List<clsServiceMasterVO>();
            //    lServices = ((PrescriptionServicesForPatient)sender).SelectedServices;
            //    AddCharges(lServices);
            //}

            #endregion

            if (((PrescriptionServicesForPatient)sender).DialogResult == true)  //if (((PrescriptionServicesForPatient)sender).DialogResult == true)
            {
                List<clsServiceMasterVO> lServices = new List<clsServiceMasterVO>();
                lServices = ((PrescriptionServicesForPatient)sender).SelectedServices;  //((PrescriptionServicesForPatient)sender).SelectedServices; 

                bool IsMultiSponser = false;
                bool IscomboPackage = false;    // added on 13082016

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

                }

                //if (IsMultiSponser == false)      // Commented on 13082018
                if (IsMultiSponser == false && IscomboPackage == false)        // modified on 13082018
                {

                    #region clsApplyPackageDiscountRateOnServiceBizActionVO II

                    clsApplyPackageDiscountRateOnServiceBizActionVO objApplyNewRate = new clsApplyPackageDiscountRateOnServiceBizActionVO();

                    objApplyNewRate.objApplyPackageDiscountRate = new List<clsApplyPackageDiscountRateOnServiceVO>();
                    objApplyNewRate.ipServiceList = new List<clsServiceMasterVO>();


                    objApplyNewRate.ipLoginUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    objApplyNewRate.ipPatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    objApplyNewRate.ipPatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;


                    objApplyNewRate.ipVisitID = VisitID;

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

                    // Commnted By CDS 
                    //if ((ServiceSearchForPackage)sender != null && ((ServiceSearchForPackage)sender).cmbPatientSource.SelectedItem != null && ((clsPatientSponsorVO)((ServiceSearchForPackage)sender).cmbPatientSource.SelectedItem).MemberRelationID > 0)
                    //    objApplyNewRate.MemberRelationID = ((clsPatientSponsorVO)((ServiceSearchForPackage)sender).cmbPatientSource.SelectedItem).MemberRelationID;
                    //else
                    //    objApplyNewRate.MemberRelationID = 2;

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
                                        // Commented By CDS 
                                        item.ConcessionPercent = 100;   //Convert.ToDecimal(item1.DiscountedPercentage);
                                        ConcessionFromPlan = true;
                                        // Commented By CDS 

                                        // For AND Condition Uptp Condtion Quantity Conditional Discount is given 
                                        //item.ConcessionAmount = 0;
                                        //item.ConcessionPercent = Convert.ToDecimal(item.ConditionalDiscount);
                                        //ConcessionFromPlan = true;
                                        //END
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
                                        //if (item.ID == item1.ServiceID && item.TariffID == item1.TariffID && item.ConditionID == 0)   // For Package New changes commented on 25062018
                                        if (item.ID == item1.ServiceID && item.TariffID == item1.TariffID && item.ConditionID == 0 && item.ProcessID == item1.ProcessID)    // For Package New changes Added on 25062018
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

                        if (IsMsgServiceConsumption)        // Package New Changes for Process Added on 24042018
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

                    #region clsApplyPackageDiscountRateOnServiceBizActionVO

                    //clsApplyPackageDiscountRateOnServiceBizActionVO objApplyNewRate = new clsApplyPackageDiscountRateOnServiceBizActionVO();

                    //objApplyNewRate.objApplyPackageDiscountRate = new List<clsApplyPackageDiscountRateOnServiceVO>();
                    //objApplyNewRate.ipServiceList = new List<clsServiceMasterVO>();


                    //objApplyNewRate.ipLoginUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    //objApplyNewRate.ipPatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    //objApplyNewRate.ipPatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;


                    //objApplyNewRate.ipVisitID = VisitID;

                    //objApplyNewRate.ipPatientGenderID = ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID;
                    //objApplyNewRate.ipPatientDateOfBirth = ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth;

                    //objApplyNewRate.ipServiceList = lServices;

                    //if ((PrescriptionServicesForPatient)sender != null && ((PrescriptionServicesForPatient)sender).cmbPatientSource.SelectedItem != null && ((clsPatientSponsorVO)((PrescriptionServicesForPatient)sender).cmbPatientSource.SelectedItem).MemberRelationID > 0)
                    //    objApplyNewRate.MemberRelationID = ((clsPatientSponsorVO)((PrescriptionServicesForPatient)sender).cmbPatientSource.SelectedItem).MemberRelationID;

                    //objApplyNewRate.SponsorID = ((MasterListItem)((PrescriptionServicesForPatient)sender).cmbTariff.SelectedItem).SponsorID;
                    //objApplyNewRate.SponsorUnitID = ((MasterListItem)((PrescriptionServicesForPatient)sender).cmbTariff.SelectedItem).SponsorUnitID;

                    //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    //client.ProcessCompleted += (s, arg) =>
                    //{
                    //    if (arg.Error == null && arg.Result != null)
                    //    {
                    //        objApplyNewRate = (clsApplyPackageDiscountRateOnServiceBizActionVO)arg.Result;


                    //        UnavailableItemStockService = new List<clsUnavailableItemStockServiceId>();

                    //        foreach (clsServiceMasterVO item in lServices)
                    //        {
                    //            if (item.ConditionID > 0)
                    //            {
                    //                if (item.ConditionalQuantity > item.ConditionalUsedQuantity && item.ConditionType == "AND" && item.IsAgeApplicable == true && item.ServiceMemberRelationID > 0)  //if (item.ConditionalQuantity > item.ConditionalUsedQuantity)
                    //                {
                    //                    item.ConcessionPercent = 100;
                    //                    ConcessionFromPlan = true;
                    //                }
                    //                else if (item.ConditionalQuantity > (item.ConditionalUsedQuantity + item.MainServiceUsedQuantity) && item.ConditionType == "OR" && item.IsAgeApplicable == true && item.ServiceMemberRelationID > 0)  //if (item.ConditionalQuantity > item.ConditionalUsedQuantity)
                    //                {
                    //                    item.ConcessionPercent = 100;
                    //                    ConcessionFromPlan = true;
                    //                }
                    //                else
                    //                {
                    //                    if ((item.IsAgeApplicable == true && item.ServiceMemberRelationID > 0))
                    //                    {
                    //                        item.ConcessionAmount = 0;
                    //                        item.ConcessionPercent = Convert.ToDecimal(item.ConditionalDiscount);
                    //                        ConcessionFromPlan = true;
                    //                    }

                    //                    if ((item.IsAgeApplicable == false || item.ServiceMemberRelationID == 0))  //item1.ActualQuantity > 0 &&
                    //                    {
                    //                        item.ConcessionPercent = 0;
                    //                        item.ConcessionAmount = 0;
                    //                    }

                    //                }

                    //            }
                    //            else
                    //            {
                    //                foreach (clsApplyPackageDiscountRateOnServiceVO item1 in objApplyNewRate.objApplyPackageDiscountRate)
                    //                {
                    //                    if (item.ID == item1.ServiceID && item.TariffID == item1.TariffID && item.ConditionID == 0)
                    //                    {

                    //                        if (item1.IsApplyOn_Rate_Percentage == 1 || item1.IsApplyOn_Rate_Percentage == 3 && item.ConditionID == 0)
                    //                        {
                    //                            item.Rate = Convert.ToDecimal(item1.DiscountedRate);

                    //                            if ((item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0) && item1.TariffID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID)
                    //                            {
                    //                                item.ConcessionAmount = 0;
                    //                                item.ConcessionPercent = Convert.ToDecimal(item1.ConcessionPercentage);
                    //                            }

                    //                            if (item1.IsDiscountOnQuantity == true || item1.ServiceMemberRelationID == 0 || item1.IsAgeApplicable == false)
                    //                            {
                    //                                item.ConcessionPercent = 0;
                    //                                item.ConcessionAmount = 0;
                    //                            }
                    //                        }


                    //                        if (item1.IsApplyOn_Rate_Percentage == 2 || item1.IsApplyOn_Rate_Percentage == 3 && item.ConditionID == 0)
                    //                        {
                    //                            if (item1.IsDiscountOnQuantity == false)
                    //                            {
                    //                                if (item.PackageServiceConditionID == 0)
                    //                                {
                    //                                    if (item1.ActualQuantity > item1.UsedQuantity && item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0)
                    //                                    {
                    //                                        item.ConcessionPercent = Convert.ToDecimal(item1.DiscountedPercentage);

                    //                                        ConcessionFromPlan = true; //Added by priyanka-to set validation on concession
                    //                                    }
                    //                                    else
                    //                                    {
                    //                                        if ((item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0) && item1.TariffID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID)
                    //                                        {
                    //                                            item.ConcessionAmount = 0;
                    //                                            item.ConcessionPercent = Convert.ToDecimal(item1.ConcessionPercentage);
                    //                                        }

                    //                                        if ((item1.IsAgeApplicable == false || item1.ServiceMemberRelationID == 0))  //item1.ActualQuantity > 0 &&
                    //                                        {
                    //                                            item.ConcessionPercent = 0;
                    //                                            item.ConcessionAmount = 0;
                    //                                        }
                    //                                    }
                    //                                }
                    //                                else if (item.PackageServiceConditionID > 0)
                    //                                {
                    //                                    if (item1.ActualQuantity > (item1.UsedQuantity + item.TotalORUsedQuantity) && item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0)
                    //                                    {
                    //                                        item.ConcessionPercent = Convert.ToDecimal(item1.DiscountedPercentage);

                    //                                        ConcessionFromPlan = true; //Added by priyanka-to set validation on concession
                    //                                    }
                    //                                    else
                    //                                    {
                    //                                        if ((item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0) && item1.TariffID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID)
                    //                                        {
                    //                                            item.ConcessionAmount = 0;
                    //                                            item.ConcessionPercent = Convert.ToDecimal(item1.ConcessionPercentage);
                    //                                        }

                    //                                        if ((item1.IsAgeApplicable == false || item1.ServiceMemberRelationID == 0))  //item1.ActualQuantity > 0 && 
                    //                                        {
                    //                                            item.ConcessionPercent = 0;
                    //                                            item.ConcessionAmount = 0;
                    //                                        }
                    //                                    }
                    //                                }

                    //                            }
                    //                            else
                    //                            {
                    //                                if (item1.ActualQuantity > (item1.UsedQuantity + item.TotalORUsedQuantity) && item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0)
                    //                                {
                    //                                    if ((item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0) && item1.TariffID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID)
                    //                                    {
                    //                                        item.ConcessionAmount = 0;
                    //                                        item.ConcessionPercent = Convert.ToDecimal(item1.ConcessionPercentage);
                    //                                    }
                    //                                }
                    //                                else
                    //                                {
                    //                                    item.ConcessionPercent = 0;
                    //                                    item.ConcessionAmount = 0;
                    //                                }
                    //                            }
                    //                        }

                    //                        if (item.IsMarkUp == true && item1.IsServiceItemStockAvailable != true)
                    //                        {
                    //                            UnavailableItemStockService.Add(new clsUnavailableItemStockServiceId { ServiceID = item.ServiceID });
                    //                        }

                    //                    }
                    //                }
                    //            }
                    //        }

                    //        StringBuilder sbServiceName = new StringBuilder();

                    //        foreach (clsUnavailableItemStockServiceId item in UnavailableItemStockService)
                    //        {
                    //            clsServiceMasterVO obj = new clsServiceMasterVO();
                    //            obj = (clsServiceMasterVO)lServices.First(z => z.ServiceID == item.ServiceID && z.IsMarkUp == true);

                    //            sbServiceName.Append(obj.ServiceName + ",");

                    //            lServices.Remove(obj);
                    //        }

                    //        if (sbServiceName.Length > 0)
                    //        {
                    //            MessageBoxControl.MessageBoxChildWindow msgWD =
                    //                            new MessageBoxControl.MessageBoxChildWindow("", "Items are not available for Service : " + sbServiceName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    //            msgWD.OnMessageBoxClosed += (re) =>
                    //            {
                    //                if (re == MessageBoxResult.OK)
                    //                {

                    //                    AddCharges(lServices);

                    //                }
                    //            };
                    //            msgWD.Show();
                    //        }
                    //        else
                    //        {
                    //            AddCharges(lServices);

                    //        }

                    //    }
                    //};
                    //client.ProcessAsync(objApplyNewRate, ((IApplicationConfiguration)App.Current).CurrentUser);
                    //client.CloseAsync();

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
                    itemC.RateEditable = mServices[i].RateEditable;
                    itemC.MinRate = Convert.ToDouble(mServices[i].MinRate);
                    itemC.MaxRate = Convert.ToDouble(mServices[i].MaxRate);
                    itemC.Rate = Convert.ToDouble(mServices[i].Rate);
                    itemC.InitialRate = itemC.Rate;  // to maintain the Initial Rate of Service after changing Doctor Service Rate.
                    itemC.TariffId = mServices[i].TariffID;
                    itemC.isPackageService = mServices[i].IsPackage;
                    itemC.IsPrescribedService = mServices[i].IsPrescribedService;

                    itemC.TotalAmount = itemC.Rate * itemC.Quantity;

                    // Added By CDS                     
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
            // fillDoctor();
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
                    //rdbAgainstServices.IsEnabled = rdbAgainstBill.IsEnabled = true; Commented By Bhushanp 19052017
                    rdbInline.IsEnabled = true;
                    break;
                case false:
                    rdbAgainstServices.IsEnabled = rdbAgainstBill.IsEnabled = false;
                    rdbAgainstBill.IsChecked = true;
                    rdbInline.IsEnabled = false;
                    break;
                default:
                    rdbAgainstServices.IsEnabled = rdbAgainstBill.IsEnabled = false;
                    rdbAgainstBill.IsChecked = true;
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
                // FillChargeList(SelectedBill.ID, SelectedBill.UnitID, false, SelectedBill.Opd_Ipd_External_Id);

                string msgText = "Are You Sure \n You Want To Settle The Bill ?";
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (arg) =>
                {
                    if (arg == MessageBoxResult.Yes)
                    {
                        bool isValid = true;
                        //isValid = tr  // CheckValidations();
                        if (isValid)
                        {
                            //long BillID;
                            //SetCommandButtonState("Modify");
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

                            if (ChargeList != null && ChargeList.Count > 0)
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
                            //BizAction.Details.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyCostingDivisionID;   //Costing Divisions for Pharmacy Billing
                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                                BizAction.Details.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID; //Costing Divisions for Pharmacy Billing
                            else
                                BizAction.Details.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyCostingDivisionID;
                        }

                        if (BizAction.Details != null && SelectedBill.BillType == BillTypes.Clinical)
                        {
                            //BizAction.Details.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.ClinicalCostingDivisionID;   //Costing Divisions for Clinical Billing
                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                                BizAction.Details.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID; //Costing Divisions for Pharmacy Billing
                            else
                                BizAction.Details.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;
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

                                // Added By CDS
                                //  mybillPayDetails.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID; //Costing Divisions for Pharmacy Billing

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

            #region added by Prashant Channe to read reports config on 1stDec2019
            string StrConfigReportsDir = ((IApplicationConfiguration)App.Current).CurrentUser.ReportsFolder;
            string URL = null;
            #endregion

            if (iBillId > 0 && iPaymentID > 0)
            {
                long UnitID = iUnitID;
                //Modified by Prashant Channe to Call from Configured reports folder on 1stDec2019
                if (!string.IsNullOrEmpty(StrConfigReportsDir))
                {
                    URL = "../" + StrConfigReportsDir + "/OPD/SettleClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&PaymentID=" + iPaymentID;
                }
                else
                {
                    URL = "../Reports/OPD/SettleClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&PaymentID=" + iPaymentID;
                }
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
                lServices = ((PackageServiceSearchForPackage)sender).SelectedOtherServices.ToList();   //lServices = ((PackageServiceSearch)sender).SelectedOtherServices.ToList();

                #region clsApplyPackageDiscountRateOnServiceBizActionVO

                clsApplyPackageDiscountRateOnServiceBizActionVO objApplyNewRate = new clsApplyPackageDiscountRateOnServiceBizActionVO();

                objApplyNewRate.objApplyPackageDiscountRate = new List<clsApplyPackageDiscountRateOnServiceVO>();
                objApplyNewRate.ipServiceList = new List<clsServiceMasterVO>();


                objApplyNewRate.ipLoginUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                objApplyNewRate.ipPatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                objApplyNewRate.ipPatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;


                objApplyNewRate.ipVisitID = VisitID;

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

                            // PatientCatID = ((clsPatientSponsorVO)((PackageServiceSearchForPackage)sender).cmbPatientSource.SelectedItem).PatientCategoryID;

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
            if (objUser.OPDAuthLvtForConcessionID != null)
            {
                ApproveFlag += 1;
                if (ApproveFlag == 1)
                {
                    if (objUser.OPDAuthLvtForConcessionID == SelectedBill.LevelID)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Bill is already approved with " + objUser.AuthLevelForConcenssionOPD + " rights ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                        msgWindowUpdate.Show();
                        ApproveFlag = 0;
                    }
                    else if (objUser.OPDAuthLvtForConcessionID < SelectedBill.LevelID)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Bill is already approved with higher rights ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                        msgWindowUpdate.Show();
                        ApproveFlag = 0;
                    }
                    else if ((objUser.OpdBillingAmmount == 0 ? false : objUser.OpdBillingAmmount < Convert.ToDecimal(ApprovalConcenssion)) || (objUser.OpdBillingPercentage == 0 ? false : (((Convert.ToDecimal(ApprovalConcenssion) / Convert.ToDecimal(ApprovalTotal)) * 100)) > objUser.OpdBillingPercentage))
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Given concenssion amount exceeds authorised amount", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                        msgWindowUpdate.Show();
                        ApproveFlag = 0;
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to Approve the request with " + objUser.AuthLevelForConcenssionOPD + " rights", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
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

                    objBill.Opd_Ipd_External_Id = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;
                    objBill.Opd_Ipd_External_UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;

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
                    ////objBill.PatientCategoryId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID;
                    objBill.PatientCategoryId = ((IApplicationConfiguration)App.Current).SelectedPatient.NewPatientCategoryID;
                }


                if (PatientCompanyID > 0)
                {
                    objBill.CompanyId = Convert.ToInt64(PatientCompanyID);
                }
                else
                {
                    ////objBill.PatientCategoryId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID;
                    objBill.CompanyId = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
                }

                objBill.IsFreezed = pFreezBill;
                objBill.PaymentDetails = pPayDetails;

                // This IS valid In Health Spring So Commented 
                //if (!string.IsNullOrEmpty(txtGrossDiscount.Text))
                //    objBill.GrossDiscountPercentage = Convert.ToDouble(txtGrossDiscount.Text);

                //if (cmbGrossDiscountReason.SelectedItem != null)
                //    objBill.GrossDiscountReasonID = ((MasterListItem)cmbGrossDiscountReason.SelectedItem).ID;


                //if (!string.IsNullOrEmpty(txtTotalBill.Text))
                //    objBill.TotalBillAmount = Convert.ToDouble(txtTotalBill.Text);

                if (!string.IsNullOrEmpty(txtTotalBill.Text))
                    objBill.TotalBillAmount = Convert.ToDouble(txtTotalBill.Text);

                if (!string.IsNullOrEmpty(txtTotalConcession.Text))
                    objBill.TotalConcessionAmount = Convert.ToDouble(txtTotalConcession.Text);
                TotalConcession = objBill.TotalConcessionAmount;

                if (!string.IsNullOrEmpty(txtNetAmount.Text))
                    objBill.NetBillAmount = Convert.ToDouble(txtNetAmount.Text);

                //objBill.ConcessionReasonId

                objBill.TotalConcessionAmount = TotCon;

                TotalConcession = objBill.TotalConcessionAmount;

                objBill.NetBillAmount = TotAmt;

                if (ChargeList != null & ChargeList.Count > 0)
                {
                    objBill.ChargeDetails = ChargeList.ToList();

                    if (pFreezBill)
                    {
                        for (int i = 0; i < ChargeList.Count; i++)
                        {
                            clsChargeVO objCharge = new clsChargeVO();
                            objCharge = ChargeList[i];
                            // Added By CDS
                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                                objCharge.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                            else
                                objCharge.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;


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

                            //foreach (var item in FollowUpDetails)
                            //{
                            //    if (item.ServiceId == objCharge.ServiceId)
                            //    {
                            //        if (FollowUpDetails.Count > 0)
                            //        {
                            //            objBill.FollowUpDetails.Add(item);
                            //        }
                            //    }
                            //}
                        }
                    }
                    else
                    {
                        for (int i = 0; i < ChargeList.Count; i++)
                        {
                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                                ChargeList[i].CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                            else
                                ChargeList[i].CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;
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
                        // Added By CDS
                        objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                    }
                    else if (objBill.ChargeDetails != null && objBill.ChargeDetails.Count > 0)
                    {
                        // Added By CDS
                        objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                    }
                    else if (objBill.PharmacyItems != null && objBill.PharmacyItems.Items != null && objBill.PharmacyItems.Items.Count > 0)
                    {
                        // Added By CDS
                        objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                    }
                }   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                else
                    objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;


                if (objBill.PaymentDetails != null)
                {
                    //objBill.PaymentDetails.CostingDivisionID = objBill.CostingDivisionID;
                    // Added By CDS
                    objBill.PaymentDetails.CostingDivisionID = objBill.CostingDivisionID;
                }
                objBill.ConcessionRemark = Convert.ToString(txtApprovalRemark.Text.Trim());
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



                //For Request
                BizAction.IsForApproval = true;
                if (BizAction.ApprovalChargeList == null)
                    BizAction.ApprovalChargeList = new List<clsChargeVO>();
                BizAction.ApprovalRequestID = SelectedBill.ApprovalRequestID;
                BizAction.ApprovalRequestUnitID = SelectedBill.ApprovalRequestUnitID;
                BizAction.ApprovedByID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                BizAction.ApprovedByName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.Name;
                //BizAction.ApprovedByName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UserName;
                if (objUser.OPDAuthLvtForConcessionID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForConcessionID)
                    BizAction.IsApproved = true;
                else
                    BizAction.IsApproved = false;
                BizAction.LevelID = objUser.OPDAuthLvtForConcessionID;
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
                        if (objUser.OPDAuthLvtForConcessionID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForConcessionID) //if (objUser.OpdAuthLvl == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForRefundID)
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
                //WIndicator.Show();
                clsGetDoctorServiceLinkingByCategoryAndServiceBizActionVO BizActionVO = new clsGetDoctorServiceLinkingByCategoryAndServiceBizActionVO();
                BizActionVO.ServiceMasterDetails = new clsServiceMasterVO();


                BizActionVO.ServiceMasterDetails.DoctorID = ((clsChargeVO)e.Row.DataContext).DoctorId;
                BizActionVO.ServiceMasterDetails.ServiceID = ((clsChargeVO)e.Row.DataContext).ServiceId;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    BizActionVO.ServiceMasterDetails = new clsServiceMasterVO();
                    //WIndicator.Close();
                    if (arg.Error == null && arg.Result != null)
                    {
                        //double tempRate = ((clsChargeVO)e.Row.DataContext).Rate;
                        if (Convert.ToDouble(((clsGetDoctorServiceLinkingByCategoryAndServiceBizActionVO)arg.Result).ServiceMasterDetails.Rate) > 0)
                        {
                            ((clsChargeVO)e.Row.DataContext).Rate = Convert.ToDouble(((clsGetDoctorServiceLinkingByCategoryAndServiceBizActionVO)arg.Result).ServiceMasterDetails.Rate);
                        }
                        else
                        {
                            if (((clsChargeVO)e.Row.DataContext).ID == 0)
                                ((clsChargeVO)e.Row.DataContext).Rate = ((clsChargeVO)e.Row.DataContext).InitialRate;

                        }
                        CalculateClinicalSummary();
                    }
                };
                client.ProcessAsync(BizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            #endregion

            // Added By CDS
            for (int i = 0; i < dgCharges.Columns.Count(); i++)
            {
                if (((clsChargeVO)e.Row.DataContext).PackageID != null)
                {
                    if (((clsChargeVO)e.Row.DataContext).PackageID > 0 && ((clsChargeVO)e.Row.DataContext).isPackageService == false)
                    {
                        if (i == 5 || i == 6)
                        {
                            // dgCharges.Columns[i].IsReadOnly = true;

                            //DataGridCell cell = dgCharges.Columns[i].GetCellContent(e.Row) as DataGridCell;
                            //cell.IsEnabled = false;
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
            // Added By CDS
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




        //private void CloseVisit(long iVisitID, long iPatientID, long iPatientUnitID)
        //{
        //    clsCloseVisitAfterBillBizActionVO BizAction = new clsCloseVisitAfterBillBizActionVO();
        //    BizAction.VisitID = iVisitID;
        //    BizAction.PatientID = iPatientID;
        //    BizAction.PatientUnitID = iPatientUnitID;
        //    BizAction.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null && arg.Result != null)
        //        {
        //        }
        //    };
        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        //}



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
                    Indicatior.Close();
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

                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {

            }
            finally
            {
                //Indicatior.Close();
            }
        }

        double ApprovalConcenssionFrontGrid, ApprovalTotalFrontGrid;
        private void CalculateClinicalSummaryForFrontGrid()
        {

            ApprovalConcenssionFrontGrid = ApprovalTotalFrontGrid = 0;

            for (int i = 0; i < ChargeListForFrontGrid.Count; i++)
            {

                //By Anjali.........................
                if (ChargeListForFrontGrid[i].PackageID == 0 || ChargeListForFrontGrid[i].isPackageService == true)
                {
                    ApprovalConcenssionFrontGrid += ChargeListForFrontGrid[i].Concession;
                    ApprovalTotalFrontGrid += ChargeListForFrontGrid[i].TotalAmount;
                }
                //...................................

            }
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForConcessionID > 0)
            {

                if ((double)objUser.OpdBillingAmmount < ApprovalConcenssionFrontGrid && (((Convert.ToDecimal(ApprovalConcenssionFrontGrid) / Convert.ToDecimal(ApprovalTotalFrontGrid)) * 100) > objUser.OpdBillingPercentage) && ((clsBillVO)dgBillList.SelectedItem).IsRequestSend == false)
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
        // Added By CDS to Bind Service Rate With Doctor Category Wise 
        private void cmbDoctor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCharges.SelectedItem != null)
            {
                FrameworkElement result = GetParent((FrameworkElement)sender, typeof(DataGridCell));

                if (result != null)
                {
                    var comboBox = (AutoCompleteBox)sender;
                    Int64 DoctorID;
                    //string BatchCode;
                    //BatchCode = tb.Text;

                    if (comboBox.SelectedItem != null)
                    {
                        DoctorID = ((MasterListItem)comboBox.SelectedItem).ID;
                        //if (((clsChargeVO)e.Row.DataContext).DoctorId != null && ((clsChargeVO)e.Row.DataContext).DoctorId > 0 && ((clsChargeVO)e.Row.DataContext).PackageID == 0)
                        //if (((clsChargeVO)dgCharges.SelectedItem).DoctorId != null && ((clsChargeVO)dgCharges.SelectedItem).DoctorId > 0 && ((clsChargeVO)dgCharges.SelectedItem).PackageID == 0)
                        if (DoctorID != null && DoctorID > 0 && ((clsChargeVO)(result.DataContext)).PackageID == 0)
                        {
                            //WIndicator.Show();
                            clsGetDoctorServiceLinkingByCategoryAndServiceBizActionVO BizActionVO = new clsGetDoctorServiceLinkingByCategoryAndServiceBizActionVO();
                            BizActionVO.ServiceMasterDetails = new clsServiceMasterVO();

                            //var comboBox = (AutoCompleteBox)sender;
                            //Int64 ID;
                            ////string BatchCode;
                            ////BatchCode = tb.Text;
                            //ID = ((MasterListItem)comboBox.SelectedItem).ID;

                            BizActionVO.ServiceMasterDetails.DoctorID = DoctorID; //ID;
                            //BizActionVO.ServiceMasterDetails.ServiceID = ((clsChargeVO)dgCharges.SelectedItem).ServiceId;  
                            BizActionVO.ServiceMasterDetails.ServiceID = ((clsChargeVO)(result.DataContext)).ServiceId;

                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            client.ProcessCompleted += (s, arg) =>
                            {
                                BizActionVO.ServiceMasterDetails = new clsServiceMasterVO();
                                //WIndicator.Close();
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

        private void rdbDetail_Click(object sender, RoutedEventArgs e)
        {
            rdbDetail.IsChecked = true;
        }


        private void rdbInline_Click(object sender, RoutedEventArgs e)
        {
            rdbInline.IsChecked = true;
        }
        //END
        #endregion

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

        private void PrintAdvance(long PatientID, long PatientUnitID, long AdvanceID, long AdvanceUnitID, long PrintID)
        {
            if (AdvanceID > 0 || PatientID > 0)
            {
                //long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                long PUnitID = PatientUnitID == 0 ? ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId : PatientUnitID;
                long PID = PatientID;
                long AID = AdvanceID;
                long AUID = AdvanceUnitID;
                // ReportID =  1 for Advance Receipt          and                2 is for Advance Refund Receipt;.

                string URL = "../Reports/OPD/Patient_AdvanceRefund.aspx?PatientID=" + PID + "&PatientUnitID=" + PUnitID + "&AdvanceID=" + AID + "&AdvanceUnitID=" + AUID + "&ReportID=" + 1 + "&PrintFomatID=" + PrintID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
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
