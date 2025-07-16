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
using PalashDynamics.Pharmacy;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.Inventory.BarCode;
using CIMS;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS.Forms;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects.Administration.PatientSourceMaster;
using PalashDynamics.UserControls;
using PalashDynamics;
using System.Windows.Browser;
using System.Text;
using PalashDynamics.Pharmacy.ItemSearch;
using System.Reflection;
using OPDModule.ItemList;
using PalashDynamics.Pharmacy.Inventory;
using PalashDynamics.Controls;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using PalashDynamics.ValueObjects.Log;
using System.Diagnostics;
using System.Threading;




namespace OPDModule.Forms
{
    public partial class CounterSale : UserControl, IInitiateCIMS
    {
        #region Variable declaration
        bool IsPageLoded = false;
        public ObservableCollection<clsItemSalesDetailsVO> PharmacyItems { get; set; }
        clsBillVO SelectedBill { get; set; }
        public long VisitId { get; set; }
        public long PatientId { get; set; }
        public bool IsFreez { get; set; }
        public bool ISPrescription { get; set; } //***//
        WaitIndicator Indicatior = null;
        LogInfoBizActionVO LogBizAction = new LogInfoBizActionVO();
        public long PatientUnitId { get; set; }
        int ClickedFlag = 0;
        public bool IsStaff { get; set; }  //***//
        StringBuilder StrPrescriptionDetailsID;//***//
        public bool AgainstDonor = false;
        public long LinkPatientID;
        public long LinkPatientUnitID;
        public long LinkCompanyID;
        public long LinkPatientSourceID;
        public long LinkTariffID;
        public long PatientRegType { get; set; }
        long CashCounterID = 0;
        String CashCounterName = "";



        //added by rohinee dated 23/9/2016 for audit trail
        bool IsAuditTrail = false;
        int lineNumber = 0;
        List<LogInfo> LogInfoList = new List<LogInfo>();  // For the Activity Log List
        LogInfo LogInformation;
        Guid objGUID;

        bool IsCSControlEnable = false;     // Use to Enable Item Selection control on Counter Sale Screen

        bool IsPatientSelect = false;   // addede on 07082018 to set values on dropdown selection changed
        List<clsPatientSponsorVO> PatientSponsorDetails = new List<clsPatientSponsorVO>();      // addede on 08082018 to check selected sponsor is valid or not

        #endregion

        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "NEW":
                    if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
                    {
                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                        mElement.Text = " : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                            " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                            ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;
                    }
                    this.DataContext = new clsPatientVO()
                    {
                        GenderID = 0,
                        RelationID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfRelationID,
                    };
                    break;
                default:
                    break;
            }
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public CounterSale()
        {
            InitializeComponent();
            dgPharmacyItems.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgPharmacyItems_CellEditEnded);
            FillGender();

            txtFirstName.Focus();

            // LogInfoList = new List<LogInfo>();  // Reset the Activity Log List

            IsAuditTrail = ((IApplicationConfiguration)App.Current).CurrentUser.IsAuditTrail;// By rohinee For Enable/Disable Audit Trail

