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


namespace CIMS.Forms
{
    public partial class frmIPDBill : UserControl, IInitiateCIMS
    {
      
        bool flagFreezFromSearch = false;
        
        #region "Paging"


        public PagedSortableCollectionView<clsBillVO> DataList { get; private set; }

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
        private void FillBillSearchList()
        {
            //checkFreezColumn = false;
            indicator.Show();

            clsGetBillSearchListBizActionVO BizAction = new clsGetBillSearchListBizActionVO();
            //BizAction.IsActive = true;
            if (dtpFromDate.SelectedDate != null)
            BizAction.FromDate = dtpFromDate.SelectedDate.Value.Date;
            if (dtpToDate.SelectedDate != null)
            BizAction.ToDate = dtpToDate.SelectedDate.Value.Date;
            
            BizAction.PatientID = PatientID;
            BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;


            //BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
            {
                BizAction.UnitID = BizAction.PatientUnitID.Value;//((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            else
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

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
                        }

                         dgBillList.ItemsSource = null;
                         dgBillList.ItemsSource = DataList;

                         dgDataPager.Source = null;
                         dgDataPager.PageSize = BizAction.MaximumRows;
                         dgDataPager.Source = DataList;

                        // checkFreezColumn = true;
                     }
                    
                   
                }
                indicator.Close();
            };
            
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

       

        #endregion

        bool IsPatientExist = false;
        long PatientID = 0;
        long BillID = 0;
        long UnitID = 0;
              
        PalashDynamics.Animations.SwivelAnimation _flip = null;
        public ObservableCollection<clsItemSalesDetailsVO> PharmacyItems { get; set; } 

        #region IInitiateCIMS Members
               
        public void Initiate(string Mode)
        {
            //throw new NotImplementedException();

            switch (Mode.ToUpper())
            {
                case "NEW":
                    
                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        return;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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
                      
                    //this.DataContext = new clsAdvanceVO() { PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, LinkServer = ((IApplicationConfiguration)App.Current).SelectedPatient.LinkServer, Date = DateTime.Now };

                    break;
               
                default:

                    break;
            }

        }

        #endregion
        
        bool IsPageLoaded = false;
        clsBillVO SelectedBill { get; set; }
        clsPatientVO myPatient { get; set; }
        bool IsEditMode = false;
        
        private void GetPatientDetails()
        {
            clsGetPatientBizActionVO BizAction1 = new clsGetPatientBizActionVO();
            BizAction1.PatientDetails = new clsPatientVO();
            BizAction1.PatientDetails.GeneralDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction1.PatientDetails.GeneralDetails.UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            //BizAction1.PatientDetails.GeneralDetails.LinkServer = ((IApplicationConfiguration)App.Current).SelectedPatient.LinkServer;
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
                    //HtmlPage.Window.Alert("Error occured while adding patient.");
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            };

            Client1.ProcessAsync(BizAction1, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client1.CloseAsync();
        }

        private void CheckVisit()
        {
           
            WaitIndicator ind = new WaitIndicator();
            ind.Show();

            clsGetVisitBizActionVO BizAction = new clsGetVisitBizActionVO();
            //BizAction.IsActive = true;
            BizAction.Details.PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.Details.PatientUnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            BizAction.GetLatestVisit = true;

            //BizAction.Details.VisitId = ((clsVisitVO)dgEncounterList.SelectedItem).VisitId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetVisitBizActionVO)arg.Result).Details != null && ((clsGetVisitBizActionVO)arg.Result).Details.ID > 0 && ((clsGetVisitBizActionVO)arg.Result).Details.VisitStatus == true)
                    {

                        InitialiseForm();
                        SetCommandButtonState("ClickNew");
                        //InitialiseForm();
                        GetPatientDetails();
                        GetVisitDetails();
                        if (((IApplicationConfiguration)App.Current).SelectedPatient.VisitID > 0)
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

                            FillChargeList(0 ,lUnitID, false, ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID);

                        }

                        _flip.Invoke(RotationType.Forward);
                        
                        ind.Close();

                    }
                    else
                    {
                        ind.Close();
                       // System.Windows.Browser.HtmlPage.Window.Alert("Visit is not marked for the Patient");
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
               
        ObservableCollection<clsChargeVO> ChargeList { get; set; }
        
        public frmIPDBill()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmBill_Loaded);
            _flip = new PalashDynamics.Animations.SwivelAnimation(LayoutRoot1, LayoutRoot2, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            dtpFromDate.SelectedDate = DateTime.Now.Date;
            dtpToDate.SelectedDate = DateTime.Now.Date;

            dgCharges.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgCharges_CellEditEnded);
            dgPharmacyItems.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgPharmacyItems_CellEditEnded);

            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsBillVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;
            //======================================================
        }

        void dgPharmacyItems_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
           
            if (e.Column.DisplayIndex == 6)
            {
                if (((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity > ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableQuantity)
                {
                    double availQty = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableQuantity;

                    ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity = availQty;
                     string msgTitle = "";
                     string msgText = "Quantity must be less than or equal to Available Quantity " + availQty;

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
                    string msgTitle = "";
                    string msgText = "Percentage should not be greater than 100";

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
                    string msgTitle = "";
                    string msgText = "Percentage should not be greater than 100";

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

                    string msgTitle = "";
                    string msgText = "Concession amount should not be greater than Amount " + ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Amount;

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

                    string msgTitle = "";
                    string msgText = "MRP must be greater than or equal to Purchase Rate :" + PurchaseRate;

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD.Show();


                }
            }

            

            CalculatePharmacySummary();
        }

        void dgCharges_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            //throw new NotImplementedException();

            if (e.Column.DisplayIndex == 3)
            {
                if (dgCharges.SelectedItem != null)
                {
                    if (((clsChargeVO)dgCharges.SelectedItem).RateEditable)
                    {
                        if ((((clsChargeVO)dgCharges.SelectedItem).Rate < ((clsChargeVO)dgCharges.SelectedItem).MinRate)
                            || (((clsChargeVO)dgCharges.SelectedItem).Rate > ((clsChargeVO)dgCharges.SelectedItem).MaxRate))
                        {
                            ((clsChargeVO)dgCharges.SelectedItem).Rate = ((clsChargeVO)dgCharges.SelectedItem).MaxRate;

                            string msgTitle = "";
                            string msgText = "Rate must be in between  " + ((clsChargeVO)dgCharges.SelectedItem).MinRate + " and " + ((clsChargeVO)dgCharges.SelectedItem).MaxRate;

                            MessageBoxControl.MessageBoxChildWindow msgWD =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWD.Show();

                        }
                    }
                }
            }
            else if (e.Column.DisplayIndex == 5 || e.Column.DisplayIndex == 6)
            {
                 if (dgCharges.SelectedItem != null)
                 {
                     if (((clsChargeVO)dgCharges.SelectedItem).Concession > ((clsChargeVO)dgCharges.SelectedItem).TotalAmount)
                     {
                         ((clsChargeVO)dgCharges.SelectedItem).Concession = ((clsChargeVO)dgCharges.SelectedItem).TotalAmount;

                         string msgTitle = "";
                         string msgText = "Concession amount should not be greater than Total Amount " + ((clsChargeVO)dgCharges.SelectedItem).TotalAmount;

                         MessageBoxControl.MessageBoxChildWindow msgWD =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                         msgWD.Show();
                     }
                 }
            }
            else if (e.Column.DisplayIndex == 4)//Quantity
            {
                if (dgCharges.SelectedItem != null)
                {
                    if (!((clsChargeVO)dgCharges.SelectedItem).Quantity.ToString().IsNumberValid())
                    {
                        string msgText = "Decimal value not allowed in Quantity field ";

                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD.Show();

                        ((clsChargeVO)dgCharges.SelectedItem).Quantity = Math.Ceiling(((clsChargeVO)dgCharges.SelectedItem).Quantity);
                    }
                  
                }
            }
            

            if (e.Column.DisplayIndex == 3 | e.Column.DisplayIndex == 4 | e.Column.DisplayIndex == 5 | e.Column.DisplayIndex == 6)         
                CalculateClinicalSummary();
          
        }

        void frmBill_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoaded)
            {
                if (IsPatientExist == false)
                {
                    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                    ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
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
                    FillBillSearchList();
                }
                IsPageLoaded = true;
            
            }
        }
        
        private void FillChargeList(long PBillID, long pUnitID, bool pIsBilled,long pVisitID)
        {
            try
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();

                clsGetChargeListBizActionVO BizAction = new clsGetChargeListBizActionVO();

                BizAction.ID = 0;
                
                BizAction.Opd_Ipd_External_Id = pVisitID;
                BizAction.Opd_Ipd_External_UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                BizAction.IsBilled = pIsBilled;
                BizAction.BillID = PBillID;
                BizAction.UnitID = pUnitID; // ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {

                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsGetChargeListBizActionVO)arg.Result).List != null)
                        {
                            List<clsChargeVO> objList;// = new List<clsChargeVO>();
                            objList = ((clsGetChargeListBizActionVO)arg.Result).List;

                            foreach (var item in objList)
                            {
                                ChargeList.Add(item);
                            }
                            dgCharges.Focus();
                            dgCharges.UpdateLayout();
                            CalculateClinicalSummary();
                            if(SelectedBill != null) SelectedBill.ChargeDetails = objList;
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

        private void FillPharmacyItemsList(long pBillID,long pBillUnitID,  bool pIsBilled)
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

                            //List<clsItemSalesDetailsVO> objList;// = new List<clsChargeVO>();
                            //objList = object.;

                            foreach (var item in mobj.Items )
                            {
                                PharmacyItems.Add(item);
                            }

                            dgPharmacyItems.Focus();
                            dgPharmacyItems.UpdateLayout();
                            CalculatePharmacySummary();
                            if (SelectedBill != null)  SelectedBill.PharmacyItems = mobj;
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
        public long VisitDoctorID { get; set; }
        long CurrentVisitTypeID = 0;

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
            //BizAction.IsActive = true;
            BizAction.Details.ID = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;
           // BizAction.GetLatestVisit = true;

            //BizAction.Details.VisitId = ((clsVisitVO)dgEncounterList.SelectedItem).VisitId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    //this.DataContext = ((clsGetVisitBizActionVO)arg.Result).Details;
                    //VisitWindow.Title = "Encounter - " + ((clsPatientVO)this.DataContext).GeneralDetails.FirstName + " " +
                    //                    ((clsPatientVO)this.DataContext).GeneralDetails.MiddleName + " " + ((clsPatientVO)this.DataContext).GeneralDetails.LastName;


                    if (((clsGetVisitBizActionVO)arg.Result).Details != null )
                    {
                        VisitDoctorID = ((clsGetVisitBizActionVO)arg.Result).Details.DoctorID;
                        CurrentVisitTypeID = ((clsGetVisitBizActionVO)arg.Result).Details.VisitTypeID;
                        CheckVisitType(CurrentVisitTypeID);

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

        private bool CheckValidations()
        {
            bool isValid = true;

            if (txtNetAmount.Text.Trim() == "")
            {
                isValid = false;
                string msgTitle = "";
                string msgText = "You can not save the Bill with Zero amount";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgWD.Show();
            }
            else if (Convert.ToDouble(txtNetAmount.Text) <= 0)
            {
              
                isValid = false;
                string msgTitle = "";
                string msgText = "You can not save the Bill with Zero amount";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgWD.Show();

            }

            foreach (var item in PharmacyItems)
            {
                if (item.Quantity > item.AvailableQuantity)
                {
                    isValid = false;
                    string msgTitle = "";
                    string msgText = "Available Quantity for " + item.ItemName + " is less than Specified Quantity. Specified Quantiry is " + item.Quantity.ToString() + " And Available Quantity is " + item.AvailableQuantity.ToString();

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgWD.Show();

                    break;
                }

                if (item.Quantity <= 0)
                {
                    isValid = false;
                  
                    string msgText = "Can not save item "+ item.ItemName + " with zero Quantity " ;

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgWD.Show();

                    break;
                }

                if (item.MRP <= 0)
                {
                    isValid = false;

                    string msgText = "Can not save item " + item.ItemName + " with zero MRP ";

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgWD.Show();

                    break;
                }
            }

      
                

            return isValid;
        }

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
                    string msgTitle = "";
                    string msgText = "Are you sure you want to Freeze the Bill ?";

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (arg) =>
                    {
                        if (arg == MessageBoxResult.Yes)
                        {
                            PaymentWindow paymentWin = new PaymentWindow();
                            paymentWin.TotalAmount = double.Parse(txtPayAmount.Text);
                            paymentWin.Initiate("Bill");

                            paymentWin.txtPayTotalAmount.Text = this.txtNetAmount.Text;
                            paymentWin.txtDiscAmt.Text = this.txtTotalConcession.Text;
                            paymentWin.txtPayableAmt.Text = this.txtPayAmount.Text;

                            if (rdbAgainstBill.IsChecked == true)
                                paymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstBill;
                            else
                                paymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstServices;

                            paymentWin.OnSaveButton_Click += new RoutedEventHandler(paymentWin_OnSaveButton_Click);
                            paymentWin.Show();
                        }
                        else
                            cmdSave.IsEnabled = true;
                    };
                    msgWD.Show();


                }
                else
                {
                    SaveBill(null, false);
                }
            }
            else
                cmdSave.IsEnabled = true;
        }
        
        private void SaveBill(clsPaymentVO pPayDetails, bool pFreezBill)
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            try
            {
                clsBillVO objBill = new clsBillVO();

               // if (IsEditMode==true)  objBill = SelectedBill;

                if (SelectedBill == null)
                {
                    objBill.Date = DateTime.Now;
                    objBill.Time = DateTime.Now;
                    IsEditMode = false;
                    objBill.Opd_Ipd_External_Id = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;
                    objBill.Opd_Ipd_External_UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;

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
                        objBill.BalanceAmountSelf = pPayDetails.BillBalanceAmount;
                        objBill.BillPaymentType = pPayDetails.BillPaymentType;
                    }
                    
                    if (!string.IsNullOrEmpty(txtPayAmount.Text))
                        objBill.SelfAmount = Convert.ToDouble(txtPayAmount.Text);

                }

                objBill.IsFreezed = pFreezBill;
                objBill.PaymentDetails = pPayDetails;
                 
                               
                if (!string.IsNullOrEmpty(txtTotalBill.Text))
                    objBill.TotalBillAmount = Convert.ToDouble(txtTotalBill.Text);

              

                if (!string.IsNullOrEmpty(txtTotalConcession.Text))
                    objBill.TotalConcessionAmount = Convert.ToDouble(txtTotalConcession.Text);

                if (!string.IsNullOrEmpty(txtNetAmount.Text))
                    objBill.NetBillAmount = Convert.ToDouble(txtNetAmount.Text);

                if (ChargeList != null & ChargeList.Count > 0)
                {
                    objBill.ChargeDetails = ChargeList.ToList();

                    if (pFreezBill)
                    {
                        for (int i = 0; i < ChargeList.Count; i++)
                        {
                            clsChargeVO objCharge = new clsChargeVO();
                            objCharge = ChargeList[i];

                            if (objCharge.ServiceSpecilizationID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathoSpecializationID)
                            {

                                objBill.PathoWorkOrder = new clsPathOrderBookingVO();
                                objBill.PathoWorkOrder.TariffServiceID = objCharge.TariffServiceId;
                                objBill.PathoWorkOrder.DoctorID = VisitDoctorID;
                                objBill.PathoWorkOrder.ServiceID = objCharge.ServiceId;
                                
                                //objBill.PathoWorkOrder.Items.Add(new clsPathOrderBookingDetailVO()
                                //{
                                //    //TariffServiceID = objCharge.TariffServiceId,
                                //    ServiceID = objCharge.ServiceId,
                                //    //DoctorID = VisitDoctorID,
                                //    //TestCharges = objCharge.Rate,
                                //    IsSampleCollected = false
                                //});
                            }
                            else if (objCharge.ServiceSpecilizationID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RadiologySpecializationID)
                            {
                                
                                objBill.RadiologyWorkOrder.OrderBookingDetails.Add(new clsRadOrderBookingDetailsVO()
                                {
                                    TariffServiceID = objCharge.TariffServiceId,
                                    ServiceID = objCharge.ServiceId,
                                    
                                    DoctorID = VisitDoctorID,
                                    IsApproved = false
                                });
                            }
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

                clsAddBillBizActionVO BizAction = new clsAddBillBizActionVO();
                BizAction.Details = new clsBillVO();
                BizAction.Details = objBill;

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

                                Indicatior.Close();
                                //cmdSave.IsEnabled = false;
                                //lnkAddItems.IsEnabled = false;
                                //lnkAddService.IsEnabled = false;
                                SetCommandButtonState("Save");
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Bill details saved successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                
                                msgW1.OnMessageBoxClosed += (re) =>
                                    {
                                        if (re == MessageBoxResult.OK)
                                        {
                                            if (pFreezBill == true)
                                            {
                                                //if(CurrentVisitTypeID !=((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyVisitTypeID)

                                                    if ((((clsAddBillBizActionVO)arg.Result).Details).BillType == PalashDynamics.ValueObjects.BillTypes.Clinical)
                                                    {
                                                        PrintBill(BillID, UnitID);
                                                    }
                                                    else if ((((clsAddBillBizActionVO)arg.Result).Details).BillType == PalashDynamics.ValueObjects.BillTypes.Pharmacy)
                                                    {
                                                        PrintPharmacyBill(BillID, UnitID);
                                                    }
                                                    else
                                                    {
                                                        PrintBill(BillID, UnitID);
                                                        PrintPharmacyBill(BillID, UnitID);
                                                        
                                                    }

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
                        //System.Windows.Browser.HtmlPage.Window.Alert("Error occured while adding visit details.");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("NT5", "Error occured while adding Bill details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        cmdSave.IsEnabled = true;
                    }

                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
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

        WaitIndicator Indicatior = null;
        void paymentWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (((CIMS.Forms.PaymentWindow)sender).DialogResult == true)
            {
                if (((PaymentWindow)sender).Payment != null)
                {
                    rdbAgainstBill.IsChecked = true;
                    SaveBill(((PaymentWindow)sender).Payment, true);
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

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            txtNetAmount.Text = "0";
            txtClinicalNetAmount.Text = "0";
            txtPharmacyNetAmount.Text = "0";
            InitialiseForm();
            SetCommandButtonState("New");
           
            _flip.Invoke(RotationType.Backward);

        }

        private void lnkAddService_Click(object sender, RoutedEventArgs e)
        {
            ServiceSearch serviceSearch = null;
            serviceSearch = new ServiceSearch();
            //serviceSearch.PatientSourceID = myPatient.
            serviceSearch.OnAddButton_Click += new RoutedEventHandler(serviceSearch_OnAddButton_Click);
            serviceSearch.Show();
        }

        void serviceSearch_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
           // throw new NotImplementedException();          

            if (((ServiceSearch)sender).DialogResult == true)
            {
                List<clsServiceMasterVO> lServices = new List<clsServiceMasterVO>();
                lServices = ((ServiceSearch)sender).SelectedServices;

                AddCharges(lServices);
            
            }
        }
             
        private void CalculateClinicalSummary()
        {
                double clinicalTotal, clinicalConcession, clinicalNetAmount;
                clinicalTotal = clinicalConcession = clinicalNetAmount = 0;

                for (int i = 0; i < ChargeList.Count ; i++)
                {                   
                    clinicalTotal += (ChargeList[i].TotalAmount);
                    clinicalConcession += ChargeList[i].Concession;
                     clinicalNetAmount += ChargeList[i].NetAmount;
                }
                //clinicalNetAmount = clinicalNetAmount;
                //dgCharges.ItemsSource = null;
                //dgCharges.ItemsSource = ChargeList;

                txtClinicalTotal.Text = String.Format("{0:0.00}",clinicalTotal);
                txtClinicalConcession.Text =String.Format("{0:0.00}", clinicalConcession);
                txtClinicalNetAmount.Text = String.Format("{0:0.00}",clinicalNetAmount);

                CalculateTotalSummary();
        }

        private void CalculatePharmacySummary()
        {

            double Total, Concession, NetAmount,TotalVat;
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
            NetAmount = (Total - Concession)+TotalVat;

          
            txtPharmacyTotal.Text =String.Format("{0:0.00}", Total);
            txtPharmacyConcession.Text =String.Format("{0:0.00}", Concession);
            txtPharmacyVatAmount.Text =String.Format("{0:0.00}", TotalVat);
            txtPharmacyNetAmount.Text = String.Format("{0:0.00}",NetAmount);

            CalculateTotalSummary();
        }

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
                              select r ;

                double? lPayAmount = results.Sum(cnt => cnt.NetAmount);

                if (lPayAmount.HasValue)
                    txtPayAmount.Text = String.Format("{0:0.00}", lPayAmount);
                else
                    txtPayAmount.Text = "0.00";
            }      
           
        }

        private void cmdDeleteCharges_Click(object sender, RoutedEventArgs e)
        {
            if (dgCharges.SelectedItem != null)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Delete the selected Service charge ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        ChargeList.RemoveAt(dgCharges.SelectedIndex);
                        CalculateClinicalSummary();
                    }
                };

                msgWD.Show();
            }
        }

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
        void ItemSearch_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ItemListNew Itemswin = (ItemListNew)sender;

            if (Itemswin.DialogResult == true)
            {
                if (Itemswin.ItemBatchList != null )
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
                                if(strError.ToString().Length>0)
                                strError.Append(",");
                                strError.Append(item1.ToList()[0].ItemName);
                          
                                Additem = false;

                            }
                        }

                        if (Additem)
                        {
                            clsItemSalesDetailsVO ObjAddItem = new clsItemSalesDetailsVO();

                            ObjAddItem.ItemID =item.ItemID;
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
                        string strMsg = "Items already added : " + strError.ToString();

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                }
            }
        }

        private void cmdDeletePharmacyItems_Click(object sender, RoutedEventArgs e)
        {
            if (dgPharmacyItems.SelectedItem != null)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Delete the selected Item ?";

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

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            SelectedBill = null;
            IsEditMode = false;
            rdbAgainstBill.IsEnabled = false;
            rdbAgainstBill.IsChecked = true;
            rdbAgainstServices.IsEnabled = false;
            CheckVisit();
           
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            bool res = true;

            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            {
                if (dtpFromDate.SelectedDate > dtpToDate.SelectedDate)
                {
                    dtpFromDate.SetValidation("From Date should be less than To Date");
                    dtpFromDate.RaiseValidationError();
                   
                    string strMsg = "From Date should be less than To Date";

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

        private void InitialiseForm()
        {
            tabBillingSubInfo.SelectedIndex = 0;
            IsEditMode = false;
            PharmacyItems = new ObservableCollection<clsItemSalesDetailsVO>();
            dgPharmacyItems.ItemsSource = PharmacyItems;
            dgPharmacyItems.UpdateLayout();

            ChargeList = new ObservableCollection<clsChargeVO>();
            dgCharges.ItemsSource = ChargeList;
            dgCharges.Focus();
            dgCharges.UpdateLayout();

            StoreID = 0;
            
            //txtClinicalNetAmount.Text = "";
            //txtPharmacyNetAmount.Text = "";

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


            chkFreezBill.IsChecked = false;
            SelectedBill = null;
            BillID = 0;
            CurrentVisitTypeID = 0;
           // cmdModify.IsEnabled = true;
            lnkAddItems.IsEnabled = true;
            lnkAddItemsFromPrescription.IsEnabled = true;
            lnkAddService.IsEnabled = true;
            lnkAddServicesFromPrescription.IsEnabled = true;
            chkFreezBill.IsEnabled = true;
            

           // SelectedBill = new clsBillVO();
           // this.DataContext = new clsGRNVO();
        }

        #region Set Command Button State New/Save/Modify/Print
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    //cmdPrint.IsEnabled = false;
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = false;
                    cmdModify.IsEnabled = false;
                    break;

                case "Save":
                    //cmdPrint.IsEnabled = true; 
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = false;
                    cmdModify.IsEnabled = false;
                    break;

                case "Modify":
                    //cmdPrint.IsEnabled = false; 
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdModify.IsEnabled = true;
                    break;

                case "ClickNew":
                    //cmdPrint.IsEnabled = true; 
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    cmdModify.IsEnabled = false;
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
                        string strMsg = "Items already added : " + strError.ToString();

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                }
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgBillList.SelectedItem != null)
                {
                    IsEditMode = true;
                     InitialiseForm();
                    SetCommandButtonState("Modify");
                    //InitialiseForm();
                    rdbAgainstBill.IsChecked = true;
                    rdbAgainstBill.IsEnabled = false;
                    rdbAgainstServices.IsEnabled = false;
                    SelectedBill = new clsBillVO();
                    SelectedBill = (clsBillVO)dgBillList.SelectedItem;

                    BillID = ((clsBillVO)dgBillList.SelectedItem).ID;
                    
                    if (((clsBillVO)dgBillList.SelectedItem).IsFreezed == true)
                    {
                     
                        cmdModify.IsEnabled = false;
                        lnkAddItems.IsEnabled = false;
                        lnkAddItemsFromPrescription.IsEnabled = false;
                        lnkAddService.IsEnabled = false;
                        lnkAddServicesFromPrescription.IsEnabled = false;
                        chkFreezBill.IsEnabled =false;

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


                        FillChargeList(SelectedBill.ID, SelectedBill.UnitID, true, SelectedBill.Opd_Ipd_External_Id);
                        FillPharmacyItemsList(SelectedBill.ID,SelectedBill.UnitID, true);
                        if (((clsBillVO)dgBillList.SelectedItem).BillPaymentType == PalashDynamics.ValueObjects.BillPaymentTypes.AgainstServices)
                            rdbAgainstServices.IsChecked = true;
                    }
                    else
                    {

                       

                        FillChargeList(SelectedBill.ID,SelectedBill.UnitID, false,SelectedBill.Opd_Ipd_External_Id);
                        FillPharmacyItemsList(SelectedBill.ID,SelectedBill.UnitID, false);
                    }
                    //long BillID;
                    chkFreezBill.IsChecked = ((clsBillVO)dgBillList.SelectedItem).IsFreezed;

                    _flip.Invoke(RotationType.Forward);
                  
                }
                

            }
            catch (Exception ex)
            {
                string err = ex.Message;
                //throw;
            }     
        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;                    

            isValid = CheckValidations();

            if (isValid)
            {
                if (chkFreezBill.IsChecked == true)
                {
                    isValid = false;
                    string msgTitle = "";
                    string msgText = "Are you sure you want to Freez the Bill ?";

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (arg) =>
                    {
                        if (arg == MessageBoxResult.Yes)
                        {
                            PaymentWindow paymentWin = new PaymentWindow();
                            paymentWin.TotalAmount = double.Parse(txtPayAmount.Text);
                            paymentWin.Initiate("Bill");

                            paymentWin.txtPayTotalAmount.Text = this.txtNetAmount.Text;
                            paymentWin.txtDiscAmt.Text = this.txtTotalConcession.Text;
                            paymentWin.txtPayableAmt.Text = this.txtPayAmount.Text;

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
                            paymentWin.OnSaveButton_Click += new RoutedEventHandler(paymentWin_OnSaveButton_Click);
                            paymentWin.Show();
                        }
                    };
                    msgWD.Show();


                }
                else
                {
                    SaveBill(null, false);
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
            
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (dgBillList.SelectedItem != null && ((clsBillVO)dgBillList.SelectedItem).IsFreezed == true)
            {

                InitialiseForm();
                SelectedBill = new clsBillVO();
                SelectedBill = (clsBillVO)dgBillList.SelectedItem;

                FillChargeList(SelectedBill.ID,SelectedBill.UnitID, false, SelectedBill.Opd_Ipd_External_Id);
                FillPharmacyItemsList(SelectedBill.ID,SelectedBill.UnitID, false);

                string msgTitle = "";
                string msgText = "Are you sure you want to Freez the Bill ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (arg) =>
                {
                    if (arg == MessageBoxResult.Yes)
                    {
                        bool isValid;
                        isValid = CheckValidations();

                        if (isValid)
                        {
                            //long BillID;
                            //SetCommandButtonState("Modify");
                            chkFreezBill.IsChecked = true;
                            flagFreezFromSearch = true;

                            PaymentWindow paymentWin = new PaymentWindow();
                            if (!(txtPayAmount.Text == null) && !(txtPayAmount.Text.Trim().Length == 0))
                            paymentWin.TotalAmount = double.Parse(txtPayAmount.Text);
                            paymentWin.Initiate("Bill");

                            paymentWin.txtPayTotalAmount.Text = this.txtNetAmount.Text;
                            paymentWin.txtDiscAmt.Text = this.txtTotalConcession.Text;
                            paymentWin.txtPayableAmt.Text = this.txtPayAmount.Text;

                            if (rdbAgainstBill.IsChecked == true)
                            {
                                paymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstBill;
                            }
                            else
                                paymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstServices;
                            //PaymentWindow paymentWin = new PaymentWindow();
                            //if (!(txtNetAmount.Text == null) && !(txtNetAmount.Text.Trim().Length == 0))
                            //    paymentWin.TotalAmount = Convert.ToDouble(txtNetAmount.Text);
                            //paymentWin.Initiate("Bill");

                            //paymentWin.txtPayTotalAmount.Text = this.txtTotalBill.Text;
                            //paymentWin.txtDiscAmt.Text = this.txtTotalConcession.Text;
                            //paymentWin.txtPayableAmt.Text = this.txtNetAmount.Text;
                            paymentWin.OnSaveButton_Click += new RoutedEventHandler(paymentWin_OnSaveButton_Click);
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
            }
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            
                

        }
        
        private void PrintBill(long iBillId,long iUnitID)
        {
            if (iBillId > 0)
            {
                //long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                long UnitID = iUnitID;
                string URL = "../Reports/OPD/ClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID; 
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }


        }

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

                        if (SelectedBill.BillType == PalashDynamics.ValueObjects.BillTypes.Clinical)
                        {
                            PrintBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID);
                        }
                        else if (SelectedBill.BillType == PalashDynamics.ValueObjects.BillTypes.Pharmacy)
                        {

                            PrintPharmacyBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID);
                        }
                        else
                        {
                            PrintBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID);
                            PrintPharmacyBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID);

                        }
                }
            }
          
        }
        
        private void PrintPharmacyBill(long iBillId,long IUnitID)
        {
            if (iBillId > 0)
            {
                //long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                long UnitID = IUnitID;
                string URL = "../Reports/OPD/PharmacyBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID; ;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }

        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void lnkAddServicesFromPrescription_Click(object sender, RoutedEventArgs e)
        {
            PrescriptionServicesForPatient PrescriptionService = new PrescriptionServicesForPatient();
            PrescriptionService.OnAddButton_Click += new RoutedEventHandler(PrescriptionService_OnAddButton_Click);
            PrescriptionService.Show();
        }

        void PrescriptionService_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            // throw new NotImplementedException();          

            if (((PrescriptionServicesForPatient)sender).DialogResult == true)
            {
                List<clsServiceMasterVO> lServices = new List<clsServiceMasterVO>();
                lServices = ((PrescriptionServicesForPatient)sender).SelectedServices;
                AddCharges(lServices);

            }
        }
        
        private void AddCharges(List<clsServiceMasterVO> mServices)
        {
            StringBuilder strError = new StringBuilder();
            //strError.Append("");

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

                    itemC.TotalAmount = itemC.Rate * itemC.Quantity;


                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 3 || ((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 6)
                    { //Staff Or Staff Dependent
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

                    ChargeList.Add(itemC);
                }
            }

            CalculateClinicalSummary();

            dgCharges.Focus();
            dgCharges.UpdateLayout();

            dgCharges.SelectedIndex = ChargeList.Count - 1;

            if (!string.IsNullOrEmpty(strError.ToString()))
            {
                string strMsg = "Services already added : " + strError.ToString();

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
           // if (chkFreezBill.IsChecked == true)
         

            switch (chkFreezBill.IsChecked)
            {
               

                case true:
                    rdbAgainstServices.IsEnabled = rdbAgainstBill.IsEnabled = true;
                    break;
                case false:
                     rdbAgainstServices.IsEnabled = rdbAgainstBill.IsEnabled = false;
                    rdbAgainstBill.IsChecked = true;
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

        private void btnSettle_Click(object sender, RoutedEventArgs e)
        {
            if (dgBillList.SelectedItem != null)
            {
                SettleBill();
            }
        }

        void SettleBill()
        {
            if (dgBillList.SelectedItem != null && ((clsBillVO)dgBillList.SelectedItem).IsFreezed == true)
            {

                InitialiseForm();
                SelectedBill = new clsBillVO();
                SelectedBill = (clsBillVO)dgBillList.SelectedItem;

               // FillChargeList(SelectedBill.ID, SelectedBill.UnitID, false, SelectedBill.Opd_Ipd_External_Id);
                

                string msgTitle = "";
                string msgText = "Are you sure you want to Settle the Bill ?";

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

                    BizAction.Details.BillID = SelectedBill.ID;
                    BizAction.Details.BillAmount = SelectedBill.NetBillAmount;
                    BizAction.Details.Date = DateTime.Now;
                    BizAction.Details.BillBalanceAmount = pPayDetails.PaidAmount;
            

                     Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {

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

                                     if (dgBillList.ItemsSource !=null)
                                     {
                                         dgBillList.ItemsSource = null;
                                         dgBillList.ItemsSource = DataList;
                                     } 
                                     
                                     MessageBoxControl.MessageBoxChildWindow msgWD =
                                        new MessageBoxControl.MessageBoxChildWindow("", "Payment Details Saved Sucessfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

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
            catch (Exception)
            {

                Indicatior.Close();
            }



        }

        private void cmdApprove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdSave_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