            IsCSControlEnable = ((IApplicationConfiguration)App.Current).CurrentUser.IsCSControlEnable;     // Use to Enable Item Selection control on Counter Sale Screen

        }

        private void CounterSale_Loaded(object sender, RoutedEventArgs e)
        {
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                  && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AllowClinicalTransaction == true)
                {
                    //do  nothing
                }
                else
                    CmdSave.IsEnabled = false;
            }

            if ((((IApplicationConfiguration)App.Current).ApplicationConfigurations).IsConcessionReadOnly == true) //***//
            {
                dgPharmacyItems.Columns[13].IsReadOnly = true;
            }
            else
            {
                dgPharmacyItems.Columns[13].IsReadOnly = false;
            }

            InitialiseForm();
            ChkValidation();

            FillPurchaseFrequencyUnit();
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
            {
                if (((IApplicationConfiguration)App.Current).SelectedPatient.IsSpouse != true)
                {
                    txtMRNo.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                    VisitId = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;
                    PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    searchPatient = true;
                    IsPatientSelect = true;             // addede on 07082018 to set values on dropdown selection changed
                    GetPatient();
                    FillPatientSponsorDetailsNew();     // addede on 08082018 to check selected sponsor is valid or not
                }
                else
                {
                    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                    ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");

                    string msgText = "Please Select Donor Patient";
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD.Show();
                }
            }

            FillPatientType();
            txtFirstName.Focus();
            txtMobileCountryCode.Text = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DefaultCountryCode;
            FillPrintFormat();
            long ClincID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            FillStores(ClincID);

            //FillCostingDivisions();  //Fill and Set Costing Divisions for Pharmacy Billing
            // Added by CDS

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
            {
                CashCounterID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
                CashCounterName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterName;
                // cmbCostingDivision.SelectedItem = CashCounterName.ToString();
            }
            else
            {
                CashCounterID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyCounterID;
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


                    cmbPrintFormat.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID;


                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception)
            {


                throw;
            }

        }
        List<clsStoreVO> objStoreList = new List<clsStoreVO>();
        private void FillStores(long pClinicID)
        {
            #region Commented By Bhushanp
            //clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            //BizAction.MasterTable = MasterTableNameList.M_Store;
            //BizAction.MasterList = new List<MasterListItem>();

            //if (pClinicID > 0)
            //{
            //    BizAction.Parent = new KeyValue { Value = "ClinicID", Key = pClinicID.ToString() };
            //}

            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //Client.ProcessCompleted += (s, e) =>
            //{
            //    if (e.Error == null && e.Result != null)
            //    {
            //        List<MasterListItem> objList = new List<MasterListItem>();


            //        objList.Add(new MasterListItem(0, "- Select -", true));
            //        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

            //        cmbStore.ItemsSource = null;
            //        cmbStore.ItemsSource = objList;

            //        cmbStore.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyStoreID;

            //        StoreID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyStoreID;

            //    }
            //};

            //Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //Client.CloseAsync();
            #endregion

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

                    NonQSAndUserDefinedStores.ToList().Insert(0, Default);

                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        if (result.ToList().Count > 0)
                        {
                            objStoreList = (List<clsStoreVO>)result.ToList();
                        }
                    }
                    else
                    {
                        if (NonQSAndUserDefinedStores.ToList().Count > 0)
                        {
                            objStoreList = (List<clsStoreVO>)NonQSAndUserDefinedStores.ToList();
                        }
                    }
                    List<MasterListItem> objList = new List<MasterListItem>();
                    foreach (clsStoreVO item in objStoreList)
                    {
                        objList.Add(new MasterListItem(item.StoreId, item.StoreName, true));
                    }

                    if (objList != null)
                    {
                        cmbStore.ItemsSource = null;
                        cmbStore.ItemsSource = objList;
                        cmbStore.SelectedItem = objList[0];
                        cmbStore.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyStoreID;
                        StoreID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyStoreID;
                    }
                    if (cmbStore.SelectedItem != null)
                    {
                        if (((MasterListItem)cmbStore.SelectedItem).ID == 0)
                        {
                            lnkAddItems.IsEnabled = false;
                            lnkAddItemsFromPrescription.IsEnabled = false;
                            cmbStore.TextBox.SetValidation("Store rights not assigned to this user");
                            cmbStore.TextBox.RaiseValidationError();
                            cmbStore.Focus();
                            MessageBoxControl.MessageBoxChildWindow msgWD =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Store rights not assigned to this user", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWD.Show();
                        }
                        else
                        {
                            lnkAddItems.IsEnabled = true;
                            lnkAddItemsFromPrescription.IsEnabled = true;
                            cmbStore.TextBox.ClearValidationError();
                        }
                    }
                }
            };
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        #endregion

        #region  FillCombobox
        private void FillGender()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_GenderMaster;
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
                    cmbGender.ItemsSource = null;
                    cmbGender.ItemsSource = objList;
                }

                if (this.DataContext != null)
                {
                    cmbGender.SelectedValue = ((clsPatientVO)this.DataContext).GenderID;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillDoctor()
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);

                    txtReferenceDoctor.ItemsSource = null;
                    txtReferenceDoctor.ItemsSource = objList;
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        #endregion


        #region Costing Divisions for Clinical & Pharmacy Billing

        private void FillCostingDivisions()
        {
            try
            {
                //Indicatior = new WaitIndicator();
                //Indicatior.Show();

                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_IVFCostingDivision;
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

                    cmbCostingDivision.ItemsSource = null;
                    cmbCostingDivision.ItemsSource = objList.DeepCopy();
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                    {
                        cmbCostingDivision.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
                    }
                    else
                    {
                        cmbCostingDivision.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyCounterID;
                    }

                    //Indicatior.Close();
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception)
            {

                // throw;
                Indicatior.Close();
                throw;
            }

        }

        # endregion

        #region Reset All Controls
        /// <summary>
        /// Purpose:For Clear UI
        /// </summary>
        private void InitialiseForm()
        {
            PharmacyItems = new ObservableCollection<clsItemSalesDetailsVO>();
            dgPharmacyItems.ItemsSource = PharmacyItems;
            dgPharmacyItems.UpdateLayout();

            txtPharmacyTotal.Text = "";
            txtPharmacyConcession.Text = "";
            txtPharmacyNetAmount.Text = "";
            //By Anjali...................
            txtRountOffAmount.Text = "";
            //..........................

            txtTotalBill.Text = "";
            txtTotalConcession.Text = "";
            txtNetAmount.Text = "";

            chkFreezBill.IsChecked = true;
            SelectedBill = null;

            StoreID = 0;

            if (IsCSControlEnable == true)    // Use to Enable Item Selection control on Counter Sale Screen
            {
                if (cmbStore.SelectedItem != null && ((MasterListItem)cmbStore.SelectedItem).ID > 0)
                {
                    StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                    //cmbStore.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyStoreID;
                }
                else
                {
                    cmbStore.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyStoreID;
                    StoreID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyStoreID;
                }
            }

            LogInfoList = new List<LogInfo>();  // Reset the Activity Log List

            IsPatientSelect = false;                                    // addede on 07082018 to set values on dropdown selection changed
            PatientSponsorDetails = new List<clsPatientSponsorVO>();    // addede on 08082018 to check selected sponsor is valid or not

            IsStaff = false;   //Added by Prashant Channe on 13thDecember2019

        }

        #endregion



        void dgPharmacyItems_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            try
            {
                objGUID = new Guid();

                long TransactionUOMID = 0;
                if (dgPharmacyItems.SelectedItem != null && ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SelectedUOM != null)
                {
                    TransactionUOMID = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SelectedUOM.ID;
                }
                else
                {
                    TransactionUOMID = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).TransactionUOMID;
                }

                if (e.Column.DisplayIndex == 6)
                {
                    if (((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).UOMConversionList == null || ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).UOMConversionList.Count == 0)
                    {
                        //DataGridColumn column = dgPharmacyItems.Columns[7];
                        //FrameworkElement fe = column.GetCellContent(e.Row);
                        //if (fe != null)
                        //{
                        //    //DataGridCell cell = (DataGridCell)result;
                        //    FillUOMConversions();
                        //}


                        //Log Write here   

                        //StackFrame stackFrame = new System.Diagnostics.StackTrace(1).GetFrame(1);
                        //string fileName = stackFrame.GetFileName();
                        //string methodName = stackFrame.GetMethod().ToString();
                        // lineNumber = stackFrame.GetFileLineNumber();

                        //lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();

                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " I : Item Quantity Changed : " //+ Convert.ToString(lineNumber)
                                                                    + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                                    + " , StoreID : " + Convert.ToString(StoreID) + " "
                                                                    + " , ItemID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ItemID) + " "
                                                                    + " , BatchID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BatchID) + " "
                                                                    + " , BatchCode : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BatchCode) + " "
                                                                    + " , ExpiryDate : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ExpiryDate) + " "
                                                                    + " , Transaction UOMID : " + Convert.ToString(TransactionUOMID) + " "
                                                                    + " , Transaction Quantity (InputTransactionQuantity) : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity) + " "
                                                                    + " , BaseConversionFactor : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseConversionFactor) + " "
                                                                    + " , Base Quantity (TransactionQuantity) : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseQuantity) + " "
                                                                    + " , Base UOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseUOMID) + " "
                                                                    + " , SUOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SUOMID) + " "
                                                                    + " , StockCF : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConversionFactor) + " "
                                                                    + " , Stocking Quantity : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity * ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConversionFactor) + " "
                                                                    + " , PUOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).PUOMID) + " "
                                                                    + " , ConcessionPercentage : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConcessionPercentage) + " "
                                                                    + " , ConcessionAmount : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConcessionAmount) + " "
                                                                    + " , AvailableStockInBase : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableStockInBase) + " ";

                            LogInfoList.Add(LogInformation);
                        }

                        //CallLogBizAction(LogBizAction);
                        ////

                        ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseQuantity = Convert.ToSingle(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity) * ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseConversionFactor;

                        //lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();

                        //Log Write here   

                        // lineNumber = stackFrame.GetFileLineNumber();
                        //lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " II : Line Number : " //+ Convert.ToString(lineNumber)
                                                                    + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                                    + " , StoreID : " + Convert.ToString(StoreID) + " "
                                                                    + " , ItemID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ItemID) + " "
                                                                    + " , BatchID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BatchID) + " "
                                                                    + " , BatchCode : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BatchCode) + " "
                                                                    + " , ExpiryDate : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ExpiryDate) + " "
                                                                    + " , Transaction UOMID : " + Convert.ToString(TransactionUOMID) + " "
                                                                    + " , Transaction Quantity (InputTransactionQuantity) : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity) + " "
                                                                    + " , BaseConversionFactor : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseConversionFactor) + " "
                                                                    + " , Base Quantity (TransactionQuantity) : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseQuantity) + " "
                                                                    + " , Base UOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseUOMID) + " "
                                                                    + " , SUOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SUOMID) + " "
                                                                    + " , StockCF : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConversionFactor) + " "
                                                                    + " , Stocking Quantity : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity * ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConversionFactor) + " "
                                                                    + " , PUOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).PUOMID) + " "
                                                                    + " , ConcessionPercentage : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConcessionPercentage) + " "
                                                                    + " , ConcessionAmount : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConcessionAmount) + " "
                                                                    + " , AvailableStockInBase : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableStockInBase) + " ";

                            LogInfoList.Add(LogInformation);
                        }
                        //CallLogBizAction(LogBizAction);
                        ////



                        if (((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity <= 0)
                        {
                            float availQty = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableStockInBase;

                            ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity = 1;
                            string msgText = "Quantity cannot be zero or less than zero";
                            ConversionsForAvailableStock();
                            MessageBoxControl.MessageBoxChildWindow msgWD =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWD.Show();
                        }
                        else
                            if (((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseQuantity > ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableStockInBase)
                            {
                                float availQty = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableStockInBase;

                                ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity = Convert.ToSingle(Math.Floor(Convert.ToDouble(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableStockInBase / ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseConversionFactor)));

                                //Log Write here   

                                // lineNumber = stackFrame.GetFileLineNumber();
                                if (IsAuditTrail)
                                {
                                    LogInformation = new LogInfo();
                                    LogInformation.guid = objGUID;
                                    LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                                    LogInformation.TimeStamp = DateTime.Now;
                                    LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                                    LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                                    LogInformation.Message = " III : Line Number : " //+ Convert.ToString(lineNumber)
                                                                            + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                                            + " , StoreID : " + Convert.ToString(StoreID) + " "
                                                                            + " , ItemID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ItemID) + " "
                                                                            + " , BatchID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BatchID) + " "
                                                                            + " , BatchCode : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BatchCode) + " "
                                                                            + " , ExpiryDate : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ExpiryDate) + " "
                                                                            + " , Transaction UOMID : " + Convert.ToString(TransactionUOMID) + " "
                                                                            + " , Transaction Quantity (InputTransactionQuantity) : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity) + " "
                                                                            + " , BaseConversionFactor : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseConversionFactor) + " "
                                                                            + " , Base Quantity (TransactionQuantity) : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseQuantity) + " "
                                                                            + " , Base UOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseUOMID) + " "
                                                                            + " , SUOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SUOMID) + " "
                                                                            + " , StockCF : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConversionFactor) + " "
                                                                            + " , Stocking Quantity : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity * ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConversionFactor) + " "
                                                                            + " , PUOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).PUOMID) + " "
                                                                            + " , ConcessionPercentage : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConcessionPercentage) + " "
                                                                            + " , ConcessionAmount : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConcessionAmount) + " "
                                                                            + " , AvailableStockInBase : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableStockInBase) + " ";

                                    LogInfoList.Add(LogInformation);
                                }
                                //CallLogBizAction(LogBizAction);
                                ////

                                string msgText = "Quantity Must Be Less Than Or Equal To Available Quantity ";
                                ConversionsForAvailableStock();
                                MessageBoxControl.MessageBoxChildWindow msgWD =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWD.Show();
                            }
                            else if (((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SelectedUOM != null && ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SelectedUOM.ID == ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseUOMID && (((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity % 1) != 0)
                            {
                                ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity = 1;

                                //Log Write here   

                                // lineNumber = stackFrame.GetFileLineNumber();
                                if (IsAuditTrail)
                                {
                                    LogInformation = new LogInfo();
                                    LogInformation.guid = objGUID;
                                    LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                                    LogInformation.TimeStamp = DateTime.Now;
                                    LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                                    LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                                    LogInformation.Message = " IV : Line Number : " //+ Convert.ToString(lineNumber)
                                                                            + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                                            + " , StoreID : " + Convert.ToString(StoreID) + " "
                                                                            + " , ItemID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ItemID) + " "
                                                                            + " , BatchID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BatchID) + " "
                                                                            + " , BatchCode : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BatchCode) + " "
                                                                            + " , ExpiryDate : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ExpiryDate) + " "
                                                                            + " , Transaction UOMID : " + Convert.ToString(TransactionUOMID) + " "
                                                                            + " , Transaction Quantity (InputTransactionQuantity) : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity) + " "
                                                                            + " , BaseConversionFactor : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseConversionFactor) + " "
                                                                            + " , Base Quantity (TransactionQuantity) : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseQuantity) + " "
                                                                            + " , Base UOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseUOMID) + " "
                                                                            + " , SUOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SUOMID) + " "
                                                                            + " , StockCF : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConversionFactor) + " "
                                                                            + " , Stocking Quantity : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity * ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConversionFactor) + " "
                                                                            + " , PUOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).PUOMID) + " "
                                                                            + " , ConcessionPercentage : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConcessionPercentage) + " "
                                                                            + " , ConcessionAmount : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConcessionAmount) + " "
                                                                            + " , AvailableStockInBase : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableStockInBase) + " ";

                                    LogInfoList.Add(LogInformation);
                                }
                                //CallLogBizAction(LogBizAction);
                                ////

                                string msgText = "Quantity Cannot be in fraction";
                                ConversionsForAvailableStock();
                                MessageBoxControl.MessageBoxChildWindow msgWD =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWD.Show();

                            }


                    }
                    else
                        if (((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SelectedUOM != null && ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SelectedUOM.ID > 0)
                        {
                            ////((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM = ((MasterListItem)cmbConversions.SelectedItem);

                            //////objConversion = CalculateConversionFactor(((MasterListItem)cmbConversions.SelectedItem).ID, ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseUOMID);
                            //CalculateConversionFactor(((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID, ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SUOMID);

                            // Function Parameters
                            // FromUOMID - Transaction UOM
                            // ToUOMID - Stocking UOM
                            CalculateConversionFactorCentral(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SelectedUOM.ID, ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SUOMID);

                            //Log Write here   

                            // lineNumber = stackFrame.GetFileLineNumber();
                            if (IsAuditTrail)
                            {
                                LogInformation = new LogInfo();
                                LogInformation.guid = objGUID;
                                LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                                LogInformation.TimeStamp = DateTime.Now;
                                LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                                LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                                LogInformation.Message = " V : Line Number : " //+ Convert.ToString(lineNumber)
                                                                        + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                                        + " , StoreID : " + Convert.ToString(StoreID) + " "
                                                                        + " , ItemID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ItemID) + " "
                                                                        + " , BatchID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BatchID) + " "
                                                                        + " , BatchCode : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BatchCode) + " "
                                                                        + " , ExpiryDate : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ExpiryDate) + " "
                                                                        + " , Transaction UOMID : " + Convert.ToString(TransactionUOMID) + " "
                                                                        + " , Transaction Quantity (InputTransactionQuantity) : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity) + " "
                                                                        + " , BaseConversionFactor : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseConversionFactor) + " "
                                                                        + " , Base Quantity (TransactionQuantity) : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseQuantity) + " "
                                                                        + " , Base UOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseUOMID) + " "
                                                                        + " , SUOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SUOMID) + " "
                                                                        + " , StockCF : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConversionFactor) + " "
                                                                        + " , Stocking Quantity : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity * ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConversionFactor) + " "
                                                                        + " , PUOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).PUOMID) + " "
                                                                        + " , ConcessionPercentage : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConcessionPercentage) + " "
                                                                        + " , ConcessionAmount : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConcessionAmount) + " "
                                                                        + " , AvailableStockInBase : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableStockInBase) + " ";

                                LogInfoList.Add(LogInformation);
                            }
                            //CallLogBizAction(LogBizAction);
                            ////

                        }
                        else
                        {
                            ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity = 0;
                            ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SingleQuantity = 0;

                            ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConversionFactor = 0;
                            ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseConversionFactor = 0;

                            ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).MRP = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).MainMRP;
                            ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).PurchaseRate = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).MainRate;

                            //Log Write here   

                            // lineNumber = stackFrame.GetFileLineNumber();
                            if (IsAuditTrail)
                            {
                                LogInformation = new LogInfo();
                                LogInformation.guid = objGUID;
                                LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                                LogInformation.TimeStamp = DateTime.Now;
                                LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                                LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                                LogInformation.Message = " VI : Line Number : " //+ Convert.ToString(lineNumber)
                                                                        + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                                        + " , StoreID : " + Convert.ToString(StoreID) + " "
                                                                        + " , ItemID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ItemID) + " "
                                                                        + " , BatchID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BatchID) + " "
                                                                        + " , BatchCode : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BatchCode) + " "
                                                                        + " , ExpiryDate : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ExpiryDate) + " "
                                                                        + " , Transaction UOMID : " + Convert.ToString(TransactionUOMID) + " "
                                                                        + " , Transaction Quantity (InputTransactionQuantity) : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity) + " "
                                                                        + " , BaseConversionFactor : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseConversionFactor) + " "
                                                                        + " , Base Quantity (TransactionQuantity) : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseQuantity) + " "
                                                                        + " , Base UOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseUOMID) + " "
                                                                        + " , SUOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SUOMID) + " "
                                                                        + " , StockCF : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConversionFactor) + " "
                                                                        + " , Stocking Quantity : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity * ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConversionFactor) + " "
                                                                        + " , PUOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).PUOMID) + " "
                                                                        + " , ConcessionPercentage : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConcessionPercentage) + " "
                                                                        + " , ConcessionAmount : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConcessionAmount) + " "
                                                                        + " , AvailableStockInBase : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableStockInBase) + " ";

                                LogInfoList.Add(LogInformation);
                            }
                            //CallLogBizAction(LogBizAction);
                            ////

                        }
                }
                else if (e.Column.DisplayIndex == 8)
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
                else if (e.Column.DisplayIndex == 10)
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
                //added by rohinee to maintain audit trail
                else if (e.Column.DisplayIndex == 13)
                {
                    //if (((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConcessionPercentage < ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).PurchaseRate)
                    //{
                    if (IsAuditTrail)
                    {
                        LogInformation = new LogInfo();
                        LogInformation.guid = objGUID;
                        LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        LogInformation.TimeStamp = DateTime.Now;
                        LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                        LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                        LogInformation.Message = " XVI :Item Concession Percentage Change : " //+ Convert.ToString(lineNumber)
                                                                + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                                + " , StoreID : " + Convert.ToString(StoreID) + " "
                                                                + " , ItemID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ItemID) + " "
                                                                + " , BatchID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BatchID) + " "
                                                                + " , BatchCode : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BatchCode) + " "
                                                                + " , ExpiryDate : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ExpiryDate) + " "
                                                                + " , ConcessionPercentage : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConcessionPercentage) + " ";

                        LogInfoList.Add(LogInformation);
                    }
                    //}
                }
                else if (e.Column.DisplayIndex == 14)
                {
                    if (IsAuditTrail)
                    {
                        LogInformation = new LogInfo();
                        LogInformation.guid = objGUID;
                        LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        LogInformation.TimeStamp = DateTime.Now;
                        LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                        LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                        LogInformation.Message = " XVII : Item Concession Amount Change : " //+ Convert.ToString(lineNumber)
                                                                + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                                + " , StoreID : " + Convert.ToString(StoreID) + " "
                                                                + " , ItemID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ItemID) + " "
                                                                + " , BatchID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BatchID) + " "
                                                                + " , BatchCode : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BatchCode) + " "
                                                                + " , ExpiryDate : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ExpiryDate) + " "
                                                                + " , ConcessionAmount : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConcessionAmount) + " ";

                        LogInfoList.Add(LogInformation);
                    }
                }
                CalculatePharmacySummary();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        private void CallLogBizAction(LogInfoBizActionVO LogBizAction)
        {
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {

            };
            Client.ProcessAsync(LogBizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }


        #region Get Pharmacy items
        /// <summary>
        /// Purpose:Showing pharmacy items list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkAddItems_Click(object sender, RoutedEventArgs e)
        {

            // Commented by AniketK on 1-Nov-2018 To skip validations while add items from other items option
            //isValid = ChkValidation();
            isValid = true;

            if (isValid == true)
            {
                ItemListNew ItemSearch = new ItemListNew();
                ItemSearch.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                ItemSearch.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
                //By Anjali...........................
                ItemSearch.IsFromCounterSale = true;
                //.....................................

                //* Added by - Ajit Jadhav
                //* Added Date - 13/9/2016
                //* Comments - Get Store ID
                StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                //***//----------------------
                ItemSearch.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID; //StoreID;
                if (StoreID == 0)
                    ItemSearch.AllowStoreSelection = true;
                else
                    ItemSearch.AllowStoreSelection = false;
                ItemSearch.OnSaveButton_Click += new RoutedEventHandler(ItemSearch_OnSaveButton_Click);
                ItemSearch.Show();
            }
            else
            {
                string msgText = "Please add patient details";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWD.Show();
            }

        }

        public long StoreID { get; set; }
        //   public long StoreID1 { get; set; }  //by umesh
        /// <summary>
        /// Purpose:Add pharmacy items to grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        double vatper;
        void ItemSearch_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ItemListNew Itemswin = (ItemListNew)sender;
            if (Itemswin.DialogResult == true)
            {
                if (Itemswin.ItemBatchList != null)
                {

                    StringBuilder strError = new StringBuilder();
                    StoreID = Itemswin.StoreID;

                    ///////////////////
                    #region Package New Changes Added on 26042018

                    //decimal ItemAmount = 0;

                    //decimal PackageConsumptionAmount = 0;
                    //decimal PharmacyConsumeAmount = 0;

                    //double TotalConsumption = 0;
                    //double PharmacyCompoRate = 0;

                    //if (cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)
                    //{
                    //    PackageConsumptionAmount = Convert.ToDecimal(((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageConsumptionAmount);
                    //    PharmacyConsumeAmount = Convert.ToDecimal(((MasterListItem)cmbApplicabelPackage.SelectedItem).PharmacyConsumeAmount);
                    //    PharmacyCompoRate = ((MasterListItem)cmbApplicabelPackage.SelectedItem).PharmacyFixedRate;
                    //}

                    #endregion

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

                        ///////////////////
                        #region Package New Changes Added on 26042018

                        //ItemAmount = 0;

                        //if (PharmacyItems != null && PharmacyItems.Count > 0)
                        //{
                        //    foreach (clsItemSalesDetailsVO itemSD in PharmacyItems)
                        //    {
                        //        ItemAmount += Convert.ToDecimal(itemSD.MRP) * Convert.ToDecimal(itemSD.Quantity);
                        //    }
                        //}

                        //ItemAmount += Convert.ToDecimal(item.MRP);

                        //TotalConsumption = Convert.ToDouble(ItemAmount + PackageConsumptionAmount + PharmacyConsumeAmount);


                        #endregion

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


                            if (cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)    // Package New Changes Added on 26042018
                            {
                                //if (TotalConsumption < PharmacyCompoRate)
                                //{
                                //    ObjAddItem.ConcessionPercentage = 100;
                                //}
                                //else
                                //{
                                //    ObjAddItem.ConcessionAmount = TotalConsumption - PharmacyCompoRate;
                                //}

                                ObjAddItem.PackageBillID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageBillID;
                                ObjAddItem.PackageBillUnitID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageBillUnitID;
                                ObjAddItem.PackageID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID;                              // Package New Changes Added on 03052018
                            }
                            //else
                            //{
                            //    ObjAddItem.ConcessionAmount = ObjAddItem.ConcessionAmount;
                            //}

                            ObjAddItem.ConcessionAmount = ObjAddItem.ConcessionAmount;

                            ObjAddItem.Amount = ObjAddItem.Amount;
                            ObjAddItem.VATPercent = 0;//item.VATPerc;
                            vatper = item.VATPerc;
                            //to make tax applicable for staff for the first time and then on as being registered patient, and walkin patients as well.
                            //if (txtMRNo.Text == "")  
                            //if (((txtMRNo.Text == "" && IsStaff == true) || (txtMRNo.Text != "" && IsStaff == true)) || (txtMRNo.Text == "" && IsStaff == false))  //Commented and modified by Prashant Channe on 20thNov2019
                            if ((txtMRNo.Text == "") || (txtMRNo.Text != "" && IsStaff == true))  //Commented and modified by Prashant Channe on 13thDec2019
                            {
                                ObjAddItem.SGSTtaxtype = item.SGSTtaxtypeSale;              //item.SGSTtaxtype;
                                ObjAddItem.SGSTapplicableon = item.SGSTapplicableonSale;    //item.SGSTapplicableon;
                                ObjAddItem.CGSTtaxtype = item.CGSTtaxtypeSale;              //item.CGSTtaxtype;
                                ObjAddItem.CGSTapplicableon = item.CGSTapplicableonSale;    //item.CGSTapplicableon;
                                ObjAddItem.IGSTtaxtype = item.IGSTtaxtypeSale;              //item.IGSTtaxtype;
                                ObjAddItem.IGSTapplicableon = item.IGSTapplicableonSale;    //item.IGSTapplicableon;

                                //ObjAddItem.SGSTPercent = Convert.ToDouble(item.SGSTPercent) == 0 ? Convert.ToDouble(item.IGSTPercent) != 0 ? Convert.ToDouble(item.IGSTPercent) / 2 : 0 : Convert.ToDouble(item.SGSTPercent);
                                //ObjAddItem.CGSTPercent = Convert.ToDouble(item.CGSTPercent) == 0 ? Convert.ToDouble(item.CGSTPercent) != 0 ? Convert.ToDouble(item.CGSTPercent) / 2 : 0 : Convert.ToDouble(item.CGSTPercent);

                                ObjAddItem.SGSTPercent = Convert.ToDouble(item.SGSTPercentSale) == 0 ? Convert.ToDouble(item.IGSTPercentSale) != 0 ? Convert.ToDouble(item.IGSTPercentSale) / 2 : 0 : Convert.ToDouble(item.SGSTPercentSale);
                                ObjAddItem.CGSTPercent = Convert.ToDouble(item.CGSTPercentSale) == 0 ? Convert.ToDouble(item.IGSTPercentSale) != 0 ? Convert.ToDouble(item.IGSTPercentSale) / 2 : 0 : Convert.ToDouble(item.CGSTPercentSale);

                                ObjAddItem.IGSTPercent = 0;
                            }
                            //by Anjali.............................
                            ObjAddItem.MRP = item.MRP;
                            ObjAddItem.OriginalMRP = ObjAddItem.MRP;
                            ObjAddItem.ItemVatType = item.ItemVatType;
                            ObjAddItem.AvailableQuantity = item.AvailableStock;
                            ObjAddItem.PurchaseRate = item.PurchaseRate;
                            ObjAddItem.ConcessionPercentage = item.DiscountOnSale;
                            ObjAddItem.ConcessionAmount = ObjAddItem.ConcessionAmount;
                            //***//----
                            if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
                            {
                                PatientRegType = ((IApplicationConfiguration)App.Current).SelectedPatient.RegType;
                            }

                            if (Staff.IsChecked == true)
                            {
                                ObjAddItem.ConcessionPercentage = item.StaffDiscount;
                            }
                            else if (txtMRNo.Text != "" && Staff.IsChecked == false && PatientRegType != 2)
                            {
                                ObjAddItem.ConcessionPercentage = item.RegisteredPatientsDiscount;
                            }
                            else
                            {
                                ObjAddItem.ConcessionPercentage = item.WalkinDiscount;
                            }
                            //--------

                            ObjAddItem.Amount = ObjAddItem.Amount;
                            ObjAddItem.NetAmount = ObjAddItem.NetAmount;
                            //By Anjali.................
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
                            objGUID = new Guid();
                            if (IsAuditTrail)
                            {
                                LogInformation = new LogInfo();
                                LogInformation.guid = objGUID;
                                LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                                LogInformation.TimeStamp = DateTime.Now;
                                LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                                LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                                LogInformation.Message = " VII : New Item Added : " //+ Convert.ToString(lineNumber)
                                                                        + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                                        + " , StoreID : " + Convert.ToString(StoreID) + " "
                                                                        + " , ItemID : " + Convert.ToString(item.ItemID) + " "
                                                                        + " , BatchID : " + Convert.ToString(item.BatchID) + " "
                                                                        + " , BatchCode : " + Convert.ToString(item.BatchCode) + " "
                                                                        + " , ExpiryDate : " + Convert.ToString(item.ExpiryDate) + " "
                                                                        + " , Transaction UOMID : " + Convert.ToString(ObjAddItem.SelectedUOM.ID) + " "
                                                                        + " , Transaction Quantity (InputTransactionQuantity) : " + Convert.ToString(ObjAddItem.Quantity) + " "
                                                                        + " , BaseConversionFactor : " + Convert.ToString(ObjAddItem.BaseConversionFactor) + " "  //item.SellingCF
                                                                        + " , Base Quantity (TransactionQuantity) : " + Convert.ToString(ObjAddItem.BaseQuantity) + " "
                                                                        + " , Base UOMID : " + Convert.ToString(ObjAddItem.BaseUOMID) + " "
                                                                        + " , SUOMID : " + Convert.ToString(ObjAddItem.SUOMID) + " "
                                                                        + " , StockCF : " + Convert.ToString(ObjAddItem.ConversionFactor) + " "
                                                                        + " , Stocking Quantity : " + Convert.ToString(ObjAddItem.Quantity * ObjAddItem.ConversionFactor) + " "
                                                                        + " , PUOMID : " + Convert.ToString(ObjAddItem.PUOMID) + " "
                                                                        + " , ConcessionPercentage : " + Convert.ToString(ObjAddItem.ConcessionPercentage) + " "
                                                                        + " , ConcessionAmount : " + Convert.ToString(ObjAddItem.ConcessionAmount) + " "
                                                                        + " , AvailableStockInBase : " + Convert.ToString(ObjAddItem.AvailableStockInBase) + "\r\n";


                                LogInfoList.Add(LogInformation);
                            }
                            PharmacyItems.Add(ObjAddItem);
                        }
                    }

                    CalculatePharmacySummary();
                    dgPharmacyItems.Focus();
                    dgPharmacyItems.UpdateLayout();
                    dgPharmacyItems.SelectedIndex = PharmacyItems.Count - 1;

                    if (!string.IsNullOrEmpty(strError.ToString()))
                    {
                        string strMsg = "Items Already Added";
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Purpose:Calculate Total pharmacy Amount
        /// </summary>
        double vatamt;
        //private void CalculatePharmacySummary()
        //{
        //    double Total, Concession, NetAmount, TotalVat;
        //    Total = Concession = NetAmount = TotalVat = 0;
        //    for (int i = 0; i < PharmacyItems.Count; i++)
        //    {
        //        Total += (PharmacyItems[i].Amount);
        //        Concession += PharmacyItems[i].ConcessionAmount;
        //        TotalVat += PharmacyItems[i].VATAmount;

        //    }

        //    NetAmount = (Total - Concession) + TotalVat;
        //    vatamt = TotalVat;
        //    txtPharmacyTotal.Text =String.Format("{0:0.00}",Total);       
        //    txtPharmacyConcession.Text =String.Format("{0:0.00}", Concession);
        //    txtPharmacyNetAmount.Text = String.Format("{0:0.00}",NetAmount);

        //    txtNetAmount.Text =String.Format("{0:0.00}", NetAmount);
        //    txtTotalBill.Text =String.Format("{0:0.00}", Total);
        //    txtTotalConcession.Text =String.Format("{0:0.00}", Concession);

        //    txtPayAmount.Text = txtNetAmount.Text;

        //}


        double roundAmt = 0;
        private void CalculatePharmacySummary()
        {
            CalculatePackageConcession();   // Package New Changes Added on 26042018

            double Total, Concession, NetAmount, TotalVat, TotalSGST, TotalCGST;
            Total = Concession = NetAmount = TotalVat = PackageConcenssion = TotalSGST = TotalCGST = 0;
            for (int i = 0; i < PharmacyItems.Count; i++)
            {
                Total += (PharmacyItems[i].Amount);
                Concession += PharmacyItems[i].ConcessionAmount;
                TotalVat += PharmacyItems[i].VATAmount;
                TotalSGST += PharmacyItems[i].SGSTAmount;
                TotalCGST += PharmacyItems[i].CGSTAmount;
                NetAmount += PharmacyItems[i].NetAmount;

                if (PharmacyItems[i].PackageID > 0)
                {
                    //PackageConcenssion += PharmacyItems[i].ConcessionAmount;          // For Package New Changes Commented on 16062018
                    PackageConcenssion += PharmacyItems[i].PackageConcessionAmount;     // For Package New Changes Added on 16062018
                }

            }


            txtPharmacyTotal.Text = String.Format("{0:0.00}", Total);
            txtPharmacyConcession.Text = String.Format("{0:0.00}", Concession);
            txtPharmacyNetAmount.Text = String.Format("{0:0.00}", NetAmount);

            txtNetAmount.Text = String.Format("{0:0.00}", NetAmount);
            txtTotalBill.Text = String.Format("{0:0.00}", Total);
            txtTotalConcession.Text = String.Format("{0:0.00}", Concession);

            //By Anjali............................
            //txtRountOffAmount.Text = Math.Round(NetAmount).ToString();
            txtRountOffAmount.Text = GetRoundOffNetAmount(string.IsNullOrEmpty(txtNetAmount.Text) ? 0 : Convert.ToDouble(txtNetAmount.Text), 0).ToString();
            //roundAmt = Math.Round(NetAmount);
            roundAmt = GetRoundOffNetAmount(string.IsNullOrEmpty(txtNetAmount.Text) ? 0 : Convert.ToDouble(txtNetAmount.Text), 0);
            //.....................................

            txtPayAmount.Text = txtNetAmount.Text;

        }

        private double GetRoundOffNetAmount(double NetAmount, double RoundOffNetAmount)
        {
            double FloorNet = 0;
            FloorNet = Math.Floor(NetAmount);

            if (FloorNet % 2 == 0) //Even
            {
                double RoundNet = NetAmount - FloorNet;
                if (RoundNet == 0.5)
                {
                    RoundOffNetAmount = Math.Round(NetAmount) + 1;
                }
                else
                {
                    RoundOffNetAmount = Math.Round(NetAmount);
                }
            }
            else //Odd
            {
                RoundOffNetAmount = Math.Round(NetAmount);
            }

            return RoundOffNetAmount;
        }




        private void CalculatePackageConcession()   // Package New Changes Added on 26042018
        {
            if (cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)
            {
                decimal ItemAmount = 0;

                decimal PackageConsumptionAmount = 0;
                decimal PharmacyConsumeAmount = 0;

                double TotalConsumption = 0;
                double PharmacyCompoRate = 0;


                PackageConsumptionAmount = Convert.ToDecimal(((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageConsumptionAmount);
                PharmacyConsumeAmount = Convert.ToDecimal(((MasterListItem)cmbApplicabelPackage.SelectedItem).PharmacyConsumeAmount);
                PharmacyCompoRate = ((MasterListItem)cmbApplicabelPackage.SelectedItem).PharmacyFixedRate;

                //TotalConsumption = Convert.ToDouble(PackageConsumptionAmount + PharmacyConsumeAmount);
                TotalConsumption = Convert.ToDouble(PharmacyConsumeAmount);

                double ConcessionAmt = 0;

                foreach (clsItemSalesDetailsVO itemSD in PharmacyItems)
                {
                    ItemAmount = Convert.ToDecimal(itemSD.MRP) * Convert.ToDecimal(itemSD.Quantity);

                    TotalConsumption = TotalConsumption + Convert.ToDouble(ItemAmount);

                    if (ItemAmount > 0)
                    {
                        //if (TotalConsumption <= PharmacyCompoRate)    // For Package New Changes commented on 29062018 
                        if (TotalConsumption <= PharmacyCompoRate)      // For Package New Changes modified on 29062018
                        {
                            //itemSD.ConcessionPercentage = 100;
                            itemSD.PackageConcessionPercentage = 100;      // For Package New Changes Added on 16062018
                            //itemSD.ConcessionAmount = itemSD.Amount;
                        }
                        else
                        {
                            ConcessionAmt = 0;
                            ConcessionAmt = Convert.ToDouble(ItemAmount) - (TotalConsumption - PharmacyCompoRate);

                            if (ConcessionAmt > 0)
                            {
                                //itemSD.ConcessionAmount = ConcessionAmt;
                                itemSD.PackageConcessionAmount = ConcessionAmt;     // For Package New Changes Added on 16062018
                            }
                            else
                            {
                                //itemSD.ConcessionAmount = 0;
                                itemSD.PackageConcessionAmount = 0;                 // For Package New Changes Added on 16062018
                            }
                        }
                    }
                    else
                    {
                        //itemSD.ConcessionPercentage = 0;
                        itemSD.PackageConcessionPercentage = 0;                     // For Package New Changes Added on 16062018
                    }
                }

            }
        }

        #region Commentedcode
        //double PharmacyVat = 0;
        //private void CalculateTotalSummary()
        //{
        //    //if (string.IsNullOrEmpty(txtPharmacyNetAmount.Text))
        //    //    txtPharmacyNetAmount.Text = "0";
        //    //if (string.IsNullOrEmpty(txtPharmacyTotal.Text))
        //    //    txtPharmacyTotal.Text = "0";
        //    //if (string.IsNullOrEmpty(txtPharmacyConcession.Text))
        //    //    txtPharmacyConcession.Text = "0";

        //    //txtNetAmount.Text = (Convert.ToDouble(txtPharmacyNetAmount.Text) + PharmacyVat).ToString();
        //    //txtTotalBill.Text = (Convert.ToDouble(txtPharmacyTotal.Text)).ToString();
        //    //txtTotalConcession.Text = (Convert.ToDouble(txtPharmacyConcession.Text)).ToString();

        //}
        #endregion

        double PackageConcenssion = 0;
        #region Save data
        /// <summary>
        /// Purpose:Save Patient, Visit and bill.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        bool isValid = true;
        bool isValid1 = true;
        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;
            if (ClickedFlag == 1)
            {

                if (txtNetAmount.Text == "" || txtNetAmount.Text == Convert.ToString(0))
                {
                    isValid = false;
                    string msgText = "You Can Not Save The Bill With Zero Amount";

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);

                    msgWD.Show();
                    ClickedFlag = 0;

                    return;
                }


                if (!string.IsNullOrEmpty(txtNetAmount.Text) && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CounterSaleBillAdresslimit > 0
                    && (decimal.Parse(txtNetAmount.Text) > ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CounterSaleBillAdresslimit) && string.IsNullOrWhiteSpace(txtDelivery.Text))
                {

                    isValid = false;
                    string msgText = "Bill Limit ₹ " + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CounterSaleBillAdresslimit.ToString("0.####") + " Exceeds, Now Address is Mandetory.";

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgWD.Show();
                    ClickedFlag = 0;
                    txtDelivery.Focus();
                    if (txtDelivery.IsEnabled == false) txtDelivery.IsEnabled = true;
                    return;
                }


                //* Added by - Ajit Jadhav
                //* Added Date - 9/9/2016
                //* Comments - Check Quantity In The List Can't Be Zero
                if (PharmacyItems.Count > 0)
                {
                    foreach (var item in PharmacyItems)
                    {
                        if (item.Quantity == 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", " Quantity In The List Can't Be Zero. Please Enter Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWD.Show();
                            ClickedFlag = 0;
                            return;
                        }
                    }
                }
                else
                {
                    isValid = false;
                    string msgText = "You can not save the Bill without Items";

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD.Show();
                    ClickedFlag = 0;

                    return;
                }


                //else if (PharmacyItems.Where(s => s.CompanyID != ((MasterListItem)cmbCompany.SelectedItem).ID || s.PatientSourceID != ((clsPatientSourceVO)cmbPatientSource.SelectedItem).ID || s.TariffID != ((MasterListItem)cmbTariff.SelectedItem).ID || s.PatientCategoryID != ((MasterListItem)cmbPatientCategory.SelectedItem).ID).Any())
                //{
                //    isValid = false;
                //    MessageBoxControl.MessageBoxChildWindow msgW2 =
                //                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Multiple Sponsor Billing is not Allowed", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                //    msgW2.Show();
                //    ClickedFlag = 0;
                //    return;
                //}


                long TransactionUOMID = 0;

                StrPrescriptionDetailsID = new StringBuilder();


                foreach (var item in PharmacyItems)
                {
                    //Log Write here   

                    // lineNumber = stackFrame.GetFileLineNumber();

                    if (StrPrescriptionDetailsID.ToString().Length > 0)
                        StrPrescriptionDetailsID.Append(",");
                    StrPrescriptionDetailsID.Append(item.PrescriptionDetailsID);

                    objGUID = new Guid();

                    TransactionUOMID = 0;
                    if (item.SelectedUOM != null)
                    {
                        TransactionUOMID = item.SelectedUOM.ID;
                    }
                    else
                    {
                        TransactionUOMID = item.TransactionUOMID;
                    }
                    if (IsAuditTrail)
                    {
                        LogInformation = new LogInfo();
                        LogInformation.guid = objGUID;
                        LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        LogInformation.TimeStamp = DateTime.Now;
                        LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                        LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                        LogInformation.Message = " VIII : Line Number : " //+ Convert.ToString(lineNumber)
                                                                + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                                + " , StoreID : " + Convert.ToString(StoreID) + " "
                                                                + " , ItemID : " + Convert.ToString(item.ItemID) + " "
                                                                + " , BatchID : " + Convert.ToString(item.BatchID) + " "
                                                                + " , BatchCode : " + Convert.ToString(item.BatchCode) + " "
                                                                + " , ExpiryDate : " + Convert.ToString(item.ExpiryDate) + " "
                                                                + " , Transaction UOMID : " + Convert.ToString(TransactionUOMID) + " "
                                                                + " , Transaction Quantity (InputTransactionQuantity) : " + Convert.ToString(item.Quantity) + " "
                                                                + " , BaseConversionFactor : " + Convert.ToString(item.BaseConversionFactor) + " "
                                                                + " , Base Quantity (TransactionQuantity) : " + Convert.ToString(item.BaseQuantity) + " "
                                                                + " , Base UOMID : " + Convert.ToString(item.BaseUOMID) + " "
                                                                + " , SUOMID : " + Convert.ToString(item.SUOMID) + " "
                                                                + " , StockCF : " + Convert.ToString(item.ConversionFactor) + " "
                                                                + " , Stocking Quantity : " + Convert.ToString(item.Quantity * item.ConversionFactor) + " "
                                                                + " , PUOMID : " + Convert.ToString(item.PUOMID) + " "
                                                                + " , ConcessionPercentage : " + Convert.ToString(item.ConcessionPercentage) + " "
                                                                + " , ConcessionAmount : " + Convert.ToString(item.ConcessionAmount) + " "
                                                                + " , AvailableStockInBase : " + Convert.ToString(item.AvailableStockInBase) + "\r\n";


                        LogInfoList.Add(LogInformation);
                    }
                    //CallLogBizAction(LogBizAction);
                    ////


                    if (item.BaseQuantity > item.AvailableStockInBase)//item.Quantity > item.AvailableQuantity
                    {
                        isValid1 = false;
                        string msgText = "Available Quantity For " + item.ItemName + " Is " + String.Format("{0:0.00}", item.AvailableQuantity);

                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgWD.Show();
                        ClickedFlag = 0;

                        break;
                    }
                    else if (item.SelectedUOM.ID == 0 || item.SelectedUOM.ID == null)
                    {
                        isValid1 = false;
                        string msgText = "Please Select UOM For Item" + item.ItemName;

                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgWD.Show();
                        ClickedFlag = 0;

                        break;
                    }
                }

                if (cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0 && (((MasterListItem)cmbApplicabelPackage.SelectedItem).PharmacyFixedRate > 0))    // For Package New Changes added on 27042018
                {
                    decimal ItemAmount = 0;

                    decimal PackageConsumptionAmount = 0;
                    decimal PharmacyConsumeAmount = 0;
                    decimal OPDConsumption = 0;

                    double TotalConsumption = 0;
                    double PharmacyCompoRate = 0;


                    PackageConsumptionAmount = Convert.ToDecimal(((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageConsumptionAmount);
                    PharmacyConsumeAmount = Convert.ToDecimal(((MasterListItem)cmbApplicabelPackage.SelectedItem).PharmacyConsumeAmount);
                    OPDConsumption = Convert.ToDecimal(((MasterListItem)cmbApplicabelPackage.SelectedItem).OPDConsumption);
                    PharmacyCompoRate = ((MasterListItem)cmbApplicabelPackage.SelectedItem).PharmacyFixedRate;

                    //TotalConsumption = Convert.ToDouble(PackageConsumptionAmount + PharmacyConsumeAmount);
                    TotalConsumption = Convert.ToDouble(PharmacyConsumeAmount);

                    foreach (clsItemSalesDetailsVO itemSD in PharmacyItems)
                    {
                        //ItemAmount = Convert.ToDecimal(itemSD.ConcessionAmount);         // For Package New Changes Commented on 16062018     // Convert.ToDecimal(itemSD.MRP) * Convert.ToDecimal(itemSD.Quantity);
                        ItemAmount = Convert.ToDecimal(itemSD.PackageConcessionAmount);    // For Package New Changes Added on 16062018

                        TotalConsumption = TotalConsumption + Convert.ToDouble(ItemAmount);


                    }

                    double TotalMaterialConsumption = 0;

                    //TotalMaterialConsumption = Convert.ToDouble(ItemAmount + PackageConsumptionAmount + OPDConsumption + PharmacyConsumeAmount);
                    TotalMaterialConsumption = Convert.ToDouble(PackageConsumptionAmount + OPDConsumption + Convert.ToDecimal(TotalConsumption));

                    if ((((MasterListItem)cmbApplicabelPackage.SelectedItem).TotalPackageAdvance) > 0)
                    {
                        //if (TotalMaterialConsumption < ((MasterListItem)cmbApplicabelPackage.SelectedItem).TotalPackageAdvance)   // For Package New Changes commented on 29062018
                        if (Math.Round(TotalMaterialConsumption, 0) <= ((MasterListItem)cmbApplicabelPackage.SelectedItem).TotalPackageAdvance)    // For Package New Changes modified on 29062018
                        {
                            if (TotalConsumption > PharmacyCompoRate)
                            {
                                //isValid = false;
                                //string msgText12 = "You can not save the Bill , as it exceeds the Package Pharmacy Component Limit";

                                //MessageBoxControl.MessageBoxChildWindow msgWD12 =
                                //new MessageBoxControl.MessageBoxChildWindow("Palash", msgText12, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                //msgWD12.Show();
                                //ClickedFlag = 0;

                                //return;
                            }
                        }
                        else
                        {
                            isValid = false;
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", " Patient doesn’t have enough advance. Kindly collect the Advance first", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            ClickedFlag = 0;
                            return;
                        }
                    }
                    else
                    {
                        isValid = false;
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("", " Patient doesn’t have enough advance. Kindly collect the Advance first", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        ClickedFlag = 0;
                        return;

                    }
                }

                if (searchPatient == true)
                {
                    bool IsValidSponsor = false;

                    if (PatientSponsorDetails != null)
                    {
                        foreach (clsPatientSponsorVO itemSponsor in PatientSponsorDetails)
                        {
                            if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == itemSponsor.PatientId && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID == itemSponsor.UnitId)
                            {
                                //cmbPatientCategory
                                if ((cmbPatientCategory.SelectedItem != null && ((MasterListItem)cmbPatientCategory.SelectedItem).ID == itemSponsor.PatientCategoryID) && (cmbPatientSource.SelectedItem != null && ((MasterListItem)cmbPatientSource.SelectedItem).ID == itemSponsor.PatientSourceID) && (cmbCompany.SelectedItem != null && ((MasterListItem)cmbCompany.SelectedItem).ID == itemSponsor.CompanyID) && (cmbTariff.SelectedItem != null && ((MasterListItem)cmbTariff.SelectedItem).ID == itemSponsor.TariffID))
                                {
                                    IsValidSponsor = true;
                                    break;
                                }
                            }
                        }

                        if (IsValidSponsor == false)
                        {
                            isValid = false;
                            MessageBoxControl.MessageBoxChildWindow msgW12 =
                            new MessageBoxControl.MessageBoxChildWindow("", " Please select valid Sponsor Details", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW12.Show();
                            ClickedFlag = 0;
                            return;
                        }
                    }
                }

                isValid = ChkValidation();
                if (isValid1 && isValid && resultround == true)
                {
                    chkFreezBill.IsChecked = true;
                    if (chkFreezBill.IsChecked == true)
                    {
                        isValid = false;
                        //   string msgText = "Are You Sure \n You Want To Freeze The Bill ?";

                        //By Anjali.......................................
                        //if (cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)
                        //{
                        //    string msgText = "Are You Sure You Want To Apply Package Discount and Save The Bill?";
                        //    MessageBoxControl.MessageBoxChildWindow msgWD =
                        //      new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        //    msgWD.OnMessageBoxClosed += (arg) =>
                        //   {
                        //       if (arg == MessageBoxResult.Yes)
                        //       {
                        //           ApplyRules();
                        //       }
                        //       else
                        //       {
                        //           ClickedFlag = 0;
                        //       }

                        //   };
                        //    msgWD.Show();

                        //}
                        //else
                        //{

                        //.....................................................


                        string msgText = "Are You Sure You Want To Save The Bill ?";

                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                        msgWD.OnMessageBoxClosed += (arg) =>
                        {
                            if (arg == MessageBoxResult.Yes)
                            {
                                PaymentWindow paymentWin = new PaymentWindow();
                                paymentWin.IsFromPharmacyBill = true;

                                paymentWin.Initiate("Bill");
                                // Uncomment by  BHUSHAN....  FOr Advance Cusumed On Pharamacy Bill . . .
                                paymentWin.tabControl1.Visibility = System.Windows.Visibility.Visible;

                                //paymentWin.txtPayTotalAmount.Text = this.txtNetAmount.Text;
                                paymentWin.txtPayTotalAmount.Text = this.txtPharmacyTotal.Text;
                                //By Anjali......................................
                                //paymentWin.txtPayableAmt.Text = this.txtPayAmount.Text;
                                //paymentWin.TotalAmount = double.Parse(txtPayAmount.Text);

                                paymentWin.PackageConcenssionAmt = Math.Round(PackageConcenssion, 0);

                                paymentWin.txtPayableAmt.Text = txtRountOffAmount.Text;
                                paymentWin.TotalAmount = double.Parse(txtRountOffAmount.Text);
                                //...............................................................
                                paymentWin.txtDiscAmt.Text = this.txtTotalConcession.Text;


                                paymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstBill;

                                if (LinkPatientID > 0)
                                {
                                    paymentWin.LinkPatientID = LinkPatientID;
                                    paymentWin.LinkPatientUnitID = LinkPatientUnitID;
                                    paymentWin.LinkCompanyID = LinkCompanyID;
                                }

                                if (cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0 && (((MasterListItem)cmbApplicabelPackage.SelectedItem).PharmacyFixedRate > 0))    // For Package New Changes added on 16062018
                                {
                                    paymentWin.IsNewPackageFlow = true;     // Set to apply new logic to retreive Patient,Package advance both & its auto consume logic : added on 16062016
                                    paymentWin.PackageID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID;
                                    paymentWin.PackageBillID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageBillID;
                                    paymentWin.PackageBillUnitID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageBillUnitID;

                                    paymentWin.txtPackagePayableAmt.Text = Math.Round(PackageConcenssion, 0).ToString();
                                    paymentWin.lblPackagePayableAmt.Visibility = Visibility.Visible;
                                    paymentWin.txtPackagePayableAmt.Visibility = Visibility.Visible;
                                }

                                if (Staff.IsChecked == true)
                                {
                                    paymentWin.IsStaff = true;
                                }

                                paymentWin.OnSaveButton_Click += new RoutedEventHandler(paymentWin_OnSaveButton_Click);
                                paymentWin.OnCancelButton_Click += new RoutedEventHandler(paymentWin_OnCancelButton_Click);
                                //  paymentWin.StoreID = StoreID;
                                paymentWin.Show();
                            }
                            else
                            {
                                ClickedFlag = 0;
                            }
                        };
                        msgWD.Show();
                        IsFreez = true;
                        // }

                    }
                    else
                    {
                        IsFreez = false;

                        if (txtMRNo.Text == "")
                        {
                            SavePatient(null, IsFreez);
                        }

                        else if (((IApplicationConfiguration)App.Current).SelectedPatient.VisitID == 0)
                        {
                            SaveVisit(((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, null, IsFreez, ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId);
                        }
                        else
                        {
                            SaveBill(null, IsFreez, ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId);
                        }
                    }
                }
                else
                {
                    ClickedFlag = 0;
                }
            }
        }

        void paymentWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            //  StoreID = ((CIMS.Forms.PaymentWindow)sender).StoreID;
            if (ClickedFlag == 1)
            {
                if (((CIMS.Forms.PaymentWindow)sender).DialogResult == true)
                {
                    if (((PaymentWindow)sender).Payment != null)
                    {
                        //if (txtMRNo.Text == "")
                        //{
                        //    checkDuplication(((PaymentWindow)sender).Payment, IsFreez);
                        //    //SavePatient(((PaymentWindow)sender).Payment, IsFreez);
                        //}
                        //else if (((IApplicationConfiguration)App.Current).SelectedPatient.VisitID == 0)
                        //{
                        //    SaveVisit(((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, ((PaymentWindow)sender).Payment, IsFreez, ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId);
                        //}
                        //else
                        //{
                        //    SaveBill(((PaymentWindow)sender).Payment, IsFreez, ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId);
                        //}
                        if (IsAuditTrail == true && ((PaymentWindow)sender).Payment.LogInfoList != null)
                        {
                            if (((PaymentWindow)sender).Payment.LogInfoList.Count > 0)   // By Rohinee For Activity Log
                            {
                                foreach (LogInfo item in ((PaymentWindow)sender).Payment.LogInfoList)
                                {
                                    LogInfoList.Add(item);
                                }
                            }
                        }

                        if (txtMRNo.Text == "")
                            checkDuplication(((PaymentWindow)sender).Payment, IsFreez);
                        else
                            SaveDetails(((PaymentWindow)sender).Payment, IsFreez, ((IApplicationConfiguration)App.Current).SelectedPatient == null ? 0 : ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient == null ? 0 : ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID);
                    }
                }
            }

        }
        void checkDuplication(clsPaymentVO pPayDetails, bool pFreezBill)
        {
            clsCheckPatientDuplicacyBizActionVO BizAction = new clsCheckPatientDuplicacyBizActionVO();

            BizAction.PatientDetails = ((clsPatientVO)this.DataContext);
            BizAction.PatientDetails.SpouseDetails = ((clsPatientVO)this.DataContext).SpouseDetails;
            if ((MasterListItem)cmbGender.SelectedItem != null)
                BizAction.PatientDetails.GeneralDetails.GenderID = ((MasterListItem)cmbGender.SelectedItem).ID;
            if (dtpDOB.SelectedDate != null)
            {
                BizAction.PatientDetails.GeneralDetails.DateOfBirth = dtpDOB.SelectedDate.Value.Date;
            }

            if (!string.IsNullOrEmpty(txtMobileCountryCode.Text))
            {
                BizAction.PatientDetails.MobileCountryCode = txtMobileCountryCode.Text;
            }

            if (!string.IsNullOrEmpty(txtContactNo.Text))
            {
                BizAction.PatientDetails.GeneralDetails.ContactNO1 = txtContactNo.Text;
            }

            BizAction.PatientDetails.SpouseDetails.GenderID = 0;
            BizAction.PatientDetails.SpouseDetails.DateOfBirth = null;
            BizAction.PatientDetails.SpouseDetails.MobileCountryCode = null;
            BizAction.PatientDetails.SpouseDetails.ContactNO1 = null;


            BizAction.PatientEditMode = false;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null && ea.Result != null)
                {
                    if (((clsCheckPatientDuplicacyBizActionVO)ea.Result).SuccessStatus != 0)
                    {
                        ClickedFlag = 0;
                        string strDuplicateMsg = "";
                        if (((clsCheckPatientDuplicacyBizActionVO)ea.Result).SuccessStatus == 3)
                        {
                            strDuplicateMsg = "Mobile Number already exists, Are you sure you want to Continue ?";

                            MessageBoxControl.MessageBoxChildWindow msgW1 = new MessageBoxControl.MessageBoxChildWindow("Palash", strDuplicateMsg, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                                {
                                    if (res == MessageBoxResult.Yes)
                                    {
                                        //SavePatient(pPayDetails, pFreezBill);
                                        SaveDetails(pPayDetails, pFreezBill, ((IApplicationConfiguration)App.Current).SelectedPatient == null ? 0 : ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient == null ? 0 : ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID);
                                    }

                                };

                            msgW1.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient already exists.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }
                    else
                    {
                        //SavePatient(pPayDetails, pFreezBill);
                        SaveDetails(pPayDetails, pFreezBill, ((IApplicationConfiguration)App.Current).SelectedPatient == null ? 0 : ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient == null ? 0 : ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID);

                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }
        void paymentWin_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag = 0;
        }

        private void SaveBill(clsPaymentVO pPayDetails, bool pFreezBill, long pPatientID, long pPatientUnitID)
        {

            Indicatior = new WaitIndicator();
            Indicatior.Show();
            try
            {
                clsBillVO objBill = new clsBillVO();

                if (SelectedBill == null)
                {
                    objBill.Date = DateTime.Now;
                    objBill.Time = DateTime.Now;
                }
                else
                    objBill = SelectedBill.DeepCopy();

                objBill.IsFreezed = pFreezBill;
                objBill.PaymentDetails = pPayDetails;

                objBill.PatientID = pPatientID;
                objBill.Opd_Ipd_External_Id = VisitId;
                objBill.Opd_Ipd_External_UnitId = pPatientUnitID;

                if (pFreezBill)
                {
                    objBill.Date = DateTime.Now;
                    objBill.Time = DateTime.Now;

                    if (pPayDetails != null)
                    {
                        objBill.BalanceAmountSelf = pPayDetails.BillBalanceAmount;
                        objBill.BillPaymentType = pPayDetails.BillPaymentType;
                    }

                    if (!string.IsNullOrEmpty(txtPayAmount.Text))
                        objBill.SelfAmount = Convert.ToDouble(txtPayAmount.Text);
                }

                if (!string.IsNullOrEmpty(txtTotalBill.Text))
                    objBill.TotalBillAmount = Convert.ToDouble(txtTotalBill.Text);

                if (!string.IsNullOrEmpty(txtTotalConcession.Text))
                    objBill.TotalConcessionAmount = Convert.ToDouble(txtTotalConcession.Text);


                //By Anjali.......................................................
                //if (!string.IsNullOrEmpty(txtNetAmount.Text))
                //    objBill.NetBillAmount = Convert.ToDouble(txtNetAmount.Text);

                if (!string.IsNullOrEmpty(txtNetAmount.Text))
                    objBill.CalculatedNetBillAmount = Convert.ToDouble(txtNetAmount.Text);
                if (!string.IsNullOrEmpty(txtNetAmount.Text))
                    objBill.NetBillAmount = Convert.ToDouble(txtRountOffAmount.Text);




                //....................................................................

                // Commented By CDS

                //objBill.CostingDivisionID = ((MasterListItem)(cmbCostingDivision.SelectedItem)).ID;   //Costing Divisions for Pharmacy Billing

                //if (objBill.PaymentDetails != null)
                //    objBill.PaymentDetails.CostingDivisionID = ((MasterListItem)(cmbCostingDivision.SelectedItem)).ID;   //Costing Divisions for Pharmacy Billing

                // Added By CDS



                //By Anjali.........................................................................................................


                if (PatientSourceID > 0)
                {
                    objBill.PatientSourceId = PatientSourceID;
                }
                else
                {
                    if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID != null)
                        objBill.PatientSourceId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;

                }

                if (cmbTariff.SelectedItem != null)
                    objBill.TariffId = ((MasterListItem)cmbTariff.SelectedItem).ID;
                else
                {
                    if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.TariffID != null)
                        objBill.TariffId = ((IApplicationConfiguration)App.Current).SelectedPatient.TariffID;
                }


                if (cmbPatientCategory.SelectedItem != null)
                {
                    objBill.PatientCategoryId = ((MasterListItem)cmbPatientCategory.SelectedItem).ID;
                }
                else
                {
                    if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.NewPatientCategoryID != null)
                        objBill.PatientCategoryId = ((IApplicationConfiguration)App.Current).SelectedPatient.NewPatientCategoryID;
                }

                if (cmbCompany.SelectedItem != null)
                    objBill.CompanyId = ((MasterListItem)cmbCompany.SelectedItem).ID;
                else
                {
                    if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID != null)
                        objBill.CompanyId = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
                }
                //.....................................................................................................................

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
                    if (txtReasonForVariance.Text != string.Empty)
                        objBill.PharmacyItems.ReasonForVariance = txtReasonForVariance.Text;
                    objBill.PharmacyItems.VisitID = VisitId;
                    objBill.PharmacyItems.PatientID = PatientId;
                    objBill.PharmacyItems.PatientUnitID = pPatientUnitID;
                    objBill.PharmacyItems.Date = objBill.Date;
                    objBill.PharmacyItems.Time = objBill.Time;
                    objBill.PharmacyItems.StoreID = StoreID;
                    objBill.PharmacyItems.CostingDivisionID = objBill.CostingDivisionID;

                    //By Anjali.........
                    objBill.PharmacyItems.VATAmount = vatamt;
                    objBill.PharmacyItems.VATPercentage = vatper;
                    //if (cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)
                    //{
                    //    objBill.PharmacyItems.PackageID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID;
                    //}


                    if (!string.IsNullOrEmpty(txtPharmacyTotal.Text))
                        objBill.PharmacyItems.TotalAmount = Convert.ToDouble(txtPharmacyTotal.Text);

                    if (!string.IsNullOrEmpty(txtPharmacyConcession.Text))
                        objBill.PharmacyItems.ConcessionAmount = Convert.ToDouble(txtPharmacyConcession.Text);
                    //By Anjali...........................................................
                    //if (!string.IsNullOrEmpty(txtPharmacyNetAmount.Text))
                    //    objBill.PharmacyItems.NetAmount = Convert.ToDouble(txtPharmacyNetAmount.Text);

                    if (!string.IsNullOrEmpty(txtRountOffAmount.Text))
                        objBill.PharmacyItems.NetAmount = Convert.ToDouble(txtRountOffAmount.Text);

                    //.......................................................................

                    objBill.PharmacyItems.Items = PharmacyItems.ToList();
                }
                clsAddBillBizActionVO BizAction = new clsAddBillBizActionVO();
                BizAction.Details = new clsBillVO();
                BizAction.Details = objBill;




                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    Indicatior.Close();
                    ClickedFlag = 0;
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            if (((clsAddBillBizActionVO)arg.Result).SuccessStatus == -10)
                            {
                                Indicatior.Close();
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Insufficient Stock", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW1.Show();
                            }
                            else if (((clsAddBillBizActionVO)arg.Result).Details != null)
                            {
                                Indicatior.Close();
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Bill Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                                    {
                                        if (res == MessageBoxResult.OK)
                                        {
                                            if (pFreezBill == true)
                                            {
                                                PrintBill((((clsAddBillBizActionVO)arg.Result).Details).ID, ((MasterListItem)cmbPrintFormat.SelectedItem).ID);
                                            }
                                            //long PID = PatientId;
                                            //long VID = VisitId;
                                            //string URL = "../Reports/OPD/PharmacyBill.aspx?PatientID=" + PID + "&VisitID=" + VID;
                                            //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                                            UserControl rootPage = Application.Current.RootVisual as UserControl;
                                            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                                            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                                            mElement.Text = "";

                                            InitialiseForm();
                                            this.DataContext = new clsPatientVO();
                                            cmbGender.SelectedValue = ((clsPatientVO)this.DataContext).GenderID;
                                            dtpDOB.SelectedDate = null;
                                            txtYY.Text = "";
                                            txtDD.Text = "";
                                            txtMM.Text = "";
                                            txtMobileCountryCode.Text = "";
                                            txtContactNo.Text = "";

                                            txtPurchaseFrequency.Text = "";
                                            searchPatient = false;

                                            txtFirstName.IsEnabled = true;
                                            txtMiddleName.IsEnabled = true;
                                            txtLastName.IsEnabled = true;
                                            txtDelivery.IsEnabled = true;
                                            cmbGender.IsEnabled = true;
                                            dtpDOB.IsEnabled = true;
                                            txtMobileCountryCode.IsEnabled = true;
                                            txtContactNo.IsEnabled = true;
                                            txtYY.IsEnabled = true;
                                            txtMM.IsEnabled = true;
                                            txtDD.IsEnabled = true;
                                            txtMobileCountryCode.Text = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DefaultCountryCode;
                                            cmbPatientCategory.IsEnabled = true;
                                            cmbPatientSource.IsEnabled = true;
                                            cmbTariff.IsEnabled = true;
                                            cmbCompany.IsEnabled = true;
                                            cmbApplicabelPackage.ItemsSource = null;
                                            cmbApplicabelPackage.Visibility = Visibility.Collapsed;
                                            ApplicablePack.Visibility = Visibility.Collapsed;

                                            txtReasonForVariance.Text = "";
                                            txtReferenceDoctor.Text = "";

                                            ChkValidation();

                                            FillPatientType();
                                            //FillCompany();
                                            FillPurchaseFrequencyUnit();
                                        }
                                    };
                                msgW1.Show();

                            }
                        }
                    }
                    else
                    {
                        Indicatior.Close();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("NT3", "Error Occured While Adding Bill Details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                Indicatior.Close();
                ClickedFlag = 0;
                throw ex;
            }
            finally
            {
            }
        }


        private void FillPurchaseFrequencyUnit()
        {
            List<MasterListItem> mlPaymode = new List<MasterListItem>();
            MasterListItem Default = new MasterListItem(0, "- Select -");
            mlPaymode.Insert(0, Default);
            EnumToList(typeof(PurchaseFrequencyUnit), mlPaymode);

            cmbPurchaseFrequency.ItemsSource = mlPaymode;
            cmbPurchaseFrequency.SelectedItem = mlPaymode[1];
        }

        public static void EnumToList(Type EnumType, List<MasterListItem> TheMasterList)
        {
            Array Values = GetValues(EnumType);
            foreach (int Value in Values)
            {

                string Display = Enum.GetName(EnumType, Value);
                MasterListItem Item = new MasterListItem(Value, Display);
                TheMasterList.Add(Item);
            }
        }

        public static object[] GetValues(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            List<object> values = new List<object>();

            var fields = from field in enumType.GetFields()
                         where field.IsLiteral
                         select field;

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(enumType);
                values.Add(value);
            }

            return values.ToArray();
        }

        /// <summary>
        /// Function is for Fetching PatientCategory
        /// </summary>
        private void FillPatientType()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.MasterTable = MasterTableNameList.M_PatientCategoryMaster;
            BizAction.MasterTable = MasterTableNameList.M_CategoryL1Master;

            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    MasterListItem Default = new MasterListItem(0, "- Select -");

                    objList.Add(Default);
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    cmbPatientCategory.ItemsSource = null;
                    cmbPatientCategory.ItemsSource = objList;

                    if (searchPatient != true)
                        cmbPatientCategory.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CategoryID;
                    else
                    {
                        if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID != 0)
                        {
                            cmbPatientCategory.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatient.NewPatientCategoryID;
                        }
                        else
                        {
                            cmbPatientCategory.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CategoryID;
                        }
                    }
                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }


        private void SavePatient(clsPaymentVO pPayDetails, bool pFreezBill)
        {

            clsAddPatientBizActionVO BizAction = new clsAddPatientBizActionVO();
            BizAction.PatientDetails = (clsPatientVO)this.DataContext;

            if (dtpDOB.SelectedDate == null)
            {
                BizAction.PatientDetails.GeneralDetails.IsAge = true;
                if (DOB != null)
                    BizAction.PatientDetails.GeneralDetails.DateOfBirth = DOB.Value.Date;
            }
            else
            {
                BizAction.PatientDetails.GeneralDetails.DateOfBirth = dtpDOB.SelectedDate;
                BizAction.PatientDetails.GeneralDetails.IsAge = false;
            }
            BizAction.PatientDetails.GeneralDetails.RegType = (short)PatientRegistrationType.Pharmacy;
            BizAction.PatientDetails.GeneralDetails.PatientTypeID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyPatientCategoryID;
            if (cmbGender.SelectedItem != null)
                BizAction.PatientDetails.GenderID = ((MasterListItem)cmbGender.SelectedItem).ID;

            BizAction.PatientDetails.ContactNo1 = txtContactNo.Text.Trim();
            if (!string.IsNullOrEmpty(txtMobileCountryCode.Text.Trim()))
                BizAction.PatientDetails.MobileCountryCode = txtMobileCountryCode.Text.Trim();
            BizAction.PatientDetails.OccupationId = 0;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                ClickedFlag = 0;
                if (arg.Error == null)
                {
                    clsAddPatientBizActionVO Obj = new clsAddPatientBizActionVO();
                    Obj = (clsAddPatientBizActionVO)arg.Result;
                    PatientId = ((clsAddPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientID;
                    PatientUnitId = ((clsAddPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.UnitId;
                    SaveSponsor(PatientId, PatientUnitId);
                    SaveVisit(Obj.PatientDetails.GeneralDetails.PatientID, pPayDetails, pFreezBill, PatientUnitId);
                }
                else
                {

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured While Adding Visit Details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void SaveVisit(long PID, clsPaymentVO pPayDetails, bool pFreezBill, long pPatientUnitID)
        {

            clsAddVisitBizActionVO BizAction = new clsAddVisitBizActionVO();
            BizAction.VisitDetails = new clsVisitVO();
            BizAction.VisitDetails.PatientId = PID;
            BizAction.VisitDetails.PatientUnitId = pPatientUnitID;
            BizAction.VisitDetails.VisitTypeID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyVisitTypeID;

            BizAction.VisitDetails.ReferredDoctor = txtReferenceDoctor.Text;
            BizAction.VisitDetails.ReferredDoctorID = ((MasterListItem)txtReferenceDoctor.SelectedItem).ID;
            BizAction.VisitDetails.VisitStatus = false; //As per discussion with girish sir and nilesh sir (25/4/2011) 
            BizAction.VisitDetails.Status = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                ClickedFlag = 0;
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        if (((clsAddVisitBizActionVO)arg.Result).VisitDetails.ID != 0)
                        {
                            VisitId = ((clsAddVisitBizActionVO)arg.Result).VisitDetails.ID;
                            SaveBill(pPayDetails, IsFreez, PID, pPatientUnitID);
                        }
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured While Adding Visit Details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        #endregion

        #region Get Patient
        /// <summary>
        /// Purpose: Get patient information using MrNo of patient.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        bool searchPatient = false;
        private void btnSearchCriteria_Click(object sender, RoutedEventArgs e)
        {

            if (Staff.IsChecked == true)
            {
                StaffSearch StfWin = new StaffSearch();
                StfWin.isfromCouterSaleStaff = true;
                StfWin.OnSaveButton_Click += new RoutedEventHandler(StaffSearch_OnSaveButton_Click);
                StfWin.Show();

            }
            else
            {
                PatientSearch Win = new PatientSearch();
                //By Anjali............
                Win.isfromCouterSale = true;
                //......................
                Win.OnSaveButton_Click += new RoutedEventHandler(PatientSearch_OnSaveButton_Click);
                Win.Show();
            }
        }


        void PatientSearch_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {

            try
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
                    WaitIndicator Indicatior = new WaitIndicator();
                    Indicatior.Show();
                    if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
                    {
                        clsGetPatientDetailsForCounterSaleBizActionVO BizAction = new clsGetPatientDetailsForCounterSaleBizActionVO();
                        BizAction.PatientDetails = new clsPatientVO();
                        BizAction.MRNO = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                        BizAction.UnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                        VisitId = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        Client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null)
                            {
                                if (arg.Result != null)
                                {
                                    searchPatient = true; // Added By Bhushanp 23052017 For Select Wrong Sponser and Source
                                    clsGetPatientDetailsForCounterSaleBizActionVO ObjPatient = new clsGetPatientDetailsForCounterSaleBizActionVO();
                                    ObjPatient = (clsGetPatientDetailsForCounterSaleBizActionVO)arg.Result;
                                    PatientId = ObjPatient.PatientDetails.GeneralDetails.PatientID;

                                    txtMRNo.Text = ObjPatient.PatientDetails.GeneralDetails.MRNo;
                                    txtFirstName.Text = ObjPatient.PatientDetails.GeneralDetails.FirstName;
                                    if (ObjPatient.PatientDetails.GeneralDetails.MiddleName != null)
                                    {
                                        txtMiddleName.Text = ObjPatient.PatientDetails.GeneralDetails.MiddleName;
                                    }
                                    txtLastName.Text = ObjPatient.PatientDetails.GeneralDetails.LastName;
                                    //dtpDOB.SelectedDate = ObjPatient.PatientDetails.GeneralDetails.DateOfBirth;

                                    //txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                                    //txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                                    //txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");
                                    if (ObjPatient.PatientDetails.GeneralDetails.IsAge == false)
                                    {
                                        dtpDOB.SelectedDate = ObjPatient.PatientDetails.GeneralDetails.DateOfBirth;
                                        txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                                        txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                                        txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");
                                    }
                                    else
                                    {
                                        DOB = ObjPatient.PatientDetails.GeneralDetails.DateOfBirthFromAge;
                                        txtYY.Text = ConvertDate(ObjPatient.PatientDetails.GeneralDetails.DateOfBirthFromAge, "YY");
                                        txtMM.Text = ConvertDate(ObjPatient.PatientDetails.GeneralDetails.DateOfBirthFromAge, "MM");
                                        txtDD.Text = ConvertDate(ObjPatient.PatientDetails.GeneralDetails.DateOfBirthFromAge, "DD");
                                    }
                                    cmbGender.SelectedValue = ObjPatient.PatientDetails.GenderID;
                                    txtMobileCountryCode.Text = ObjPatient.PatientDetails.MobileCountryCode.ToString();
                                    txtContactNo.Text = ObjPatient.PatientDetails.ContactNo1.ToString();
                                    // txtReferenceDoctor.Text = ObjPatient.PatientDetails.Doctor.ToString();
                                    //cmbPatientCategory.SelectedValue = ObjPatient.PatientDetails.PatientSponsorCategoryID;      // Commented on 07082018 to avoid multiple calls to FillCompany()
                                    txtDelivery.Text = ObjPatient.PatientDetails.AddressLine1;

                                    txtFirstName.IsEnabled = false;
                                    txtMiddleName.IsEnabled = false;
                                    txtLastName.IsEnabled = false;
                                    cmbGender.IsEnabled = false;
                                    dtpDOB.IsEnabled = false;
                                    txtMobileCountryCode.IsEnabled = false;
                                    txtContactNo.IsEnabled = false;
                                    txtYY.IsEnabled = false;
                                    txtMM.IsEnabled = false;
                                    txtDD.IsEnabled = false;
                                    txtDelivery.IsEnabled = false;

                                    IsPatientSelect = true;     // addede on 07082018 to set values on dropdown selection changed

                                    IsStaff = ObjPatient.PatientDetails.IsStaffPatient;  //Added by Prashant Channe on 20thNovember2019

                                    //Commented on 07082018 to select single package from diferent package
                                    //cmbPatientCategory.IsEnabled = false;
                                    //cmbPatientSource.IsEnabled = false;
                                    //cmbTariff.IsEnabled = false;
                                    //cmbCompany.IsEnabled = false;

                                    //By Anjali................
                                    //FillPackage(((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID);  // Commented on 07082018 to avoid multiple calls to FillPackage()
                                    //........................
                                    CheckVisit();
                                    FillPatientType();
                                    FillPatientSponsorDetailsNew();     // addede on 08082018 to check selected sponsor is valid or not
                                }
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured While Adding Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW1.Show();
                            }
                            Indicatior.Close();
                        };

                        Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        Client.CloseAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        void StaffSearch_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient.MRNo != null)
            {
                WaitIndicator Indicatior = new WaitIndicator();
                Indicatior.Show();
                try
                {
                    clsGetPatientDetailsForCounterSaleBizActionVO BizAction = new clsGetPatientDetailsForCounterSaleBizActionVO();
                    BizAction.PatientDetails = new clsPatientVO();
                    BizAction.MRNO = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            if (arg.Result != null)
                            {
                                //searchPatient = true;
                                clsGetPatientDetailsForCounterSaleBizActionVO ObjPatient = new clsGetPatientDetailsForCounterSaleBizActionVO();
                                ObjPatient = (clsGetPatientDetailsForCounterSaleBizActionVO)arg.Result;
                                PatientId = ObjPatient.PatientDetails.GeneralDetails.PatientID;

                                txtMRNo.Text = ObjPatient.PatientDetails.GeneralDetails.MRNo;
                                txtFirstName.Text = ObjPatient.PatientDetails.GeneralDetails.FirstName;
                                if (ObjPatient.PatientDetails.GeneralDetails.MiddleName != null)
                                {
                                    txtMiddleName.Text = ObjPatient.PatientDetails.GeneralDetails.MiddleName;
                                }
                                txtLastName.Text = ObjPatient.PatientDetails.GeneralDetails.LastName;
                                if (ObjPatient.PatientDetails.GeneralDetails.IsAge == false)
                                {
                                    dtpDOB.SelectedDate = ObjPatient.PatientDetails.GeneralDetails.DateOfBirth;
                                    txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                                    txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                                    txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");
                                }

                                txtDelivery.Text = ObjPatient.PatientDetails.AddressLine1;

                                cmbGender.SelectedValue = ObjPatient.PatientDetails.GenderID;
                                txtMobileCountryCode.Text = ObjPatient.PatientDetails.MobileCountryCode.ToString();
                                txtContactNo.Text = ObjPatient.PatientDetails.ContactNo1.ToString();
                                cmbPatientCategory.SelectedValue = ObjPatient.PatientDetails.PatientSponsorCategoryID;
                                txtMobileCountryCode.IsEnabled = false;
                                txtContactNo.IsEnabled = false;

                                IsPatientSelect = true;     // addede on 07082018 to set values on dropdown selection changed

                                IsStaff = ObjPatient.PatientDetails.IsStaffPatient;  //Added by Prashant Channe on 13thDecember2019

                                //Commented on 07082018 to select single package from diferent package
                                //Commented on 07082018 to select single package from diferent package
                                //cmbPatientCategory.IsEnabled = false;
                                //cmbPatientSource.IsEnabled = false;
                                //cmbTariff.IsEnabled = false;
                                //cmbCompany.IsEnabled = false;

                                //FillPackage(PatientId, ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID);   // commented on 08082018 to avoid multiple call to FillPackage()
                                CheckVisit();
                                FillPatientType();
                                FillPatientSponsorDetailsNew();     // addede on 08082018 to check selected sponsor is valid or not
                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured While Adding Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
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
            }
            else
            {
                txtFirstName.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName;
                txtLastName.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;
                txtMiddleName.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName;
                cmbGender.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID;
                dtpDOB.SelectedDate = ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth;
                txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");
                IsStaff = true;
                txtDelivery.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.AddressLine1;

                FillPatientType();  // added on 08082018 to set default sponsor details

                // commented on 08082018 as Package is available for only registered patient
                //FillPackage(((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID);  
            }
            txtFirstName.IsEnabled = false;
            txtMiddleName.IsEnabled = false;
            txtLastName.IsEnabled = false;
            cmbGender.IsEnabled = false;
            dtpDOB.IsEnabled = false;
            txtYY.IsEnabled = false;
            txtMM.IsEnabled = false;
            txtDD.IsEnabled = false;
            txtDelivery.IsEnabled = false;
        }

        /// <summary>
        /// Purpose:Get patient information using patient Id.
        /// </summary>
        private void GetPatient()
        {
            clsGetPatientBizActionVO BizAction1 = new clsGetPatientBizActionVO();
            BizAction1.PatientDetails = new clsPatientVO();
            BizAction1.PatientDetails.GeneralDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction1.PatientDetails.GeneralDetails.UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;

            BizAction1.PatientDetails.GeneralDetails.LinkServer = ((IApplicationConfiguration)App.Current).SelectedPatient.LinkServer;
            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);

            Client1.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        clsGetPatientBizActionVO Patient = new clsGetPatientBizActionVO();
                        Patient = (clsGetPatientBizActionVO)arg.Result;

                        txtFirstName.Text = Patient.PatientDetails.GeneralDetails.FirstName;
                        if (Patient.PatientDetails.GeneralDetails.MiddleName != null)
                        {
                            txtMiddleName.Text = Patient.PatientDetails.GeneralDetails.MiddleName;
                        }
                        txtLastName.Text = Patient.PatientDetails.GeneralDetails.LastName;
                        txtDelivery.Text = Patient.PatientDetails.AddressLine1;
                        //dtpDOB.SelectedDate = Patient.PatientDetails.GeneralDetails.DateOfBirth;

                        //txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                        //txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                        //txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");


                        if (Patient.PatientDetails.GeneralDetails.IsAge == false)
                        {
                            dtpDOB.SelectedDate = Patient.PatientDetails.GeneralDetails.DateOfBirth;
                            txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                            txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                            txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");
                        }
                        else
                        {
                            DOB = Patient.PatientDetails.GeneralDetails.DateOfBirthFromAge;
                            txtYY.Text = ConvertDate(Patient.PatientDetails.GeneralDetails.DateOfBirthFromAge, "YY");
                            txtMM.Text = ConvertDate(Patient.PatientDetails.GeneralDetails.DateOfBirthFromAge, "MM");
                            txtDD.Text = ConvertDate(Patient.PatientDetails.GeneralDetails.DateOfBirthFromAge, "DD");
                        }

                        cmbGender.SelectedValue = Patient.PatientDetails.GenderID;
                        txtMobileCountryCode.Text = Patient.PatientDetails.MobileCountryCode.ToString();
                        txtContactNo.Text = Convert.ToString(Patient.PatientDetails.ContactNo1);

                        cmbPatientCategory.SelectedValue = Patient.PatientDetails.GeneralDetails.PatientSponsorCategoryID;

                        //cmbPatientCategory.SelectedItem = Patient.PatientDetails.PatientTypeID;
                        //txtReferenceDoctor.Text = ObjPatient.PatientDetails.Doctor.ToString();





                        txtFirstName.IsEnabled = false;
                        txtMiddleName.IsEnabled = false;
                        txtLastName.IsEnabled = false;
                        txtDelivery.IsEnabled = false;
                        cmbGender.IsEnabled = false;
                        dtpDOB.IsEnabled = false;
                        txtMobileCountryCode.IsEnabled = false;
                        txtContactNo.IsEnabled = false;
                        txtYY.IsEnabled = false;
                        txtMM.IsEnabled = false;
                        txtDD.IsEnabled = false;


                        cmbPatientCategory.IsEnabled = false;
                        cmbPatientSource.IsEnabled = false;
                        cmbTariff.IsEnabled = false;
                        cmbCompany.IsEnabled = false;


                        //By Anjali................
                        FillPackage(((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId);
                        //........................



                    }
                }
                else
                {

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("", "Error occured While Processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };

            Client1.ProcessAsync(BizAction1, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client1.CloseAsync();
        }

        public void FillPatientSponsorDetailsNew()
        {
            try
            {
                clsGetPatientSponsorListBizActionVO BizAction = new clsGetPatientSponsorListBizActionVO();
                BizAction.SponsorID = 0;
                //if (LinkPatientID > 0 && LinkPatientUnitID > 0)
                //{
                //    BizAction.PatientID = LinkPatientID;
                //    BizAction.PatientUnitID = LinkPatientUnitID;
                //}
                //else
                //{
                BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                //}
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails != null)
                    {
                        //((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails.ForEach(z => z.ID = z.PatientSourceID);  //// added by ashish Z. on Dated 260716 for checking the Multiple Sponsor Billing on on frmBill
                        //cmbPatientSource.ItemsSource = ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails;
                        //if (((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails != null || ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails.Count > 0)
                        //{
                        //    cmbPatientSource.SelectedValue = ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails[((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails.Count - 1].PatientSourceID;
                        //}

                        PatientSponsorDetails = ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails;      // addede on 08082018 to check selected sponsor is valid or not
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                //Indicatior.Close();
                // throw;
            }
        }

        #endregion

        #region Calculate age from birthdate
        /// <summary>
        /// Purpose:Get Age from birthdate.
        /// </summary>
        /// <param name="Datevalue"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private string ConvertDate(object Datevalue, string parameter)
        {
            if (Datevalue != null)
            {
                try
                {
                    DateTime BirthDate = (DateTime)Datevalue;
                    TimeSpan difference = DateTime.Now.Subtract(BirthDate);

                    //return date.ToString(parameter.ToString());
                    // This is to convert the timespan to datetime object
                    DateTime age = DateTime.MinValue + difference;

                    // Min value is 01/01/0001
                    // Actual age is say 24 yrs, 9 months and 3 days represented as timespan
                    // Min Valye + actual age = 25 yrs , 10 months and 4 days.
                    // subtract our addition or 1 on all components to get the actual date.
                    string result = "";
                    switch (parameter.ToString().ToUpper())
                    {
                        case "YY":
                            result = (age.Year - 1).ToString();
                            break;
                        case "MM":
                            result = (age.Month - 1).ToString();
                            break;
                        case "DD":
                            int day = BirthDate.Day;
                            int curday = DateTime.Now.Day;
                            int dif = 0;
                            if (day > curday)
                                dif = (curday + 30) - day;
                            else
                                dif = curday - day;
                            result = dif.ToString();
                            //result = (age.Day - 1).ToString();
                            break;
                        default:
                            result = (age.Year - 1).ToString();
                            break;
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    string err = ex.Message;
                    return string.Empty;
                }
            }
            else
                return string.Empty;
        }

        private DateTime? ConvertDateBack(string parameter, int value, DateTime? DateTobeConvert)
        {
            try
            {
                DateTime BirthDate;
                if (DateTobeConvert != null && parameter.ToString().ToUpper() != "YY")
                    BirthDate = DateTobeConvert.Value;
                else
                    BirthDate = DateTime.Now;


                int mValue = Int32.Parse(value.ToString());

                switch (parameter.ToString().ToUpper())
                {
                    case "YY":
                        BirthDate = BirthDate.AddYears(-mValue);

                        break;
                    case "MM":
                        BirthDate = BirthDate.AddMonths(-mValue);
                        // result = (age.Month - 1).ToString();
                        break;
                    case "DD":
                        //result = (age.Day - 1).ToString();
                        BirthDate = BirthDate.AddDays(-mValue);
                        break;
                    default:
                        BirthDate = BirthDate.AddYears(-mValue);
                        break;
                }
                return BirthDate;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return null;
            }

        }

        #endregion

        #region Get Visit details
        /// <summary>
        /// Purpose:Check patient visit.
        /// </summary>
        private void CheckVisit()
        {
            WaitIndicator ind = new WaitIndicator();
            ind.Show();

            clsGetVisitBizActionVO BizAction = new clsGetVisitBizActionVO();
            BizAction.Details.PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.Details.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizAction.GetLatestVisit = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetVisitBizActionVO)arg.Result).Details != null && ((clsGetVisitBizActionVO)arg.Result).Details.ID > 0 && ((clsGetVisitBizActionVO)arg.Result).Details.VisitStatus == true)
                    {
                        ind.Close();
                    }
                    else
                    {
                        ind.Close();
                        //MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //       new MessageBoxControl.MessageBoxChildWindow("", "Visit is not marked for the Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //msgW1.Show();
                        return;
                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void GetVisitDetails()
        {
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
                        //CheckVisitType(((clsGetVisitBizActionVO)arg.Result).Details.VisitTypeID);
                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        #endregion

        #region Delete Items from list

        /// <summary>
        /// Purpose:Delete Items from Datagrid.
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


                        if (dgPharmacyItems.SelectedItem == null && ISPrescription == true) //***//
                        {
                            ISPrescription = false;
                            txtReferenceDoctor.IsEnabled = true;
                            txtReferenceDoctor.RaiseValidationError();
                            txtReferenceDoctor.Focus();
                        }
                    }
                };

                msgWD.Show();
            }
        }
        #endregion

        #region Validation

        private void txtReferenceDoctor_GotFocus(object sender, RoutedEventArgs e)
        {
            FillDoctor();
        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtYY_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (txtYY.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtYY.Text.Trim()))
            //{
            //    int val = int.Parse(txtYY.Text.Trim());
            //    if (val > 0)
            //    {
            //        dtpDOB.SelectedDate = ConvertDateBack("YY", val, dtpDOB.SelectedDate);
            //    }
            //}

            //if (txtMM.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtMM.Text.Trim()))
            //{
            //    int val = int.Parse(txtMM.Text.Trim());
            //    if (val > 0)
            //    {
            //        dtpDOB.SelectedDate = ConvertDateBack("MM", val, dtpDOB.SelectedDate);
            //    }
            //}

            //if (txtDD.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtDD.Text.Trim()))
            //{
            //    int val = int.Parse(txtDD.Text.Trim());
            //    if (val > 0)
            //        dtpDOB.SelectedDate = ConvertDateBack("DD", val, dtpDOB.SelectedDate);
            //}
            //txtMM.SelectAll();

            CalculateBirthDate();
            txtMM.SelectAll();
        }

        private void txtMM_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (txtYY.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtYY.Text.Trim()))
            //{
            //    int val = int.Parse(txtYY.Text.Trim());
            //    if (val > 0)
            //    {
            //        dtpDOB.SelectedDate = ConvertDateBack("YY", val, dtpDOB.SelectedDate);
            //    }
            //}

            //if (txtMM.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtMM.Text.Trim()))
            //{
            //    int val = int.Parse(txtMM.Text.Trim());
            //    if (val > 0)
            //    {
            //        dtpDOB.SelectedDate = ConvertDateBack("MM", val, dtpDOB.SelectedDate);
            //    }
            //}

            //if (txtDD.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtDD.Text.Trim()))
            //{
            //    int val = int.Parse(txtDD.Text.Trim());
            //    if (val > 0)
            //        dtpDOB.SelectedDate = ConvertDateBack("DD", val, dtpDOB.SelectedDate);
            //}

            //txtDD.SelectAll();

            CalculateBirthDate();
            txtDD.SelectAll();
        }

        private void txtDD_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (txtYY.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtYY.Text.Trim()))
            //{
            //    int val = int.Parse(txtYY.Text.Trim());
            //    if (val > 0)
            //    {
            //        dtpDOB.SelectedDate = ConvertDateBack("YY", val, dtpDOB.SelectedDate);
            //    }
            //}

            //if (txtMM.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtMM.Text.Trim()))
            //{
            //    int val = int.Parse(txtMM.Text.Trim());
            //    if (val > 0)
            //    {
            //        dtpDOB.SelectedDate = ConvertDateBack("MM", val, dtpDOB.SelectedDate);
            //    }
            //}

            //if (txtDD.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtDD.Text.Trim()))
            //{
            //    int val = int.Parse(txtDD.Text.Trim());
            //    if (val > 0)
            //        dtpDOB.SelectedDate = ConvertDateBack("DD", val, dtpDOB.SelectedDate);
            //}
            CalculateBirthDate();
        }

        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (string.IsNullOrEmpty(((TextBox)sender).Text))
            //{

            //}
            //else if (!((TextBox)sender).Text.IsNumberValid())
            //{
            //    ((TextBox)sender).Text = textBefore;
            //    ((TextBox)sender).SelectionStart = selectionStart;
            //    ((TextBox)sender).SelectionLength = selectionLength;
            //    textBefore = "";
            //    selectionStart = 0;
            //    selectionLength = 0;
            //}

            if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
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

        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFirstName.Text = txtFirstName.Text.ToTitleCase();
        }

        private void TextName_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsPersonNameValid())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtMiddleName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtMiddleName.Text = txtMiddleName.Text.ToTitleCase();
        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtLastName.Text = txtLastName.Text.ToTitleCase();
        }

        private void dtpDOB_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (((clsPatientVO)this.DataContext).GeneralDetails.DateOfBirth > DateTime.Now)
            //{
            //    dtpDOB.SetValidation("Date Of Birth Can Not Be Greater Than Today");
            //    dtpDOB.RaiseValidationError();
            //    txtYY.Text = "0";
            //    txtMM.Text = "0";
            //    txtDD.Text = "0";
            //}
            //else
            //{
            //    dtpDOB.ClearValidationError();
            //    txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
            //    txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
            //    txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");
            //}

            //txtYY.SelectAll();

            if (dtpDOB.SelectedDate != null)
            {
                txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");
            }
            txtYY.SelectAll();
        }

        private void WaterMarkTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((WaterMarkTextbox)sender).Text;
            selectionStart = ((WaterMarkTextbox)sender).SelectionStart;
            selectionLength = ((WaterMarkTextbox)sender).SelectionLength;
        }

        private void WaterMarkTextbox_OnTextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((WaterMarkTextbox)sender).Text))
            {
                if (((WaterMarkTextbox)sender).Text.Length == 1)
                {
                    if (!((WaterMarkTextbox)sender).Text.IsValueNotZero())
                    {
                        ((WaterMarkTextbox)sender).Text = textBefore;
                        ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                        ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                }
                else if (!((WaterMarkTextbox)sender).Text.IsItNumber())//if (!((WaterMarkTextbox)sender).Text.IsNumberValid())
                {
                    ((WaterMarkTextbox)sender).Text = textBefore;
                    ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                    ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
                else if (((WaterMarkTextbox)sender).Text.Length > 14)
                {
                    ((WaterMarkTextbox)sender).Text = textBefore;
                    ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                    ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        private void txtContactNo1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtContactNo.Text.Trim()))
            {
                if (Extensions.IsPhoneNumberValid(txtContactNo.Text) == false)
                {
                    txtContactNo.Textbox.SetValidation("Please Enter Number In Correct Format");
                    txtContactNo.Textbox.RaiseValidationError();

                }
                else
                    txtContactNo.Textbox.ClearValidationError();
            }
        }

        private void txtReferenceDoctor_LostFocus(object sender, RoutedEventArgs e)
        {
            txtReferenceDoctor.Text = txtReferenceDoctor.Text.ToTitleCase();

        }

        private bool ChkValidation()
        {
            bool result = true;

            if (txtReferenceDoctor.Text == "" && ISPrescription == false)
            {
                txtReferenceDoctor.SetValidation("Please Enter Reference Doctor");
                txtReferenceDoctor.RaiseValidationError();
                result = false;
                txtReferenceDoctor.Focus();
            }
            else
                txtReferenceDoctor.ClearValidationError();

            //if (txtLastName.Text == "")
            //{
            //    txtLastName.SetValidation("Please Enter Surname");
            //    txtLastName.RaiseValidationError();
            //    result = false;
            //    txtLastName.Focus();
            //}
            //else
            //    txtLastName.ClearValidationError();

            //if (txtMobileCountryCode.Text == null || txtMobileCountryCode.Text.Trim() == "")
            //{
            //    txtMobileCountryCode.Textbox.SetValidation("Mobile Country Code Is Required");
            //    txtMobileCountryCode.Textbox.RaiseValidationError();
            //    txtMobileCountryCode.Focus();

            //    result = false;
            //}
            //else
            //    txtMobileCountryCode.Textbox.ClearValidationError();

            //if (txtContactNo.Text == null || txtContactNo.Text.Trim() == "")
            //{
            //    txtContactNo.Textbox.SetValidation("Mobile Number Is Required");
            //    txtContactNo.Textbox.RaiseValidationError();
            //    txtContactNo.Focus();

            //    result = false;
            //}
            //else 

            if (txtContactNo.Text.Trim() != "")
            {
                if (txtContactNo.Text.Trim().Length > 14)
                {
                    txtContactNo.Textbox.SetValidation("Mobile Number Should Be 14 Digit");
                    txtContactNo.Textbox.RaiseValidationError();
                    txtContactNo.Focus();
                    result = false;
                }
            }
            else
                txtContactNo.Textbox.ClearValidationError();

            if ((MasterListItem)cmbGender.SelectedItem == null)
            {
                cmbGender.TextBox.SetValidation("Gender Is Required");
                cmbGender.TextBox.RaiseValidationError();
                cmbGender.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbGender.SelectedItem).ID == 0)
            {
                cmbGender.TextBox.SetValidation("Gender Is Required");
                cmbGender.TextBox.RaiseValidationError();
                cmbGender.Focus();
                result = false;
            }
            else
                cmbGender.TextBox.ClearValidationError();

            if (txtYY.Text != "")
            {
                if (Convert.ToInt16(txtYY.Text) > 120)
                {
                    txtYY.SetValidation("Age Can Not Be Greater Than 121");
                    txtYY.RaiseValidationError();
                    txtYY.Focus();
                    result = false;
                    ClickedFlag = 0;
                }
                else
                    txtYY.ClearValidationError();
            }
            if (string.IsNullOrEmpty(txtYY.Text) && string.IsNullOrEmpty(txtMM.Text) && string.IsNullOrEmpty(txtDD.Text))
            {
                txtYY.SetValidation("Age Is Required");
                txtYY.RaiseValidationError();
                txtMM.SetValidation("Age Is Required");
                txtMM.RaiseValidationError();
                txtDD.SetValidation("Age Is Required");
                txtDD.RaiseValidationError();
                result = false;
                txtYY.Focus();

            }
            else
            {
                txtYY.ClearValidationError();
                txtMM.ClearValidationError();
                txtDD.ClearValidationError();
            }


            if (txtFirstName.Text == "")
            {
                txtFirstName.SetValidation("Please Enter First Name");
                txtFirstName.RaiseValidationError();
                result = false;
                txtFirstName.Focus();
            }
            else
                txtFirstName.ClearValidationError();


            if (cmbPatientCategory.SelectedItem == null)
            {
                cmbPatientCategory.TextBox.SetValidation("Patient Category Is Required");
                cmbPatientCategory.TextBox.RaiseValidationError();
                cmbPatientCategory.Focus();
                result = false;
            }
            else if (cmbPatientCategory.SelectedItem != null && ((MasterListItem)cmbPatientCategory.SelectedItem).ID == 0)
            {
                cmbPatientCategory.TextBox.SetValidation("Patient Category Is Required");
                cmbPatientCategory.TextBox.RaiseValidationError();
                cmbPatientCategory.Focus();
                result = false;
            }
            else
                cmbPatientCategory.TextBox.ClearValidationError();

            if (cmbPatientSource.SelectedItem == null)
            {
                cmbPatientSource.TextBox.SetValidation("Patient Source Is Required");
                cmbPatientSource.TextBox.RaiseValidationError();
                cmbPatientSource.Focus();
                result = false;
            }
            else if (cmbPatientSource.SelectedItem != null && ((MasterListItem)cmbPatientSource.SelectedItem).ID == 0)
            {
                cmbPatientSource.TextBox.SetValidation("Patient Source Is Required");
                cmbPatientSource.TextBox.RaiseValidationError();
                cmbPatientSource.Focus();
                result = false;
            }
            else
                cmbPatientSource.TextBox.ClearValidationError();

            if (cmbCompany.SelectedItem == null)
            {
                cmbCompany.TextBox.SetValidation("Patient Source Is Required");
                cmbCompany.TextBox.RaiseValidationError();
                cmbCompany.Focus();
                result = false;
            }
            else if (cmbCompany.SelectedItem != null && ((MasterListItem)cmbCompany.SelectedItem).ID == 0)
            {
                cmbCompany.TextBox.SetValidation("Company Is Required");
                cmbCompany.TextBox.RaiseValidationError();
                cmbCompany.Focus();
                result = false;
            }
            else
                cmbCompany.TextBox.ClearValidationError();

            if (cmbTariff.SelectedItem == null)
            {
                cmbTariff.TextBox.SetValidation("Tariff Source Is Required");
                cmbTariff.TextBox.RaiseValidationError();
                cmbTariff.Focus();
                result = false;
            }
            else if (cmbTariff.SelectedItem != null && ((MasterListItem)cmbTariff.SelectedItem).ID == 0)
            {
                cmbTariff.TextBox.SetValidation("Tariff Is Required");
                cmbTariff.TextBox.RaiseValidationError();
                cmbTariff.Focus();
                result = false;
            }
            else
                cmbTariff.TextBox.ClearValidationError();

            //if ((MasterListItem)cmbCostingDivision.SelectedItem == null || ((MasterListItem)cmbCostingDivision.SelectedItem).ID == 0)
            //{
            //    cmbCostingDivision.TextBox.SetValidation("Please Select Costing Division ");
            //    cmbCostingDivision.TextBox.RaiseValidationError();
            //    cmbCostingDivision.Focus();
            //    return false;
            //}
            //else
            //    cmbCostingDivision.ClearValidationError();
            if (PharmacyItems != null && PharmacyItems.Count > 0)
            {
                long TransactionUOMID = 0;

                foreach (var item in PharmacyItems)
                {
                    //Log Write here   

                    // lineNumber = stackFrame.GetFileLineNumber();

                    objGUID = new Guid();

                    TransactionUOMID = 0;
                    if (item.SelectedUOM != null)
                    {
                        TransactionUOMID = item.SelectedUOM.ID;
                    }
                    else
                    {
                        TransactionUOMID = item.TransactionUOMID;
                    }
                    if (IsAuditTrail)
                    {
                        LogInformation = new LogInfo();
                        LogInformation.guid = objGUID;
                        LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        LogInformation.TimeStamp = DateTime.Now;
                        LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                        LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                        LogInformation.Message = " IX : Line Number : " //+ Convert.ToString(lineNumber)
                                                                + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                                + " , StoreID : " + Convert.ToString(StoreID) + " "
                                                                + " , ItemID : " + Convert.ToString(item.ItemID) + " "
                                                                + " , BatchID : " + Convert.ToString(item.BatchID) + " "
                                                                + " , BatchCode : " + Convert.ToString(item.BatchCode) + " "
                                                                + " , ExpiryDate : " + Convert.ToString(item.ExpiryDate) + " "
                                                                + " , Transaction UOMID : " + Convert.ToString(TransactionUOMID) + " "
                                                                + " , Transaction Quantity (InputTransactionQuantity) : " + Convert.ToString(item.Quantity) + " "
                                                                + " , BaseConversionFactor : " + Convert.ToString(item.BaseConversionFactor) + " "
                                                                + " , Base Quantity (TransactionQuantity) : " + Convert.ToString(item.BaseQuantity) + " "
                                                                + " , Base UOMID : " + Convert.ToString(item.BaseUOMID) + " "
                                                                + " , SUOMID : " + Convert.ToString(item.SUOMID) + " "
                                                                + " , StockCF : " + Convert.ToString(item.ConversionFactor) + " "
                                                                + " , Stocking Quantity : " + Convert.ToString(item.Quantity * item.ConversionFactor) + " "
                                                                + " , PUOMID : " + Convert.ToString(item.PUOMID) + " "
                                                                + " , ConcessionPercentage : " + Convert.ToString(item.ConcessionPercentage) + " "
                                                                + " , ConcessionAmount : " + Convert.ToString(item.ConcessionAmount) + " "
                                                                + " , AvailableStockInBase : " + Convert.ToString(item.AvailableStockInBase) + "\r\n";

                        LogInfoList.Add(LogInformation);
                    }
                    //CallLogBizAction(LogBizAction);
                    ////


                    if (item.IsPrescribedMedicine == true)
                        //if (PharmacyItems.Where(s => s.IsPrescribedMedicine == true && s.ItemID == item.ItemID).Sum(x => x.BaseQuantity) != item.NewPendingQuantity)  //commented by neena
                        if (PharmacyItems.Where(s => s.IsPrescribedMedicine == true && s.ItemID == item.ItemID).Sum(x => x.BaseQuantity) != item.ActualPrescribedBaseQuantity) //added by neena
                        {
                            if (txtReasonForVariance.Text == string.Empty)
                            {
                                txtReasonForVariance.SetValidation("Prescribed Quantity varies From Actual Quantity, Reason For Variance Is Required");
                                txtReasonForVariance.RaiseValidationError();
                                result = false;
                                txtReasonForVariance.Focus();
                                break;
                            }
                            else
                                txtReasonForVariance.ClearValidationError();

                        }
                }




                //if (PharmacyItems.Where(s => s.IsPrescribedMedicine == true && s.it).Any()) //   if(PharmacyItems.Where(s => s.IsPrescribedMedicine ==true && s.NewPendingQuantity != s.BaseQuantity).Any())
                //{
                //    if (txtReasonForVariance.Text == string.Empty)
                //        {
                //            txtReasonForVariance.SetValidation("Prescribed Quantity varies From Actual Quantity, Reason For Variance Is Required");
                //            txtReasonForVariance.RaiseValidationError();
                //            result = false;
                //            txtReasonForVariance.Focus();
                //        }
                //        else
                //        txtReasonForVariance.ClearValidationError();
                //}


            }

            if (PharmacyItems != null && PharmacyItems.Count > 0)
            {
                foreach (var item in PharmacyItems)
                {
                    if (item.IsPrescribedMedicine == true)
                        //if (PharmacyItems.Where(s => s.IsPrescribedMedicine == true && s.ItemID == item.ItemID).Sum(x => x.BaseQuantity) != item.NewPendingQuantity)  //commented by neena
                        if (PharmacyItems.Where(s => s.IsPrescribedMedicine == true && s.PrescriptionID == item.PrescriptionID).Sum(x => x.BaseQuantity) != item.TotalNewPendingQuantity) //added by neena
                        {
                            if (txtReasonForVariance.Text == string.Empty)
                            {
                                txtReasonForVariance.SetValidation("Prescribed Quantity varies From Actual Quantity, Reason For Variance Is Required");
                                txtReasonForVariance.RaiseValidationError();
                                result = false;
                                txtReasonForVariance.Focus();
                                break;
                            }
                        }
                        else
                            txtReasonForVariance.ClearValidationError();
                }
            }





            return result;
        }

        #endregion


        #region Print Bill
        /// <summary>
        /// Purpose:Print pharmacy bill.
        /// </summary>
        /// <param name="iBillId"></param>
        private void PrintBill(long iBillId, long PrintID)
        {
            if (iBillId > 0)
            {
                #region added by Prashant Channe to read reports config on 5thDec2019
                string StrConfigReportsDir = ((IApplicationConfiguration)App.Current).CurrentUser.ReportsFolder;
                string URL = null;
                #endregion
                //long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //string LoginUserName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UserName;
                //string URL = "../Reports/OPD/PharmacyBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&ReportID=" + 2 + "&PrintFomatID=" + PrintID;               
                //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                long PackageID = 0;
                if (cmbApplicabelPackage.SelectedItem != null)
                {
                    PackageID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID;
                }
                long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                string LoginUserName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UserName;

                //Added by Prashant Channe on 5thDec2019 to configure reports folder for fertilitypoint
                if (!string.IsNullOrEmpty(StrConfigReportsDir))
                {
                    URL = "../" + StrConfigReportsDir + "/OPD/PharmacyBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&ReportID=" + 2 + "&PrintFomatID=" + PrintID + "&PackageID=" + PackageID;
                }
                else
                {
                    // string URL = "../Reports/OPD/PharmacyBillCash.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&LoginUserName=" + LoginUserName + "&PrintFomatID=" + PrintID; ;
                    URL = "../Reports/OPD/PharmacyBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&ReportID=" + 2 + "&PrintFomatID=" + PrintID + "&PackageID=" + PackageID;
                }
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }
        #endregion

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            //((IApplicationConfiguration)App.Current).FillMenu("Find Patient");
        }

        #region Add Prescription items
        private void SaveSponsor(long PPatientID, long PPatientUnitID)
        {
            clsPatientSponsorVO ObjSponsor = new clsPatientSponsorVO()
               {
                   //PatientId = PPatientID,
                   //PatientUnitId = PPatientUnitID,
                   //PatientSourceID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientSourceID,
                   //CompanyID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfCompanyID,
                   //TariffID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID

                   PatientId = PPatientID,
                   PatientUnitId = PPatientUnitID,
                   PatientCategoryID = ((MasterListItem)cmbPatientCategory.SelectedItem).ID,
                   PatientSourceID = ((MasterListItem)cmbPatientSource.SelectedItem).ID,
                   CompanyID = ((MasterListItem)cmbCompany.SelectedItem).ID,
                   TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID
               };

            clsAddPatientSponsorBizActionVO BizAction = new clsAddPatientSponsorBizActionVO();
            BizAction.PatientSponsorDetails = ObjSponsor;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("", "Error occured While Saving Sponsor.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void lnkAddItemsFromPrescription_Click(object sender, RoutedEventArgs e)
        {

            //if (VisitId > 0 && PatientId > 0)
            //{
            //    DrugsForPatientPrescription DrugSearch = new DrugsForPatientPrescription();
            //    DrugSearch.StoreID = StoreID;
            //    DrugSearch.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;

            //    if (StoreID == 0)
            //        DrugSearch.cmbStore.IsEnabled = true;
            //    else
            //        DrugSearch.cmbStore.IsEnabled = false;

            //    DrugSearch.VisitID = VisitId; // ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;

            //    DrugSearch.OnSaveButton_Click += new RoutedEventHandler(DrugSearch_OnSaveButton_Click);
            //    DrugSearch.Show();
            //}
            //else
            //{
            //    string strMsg = "Visit Not Marked For The Patient";
            //    MessageBoxControl.MessageBoxChildWindow msgWg =
            //               new MessageBoxControl.MessageBoxChildWindow("", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
            //    msgWg.Show();
            //}

            if (VisitId > 0 && PatientId > 0)
            {
                if (PharmacyItems == null)
                    PharmacyItems = new ObservableCollection<clsItemSalesDetailsVO>();
                NewDrugsForPatientPrescription DrugSearch = new NewDrugsForPatientPrescription();

                StoreID = ((MasterListItem)cmbStore.SelectedItem).ID; //added by neena
                DrugSearch.StoreID = StoreID;
                DrugSearch.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;

                if (StoreID == 0)
                    DrugSearch.cmbStore.IsEnabled = true;
                else
                    DrugSearch.cmbStore.IsEnabled = false;

                DrugSearch.VisitID = VisitId; // ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;

                DrugSearch.OnSaveButton_Click += new RoutedEventHandler(DrugSearch_OnSaveButton_Click);
                DrugSearch.Show();
            }
            else
            {
                string strMsg = "Visit Not Marked For The Patient";
                MessageBoxControl.MessageBoxChildWindow msgWg =
                           new MessageBoxControl.MessageBoxChildWindow("", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                msgWg.Show();
            }

        }

        #region OLD Commented By CDS
        //void DrugSearch_OnSaveButton_Click(object sender, RoutedEventArgs e)
        //{
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
        //                        IsPrescribedMedicine = Itemswin.SelectedDrug.IsPrescribedMedicine,
        //                        BatchID = item.BatchID,
        //                        BatchCode = item.BatchCode,
        //                        ExpiryDate = item.ExpiryDate,
        //                        Quantity = Convert.ToDouble(Itemswin.SelectedDrug.Quantity),
        //                        MRP = item.MRP,
        //                        Amount = (item.MRP * Convert.ToDouble(Itemswin.SelectedDrug.Quantity)),
        //                        AvailableQuantity = item.AvailableStock,
        //                        ConcessionPercentage = item.DiscountOnSale,
        //                        VATPercent = item.VATPerc,

        //                        InclusiveOfTax = (bool)Itemswin.SelectedDrug.IsInclusiveOfTax,
        //                        CategoryId = item.CategoryId,
        //                        GroupId = item.GroupId,
        //                        Manufacture = item.Manufacturer,
        //                        PregnancyClass = item.PreganancyClass
        //                    });
        //                }
        //            }

        //            //CalculatePharmacySummary();
        //            GetPackageItemDiscount();

        //            dgPharmacyItems.Focus();
        //            dgPharmacyItems.UpdateLayout();
        //            dgPharmacyItems.SelectedIndex = PharmacyItems.Count - 1;

        //            if (!string.IsNullOrEmpty(strError.ToString()))
        //            {
        //                string strMsg = "Items Already Added : " + strError.ToString();

        //                MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                           new MessageBoxControl.MessageBoxChildWindow("", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //                msgW1.Show();
        //            }
        //        }
        //    }
        //}
        #endregion

        void DrugSearch_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {

            NewDrugsForPatientPrescription Itemswin = (NewDrugsForPatientPrescription)sender;
            if (Itemswin.DialogResult == true)
            {

                if (Itemswin.SelectedDrug != null && Itemswin.SelectedBatches != null)
                {
                    //if (PharmacyItems != null && PharmacyItems.Count > 0 && PharmacyItems.Where(s => s.CompanyID != ((MasterListItem)cmbCompany.SelectedItem).ID || s.PatientSourceID != ((clsPatientSourceVO)cmbPatientSource.SelectedItem).ID || s.TariffID != ((MasterListItem)cmbTariff.SelectedItem).ID || s.PatientCategoryID != ((MasterListItem)cmbPatientCategory.SelectedItem).ID).Any())
                    //{
                    //      MessageBoxControl.MessageBoxChildWindow msgW2 =
                    //                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Multiple Sponsor Billing is not Allowed", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    //        msgW2.Show();
                    //}
                    //else
                    //{
                    StringBuilder strError = new StringBuilder();
                    StoreID = Itemswin.StoreID;

                    foreach (var item in Itemswin.ItemBatchList)
                    {
                        if (item.Status == true)
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
                                //PharmacyItems.Add(new clsItemSalesDetailsVO()
                                //{
                                //    ItemID = item.PrescribeDrugs.DrugID,
                                //    ItemName = item.PrescribeDrugs.DrugName,
                                //    IsPrescribedMedicine = item.PrescribeDrugs.IsPrescribedMedicine,
                                //    BatchID = item.BatchID,
                                //    BatchCode = item.BatchCode,
                                //    ExpiryDate = item.ExpiryDate,
                                //    Quantity = Convert.ToDouble(Itemswin.SelectedDrug.Quantity),
                                //    MRP = item.MRP,
                                //    Amount = (item.MRP * Convert.ToDouble(Itemswin.SelectedDrug.Quantity)),
                                //    AvailableQuantity = item.AvailableStock,
                                //    ConcessionPercentage = item.DiscountOnSale,
                                //    VATPercent = item.VATPerc,
                                //    InclusiveOfTax = (bool)item.PrescribeDrugs.IsInclusiveOfTax,
                                //    CategoryId = item.CategoryId,
                                //    GroupId = item.GroupId,
                                //    Manufacture = item.Manufacturer,
                                //    PregnancyClass = item.PreganancyClass,
                                //    PrescriptionDetailsID = item.PrescribeDrugs.ID,
                                //    PrescriptionDetailsUnitID = item.PrescribeDrugs.UnitID
                                //});

                                ISPrescription = true;
                                txtReferenceDoctor.IsEnabled = false;
                                txtReferenceDoctor.ClearValidationError();

                                clsItemSalesDetailsVO obj = new clsItemSalesDetailsVO();
                                obj.ItemCode = item.ItemCode;
                                obj.ItemID = item.PrescribeDrugs.DrugID;
                                obj.ItemName = item.PrescribeDrugs.DrugName;
                                obj.PrescribeQuantity = Convert.ToDouble(item.PrescribeDrugs.Quantity);
                                obj.NewPendingQuantity = Convert.ToDouble(item.PrescribeDrugs.NewPendingQuantity);
                                obj.IsPrescribedMedicine = item.PrescribeDrugs.IsPrescribedMedicine;
                                obj.BatchID = item.BatchID;
                                obj.BatchCode = item.BatchCode;
                                obj.ExpiryDate = item.ExpiryDate;
                                //obj.Quantity = Convert.ToDouble(Itemswin.SelectedDrug.Quantity);
                                obj.Quantity = Convert.ToDouble(item.PrescribeDrugs.NewPendingQuantity);
                                obj.MRP = item.MRP;
                                obj.Amount = (item.MRP * Convert.ToDouble(item.PrescribeDrugs.Quantity));
                                obj.AvailableQuantity = item.AvailableStock;
                                obj.ConcessionPercentage = item.DiscountOnSale;

                                if (cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)    // Package New Changes Added on 26042018
                                {
                                    obj.PackageBillID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageBillID;
                                    obj.PackageBillUnitID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageBillUnitID;
                                    obj.PackageID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID;                                     // Package New Changes Added on 03052018
                                }

                                //obj.VATPercent = item.VATPerc;
                                //obj.InclusiveOfTax = (bool)item.PrescribeDrugs.IsInclusiveOfTax;
                                obj.InclusiveOfTax = item.InclusiveOfTax;
                                obj.CategoryId = item.CategoryId;
                                obj.GroupId = item.GroupId;
                                obj.Manufacture = item.Manufacturer;
                                obj.PregnancyClass = item.PreganancyClass;
                                obj.PrescriptionDetailsID = item.PrescribeDrugs.ID;
                                obj.PrescriptionDetailsUnitID = item.PrescribeDrugs.UnitID;
                                //

                                if (txtMRNo.Text == "")     // Condition added on 16072018 as per discussed with AJ
                                {
                                    obj.SGSTtaxtype = item.SGSTtaxtypeSale;              //item.SGSTtaxtype;
                                    obj.SGSTapplicableon = item.SGSTapplicableonSale;    //item.SGSTapplicableon;
                                    obj.CGSTtaxtype = item.CGSTtaxtypeSale;              //item.CGSTtaxtype;
                                    obj.CGSTapplicableon = item.CGSTapplicableonSale;    //item.CGSTapplicableon;
                                    obj.IGSTtaxtype = item.IGSTtaxtypeSale;              //item.IGSTtaxtype;
                                    obj.IGSTapplicableon = item.IGSTapplicableonSale;    //item.IGSTapplicableon;

                                    //ObjAddItem.SGSTPercent = Convert.ToDouble(item.SGSTPercent) == 0 ? Convert.ToDouble(item.IGSTPercent) != 0 ? Convert.ToDouble(item.IGSTPercent) / 2 : 0 : Convert.ToDouble(item.SGSTPercent);
                                    //ObjAddItem.CGSTPercent = Convert.ToDouble(item.CGSTPercent) == 0 ? Convert.ToDouble(item.CGSTPercent) != 0 ? Convert.ToDouble(item.CGSTPercent) / 2 : 0 : Convert.ToDouble(item.CGSTPercent);

                                    obj.SGSTPercent = Convert.ToDouble(item.SGSTPercentSale) == 0 ? Convert.ToDouble(item.IGSTPercentSale) != 0 ? Convert.ToDouble(item.IGSTPercentSale) / 2 : 0 : Convert.ToDouble(item.SGSTPercentSale);
                                    obj.CGSTPercent = Convert.ToDouble(item.CGSTPercentSale) == 0 ? Convert.ToDouble(item.IGSTPercentSale) != 0 ? Convert.ToDouble(item.IGSTPercentSale) / 2 : 0 : Convert.ToDouble(item.CGSTPercentSale);

                                    obj.IGSTPercent = 0;
                                }

                                //by Anjali.............................
                                obj.Shelfname = item.Shelfname;
                                obj.Containername = item.Containername;
                                obj.Rackname = item.Rackname;
                                obj.AvailableStockInBase = item.AvailableStockInBase;

                                //obj.ActualPrescribedBaseQuantity = Convert.ToSingle(item.PrescribeDrugs.NewPendingQuantity);  commented by neena

                                //added by neena
                                //obj.ActualPrescribedBaseQuantity = Convert.ToSingle(item.PrescribeDrugs.NewPendingQuantity * item.SellingCF);
                                obj.ActualPrescribedBaseQuantity = Convert.ToSingle(item.PrescribeDrugs.NewPendingQuantity);
                                obj.TotalNewPendingQuantity = Convert.ToSingle(item.PrescribeDrugs.TotalNewPendingQuantity);
                                obj.PrescriptionID = item.PrescribeDrugs.PrescriptionID;
                                //

                                obj.ItemVatType = item.ItemVatType;

                                obj.StockUOM = item.SUOM;
                                obj.PurchaseUOM = item.PUOM;
                                obj.PUOM = item.PUOM;
                                obj.MainPUOM = item.PUOM;
                                obj.SUOM = item.SUOM;

                                obj.ConversionFactor = 1;
                                obj.PUOMID = item.PUM;
                                obj.SUOMID = item.SUM;
                                obj.BaseUOMID = item.BaseUM;
                                obj.BaseUOM = item.BaseUMString;
                                obj.SellingUOMID = item.SellingUM;
                                obj.SellingUOM = item.SellingUMString;
                                obj.MainMRP = Convert.ToSingle(item.MRP);
                                obj.MainRate = Convert.ToSingle(item.PurchaseRate);

                                //obj.BaseQuantity =  Convert.ToSingle(Itemswin.SelectedDrug.NewPendingQuantity);

                                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsSellBySellingUnit == true)
                                {
                                    obj.SelectedUOM = new MasterListItem(obj.SellingUOMID, obj.SellingUOM);
                                    float CalculatedFromCF = item.SellingCF / item.StockingCF;

                                    //obj.Quantity = Math.Ceiling(item.PrescribeDrugs.NewPendingQuantity / item.SellingCF);  //commented by neena

                                    //added by neena
                                    obj.Quantity = Math.Ceiling(obj.ActualPrescribedBaseQuantity / item.SellingCF);
                                    //

                                    obj.ConversionFactor = Convert.ToSingle(CalculatedFromCF);
                                    obj.BaseConversionFactor = item.SellingCF;

                                    obj.BaseQuantity = Convert.ToSingle(obj.Quantity * item.SellingCF);

                                    //obj.MainRate = Convert.ToSingle(item.PurchaseRate);
                                    obj.MainRate = Convert.ToSingle(item.PurchaseRate) / Convert.ToSingle(item.StockingCF);
                                    //obj.BaseRate = Convert.ToSingle(obj.MainRate);
                                    obj.BaseRate = Convert.ToSingle(item.PurchaseRate) / Convert.ToSingle(item.StockingCF);
                                    //obj.PurchaseRate = obj.MainRate * item.SellingCF;
                                    obj.PurchaseRate = obj.BaseRate * item.SellingCF;

                                    //obj.MainMRP = Convert.ToSingle(item.MRP);
                                    obj.MainMRP = Convert.ToSingle(item.MRP) / Convert.ToSingle(item.StockingCF);
                                    //obj.BaseMRP = Convert.ToSingle(obj.MainMRP);
                                    obj.BaseMRP = Convert.ToSingle(item.MRP) / Convert.ToSingle(item.StockingCF);
                                    //obj.MRP = obj.MainMRP * item.SellingCF;
                                    obj.MRP = obj.BaseMRP * item.SellingCF;

                                    ////ObjAddItem.MainRate = Convert.ToSingle(item.PurchaseRate);
                                    //ObjAddItem.MainRate = Convert.ToSingle(item.PurchaseRate) / Convert.ToSingle(item.StockingCF);
                                    ////ObjAddItem.PurchaseRate = item.PurchaseRate * item.SellingCF;
                                    //ObjAddItem.BaseRate = Convert.ToSingle(item.PurchaseRate) / Convert.ToSingle(item.StockingCF);
                                    //ObjAddItem.PurchaseRate = ObjAddItem.BaseRate * item.SellingCF;

                                    ////ObjAddItem.MainMRP = Convert.ToSingle(item.MRP);
                                    //ObjAddItem.MainMRP = Convert.ToSingle(item.MRP) / Convert.ToSingle(item.StockingCF);
                                    ////ObjAddItem.MRP = item.MRP * item.SellingCF;
                                    //ObjAddItem.BaseMRP = Convert.ToSingle(item.MRP) / Convert.ToSingle(item.StockingCF);
                                    //ObjAddItem.MRP = ObjAddItem.BaseMRP * item.SellingCF;

                                }
                                else
                                {
                                    obj.SelectedUOM = new MasterListItem(0, "--Select--");
                                }
                                //........................................
                                //For Multiple Sponser.........................
                                //if (cmbCompany.SelectedItem != null)
                                //    obj.CompanyID = ((MasterListItem)cmbCompany.SelectedItem).ID;
                                //if (cmbPatientSource.SelectedItem != null)
                                //    obj.PatientSourceID = ((clsPatientSourceVO)cmbPatientSource.SelectedItem).ID;
                                //if (cmbTariff.SelectedItem != null)
                                //    obj.TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;
                                //if (cmbPatientCategory.SelectedItem != null)
                                //    obj.PatientCategoryID = ((MasterListItem)cmbPatientCategory.SelectedItem).ID;
                                //.............................................

                                //Log Write here   

                                // lineNumber = stackFrame.GetFileLineNumber();

                                objGUID = new Guid();
                                if (IsAuditTrail)
                                {
                                    LogInformation = new LogInfo();
                                    LogInformation.guid = objGUID;
                                    LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                                    LogInformation.TimeStamp = DateTime.Now;
                                    LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                                    LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                                    LogInformation.Message = " X : Line Number : " //+ Convert.ToString(lineNumber)
                                                                            + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                                            + " , StoreID : " + Convert.ToString(StoreID) + " "
                                                                            + " , ItemID : " + Convert.ToString(item.ItemID) + " "
                                                                            + " , BatchID : " + Convert.ToString(item.BatchID) + " "
                                                                            + " , BatchCode : " + Convert.ToString(item.BatchCode) + " "
                                                                            + " , ExpiryDate : " + Convert.ToString(item.ExpiryDate) + " "
                                                                            + " , Transaction UOMID : " + Convert.ToString(obj.SelectedUOM.ID) + " "
                                                                            + " , Transaction Quantity (InputTransactionQuantity) : " + Convert.ToString(obj.Quantity) + " "
                                                                            + " , BaseConversionFactor : " + Convert.ToString(obj.BaseConversionFactor) + " "
                                                                            + " , Base Quantity (TransactionQuantity) : " + Convert.ToString(obj.BaseQuantity) + " "
                                                                            + " , Base UOMID : " + Convert.ToString(obj.BaseUOMID) + " "
                                                                            + " , Base SUOMID : " + Convert.ToString(obj.SUOMID) + " "
                                                                            + " , StockCF : " + Convert.ToString(obj.ConversionFactor) + " "
                                                                            + " , Stocking Quantity : " + Convert.ToString(obj.Quantity * obj.ConversionFactor) + " "
                                                                            + " , PUOMID : " + Convert.ToString(obj.PUOMID) + " "
                                                                            + " , AvailableStockInBase : " + Convert.ToString(obj.AvailableStockInBase) + "\r\n";

                                    LogInfoList.Add(LogInformation);
                                }
                                //CallLogBizAction(LogBizAction);
                                ////

                                PharmacyItems.Add(obj);

                            }
                        }
                    }
                    // }
                    CalculatePharmacySummary();
                    //  GetPackageItemDiscount();

                }
            }
        }
        #endregion

        // Added By CDS For Package 
        bool ConcessionFromPlan = false;

        //By Anjali......................
        float TotalCalculatedBudget;
        float CalculatedBudget;
        float TotalBudget;
        float pharmacyBudget;       // Package Change 17042017
        float pendingBudget;        // Package Change 17042017
        //...............................

        private void GetPackageItemDiscount()
        {

            clsApplyPackageDiscountRateToItems BizAction = new clsApplyPackageDiscountRateToItems();
            BizAction.objApplyItemPackageDiscountRateDetails = new PalashDynamics.ValueObjects.Billing.clsApplyPackageDiscountRateOnItemVO();
            BizAction.objApplyItemPackageDiscountRateDetails.PatientCatagoryL1 = ((MasterListItem)cmbPatientCategory.SelectedItem).ID;
            // BizAction.objApplyItemPackageDiscountRateDetails.PatientCatagoryL2 = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceID;
            BizAction.objApplyItemPackageDiscountRateDetails.PatientCatagoryL2 = ((MasterListItem)cmbPatientSource.SelectedItem).ID;
            if (cmbTariff.SelectedItem != null)
                BizAction.objApplyItemPackageDiscountRateDetails.PatientCatagoryL3 = ((MasterListItem)cmbTariff.SelectedItem).ID;
            if (cmbCompany.SelectedItem != null)
                BizAction.objApplyItemPackageDiscountRateDetails.CompanyID = ((MasterListItem)cmbCompany.SelectedItem).ID;

            string ItemIDs = "";
            if (PharmacyItems != null && PharmacyItems.Count > 0)
            {
                foreach (var item in PharmacyItems)
                {
                    ItemIDs = ItemIDs + item.ItemID;
                    ItemIDs = ItemIDs + ",";
                }

                if (ItemIDs.EndsWith(","))
                    ItemIDs = ItemIDs.Remove(ItemIDs.Length - 1, 1);
            }

            BizAction.objApplyItemPackageDiscountRateDetails.ItemIDs = ItemIDs;


            //By Anjali.................................
            BizAction.objApplyItemPackageDiscountRateDetails.PatientGenderID = ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID;
            BizAction.objApplyItemPackageDiscountRateDetails.PatientDateOfBirth = Convert.ToDateTime(((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth);
            BizAction.objApplyItemPackageDiscountRateDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.ID;
            BizAction.objApplyItemPackageDiscountRateDetails.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
            if (cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)
            {
                BizAction.objApplyItemPackageDiscountRateDetails.PackageID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID;
            }
            //..............................................


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                ClickedFlag = 0;
                if (arg.Error == null && arg.Result != null && ((clsApplyPackageDiscountRateToItems)arg.Result).objApplyItemPackageDiscountRate != null)
                {

                    if (((clsApplyPackageDiscountRateToItems)arg.Result).objApplyItemPackageDiscountRate != null)
                    {
                        clsApplyPackageDiscountRateToItems ItemDiscountList = ((clsApplyPackageDiscountRateToItems)arg.Result);

                        if (ItemDiscountList.objApplyItemPackageDiscountRate.Count > 0)
                        {
                            TotalCalculatedBudget = ItemDiscountList.objApplyItemPackageDiscountRate[0].CalculatedTotalBudget;
                            TotalBudget = ItemDiscountList.objApplyItemPackageDiscountRate[0].TotalBudget;
                            CalculatedBudget = ItemDiscountList.objApplyItemPackageDiscountRate[0].CalculatedBudget;
                        }


                        //foreach (var item in ItemDiscountList.objApplyItemPackageDiscountRate)
                        //{
                        //    foreach (var item1 in PharmacyItems)
                        //    {
                        //        if (item.ApplicableToAllDiscount > 0)
                        //        {
                        //            item1.ConcessionPercentage = item.DiscountedPercentage;
                        //            ConcessionFromPlan = true;
                        //        }
                        //        else
                        //        {
                        //            if (item.IsCategory == false && item.IsGroup == false)
                        //            {
                        //                if (item1.ItemID == item.ItemId)
                        //                {
                        //                    item1.ConcessionPercentage = item.DiscountedPercentage;
                        //                    ConcessionFromPlan = true;

                        //                    //By Anjali..........................
                        //                    item1.Budget = item.Budget;
                        //                    item1.CalculatedBudget = item.CalculatedBudget;
                        //                    item1.IsPackageForItem = true;
                        //                    item1.IsPackageForCategory = false;
                        //                    item1.IsPackageForGroup = false;
                        //                    //......................................
                        //                }
                        //                else
                        //                {
                        //                    item1.IsPackageForItem = false;
                        //                    item1.IsPackageForCategory = false;
                        //                    item1.IsPackageForGroup = false;
                        //                }

                        //            }
                        //            else if (item.IsCategory == true && item.IsGroup == false)
                        //            {
                        //                if (item1.CategoryId == item.CategoryId)
                        //                {
                        //                    item1.ConcessionPercentage = item.DiscountedPercentage;
                        //                    ConcessionFromPlan = true;
                        //                    //By Anjali..........................
                        //                    item1.Budget = item.Budget;
                        //                    item1.CalculatedBudget = item.CalculatedBudget;

                        //                    item1.IsPackageForItem = false;
                        //                    item1.IsPackageForCategory = true;
                        //                    item1.IsPackageForGroup = false;
                        //                    //......................................
                        //                }
                        //                else
                        //                {
                        //                    item1.IsPackageForItem = false;
                        //                    item1.IsPackageForCategory = false;
                        //                    item1.IsPackageForGroup = false;
                        //                }
                        //            }
                        //            else if (item.IsGroup == true && item.IsCategory == false)
                        //            {
                        //                if (item1.GroupId == item.GroupId)
                        //                {
                        //                    item1.ConcessionPercentage = item.DiscountedPercentage;
                        //                    ConcessionFromPlan = true;
                        //                    //By Anjali..........................
                        //                    item1.Budget = item.Budget;
                        //                    item1.CalculatedBudget = item.CalculatedBudget;

                        //                    item1.IsPackageForItem = false;
                        //                    item1.IsPackageForCategory = false;
                        //                    item1.IsPackageForGroup = true;
                        //                    //......................................
                        //                }
                        //                else
                        //                {
                        //                    item1.IsPackageForItem = false;
                        //                    item1.IsPackageForCategory = false;
                        //                    item1.IsPackageForGroup = false;
                        //                }
                        //            }
                        //        }
                        //    }
                        //}



                        if (TotalCalculatedBudget < TotalBudget)
                        {

                            foreach (var item in ItemDiscountList.objApplyItemPackageDiscountRate)
                            {
                                foreach (var item1 in PharmacyItems)
                                {
                                    if (item.ApplicableToAllDiscount > 0)
                                    {
                                        item1.ConcessionPercentage = item.DiscountedPercentage;
                                        ConcessionFromPlan = true;
                                    }
                                    else
                                    {
                                        if (item.IsCategory == false && item.IsGroup == false)
                                        {
                                            if (item1.ItemID == item.ItemId)
                                            {

                                                item1.Budget = item.Budget;
                                                item1.CalculatedBudget = item.CalculatedBudget;
                                                item1.ConcessionPercentage = item.DiscountedPercentage;
                                                ConcessionFromPlan = true;
                                                if (item1.Budget < item1.NetAmount)
                                                {
                                                    item1.NetAmount = item1.NetAmount - item1.Budget;
                                                }

                                                TotalCalculatedBudget = TotalCalculatedBudget - Convert.ToSingle(item1.NetAmount);
                                                item1.IsPackageForItem = true;
                                                item1.IsPackageForCategory = false;
                                                item1.IsPackageForGroup = false;

                                            }
                                            else
                                            {
                                                item1.IsPackageForItem = false;
                                                item1.IsPackageForCategory = false;
                                                item1.IsPackageForGroup = false;
                                            }

                                        }
                                        else if (item.IsCategory == true && item.IsGroup == false)
                                        {
                                            if (item1.CategoryId == item.CategoryId)
                                            {
                                                item1.Budget = item.Budget;
                                                item1.CalculatedBudget = item.CalculatedBudget;
                                                item1.ConcessionPercentage = item.DiscountedPercentage;
                                                ConcessionFromPlan = true;
                                                if (item1.Budget < (CalculatedBudget + item1.NetAmount))
                                                {
                                                    item1.ConcessionPercentage = 0;
                                                }
                                                else
                                                {
                                                    TotalCalculatedBudget = TotalCalculatedBudget - Convert.ToSingle(item1.NetAmount);
                                                    CalculatedBudget = CalculatedBudget + Convert.ToSingle(item1.NetAmount);
                                                }
                                                item1.IsPackageForItem = false;
                                                item1.IsPackageForCategory = true;
                                                item1.IsPackageForGroup = false;
                                            }
                                            else
                                            {
                                                item1.IsPackageForItem = false;
                                                item1.IsPackageForCategory = false;
                                                item1.IsPackageForGroup = false;
                                            }
                                        }
                                        else if (item.IsGroup == true && item.IsCategory == false)
                                        {
                                            if (item1.GroupId == item.GroupId)
                                            {
                                                item1.Budget = item.Budget;
                                                item1.CalculatedBudget = item.CalculatedBudget;
                                                item1.ConcessionPercentage = item.DiscountedPercentage;
                                                ConcessionFromPlan = true;
                                                if (item1.Budget < (CalculatedBudget + item1.NetAmount))
                                                {
                                                    item1.ConcessionPercentage = 0;
                                                }
                                                else
                                                {
                                                    TotalCalculatedBudget = TotalCalculatedBudget - Convert.ToSingle(item1.NetAmount);
                                                    CalculatedBudget = CalculatedBudget + Convert.ToSingle(item1.NetAmount);
                                                }
                                                item1.IsPackageForItem = false;
                                                item1.IsPackageForCategory = false;
                                                item1.IsPackageForGroup = true;
                                            }
                                            else
                                            {
                                                item1.IsPackageForItem = false;
                                                item1.IsPackageForCategory = false;
                                                item1.IsPackageForGroup = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        CalculatePharmacySummary();
                    }
                    else
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding visit details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();

                    }
                }
                else
                {
                    CalculatePharmacySummary();
                }


            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }


        private void cmbStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbStore.SelectedItem != null)
            {
                StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;

                if (PharmacyItems != null && PharmacyItems.Count > 0)
                {
                    PharmacyItems = new ObservableCollection<clsItemSalesDetailsVO>();
                    dgPharmacyItems.ItemsSource = null;
                    dgPharmacyItems.ItemsSource = PharmacyItems;
                    dgPharmacyItems.UpdateLayout();
                    CalculatePharmacySummary();
                }

                // For Item Selection Control   
                if (IsCSControlEnable == true)    // Use to Enable Item Selection control on Counter Sale Screen
                {
                    if (StoreID > 0)   // && SupplierID > 0 && rdbDirectPur.IsChecked == true
                    {
                        BdrItemSearch.Visibility = Visibility.Visible;
                        AttachItemSearchControl(StoreID, 0);   //AttachItemSearchControl(StoreID, SupplierID);
                    }
                    else
                    {
                        BdrItemSearch.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }



        void mgbxnew_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                ItemListNew ItemSearch = new ItemListNew();
                ItemSearch.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                ItemSearch.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;

                ItemSearch.ShowZeroStockBatches = true;
                ItemSearch.ShowNonZeroStockBatchesSetFromCounterSale = true;

                ItemSearch.ShowNotShowPlusThreeMonthExp = true;
                ItemSearch.ShowPlusThreeMonthExpSetFromCounterSale = true;

                ItemSearch.StoreID = StoreID;
                if (StoreID == 0)
                    ItemSearch.AllowStoreSelection = true;
                else
                    ItemSearch.AllowStoreSelection = false;

                ItemSearch.OnSaveButton_Click += new RoutedEventHandler(ItemSearch_OnSaveButton_Click);
                ItemSearch.Show();
            }
        }

        private void CmdPreBills_Click(object sender, RoutedEventArgs e)
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID > 0)
            {
                frm_PrePharmacyBills objPreBills = new frm_PrePharmacyBills();

                objPreBills.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                objPreBills.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;

                objPreBills.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("", "Please Select Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        /// <summary>
        /// Function is for Fetching Patient Source List.
        /// </summary>
        private void FillPatientSource()
        {
            clsGetPatientSourceListByPatientCategoryIdBizActionVO BizAction = new clsGetPatientSourceListByPatientCategoryIdBizActionVO();

            if (cmbPatientCategory.SelectedItem != null)
                BizAction.SelectedPatientCategoryIdID = ((MasterListItem)cmbPatientCategory.SelectedItem).ID;

            BizAction.PatientSourceDetails = new List<clsPatientSourceVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            List<clsPatientSourceVO> objList = new List<clsPatientSourceVO>();
            clsPatientSourceVO Default = new clsPatientSourceVO { ID = 0, Description = "- Select -" };

            objList.Add(Default);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    objList.AddRange(((clsGetPatientSourceListByPatientCategoryIdBizActionVO)e.Result).PatientSourceDetails);
                    cmbPatientSource.ItemsSource = null;
                    cmbPatientSource.ItemsSource = objList;

                    //if (((MasterListItem)cmbPatientCategory.SelectedItem).ID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CategoryID)
                    //{
                    //    if (searchPatient != true)
                    //    {

                    //        cmbPatientSource.SelectedItem = Default;
                    //    }
                    //}


                    if (searchPatient != true)
                    {
                        if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.CategoryID == ((MasterListItem)cmbPatientCategory.SelectedItem).ID)
                            cmbPatientSource.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientSourceID;
                        else
                            cmbPatientSource.SelectedValue = (long)0;
                    }
                    else
                    {
                        if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID != 0)
                        {
                            cmbPatientSource.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;
                        }
                        else
                        {
                            cmbPatientSource.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientSourceID;

                        }
                    }
                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void cmbPatientCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (cmbPatientCategory.SelectedItem != null && ((MasterListItem)cmbPatientCategory.SelectedItem).ID >= 0)
            //{
            //   FillPatientSource();
            //    //FillPatientSponsorDetails();

            //    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.Corporate_PatientCategoryL1Id == ((MasterListItem)cmbPatientCategory.SelectedItem).ID)
            //    {
            //        cmbCompany.IsEnabled = true;
            //    }
            //    else
            //    {
            //        cmbCompany.IsEnabled = false;
            //    }
            //}

            if (cmbPatientCategory.SelectedItem != null && ((MasterListItem)cmbPatientCategory.SelectedItem).ID > 0)
            {
                FillPatientSource(((MasterListItem)cmbPatientCategory.SelectedItem).ID);
            }
        }

        /// <summary>
        /// Function is for Fetching Company List
        /// </summary>
        private void FillCompany()
        {
            //clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();

            //BizAction.MasterTable = MasterTableNameList.M_CompanyMaster;
            //BizAction.MasterList = new List<MasterListItem>();

            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //Client.ProcessCompleted += (s, e) =>
            //{


            //    if (e.Error == null && e.Result != null)
            //    {
            //        List<MasterListItem> objList = new List<MasterListItem>();

            //        objList.Add(new MasterListItem(0, "- Select -"));
            //        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
            //        cmbCompany.ItemsSource = null;
            //        cmbCompany.ItemsSource = objList;


            //        if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == ((MasterListItem)cmbPatientCategory.SelectedItem).ID)
            //        {
            //            if (((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID != 0)
            //            {
            //                cmbCompany.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
            //            }
            //            else
            //            {
            //                cmbCompany.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfCompanyID;
            //            }
            //        }
            //        else
            //        {
            //            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientCategoryL1Id_Retail == ((MasterListItem)cmbPatientCategory.SelectedItem).ID)
            //            {
            //                cmbCompany.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfCompanyID;
            //            }
            //            else
            //            {
            //                if (objList.Count > 0)
            //                {
            //                    cmbCompany.SelectedItem = objList[1];
            //                }
            //            }
            //        }

            //        if (this.DataContext != null)
            //        {



            //        }
            //    }

            //};

            //Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //Client.CloseAsync();


            clsGetPatientSponsorCompanyListBizActionVO BizAction = new clsGetPatientSponsorCompanyListBizActionVO();

            BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            BizAction.CheckDate = DateTime.Now.Date.Date;
            BizAction.PatientSourceID = PatientSourceID;

            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetPatientSponsorCompanyListBizActionVO)arg.Result).MasterList);
                    cmbCompany.ItemsSource = null;
                    cmbCompany.ItemsSource = objList;

                    if (objList.Count > 1)
                    {


                        if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == ((MasterListItem)cmbPatientCategory.SelectedItem).ID)
                        {
                            if (((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID != 0)
                            {
                                cmbCompany.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
                            }
                            else
                            {
                                cmbCompany.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfCompanyID;
                            }
                        }
                        else
                        {
                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientCategoryL1Id_Retail == ((MasterListItem)cmbPatientCategory.SelectedItem).ID)
                            {
                                cmbCompany.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfCompanyID;
                            }
                            else
                            {
                                cmbCompany.SelectedItem = objList[1];
                            }
                        }
                    }
                    else
                        cmbCompany.SelectedItem = objList[0];



                }




            };
            client.ProcessAsync(BizAction, App.SessionUser);
            client.CloseAsync();
        }

        private void cmbPatientSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //if (cmbPatientSource.SelectedItem != null)
            //{
            //    //PatientSourceID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceID;
            //    PatientSourceID = ((clsPatientSourceVO)cmbPatientSource.SelectedItem).ID;

            //    FillCompany();
            //}
            if (cmbPatientSource.SelectedItem != null && ((MasterListItem)cmbPatientSource.SelectedItem).ID > 0)
            {
                PatientSourceID = ((MasterListItem)cmbPatientSource.SelectedItem).ID;
                FillCompany(((MasterListItem)cmbPatientSource.SelectedItem).ID);
            }
            else
            {
                if (IsPatientSelect == false)   // addede on 07082018 to set values on dropdown selection changed
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));

                    cmbCompany.ItemsSource = objList;
                    cmbCompany.SelectedItem = objList[0];
                    cmbCompany.SelectedValue = (long)0;
                }
            }
        }
        public long PatientSourceID { get; set; }
        private void cmbCompany_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {



            //if (cmbCompany.SelectedItem != null && ((MasterListItem)cmbCompany.SelectedItem).ID > 0)
            //{
            //    //PatientSourceID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceID;
            //    PatientSourceID = ((clsPatientSourceVO)cmbPatientSource.SelectedItem).ID;

            //    FillNewTariff(((MasterListItem)cmbCompany.SelectedItem).ID);

            //}

            if (cmbCompany.SelectedItem != null && ((MasterListItem)cmbCompany.SelectedItem).ID > 0)
            {
                PatientSourceID = ((MasterListItem)cmbPatientSource.SelectedItem).ID;
                FillTariffMaster(0, ((MasterListItem)cmbCompany.SelectedItem).ID);
            }
            else
            {
                if (IsPatientSelect == false)   // addede on 07082018 to set values on dropdown selection changed
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));

                    cmbTariff.ItemsSource = objList;
                    cmbTariff.SelectedItem = objList[0];
                    cmbTariff.SelectedValue = (long)0;
                }
            }
        }
        private void FillNewTariff(long pCompanyID)
        {
            clsGetPatientSponsorTariffListBizActionVO BizAction = new clsGetPatientSponsorTariffListBizActionVO();

            BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            BizAction.CheckDate = DateTime.Now.Date.Date;
            //BizAction.PatientSourceID = PatientSourceID;
            BizAction.PatientCompanyID = pCompanyID;

            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.AddRange(((clsGetPatientSponsorTariffListBizActionVO)arg.Result).MasterList);
                    cmbTariff.ItemsSource = null;

                    cmbTariff.ItemsSource = objList;
                    if (objList.Count > 0)
                    {

                        if (searchPatient != true)
                            cmbTariff.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID;
                        else
                        {
                            if (((IApplicationConfiguration)App.Current).SelectedPatient.TariffID != 0)
                            {
                                cmbTariff.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatient.TariffID;
                            }
                            else
                            {
                                cmbTariff.SelectedValue = objList[0].ID;
                            }
                        }
                    }


                }




            };
            client.ProcessAsync(BizAction, App.SessionUser);
            client.CloseAsync();
        }
        private void FillTariffMaster(long pPatientsourceID, long pCompanyID)
        {
            clsGetTariffMasterVO BizAction = new clsGetTariffMasterVO();

            BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizAction.PatientSourceID = pPatientsourceID;
            BizAction.CompanyID = pCompanyID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                List<MasterListItem> objList = new List<MasterListItem>();

                objList.Add(new MasterListItem(0, "- Select -"));
                if (e.Error == null && e.Result != null)
                {
                    objList.AddRange(((clsGetTariffMasterVO)e.Result).List);
                }

                cmbTariff.ItemsSource = null;
                cmbTariff.ItemsSource = objList;

                if (cmbCompany.SelectedItem != null)
                {
                    if (((MasterListItem)cmbCompany.SelectedItem).ID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CompanyPatientSourceID)
                    {
                        cmbTariff.SelectedItem = objList[0];
                    }
                }

                if (searchPatient != true)
                    cmbTariff.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID;
                else
                {
                    if (IsPatientSelect == true)    // addede on 07082018 to set values on dropdown selection changed
                    {
                        if (((IApplicationConfiguration)App.Current).SelectedPatient.TariffID != 0)
                        {
                            cmbTariff.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatient.TariffID;
                        }
                        else
                        {
                            cmbTariff.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID;
                        }
                    }
                    else
                    {
                        cmbTariff.SelectedValue = (long)0;   // addede on 07082018 to set values on dropdown selection changed
                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }

        private void cmbTariff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            #region MyRegion

            if (cmbTariff.SelectedItem != null && ((MasterListItem)cmbTariff.SelectedItem).ID > 0)
            {
                if (((IApplicationConfiguration)App.Current).SelectedPatient.MRNo != null)
                {
                    if ((cmbPatientSource.SelectedItem != null && ((MasterListItem)cmbPatientSource.SelectedItem).ID > 0) && (cmbCompany.SelectedItem != null && ((MasterListItem)cmbCompany.SelectedItem).ID > 0) && (cmbTariff.SelectedItem != null && ((MasterListItem)cmbTariff.SelectedItem).ID > 0))
                        FillPackage(((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID);
                }
            }
            else
            {
                List<MasterListItem> objList = new List<MasterListItem>();
                objList.Add(new MasterListItem(0, "- Select -"));

                cmbApplicabelPackage.ItemsSource = objList;
                cmbApplicabelPackage.SelectedItem = objList[0];
                cmbApplicabelPackage.SelectedValue = (long)0;
            }
            #endregion
        }

        private void txtPurchaseFrequency_TextChanged(object sender, TextChangedEventArgs e)
        {
            Exception X = new Exception();

            TextBox T = (TextBox)sender;

            try
            {
                if (T.Text != "-")
                {

                    int xx = Convert.ToInt32(T.Text);

                    if (T.Text.Contains(','))
                        throw X;
                    if (T.Text.Contains('.'))
                        throw X;
                }
            }
            catch (Exception)
            {
                try
                {
                    int CursorIndex = T.SelectionStart - 1;
                    T.Text = T.Text.Remove(CursorIndex, 1);

                    T.SelectionStart = CursorIndex;
                    T.SelectionLength = 0;
                }
                catch (Exception) { }
            }
        }

        private void txtPurchaseFrequency_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void GetDeliveryAddress()
        {
            //clsGetCenterDeliveryAddressVO BizAction1 = new clsGetCenterDeliveryAddressVO();
            //BizAction1.CenterWise = true;
            //BizAction1.CenterID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;


            //Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);

            //Client1.ProcessCompleted += (s, arg) =>
            //{
            //    if (arg.Error == null)
            //    {
            //        if (arg.Result != null)
            //        {
            //            clsGetCenterDeliveryAddressVO BizActionCenterAddress = new clsGetCenterDeliveryAddressVO();
            //            BizActionCenterAddress = (clsGetCenterDeliveryAddressVO)arg.Result;
            //            txtDelivery.Text = BizActionCenterAddress.CenterAddress;
            //        }
            //    }
            //    else
            //    {

            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //          new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //        msgW1.Show();
            //    }
            //};

            //Client1.ProcessAsync(BizAction1, ((IApplicationConfiguration)App.Current).CurrentUser);
            //Client1.CloseAsync();
        }

        private void CenterDelivery_Click(object sender, RoutedEventArgs e)
        {
            GetDeliveryAddress();
            txtLandMark.Text = "";
        }

        private void HomeDelivery_Click(object sender, RoutedEventArgs e)
        {
            //if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID > 0)
            //{
            //    clsGetCenterDeliveryAddressVO BizAction1 = new clsGetCenterDeliveryAddressVO();

            //    BizAction1.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            //    BizAction1.CenterID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //    Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //    PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);

            //    Client1.ProcessCompleted += (s, arg) =>
            //    {
            //        if (arg.Error == null)
            //        {
            //            if (arg.Result != null)
            //            {
            //                clsGetCenterDeliveryAddressVO BizActionPatientAddress = new clsGetCenterDeliveryAddressVO();
            //                BizActionPatientAddress = (clsGetCenterDeliveryAddressVO)arg.Result;
            //                if (BizActionPatientAddress.PatientAddress != null)
            //                {
            //                    txtDelivery.Text = BizActionPatientAddress.PatientAddress;
            //                }
            //                else
            //                {
            //                    txtDelivery.Text = System.String.Empty;
            //                }
            //            }
            //        }
            //        else
            //        {

            //            MessageBoxControl.MessageBoxChildWindow msgW1 =
            //              new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //            msgW1.Show();
            //        }
            //    };

            //    Client1.ProcessAsync(BizAction1, ((IApplicationConfiguration)App.Current).CurrentUser);
            //    Client1.CloseAsync();
            //    txtLandMark.Text = PatientLandMark;
            //}
            //else
            //{
            //    txtDelivery.Text = "";
            //}
        }
        public long? PatientCategoryL1Id_Retail = 0;
        public void FillPatientSponsorDetails()
        {
            try
            {
                clsGetPatientSponsorListBizActionVO BizAction = new clsGetPatientSponsorListBizActionVO();

                BizAction.SponsorID = 0;
                BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails != null)
                    {
                        cmbPatientSource.ItemsSource = ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails;
                        cmbPatientSource.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;


                        foreach (var item in ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails)
                        {
                            if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID == item.PatientSourceID)
                            {
                                PatientCategoryL1Id_Retail = item.PatientCategoryID;
                            }
                        }
                        //  FillPackage(((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId);
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                //Indicatior.Close();
                // throw;
            }
        }


        private void txtMobileCountryCode_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((WaterMarkTextbox)sender).Text;
            selectionStart = ((WaterMarkTextbox)sender).SelectionStart;
            selectionLength = ((WaterMarkTextbox)sender).SelectionLength;
        }

        private void txtMobileCountryCode_OnTextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((WaterMarkTextbox)sender).Text))
            {
                if (!((WaterMarkTextbox)sender).Text.IsValidCountryCode() && textBefore != null)
                {
                    ((WaterMarkTextbox)sender).Text = textBefore;
                    ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                    ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
                else if (((WaterMarkTextbox)sender).Text.Length > 4)
                {
                    ((WaterMarkTextbox)sender).Text = textBefore;
                    ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                    ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        DateTime? DOB = null;
        void CalculateBirthDate()
        {


            int Yearval = 0;
            int Monthval = 0;
            int DayVal = 0;

            if (txtYY.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtYY.Text.Trim()))
                Yearval = int.Parse(txtYY.Text.Trim());

            if (txtMM.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtMM.Text.Trim()))
                Monthval = int.Parse(txtMM.Text.Trim());


            if (txtDD.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtDD.Text.Trim()))
                DayVal = int.Parse(txtDD.Text.Trim());

            if (Yearval > 0 || Monthval > 0 || DayVal > 0)
            {
                DOB = CalculateDateFromAge(Yearval, Monthval, DayVal);

            }

        }


        private DateTime? CalculateDateFromAge(int Year, int Month, int Days)
        {
            try
            {
                DateTime BirthDate;

                BirthDate = DateTime.Now;
                if (Year > 0)
                {
                    BirthDate = BirthDate.AddYears(-Year);
                }

                if (Month > 0)
                {
                    BirthDate = BirthDate.AddMonths(-Month);
                }

                if (Days > 0)
                {
                    BirthDate = BirthDate.AddDays(-Days);
                }
                return BirthDate;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return null;
            }
        }

        private void txtRountOffAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Text.Trim() != "" && (!((TextBox)sender).Text.IsPositiveNumber()) && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtRountOffAmount_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;

        }
        bool resultround = true;
        private void txtRountOffAmount_LostFocus(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrEmpty(txtRountOffAmount.Text) && txtRountOffAmount.Text.IsValueDouble())
            {
                if (Convert.ToDouble(txtRountOffAmount.Text) > Comman.MinMaxRoundOff + roundAmt)
                {
                    txtRountOffAmount.SetValidation("You can Round up to (+ or -)" + Comman.MinMaxRoundOff + ", Round Off Amount Exceeds the value");
                    txtRountOffAmount.RaiseValidationError();
                    resultround = false;
                }
                else if (Comman.MinMaxRoundOff > roundAmt)
                {

                    if ((Convert.ToDouble(txtRountOffAmount.Text) < roundAmt - Comman.MinMaxRoundOff) && Convert.ToDouble(txtRountOffAmount.Text) < 0)
                    {
                        txtRountOffAmount.SetValidation("You can Round up to (+ or -)" + Comman.MinMaxRoundOff + ",Round Off Amount can not be less than zero");
                        txtRountOffAmount.RaiseValidationError();
                        resultround = false;
                    }
                    else if (Convert.ToDouble(txtRountOffAmount.Text) < roundAmt - Comman.MinMaxRoundOff)
                    {
                        txtRountOffAmount.SetValidation("You can Round up to (+ or -)" + Comman.MinMaxRoundOff + ",Round Off Amount is less then the value");
                        txtRountOffAmount.RaiseValidationError();
                        resultround = false;
                    }
                    else
                    {
                        txtRountOffAmount.ClearValidationError();
                        resultround = true;
                    }

                }
                else if (Comman.MinMaxRoundOff < roundAmt)
                {
                    if (Convert.ToDouble(txtRountOffAmount.Text) < roundAmt - Comman.MinMaxRoundOff)
                    {
                        txtRountOffAmount.SetValidation("You can Round up to (+ or -)" + Comman.MinMaxRoundOff + ",Round Off Amount is less then the value");
                        txtRountOffAmount.RaiseValidationError();
                        resultround = false;
                    }
                    else
                    {
                        txtRountOffAmount.ClearValidationError();
                        resultround = true;
                    }
                }
                else
                {
                    txtRountOffAmount.ClearValidationError();
                    resultround = true;
                }
            }
            else
            {
                txtRountOffAmount.Text = roundAmt.ToString();
                resultround = true;

            }
        }

        List<MasterListItem> objPackageList = new List<MasterListItem>();
        //By Anjali.......................
        private void FillPackage(long PatientID1, long UnitId1)
        {
            try
            {

                clsGetPatientPackageInfoListBizActionVO BizAction = new clsGetPatientPackageInfoListBizActionVO();
                BizAction.IsfromCounterSale = true;
                //BizAction.IsfromCounterSale = false; // commented by Ashish Z. on dated 16

                if (LinkPatientID > 0 && LinkPatientUnitID > 0)
                {
                    BizAction.PatientID1 = LinkPatientID;
                    BizAction.PatientUnitID1 = LinkPatientUnitID;
                    BizAction.PatientSourceID = LinkPatientSourceID;
                    if (LinkCompanyID != null && LinkCompanyID > 0)
                    {
                        BizAction.PatientCompanyID = LinkCompanyID;
                    }
                    if (LinkTariffID != null && LinkTariffID > 0)
                    {
                        BizAction.PatientTariffID = LinkTariffID;
                    }
                }
                else
                {
                    BizAction.PatientID1 = PatientID1;
                    BizAction.PatientUnitID1 = UnitId1;
                    BizAction.PatientSourceID = PatientSourceID;
                    if (cmbCompany.SelectedItem != null && ((MasterListItem)cmbCompany.SelectedItem).ID > 0)
                    {
                        BizAction.PatientCompanyID = ((MasterListItem)cmbCompany.SelectedItem).ID;
                    }
                    if (cmbTariff.SelectedItem != null && ((MasterListItem)cmbTariff.SelectedItem).ID > 0)
                    {
                        BizAction.PatientTariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;
                    }
                }
                BizAction.CheckDate = DateTime.Now.Date.Date;

                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        objPackageList = new List<MasterListItem>();
                        objPackageList.Add(new MasterListItem(0, "-- Select --"));
                        objPackageList.AddRange(((clsGetPatientPackageInfoListBizActionVO)arg.Result).MasterList);

                        cmbApplicabelPackage.ItemsSource = null;
                        cmbApplicabelPackage.ItemsSource = objPackageList;

                        if (objPackageList.Count > 1)
                        {
                            //CmdApplyRules.Visibility = Visibility.Visible;    // For Package New Changes Commented on 27042018
                            cmbApplicabelPackage.IsEnabled = true;
                            ApplicablePack.Visibility = Visibility.Visible;
                            cmbApplicabelPackage.Visibility = Visibility.Visible;

                            //cmdAddPackageItems.Visibility = Visibility.Visible;  //Added By Ashish Z. for Get Package Items on dated 100616
                            //cmdAddPackageItems.IsEnabled = true; //Added By Ashish Z. for Get Package Items on dated 100616

                            //var list1 = from ls in objList
                            //            orderby ls.ID descending
                            //            select ls.ID;

                            var list1 = from ls in objPackageList
                                        orderby ls.FilterID descending
                                        select ls.ID;

                            cmbApplicabelPackage.SelectedValue = list1.ToList()[0];

                            #region For Package New Changes added on 05062018

                            if (searchPatient == true && cmbApplicabelPackage.Visibility == Visibility.Visible)
                            {
                                string msgText = "Package Available For Billing";
                                MessageBoxControl.MessageBoxChildWindow msgP =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgP.Show();
                            }

                            #endregion
                        }
                        else
                        {
                            cmbApplicabelPackage.ItemsSource = null;
                            cmbApplicabelPackage.SelectedItem = objPackageList[0];
                        }
                    }

                    IsPatientSelect = false;    // addede on 07082018 to set values on dropdown selection changed
                };
                client.ProcessAsync(BizAction, App.SessionUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                IsPatientSelect = false;    // addede on 07082018 to set values on dropdown selection changed
            }
            finally
            {
                IsPatientSelect = false;    // addede on 07082018 to set values on dropdown selection changed
            }

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
                clsGetPatientDetailsForCounterSaleBizActionVO BizAction = new clsGetPatientDetailsForCounterSaleBizActionVO();
                BizAction.PatientDetails = new clsPatientVO();
                BizAction.MRNO = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                VisitId = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            searchPatient = true; // Added By Bhushanp 23052017 For Select Wrong Sponser and Source
                            clsGetPatientDetailsForCounterSaleBizActionVO ObjPatient = new clsGetPatientDetailsForCounterSaleBizActionVO();
                            ObjPatient = (clsGetPatientDetailsForCounterSaleBizActionVO)arg.Result;
                            PatientId = ObjPatient.PatientDetails.GeneralDetails.PatientID;

                            txtMRNo.Text = ObjPatient.PatientDetails.GeneralDetails.MRNo;
                            txtFirstName.Text = ObjPatient.PatientDetails.GeneralDetails.FirstName;
                            if (ObjPatient.PatientDetails.GeneralDetails.MiddleName != null)
                            {
                                txtMiddleName.Text = ObjPatient.PatientDetails.GeneralDetails.MiddleName;
                            }
                            txtLastName.Text = ObjPatient.PatientDetails.GeneralDetails.LastName;

                            if (ObjPatient.PatientDetails.GeneralDetails.IsAge == false)
                            {
                                dtpDOB.SelectedDate = ObjPatient.PatientDetails.GeneralDetails.DateOfBirth;
                                txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                                txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                                txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");
                            }
                            else
                            {
                                DOB = ObjPatient.PatientDetails.GeneralDetails.DateOfBirthFromAge;
                                txtYY.Text = ConvertDate(ObjPatient.PatientDetails.GeneralDetails.DateOfBirthFromAge, "YY");
                                txtMM.Text = ConvertDate(ObjPatient.PatientDetails.GeneralDetails.DateOfBirthFromAge, "MM");
                                txtDD.Text = ConvertDate(ObjPatient.PatientDetails.GeneralDetails.DateOfBirthFromAge, "DD");
                            }


                            cmbGender.SelectedValue = ObjPatient.PatientDetails.GenderID;
                            txtMobileCountryCode.Text = ObjPatient.PatientDetails.MobileCountryCode.ToString();
                            txtContactNo.Text = ObjPatient.PatientDetails.ContactNo1.ToString();
                            txtDelivery.Text = ObjPatient.PatientDetails.AddressLine1;

                            txtFirstName.IsEnabled = false;
                            txtMiddleName.IsEnabled = false;
                            txtLastName.IsEnabled = false;
                            cmbGender.IsEnabled = false;
                            dtpDOB.IsEnabled = false;
                            txtMobileCountryCode.IsEnabled = false;
                            txtContactNo.IsEnabled = false;
                            txtYY.IsEnabled = false;
                            txtMM.IsEnabled = false;
                            txtDD.IsEnabled = false;
                            txtDelivery.IsEnabled = false;

                            IsPatientSelect = true;     // addede on 07082018 to set values on dropdown selection changed

                            CheckVisit();
                            FillPatientType();
                            FillPatientSponsorDetailsNew();     // addede on 08082018 to check selected sponsor is valid or not
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured While Adding Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                    Indicatior.Close();
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }



            //try
            //{
            //    clsGetPatientPackageInfoListBizActionVO BizAction = new clsGetPatientPackageInfoListBizActionVO();
            //    BizAction.IsfromCounterSale = true;
            //    BizAction.PatientID1 = LinkPatientID;
            //    BizAction.PatientUnitID1 = LinkPatientUnitID;
            //    BizAction.PatientSourceID = LinkPatientSourceID;
            //    if (LinkCompanyID != null && LinkCompanyID > 0)
            //    {
            //        BizAction.PatientCompanyID = LinkCompanyID;
            //    }
            //    if (LinkTariffID != null && LinkTariffID > 0)
            //    {
            //        BizAction.PatientTariffID = LinkTariffID;
            //    }

            //    BizAction.CheckDate = DateTime.Now.Date.Date;

            //    BizAction.MasterList = new List<MasterListItem>();
            //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            //    client.ProcessCompleted += (s, arg) =>
            //    {
            //        if (arg.Error == null && arg.Result != null)
            //        {
            //            objPackageList = new List<MasterListItem>();
            //            objPackageList.Add(new MasterListItem(0, "-- Select --"));
            //            objPackageList.AddRange(((clsGetPatientPackageInfoListBizActionVO)arg.Result).MasterList);

            //            cmbApplicabelPackage.ItemsSource = null;
            //            cmbApplicabelPackage.ItemsSource = objPackageList;

            //            if (objPackageList.Count > 1)
            //            {
            //                cmbApplicabelPackage.IsEnabled = true;
            //                ApplicablePack.Visibility = Visibility.Visible;
            //                cmbApplicabelPackage.Visibility = Visibility.Visible;

            //                var list1 = from ls in objPackageList
            //                            orderby ls.FilterID descending
            //                            select ls.ID;

            //                cmbApplicabelPackage.SelectedValue = list1.ToList()[0];

            //                #region For Package New Changes added on 05062018

            //                if (searchPatient == true && cmbApplicabelPackage.Visibility == Visibility.Visible)
            //                {
            //                    string msgText = "Package Available For Billing";
            //                    MessageBoxControl.MessageBoxChildWindow msgP =
            //                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //                    msgP.Show();
            //                }

            //                #endregion
            //            }
            //            else
            //            {
            //                cmbApplicabelPackage.ItemsSource = null;
            //                cmbApplicabelPackage.SelectedItem = objPackageList[0];
            //            }
            //        }

            //        IsPatientSelect = false;    // addede on 07082018 to set values on dropdown selection changed
            //    };
            //    client.ProcessAsync(BizAction, App.SessionUser);
            //    client.CloseAsync();
            //}
            //catch (Exception ex)
            //{
            //    IsPatientSelect = false;    // addede on 07082018 to set values on dropdown selection changed
            //}
            //finally
            //{
            //    IsPatientSelect = false;    // addede on 07082018 to set values on dropdown selection changed
            //}
        }

        //else
        //{
        //    try
        //    {
        //        clsGetPatientPackageInfoListBizActionVO BizAction = new clsGetPatientPackageInfoListBizActionVO();
        //        BizAction.IsfromCounterSale = true;
        //        BizAction.PatientID1 = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
        //        BizAction.PatientUnitID1 = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
        //        BizAction.PatientSourceID = PatientSourceID;
        //        if (cmbCompany.SelectedItem != null && ((MasterListItem)cmbCompany.SelectedItem).ID > 0)
        //        {
        //            BizAction.PatientCompanyID = ((MasterListItem)cmbCompany.SelectedItem).ID;
        //        }
        //        if (cmbTariff.SelectedItem != null && ((MasterListItem)cmbTariff.SelectedItem).ID > 0)
        //        {
        //            BizAction.PatientTariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;
        //        }                   
        //        BizAction.CheckDate = DateTime.Now.Date.Date;

        //        BizAction.MasterList = new List<MasterListItem>();
        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //        client.ProcessCompleted += (s, arg) =>
        //        {
        //            if (arg.Error == null && arg.Result != null)
        //            {
        //                objPackageList = new List<MasterListItem>();
        //                objPackageList.Add(new MasterListItem(0, "-- Select --"));
        //                objPackageList.AddRange(((clsGetPatientPackageInfoListBizActionVO)arg.Result).MasterList);

        //                cmbApplicabelPackage.ItemsSource = null;
        //                cmbApplicabelPackage.ItemsSource = objPackageList;

        //                if (objPackageList.Count > 1)
        //                {
        //                    cmbApplicabelPackage.IsEnabled = true;
        //                    ApplicablePack.Visibility = Visibility.Visible;
        //                    cmbApplicabelPackage.Visibility = Visibility.Visible;

        //                    var list1 = from ls in objPackageList
        //                                orderby ls.FilterID descending
        //                                select ls.ID;

        //                    cmbApplicabelPackage.SelectedValue = list1.ToList()[0];

        //                    #region For Package New Changes added on 05062018

        //                    if (searchPatient == true && cmbApplicabelPackage.Visibility == Visibility.Visible)
        //                    {
        //                        string msgText = "Package Available For Billing";
        //                        MessageBoxControl.MessageBoxChildWindow msgP =
        //                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //                        msgP.Show();
        //                    }

        //                    #endregion
        //                }
        //                else
        //                {
        //                    cmbApplicabelPackage.ItemsSource = null;
        //                    cmbApplicabelPackage.SelectedItem = objPackageList[0];
        //                }
        //            }

        //            IsPatientSelect = false;    // addede on 07082018 to set values on dropdown selection changed
        //        };
        //        client.ProcessAsync(BizAction, App.SessionUser);
        //        client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        IsPatientSelect = false;    // addede on 07082018 to set values on dropdown selection changed
        //    }
        //    finally
        //    {
        //        IsPatientSelect = false;    // addede on 07082018 to set values on dropdown selection changed
        //    }
        //}

        //}





        void DonorLinWin_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            //if (SelectedBill != null)
            //    //cmdSave.IsEnabled = false;
            //else
            //cmdSave.IsEnabled = true;
        }



        private void cmbApplicabelPackage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // For Package New Changes Commented on 27042018
            //if (cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)
            //{
            //    CmdApplyRules.Visibility = Visibility.Visible;
            //    cmdAddPackageItems.IsEnabled = true; //Added By Ashish Z. for Get Package Items on dated 100616
            //}
            //else
            //{
            //    CmdApplyRules.Visibility = Visibility.Collapsed;
            //    cmdAddPackageItems.IsEnabled = false; //Added By Ashish Z. for Get Package Items on dated 100616
            //}

            if (cmbApplicabelPackage.SelectedItem != null)      // For Package New Changes Added on 27042018
            {
                if (PharmacyItems != null && PharmacyItems.Count > 0)
                {
                    PharmacyItems = new ObservableCollection<clsItemSalesDetailsVO>();
                    dgPharmacyItems.ItemsSource = null;
                    dgPharmacyItems.ItemsSource = PharmacyItems;
                    dgPharmacyItems.UpdateLayout();
                    CalculatePharmacySummary();
                }
            }
        }

        private void dgPharmacyItems_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (((clsItemSalesDetailsVO)e.Row.DataContext).PackageID > 0)
            {
                //e.Row.IsEnabled = false;

                //DataGridColumn column = dgPharmacyItems.Columns[10];
                //FrameworkElement fe = column.GetCellContent(e.Row);
                //FrameworkElement result = GetParent(fe, typeof(DataGridCell));

                //if (result != null)
                //{
                //    DataGridCell cell = (DataGridCell)result;
                //    cell.IsEnabled = false;
                //}

                //DataGridColumn column1 = dgPharmacyItems.Columns[11];
                //FrameworkElement fe1 = column1.GetCellContent(e.Row);
                //FrameworkElement result1 = GetParent(fe1, typeof(DataGridCell));

                //if (result1 != null)
                //{
                //    DataGridCell cell = (DataGridCell)result1;
                //    cell.IsEnabled = false;
                //}

            }

            if (((clsItemSalesDetailsVO)e.Row.DataContext).PackageBillID > 0)       // Package New Changes Added on 27042018
            {
                //For Concession %
                DataGridColumn column = dgPharmacyItems.Columns[13];
                FrameworkElement fe = column.GetCellContent(e.Row);
                FrameworkElement result = GetParent(fe, typeof(DataGridCell));

                if (result != null)
                {
                    DataGridCell cell = (DataGridCell)result;
                    cell.IsEnabled = false;
                }

                //For Concession Amount
                DataGridColumn column2 = dgPharmacyItems.Columns[14];
                FrameworkElement fe2 = column2.GetCellContent(e.Row);
                FrameworkElement result2 = GetParent(fe2, typeof(DataGridCell));

                if (result2 != null)
                {
                    DataGridCell cell2 = (DataGridCell)result2;
                    cell2.IsEnabled = false;
                }

                //For Quantity
                DataGridColumn column3 = dgPharmacyItems.Columns[6];
                FrameworkElement fe3 = column3.GetCellContent(e.Row);
                FrameworkElement result3 = GetParent(fe3, typeof(DataGridCell));

                if (result3 != null)
                {
                    DataGridCell cell3 = (DataGridCell)result3;
                    cell3.IsEnabled = true;
                }

                //For UOM
                DataGridColumn column4 = dgPharmacyItems.Columns[7];
                FrameworkElement fe4 = column4.GetCellContent(e.Row);
                FrameworkElement result4 = GetParent(fe4, typeof(DataGridCell));

                if (result4 != null)
                {
                    DataGridCell cell4 = (DataGridCell)result4;
                    cell4.IsEnabled = true;
                }
            }

            //if (((clsItemSalesDetailsVO)e.Row.DataContext).PrescriptionDetailsID > 0)
            //{
            //    DataGridColumn column = dgPharmacyItems.Columns[7];

            //    FrameworkElement fe = column.GetCellContent(e.Row);
            //    FrameworkElement result = GetParent(fe, typeof(DataGridCell));

            //    if (result != null)
            //    {
            //        DataGridCell cell = (DataGridCell)result;
            //         cell.IsEnabled = false;
            //    }
            //}
            //dgPharmacyItems.SelectedItem = ((clsItemSalesDetailsVO)e.Row.DataContext);

            //DataGridColumn column = dgPharmacyItems.Columns[7];
            //FrameworkElement fe = column.GetCellContent(e.Row);
            // FrameworkElement result = GetParent(fe, typeof(DataGridCell));

            //if (fe != null)
            //{
            //    //DataGridCell cell = (DataGridCell)result;

            //    FillUOMConversions((AutoCompleteBox)fe);
            //}

            #region Commented by Ashish Z on dated 140616
            //Added by Ashish Z. on dated 140616
            //if (((clsItemSalesDetailsVO)e.Row.DataContext).IsPackageForItem == true)
            //{
            //    //For Concession %
            //    DataGridColumn column = dgPharmacyItems.Columns[13];
            //    FrameworkElement fe = column.GetCellContent(e.Row);
            //    FrameworkElement result = GetParent(fe, typeof(DataGridCell));

            //    if (result != null)
            //    {
            //        DataGridCell cell = (DataGridCell)result;
            //        cell.IsEnabled = false;
            //    }

            //    //For Concession Amount
            //    DataGridColumn column1 = dgPharmacyItems.Columns[14];
            //    FrameworkElement fe1 = column1.GetCellContent(e.Row);
            //    FrameworkElement result1 = GetParent(fe1, typeof(DataGridCell));

            //    if (result1 != null)
            //    {
            //        DataGridCell cell = (DataGridCell)result1;
            //        cell.IsEnabled = false;
            //    }

            //    //For Concession Amount
            //    DataGridColumn column2 = dgPharmacyItems.Columns[29];
            //    FrameworkElement fe2 = column2.GetCellContent(e.Row);
            //    FrameworkElement result2 = GetParent(fe1, typeof(DataGridCell));

            //    if (result2 != null)
            //    {
            //        DataGridCell cell = (DataGridCell)result2;
            //        cell.Visibility = Visibility.Visible;
            //    }

            //    //For Budget
            //    DataGridColumn column3 = dgPharmacyItems.Columns[30];
            //    FrameworkElement fe3 = column3.GetCellContent(e.Row);
            //    FrameworkElement result3 = GetParent(fe3, typeof(DataGridCell));

            //    if (result3 != null)
            //    {
            //        DataGridCell cell = (DataGridCell)result3;
            //        cell.Visibility = Visibility.Visible;
            //    }
            //}
            //else
            //{

            //}
            #endregion

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
        private void ApplyRules()
        {
            GetPackageItemDiscountNew();
        }
        ObservableCollection<clsItemSalesDetailsVO> ItemList = new ObservableCollection<clsItemSalesDetailsVO>();
        private void GetPackageItemDiscountNew()
        {

            clsApplyPackageDiscountRateToItems BizAction = new clsApplyPackageDiscountRateToItems();
            BizAction.objApplyItemPackageDiscountRateDetails = new PalashDynamics.ValueObjects.Billing.clsApplyPackageDiscountRateOnItemVO();
            BizAction.objApplyItemPackageDiscountRateDetails.PatientCatagoryL1 = ((MasterListItem)cmbPatientCategory.SelectedItem).ID;
            // BizAction.objApplyItemPackageDiscountRateDetails.PatientCatagoryL2 = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceID;
            BizAction.objApplyItemPackageDiscountRateDetails.PatientCatagoryL2 = ((MasterListItem)cmbPatientSource.SelectedItem).ID;
            if (cmbTariff.SelectedItem != null)
                BizAction.objApplyItemPackageDiscountRateDetails.PatientCatagoryL3 = ((MasterListItem)cmbTariff.SelectedItem).ID;
            if (cmbCompany.SelectedItem != null)
                BizAction.objApplyItemPackageDiscountRateDetails.CompanyID = ((MasterListItem)cmbCompany.SelectedItem).ID;

            string ItemIDs = "";
            if (PharmacyItems != null && PharmacyItems.Count > 0)
            {
                foreach (var item in PharmacyItems)
                {
                    ItemIDs = ItemIDs + item.ItemID;
                    ItemIDs = ItemIDs + ",";
                }

                if (ItemIDs.EndsWith(","))
                    ItemIDs = ItemIDs.Remove(ItemIDs.Length - 1, 1);
            }

            BizAction.objApplyItemPackageDiscountRateDetails.ItemIDs = ItemIDs;


            //By Anjali.................................
            BizAction.objApplyItemPackageDiscountRateDetails.PatientGenderID = ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID;
            BizAction.objApplyItemPackageDiscountRateDetails.PatientDateOfBirth = Convert.ToDateTime(((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth);
            BizAction.objApplyItemPackageDiscountRateDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.objApplyItemPackageDiscountRateDetails.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
            if (cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)
            {
                BizAction.objApplyItemPackageDiscountRateDetails.PackageID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID;
                BizAction.objApplyItemPackageDiscountRateDetails.PackageBillID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageBillID;
                BizAction.objApplyItemPackageDiscountRateDetails.PackageBillUnitID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageBillUnitID;
            }
            //..............................................


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                ClickedFlag = 0;
                if (arg.Error == null && arg.Result != null && ((clsApplyPackageDiscountRateToItems)arg.Result).objApplyItemPackageDiscountRate != null)
                {

                    if (((clsApplyPackageDiscountRateToItems)arg.Result).objApplyItemPackageDiscountRate != null)
                    {
                        clsApplyPackageDiscountRateToItems ItemDiscountList = ((clsApplyPackageDiscountRateToItems)arg.Result);
                        ItemList = new ObservableCollection<clsItemSalesDetailsVO>();
                        foreach (var item in PharmacyItems)
                        {
                            // item.ActualNetAmt = item.NetAmount;
                            ItemList.Add(item.DeepCopy());
                        }
                        if (ItemDiscountList.objApplyItemPackageDiscountRate.Count > 0)
                        {
                            TotalCalculatedBudget = ItemDiscountList.objApplyItemPackageDiscountRate[0].CalculatedTotalBudget;
                            TotalBudget = ItemDiscountList.objApplyItemPackageDiscountRate[0].TotalBudget;
                            CalculatedBudget = ItemDiscountList.objApplyItemPackageDiscountRate[0].CalculatedBudget;

                            pharmacyBudget = ItemDiscountList.objApplyItemPackageDiscountRate[0].Budget;    // Package Change 17042017
                        }

                        #region old Commented
                        //foreach (var item in ItemDiscountList.objApplyItemPackageDiscountRate)
                        //{
                        //    foreach (var item1 in PharmacyItems)
                        //    {
                        //        if (item.ApplicableToAllDiscount > 0)
                        //        {
                        //            item1.ConcessionPercentage = item.DiscountedPercentage;
                        //            ConcessionFromPlan = true;
                        //        }
                        //        else
                        //        {
                        //            if (item.IsCategory == false && item.IsGroup == false)
                        //            {
                        //                if (item1.ItemID == item.ItemId)
                        //                {
                        //                    item1.ConcessionPercentage = item.DiscountedPercentage;
                        //                    ConcessionFromPlan = true;

                        //                    //By Anjali..........................
                        //                    item1.Budget = item.Budget;
                        //                    item1.CalculatedBudget = item.CalculatedBudget;
                        //                    item1.IsPackageForItem = true;
                        //                    item1.IsPackageForCategory = false;
                        //                    item1.IsPackageForGroup = false;
                        //                    //......................................
                        //                }
                        //                else
                        //                {
                        //                    item1.IsPackageForItem = false;
                        //                    item1.IsPackageForCategory = false;
                        //                    item1.IsPackageForGroup = false;
                        //                }

                        //            }
                        //            else if (item.IsCategory == true && item.IsGroup == false)
                        //            {
                        //                if (item1.CategoryId == item.CategoryId)
                        //                {
                        //                    item1.ConcessionPercentage = item.DiscountedPercentage;
                        //                    ConcessionFromPlan = true;
                        //                    //By Anjali..........................
                        //                    item1.Budget = item.Budget;
                        //                    item1.CalculatedBudget = item.CalculatedBudget;

                        //                    item1.IsPackageForItem = false;
                        //                    item1.IsPackageForCategory = true;
                        //                    item1.IsPackageForGroup = false;
                        //                    //......................................
                        //                }
                        //                else
                        //                {
                        //                    item1.IsPackageForItem = false;
                        //                    item1.IsPackageForCategory = false;
                        //                    item1.IsPackageForGroup = false;
                        //                }
                        //            }
                        //            else if (item.IsGroup == true && item.IsCategory == false)
                        //            {
                        //                if (item1.GroupId == item.GroupId)
                        //                {
                        //                    item1.ConcessionPercentage = item.DiscountedPercentage;
                        //                    ConcessionFromPlan = true;
                        //                    //By Anjali..........................
                        //                    item1.Budget = item.Budget;
                        //                    item1.CalculatedBudget = item.CalculatedBudget;

                        //                    item1.IsPackageForItem = false;
                        //                    item1.IsPackageForCategory = false;
                        //                    item1.IsPackageForGroup = true;
                        //                    //......................................
                        //                }
                        //                else
                        //                {
                        //                    item1.IsPackageForItem = false;
                        //                    item1.IsPackageForCategory = false;
                        //                    item1.IsPackageForGroup = false;
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                        #endregion

                        double PreviousConcession = 0;
                        double ConsumeBudget = 0;

                        //if (TotalCalculatedBudget < TotalBudget)
                        //{

                        ConsumeBudget = TotalCalculatedBudget;                      // Package Change 19042017
                        pendingBudget = pharmacyBudget - TotalCalculatedBudget;     // Package Change 17042017

                        foreach (var item in ItemDiscountList.objApplyItemPackageDiscountRate)
                        {
                            foreach (var item1 in ItemList)
                            {

                                if (item.ApplicableToAll == true)   // Package Change 17042017  //if (item.ApplicableToAllDiscount > 0)
                                {
                                    if (TotalCalculatedBudget < pharmacyBudget)       // Package Change 17042017  //if (TotalCalculatedBudget < TotalBudget)
                                    {
                                        item1.PackageID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID;

                                        item1.PackageBillID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageBillID;                // Package Change 18042017
                                        item1.PackageBillUnitID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageBillUnitID;        // Package Change 18042017

                                        item1.Budget = item.Budget;
                                        item1.CalculatedBudget = item.CalculatedBudget;

                                        item1.VATPercent = 0;
                                        item1.VATAmount = 0;

                                        item1.ConcessionPercentage = 0;
                                        item1.ConcessionAmount = 0;
                                        item1.ConcessionPercentage = item.DiscountedPercentage;
                                        ConcessionFromPlan = true;

                                        TotalCalculatedBudget = TotalCalculatedBudget + (Convert.ToSingle(item1.Quantity) * item1.MainRate);      // Convert.ToSingle(item1.NetAmount);

                                        if (TotalCalculatedBudget < pharmacyBudget)
                                        {
                                            if (pendingBudget < item1.NetAmount)     // Package Change 17042017      if (item1.Budget < item1.NetAmount) 
                                            {
                                                //item1.NetAmtCalculation = pendingBudget;   // item1.Budget;    // Package Change 17042017  
                                            }
                                            else   // Package Change 17042017  
                                            {
                                                item1.NetAmtCalculation = (Convert.ToSingle(item1.Quantity) * item1.MainRate);             //item1.NetAmount;                                 // Package Change 17042017  
                                                pendingBudget = pendingBudget - (Convert.ToSingle(item1.Quantity) * item1.MainRate);      //Convert.ToSingle(item1.NetAmount);        // Package Change 17042017  
                                            }
                                        }
                                        else
                                        {
                                            if (pendingBudget > 0)                          // Package Change 17042017 
                                            {
                                                //item1.VATPercent = 0;
                                                //item1.VATAmount = 0;

                                                item1.ConcessionPercentage = 0;
                                                item1.ConcessionAmount = 0;
                                                item1.ConcessionAmount = pendingBudget;
                                                item1.NetAmtCalculation = (Convert.ToSingle(item1.Quantity) * item1.MainRate);      //item1.NetAmount;
                                                pendingBudget = 0;
                                            }
                                            else
                                            {
                                                item1.ConcessionAmount = 0;
                                                item1.ConcessionPercentage = PreviousConcession;
                                                item1.NetAmtCalculation = 0;
                                            }
                                        }

                                    }
                                    else     // Package Change 17042017  
                                    {
                                        item1.ConcessionAmount = 0;
                                        item1.ConcessionPercentage = PreviousConcession;
                                        item1.NetAmtCalculation = 0;
                                    }

                                    item1.IsPackageForItem = true;
                                    item1.IsPackageForCategory = false;
                                    item1.IsPackageForGroup = false;
                                }
                                else
                                {
                                    PreviousConcession = 0;

                                    if (item.IsCategory == false && item.IsGroup == false)
                                    {
                                        if (item1.ItemID == item.ItemId)
                                        {
                                            if (TotalCalculatedBudget < TotalBudget)
                                            {
                                                item1.PackageID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID;
                                                item1.Budget = item.Budget;
                                                item1.CalculatedBudget = item.CalculatedBudget;

                                                PreviousConcession = item1.ConcessionPercentage;

                                                item1.ConcessionAmount = 0;
                                                item1.ConcessionPercentage = 0;
                                                item1.ConcessionPercentage = item.DiscountedPercentage;
                                                ConcessionFromPlan = true;

                                                //TotalCalculatedBudget = TotalCalculatedBudget + Convert.ToSingle(item1.NetAmount);
                                                TotalCalculatedBudget = TotalCalculatedBudget + Convert.ToSingle(item1.ActualNetAmt);

                                                if (TotalCalculatedBudget < TotalBudget)
                                                {
                                                    if (item1.Budget < item1.NetAmount)
                                                    {
                                                        item1.NetAmtCalculation = item1.Budget;
                                                    }
                                                    else
                                                    {
                                                        item1.NetAmtCalculation = item1.NetAmount;
                                                    }
                                                }
                                                else
                                                {
                                                    item1.ConcessionAmount = 0;
                                                    item1.ConcessionPercentage = PreviousConcession;
                                                    item1.NetAmtCalculation = 0;
                                                }

                                                item1.IsPackageForItem = true;
                                                item1.IsPackageForCategory = false;
                                                item1.IsPackageForGroup = false;
                                            }
                                            else
                                            {
                                                //item1.Budget = item.Budget;
                                            }
                                        }
                                        else
                                        {
                                            item1.IsPackageForItem = false;
                                            item1.IsPackageForCategory = false;
                                            item1.IsPackageForGroup = false;
                                        }

                                    }
                                    #region Old Commented
                                    //else if (item.IsCategory == true && item.IsGroup == false)
                                    //{
                                    //    if (item1.CategoryId == item.CategoryId)
                                    //    {
                                    //        item1.ConcessionPercentage = item.DiscountedPercentage;
                                    //        item1.PackageID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID;
                                    //        item1.Budget = item.Budget;
                                    //        item1.CalculatedBudget = item.CalculatedBudget;

                                    //        ConcessionFromPlan = true;
                                    //        if (item1.Budget < (CalculatedBudget + item1.NetAmount))
                                    //        {
                                    //            item1.ConcessionPercentage = 0;
                                    //        }
                                    //        else
                                    //        {
                                    //            TotalCalculatedBudget = TotalCalculatedBudget - Convert.ToSingle(item1.NetAmount);
                                    //            CalculatedBudget = CalculatedBudget + Convert.ToSingle(item1.NetAmount);
                                    //        }
                                    //        item1.IsPackageForItem = false;
                                    //        item1.IsPackageForCategory = true;
                                    //        item1.IsPackageForGroup = false;
                                    //    }
                                    //    else
                                    //    {
                                    //        item1.IsPackageForItem = false;
                                    //        item1.IsPackageForCategory = false;
                                    //        item1.IsPackageForGroup = false;
                                    //    }
                                    //}
                                    //else if (item.IsGroup == true && item.IsCategory == false)
                                    //{
                                    //    if (item1.GroupId == item.GroupId)
                                    //    {
                                    //        item1.ConcessionPercentage = item.DiscountedPercentage;
                                    //        item1.PackageID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID;
                                    //        item1.Budget = item.Budget;
                                    //        item1.CalculatedBudget = item.CalculatedBudget;
                                    //        ConcessionFromPlan = true;
                                    //        if (item1.Budget < (CalculatedBudget + item1.NetAmount))
                                    //        {
                                    //            item1.ConcessionPercentage = 0;
                                    //        }
                                    //        else
                                    //        {
                                    //            TotalCalculatedBudget = TotalCalculatedBudget - Convert.ToSingle(item1.NetAmount);
                                    //            CalculatedBudget = CalculatedBudget + Convert.ToSingle(item1.NetAmount);
                                    //        }
                                    //        item1.IsPackageForItem = false;
                                    //        item1.IsPackageForCategory = false;
                                    //        item1.IsPackageForGroup = true;
                                    //    }
                                    //    else
                                    //    {
                                    //        item1.IsPackageForItem = false;
                                    //        item1.IsPackageForCategory = false;
                                    //        item1.IsPackageForGroup = false;
                                    //    }
                                    //}
                                    #endregion

                                }
                                //}
                            }
                        }
                        //}       // Package Change 17042017

                        ItemComparisonWindowForCounterSale win = new ItemComparisonWindowForCounterSale();
                        win.dataGrid2.ItemsSource = PharmacyItems;
                        win.dgSelectedItemList.ItemsSource = ItemList;
                        win.txtBudget.Text = String.Format("{0:0.00}", pharmacyBudget);        // Package Change 18042017    // win.txtBudget.Text = String.Format("{0:0.00}", TotalBudget);
                        win.txtConsumeBudget.Text = String.Format("{0:0.00}", ConsumeBudget);        // Package Change 18042017
                        win.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
                        win.OnCancelButton_Click += new RoutedEventHandler(Win_OnCancelButton_Click);
                        win.Show();
                    }
                    else
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding visit details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();

                    }
                }



            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        bool IsFromPackage = false;
        void Win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            cmbApplicabelPackage.IsEnabled = false;
            CmdApplyRules.Visibility = Visibility.Collapsed;
            IsFromPackage = ((ItemComparisonWindowForCounterSale)sender).IsFromPackage;
            PharmacyItems = new ObservableCollection<clsItemSalesDetailsVO>();
            PharmacyItems = ItemList.DeepCopy(); ;
            dgPharmacyItems.ItemsSource = null;
            dgPharmacyItems.ItemsSource = PharmacyItems;
            CalculatePharmacySummaryNew();
            ClickedFlagForApplyRules = 0;

        }

        private void CalculatePharmacySummaryNew()
        {
            double Total, Concession, NetAmount, TotalVat;
            Total = Concession = NetAmount = TotalVat = PackageConcenssion = 0;
            for (int i = 0; i < PharmacyItems.Count; i++)
            {
                Total += (PharmacyItems[i].Amount);
                Concession += PharmacyItems[i].ConcessionAmount;
                TotalVat += PharmacyItems[i].VATAmount;
                NetAmount += PharmacyItems[i].NetAmount;

                if (PharmacyItems[i].PackageID > 0)
                {
                    //PackageConcenssion += PharmacyItems[i].ConcessionAmount;          // For Package New Changes Commented on 16062018
                    PackageConcenssion += PharmacyItems[i].PackageConcessionAmount;     // For Package New Changes Added on 16062018
                }
            }

            txtPharmacyTotal.Text = String.Format("{0:0.00}", Total);
            txtPharmacyConcession.Text = String.Format("{0:0.00}", Concession);
            txtPharmacyNetAmount.Text = String.Format("{0:0.00}", NetAmount);

            txtNetAmount.Text = String.Format("{0:0.00}", NetAmount);
            txtTotalBill.Text = String.Format("{0:0.00}", Total);
            txtTotalConcession.Text = String.Format("{0:0.00}", Concession);

            //By Anjali............................
            //txtRountOffAmount.Text = Math.Round(NetAmount).ToString();
            txtRountOffAmount.Text = GetRoundOffNetAmount(string.IsNullOrEmpty(txtNetAmount.Text) ? 0 : Convert.ToDouble(txtNetAmount.Text), 0).ToString();
            //roundAmt = Math.Round(NetAmount);
            roundAmt = GetRoundOffNetAmount(string.IsNullOrEmpty(txtNetAmount.Text) ? 0 : Convert.ToDouble(txtNetAmount.Text), 0);
            //.....................................

            txtPayAmount.Text = txtNetAmount.Text;



            //PaymentWindow paymentWin = new PaymentWindow();
            //paymentWin.IsFromPharmacyBill = true;

            //paymentWin.Initiate("Bill");
            //// Uncomment by  BHUSHAN....  FOr Advance Cusumed On Pharamacy Bill . . .
            //paymentWin.tabControl1.Visibility = System.Windows.Visibility.Visible;

            ////paymentWin.txtPayTotalAmount.Text = this.txtNetAmount.Text;
            //paymentWin.txtPayTotalAmount.Text = this.txtPharmacyTotal.Text;
            ////By Anjali......................................
            ////paymentWin.txtPayableAmt.Text = this.txtPayAmount.Text;
            ////paymentWin.TotalAmount = double.Parse(txtPayAmount.Text);
            //paymentWin.txtPayableAmt.Text = txtRountOffAmount.Text;
            //paymentWin.TotalAmount = double.Parse(txtRountOffAmount.Text);
            ////...............................................................
            //paymentWin.txtDiscAmt.Text = this.txtTotalConcession.Text;


            //paymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstBill;
            //ClickedFlag = 1;
            //IsFreez = true;
            //paymentWin.OnSaveButton_Click += new RoutedEventHandler(paymentWin_OnSaveButton_Click);
            //paymentWin.OnCancelButton_Click += new RoutedEventHandler(paymentWin_OnCancelButton_Click);
            ////  paymentWin.StoreID = StoreID;
            //paymentWin.Show();


        }
        void Win_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlagForApplyRules = 0;
            //dgPharmacyItems.ItemsSource = null;
            //dgPharmacyItems.ItemsSource = PharmacyItems;
        }

        private void cmbUOM_GotFocus(object sender, RoutedEventArgs e)
        {
            AutoCompleteComboBox cmbConversions = (AutoCompleteComboBox)sender;

            if (cmbConversions.ItemsSource == null || (cmbConversions.ItemsSource != null && ((List<MasterListItem>)cmbConversions.ItemsSource).ToList().Count == 0) || (((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).UOMConversionList == null || ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).UOMConversionList.Count == 0))
            {
                FillUOMConversions(cmbConversions);
            }
        }

        private void cmbUOM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgPharmacyItems.SelectedItem != null && ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).UOMConversionList != null && ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).UOMConversionList.Count > 0)
            {
                //List<clsConversionsVO> UOMConvertLIst = new List<clsConversionsVO>();

                //if (dgAddOpeningBalanceItems.SelectedItem != null)
                //    UOMConvertLIst = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).UOMConversionList;

                clsConversionsVO objConversion = new clsConversionsVO();

                AutoCompleteComboBox cmbConversions = (AutoCompleteComboBox)sender;
                long SelectedUomId = 0;


                if (((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SelectedUOM != null)
                {
                    CalculateConversionFactorCentral(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SelectedUOM.ID, ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SUOMID);
                }

                if (cmbConversions.SelectedItem != null && ((MasterListItem)cmbConversions.SelectedItem).ID > 0)
                {
                    ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SelectedUOM = ((MasterListItem)cmbConversions.SelectedItem);

                    ////objConversion = CalculateConversionFactor(((MasterListItem)cmbConversions.SelectedItem).ID, ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseUOMID);
                    //CalculateConversionFactor(((MasterListItem)cmbConversions.SelectedItem).ID, ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SUOMID);

                    // Function Parameters
                    // FromUOMID - Transaction UOM
                    // ToUOMID - Stocking UOM
                    //CalculateConversionFactorCentral(((MasterListItem)cmbConversions.SelectedItem).ID, ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SUOMID);
                }
                else
                {
                    //MasterListItem objConversionSet = new MasterListItem();
                    //objConversionSet.ID = 0;
                    //objConversionSet.Description = "- Select -";

                    //cmbConversions.SelectedItem = objConversionSet;

                    ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SingleQuantity = 0;
                    ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity = 0;

                    ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConversionFactor = 0;
                    ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseConversionFactor = 0;

                    ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).MRP = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).MainMRP;
                    ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).PurchaseRate = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).MainRate;
                }

                ////if (cmbConversions.SelectedItem != null)
                ////    SelectedUomId = ((MasterListItem)cmbConversions.SelectedItem).ID;

                ////if (UOMConvertLIst.Count > 0)
                ////    objConversion = UOMConvertLIst.Where(z => z.FromUOMID == SelectedUomId && z.ToUOMID == ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SUOMID).FirstOrDefault();

                //if (cmbConversions.SelectedItem != null)
                //{
                //    if (((MasterListItem)cmbConversions.SelectedItem).ID == ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SUOMID)
                //    {
                //        ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor = 1;
                //    }
                //    else
                //    {
                //        if (objConversion != null)
                //        {
                //            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor = objConversion.ConversionFactor;
                //        }
                //        else
                //        {
                //            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor = 0;
                //        }
                //    }
                //}

                ////objConversion.ID = 0;
                ////objConversion.Description = "- Select -";
                ////UOMConvertLIst.Add(objConversion);

                ////UOMConvertLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList);
                ////cmbConversions.ItemsSource = UOMConvertLIst.DeepCopy();

                ////List<clsConversionsVO> ConvertLst = new List<clsConversionsVO>();
                ////ConvertLst.AddRange(UOMConversionLIst.GroupBy(x => x.FromUOM).Select(y => y.First()).ToList());
                ////ConvertLst.AddRange(UOMConversionLIst.GroupBy(x => x.ToUOM).Select(y => y.First()).ToList());

                ////List<clsConversionsVO> MainConvertLst = new List<clsConversionsVO>();
                ////MainConvertLst = UOMConversionLIst.GroupBy(x => x.FromUOM).Select(y => y.First()).ToList().DeepCopy();

                ////ConvertLst.AddRange(UOMConversionLIst.GroupBy(x => x.FromUOM).Select(y => y.First()).ToList());


                //((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SingleQuantity = 0;
                //((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Quantity = 0;


                ////if (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID != ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SUOMID)
                ////{
                ////    if (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor > 0)
                ////    {
                ////        ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MRP = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainMRP / ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor);
                ////        ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Rate = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainRate / ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor);
                ////    }
                ////}
                ////else
                ////{

                ////    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MRP = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainMRP;
                ////    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Rate = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainRate;

                ////}

                ////if (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedPUM.Description != ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).PUOM)
                ////{

                ////    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MRP = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainMRP;
                ////    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Rate = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainRate;

                ////}
                ////else
                ////{


                ////    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MRP = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainMRP / ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor);
                ////    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Rate = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainRate / ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor);

                ////}


                //CalculateOpeningBalanceSummary();
                CalculatePharmacySummary();
            }
        }

        // Method To Fill Unit Of Mesurements with Conversion Factors for Selected Item


        private void CalculateConversionFactorCentral(long FromUOMID, long ToUOMID)
        {
            clsConversions objConversionVO = new clsConversions();

            try
            {
                List<clsConversionsVO> UOMConvertLIst = new List<clsConversionsVO>();

                if (dgPharmacyItems.SelectedItem != null)
                {
                    UOMConvertLIst = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).UOMConversionList;



                    objConversionVO.UOMConvertLIst = UOMConvertLIst;

                    objConversionVO.MainMRP = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).MainMRP;
                    objConversionVO.MainRate = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).MainRate;
                    objConversionVO.SingleQuantity = Convert.ToSingle(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity);
                    //objConversionVO.SingleQuantity = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).SingleQuantity;

                    long BaseUOMID = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseUOMID;
                    //long TransactionUOMID = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID;

                    // Function Parameters
                    // FromUOMID - Transaction UOM
                    // ToUOMID - Stocking UOM
                    // BaseUOMID - Base UOM
                    objConversionVO.CalculateConversionFactor(FromUOMID, ToUOMID, BaseUOMID);

                    ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).PurchaseRate = objConversionVO.Rate;
                    ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).MRP = objConversionVO.MRP;


                    //((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Quantity = objConversionVO.Quantity;
                    ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity = objConversionVO.SingleQuantity;
                    //((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).TotalQuantity =Convert.ToDouble(objConversionVO.Quantity);
                    ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseQuantity = objConversionVO.BaseQuantity;

                    ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseRate = objConversionVO.BaseRate;
                    ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseMRP = objConversionVO.BaseMRP;

                    ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConversionFactor = objConversionVO.ConversionFactor;
                    ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseConversionFactor = objConversionVO.BaseConversionFactor;

                    //Log Write here   

                    // lineNumber = stackFrame.GetFileLineNumber();

                    objGUID = new Guid();

                    long TransactionUOMID = 0;
                    if (dgPharmacyItems.SelectedItem != null && ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SelectedUOM != null)
                    {
                        TransactionUOMID = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SelectedUOM.ID;
                    }
                    else
                    {
                        TransactionUOMID = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).TransactionUOMID;
                    }
                    if (IsAuditTrail)
                    {
                        LogInformation = new LogInfo();
                        LogInformation.guid = objGUID;
                        LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        LogInformation.TimeStamp = DateTime.Now;
                        LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                        LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                        LogInformation.Message = " XI : Item UOM Changed  : " //+ Convert.ToString(lineNumber)
                                                                + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                                + " , StoreID : " + Convert.ToString(StoreID) + " "
                                                                + " , ItemID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ItemID) + " "
                                                                + " , BatchID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BatchID) + " "
                                                                + " , BatchCode : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BatchCode) + " "
                                                                + " , ExpiryDate : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ExpiryDate) + " "
                                                                + " , Transaction UOMID : " + Convert.ToString(TransactionUOMID) + " "
                                                                + " , Transaction Quantity (InputTransactionQuantity) : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity) + " "
                                                                + " , BaseConversionFactor : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseConversionFactor) + " "
                                                                + " , Base Quantity (TransactionQuantity) : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseQuantity) + " "
                                                                + " , Base UOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseUOMID) + " "
                                                                + " , SUOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SUOMID) + " "
                                                                + " , StockCF : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConversionFactor) + " "
                                                                + " , Stocking Quantity : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity * ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConversionFactor) + " "
                                                                + " , PUOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).PUOMID) + " "
                                                                + " , ConcessionPercentage : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConcessionPercentage) + " "
                                                                + " , ConcessionAmount : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConcessionAmount) + " "
                                                                + " , AvailableStockInBase : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableStockInBase) + " ";


                        LogInfoList.Add(LogInformation);
                    }
                    //CallLogBizAction(LogBizAction);
                    ////

                    if (((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity <= 0)
                    {
                        float availQty = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableStockInBase;

                        ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity = 1;
                        string msgText = "Quantity cannot be zero or less than zero";
                        ConversionsForAvailableStock();
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD.Show();
                    }
                    else
                        if (((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseQuantity > ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableStockInBase)
                        {
                            float availQty = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableStockInBase;

                            ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity = Convert.ToSingle(Math.Floor(Convert.ToDouble(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableStockInBase / objConversionVO.BaseConversionFactor)));
                            string msgText = "Quantity Must Be Less Than Or Equal To Available Quantity ";
                            ConversionsForAvailableStock();
                            MessageBoxControl.MessageBoxChildWindow msgWD =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWD.Show();
                        }
                        else if (((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SelectedUOM != null && ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SelectedUOM.ID == ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseUOMID && (((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity % 1) != 0)
                        {
                            ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity = 1;
                            string msgText = "Quantity Cannot be in fraction";
                            ConversionsForAvailableStock();
                            MessageBoxControl.MessageBoxChildWindow msgWD =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWD.Show();

                        }
                    CalculatePharmacySummary();


                }

            }
            catch (Exception ex)
            {
                objConversionVO = null;
            }
        }
        private void ConversionsForAvailableStock()
        {
            //Log Write here   

            // lineNumber = stackFrame.GetFileLineNumber();

            objGUID = new Guid();

            long TransactionUOMID = 0;
            if (dgPharmacyItems.SelectedItem != null && ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SelectedUOM != null)
            {
                TransactionUOMID = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SelectedUOM.ID;
            }
            else
            {
                TransactionUOMID = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).TransactionUOMID;
            }
            if (IsAuditTrail)
            {
                LogInformation = new LogInfo();
                LogInformation.guid = objGUID;
                LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                LogInformation.TimeStamp = DateTime.Now;
                LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                LogInformation.Message = " XII : Line Number : " //+ Convert.ToString(lineNumber)
                                                        + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                        + " , StoreID : " + Convert.ToString(StoreID) + " "
                                                        + " , ItemID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ItemID) + " "
                                                        + " , BatchID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BatchID) + " "
                                                        + " , BatchCode : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BatchCode) + " "
                                                        + " , ExpiryDate : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ExpiryDate) + " "
                                                        + " , Transaction UOMID : " + Convert.ToString(TransactionUOMID) + " "
                                                        + " , Transaction Quantity (InputTransactionQuantity) : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity) + " "
                                                        + " , BaseConversionFactor : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseConversionFactor) + " "
                                                        + " , Base Quantity (TransactionQuantity) : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseQuantity) + " "
                                                        + " , Base UOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseUOMID) + " "
                                                        + " , SUOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SUOMID) + " "
                                                        + " , StockCF : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConversionFactor) + " "
                                                        + " , Stocking Quantity : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity * ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConversionFactor) + " "
                                                        + " , PUOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).PUOMID) + " "
                                                        + " , ConcessionPercentage : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConcessionPercentage) + " "
                                                        + " , ConcessionAmount : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConcessionAmount) + " "
                                                        + " , AvailableStockInBase : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableStockInBase) + " ";


                LogInfoList.Add(LogInformation);
            }
            //CallLogBizAction(LogBizAction);
            ////

            ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseQuantity = Convert.ToSingle(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity) * ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseConversionFactor;
            //((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseRate = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).MainRate * ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseConversionFactor;
            //((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseMRP = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).MainMRP * ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseConversionFactor;
            CalculatePharmacySummary();


            //Log Write here   

            // lineNumber = stackFrame.GetFileLineNumber();

            objGUID = new Guid();
            if (IsAuditTrail)
            {
                LogInformation = new LogInfo();
                LogInformation.guid = objGUID;
                LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                LogInformation.TimeStamp = DateTime.Now;
                LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                LogInformation.Message = " XIII : Line Number : " //+ Convert.ToString(lineNumber)
                                                        + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                        + " , StoreID : " + Convert.ToString(StoreID) + " "
                                                        + " , ItemID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ItemID) + " "
                                                        + " , BatchID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BatchID) + " "
                                                        + " , BatchCode : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BatchCode) + " "
                                                        + " , ExpiryDate : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ExpiryDate) + " "
                                                        + " , Transaction UOMID : " + Convert.ToString(TransactionUOMID) + " "
                                                        + " , Transaction Quantity (InputTransactionQuantity) : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity) + " "
                                                        + " , BaseConversionFactor : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseConversionFactor) + " "
                                                        + " , Base Quantity (TransactionQuantity) : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseQuantity) + " "
                                                        + " , Base UOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseUOMID) + " "
                                                        + " , SUOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SUOMID) + " "
                                                        + " , StockCF : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConversionFactor) + " "
                                                        + " , Stocking Quantity : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity * ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConversionFactor) + " "
                                                        + " , PUOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).PUOMID) + " "
                                                        + " , ConcessionPercentage : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConcessionPercentage) + " "
                                                        + " , ConcessionAmount : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConcessionAmount) + " "
                                                        + " , AvailableStockInBase : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableStockInBase) + " ";





                LogInfoList.Add(LogInformation);
            }
            //CallLogBizAction(LogBizAction);
            ////
        }
        private void FillUOMConversions(AutoCompleteComboBox cmbConversions)
        {
            WaitIndicator IndicatiorConversions = new WaitIndicator();
            try
            {

                IndicatiorConversions.Show();

                clsGetItemConversionFactorListBizActionVO BizAction = new clsGetItemConversionFactorListBizActionVO();

                BizAction.ItemID = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ItemID;
                BizAction.UOMConversionList = new List<clsConversionsVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        IndicatiorConversions.Close();

                        List<MasterListItem> UOMConvertLIst = new List<MasterListItem>();
                        MasterListItem objConversion = new MasterListItem();
                        objConversion.ID = 0;
                        objConversion.Description = "- Select -";
                        UOMConvertLIst.Add(objConversion);

                        if (((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList != null)
                            UOMConvertLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList);
                        cmbConversions.ItemsSource = UOMConvertLIst.DeepCopy();




                        if (((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SelectedUOM != null)
                        {
                            // cmbConversions.SelectedValue = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SelectedUOM.ID;
                            cmbConversions.SelectedItem = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SelectedUOM;
                        }
                        else if (UOMConvertLIst != null)
                            cmbConversions.SelectedItem = UOMConvertLIst[0];

                        //List<clsConversionsVO> ConvertLst = new List<clsConversionsVO>();
                        //ConvertLst.AddRange(UOMConversionLIst.GroupBy(x => x.FromUOM).Select(y => y.First()).ToList());
                        //ConvertLst.AddRange(UOMConversionLIst.GroupBy(x => x.ToUOM).Select(y => y.First()).ToList());

                        //List<clsConversionsVO> MainConvertLst = new List<clsConversionsVO>();
                        //MainConvertLst = UOMConversionLIst.GroupBy(x => x.FromUOM).Select(y => y.First()).ToList().DeepCopy();

                        //ConvertLst.AddRange(UOMConversionLIst.GroupBy(x => x.FromUOM).Select(y => y.First()).ToList());



                        List<clsConversionsVO> UOMConversionLIst = new List<clsConversionsVO>();
                        UOMConversionLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConversionList);

                        if (dgPharmacyItems.SelectedItem != null)
                        {
                            ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).UOMConversionList = UOMConversionLIst.DeepCopy();
                            ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).UOMList = UOMConvertLIst.DeepCopy();
                            cmbConversions.SelectedItem = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SelectedUOM;
                            cmbConversions.SelectedValue = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SelectedUOM.ID;
                        }


                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                IndicatiorConversions.Close();
                throw;
            }


        }
        int index = -1;
        private void cmbUOM_Loaded(object sender, RoutedEventArgs e)
        {
            //AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;
            //if (((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem) != null)
            //{

            //   if (dgPharmacyItems.SelectedIndex != index)
            //    if (cmbConversions.ItemsSource == null || (cmbConversions.ItemsSource != null && ((List<MasterListItem>)cmbConversions.ItemsSource).ToList().Count == 0) || (((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).UOMConversionList == null || ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).UOMConversionList.Count == 0))
            //    {
            //        FillUOMConversions(cmbConversions);
            //    }
            //   index = dgPharmacyItems.SelectedIndex;
            //}
        }
        int ClickedFlagForApplyRules = 0;
        private void CmdApplyRules_Click(object sender, RoutedEventArgs e)
        {
            if (PharmacyItems != null && PharmacyItems.Count > 0)
            {
                ClickedFlagForApplyRules += 1;
                if (ClickedFlagForApplyRules == 1)
                {
                    bool isValid = true;
                    foreach (var item in PharmacyItems)
                    {
                        //Log Write here   

                        // lineNumber = stackFrame.GetFileLineNumber();

                        objGUID = new Guid();

                        long TransactionUOMID = 0;
                        if (item.SelectedUOM != null)
                        {
                            TransactionUOMID = item.SelectedUOM.ID;
                        }
                        else
                        {
                            TransactionUOMID = item.TransactionUOMID;
                        }

                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " XIV : Line Number : " //+ Convert.ToString(lineNumber)
                                                                    + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                                    + " , StoreID : " + Convert.ToString(StoreID) + " "
                                                                    + " , ItemID : " + Convert.ToString(item.ItemID) + " "
                                                                    + " , BatchID : " + Convert.ToString(item.BatchID) + " "
                                                                    + " , BatchCode : " + Convert.ToString(item.BatchCode) + " "
                                                                    + " , ExpiryDate : " + Convert.ToString(item.ExpiryDate) + " "
                                                                    + " , Transaction UOMID : " + Convert.ToString(TransactionUOMID) + " "
                                                                    + " , Transaction Quantity (InputTransactionQuantity) : " + Convert.ToString(item.Quantity) + " "
                                                                    + " , BaseConversionFactor : " + Convert.ToString(item.BaseConversionFactor) + " "
                                                                    + " , Base Quantity (TransactionQuantity) : " + Convert.ToString(item.BaseQuantity) + " "
                                                                    + " , Base UOMID : " + Convert.ToString(item.BaseUOMID) + " "
                                                                    + " , SUOMID : " + Convert.ToString(item.SUOMID) + " "
                                                                    + " , StockCF : " + Convert.ToString(item.ConversionFactor) + " "
                                                                    + " , Stocking Quantity : " + Convert.ToString(item.Quantity * item.ConversionFactor) + " "
                                                                    + " , PUOMID : " + Convert.ToString(item.PUOMID) + " "
                                                                    + " , ConcessionPercentage : " + Convert.ToString(item.ConcessionPercentage) + " "
                                                                    + " , ConcessionAmount : " + Convert.ToString(item.ConcessionAmount) + " "
                                                                    + " , AvailableStockInBase : " + Convert.ToString(item.AvailableStockInBase) + " ";


                            LogInfoList.Add(LogInformation);
                        }
                        //CallLogBizAction(LogBizAction);
                        ////

                        if (item.BaseQuantity > item.AvailableStockInBase)//item.Quantity > item.AvailableQuantity
                        {
                            isValid = false;
                            string msgText = "Available Quantity For " + item.ItemName + " Is " + String.Format("{0:0.00}", item.AvailableQuantity);

                            MessageBoxControl.MessageBoxChildWindow msgWD =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgWD.Show();
                            ClickedFlagForApplyRules = 0;

                            break;
                        }
                        else if (item.SelectedUOM.ID == 0 || item.SelectedUOM.ID == null)
                        {
                            isValid = false;
                            string msgText = "Please Select UOM For Item" + item.ItemName;

                            MessageBoxControl.MessageBoxChildWindow msgWD =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgWD.Show();
                            ClickedFlagForApplyRules = 0;

                            break;
                        }
                    }
                    isValid = ChkValidation();
                    if (isValid && resultround == true)
                    {
                        chkFreezBill.IsChecked = true;
                        if (chkFreezBill.IsChecked == true)
                        {
                            isValid = false;

                            if (cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)
                            {
                                string msgText = "Are You Sure You Want To Apply Package Discount ?";
                                MessageBoxControl.MessageBoxChildWindow msgWD =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                msgWD.OnMessageBoxClosed += (arg) =>
                               {
                                   if (arg == MessageBoxResult.Yes)
                                   {
                                       ApplyRules();
                                   }
                                   else
                                   {
                                       ClickedFlagForApplyRules = 0;
                                   }

                               };
                                msgWD.Show();

                            }
                        }
                        else
                        {
                            ClickedFlagForApplyRules = 0;
                        }
                    }
                    else
                    {
                        ClickedFlagForApplyRules = 0;
                    }

                }
                else
                {
                    ClickedFlagForApplyRules = 0;
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgWD =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Add Items", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgWD.Show();

            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgPharmacyItems.SelectedItem != null)
            {
                Conversion win = new Conversion();

                win.FillUOMConversions(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ItemID, ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SelectedUOM.ID);
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

            ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SelectedUOM = (MasterListItem)Itemswin.cmbConversion.SelectedItem;
            ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).UOMList = Itemswin.UOMConvertLIst;
            ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).UOMConversionList = Itemswin.UOMConversionLIst;

            CalculateConversionFactorCentral(((MasterListItem)Itemswin.cmbConversion.SelectedItem).ID, ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SUOMID);



        }
        //...................................
        private void FillUOMConversions()
        {
            WaitIndicator IndicatiorConversions = new WaitIndicator();
            try
            {

                IndicatiorConversions.Show();

                clsGetItemConversionFactorListBizActionVO BizAction = new clsGetItemConversionFactorListBizActionVO();

                BizAction.ItemID = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ItemID;
                BizAction.UOMConversionList = new List<clsConversionsVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        IndicatiorConversions.Close();

                        List<MasterListItem> UOMConvertLIst = new List<MasterListItem>();
                        MasterListItem objConversion = new MasterListItem();
                        objConversion.ID = 0;
                        objConversion.Description = "- Select -";
                        UOMConvertLIst.Add(objConversion);

                        if (((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList != null)
                            UOMConvertLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList);
                        List<clsConversionsVO> UOMConversionLIst = new List<clsConversionsVO>();
                        UOMConversionLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConversionList);

                        if (dgPharmacyItems.SelectedItem != null)
                        {
                            ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).UOMConversionList = UOMConversionLIst.DeepCopy();
                            ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).UOMList = UOMConvertLIst.DeepCopy();

                        }
                        CalculateConversionFactorCentral(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SelectedUOM.ID, ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SUOMID);


                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                IndicatiorConversions.Close();
                throw;
            }


        }

        private void txtReferenceDoctor_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!((AutoCompleteBox)sender).Text.IsOnlyCharacters())
            {
                ((AutoCompleteBox)sender).Text = textBefore;
                //((AutoCompleteBox)sender).SelectionStart = selectionStart;
                //((AutoCompleteBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtReferenceDoctor_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((AutoCompleteBox)sender).Text;
            //selectionStart = ((AutoCompleteBox)sender).selectionStart;
            //selectionLength = ((AutoCompleteBox)sender).SelectionLength;
        }
        //.........................................................................
        private void FillPatientSource(long PatientCategoryID)
        {
            #region Commented BY CDS
            //clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();

            //BizAction.MasterTable = MasterTableNameList.M_PatientCategoryMaster;
            //BizAction.MasterList = new List<MasterListItem>();

            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //Client.ProcessCompleted += (s, e) =>
            //{
            //    if (e.Error == null && e.Result != null)
            //    {
            //        List<MasterListItem> objList = new List<MasterListItem>();

            //        MasterListItem Default = new MasterListItem(0, "- Select -");

            //        objList.Add(Default);
            //        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

            //        cmbPatientCategory.ItemsSource = null;
            //        cmbPatientCategory.ItemsSource = objList;


            //        cmbPatientCategory.SelectedItem = Default;

            //        if (this.DataContext != null)
            //        {

            //            cmbPatientCategory.SelectedValue = ((clsPatientSponsorVO)this.DataContext).PatientCategoryID;
            //        }
            //    }
            //};

            //Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //Client.CloseAsync();
            #endregion

            clsGetPatientCategoryMasterVO BizAction = new clsGetPatientCategoryMasterVO();

            BizAction.PatientSourceID = PatientCategoryID;

            //if (myPatient != null && myPatient.GeneralDetails.PatientSourceID == pPatientsourceID)
            //{
            //    BizAction.ParentPatientID = myPatient.ParentPatientID;
            //    BizAction.PatientSourceType = 1; //For Loyalty
            //}

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                List<MasterListItem> objList = new List<MasterListItem>();

                objList.Add(new MasterListItem(0, "- Select -"));
                if (e.Error == null && e.Result != null)
                {
                    objList.AddRange(((clsGetPatientCategoryMasterVO)e.Result).List);
                }

                cmbPatientSource.ItemsSource = null;
                cmbPatientSource.ItemsSource = objList;


                if (searchPatient != true)
                {
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.CategoryID == ((MasterListItem)cmbPatientCategory.SelectedItem).ID)
                        cmbPatientSource.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientSourceID;
                    else
                        cmbPatientSource.SelectedValue = (long)0;
                }
                else
                {
                    if (IsPatientSelect == true)    // addede on 07082018 to set values on dropdown selection changed
                    {
                        if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID != 0)
                        {
                            cmbPatientSource.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;
                        }
                        else
                        {
                            cmbPatientSource.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientSourceID;

                        }
                    }
                    else
                    {
                        cmbPatientSource.SelectedValue = (long)0;   // addede on 07082018 to set values on dropdown selection changed
                    }
                }

                //FillCompany(((MasterListItem)cmbPatientSource.SelectedItem).ID);      // Commented on 07082018 to avoid multiple calls to FillCompany()



            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }

        private void FillCompany(long PatientSourceID)
        {

            clsGetCompanyMasterVO BizAction = new clsGetCompanyMasterVO();

            BizAction.PatientCategoryID = PatientSourceID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                List<MasterListItem> objList = new List<MasterListItem>();

                objList.Add(new MasterListItem(0, "- Select -"));
                if (e.Error == null && e.Result != null)
                {
                    objList.AddRange(((clsGetCompanyMasterVO)e.Result).List);
                }

                cmbCompany.ItemsSource = null;
                cmbCompany.ItemsSource = objList;
                //cmbCompany.SelectedItem = objList[0];     // comment on 08082018 to avoid multiple call


                if (objList.Count > 1)
                {
                    //if (((IApplicationConfiguration)App.Current).SelectedPatient.NewPatientCategoryID == ((MasterListItem)cmbPatientCategory.SelectedItem).ID)
                    if (searchPatient == true && ((IApplicationConfiguration)App.Current).SelectedPatient.NewPatientCategoryID == ((MasterListItem)cmbPatientCategory.SelectedItem).ID)
                    {
                        if (IsPatientSelect == true)    // addede on 07082018 to set values on dropdown selection changed
                        {
                            if (((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID != 0)
                            {
                                cmbCompany.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
                            }
                            else
                            {
                                cmbCompany.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfCompanyID;
                            }
                        }
                        else
                        {
                            cmbCompany.SelectedValue = (long)0;   // addede on 07082018 to set values on dropdown selection changed
                        }
                    }
                    else
                    {
                        if (searchPatient != true)    // addede on 07082018 to set values on dropdown selection changed
                        {
                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.CategoryID == ((MasterListItem)cmbPatientCategory.SelectedItem).ID)
                            {
                                cmbCompany.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfCompanyID;
                            }
                            else
                            {
                                cmbCompany.SelectedItem = objList[0];
                            }
                        }
                        else
                        {
                            cmbCompany.SelectedValue = (long)0;   // addede on 07082018 to set values on dropdown selection changed
                        }
                    }
                }
                else
                    cmbCompany.SelectedItem = objList[0];

                //FillTariffMasterNew(0, ((MasterListItem)cmbCompany.SelectedItem).ID);     // Commented on 07082018 to avoid multiple calls to FillTariffMasterNew()




            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

        }

        private void FillTariffMasterNew(long pPatientsourceID, long pCompanyID)
        {
            clsGetTariffMasterVO BizAction = new clsGetTariffMasterVO();

            BizAction.CompanyID = pCompanyID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                List<MasterListItem> objList = new List<MasterListItem>();

                objList.Add(new MasterListItem(0, "- Select -"));
                if (e.Error == null && e.Result != null)
                {
                    objList.AddRange(((clsGetTariffMasterVO)e.Result).List);
                }

                cmbTariff.ItemsSource = null;
                cmbTariff.ItemsSource = objList;

                if (cmbCompany.SelectedItem != null)
                {
                    if (((MasterListItem)cmbCompany.SelectedItem).ID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CompanyPatientSourceID)
                    {
                        cmbTariff.SelectedItem = objList[0];
                    }
                }

                if (searchPatient != true)
                    cmbTariff.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID;
                else
                {
                    if (((IApplicationConfiguration)App.Current).SelectedPatient.TariffID != 0)
                    {
                        cmbTariff.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatient.TariffID;
                    }
                    else
                    {
                        cmbTariff.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID;
                    }
                }

            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

        }
        //..........................................................................


        private void SaveDetails(clsPaymentVO pPayDetails, bool pFreezBill, long pPatientID, long pPatientUnitID)
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            try
            {
                clsAddPatientBizActionVO PatientDetails = new clsAddPatientBizActionVO();
                clsAddVisitBizActionVO Visit = new clsAddVisitBizActionVO();


                if (txtMRNo.Text == "")
                {
                    //For Patient Details.......

                    PatientDetails.IsSavePatientFromOPD = true;
                    PatientDetails.IsSaveSponsor = true;
                    PatientDetails.PatientDetails = (clsPatientVO)this.DataContext;

                    PatientDetails.PatientDetails.AddressLine1 = ((clsPatientVO)this.DataContext).AddressLine1;

                    if (dtpDOB.SelectedDate == null)
                    {
                        PatientDetails.PatientDetails.GeneralDetails.IsAge = true;
                        if (DOB != null)
                            PatientDetails.PatientDetails.GeneralDetails.DateOfBirth = DOB.Value.Date;
                    }
                    else
                    {
                        PatientDetails.PatientDetails.GeneralDetails.DateOfBirth = dtpDOB.SelectedDate;
                        PatientDetails.PatientDetails.GeneralDetails.IsAge = false;
                    }
                    PatientDetails.PatientDetails.GeneralDetails.RegType = (short)PatientRegistrationType.Pharmacy;
                    PatientDetails.PatientDetails.GeneralDetails.PatientTypeID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyPatientCategoryID;
                    if (cmbGender.SelectedItem != null)
                        PatientDetails.PatientDetails.GenderID = ((MasterListItem)cmbGender.SelectedItem).ID;

                    PatientDetails.PatientDetails.ContactNo1 = txtContactNo.Text.Trim();
                    if (!string.IsNullOrEmpty(txtMobileCountryCode.Text.Trim()))
                        PatientDetails.PatientDetails.MobileCountryCode = txtMobileCountryCode.Text.Trim();
                    PatientDetails.PatientDetails.OccupationId = 0;

                    PatientDetails.PatientDetails.IsStaffPatient = IsStaff;
                    PatientDetails.PatientDetails.StaffID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    //..........................

                    // For Sponser..................
                    clsAddPatientSponsorBizActionVO PSponser = new clsAddPatientSponsorBizActionVO();
                    PSponser.PatientSponsorDetails = new clsPatientSponsorVO()
                   {
                       PatientId = pPatientID,
                       PatientUnitId = pPatientUnitID,
                       PatientCategoryID = ((MasterListItem)cmbPatientCategory.SelectedItem).ID,
                       PatientSourceID = ((MasterListItem)cmbPatientSource.SelectedItem).ID,
                       CompanyID = ((MasterListItem)cmbCompany.SelectedItem).ID,
                       TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID
                   };
                    PatientDetails.BizActionVOSaveSponsor = PSponser;


                    //................................

                    //added by rohinee for audit trail
                    if (IsAuditTrail)
                    {
                        LogInformation = new LogInfo();
                        LogInformation.guid = objGUID;
                        LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        LogInformation.TimeStamp = DateTime.Now;
                        LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                        LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                        LogInformation.Message = " XVIII : New Patient Register : " //+ Convert.ToString(lineNumber)
                                                                + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                                + " , FirstName : " + Convert.ToString(PatientDetails.PatientDetails.GeneralDetails.FirstName) + " "
                                                                + " , MiddleName : " + Convert.ToString(PatientDetails.PatientDetails.GeneralDetails.MiddleName) + " "
                                                                + " , LastName : " + Convert.ToString(PatientDetails.PatientDetails.GeneralDetails.LastName) + " "
                                                                + " , DateOfBirth : " + Convert.ToString(PatientDetails.PatientDetails.GeneralDetails.DateOfBirth) + " "
                                                                + " , ExpiryDate : " + Convert.ToString(PatientDetails.PatientDetails.GeneralDetails.IsAge) + " "
                                                                + " , RegType : " + Convert.ToString(PatientDetails.PatientDetails.GeneralDetails.RegType) + " "
                                                                + " , PatientTypeID : " + Convert.ToString(PatientDetails.PatientDetails.GeneralDetails.PatientTypeID) + " "
                                                                + " , GenderID : " + Convert.ToString(PatientDetails.PatientDetails.GenderID) + " "
                                                                + " , Mobile : " + Convert.ToString(PatientDetails.PatientDetails.MobileCountryCode) + " " + Convert.ToString(PatientDetails.PatientDetails.ContactNo1) + " "
                                                                + " , Age : " + Convert.ToString(txtYY.Text) + " YY " + Convert.ToString(txtMM.Text) + " MM " + Convert.ToString(txtDD.Text) + " DD "
                                                                + " , ReferenceDoctor : " + Convert.ToString(txtReferenceDoctor.Text) + " "
                                                                + " , PurchaseFrequency : " + Convert.ToString(txtPurchaseFrequency.Text) + " "
                                                                + " , PatientSourceID : " + Convert.ToString(PSponser.PatientSponsorDetails.PatientSourceID) + " "
                                                                + " , PatientSourceID : " + Convert.ToString(PSponser.PatientSponsorDetails.PatientCategoryID) + " "
                                                                + " , CompanyID : " + Convert.ToString(PSponser.PatientSponsorDetails.CompanyID) + " "
                                                                + " , TariffID : " + Convert.ToString(PSponser.PatientSponsorDetails.TariffID) + "\r\n";
                        LogInfoList.Add(LogInformation);
                    }

                }
                if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID == 0 || VisitId == 0)
                {
                    //For Visit...........................

                    Visit.VisitDetails = new clsVisitVO();
                    Visit.VisitDetails.PatientId = pPatientID;
                    Visit.VisitDetails.PatientUnitId = pPatientUnitID;
                    Visit.VisitDetails.VisitTypeID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyVisitTypeID;
                    Visit.VisitDetails.ReferredDoctor = txtReferenceDoctor.Text;
                    if (txtReferenceDoctor.SelectedItem != null)
                        Visit.VisitDetails.ReferredDoctorID = ((MasterListItem)txtReferenceDoctor.SelectedItem).ID;
                    Visit.VisitDetails.VisitStatus = false; //As per discussion with girish sir and nilesh sir (25/4/2011) 
                    Visit.VisitDetails.Status = true;

                    //.....................................


                }

                //For Bill......................

                clsBillVO objBill = new clsBillVO();

                if (SelectedBill == null)
                {
                    objBill.Date = DateTime.Now;
                    objBill.Time = DateTime.Now;
                }
                else
                    objBill = SelectedBill.DeepCopy();

                objBill.IsFreezed = pFreezBill;
                objBill.PaymentDetails = pPayDetails;

                objBill.PatientID = pPatientID;
                objBill.Opd_Ipd_External_Id = VisitId;
                objBill.Opd_Ipd_External_UnitId = pPatientUnitID;

                if (pFreezBill)
                {
                    objBill.Date = DateTime.Now;
                    objBill.Time = DateTime.Now;

                    if (pPayDetails != null)
                    {
                        objBill.BalanceAmountSelf = pPayDetails.BillBalanceAmount;
                        objBill.BillPaymentType = pPayDetails.BillPaymentType;
                    }

                    if (!string.IsNullOrEmpty(txtPayAmount.Text))
                        objBill.SelfAmount = Convert.ToDouble(txtPayAmount.Text);
                }

                if (!string.IsNullOrEmpty(txtTotalBill.Text))
                    objBill.TotalBillAmount = Convert.ToDouble(txtTotalBill.Text);

                if (!string.IsNullOrEmpty(txtTotalConcession.Text))
                    objBill.TotalConcessionAmount = Convert.ToDouble(txtTotalConcession.Text);

                if (!string.IsNullOrEmpty(txtNetAmount.Text))
                    objBill.CalculatedNetBillAmount = Convert.ToDouble(txtNetAmount.Text);
                if (!string.IsNullOrEmpty(txtNetAmount.Text))
                    objBill.NetBillAmount = Convert.ToDouble(txtRountOffAmount.Text);

                if (PatientSourceID > 0)
                {
                    objBill.PatientSourceId = PatientSourceID;
                }
                else
                {
                    if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
                        objBill.PatientSourceId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;

                }

                if (cmbTariff.SelectedItem != null)
                    objBill.TariffId = ((MasterListItem)cmbTariff.SelectedItem).ID;
                else
                {
                    if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
                        objBill.TariffId = ((IApplicationConfiguration)App.Current).SelectedPatient.TariffID;
                }

                if (cmbPatientCategory.SelectedItem != null)
                {
                    objBill.PatientCategoryId = ((MasterListItem)cmbPatientCategory.SelectedItem).ID;
                }
                else
                {
                    if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
                        objBill.PatientCategoryId = ((IApplicationConfiguration)App.Current).SelectedPatient.NewPatientCategoryID;
                }

                if (cmbCompany.SelectedItem != null)
                    objBill.CompanyId = ((MasterListItem)cmbCompany.SelectedItem).ID;
                else
                {
                    if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
                        objBill.CompanyId = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
                }

                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                    objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;  //Costing Divisions for Pharmacy Billing
                else
                    objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyCounterID;

                if (objBill.PaymentDetails != null)
                {
                    objBill.PaymentDetails.CostingDivisionID = objBill.CostingDivisionID;   //Costing Divisions for Pharmacy Billing
                }

                objBill.AgainstDonor = AgainstDonor;
                objBill.LinkPatientID = LinkPatientID;
                objBill.LinkPatientUnitID = LinkPatientUnitID;

                if (PharmacyItems.Count > 0)
                {
                    objBill.PharmacyItems.Items = new List<clsItemSalesDetailsVO>();
                    if (txtReasonForVariance.Text != string.Empty)
                        objBill.PharmacyItems.ReasonForVariance = txtReasonForVariance.Text;
                    objBill.PharmacyItems.ReferenceDoctor = txtReferenceDoctor.Text;
                    if (txtReferenceDoctor.SelectedItem != null)
                        objBill.PharmacyItems.ReferenceDoctorID = ((MasterListItem)txtReferenceDoctor.SelectedItem).ID;
                    objBill.PharmacyItems.VisitID = VisitId;
                    objBill.PharmacyItems.PatientID = PatientId;
                    objBill.PharmacyItems.PatientUnitID = pPatientUnitID;
                    objBill.PharmacyItems.Date = objBill.Date;
                    objBill.PharmacyItems.Time = objBill.Time;
                    objBill.PharmacyItems.StoreID = StoreID;
                    objBill.PharmacyItems.CostingDivisionID = objBill.CostingDivisionID;
                    objBill.PharmacyItems.VATAmount = vatamt;
                    objBill.PharmacyItems.VATPercentage = vatper;

                    if (!string.IsNullOrEmpty(txtPharmacyTotal.Text))
                        objBill.PharmacyItems.TotalAmount = Convert.ToDouble(txtPharmacyTotal.Text);

                    if (!string.IsNullOrEmpty(txtPharmacyConcession.Text))
                        objBill.PharmacyItems.ConcessionAmount = Convert.ToDouble(txtPharmacyConcession.Text);

                    if (!string.IsNullOrEmpty(txtRountOffAmount.Text))
                        objBill.PharmacyItems.NetAmount = Convert.ToDouble(txtRountOffAmount.Text);

                    objBill.PharmacyItems.Items = PharmacyItems.ToList();


                }


                //...............................

                clsAddBillBizActionVO BizAction = new clsAddBillBizActionVO();
                BizAction.IsFromCounterSale = true;
                BizAction.Details = new clsBillVO();

                BizAction.PrescriptionDetailsDrugID = StrPrescriptionDetailsID.ToString(); //***//
                if (cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID != 0)
                {
                    BizAction.IsCouterSalesPackage = true;
                }


                if (txtMRNo.Text == "")
                {
                    BizAction.objPatientVODetails = new clsAddPatientBizActionVO();
                    BizAction.objPatientVODetails = PatientDetails;
                }
                if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID == 0 || VisitId == 0)
                {
                    BizAction.objVisitVODetails = new clsAddVisitBizActionVO();
                    BizAction.objVisitVODetails = Visit;
                }
                BizAction.Details = objBill;

                BizAction.LogInfoList = new List<LogInfo>();  // For the Activity Log List
                BizAction.LogInfoList = LogInfoList.DeepCopy();

                #region Package New Changes Added on 28042018

                if (PharmacyItems.Count > 0)
                {
                    if (cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)
                    {
                        BizAction.objPatientPackInfoVODetails = new clsGetPatientPackageInfoListBizActionVO();

                        BizAction.IsPackagePharmacyConsumption = true;      // Set to True when call from Counter Sale to check whether Pharmacy Consumed Amount > Pharmacy  Component

                        BizAction.objPatientPackInfoVODetails.PackageBillID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageBillID;
                        BizAction.objPatientPackInfoVODetails.PackageBillUnitID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageBillUnitID;
                    }
                }

                #endregion

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    Indicatior.Close();
                    ClickedFlag = 0;
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            if (((clsAddBillBizActionVO)arg.Result).SuccessStatus == -10)
                            {
                                Indicatior.Close();
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Insufficient Stock ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW1.Show();
                            }
                            else if (((clsAddBillBizActionVO)arg.Result).SuccessStatusForCSConsumtion == 1)  // Set to 1 when Pharmacy Consumed Amount > Pharmacy  Component // Package New Changes Added on 28042018
                            {
                                if (((clsAddBillBizActionVO)arg.Result).objPatientPackInfoVODetails != null && ((clsAddBillBizActionVO)arg.Result).objPatientPackInfoVODetails.MasterList != null)
                                {
                                    if (((clsAddBillBizActionVO)arg.Result).objPatientPackInfoVODetails.MasterList.Count > 0)
                                    {
                                        if (((clsAddBillBizActionVO)arg.Result).objPatientPackInfoVODetails.MasterList[0].PharmacyConsumeAmount > ((clsAddBillBizActionVO)arg.Result).objPatientPackInfoVODetails.MasterList[0].PharmacyFixedRate)
                                        {
                                            Indicatior.Close();
                                            string MsgCS = "Pharmacy Component Limit Can Not Be Exceed : " + ((clsAddBillBizActionVO)arg.Result).objPatientPackInfoVODetails.MasterList[0].PharmacyFixedRate.ToString();
                                            MessageBoxControl.MessageBoxChildWindow msgW12 =
                                             new MessageBoxControl.MessageBoxChildWindow("Palash", MsgCS, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                            msgW12.Show();
                                        }
                                    }
                                }
                            }
                            else if (((clsAddBillBizActionVO)arg.Result).Details != null)
                            {

                                Indicatior.Close();
                                string strMsg;
                                if (((clsAddBillBizActionVO)arg.Result).Details.BalanceAmountSelf == ((clsAddBillBizActionVO)arg.Result).Details.TotalBillAmount)
                                {
                                    strMsg = "Patient bill is saved a Credit.";
                                }
                                else
                                {
                                    strMsg = "Bill Details Saved Successfully.";
                                }

                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                                    {
                                        if (res == MessageBoxResult.OK)
                                        {
                                            if (pFreezBill == true)
                                            {
                                                PrintBill((((clsAddBillBizActionVO)arg.Result).Details).ID, ((MasterListItem)cmbPrintFormat.SelectedItem).ID);
                                            }
                                            UserControl rootPage = Application.Current.RootVisual as UserControl;
                                            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                                            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                                            mElement.Text = "";

                                            InitialiseForm();
                                            this.DataContext = new clsPatientVO();
                                            cmbGender.SelectedValue = ((clsPatientVO)this.DataContext).GenderID;
                                            dtpDOB.SelectedDate = null;
                                            txtYY.Text = "";
                                            txtDD.Text = "";
                                            txtMM.Text = "";
                                            txtMobileCountryCode.Text = "";
                                            txtContactNo.Text = "";

                                            txtPurchaseFrequency.Text = "";
                                            searchPatient = false;

                                            txtFirstName.IsEnabled = true;
                                            txtMiddleName.IsEnabled = true;
                                            txtLastName.IsEnabled = true;
                                            txtDelivery.IsEnabled = true;
                                            cmbGender.IsEnabled = true;
                                            dtpDOB.IsEnabled = true;
                                            txtMobileCountryCode.IsEnabled = true;
                                            txtMobileCountryCode.Text = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DefaultCountryCode;
                                            txtContactNo.IsEnabled = true;
                                            txtYY.IsEnabled = true;
                                            txtMM.IsEnabled = true;
                                            txtDD.IsEnabled = true;

                                            cmbPatientCategory.IsEnabled = true;
                                            cmbPatientSource.IsEnabled = true;
                                            cmbTariff.IsEnabled = true;
                                            cmbCompany.IsEnabled = true;
                                            cmbApplicabelPackage.ItemsSource = null;
                                            cmbApplicabelPackage.SelectedItem = null;   // added on 19072018 to reset selected package for next bill
                                            cmbApplicabelPackage.Visibility = Visibility.Collapsed;
                                            ApplicablePack.Visibility = Visibility.Collapsed;

                                            txtReasonForVariance.Text = "";
                                            txtReferenceDoctor.Text = "";
                                            Staff.IsChecked = false;

                                            ChkValidation();
                                            FillPatientType();
                                            FillPurchaseFrequencyUnit();

                                            LogInfoList = new List<LogInfo>();  // Reset the Activity Log List

                                        }
                                    };
                                msgW1.Show();

                            }
                        }

                    }
                    else
                    {

                        Indicatior.Close();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("NT4", "Error Occured While Adding Bill Details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                Indicatior.Close();
                ClickedFlag = 0;
                throw ex;
            }
            finally
            {
            }
        }

        //***Added By Ashish for Get Package Items on dated 100616
        private void cmdAddPackageItems_Click(object sender, RoutedEventArgs e)
        {
            PackageItemList win_PackageItemList = new PackageItemList();
            win_PackageItemList.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            win_PackageItemList.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
            win_PackageItemList.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID; //StoreID;
            win_PackageItemList.IsFromCounterSale = true;
            if (StoreID == 0)
                win_PackageItemList.AllowStoreSelection = true;
            else
                win_PackageItemList.AllowStoreSelection = false;
            if (cmbApplicabelPackage.SelectedItem != null && (cmbApplicabelPackage.SelectedItem as MasterListItem).ID > 0)
            {
                win_PackageItemList.PackageID = (cmbApplicabelPackage.SelectedItem as MasterListItem).ID;
                win_PackageItemList.PackageName = (cmbApplicabelPackage.SelectedItem as MasterListItem).Description;
            }
            if (cmbPatientCategory.SelectedItem != null && (cmbPatientCategory.SelectedItem as MasterListItem).ID > 0)
            {
                win_PackageItemList.PatientCatagoryL1 = ((MasterListItem)cmbPatientCategory.SelectedItem).ID;
            }
            if (cmbPatientSource.SelectedItem != null && (cmbPatientSource.SelectedItem as MasterListItem).ID > 0)
            {
                win_PackageItemList.PatientCatagoryL2 = ((MasterListItem)cmbPatientSource.SelectedItem).ID;
            }
            if (cmbTariff.SelectedItem != null && (cmbTariff.SelectedItem as MasterListItem).ID > 0)
            {
                win_PackageItemList.PatientCatagoryL3 = ((MasterListItem)cmbTariff.SelectedItem).ID;
            }
            if (cmbCompany.SelectedItem != null && (cmbCompany.SelectedItem as MasterListItem).ID > 0)
            {
                win_PackageItemList.CompanyID = ((MasterListItem)cmbCompany.SelectedItem).ID;
            }

            win_PackageItemList.PatientGenderID = ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID;
            win_PackageItemList.PatientDateOfBirth = Convert.ToDateTime(((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth);
            win_PackageItemList.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            win_PackageItemList.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
            win_PackageItemList.objUserVO = ((IApplicationConfiguration)App.Current).CurrentUser;
            win_PackageItemList.IsSellBySellingUnit = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsSellBySellingUnit;

            win_PackageItemList.OnSaveButton_Click += new RoutedEventHandler(PackageItemList_OnSaveButton_Click);
            win_PackageItemList.Show();
        }

        void PackageItemList_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            PackageItemList Itemswin = (PackageItemList)sender;
            if (Itemswin.DialogResult == true)
            {
                if (Itemswin.ItemList != null)
                {
                    StringBuilder strError = new StringBuilder();
                    StoreID = Itemswin.StoreID;
                    foreach (var item in Itemswin.ItemList)
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
                            //PharmacyItems = Itemswin.ItemList;

                            clsItemSalesDetailsVO ObjAddItem = new clsItemSalesDetailsVO();
                            ObjAddItem.ItemCode = item.ItemCode;
                            ObjAddItem.ItemID = item.ItemID;
                            ObjAddItem.ItemName = item.ItemName;
                            ObjAddItem.Manufacture = item.Manufacture;
                            ObjAddItem.PregnancyClass = item.PregnancyClass;
                            ObjAddItem.BatchID = item.BatchID;
                            ObjAddItem.BatchCode = item.BatchCode;
                            ObjAddItem.ExpiryDate = item.ExpiryDate;
                            ObjAddItem.Quantity = 1;
                            ObjAddItem.InclusiveOfTax = item.InclusiveOfTax;
                            //ObjAddItem.OriginalMRP = ObjAddItem.MRP;
                            //ObjAddItem.AvailableQuantity = item.AvailableQuantity;
                            //ObjAddItem.PurchaseRate = item.PurchaseRate;
                            //ObjAddItem.ConcessionPercentage = item.ConcessionPercentage;
                            //ObjAddItem.ConcessionAmount = ObjAddItem.ConcessionAmount;
                            //ObjAddItem.Amount = ObjAddItem.Amount;
                            //ObjAddItem.VATPercent = item.VATPercent;
                            vatper = item.VATPercent;
                            ObjAddItem.MRP = item.MRP;
                            ObjAddItem.OriginalMRP = ObjAddItem.MRP;
                            ObjAddItem.ItemVatType = item.ItemVatType;
                            ObjAddItem.AvailableQuantity = item.AvailableQuantity;
                            ObjAddItem.PurchaseRate = item.PurchaseRate;
                            ObjAddItem.ConcessionPercentage = item.DiscountPerc;//item.ConcessionPercentage;
                            ObjAddItem.ConcessionAmount = ObjAddItem.ConcessionAmount;
                            ObjAddItem.Amount = ObjAddItem.Amount;
                            ObjAddItem.VATPercent = item.VATPercent;
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
                            ObjAddItem.PUOMID = item.PUOMID;
                            ObjAddItem.SUOMID = item.SUOMID;
                            ObjAddItem.BaseUOMID = item.BaseUOMID;
                            ObjAddItem.BaseUOM = item.BaseUOM;
                            ObjAddItem.SellingUOMID = item.SellingUOMID;
                            ObjAddItem.SellingUOM = item.SellingUOM;
                            ObjAddItem.MainMRP = Convert.ToSingle(item.MRP);
                            ObjAddItem.MainRate = Convert.ToSingle(item.PurchaseRate);



                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsSellBySellingUnit == true)
                            {
                                ObjAddItem.SelectedUOM = new MasterListItem(ObjAddItem.SellingUOMID, ObjAddItem.SellingUOM);

                                //float CalculatedFromCF = item.SellingCF / item.StockingCF;

                                ObjAddItem.ConversionFactor = item.ConversionFactor; //CalculatedFromCF;
                                ObjAddItem.BaseConversionFactor = item.BaseConversionFactor; //item.SellingCF;

                                ObjAddItem.BaseQuantity = item.BaseQuantity;//1 * item.SellingCF;
                                ObjAddItem.MainRate = item.MainRate;//Convert.ToSingle(item.PurchaseRate) / Convert.ToSingle(item.StockingCF);
                                ObjAddItem.BaseRate = item.BaseRate; //Convert.ToSingle(item.PurchaseRate) / Convert.ToSingle(item.StockingCF);
                                ObjAddItem.PurchaseRate = item.PurchaseRate; //ObjAddItem.BaseRate * item.SellingCF;
                                //ObjAddItem.MainMRP = Convert.ToSingle(item.MRP) / Convert.ToSingle(item.StockingCF);
                                //ObjAddItem.BaseMRP = Convert.ToSingle(item.MRP) / Convert.ToSingle(item.StockingCF);
                                ObjAddItem.MRP = item.MRP; //ObjAddItem.BaseMRP * item.SellingCF;
                            }
                            else
                            {
                                ObjAddItem.SelectedUOM = new MasterListItem(0, "--Select--");
                            }

                            ObjAddItem.Budget = item.Budget;
                            ObjAddItem.DiscountPerc = item.DiscountPerc;
                            ObjAddItem.CalculatedBudget = item.CalculatedBudget;
                            ObjAddItem.IsPackageForItem = item.IsPackageForItem;
                            ObjAddItem.PackageID = item.PackageID;

                            PharmacyItems.Add(ObjAddItem);

                            //Log Write here   

                            // lineNumber = stackFrame.GetFileLineNumber();

                            objGUID = new Guid();
                            if (IsAuditTrail)
                            {
                                LogInformation = new LogInfo();
                                LogInformation.guid = objGUID;
                                LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                                LogInformation.TimeStamp = DateTime.Now;
                                LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                                LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                                LogInformation.Message = " XV : Line Number : " //+ Convert.ToString(lineNumber)
                                                                        + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                                        + " , StoreID : " + Convert.ToString(StoreID) + " "
                                                                        + " , ItemID : " + Convert.ToString(item.ItemID) + " "
                                                                        + " , BatchID : " + Convert.ToString(item.BatchID) + " "
                                                                        + " , BatchCode : " + Convert.ToString(item.BatchCode) + " "
                                                                        + " , ExpiryDate : " + Convert.ToString(item.ExpiryDate) + " "
                                                                        + " , Transaction UOMID : " + Convert.ToString(ObjAddItem.SelectedUOM.ID) + " "
                                                                        + " , Transaction Quantity (InputTransactionQuantity) : " + Convert.ToString(ObjAddItem.Quantity) + " "
                                                                        + " , BaseConversionFactor : " + Convert.ToString(ObjAddItem.BaseConversionFactor) + " "
                                                                        + " , Base Quantity (TransactionQuantity) : " + Convert.ToString(ObjAddItem.BaseQuantity) + " "
                                                                        + " , Base UOMID : " + Convert.ToString(ObjAddItem.BaseUOMID) + " "
                                                                        + " , SUOMID : " + Convert.ToString(ObjAddItem.SUOMID) + " "
                                                                        + " , StockCF : " + Convert.ToString(ObjAddItem.ConversionFactor) + " "
                                                                        + " , Stocking Quantity : " + Convert.ToString(ObjAddItem.Quantity * ObjAddItem.ConversionFactor) + " "
                                                                        + " , PUOMID : " + Convert.ToString(ObjAddItem.PUOMID) + " "
                                                                        + " , ConcessionPercentage : " + Convert.ToString(item.ConcessionPercentage) + " "
                                                                        + " , ConcessionAmount : " + Convert.ToString(item.ConcessionAmount) + " "
                                                                        + " , AvailableStockInBase : " + Convert.ToString(ObjAddItem.AvailableStockInBase) + "\r\n";


                                LogInfoList.Add(LogInformation);
                            }
                            //CallLogBizAction(LogBizAction);
                            ////

                        }
                    }
                    CalculatePharmacySummary();
                    dgPharmacyItems.Focus();
                    dgPharmacyItems.UpdateLayout();
                    dgPharmacyItems.SelectedIndex = PharmacyItems.Count - 1;

                    if (!string.IsNullOrEmpty(strError.ToString()))
                    {
                        string strMsg = "Items Already Added";
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                }
            }
        }
        //***

        /// <summary>
        /// Counter sale key up event help you to catch key control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanCounterSale_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Ctrl)
            {
                //if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                //{
                ItemListNew ItemSearch = new ItemListNew();
                ItemSearch.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                ItemSearch.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
                //By Anjali...........................
                ItemSearch.IsFromCounterSale = true;
                //.....................................

                ItemSearch.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID; //StoreID;
                if (StoreID == 0)
                    ItemSearch.AllowStoreSelection = true;
                else
                    ItemSearch.AllowStoreSelection = false;
                ItemSearch.OnSaveButton_Click += new RoutedEventHandler(ItemSearch_OnSaveButton_Click);
                ItemSearch.Show();
                ItemSearch.txtItemName.Focus();
                // }
            }

            if (e.Key == Key.Insert)
            {
                PatientSearch Win = new PatientSearch();
                //By Anjali............
                Win.isfromCouterSale = true;
                //......................
                Win.OnSaveButton_Click += new RoutedEventHandler(PatientSearch_OnSaveButton_Click);
                Win.Show();
            }
        }


        # region For Item Selection Control

        ItemSearchRowForCounterSale _ItemSearchRowControl = null;

        private void AttachItemSearchControl(long StoreID, long SupplierID)
        {

            //   BdrItemSearch.Visibility = Visibility.Visible;
            ItemSearchStackPanel.Children.Clear();
            _ItemSearchRowControl = new ItemSearchRowForCounterSale(StoreID, SupplierID);
            _ItemSearchRowControl.OnQuantityEnter_Click += new RoutedEventHandler(_ItemSearchRowControl_OnQuantityEnter_Click);
            // _ItemSearchRowControl.SetFocus();
            ItemSearchStackPanel.Children.Add(_ItemSearchRowControl);


        }

        void _ItemSearchRowControl_OnQuantityEnter_Click(object sender, RoutedEventArgs e)
        {
            isValid = ChkValidation();
            if (isValid == true)
            {
                ItemSearchRowForCounterSale _ItemSearchRowControl = (ItemSearchRowForCounterSale)sender;

                //if (Itemswin.DialogResult == true)
                //{
                //    if (Itemswin.ItemBatchList != null)
                //    {
                ////if (PharmacyItems != null && PharmacyItems.Count > 0 && PharmacyItems.Where(s => s.CompanyID != ((MasterListItem)cmbCompany.SelectedItem).ID || s.PatientSourceID != ((clsPatientSourceVO)cmbPatientSource.SelectedItem).ID || s.TariffID != ((MasterListItem)cmbTariff.SelectedItem).ID || s.PatientCategoryID != ((MasterListItem)cmbPatientCategory.SelectedItem).ID).Any())
                ////{
                ////      MessageBoxControl.MessageBoxChildWindow msgW2 =
                ////                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Multiple Sponsor Billing is not Allowed", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                ////        msgW2.Show();
                ////}
                ////else
                ////{


                StringBuilder strError = new StringBuilder();

                //StoreID = Itemswin.StoreID;

                //foreach (var item in Itemswin.ItemBatchList)
                //{
                bool Additem = true;
                if (PharmacyItems != null && PharmacyItems.Count > 0)
                {

                    var item1 = from r in PharmacyItems
                                where (r.BatchID == _ItemSearchRowControl.SelectedItems[0].BatchID)        // where (r.BatchID == item.BatchID)
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

                    ObjAddItem = _ItemSearchRowControl.SelectedItems[0];

                    #region Commented

                    //ObjAddItem.ItemCode = _ItemSearchRowControl.SelectedItems[0].ItemCode;              //item.ItemCode;
                    //ObjAddItem.ItemID = _ItemSearchRowControl.SelectedItems[0].ItemID;                  // item.ItemID;
                    //ObjAddItem.ItemName = _ItemSearchRowControl.SelectedItems[0].ItemName;              // item.ItemName;
                    //ObjAddItem.Manufacture = _ItemSearchRowControl.SelectedItems[0].Manufacture;        // item.Manufacturer;
                    //ObjAddItem.PregnancyClass = _ItemSearchRowControl.SelectedItems[0].PregnancyClass;  // item.PreganancyClass;
                    //ObjAddItem.BatchID = _ItemSearchRowControl.SelectedItems[0].BatchID;                // item.BatchID;
                    //ObjAddItem.BatchCode = _ItemSearchRowControl.SelectedItems[0].BatchCode;            // item.BatchCode;
                    //ObjAddItem.ExpiryDate = _ItemSearchRowControl.SelectedItems[0].ExpiryDate;          // item.ExpiryDate;
                    //ObjAddItem.Quantity = 1;
                    //ObjAddItem.InclusiveOfTax = _ItemSearchRowControl.SelectedItems[0].InclusiveOfTax;  // item.InclusiveOfTax;

                    //#region Commented II

                    ////if (item.InclusiveOfTax == false)
                    ////    ObjAddItem.MRP = (((item.MRP * item.TotalSalesTaxPercent) / 100) + item.MRP);
                    ////else
                    ////    ObjAddItem.MRP = item.MRP;


                    ////if (Itemswin.SelectedItems != null)                         // BY BHUSHAN . . . 
                    ////{
                    ////    if (Itemswin.SelectedItems[0].InclusiveOfTax == false)
                    ////        ObjAddItem.MRP = (((item.MRP * item.TotalSalesTaxPercent) / 100) + item.MRP);
                    ////    else
                    ////        ObjAddItem.MRP = item.MRP;
                    ////}
                    ////else
                    ////{                                                                        // BY BHUSHAN . . . 
                    ////    if (item.InclusiveOfTax == false)
                    ////        ObjAddItem.MRP = (((item.MRP * item.TotalSalesTaxPercent) / 100) + item.MRP);
                    ////    else
                    ////        ObjAddItem.MRP = item.MRP;
                    ////}

                    //#endregion

                    //ObjAddItem.OriginalMRP = _ItemSearchRowControl.SelectedItems[0].MRP;                                // ObjAddItem.MRP;
                    //ObjAddItem.AvailableQuantity = _ItemSearchRowControl.SelectedItems[0].AvailableStockInBase;         //item.AvailableStock;
                    //ObjAddItem.PurchaseRate = _ItemSearchRowControl.SelectedItems[0].PurchaseRate;                      // item.PurchaseRate;
                    ////ObjAddItem.ConcessionPercentage = _ItemSearchRowControl.SelectedItems[0].DiscountOnSale;            // item.DiscountOnSale;       // NA

                    //ObjAddItem.ConcessionAmount = _ItemSearchRowControl.SelectedItems[0].ConcessionAmount;              // ObjAddItem.ConcessionAmount;
                    //ObjAddItem.Amount = _ItemSearchRowControl.SelectedItems[0].Amount;                                  // ObjAddItem.Amount;
                    ////ObjAddItem.VATPercent = _ItemSearchRowControl.SelectedItems[0].VATPerc;                             // item.VATPerc;      // NA
                    ////vatper = _ItemSearchRowControl.SelectedItems[0].VATPerc;                                            // item.VATPerc;      // NA

                    ////by Anjali.............................
                    //ObjAddItem.MRP = _ItemSearchRowControl.SelectedItems[0].MRP;                                    // item.MRP;
                    //ObjAddItem.OriginalMRP = ObjAddItem.MRP;
                    //ObjAddItem.ItemVatType = _ItemSearchRowControl.SelectedItems[0].ItemVatType;                    // item.ItemVatType;
                    //ObjAddItem.AvailableQuantity = _ItemSearchRowControl.SelectedItems[0].AvailableStockInBase;     // item.AvailableStock;
                    //ObjAddItem.PurchaseRate = _ItemSearchRowControl.SelectedItems[0].PurchaseRate;                  // item.PurchaseRate;
                    ////ObjAddItem.ConcessionPercentage = _ItemSearchRowControl.SelectedItems[0].DiscountOnSale;        // item.DiscountOnSale;       // NA
                    //ObjAddItem.ConcessionAmount = _ItemSearchRowControl.SelectedItems[0].ConcessionAmount;          // ObjAddItem.ConcessionAmount;
                    //ObjAddItem.Amount = _ItemSearchRowControl.SelectedItems[0].Amount;                              // ObjAddItem.Amount;

                    //////ObjAddItem.VATPercent = item.VATPerc;
                    //////Updated by MMBABU
                    //////if (item.VATPerc > 0)
                    //////    ObjAddItem.VATPercent = item.VATPerc / 100;
                    //////else
                    //////ObjAddItem.VATPercent = item.VATPerc;

                    ////ObjAddItem.VATPercent = _ItemSearchRowControl.SelectedItems[0].TotalSalesTaxPercent;            // item.TotalSalesTaxPercent;     // NA

                    //////........................................
                    //////ObjAddItem.VATAmount = ObjAddItem.VATAmount;

                    //ObjAddItem.NetAmount = _ItemSearchRowControl.SelectedItems[0].NetAmount;                        // ObjAddItem.NetAmount;



                    ////By Anjali.................
                    //ObjAddItem.Shelfname = _ItemSearchRowControl.SelectedItems[0].Shelfname;                        // item.Shelfname;
                    //ObjAddItem.Containername = _ItemSearchRowControl.SelectedItems[0].Containername;                // item.Containername;
                    //ObjAddItem.Rackname = _ItemSearchRowControl.SelectedItems[0].Rackname;                          // item.Rackname;

                    //ObjAddItem.AvailableStockInBase = _ItemSearchRowControl.SelectedItems[0].AvailableStockInBase;  // item.AvailableStockInBase;

                    //ObjAddItem.StockUOM = _ItemSearchRowControl.SelectedItems[0].SUOM;                              // item.SUOM;
                    //ObjAddItem.PurchaseUOM = _ItemSearchRowControl.SelectedItems[0].PUOM;                           // item.PUOM;
                    //ObjAddItem.PUOM = _ItemSearchRowControl.SelectedItems[0].PUOM;                                  // item.PUOM;
                    //ObjAddItem.MainPUOM = _ItemSearchRowControl.SelectedItems[0].PUOM;                              // item.PUOM;
                    //ObjAddItem.SUOM = _ItemSearchRowControl.SelectedItems[0].SUOM;                                  // item.SUOM;

                    //ObjAddItem.ConversionFactor = Convert.ToSingle(_ItemSearchRowControl.SelectedItems[0].ConversionFactor);      // item.ConversionFactor
                    //ObjAddItem.PUOMID = _ItemSearchRowControl.SelectedItems[0].PUOMID;                                            // item.PUM;
                    //ObjAddItem.SUOMID = _ItemSearchRowControl.SelectedItems[0].SUOMID;                                            // item.SUM;
                    //ObjAddItem.BaseUOMID = _ItemSearchRowControl.SelectedItems[0].BaseUOMID;                                      // item.BaseUM;
                    //ObjAddItem.BaseUOM = _ItemSearchRowControl.SelectedItems[0].BaseUOM;                                          // item.BaseUMString;
                    //ObjAddItem.SellingUOMID = _ItemSearchRowControl.SelectedItems[0].SellingUOMID;                                // item.SellingUM;
                    //ObjAddItem.SellingUOM = _ItemSearchRowControl.SelectedItems[0].SellingUOM;                                    // item.SellingUMString;
                    //ObjAddItem.MainMRP = Convert.ToSingle(_ItemSearchRowControl.SelectedItems[0].MRP);                            // Convert.ToSingle(item.MRP);
                    //ObjAddItem.MainRate = Convert.ToSingle(_ItemSearchRowControl.SelectedItems[0].PurchaseRate);                  // Convert.ToSingle(item.PurchaseRate);

                    //if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsSellBySellingUnit == true)
                    //{
                    //    ObjAddItem.SelectedUOM = new MasterListItem(ObjAddItem.SellingUOMID, ObjAddItem.SellingUOM);

                    //    float CalculatedFromCF = 0; // NA    _ItemSearchRowControl.SelectedItems[0].SellingCF / _ItemSearchRowControl.SelectedItems[0].StockingCF;        // item.SellingCF / item.StockingCF;

                    //    ObjAddItem.ConversionFactor = CalculatedFromCF;
                    //    //ObjAddItem.BaseConversionFactor = _ItemSearchRowControl.SelectedItems[0].SellingCF;     // item.SellingCF;    // NA  

                    //    //ObjAddItem.BaseQuantity = 1 * _ItemSearchRowControl.SelectedItems[0].SellingCF;       // 1 * item.SellingCF;  // NA

                    //    ////ObjAddItem.MainRate = Convert.ToSingle(item.PurchaseRate);
                    //    ObjAddItem.MainRate = Convert.ToSingle(_ItemSearchRowControl.SelectedItems[0].PurchaseRate) / Convert.ToSingle(_ItemSearchRowControl.SelectedItems[0].StockCF);      // Convert.ToSingle(item.PurchaseRate) / Convert.ToSingle(item.StockingCF);
                    //    ////ObjAddItem.PurchaseRate = item.PurchaseRate * item.SellingCF;
                    //    ObjAddItem.BaseRate = Convert.ToSingle(_ItemSearchRowControl.SelectedItems[0].PurchaseRate) / Convert.ToSingle(_ItemSearchRowControl.SelectedItems[0].StockCF);      // Convert.ToSingle(item.PurchaseRate) / Convert.ToSingle(item.StockingCF);

                    //    //ObjAddItem.PurchaseRate = ObjAddItem.BaseRate * _ItemSearchRowControl.SelectedItems[0].SellingCF;     // ObjAddItem.BaseRate * item.SellingCF;        // NA

                    //    ////ObjAddItem.MainMRP = Convert.ToSingle(item.MRP);
                    //    ObjAddItem.MainMRP = Convert.ToSingle(_ItemSearchRowControl.SelectedItems[0].MRP) / Convert.ToSingle(_ItemSearchRowControl.SelectedItems[0].StockCF);    // Convert.ToSingle(item.MRP) / Convert.ToSingle(item.StockingCF);
                    //    ////ObjAddItem.MRP = item.MRP * item.SellingCF;
                    //    ObjAddItem.BaseMRP = Convert.ToSingle(_ItemSearchRowControl.SelectedItems[0].MRP) / Convert.ToSingle(_ItemSearchRowControl.SelectedItems[0].StockCF);    // Convert.ToSingle(item.MRP) / Convert.ToSingle(item.StockingCF);

                    //    //ObjAddItem.MRP = ObjAddItem.BaseMRP * _ItemSearchRowControl.SelectedItems[0].SellingCF;   // ObjAddItem.BaseMRP * item.SellingCF;     // NA

                    //}
                    //else
                    //{
                    //    ObjAddItem.SelectedUOM = new MasterListItem(0, "--Select--");
                    //}

                    ////..........................
                    ////For Multiple Sponser.........................
                    ////if (cmbCompany.SelectedItem != null)
                    ////    ObjAddItem.CompanyID = ((MasterListItem)cmbCompany.SelectedItem).ID;
                    ////if (cmbPatientSource.SelectedItem != null)
                    ////    ObjAddItem.PatientSourceID = ((clsPatientSourceVO)cmbPatientSource.SelectedItem).ID;
                    ////if (cmbTariff.SelectedItem != null)
                    ////   ObjAddItem.TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;
                    ////if (cmbPatientCategory.SelectedItem != null)
                    ////   ObjAddItem.PatientCategoryID = ((MasterListItem)cmbPatientCategory.SelectedItem).ID;
                    ////.............................................

                    ////Log Write here   

                    //// lineNumber = stackFrame.GetFileLineNumber();

                    #endregion

                    objGUID = new Guid();
                    if (IsAuditTrail)
                    {
                        LogInformation = new LogInfo();
                        LogInformation.guid = objGUID;
                        LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        LogInformation.TimeStamp = DateTime.Now;
                        LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                        LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                        LogInformation.Message = " XIX : New Item Added : " //+ Convert.ToString(lineNumber)
                                                                + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                                + " , StoreID : " + Convert.ToString(StoreID) + " "
                                                                + " , ItemID : " + Convert.ToString(_ItemSearchRowControl.SelectedItems[0].ItemID) + " "
                                                                + " , BatchID : " + Convert.ToString(_ItemSearchRowControl.SelectedItems[0].BatchID) + " "
                                                                + " , BatchCode : " + Convert.ToString(_ItemSearchRowControl.SelectedItems[0].BatchCode) + " "
                                                                + " , ExpiryDate : " + Convert.ToString(_ItemSearchRowControl.SelectedItems[0].ExpiryDate) + " "
                                                                + " , Transaction UOMID : " + Convert.ToString(ObjAddItem.SelectedUOM.ID) + " "
                                                                + " , Transaction Quantity (InputTransactionQuantity) : " + Convert.ToString(ObjAddItem.Quantity) + " "
                                                                + " , BaseConversionFactor : " + Convert.ToString(ObjAddItem.BaseConversionFactor) + " "  //item.SellingCF
                                                                + " , Base Quantity (TransactionQuantity) : " + Convert.ToString(ObjAddItem.BaseQuantity) + " "
                                                                + " , Base UOMID : " + Convert.ToString(ObjAddItem.BaseUOMID) + " "
                                                                + " , SUOMID : " + Convert.ToString(ObjAddItem.SUOMID) + " "
                                                                + " , StockCF : " + Convert.ToString(ObjAddItem.ConversionFactor) + " "
                                                                + " , Stocking Quantity : " + Convert.ToString(ObjAddItem.Quantity * ObjAddItem.ConversionFactor) + " "
                                                                + " , PUOMID : " + Convert.ToString(ObjAddItem.PUOMID) + " "
                                                                + " , ConcessionPercentage : " + Convert.ToString(ObjAddItem.ConcessionPercentage) + " "
                                                                + " , ConcessionAmount : " + Convert.ToString(ObjAddItem.ConcessionAmount) + " "
                                                                + " , AvailableStockInBase : " + Convert.ToString(ObjAddItem.AvailableStockInBase) + "\r\n";


                        LogInfoList.Add(LogInformation);
                    }

                    //CallLogBizAction(LogBizAction);
                    ////

                    //***//----
                    if (Staff.IsChecked == true)
                    {
                        ObjAddItem.ConcessionPercentage = ObjAddItem.StaffDiscount;
                    }
                    else if (txtMRNo.Text != "" && Staff.IsChecked == false)
                    {
                        ObjAddItem.ConcessionPercentage = ObjAddItem.RegisteredPatientsDiscount;
                    }
                    else
                    {
                        ObjAddItem.ConcessionPercentage = ObjAddItem.WalkinDiscount;
                    }


                    if (txtMRNo.Text != "")
                    {
                        ObjAddItem.SGSTtaxtype = 0;
                        ObjAddItem.SGSTapplicableon = 0;
                        ObjAddItem.CGSTtaxtype = 0;
                        ObjAddItem.CGSTapplicableon = 0;
                        ObjAddItem.IGSTtaxtype = 0;
                        ObjAddItem.IGSTapplicableon = 0;
                        ObjAddItem.SGSTPercent = 0;
                        ObjAddItem.CGSTPercent = 0;
                        ObjAddItem.IGSTPercent = 0;
                    }

                    //--------

                    if (cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)    // Package New Changes Added on 06072018
                    {
                        ObjAddItem.PackageBillID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageBillID;
                        ObjAddItem.PackageBillUnitID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageBillUnitID;
                        ObjAddItem.PackageID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID;                              // Package New Changes Added on 06072018
                    }

                    PharmacyItems.Add(ObjAddItem);

                }

                if (_ItemSearchRowControl != null && _ItemSearchRowControl.SelectedItems != null)
                {
                    _ItemSearchRowControl.SelectedItems[0] = null;
                    _ItemSearchRowControl.cmbItemName.SelectedItem = null;
                    _ItemSearchRowControl.cmbItemCode.SelectedItem = null;
                }

                //}   // end of : foreach (var item in Itemswin.ItemBatchList)

                CalculatePharmacySummary();
                // GetPackageItemDiscount();
                dgPharmacyItems.Focus();
                dgPharmacyItems.UpdateLayout();
                dgPharmacyItems.SelectedIndex = PharmacyItems.Count - 1;

                if (!string.IsNullOrEmpty(strError.ToString()))
                {
                    string strMsg = "Items Already Added";
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }

                //    }   // end of : if (Itemswin.ItemBatchList != null)

                ////}

                //}   // end of : if (Itemswin.DialogResult == true)

                _ItemSearchRowControl.cmbItemName.Focus();
            }
            else
            {
                string msgText = "Please add patient details";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWD.Show();
            }

        }

        #endregion

        #region Barcode
        private void txtBarCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (chkIsManualBarCodeEnter.IsChecked == true)
            {
                if (e.Key == Key.Enter && !string.IsNullOrEmpty(txtBarCode.Text))
                {
                    if (ChkValidation() == true)
                    {
                        if (cmbStore.SelectedItem == null || ((MasterListItem)cmbStore.SelectedItem).ID == 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow mgbx = null;
                            mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            mgbx.Show();
                            return;
                        }

                        if (!String.IsNullOrEmpty(txtBarCode.Text))
                        {

                            FillItemGrid();
                            //GetBarCodeItemBatches()
                            CalculatePharmacySummary();
                            txtBarCode.Focus();
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please add Patient details", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD.OnMessageBoxClosed += (arg) =>
                        {
                            if (arg == MessageBoxResult.OK)
                            {
                                txtBarCode.Text = "";
                                txtBarCode.Focus();
                            }
                        };
                        msgWD.Show();
                    }
                }
            }
        }

        private void txtBarCode_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (chkIsManualBarCodeEnter.IsChecked == false && !string.IsNullOrEmpty(txtBarCode.Text) && Convert.ToString(txtBarCode.Text).Length == 5)
            {
                if (ChkValidation() == true)
                {
                    if (cmbStore.SelectedItem == null || ((MasterListItem)cmbStore.SelectedItem).ID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = null;
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();
                        return;
                    }

                    if (!String.IsNullOrEmpty(txtBarCode.Text))
                    {
                        FillItemGrid();
                        CalculatePharmacySummary();
                        txtBarCode.Focus();
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please add Patient details", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD.OnMessageBoxClosed += (arg) =>
                    {
                        if (arg == MessageBoxResult.OK)
                        {
                            txtBarCode.Text = "";
                            txtBarCode.Focus();
                        }
                    };
                    msgWD.Show();
                }
            }

        }

        private void CmdSearchBarcode_Click(object sender, RoutedEventArgs e)
        {
            if (cmbStore.SelectedItem == null || ((MasterListItem)cmbStore.SelectedItem).ID == 0)
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = null;
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
                return;
            }

            if (!String.IsNullOrEmpty(txtBarCode.Text))
            {
                FillItemGrid();
                CalculatePharmacySummary();
                txtBarCode.Focus();
            }
        }

        Int64 RItemId;
        public void FillItemGrid()
        {
            clsCounterSaleBarCodeBizActionVO BizActionObj = new clsCounterSaleBarCodeBizActionVO();
            //BizActionObj.IssueList = new List<clsItemSalesDetailsVO>();
            BizActionObj.ItemBatchListForBarCode = new List<clsItembatchSearchVO>();
            WaitIndicator w = new WaitIndicator();
            w.Show();
            try
            {

                BizActionObj.BarCode = txtBarCode.Text;
                BizActionObj.StoreId = ((MasterListItem)(cmbStore.SelectedItem)).ID;
                BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        StringBuilder strError = new StringBuilder();
                        BizActionObj = (clsCounterSaleBarCodeBizActionVO)e.Result;

                        BizActionObj.ItemBatchListForBarCode = ((clsCounterSaleBarCodeBizActionVO)e.Result).ItemBatchListForBarCode;

                        if (BizActionObj.ItemBatchListForBarCode.Count == 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Batch is not Available for this Item.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.OnMessageBoxClosed += (arg) =>
                            {
                                if (arg == MessageBoxResult.OK)
                                {
                                    txtBarCode.Text = "";
                                    txtBarCode.Focus();
                                }
                            };
                            msgW1.Show();
                            return;
                        }
                        if (BizActionObj.ItemBatchListForBarCode.Where(z => z.AvailableStock <= 0).Any())
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Stock is not Available for this Batch Item.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.OnMessageBoxClosed += (arg) =>
                            {
                                if (arg == MessageBoxResult.OK)
                                {
                                    txtBarCode.Text = "";
                                    txtBarCode.Focus();
                                }
                            };
                            msgW1.Show();
                            return;
                        }

                        foreach (var item in BizActionObj.ItemBatchListForBarCode)
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
                                                ItemName = r.ItemName,
                                                BatchCode = r.BatchCode
                                            };

                                if (item1.ToList().Count > 0)
                                {
                                    if (strError.ToString().Length > 0)
                                        strError.Append(",");
                                    strError.Append(item1.ToList()[0].ItemName + " with Batchcode " + item1.ToList()[0].BatchCode);
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


                                if (cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)    // Package New Changes Added on 26042018
                                {
                                    ObjAddItem.PackageBillID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageBillID;
                                    ObjAddItem.PackageBillUnitID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageBillUnitID;
                                    ObjAddItem.PackageID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID;                              // Package New Changes Added on 03052018
                                }
                                ObjAddItem.ConcessionAmount = ObjAddItem.ConcessionAmount;

                                ObjAddItem.Amount = ObjAddItem.Amount;
                                ObjAddItem.VATPercent = 0;//item.VATPerc;
                                vatper = item.VATPerc;
                                if (txtMRNo.Text == "")
                                {
                                    ObjAddItem.SGSTtaxtype = item.SGSTtaxtypeSale;              //item.SGSTtaxtype;
                                    ObjAddItem.SGSTapplicableon = item.SGSTapplicableonSale;    //item.SGSTapplicableon;
                                    ObjAddItem.CGSTtaxtype = item.CGSTtaxtypeSale;              //item.CGSTtaxtype;
                                    ObjAddItem.CGSTapplicableon = item.CGSTapplicableonSale;    //item.CGSTapplicableon;
                                    ObjAddItem.IGSTtaxtype = item.IGSTtaxtypeSale;              //item.IGSTtaxtype;
                                    ObjAddItem.IGSTapplicableon = item.IGSTapplicableonSale;    //item.IGSTapplicableon;

                                    //ObjAddItem.SGSTPercent = Convert.ToDouble(item.SGSTPercent) == 0 ? Convert.ToDouble(item.IGSTPercent) != 0 ? Convert.ToDouble(item.IGSTPercent) / 2 : 0 : Convert.ToDouble(item.SGSTPercent);
                                    //ObjAddItem.CGSTPercent = Convert.ToDouble(item.CGSTPercent) == 0 ? Convert.ToDouble(item.CGSTPercent) != 0 ? Convert.ToDouble(item.CGSTPercent) / 2 : 0 : Convert.ToDouble(item.CGSTPercent);

                                    ObjAddItem.SGSTPercent = Convert.ToDouble(item.SGSTPercentSale) == 0 ? Convert.ToDouble(item.IGSTPercentSale) != 0 ? Convert.ToDouble(item.IGSTPercentSale) / 2 : 0 : Convert.ToDouble(item.SGSTPercentSale);
                                    ObjAddItem.CGSTPercent = Convert.ToDouble(item.CGSTPercentSale) == 0 ? Convert.ToDouble(item.IGSTPercentSale) != 0 ? Convert.ToDouble(item.IGSTPercentSale) / 2 : 0 : Convert.ToDouble(item.CGSTPercentSale);

                                    ObjAddItem.IGSTPercent = 0;
                                }
                                //by Anjali.............................
                                ObjAddItem.MRP = item.MRP;
                                ObjAddItem.OriginalMRP = ObjAddItem.MRP;
                                ObjAddItem.ItemVatType = item.ItemVatType;
                                ObjAddItem.AvailableQuantity = item.AvailableStock;
                                ObjAddItem.PurchaseRate = item.PurchaseRate;

                                ObjAddItem.ConcessionPercentage = item.DiscountOnSale;
                                ObjAddItem.ConcessionAmount = ObjAddItem.ConcessionAmount;
                                //***//----
                                if (Staff.IsChecked == true)
                                {
                                    ObjAddItem.ConcessionPercentage = item.StaffDiscount;
                                }
                                else if (txtMRNo.Text != "" && Staff.IsChecked == false)
                                {
                                    ObjAddItem.ConcessionPercentage = item.RegisteredPatientsDiscount;
                                }
                                else
                                {
                                    ObjAddItem.ConcessionPercentage = item.WalkinDiscount;
                                }

                                //--------

                                ObjAddItem.Amount = ObjAddItem.Amount;
                                ObjAddItem.NetAmount = ObjAddItem.NetAmount;
                                //By Anjali.................
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
                                objGUID = new Guid();

                                PharmacyItems.Add(ObjAddItem);
                            }
                        }

                        CalculatePharmacySummary();
                        dgPharmacyItems.Focus();
                        dgPharmacyItems.UpdateLayout();
                        dgPharmacyItems.SelectedIndex = PharmacyItems.Count - 1;
                        txtBarCode.Text = "";
                        txtBarCode.Focus();
                        if (chkIsManualBarCodeEnter.IsChecked == true) chkIsManualBarCodeEnter.IsChecked = false;
                        //if (BizActionObj.ItemBatchListForBarCode.Count == 0)
                        //{

                        //    string strMsg = "Item Not found.! Do You Want To Add Items Manually?:" + strError;
                        //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //               new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        //    msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(mgbxnew_OnMessageBoxClosed);
                        //    msgW1.Show();

                        //}

                        if (!string.IsNullOrEmpty(strError.ToString()))
                        {
                            string strMsg = "Item already added :" + strError;
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.OnMessageBoxClosed += (arg) =>
                            {
                                if (arg == MessageBoxResult.OK)
                                {
                                    txtBarCode.Text = "";
                                    txtBarCode.Focus();
                                }
                            };
                            msgW1.Show();
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


        #endregion



    }



}


























