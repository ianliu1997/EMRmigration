using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PalashDynamics.Animations;
using CIMS;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.UserControls;
using System.IO;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Browser;
using System.Windows.Markup;
using System.Windows.Data;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;
using DataDrivenApplication.Forms;
using PalashDynamics.IVF.PatientList;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Inventory;

namespace PalashDynamics.IVF.TherpyExecution
{
    public partial class IVFTherapyExecutionForSurrogate : UserControl, IInitiateCIMS
    {
        #region Variables
        WaitIndicator wait = new WaitIndicator();
        private SwivelAnimation objAnimation;
        private bool IsPatientExist;
        byte[] AttachedFileContents;
        string AttachedFileName;
        System.Collections.ObjectModel.ObservableCollection<string> AttachedFileNameList = new System.Collections.ObjectModel.ObservableCollection<string>();
        PagedCollectionView collection;
        PagedCollectionView Execollection;
        PagedCollectionView Exe1collection;
        long SurrogateNo;
        long ANCID = 0;
        public static clsTherapyExecutionVO TherapyExeDetails { get; set; }
        public bool isDrugForSurrogate = false;
        public long TherapyID;
        #endregion

        #region Properties

        private clsCoupleVO _CoupleDetails = new clsCoupleVO();
        public clsCoupleVO CoupleDetails
        {
            get
            {
                return _CoupleDetails;
            }
            set
            {
                _CoupleDetails = value;
            }
        }

        private List<clsCoupleVO> _AllCoupleDetails = new List<clsCoupleVO>();
        public List<clsCoupleVO> AllCoupleDetails
        {
            get
            {
                return _AllCoupleDetails;
            }
            set
            {
                _AllCoupleDetails = value;
            }
        }

        private clsGetTherapyListBizActionVO _TherapyDetails = new clsGetTherapyListBizActionVO();
        public clsGetTherapyListBizActionVO SelectedTherapyDetails
        {
            get
            {
                return _TherapyDetails;
            }
            set
            {
                _TherapyDetails = value;
            }
        }

        private List<clsPlanTherapyVO> _TherapyDetailsList = new List<clsPlanTherapyVO>();
        public List<clsPlanTherapyVO> TherapyDetailsList
        {
            get
            {
                return _TherapyDetailsList;
            }
            set
            {
                _TherapyDetailsList = value;
            }
        }

        public bool IsExpendedWindow { get; set; }

        public string Action { get; set; }
        public string ModuleName { get; set; }
        UIElement myData = null;
        public Boolean a;

        //public ObservableCollection<clsPlanTherapyVO> ANCList = new ObservableCollection<clsPlanTherapyVO>();
        public ObservableCollection<clsTherapyANCVO> ANCList = new ObservableCollection<clsTherapyANCVO>();
        #endregion

        #region Constructor
        public IVFTherapyExecutionForSurrogate()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(grdFrontPanel, grdBackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            this.Loaded += new RoutedEventHandler(IVFTherapyExecutionForSurrogate_Loaded);
            SelectedTherapyDetails = new clsGetTherapyListBizActionVO();
            SelectedTherapyDetails.TherapyDetailsList = new List<clsPlanTherapyVO>();
            App.TherapyExecution = new IVFTherapyExecution();
            App.TherapyExecutionSurrogate = this;        
        }

        #endregion

        #region Load Event
        void IVFTherapyExecutionForSurrogate_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsExpendedWindow)
            {
                try
                {

                    if (IsPatientExist == false)
                    {                       
                        ModuleName = "PalashDynamics";
                        Action = "CIMS.Forms.PatientList";
                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        WebClient c2 = new WebClient();
                        c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                        c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                    }
                    else
                    {
                        fillCoupleDetails();
                        fillProtocolType();
                        fillMainIndication();
                        fillSpermCollection();
                        fillPlannedTreatment();
                        fillExternalStimulation();
                        fillDoctor();
                        FillBaby();
                        //By Anjali...........
                        FillBabyType();
                        FillBabyFitness();

                        FillModeofDelivery();
                        SelectedTherapyDetails.TherapyDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.UnitId;
                        lblFemale.Text = ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).FirstName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).MiddleName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).LastName;
                      
                        if (((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).PatientID != 0)
                        {
                            if (((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).Gender.Equals("Male"))
                            {
                                changeFemalePatient.Visibility = Visibility.Collapsed;
                                changeMalePatient.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                changeFemalePatient.Visibility = Visibility.Collapsed;
                            }
                        }
                    }
                    ExeGrid.KeyUp += new KeyEventHandler(ExeGrid_KeyUp);
                    ExeGrid1.KeyUp += new KeyEventHandler(ExeGrid1_KeyUp);

                    IsExpendedWindow = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region Setup Page
        public void setupPage(long TherapyID,long TabId)
        {
            try
            {
                wait.Show();
            clsGetTherapyListBizActionVO BizAction = new clsGetTherapyListBizActionVO();
            BizAction.TherapyID = TherapyID;
            BizAction.TabID = TabId;
            BizAction.TherapyDetails.IsSurrogate = true;
            BizAction.Flag = true;
            if (SelectedTherapyDetails.TherapyDetails.UnitID == 0)
            {
                BizAction.TherapyUnitID=((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.UnitId;
            }
            else
            {
                BizAction.TherapyUnitID = SelectedTherapyDetails.TherapyDetails.UnitID;
            }

            if (CoupleDetails != null)
            {
                BizAction.CoupleID = CoupleDetails.CoupleId;
                BizAction.CoupleUintID = CoupleDetails.CoupleUnitId;
            }
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (TherapyID == 0)// fill Therapy Data Grid
                    {
                        dgTherpyRptList.ItemsSource = null;
                        dgTherpyRptList.ItemsSource = ((clsGetTherapyListBizActionVO)args.Result).TherapyDetailsList;
                        dgTherpyRptList.SelectedIndex = 0;
                        TherapyDetailsList = new List<clsPlanTherapyVO>();
                        TherapyDetailsList = ((clsGetTherapyListBizActionVO)args.Result).TherapyDetailsList;
                            if (TherapyDetailsList != null)
                            {
                                foreach (var item in TherapyDetailsList)
                                {
                                    if (item.IsClosed == false)
                                    {
                                        a = true;
                                        break;
                                    }
                                    else
                                        a = false;
                                }
                            }
                            wait.Close();
                        }
                       
                    else// Fill Other Data Grid According to TAbID
                    {
                        if (TabId == 0)
                        {
                            dgDocumentGrid.ItemsSource = null;
                            dgFollicularMonitoring.ItemsSource = null;
                            dgDocumentGrid.ItemsSource = ((clsGetTherapyListBizActionVO)args.Result).TherapyDocument;
                            dgFollicularMonitoring.ItemsSource = ((clsGetTherapyListBizActionVO)args.Result).FollicularMonitoringList;
                           
                            foreach (var item in  ((clsGetTherapyListBizActionVO)args.Result).ANCList)
                            {
                                ANCList.Add(item);                                
                            }
                            dgANCList.ItemsSource = ANCList;

                            dgDocumentGrid.SelectedIndex = 0;
                            dgANCList.SelectedIndex = 0;
                            SelectedTherapyDetails.TherapyDocument = ((clsGetTherapyListBizActionVO)args.Result).TherapyDocument;
                            //if(BizAction.IsSurrogate ==false)
                             SelectedTherapyDetails.TherapyExecutionList = ((clsGetTherapyListBizActionVO)args.Result).TherapyExecutionList;
                           //else
                             SelectedTherapyDetails.TherapyExecutionListSurrogate = ((clsGetTherapyListBizActionVO)args.Result).TherapyExecutionListSurrogate;
                         
                            SelectedTherapyDetails.FollicularMonitoringList = ((clsGetTherapyListBizActionVO)args.Result).FollicularMonitoringList;
                            SelectedTherapyDetails.ANCList = ((clsGetTherapyListBizActionVO)args.Result).ANCList;
                            SelectedTherapyDetails.TherapyDelivery = ((clsGetTherapyListBizActionVO)args.Result).TherapyDelivery;
                        
                            this.DataContext = SelectedTherapyDetails.TherapyDelivery;
                            //cmbBaby.SelectedValue = ((clsTherapyDeliveryVO)(clsGetTherapyListBizActionVO)SelectedTherapyDetails.TherapyDelivery).Baby;
                            cmbBaby.SelectedValue = ((clsTherapyDeliveryVO)this.DataContext).Baby;
                            cmbMode.SelectedValue = ((clsTherapyDeliveryVO)SelectedTherapyDetails.TherapyDelivery).Mode;
                            dtdeliverydate.SelectedDate = ((clsTherapyDeliveryVO)this.DataContext).DeliveryDate;
                            txtWeight.Text = Convert.ToString(((clsTherapyDeliveryVO)this.DataContext).Weight);
                            dttimeofbirth.Value = ((clsTherapyDeliveryVO)this.DataContext).TimeofBirth;
                            this.DataContext = null;
                            this.DataContext = SelectedTherapyDetails;
                            InitExecutionGrid();
                            InitExecutionGridSurrogate();
                            BindExecutionGrid();                    
                            BindExecutionGridSurrogate();                      
                            wait.Close();
                        }
                        else
                        {
                            if(TabId==((int)TherapyTabs.FollicularMonitoring))
                            {
                                dgFollicularMonitoring.ItemsSource = null;
                                dgFollicularMonitoring.ItemsSource = ((clsGetTherapyListBizActionVO)args.Result).FollicularMonitoringList;
                                SelectedTherapyDetails.FollicularMonitoringList = ((clsGetTherapyListBizActionVO)args.Result).FollicularMonitoringList;
                                this.DataContext = SelectedTherapyDetails;
                                InitExecutionGrid();
                                //InitExecutionGridSurrogate();
                                BindExecutionGrid();
                                //BindExecutionGridSurrogate();
                                wait.Close();
                            }
                            else if(TabId==((int)TherapyTabs.Documents))
                            {
                                dgDocumentGrid.ItemsSource = null;
                                dgDocumentGrid.ItemsSource = ((clsGetTherapyListBizActionVO)args.Result).TherapyDocument;
                                SelectedTherapyDetails.TherapyDocument = ((clsGetTherapyListBizActionVO)args.Result).TherapyDocument;
                                this.DataContext = SelectedTherapyDetails;
                                InitExecutionGrid();
                               // InitExecutionGridSurrogate();
                                BindExecutionGrid();
                               // BindExecutionGridSurrogate();
                                wait.Close();
                            }
                            else if (TabId == ((int)TherapyTabs.Execution))
                            {
                               // if(BizAction.IsSurrogate==false)                              
                              SelectedTherapyDetails.TherapyExecutionList = ((clsGetTherapyListBizActionVO)args.Result).TherapyExecutionList;
                                 // else
                              SelectedTherapyDetails.TherapyExecutionListSurrogate = ((clsGetTherapyListBizActionVO)args.Result).TherapyExecutionListSurrogate;
                           
                                this.DataContext = SelectedTherapyDetails;
                                InitExecutionGrid();
                                InitExecutionGridSurrogate();
                                BindExecutionGrid();
                                BindExecutionGridSurrogate();                                                          
                            }
                            else if (TabId == ((int)TherapyTabs.ANCVisit))
                            {
                                SelectedTherapyDetails.ANCList = ((clsGetTherapyListBizActionVO)args.Result).ANCList;
                                this.DataContext = SelectedTherapyDetails;
                            }
                            else if (TabId == ((int)TherapyTabs.Deliverydetails))
                            {
                                SelectedTherapyDetails.TherapyDelivery = ((clsGetTherapyListBizActionVO)args.Result).TherapyDelivery;
                                this.DataContext = SelectedTherapyDetails.TherapyDelivery;
                                //cmbCategory.SelectedValue = ((clsPathoResultEntryVO)this.DataContext).CategoryID;
                                cmbBaby.SelectedItem = ((clsTherapyDeliveryVO)this.DataContext).Baby;                                
                                cmbMode.SelectedItem = ((clsTherapyDeliveryVO)this.DataContext).Mode;
                                dtdeliverydate.SelectedDate = ((clsTherapyDeliveryVO)this.DataContext).DeliveryDate;
                                txtWeight.Text = Convert.ToString(((clsTherapyDeliveryVO)this.DataContext).Weight);
                                dttimeofbirth.Value = ((clsTherapyDeliveryVO)this.DataContext).TimeofBirth;
                            }                            
                        }
                        dgTherpyRptList.SelectedItem = TherapyDetailsList.FirstOrDefault(p => p.ID == TherapyID);
                    }
                    wait.Close();
                }
                else
                {
                    wait.Close();
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
            }
            catch(Exception ex)
            {
                wait.Close();
                throw ex;
            }
        }
        #endregion

        #region Set All Fields
        private void setAllFields()
        {
        }
        #endregion

        #region Check Patient Is Selected/Not
        public void Initiate(string Mode)
        {
            switch (Mode.ToUpper())
            {
                case "NEW":
                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
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
                   
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                    mElement.Text = " : " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).FirstName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).MiddleName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).LastName;
                     break;   
                default:
                    break;
            }
        }

        #endregion

        #region added by priti for getting surrogate patient header
        private void LoadPatientHeader()
        {
            clsGetPatientBizActionVO BizAction = new PalashDynamics.ValueObjects.Patient.clsGetPatientBizActionVO();
            BizAction.PatientDetails = new PalashDynamics.ValueObjects.Patient.clsPatientVO();         
            BizAction.SurrogateID = SurrogateNo;
            BizAction.PatientDetails.GeneralDetails = (clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.PatientDetails.GeneralDetails = ((clsGetPatientBizActionVO)args.Result).PatientDetails.GeneralDetails;
                    BizAction.PatientDetails.SpouseDetails = ((clsGetPatientBizActionVO)args.Result).PatientDetails.SpouseDetails;
                    Male.Visibility = Visibility.Collapsed;
                    Patient.Visibility = Visibility.Visible;
                    Patient.DataContext = BizAction.PatientDetails.GeneralDetails;
                    lblSurrogate.Text = ((clsGetPatientBizActionVO)args.Result).PatientDetails.GeneralDetails.PatientName;
                    //if (imgPhoto12 != null) 
                    if (BizAction.PatientDetails.Photo != null)
                    {
                        WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto12.Width, (int)imgPhoto12.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                        bmp.FromByteArray(((clsGetPatientBizActionVO)args.Result).PatientDetails.Photo);
                        imgPhoto12.Source = bmp;
                    }
                    else
                    {
                        imgPhoto12.Source = null;
                    }
               
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

      #endregion

        #region Fill Couple Details
        private void fillCoupleDetails()
        {
            try
            {
            clsGetCoupleDetailsBizActionVO BizAction = new clsGetCoupleDetailsBizActionVO();
            BizAction.PatientID = ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
            BizAction.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
            BizAction.IsAllCouple = false;
            BizAction.CoupleDetails = new clsCoupleVO();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            wait.Show();
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.CoupleId > 0)
                    {
                        
                        BizAction.CoupleDetails.MalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient;
                        BizAction.CoupleDetails.FemalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient;
                        CoupleDetails.MalePatient = new clsPatientGeneralVO();
                        CoupleDetails.FemalePatient = new clsPatientGeneralVO();
                        CoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails;
                        if (BizAction.CoupleDetails.MalePatient == null || BizAction.CoupleDetails.FemalePatient == null || BizAction.CoupleDetails.FemalePatient.PatientID == 0 || BizAction.CoupleDetails.MalePatient.PatientID == 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot Create Therapy, Therapy is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();

                            ModuleName = "PalashDynamics";
                            Action = "CIMS.Forms.PatientList";
                            UserControl rootPage = Application.Current.RootVisual as UserControl;

                            WebClient c2 = new WebClient();
                            c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                            c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                            wait.Close();
                        }
                        else
                        {
                            GetHeightAndWeight(BizAction.CoupleDetails);
                            if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.Photo != null)
                            {
                                WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto13.Width, (int)imgPhoto13.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                                bmp.FromByteArray(((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.Photo);
                                imgPhoto13.Source = bmp;
                                imgPhoto13.Visibility = System.Windows.Visibility.Visible;
                            }
                            else
                            {
                                imgP1.Visibility = System.Windows.Visibility.Visible;
                            }

                            if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.Photo != null)
                            {
                                WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto12.Width, (int)imgPhoto12.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                                bmp.FromByteArray(((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.Photo);
                                imgPhoto12.Source = bmp;
                                imgPhoto12.Visibility = System.Windows.Visibility.Visible;
                            }
                            else
                            {
                                imgP2.Visibility = System.Windows.Visibility.Visible;
                            }      
                            setupPage(0, 0);//Fill Therpy List
                        }
                    }

                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot Create Therapy, Therapy is Only For Active Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        ModuleName = "PalashDynamics";
                        Action = "CIMS.Forms.PatientList";
                        UserControl rootPage = Application.Current.RootVisual as UserControl;

                        WebClient c2 = new WebClient();
                        c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                        c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                        wait.Close();
                    }
                }
                else
                {
                    wait.Close();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
            }
            catch(Exception ex)
            {
                wait.Close();
                throw ex;
            }
        }

        private void GetHeightAndWeight(clsCoupleVO CoupleDetails)
        {
            clsGetGetCoupleHeightAndWeightBizActionVO BizAction = new clsGetGetCoupleHeightAndWeightBizActionVO();
            BizAction.CoupleDetails = new clsCoupleVO();
            BizAction.FemalePatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.MalePatientID = CoupleDetails.MalePatient.PatientID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (((clsGetGetCoupleHeightAndWeightBizActionVO)args.Result).CoupleDetails != null)
                        BizAction.CoupleDetails = ((clsGetGetCoupleHeightAndWeightBizActionVO)args.Result).CoupleDetails;
                    if (BizAction.CoupleDetails != null)
                    {
                        clsPatientGeneralVO FemalePatientDetails = new clsPatientGeneralVO();
                        FemalePatientDetails = CoupleDetails.FemalePatient;
                        FemalePatientDetails.Height = BizAction.CoupleDetails.FemalePatient.Height;
                        FemalePatientDetails.Weight = BizAction.CoupleDetails.FemalePatient.Weight;
                        FemalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", BizAction.CoupleDetails.FemalePatient.BMI));
                        FemalePatientDetails.Alerts = BizAction.CoupleDetails.FemalePatient.Alerts;
                        Female.DataContext = FemalePatientDetails;

                        clsPatientGeneralVO MalePatientDetails = new clsPatientGeneralVO();
                        MalePatientDetails = CoupleDetails.MalePatient;
                        MalePatientDetails.Height = BizAction.CoupleDetails.MalePatient.Height;
                        MalePatientDetails.Weight = BizAction.CoupleDetails.MalePatient.Weight;
                        MalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", BizAction.CoupleDetails.MalePatient.BMI));
                        MalePatientDetails.Alerts = BizAction.CoupleDetails.MalePatient.Alerts;
                        Male.DataContext = MalePatientDetails;
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        #endregion

        #region Fill All Couple Details
        private void fillAllCoupleDetails()
        {
            try
            {
                clsGetCoupleDetailsBizActionVO BizAction = new clsGetCoupleDetailsBizActionVO();
                BizAction.PatientID = ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
                BizAction.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
                BizAction.IsAllCouple = true;
                BizAction.CoupleDetails = new clsCoupleVO();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        BizAction.CoupleDetails.MalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient;
                        BizAction.CoupleDetails.FemalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient;
                        AllCoupleDetails = new List<clsCoupleVO>();                        
                        AllCoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).AllCoupleDetails;                      
                    }                                    
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
                throw ex;
            }
        }
        #endregion

        #region Fill Combobox
        //By Anjali..........
        private void FillBabyType()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_BabyType;
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
                    cmbBabyType.ItemsSource = null;
                    cmbBabyType.ItemsSource = objList;
                    cmbBabyType.SelectedValue = (long)0;
                }
                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void FillBabyFitness()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_BabyFitness;
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
                    // 6 month
                    cmbBO6month.ItemsSource = null;
                    cmbBO6month.ItemsSource = objList;
                    cmbBO6month.SelectedValue = (long)0;

                    cmbBO6month_m.ItemsSource = null;
                    cmbBO6month_m.ItemsSource = objList;
                    cmbBO6month_m.SelectedValue = (long)0;
                    // 1 Year
                    cmbBO1yr.ItemsSource = null;
                    cmbBO1yr.ItemsSource = objList;
                    cmbBO1yr.SelectedValue = (long)0;

                    cmbBO1yr_m.ItemsSource = null;
                    cmbBO1yr_m.ItemsSource = objList;
                    cmbBO1yr_m.SelectedValue = (long)0;
                    // 5 year
                    cmbBO5yr.ItemsSource = null;
                    cmbBO5yr.ItemsSource = objList;
                    cmbBO5yr.SelectedValue = (long)0;

                    cmbBO5yr_m.ItemsSource = null;
                    cmbBO5yr_m.ItemsSource = objList;
                    cmbBO5yr_m.SelectedValue = (long)0;
                    // 10 year
                    cmbBO10yr.ItemsSource = null;
                    cmbBO10yr.ItemsSource = objList;
                    cmbBO10yr.SelectedValue = (long)0;

                    cmbBO10yr_m.ItemsSource = null;
                    cmbBO10yr_m.ItemsSource = objList;
                    cmbBO10yr_m.SelectedValue = (long)0;
                    // 20 Year
                    cmbBO20yr.ItemsSource = null;
                    cmbBO20yr.ItemsSource = objList;
                    cmbBO20yr.SelectedValue = (long)0;

                    cmbBO20yr_m.ItemsSource = null;
                    cmbBO20yr_m.ItemsSource = objList;
                    cmbBO20yr_m.SelectedValue = (long)0;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void fillProtocolType()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVFProtocolType;
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
                    cmbProtocol.ItemsSource = null;
                    cmbProtocol.ItemsSource = objList;
                    cmbProtocol.SelectedValue = (long)0;
                }
                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void fillMainIndication()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVFMainIndication;
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
                    cmbMainIndication.ItemsSource = null;
                    cmbMainIndication.ItemsSource = objList;
                    cmbMainIndication.SelectedValue = (long)0;
                }
                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void fillSpermCollection()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVFPlannedSpermCollection;
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
                    cmbSpermCollection.ItemsSource = null;
                    cmbSpermCollection.ItemsSource = objList;
                    cmbSpermCollection.SelectedValue = (long)0;
                }

                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void fillPlannedTreatment()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVFPlannedTreatment;
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
                    cmbPlannedTreatment.ItemsSource = null;
                    cmbPlannedTreatment.ItemsSource = objList;
                    cmbPlannedTreatment.SelectedValue = (long)0;
                }
                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }        

        private void fillExternalStimulation()
        {
            List<MasterListItem> Items = new List<MasterListItem>();

            MasterListItem Item = new MasterListItem();
            Item.ID = (int)0;
            Item.Description ="--Select--";
            Items.Add(Item);
            
            Item = new MasterListItem();
            Item.ID = (int)ExternalStimulation.Yes;
            Item.Description = ExternalStimulation.Yes.ToString();
            Items.Add(Item);
            
            Item = new MasterListItem();
            Item.ID = (int)ExternalStimulation.No;
            Item.Description = ExternalStimulation.No.ToString();
            Items.Add(Item);
            cmbSimulation.ItemsSource = Items;
            cmbSimulation.SelectedValue = (long)0;
        }


        private void fillDoctor()
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            BizAction.UnitId = 0;
            BizAction.DepartmentId = 0;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);

                    cmbPhysician.ItemsSource = null;
                    cmbPhysician.ItemsSource = objList;
                    cmbPhysician.SelectedValue = (long)0;
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }



        #endregion

        #region Get Patient EMR Details(Height and Weight)

        private void getEMRDetails(clsPatientGeneralVO PatientDetails,string Gender)
        {
            clsGetEMRDetailsBizActionVO BizAction = new clsGetEMRDetailsBizActionVO();
            BizAction.PatientID = PatientDetails.PatientID;
            BizAction.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
            BizAction.TemplateID = 8;//Using For Getting Height Wight Of Patient 
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Double height = 0;
            Double weight = 0;

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.EMRDetailsList = ((clsGetEMRDetailsBizActionVO)args.Result).EMRDetailsList;

                    if (BizAction.EMRDetailsList != null || BizAction.EMRDetailsList.Count>0)
                    {
                        for (int i = 0; i < BizAction.EMRDetailsList.Count; i++)
                        {
                            if (BizAction.EMRDetailsList[i].ControlCaption.Equals("Height"))
                            {
                                if(!string.IsNullOrEmpty(BizAction.EMRDetailsList[i].Value))
                                {
                                    height = Convert.ToDouble(BizAction.EMRDetailsList[i].Value);
                                    if (height != 0 && weight != 0)
                                    {
                                        break;
                                    }
                                }
                            }
                            if (BizAction.EMRDetailsList[i].ControlCaption.Equals("Weight"))
                            {
                                 if(!string.IsNullOrEmpty(BizAction.EMRDetailsList[i].Value))
                                {
                                    weight = Convert.ToDouble(BizAction.EMRDetailsList[i].Value);
                                    if (height != 0 && weight != 0)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        
                        if (height != 0 && weight != 0)
                        {
                            if (Gender.Equals("F"))
                            {
                                PatientDetails.Height = height;
                                PatientDetails.Weight = weight;
                                PatientDetails.BMI = Math.Round(CalculateBMI(height, weight),2);
                                PatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", PatientDetails.BMI));
                                Female.DataContext = PatientDetails;
                            }
                            else
                            {
                                PatientDetails.Height = height;
                                PatientDetails.Weight = weight;
                                PatientDetails.BMI = Math.Round(CalculateBMI(height, weight),2);
                                PatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", PatientDetails.BMI));
                                Male.DataContext = PatientDetails;
                            }
                        }
                        else
                        {
                            if (Gender.Equals("F"))
                            {
                                Female.DataContext = PatientDetails;
                            }
                            else
                            {
                                Male.DataContext = PatientDetails;
                            }
                        }
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        #endregion

        #region Calculate BMI
        private double CalculateBMI(Double Height,Double Weight)
        {
            try
            {
                if (Weight==0)
                {
                    return 0.0;                    
                }
                else if (Height == 0 )
                {                    
                   return 0.0;
                }
                else
                {
                    double weight = Convert.ToDouble(Weight);
                    double height = Convert.ToDouble(Height);
                    double TotalBMI = weight / height;
                    TotalBMI = TotalBMI / height;
                    TotalBMI = TotalBMI * 10000;
                    return TotalBMI;                    
                }
            }
            catch (Exception ex)
            {
                return 0.0;
                throw ex;
            }
        }
        #endregion

        #region Save /Update Data(TabId is used which tab u want to upadate and save(According to TherapyTabs Enum(Like 1 for General Details .......))
        private void saveUpdateGeneralDetails(long ID,long TabID,long DocumentId ,String Msg,long ANCID, long DeliveryID)
        {           
            try
            {
                clsAddPlanTherapyBizActionVO BizAction = new clsAddPlanTherapyBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                BizAction.TherapyDetails = new clsPlanTherapyVO();
                BizAction.TherapyDetails = ((clsPlanTherapyVO)grdBackPanel.DataContext);
                BizAction.TherapyDetails.ID = ID;
                BizAction.TherapyDetails.TabID = TabID;
                BizAction.TherapyDetails.PatientUintId = ((clsPatientGeneralVO)Female.DataContext).UnitId;
                BizAction.TherapyDetails.CoupleId = CoupleDetails.CoupleId;
                BizAction.TherapyDetails.CoupleUnitId = CoupleDetails.CoupleUnitId;
                BizAction.TherapyDetails.IsSurrogate = true;
                BizAction.TherapyDetails.SurrogateID = SurrogateNo;
               
                #region Data Used For Therapy General Details
                if (TabID == 1)
                {
                    if (cmbPhysician.SelectedItem != null)
                    {
                        BizAction.TherapyDetails.PhysicianId = ((MasterListItem)cmbPhysician.SelectedItem).ID;
                    }
                    if (cmbMainIndication.SelectedItem != null)
                    {
                        BizAction.TherapyDetails.MainInductionID = ((MasterListItem)cmbMainIndication.SelectedItem).ID;
                    }
                    if (cmbPlannedTreatment.SelectedItem != null)
                    {
                        BizAction.TherapyDetails.PlannedTreatmentID = ((MasterListItem)cmbPlannedTreatment.SelectedItem).ID;
                    }
                    if (cmbProtocol.SelectedItem != null)
                    {
                        BizAction.TherapyDetails.ProtocolTypeID = ((MasterListItem)cmbProtocol.SelectedItem).ID;
                    }
                    if (cmbSimulation.SelectedItem != null)
                    {
                        BizAction.TherapyDetails.ExternalSimulationID = ((MasterListItem)cmbSimulation.SelectedItem).ID;
                    }
                    if (cmbSpermCollection.SelectedItem != null)
                    {
                        BizAction.TherapyDetails.PlannedSpermCollectionID = ((MasterListItem)cmbSpermCollection.SelectedItem).ID;
                    }
                    if (rdoBSCGAss1Negative.IsChecked!=null )
                    {
                        if (rdoBSCGAss1Negative.IsChecked == true)
                        {
                            BizAction.TherapyDetails.BHCGAss1IsBSCG = false;
                        }
                    }
                    if (rdoBSCGAss1Positve.IsChecked != null)
                    {
                        if (rdoBSCGAss1Positve.IsChecked == true)
                        {
                            BizAction.TherapyDetails.BHCGAss1IsBSCG = true;
                        }
                    }
                    if (rdoBSCGAss2Negative.IsChecked != null)
                    {
                        if (rdoBSCGAss2Negative.IsChecked == true)
                        {
                            BizAction.TherapyDetails.BHCGAss2IsBSCG = false;
                        }
                    }
                    if (rdoBSCGAss2Positve.IsChecked != null)
                    {
                        if (rdoBSCGAss2Positve.IsChecked == true)
                        {
                            BizAction.TherapyDetails.BHCGAss2IsBSCG = true;
                        }
                    }
                    if (rdoPregnancyAchievedNo.IsChecked != null)
                    {
                        if (rdoPregnancyAchievedNo.IsChecked == true)
                        {
                            BizAction.TherapyDetails.IsPregnancyAchieved = false;
                        }
                    }
                    if (rdoPregnancyAchievedYes.IsChecked != null)
                    {
                        if (rdoPregnancyAchievedYes.IsChecked == true)
                        {
                            BizAction.TherapyDetails.IsPregnancyAchieved = true;
                        }
                    }
                }
                #endregion

                #region Data Used For Therapy OutCome

                if (TabID == 6)
                {
                    if (rdoBSCGAss1Negative.IsChecked != null)
                    {
                        if (rdoBSCGAss1Negative.IsChecked == true)
                        {
                            BizAction.TherapyDetails.BHCGAss1IsBSCG = false;
                        }
                    }
                    if (rdoBSCGAss1Positve.IsChecked != null)
                    {
                        if (rdoBSCGAss1Positve.IsChecked == true)
                        {
                            BizAction.TherapyDetails.BHCGAss1IsBSCG = true;
                        }
                    }
                    if (rdoBSCGAss2Negative.IsChecked != null)
                    {
                        if (rdoBSCGAss2Negative.IsChecked == true)
                        {
                            BizAction.TherapyDetails.BHCGAss2IsBSCG = false;
                        }
                    }
                    if (rdoBSCGAss2Positve.IsChecked != null)
                    {
                        if (rdoBSCGAss2Positve.IsChecked == true)
                        {
                            BizAction.TherapyDetails.BHCGAss2IsBSCG = true;
                        }
                    }
                    if (rdoPregnancyAchievedNo.IsChecked != null)
                    {
                        if (rdoPregnancyAchievedNo.IsChecked == true)
                        {
                            BizAction.TherapyDetails.IsPregnancyAchieved = false;
                        }
                    }
                    if (rdoPregnancyAchievedYes.IsChecked != null)
                    {
                        if (rdoPregnancyAchievedYes.IsChecked == true)
                        {
                            BizAction.TherapyDetails.IsPregnancyAchieved = true;
                        }
                    }

                    //By Anjali........ On 21/01/2014
                    if (cmbBabyType.SelectedItem != null)
                    {
                        BizAction.TherapyDetails.BabyTypeID = ((MasterListItem)cmbBabyType.SelectedItem).ID;
                    }
                    
                    if (txtOHSSremark.Text != null && txtOHSSremark.Text != string.Empty)
                    {
                        BizAction.TherapyDetails.OHSSRemark = txtOHSSremark.Text.Trim();
                    }

                    if (chkOHSSEarly.IsChecked != null)
                    {
                        if (chkOHSSEarly.IsChecked == true)
                        {
                            BizAction.TherapyDetails.OHSSEarly = true;
                        }
                    }

                    if (chkOHSSLate.IsChecked != null)
                    {
                        if (chkOHSSLate.IsChecked == true)
                        {
                            BizAction.TherapyDetails.OHSSLate = true;
                        }
                    }
                    if (chkOHSSmild.IsChecked != null)
                    {
                        if (chkOHSSmild.IsChecked == true)
                        {
                            BizAction.TherapyDetails.OHSSMild = true;
                        }
                    }
                    if (chkOHSSmod.IsChecked != null)
                    {
                        if (chkOHSSmod.IsChecked == true)
                        {
                            BizAction.TherapyDetails.OHSSMode = true;
                        }
                    }
                    if (chkOHSSsevere.IsChecked != null)
                    {
                        if (chkOHSSsevere.IsChecked == true)
                        {
                            BizAction.TherapyDetails.OHSSSereve = true;
                        }
                    }
                    // Outcome Baby
                    if (cmbBO6month.SelectedItem != null)                   // 6 month
                    {
                        BizAction.TherapyDetails.SIXmonthFitnessID = ((MasterListItem)cmbBO6month.SelectedItem).ID;
                    }

                    if (txtBO6month.Text != null && txtBO6month.Text != string.Empty)
                    {
                        BizAction.TherapyDetails.SIXmonthFitnessRemark = txtBO6month.Text.Trim();
                    }


                    if (cmbBO6month_m.SelectedItem != null)                   // 6 month Mother
                    {
                        BizAction.TherapyDetails.SIXmonthFitnessID_m = ((MasterListItem)cmbBO6month_m.SelectedItem).ID;
                    }

                    if (txtBO6month_m.Text != null && txtBO6month_m.Text != string.Empty)
                    {
                        BizAction.TherapyDetails.SIXmonthFitnessRemark_m = txtBO6month_m.Text.Trim();
                    }

                    if (cmbBO1yr.SelectedItem != null)                      // 1 year
                    {
                        BizAction.TherapyDetails.ONEyFitnessID = ((MasterListItem)cmbBO1yr.SelectedItem).ID;
                    }

                    if (txtBO1Year.Text != null && txtBO1Year.Text != string.Empty)
                    {
                        BizAction.TherapyDetails.ONEyFitnessRemark= txtBO1Year.Text.Trim();
                    }

                    if (cmbBO1yr_m.SelectedItem != null)                      // 1 year   MOther
                    {
                        BizAction.TherapyDetails.ONEyFitnessID_m = ((MasterListItem)cmbBO1yr_m.SelectedItem).ID;
                    }

                    if (txtBO1Year_m.Text != null && txtBO1Year_m.Text != string.Empty)
                    {
                        BizAction.TherapyDetails.ONEyFitnessRemark_m = txtBO1Year_m.Text.Trim();
                    }

                    if (cmbBO5yr.SelectedItem != null)                      // 5 year
                    {
                        BizAction.TherapyDetails.FIVEyFitnessID = ((MasterListItem)cmbBO5yr.SelectedItem).ID;
                    }

                    if (txtBO5Year.Text != null && txtBO5Year.Text != string.Empty)
                    {
                        BizAction.TherapyDetails.FIVEyFitnessRemark = txtBO5Year.Text.Trim();
                    }

                    if (cmbBO5yr_m.SelectedItem != null)                      // 5 year Mother
                    {
                        BizAction.TherapyDetails.FIVEyFitnessID_m = ((MasterListItem)cmbBO5yr_m.SelectedItem).ID;
                    }

                    if (txtBO5Year_m.Text != null && txtBO5Year_m.Text != string.Empty)
                    {
                        BizAction.TherapyDetails.FIVEyFitnessRemark_m = txtBO5Year_m.Text.Trim();
                    }

                    if (cmbBO10yr.SelectedItem != null)                     // 10Year
                    {
                        BizAction.TherapyDetails.TENyFitnessID = ((MasterListItem)cmbBO10yr.SelectedItem).ID;
                    }

                    if (txtBO10Year.Text != null && txtBO10Year.Text != string.Empty)
                    {
                        BizAction.TherapyDetails.TENyFitnessRemark = txtBO10Year.Text.Trim();
                    }

                    if (cmbBO10yr_m.SelectedItem != null)                     // 10Year Mother
                    {
                        BizAction.TherapyDetails.TENyFitnessID_m = ((MasterListItem)cmbBO10yr_m.SelectedItem).ID;
                    }

                    if (txtBO10Year_m.Text != null && txtBO10Year_m.Text != string.Empty)
                    {
                        BizAction.TherapyDetails.TENyFitnessRemark_m = txtBO10Year_m.Text.Trim();
                    }


                    if (cmbBO20yr.SelectedItem != null)        // 20 year/.....
                    {
                        BizAction.TherapyDetails.TWENTYyFitnessID = ((MasterListItem)cmbBO20yr.SelectedItem).ID;
                    }

                    if (txtBO20Year.Text != null && txtBO20Year.Text != string.Empty)
                    {
                        BizAction.TherapyDetails.TWENTYyFitnessRemark = txtBO20Year.Text.Trim();
                    }

                    if (cmbBO20yr_m.SelectedItem != null)        // 20 year/.....    MOther
                    {
                        BizAction.TherapyDetails.TWENTYyFitnessID_m = ((MasterListItem)cmbBO20yr_m.SelectedItem).ID;
                    }

                    if (txtBO20Year_m.Text != null && txtBO20Year_m.Text != string.Empty)
                    {
                        BizAction.TherapyDetails.TWENTYyFitnessRemark_m = txtBO20Year_m.Text.Trim();
                    }   
                 }

                #endregion

                #region Therapy Document

                if (TabID == 4)
                {
                    BizAction.TherapyDocument = new PalashDynamics.ValueObjects.IVFPlanTherapy.clsTherapyDocumentsVO();
                    BizAction.TherapyDocument.ID = DocumentId;// ((clsTherapyDocumentsVO)dgDocumentGrid.SelectedItem).ID;
                    BizAction.TherapyDocument.Date = System.DateTime.Now;
                    BizAction.TherapyDocument.AttachedFileContent = AttachedFileContents;
                    BizAction.TherapyDocument.AttachedFileName = AttachedFileName;
                    BizAction.TherapyDocument.Title = txtTitle.Text.Trim();
                    BizAction.TherapyDocument.Description = txtDescription.Text.Trim();
                    if (DocumentId == 0)
                    {
                        BizAction.TherapyDocument.IsDeleted = false;
                    }
                    else
                    {
                        BizAction.TherapyDocument.IsDeleted = true;
                    }
                }

                #endregion
                #region ANCVisit
                if (TabID == 7)
                {
                    //BizAction.ANCList  = new List<clsTherapyANCVO>();
                    BizAction.ANCList = ANCList.ToList();               
                    BizAction.TherapyANCVisits.ANCID = ANCID;                
                    }                 
                #endregion
                #region Delivery details
                if (TabID == 8)
                {
                    BizAction.TherapyDelivery = new clsTherapyDeliveryVO();
                    BizAction.TherapyDelivery.ID = DeliveryID;
                    BizAction.TherapyDelivery.DeliveryDate = dtdeliverydate.SelectedDate;
                    BizAction.TherapyDelivery.Weight = Convert.ToDouble(txtWeight.Text);
                    BizAction.TherapyDelivery.TimeofBirth = dttimeofbirth.Value;
                    if (cmbMode.SelectedItem != null)
                    {                    
                        BizAction.TherapyDelivery.Mode = ((MasterListItem)cmbMode.SelectedItem).Description;
                    }
                    if (cmbBaby.SelectedItem != null)
                    {
                        BizAction.TherapyDelivery.Baby = ((MasterListItem)cmbBaby.SelectedItem).Description;
                    }                  
                }
                #endregion

                #region Service Call (Check Validation)

                client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", Msg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            dtthreapystartdate.IsEnabled = false;

                            #region Set Tab When Therapy is Generate
                            if (TabID == 1)
                            {
                                TabEmbrology.Visibility = Visibility.Visible;
                                TabDocuments.Visibility = Visibility.Visible;
                                TabFolM.Visibility = Visibility.Visible;
                                TabOutcome.Visibility = Visibility.Visible;
                                ExeCalander.Visibility = Visibility.Visible;
                                TabANCVisit.Visibility = Visibility.Visible;
                                TabDelieveryDetails.Visibility = Visibility.Visible;
                            }
                            #endregion
                            if (TabID == 4)//When Document Is Added and Upadated
                            {
                                setupPage(((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).ID,TabID);//Fill Document Data Grid
                            }
                            else if (TabID== (int)TherapyTabs.General)
                            {
                                SelectedTherapyDetails.TherapyDetails = new clsPlanTherapyVO();
                                SelectedTherapyDetails.TherapyDetails.ID = ((clsAddPlanTherapyBizActionVO)args.Result).SuccessStatus;
                                BizAction.TherapyDetails.UnitID = ((clsPlanTherapyVO)grdBackPanel.DataContext).UnitID;
                                SelectedTherapyDetails.TherapyDetails.TherapyStartDate = dtthreapystartdate.SelectedDate;
                                setupPage(0, 0);//to Get Therapy List
                                setupPage(SelectedTherapyDetails.TherapyDetails.ID, 0);//Get Selected Therapy Details
                            }

                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                
                #endregion
            }
            catch (Exception ex)
            {
                wait.Close();
                throw ex;
            }
        }
        #endregion

        #region Save/Cancel Event

        #region General Details Add/Update
        
        #region Validation

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        public bool GenValidation()
        {
            if (dtthreapystartdate.SelectedDate == null)
            {
                dtthreapystartdate.SetValidation("Please Select Therapy Date");
                dtthreapystartdate.RaiseValidationError();
                dtthreapystartdate.Focus();
                return false;
            }
            else if (cmbPlannedTreatment.SelectedItem == null)
            {
                dtthreapystartdate.ClearValidationError();
                cmbPlannedTreatment.TextBox.SetValidation("Please Select Planned Treatment");
                cmbPlannedTreatment.TextBox.RaiseValidationError();
                cmbPlannedTreatment.Focus();
                return false;
            }
            else if (((MasterListItem)cmbPlannedTreatment.SelectedItem).ID == 0)
            {                
                dtthreapystartdate.ClearValidationError();
                cmbPlannedTreatment.TextBox.SetValidation("Please Select Planned Treatment");
                cmbPlannedTreatment.TextBox.RaiseValidationError();
                cmbPlannedTreatment.Focus();
                return false;
            }
            else if (cmbMainIndication.SelectedItem == null)
            {
                cmbPlannedTreatment.ClearValidationError();
                dtthreapystartdate.ClearValidationError();
                cmbMainIndication.TextBox.SetValidation("Please Select Main Indication");
                cmbMainIndication.TextBox.RaiseValidationError();
                cmbMainIndication.Focus();
                return false;
            }
            else if (((MasterListItem)cmbMainIndication.SelectedItem).ID == 0)
            {
                cmbPlannedTreatment.ClearValidationError();
                dtthreapystartdate.ClearValidationError();
                cmbMainIndication.TextBox.SetValidation("Please Select Main Indication");
                cmbMainIndication.TextBox.RaiseValidationError();
                cmbMainIndication.Focus();
                return false;
            }
            else if (cmbPhysician.SelectedItem == null)
            {
                cmbPlannedTreatment.ClearValidationError();
                dtthreapystartdate.ClearValidationError();
                cmbMainIndication.ClearValidationError();
                cmbPhysician.TextBox.SetValidation("Please Select Physician");
                cmbPhysician.TextBox.RaiseValidationError();
                cmbPhysician.Focus();
                return false;
            }
            else if (((MasterListItem)cmbPhysician.SelectedItem).ID == 0)
            {
                cmbPlannedTreatment.ClearValidationError();
                dtthreapystartdate.ClearValidationError();
                cmbMainIndication.ClearValidationError();
                cmbPhysician.TextBox.SetValidation("Please Select Physician");
                cmbPhysician.TextBox.RaiseValidationError();
                cmbPhysician.Focus();
                return false;
            }
            else
            {
                cmbPlannedTreatment.ClearValidationError();
                dtthreapystartdate.ClearValidationError();
                cmbMainIndication.ClearValidationError();
                cmbPhysician.ClearValidationError();
                return true;
            }
            //Added By Yogita
            if (txtPlannedNoOfEmbryos.Text != "" || txtPlannedNoOfEmbryos.Text.IsNumberValid() == true)
            { 
                
                txtPlannedNoOfEmbryos.SetValidation("No. of Embryos should be in Number.");
                txtPlannedNoOfEmbryos.RaiseValidationError();
                txtPlannedNoOfEmbryos.Focus();
                return false;
            }
            else
            {
                txtPlannedNoOfEmbryos.ClearValidationError();
            }
        }
        //End
        #endregion
        
        private void cmdGenerate_Click(object sender, RoutedEventArgs e)
        {
            if (cmdGenerate.Visibility == Visibility.Visible)
            {
                if (GenValidation())
                {
                    string msgTitle = "Palash";
                    string msgText = "Are You Sure \n You Want To Save Therapy General Details?";
                    MessageBoxControl.MessageBoxChildWindow msgWin =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWin.OnMessageBoxClosed += (result) =>
                    {
                        if (result == MessageBoxResult.Yes)
                        {
                            saveUpdateGeneralDetails(0, (int)TherapyTabs.General, 0, "Therapy General Details Saved Successfully",0,0);
                            cmdGenerate.IsEnabled = false;
                            if(txtSurrogateCode.Text !=null && txtSurrogateCode.Text !="")
                            {
                             LoadPatientHeader();                                
                            }
                        }
                    };
                    msgWin.Show();         
                }
            }
            else
            {
                if (GenValidation())
                {
                    string msgTitle = "Palash";
                    string msgText = "Are You Sure \n You Want To Updated Therapy General Details?";
                    MessageBoxControl.MessageBoxChildWindow msgWin =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWin.OnMessageBoxClosed += (result) =>
                    {
                        if (result == MessageBoxResult.Yes)
                        {
                            saveUpdateGeneralDetails(((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).ID, (int)TherapyTabs.General, 0, "Therapy General Details Updated Successfully",0,0);
                            cmdGenerate.IsEnabled = false;
                            if (txtSurrogateCode.Text != null && txtSurrogateCode.Text != "")
                            {
                                LoadPatientHeader();
                            }
                        }
                    };
                    msgWin.Show();
                }
                cmdGenerate.Visibility = Visibility.Collapsed;
            }        
        }

        #endregion

        #region Luteal Comments Add/Update

        private void cmdEmbryologySave_Click(object sender, RoutedEventArgs e)
        {
            string msgTitle = "Palash";
            string msgText = "Are you sure you want to save Luteal Comment Details";
            MessageBoxControl.MessageBoxChildWindow msgWin =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

            msgWin.OnMessageBoxClosed += (result) =>
            {
                if (result == MessageBoxResult.Yes)
                {
                    saveUpdateGeneralDetails(((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).ID, (int)TherapyTabs.LutealComments, 0, "Luteal Comment Updated Successfully",0,0);
                }
            };
            msgWin.Show();        
        }

        #endregion

        #region OutCome Add/Update
        private void cmdOutComeSave_Click(object sender, RoutedEventArgs e)
        {
                bool saveDtls = true;
                saveDtls = validate();
                if (saveDtls == true)
                {
                    string msgTitle = "Palash";
                    string msgText = "Are you sure you want to save OutCome Details";
                    MessageBoxControl.MessageBoxChildWindow msgWin =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWin.OnMessageBoxClosed += (result) =>
                    {
                        if (result == MessageBoxResult.Yes)
                        {
                            saveUpdateGeneralDetails(((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).ID, (int)TherapyTabs.Outcome, 0, "OutCome Updated Successfully",0,0);
                        }
                    };
                    msgWin.Show();
            }
        }
        #endregion

        private bool validate()
        {
            bool result = true;
            if (rdoPregnancyAchievedYes.IsChecked == true)
            {
                //if(dtpPregnancy!=null)
                if (string.IsNullOrEmpty(dtpPregnancy.Text))
                {
                    dtpPregnancy.SetValidation("Pregnancy Confirm Date is required");
                    dtpPregnancy.RaiseValidationError();
                    dtpPregnancy.Focus();
                    result = false;
                }
            }
            else
            {
                dtpPregnancy.ClearValidationError();
            }
                  return result;
        }

        #region Add/Update/View/Delete Document Events

        private void hpyrlinkFileView_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((PalashDynamics.ValueObjects.IVFPlanTherapy.clsTherapyDocumentsVO)dgDocumentGrid.SelectedItem).AttachedFileName))
            {
                if (((PalashDynamics.ValueObjects.IVFPlanTherapy.clsTherapyDocumentsVO)dgDocumentGrid.SelectedItem).AttachedFileContent != null)
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + ((PalashDynamics.ValueObjects.IVFPlanTherapy.clsTherapyDocumentsVO)dgDocumentGrid.SelectedItem).AttachedFileName });
                            AttachedFileNameList.Add(((PalashDynamics.ValueObjects.IVFPlanTherapy.clsTherapyDocumentsVO)dgDocumentGrid.SelectedItem).AttachedFileName);
                        }
                    };
                    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", ((PalashDynamics.ValueObjects.IVFPlanTherapy.clsTherapyDocumentsVO)dgDocumentGrid.SelectedItem).AttachedFileName, ((PalashDynamics.ValueObjects.IVFPlanTherapy.clsTherapyDocumentsVO)dgDocumentGrid.SelectedItem).AttachedFileContent);
                }
            }
        }

        private void cmdAttachedDoc_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() == true)
            {
                AttachedFileName= openDialog.File.Name;
                txtFileName.Text = openDialog.File.Name;
                try
                {
                    using (Stream stream = openDialog.File.OpenRead())
                    {
                        AttachedFileContents = new byte[stream.Length];
                        stream.Read(AttachedFileContents, 0, (int)stream.Length);
                    }
                }
                catch (Exception ex)
                {
                    string msgText = "Error while reading file.";
                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow("Palsh", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                    throw ex;
                }
            }
        }

        private void cmdDeleteDoc_Click(object sender, RoutedEventArgs e)
        {
            string msgTitle = "Palash";
            string msgText = "Are you sure you want to Delete this Document ?";
            MessageBoxControl.MessageBoxChildWindow msgWin =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgWin.OnMessageBoxClosed += (result) =>
            {
                if (result == MessageBoxResult.Yes)
                {
                    saveUpdateGeneralDetails(((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).ID, (int)TherapyTabs.Documents, ((PalashDynamics.ValueObjects.IVFPlanTherapy.clsTherapyDocumentsVO)dgDocumentGrid.SelectedItem).ID, "Therapy Document Deleted Successfully", 0, 0);
                }
                else
                {
                    txtTitle.Text = "";
                    txtDescription.Text = "";
                    txtFileName.Text = "";
                    AttachedFileName = "";
                }
            };
            msgWin.Show();            
        }

        private void CmdAddDocument_Click(object sender, RoutedEventArgs e)
        {
            if (txtTitle.Text.Length == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Title", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
            }
            else if (txtDescription.Text.Length == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Description", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();                
            }
            else if (txtFileName.Text.Length == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Browse File", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
            }
            else
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to save Therapy Document";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += (result) =>
                {
                    if (result == MessageBoxResult.Yes)
                    {
                        saveUpdateGeneralDetails(((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).ID,(int) TherapyTabs.Documents, 0, "Therapy Document Saved Successfully",0,0);
                        txtTitle.Text = "";
                        txtDescription.Text = "";
                        txtFileName.Text = "";
                    }
                    else
                    {
                        txtTitle.Text = "";
                        txtDescription.Text = "";
                        txtFileName.Text = "";
                    }
                };
                msgWin.Show();
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
            DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
            client.GlobalDeleteFileCompleted += (s1, args1) =>
            {
                if (args1.Error == null)
                {
                }
            };
            client.GlobalDeleteFileAsync("../UserUploadedFilesByTemplateTool", AttachedFileNameList);
        }

        #endregion

        #region Cancel Button Event
        private void cmdOutComeCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grdBackPanel.DataContext = new clsPlanTherapyVO();
                objAnimation.Invoke(RotationType.Backward);
                setupPage(0,0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

            #endregion

        #region Therapy View Event
        private void cmdViewtherapy_Click(object sender, RoutedEventArgs e)
        {
            SelectedTherapyDetails.TherapyDetails = new clsPlanTherapyVO();
            if ((((clsPlanTherapyVO)dgTherpyRptList.SelectedItem)).IsClosed)
            {
                cmdSaveGeneral.IsEnabled = false;               
                cmdOutComeSave.IsEnabled = false;
                cmdAdd.IsEnabled = false;
                CmdAddDocument.IsEnabled = false;
                cmdDrugfemale.IsEnabled = false;
                cmdDrugSurrogate.IsEnabled = false;
                cmdSave.IsEnabled = false;
                cmdSaveDelivery.IsEnabled = false;
                cmdEmbryologySave.IsEnabled = false;
                CmdBrowse.IsEnabled = false;
            }
            else
            {
                cmdSaveGeneral.IsEnabled = true;              
                cmdOutComeSave.IsEnabled = true;
                cmdAdd.IsEnabled = true;
                CmdAddDocument.IsEnabled = true;
                cmdDrugfemale.IsEnabled = true;
                cmdDrugSurrogate.IsEnabled = true;
                cmdSave.IsEnabled = true;
                cmdSaveDelivery.IsEnabled = true;
                cmdEmbryologySave.IsEnabled = true;
                CmdBrowse.IsEnabled = true;
            }
            cmdGenerate.Visibility = Visibility.Collapsed;
            cmdSaveGeneral.Visibility = Visibility.Visible;
            tabControl2.SelectedIndex = 0;
            dtthreapystartdate.IsEnabled = false;
            clsPlanTherapyVO TherapyDetails = new clsPlanTherapyVO();
            
            #region Set Tab When Therapy is Generate

            TabEmbrology.Visibility = Visibility.Visible;
            TabDocuments.Visibility = Visibility.Visible;
            TabFolM.Visibility = Visibility.Visible;
            TabOutcome.Visibility = Visibility.Visible;
            ExeCalander.Visibility = Visibility.Visible;
            TabANCVisit.Visibility = Visibility.Visible;
            TabDelieveryDetails.Visibility = Visibility.Visible;
            
            #endregion

            #region Set Combo Box Values

            if (dgTherpyRptList.SelectedItem !=null)
            {
                grdBackPanel.DataContext = ((clsPlanTherapyVO)dgTherpyRptList.SelectedItem);
                cmbPhysician.SelectedValue = (long)((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).PhysicianId;
                cmbMainIndication.SelectedValue = (long)((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).MainInductionID;
                cmbPlannedTreatment.SelectedValue = (long)((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).PlannedTreatmentID;
                cmbProtocol.SelectedValue = (long)((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).ProtocolTypeID;
                cmbSimulation.SelectedValue = (long)((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).ExternalSimulationID;
                cmbSpermCollection.SelectedValue = (long)((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).PlannedSpermCollectionID;
                txtSurrogateCode.Text = (string)((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).SurrogateMRNo;
                
                //By Anjal...........On 21/01/2014
                

                cmbBabyType.SelectedValue = (long)((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).BabyTypeID;
                txtOHSSremark.Text = Convert.ToString(TherapyDetails.OHSSRemark);
                cmbBO6month.SelectedValue = (long)((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).SIXmonthFitnessID;
                txtBO6month.Text = Convert.ToString(TherapyDetails.SIXmonthFitnessRemark);
                cmbBO1yr.SelectedValue = (long)((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).ONEyFitnessID;
                txtBO1Year.Text = Convert.ToString(TherapyDetails.ONEyFitnessRemark);
                cmbBO5yr.SelectedValue = (long)((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).FIVEyFitnessID;
                txtBO5Year.Text = Convert.ToString(TherapyDetails.FIVEyFitnessRemark);
                cmbBO10yr.SelectedValue = (long)((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).TENyFitnessID;
                txtBO10Year.Text = Convert.ToString(TherapyDetails.TENyFitnessRemark);
                cmbBO20yr.SelectedValue = (long)((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).TWENTYyFitnessID;
                txtBO20Year.Text = Convert.ToString(TherapyDetails.TWENTYyFitnessRemark);

                cmbBO6month_m.SelectedValue = (long)((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).SIXmonthFitnessID_m;
                txtBO6month_m.Text = Convert.ToString(TherapyDetails.SIXmonthFitnessRemark_m);
                cmbBO1yr_m.SelectedValue = (long)((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).ONEyFitnessID_m;
                txtBO1Year_m.Text = Convert.ToString(TherapyDetails.ONEyFitnessRemark_m);
                cmbBO5yr_m.SelectedValue = (long)((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).FIVEyFitnessID_m;
                txtBO5Year_m.Text = Convert.ToString(TherapyDetails.FIVEyFitnessRemark_m);
                cmbBO10yr_m.SelectedValue = (long)((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).TENyFitnessID_m;
                txtBO10Year_m.Text = Convert.ToString(TherapyDetails.TENyFitnessRemark_m);
                cmbBO20yr_m.SelectedValue = (long)((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).TWENTYyFitnessID_m;
                txtBO20Year_m.Text = Convert.ToString(TherapyDetails.TWENTYyFitnessRemark_m);


                SurrogateNo = ((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).SurrogateID;
               
            }

            #endregion
            #region Set All Tabs
            SelectedTherapyDetails.TherapyDetails = new clsPlanTherapyVO();
            SelectedTherapyDetails.TherapyDetails = ((clsPlanTherapyVO)dgTherpyRptList.SelectedItem);
            TherapyID = ((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).ID;
            setupPage(((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).ID, 0);//Fill Document Data Grid                
            #endregion

             #region Set Document Data Grid
            SelectedTherapyDetails.TherapyDetails = new clsPlanTherapyVO();
            SelectedTherapyDetails.TherapyDetails = ((clsPlanTherapyVO)dgTherpyRptList.SelectedItem);            
            #endregion
            if (dtthreapystartdate.SelectedDate != null)
            {
                dtStartDate.DisplayDateStart = dtthreapystartdate.SelectedDate;
                dtStartDate.DisplayDateEnd = dtthreapystartdate.SelectedDate;
                dtEndDate.DisplayDateStart = dtthreapystartdate.SelectedDate;
                dtEndDate.DisplayDateEnd = dtthreapystartdate.SelectedDate.Value.AddDays(60);
            }
            if (txtSurrogateCode.Text != null && txtSurrogateCode.Text != "")
            {
                LoadPatientHeader();
            }
            try
            {
                objAnimation.Invoke(RotationType.Forward);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Add New Therapy 
        private void cmdPlanTherapy_Click(object sender, RoutedEventArgs e)
        {
            if (a == true)
            //if ((clsPlanTherapyVO)dgTherpyRptList.SelectedItem != null &&  ((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).IsClosed == false)
            {
                MessageBoxControl.MessageBoxChildWindow msgW2 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Close The Previous Therapy ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW2.Show();
            }
            else
            {
                cmdGenerate.Visibility = Visibility.Visible;
                cmdSaveGeneral.Visibility = Visibility.Collapsed;

                grdBackPanel.DataContext = new clsPlanTherapyVO();
                dtthreapystartdate.IsEnabled = true;
                dtStartDate.IsEnabled = false;
                dtEndDate.IsEnabled = false;
                
                #region Set Tab When Therapy is Generate
                TabEmbrology.Visibility = Visibility.Collapsed;
                TabDocuments.Visibility = Visibility.Collapsed;
                TabFolM.Visibility = Visibility.Collapsed;
                TabOutcome.Visibility = Visibility.Collapsed;
                ExeCalander.Visibility = Visibility.Collapsed;
                GeneralDetail.Visibility = Visibility.Visible;
                TabANCVisit.Visibility = Visibility.Collapsed;
                TabDelieveryDetails.Visibility = Visibility.Collapsed;
                tabControl2.SelectedIndex = 0;
                #endregion

                #region All Combo Boxes set Default Value
                cmbPhysician.SelectedValue = (long)0;
                cmbMainIndication.SelectedValue = (long)0;
                cmbPlannedTreatment.SelectedValue = (long)0;
                cmbProtocol.SelectedValue = (long)0;
                cmbSimulation.SelectedValue = (long)0;
                cmbSpermCollection.SelectedValue = (long)0;
                #endregion

                #region Document Tab
                txtTitle.Text = "";
                txtDescription.Text = "";
                txtFileName.Text = "";
                #endregion
                try
                {
                    objAnimation.Invoke(RotationType.Forward);
                }
                catch (Exception)
                {
                    throw;
                }
            }
                
            }
            
        #endregion

        #region Change Partner
        private void changeMalePatient_Click(object sender, RoutedEventArgs e)
        {
            ChangePartner partner = new ChangePartner();
            partner.Show();
            partner.OnSaveButton_Click += new RoutedEventHandler(partner_OnSaveButton_Click);
        }

        void partner_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            CoupleDetails = ((ChangePartner)sender).CoupleDetails;

            getEMRDetails(CoupleDetails.MalePatient, "M");
            getEMRDetails(CoupleDetails.FemalePatient, "F");
            setupPage(0,0);//Fill Therpy List
            try
            {
                objAnimation.Invoke(RotationType.Backward);
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region Add Drug

        private void cmdNewExeDrug_Click(object sender, RoutedEventArgs e)
        {
            isDrugForSurrogate = false;
            AddDrug addDrug = new AddDrug();
            addDrug.IsEdit = false;
            addDrug.TherpayExeId = 0;
            addDrug.TherapyStartDate = SelectedTherapyDetails.TherapyDetails.TherapyStartDate;
            addDrug.DrugId = 0;
            addDrug.OnSaveButton_Click+=new RoutedEventHandler(addDrug_OnSaveButton_Click);           
            addDrug.Show();
            
        }
        
        public void addDrug_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (((AddDrug)sender).IsEdit == false)
            {
                string kEY = ((AddDrug)sender).txtForDays.Text.Trim();
                string VALUE = ((AddDrug)sender).txtDosage.Text.Trim();
                clsAddPlanTherapyBizActionVO BizAction = new clsAddPlanTherapyBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                BizAction.TherapyDetails = new clsPlanTherapyVO();
                BizAction.TherapyDetails.ID = SelectedTherapyDetails.TherapyDetails.ID;
                BizAction.TherapyDetails.UnitID = SelectedTherapyDetails.TherapyDetails.UnitID;
                BizAction.TherapyDetails.IsSurrogateDrug = isDrugForSurrogate;
                BizAction.TherapyDetails.IsSurrogateCalendar = isDrugForSurrogate;
                BizAction.TherapyDetails.ThreapyExecutionId = 0;
                BizAction.TherapyDetails.SurrogateExecutionId = 0;
                
                BizAction.TherapyDetails.TherapyExeTypeID = (int)TherapyGroup.Drug;
                BizAction.TherapyDetails.TherapyStartDate = SelectedTherapyDetails.TherapyDetails.TherapyStartDate;
                BizAction.TherapyDetails.PhysicianId = SelectedTherapyDetails.TherapyDetails.PhysicianId;
                BizAction.TherapyDetails.TabID = (int)TherapyTabs.Execution;
                BizAction.TherapyDetails.Day = kEY;
                BizAction.TherapyDetails.Value = VALUE;
                BizAction.TherapyDetails.DrugTime = ((AddDrug)sender).dtthreapystartdate.SelectedDate.Value;
                BizAction.TherapyDetails.DrugTime = BizAction.TherapyDetails.DrugTime.Value.Add(((AddDrug)sender).txtTime.Value.Value.TimeOfDay);
                BizAction.TherapyDetails.DrugNotes = ((AddDrug)sender).txtDrugNotes.Text.Trim();
                BizAction.TherapyDetails.ThearpyTypeDetailId = ((MasterListItem)((AddDrug)sender).CmbDrug.SelectedItem).ID;
                #region Service Call (Check Validation)

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        setupPage(((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).ID, (int)TherapyTabs.Execution);
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Drug Added Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                #endregion
            }
            else if (((AddDrug)sender).IsEdit == true)
            {
                string kEY = ((AddDrug)sender).txtForDays.Text.Trim();
                string VALUE = ((AddDrug)sender).txtDosage.Text.Trim();
                clsAddPlanTherapyBizActionVO BizAction = new clsAddPlanTherapyBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                BizAction.TherapyDetails = new clsPlanTherapyVO();
                BizAction.TherapyDetails.ID = SelectedTherapyDetails.TherapyDetails.ID;
                BizAction.TherapyDetails.UnitID = SelectedTherapyDetails.TherapyDetails.UnitID;
                BizAction.TherapyDetails.IsSurrogateCalendar = isDrugForSurrogate;

                //BizAction.TherapyDetails.ThreapyExecutionId = ((AddDrug)sender).TherpayExeId;
                BizAction.TherapyDetails.SurrogateExecutionId = ((AddDrug)sender).TherpayExeId;
                BizAction.TherapyDetails.TherapyExeTypeID = (int)TherapyGroup.Drug;
                BizAction.TherapyDetails.TherapyStartDate =  SelectedTherapyDetails.TherapyDetails.TherapyStartDate;
                BizAction.TherapyDetails.PhysicianId = SelectedTherapyDetails.TherapyDetails.PhysicianId;
                BizAction.TherapyDetails.TabID = (int)TherapyTabs.Execution;
                BizAction.TherapyDetails.Day = kEY;
                BizAction.TherapyDetails.Value = VALUE;

                BizAction.TherapyDetails.IsSurrogateDrug = isDrugForSurrogate;
                BizAction.TherapyDetails.DrugTime = ((AddDrug)sender).dtthreapystartdate.SelectedDate.Value.Date;
                BizAction.TherapyDetails.DrugTime = BizAction.TherapyDetails.DrugTime.Value.Add(((AddDrug)sender).txtTime.Value.Value.TimeOfDay);
                BizAction.TherapyDetails.DrugNotes = ((AddDrug)sender).txtDrugNotes.Text.Trim();
                BizAction.TherapyDetails.ThearpyTypeDetailId = ((MasterListItem)((AddDrug)sender).CmbDrug.SelectedItem).ID;
                #region Service Call (Check Validation)

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    setupPage(((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).ID, (int)TherapyTabs.Execution);
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Drug Added Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();  
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
            #endregion
            }
        }

        #endregion

        #region Click Event      

        private void ExeGrid_CurrentCellChanged(object sender, EventArgs e)
        {
        }

        private void ExeGrid_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
        }

        private void ExeGrid_RowEditEnded(object sender, DataGridRowEditEndedEventArgs e)
        {        }

        private void ExeGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
        }

        private void cmdDrugChart_Click(object sender, RoutedEventArgs e)
        {
        }

        private void cmdFollicularMonitoring_Click(object sender, RoutedEventArgs e)
        {
        }
        private void ExeGrid1_CurrentCellChanged(object sender, EventArgs e)
        {
        }
        private void ExeGrid1_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
        }
        private void ExeGrid1_RowEditEnded(object sender, DataGridRowEditEndedEventArgs e)
        {
        }
        private void ExeGrid1_LoadingRow(object sender, DataGridRowEventArgs e)
        {
        }
        #endregion

        #region Data Grid Selection Changed

        private void dgTherpyRptList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {           
        }
        #endregion

        #region Therapy Execution

        private void InitExecutionGrid()
        {
            if (ExeGrid.Columns.Count > 1)
            {
                while (ExeGrid.Columns.Count != 1)
                {
                    ExeGrid.Columns.RemoveAt(1);
                }
            }          
            for (int i = 0; i < 60; i++)
            {
                DataGridTemplateColumn column = new DataGridTemplateColumn();
                //column.Width = new DataGridLength(70);
                column.CellTemplate = CreateExe("Day" + (i + 1).ToString());
                DateTime? drsd = ((clsGetTherapyListBizActionVO)this.DataContext).TherapyDetails.TherapyStartDate;
                column.Header = drsd.HasValue ? drsd.Value.AddDays(i).ToString("dd-MMM-yy") : (i + 1).ToString();
                ExeGrid.Columns.Add(column);              
            }
        }

        private DataTemplate Create(string BindingExp, bool IsReadOnly)
        {
            string ReadOnly = IsReadOnly ? "IsReadOnly='False'" : "IsReadOnly='True'";
            string Enabled = IsReadOnly ? "IsEnabled='True'" : "IsEnabled='False'";
            DataTemplate dt = null;
            string xaml = @"<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' xmlns:Converter=""clr-namespace:IVF.Morpheus.WebClient;assembly=IVF.Morpheus.WebClient"">"
            + "<Converter:ExtendedGrid>"
            + "<Converter:ExtendedGrid.Resources>"
            + "<Converter:VisibilityConverter  x:Key='VisibilityConverter'></Converter:VisibilityConverter>"
            + "<Converter:StringToBoolConverter  x:Key='StringToBoolConverter'></Converter:StringToBoolConverter>"
            + "<Converter:TherapyToTimeConverter  x:Key='TherapyToTimeConverter'></Converter:TherapyToTimeConverter>"
            + "<Converter:TherapyToDateTimeConverter  x:Key='TherapyToDateTimeConverter'></Converter:TherapyToDateTimeConverter>"
            + "<Converter:TherapyToVisibilityConverter  x:Key='TherapyToVisibilityConverter'></Converter:TherapyToVisibilityConverter>"
            + "</Converter:ExtendedGrid.Resources>"
            + @"<TextBlock  Text='" + BindingExp + "' Visibility='Collapsed'/>"
            + "<TextBox " + ReadOnly + " Text='{Binding " + BindingExp + ",Mode=TwoWay}' MaxLength='6' Visibility='{Binding IsText,Converter={StaticResource VisibilityConverter}}' VerticalAlignment='Center' Width='40'><ToolTipService.ToolTip><ToolTip  DataContext='{Binding CurrentTherapy,Converter={StaticResource TherapyToDateTimeConverter},ConverterParameter=" + BindingExp + "}' Content='{Binding TimeOfDay,Converter={StaticResource TherapyToTimeConverter}}'></ToolTip></ToolTipService.ToolTip></TextBox>"
            + "<CheckBox " + Enabled + " IsChecked='{Binding " + BindingExp + ",Mode=TwoWay, Converter={StaticResource StringToBoolConverter}}' Visibility='{Binding IsBool,Converter={StaticResource VisibilityConverter}}'  VerticalAlignment='Center' HorizontalAlignment='Center' ></CheckBox>"
            + "</Converter:ExtendedGrid>"
        + "</DataTemplate>";
            dt = (DataTemplate)XamlReader.Load(xaml);
            return dt;
        }

        private DataTemplate CreateExe(string BindingExp)
        {
            DataTemplate dt = null;
          
            string xaml = @"<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' xmlns:Converter=""clr-namespace:PalashDynamics.IVF;assembly=PalashDynamics.IVF"">"
           + "<Converter:ExeExtendedGrid>"
           + "<Converter:ExeExtendedGrid.Resources>"
           + "<Converter:VisibilityConverter  x:Key='VisibilityConverter'></Converter:VisibilityConverter>"
           + "<Converter:StringToBoolConverter  x:Key='StringToBoolConverter'></Converter:StringToBoolConverter>"
           + "<Converter:TherapyToTimeConverter  x:Key='TherapyToTimeConverter'></Converter:TherapyToTimeConverter>"
           + "<Converter:TherapyToDateTimeConverter  x:Key='TherapyToDateTimeConverter'></Converter:TherapyToDateTimeConverter>"
           + "<Converter:ToggleBooleanValueConverter  x:Key='ToggleBooleanValueConverter'></Converter:ToggleBooleanValueConverter>"
           + "<Converter:TherapyToVisibilityConverter  x:Key='TherapyToVisibilityConverter'></Converter:TherapyToVisibilityConverter>"
           + "<Converter:NumberToBrushConverter  x:Key='NumberToBrushConverter' Id='{Binding Id}'></Converter:NumberToBrushConverter>"
            + "</Converter:ExeExtendedGrid.Resources>"
           + @"<TextBlock  Text='" + BindingExp + "' Visibility='Collapsed'/>"
            + "<TextBox Background='{Binding CurrentTherapy, Converter={StaticResource NumberToBrushConverter},ConverterParameter=" + BindingExp + "}' Text='{Binding " + BindingExp + ",Mode=TwoWay}' Visibility='{Binding IsText,Converter={StaticResource VisibilityConverter}}' VerticalAlignment='Center' MaxLength='6'><ToolTipService.ToolTip><ToolTip  DataContext='{Binding CurrentTherapy,Converter={StaticResource TherapyToDateTimeConverter},ConverterParameter=" + BindingExp + "}' Content='{Binding TimeOfDay,Converter={StaticResource TherapyToTimeConverter}}'></ToolTip></ToolTipService.ToolTip></TextBox>"
               + "<CheckBox  IsChecked='{Binding " + BindingExp + ",Mode=TwoWay,Converter={StaticResource StringToBoolConverter}}' Visibility='{Binding IsBool,Converter={StaticResource VisibilityConverter}}' IsEnabled='{Binding IsBool,Converter={StaticResource ToggleBooleanValueConverter}}' VerticalAlignment='Center' HorizontalAlignment='Center'></CheckBox>"
                //+ "<CheckBox  IsChecked='{Binding " + BindingExp + ",Mode=TwoWay,Converter={StaticResource StringToBoolConverter}}' IsEnabled='{Binding "+ BindingExp +",Converter={StaticResource NegateValueConverter}}' VerticalAlignment='Center' HorizontalAlignment='Center'></CheckBox>"
               + "</Converter:ExeExtendedGrid>"
       + "</DataTemplate>";     
            //NumberToBrushConverter
            dt = (DataTemplate)XamlReader.Load(xaml);
            return dt;
        }

        public void BindExecutionGrid()
        {
            try
            {
                if (SelectedTherapyDetails.TherapyExecutionList == null)
                {
                    SelectedTherapyDetails.TherapyExecutionList = new List<clsTherapyExecutionVO>();
                }
                ExeGrid.ItemsSource = null;
                //var list = from n in App.ExecutionTherapyList where (n.IsDeactive != true) select n;
                Execollection = new PagedCollectionView(SelectedTherapyDetails.TherapyExecutionList);
                Execollection.GroupDescriptions.Add(new
                    PropertyGroupDescription("Therapy Group"));
                ExeGrid.ItemsSource = Execollection;             
            }
            catch (Exception ex)
            {
                 throw ex;
            }
        }

       
         public void BindExecutionGridSurrogate()
         {
             try
             {
                 if (SelectedTherapyDetails.TherapyExecutionListSurrogate == null)
                 {
                     SelectedTherapyDetails.TherapyExecutionListSurrogate = new List<clsTherapyExecutionVO>();
                 }                 
                 ExeGrid1.ItemsSource = null;
                 //var list = from n in App.ExecutionTherapyList where (n.IsDeactive != true) select n;
                 Exe1collection = new PagedCollectionView(SelectedTherapyDetails.TherapyExecutionListSurrogate);
                 Exe1collection.GroupDescriptions.Add(new
                     PropertyGroupDescription("Therapy Group"));
                 ExeGrid1.ItemsSource = Exe1collection;
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

        void ExeGrid_KeyUp(object sender, KeyEventArgs e)
        {
            #region   Drug Row Deletion   
            if (e.Key == Key.Delete && e.PlatformKeyCode == 46 && ((clsTherapyExecutionVO)((DataGrid)sender).SelectedItem).ThearpyTypeDetailId!=0)
            {
                string msgTitle = "Palash";
                string msgText = "Are You sure you want to Delete Drug?";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += (result) =>
                {
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            //string kEY = ((TextBlock)((ExeExtendedGrid)((TextBox)(sender)).Parent).Children[0]).Text;//
                            string VALUE = "DeleteAll"; //GetPropertyValue(kEY, (clsTherapyExecutionVO)((TextBox)(sender)).DataContext);
                            clsAddPlanTherapyBizActionVO BizAction = new clsAddPlanTherapyBizActionVO();
                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            BizAction.TherapyDetails = new clsPlanTherapyVO();
                            BizAction.TherapyDetails.ID = ((clsTherapyExecutionVO)((DataGrid)sender).SelectedItem).PlanTherapyId;
                            BizAction.TherapyDetails.UnitID = ((clsTherapyExecutionVO)((DataGrid)sender).SelectedItem).PlanTherapyUnitId;
                            BizAction.TherapyDetails.ThreapyExecutionId = ((clsTherapyExecutionVO)((DataGrid)sender).SelectedItem).ID;
                            // added for surrogacy
                            BizAction.TherapyDetails.SurrogateExecutionId = ((clsTherapyExecutionVO)((DataGrid)sender).SelectedItem).SurrogateExeID;
                            BizAction.TherapyDetails.TherapyExeTypeID = (int)TherapyGroup.Drug;
                            BizAction.TherapyDetails.TherapyStartDate = ((clsTherapyExecutionVO)((DataGrid)sender).SelectedItem).TherapyStartDate;
                            BizAction.TherapyDetails.PhysicianId = ((clsTherapyExecutionVO)((DataGrid)sender).SelectedItem).PhysicianID;
                            BizAction.TherapyDetails.TabID = (int)TherapyTabs.Execution;
                            BizAction.TherapyDetails.Day = "0";
                            BizAction.TherapyDetails.Value = VALUE;
                            BizAction.TherapyDetails.ThearpyTypeDetailId = SelectedTherapyDetails.TherapyDetails.ThearpyTypeDetailId;
                            #region Service Call (Check Validation)

                            client.ProcessCompleted += (s, args) =>
                            {
                                if (args.Error == null && args.Result != null)
                                {
                                    App.TherapyExecution.setupPage(((clsTherapyExecutionVO)((DataGrid)sender).SelectedItem).PlanTherapyId, (int)TherapyTabs.Execution);
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Drug Deleted Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                }
                            };
                            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                            client.CloseAsync();
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                };
                msgWin.Show();

            }
            #endregion
        }

        void ExeGrid1_KeyUp(object sender, KeyEventArgs e)
        {
            #region   Drug Row Deletion
            if (e.Key == Key.Delete && e.PlatformKeyCode == 46 && ((clsTherapyExecutionVO)((DataGrid)sender).SelectedItem).ThearpyTypeDetailId != 0)
            {
                string msgTitle = "Palash";
                string msgText = "Are You Sure \n You Want To Delete Drug?";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += (result) =>
                {
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            //string kEY = ((TextBlock)((ExeExtendedGrid)((TextBox)(sender)).Parent).Children[0]).Text;//
                            string VALUE = "DeleteAll"; //GetPropertyValue(kEY, (clsTherapyExecutionVO)((TextBox)(sender)).DataContext);

                            clsAddPlanTherapyBizActionVO BizAction = new clsAddPlanTherapyBizActionVO();
                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            BizAction.TherapyDetails = new clsPlanTherapyVO();
                            BizAction.TherapyDetails.ID = ((clsTherapyExecutionVO)((DataGrid)sender).SelectedItem).PlanTherapyId;
                            BizAction.TherapyDetails.UnitID = ((clsTherapyExecutionVO)((DataGrid)sender).SelectedItem).PlanTherapyUnitId;
                            BizAction.TherapyDetails.IsSurrogateCalendar = true;
                            BizAction.TherapyDetails.IsSurrogateDrug = true;
                     
                            // added for surrogacy
                            BizAction.TherapyDetails.SurrogateExecutionId = ((clsTherapyExecutionVO)((DataGrid)sender).SelectedItem).ID;                        
                            BizAction.TherapyDetails.TherapyExeTypeID = (int)TherapyGroup.Drug;
                            BizAction.TherapyDetails.TherapyStartDate = ((clsTherapyExecutionVO)((DataGrid)sender).SelectedItem).TherapyStartDate;
                            BizAction.TherapyDetails.PhysicianId = ((clsTherapyExecutionVO)((DataGrid)sender).SelectedItem).PhysicianID;
                            BizAction.TherapyDetails.TabID = (int)TherapyTabs.Execution;
                            BizAction.TherapyDetails.Day = "0";
                            BizAction.TherapyDetails.Value = VALUE;
                            BizAction.TherapyDetails.ThearpyTypeDetailId = SelectedTherapyDetails.TherapyDetails.ThearpyTypeDetailId;
                            #region Service Call (Check Validation)

                            client.ProcessCompleted += (s, args) =>
                            {
                                if (args.Error == null && args.Result != null)
                                {
                                    App.TherapyExecutionSurrogate.setupPage(((clsTherapyExecutionVO)((DataGrid)sender).SelectedItem).PlanTherapyId, (int)TherapyTabs.Execution);
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Drug Deleted Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                }
                            };
                            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                            client.CloseAsync();
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                };
                msgWin.Show();

            }
            #endregion
        }

        #endregion

        #region surrogacy calendar
        private void InitExecutionGridSurrogate()
        {
            if (ExeGrid1.Columns.Count > 1)
            {
                while (ExeGrid1.Columns.Count != 1)
                {
                    ExeGrid1.Columns.RemoveAt(1);
                }
            }
            for (int i = 0; i < 60; i++)
            {
                DataGridTemplateColumn column1 = new DataGridTemplateColumn();
                //column.Width = new DataGridLength(70);                
                column1.CellTemplate = CreateExeSurrogate("Day" + (i + 1).ToString());
                DateTime? drsd1 = ((clsGetTherapyListBizActionVO)this.DataContext).TherapyDetails.TherapyStartDate;
                column1.Header = drsd1.HasValue ? drsd1.Value.AddDays(i).ToString("dd-MMM-yy") : (i + 1).ToString();
                ExeGrid1.Columns.Add(column1);
            }
        }

        private DataTemplate CreateSurrogate(string BindingExpSurrogate, bool IsReadOnly)
        {
            string ReadOnly = IsReadOnly ? "IsReadOnly='False'" : "IsReadOnly='True'";
            string Enabled = IsReadOnly ? "IsEnabled='True'" : "IsEnabled='False'";
            DataTemplate dt = null;
            string xaml = @"<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' xmlns:Converter=""clr-namespace:IVF.Morpheus.WebClient;assembly=IVF.Morpheus.WebClient"">"
            + "<Converter:ExtendedGridSurrogate>"
            + "<Converter:ExtendedGridSurrogate.Resources>"
            + "<Converter:VisibilityConverter  x:Key='VisibilityConverter'></Converter:VisibilityConverter>"
            + "<Converter:StringToBoolConverter  x:Key='StringToBoolConverter'></Converter:StringToBoolConverter>"
            + "<Converter:TherapyToTimeConverter  x:Key='TherapyToTimeConverter'></Converter:TherapyToTimeConverter>"
            + "<Converter:TherapyToDateTimeConverter  x:Key='TherapyToDateTimeConverter'></Converter:TherapyToDateTimeConverter>"
            + "<Converter:TherapyToVisibilityConverter  x:Key='TherapyToVisibilityConverter'></Converter:TherapyToVisibilityConverter>"
            + "</Converter:ExtendedGridSurrogate.Resources>"
            + @"<TextBlock  Text='" + BindingExpSurrogate + "' Visibility='Collapsed'/>"
            + "<TextBox " + ReadOnly + " Text='{Binding " + BindingExpSurrogate + ",Mode=TwoWay}' MaxLength='6' Visibility='{Binding IsText,Converter={StaticResource VisibilityConverter}}' VerticalAlignment='Center' Width='40'><ToolTipService.ToolTip><ToolTip  DataContext='{Binding CurrentTherapy,Converter={StaticResource TherapyToDateTimeConverter},ConverterParameter=" + BindingExpSurrogate + "}' Content='{Binding TimeOfDay,Converter={StaticResource TherapyToTimeConverter}}'></ToolTip></ToolTipService.ToolTip></TextBox>"
            + "<CheckBox " + Enabled + " IsChecked='{Binding " + BindingExpSurrogate + ",Mode=TwoWay, Converter={StaticResource StringToBoolConverter}}' Visibility='{Binding IsBool,Converter={StaticResource VisibilityConverter}}'  VerticalAlignment='Center' HorizontalAlignment='Center' ></CheckBox>"
            + "</Converter:ExtendedGridSurrogate>"
        + "</DataTemplate>";
            dt = (DataTemplate)XamlReader.Load(xaml);
            return dt;
        }
        
        private DataTemplate CreateExeSurrogate(string BindingExpSurrogate)
        {
            DataTemplate dt = null;
            string xaml = @"<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' xmlns:Converter=""clr-namespace:PalashDynamics.IVF;assembly=PalashDynamics.IVF"">"
              + "<Converter:ExtendedGridSurrogate>"
              + "<Converter:ExtendedGridSurrogate.Resources>"
              + "<Converter:VisibilityConverter  x:Key='VisibilityConverter'></Converter:VisibilityConverter>"
              + "<Converter:StringToBoolConverter  x:Key='StringToBoolConverter'></Converter:StringToBoolConverter>"
              + "<Converter:TherapyToTimeConverter  x:Key='TherapyToTimeConverter'></Converter:TherapyToTimeConverter>"
              + "<Converter:TherapyToDateTimeConverter  x:Key='TherapyToDateTimeConverter'></Converter:TherapyToDateTimeConverter>"
              + "<Converter:ToggleBooleanValueConverter  x:Key='ToggleBooleanValueConverter'></Converter:ToggleBooleanValueConverter>"
              + "<Converter:TherapyToVisibilityConverter  x:Key='TherapyToVisibilityConverter'></Converter:TherapyToVisibilityConverter>"
              + "<Converter:NumberToBrushConverter  x:Key='NumberToBrushConverter' Id='{Binding Id}'></Converter:NumberToBrushConverter>"
               + "</Converter:ExtendedGridSurrogate.Resources>"
              + @"<TextBlock  Text='" + BindingExpSurrogate + "' Visibility='Collapsed'/>"
               + "<TextBox Background='{Binding CurrentTherapy, Converter={StaticResource NumberToBrushConverter},ConverterParameter=" + BindingExpSurrogate + "}' Text='{Binding " + BindingExpSurrogate + ",Mode=TwoWay}' Visibility='{Binding IsText,Converter={StaticResource VisibilityConverter}}' VerticalAlignment='Center' MaxLength='6'><ToolTipService.ToolTip><ToolTip  DataContext='{Binding CurrentTherapy,Converter={StaticResource TherapyToDateTimeConverter},ConverterParameter=" + BindingExpSurrogate + "}' Content='{Binding TimeOfDay,Converter={StaticResource TherapyToTimeConverter}}'></ToolTip></ToolTipService.ToolTip></TextBox>"
                  + "<CheckBox  IsChecked='{Binding " + BindingExpSurrogate + ",Mode=TwoWay,Converter={StaticResource StringToBoolConverter}}' Visibility='{Binding IsBool,Converter={StaticResource VisibilityConverter}}' IsEnabled='{Binding IsBool,Converter={StaticResource ToggleBooleanValueConverter}}' VerticalAlignment='Center' HorizontalAlignment='Center'></CheckBox>"
                //+ "<CheckBox  IsChecked='{Binding " + BindingExp + ",Mode=TwoWay,Converter={StaticResource StringToBoolConverter}}' IsEnabled='{Binding "+ BindingExp +",Converter={StaticResource NegateValueConverter}}' VerticalAlignment='Center' HorizontalAlignment='Center'></CheckBox>"
                  + "</Converter:ExtendedGridSurrogate>"
          + "</DataTemplate>";
            //NumberToBrushConverter
            dt = (DataTemplate)XamlReader.Load(xaml);
            return dt;
        }
        #endregion

        #region TAb Selection Changed Event
        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ( ((TabControl)sender).SelectedIndex == ((int)TherapyTabs.FollicularMonitoring) - 1)
            {
                setupPage(SelectedTherapyDetails.TherapyDetails.ID, ((int)TherapyTabs.FollicularMonitoring));//Fill Document Data Grid
            }           
            else if(((TabControl)sender).SelectedIndex == ((int)TherapyTabs.Deliverydetails)-1)
            {
                setupPage(SelectedTherapyDetails.TherapyDetails.ID, ((int)TherapyTabs.Deliverydetails));
            }
        }
        #endregion

        #region Date Selection Changed Event
        private void dtthreapystartdate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtthreapystartdate.SelectedDate != null)
            {
                dtStartDate.DisplayDateStart = dtthreapystartdate.SelectedDate;
                dtStartDate.DisplayDateEnd = dtthreapystartdate.SelectedDate.Value.AddDays(60);
                dtEndDate.DisplayDateStart = dtthreapystartdate.SelectedDate;
                dtEndDate.DisplayDateEnd = dtthreapystartdate.SelectedDate.Value.AddDays(60);
            }
        }

        private void dtStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtStartDate.SelectedDate != null)
            {
                dtEndDate.DisplayDateStart = dtStartDate.SelectedDate.Value.AddDays(1) ;
                dtEndDate.DisplayDateEnd = dtthreapystartdate.SelectedDate.Value.AddDays(60);
            }
        }

        private void dtEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        #endregion

        #region 

        private void TherpayExeRowDelete_Click(object sender, RoutedEventArgs e)
        {
        }

        private void TherpayExe1RowDelete_Click(object sender, RoutedEventArgs e)
        {
        }

        #endregion

        #region Follicular Monitoring View/Update

        private void FollicularMonitoringView_Click(object sender, RoutedEventArgs e)
        {
            //Call Function to Get Size Details of Follicular Monitoring
            FillFollicularSizeDG(((clsFollicularMonitoring)dgFollicularMonitoring.SelectedItem).ID);
        }

        public void FillFollicularSizeDG(long FollicularID)
        {
            clsGetFollicularMonitoringSizeDetailsBizActionVO BizAction = new clsGetFollicularMonitoringSizeDetailsBizActionVO();
            BizAction.FollicularMonitoringSizeList = new List<clsFollicularMonitoringSizeDetails>();
            BizAction.FollicularID = FollicularID;
            BizAction.UnitID = ((clsFollicularMonitoring)dgFollicularMonitoring.SelectedItem).UnitID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    ((clsFollicularMonitoring)dgFollicularMonitoring.SelectedItem).SizeList = ((clsGetFollicularMonitoringSizeDetailsBizActionVO)arg.Result).FollicularMonitoringSizeList;
                    FollicularMonitoring addFollic = new FollicularMonitoring();
                    addFollic.fillDoctor(((clsFollicularMonitoring)dgFollicularMonitoring.SelectedItem).PhysicianID);
                    addFollic.DataContext = ((clsFollicularMonitoring)dgFollicularMonitoring.SelectedItem);
                    addFollic.AttachedFileContents = ((clsFollicularMonitoring)dgFollicularMonitoring.SelectedItem).AttachmentFileContent;
                    addFollic.dtpFollicularDate.IsEnabled = false;
                    addFollic.OnSaveButton_Click += new RoutedEventHandler(addFollic_OnSaveButton_Click);
                    addFollic.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        bool Flag = false;
        void addFollic_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {            
            try
            {
                wait.Show();
                clsUpdateFollicularMonitoringBizActionVO BizAction = new clsUpdateFollicularMonitoringBizActionVO();
                BizAction.FollicularID = ((clsFollicularMonitoring)dgFollicularMonitoring.SelectedItem).ID;
                BizAction.FollicularMonitoringDetial = ((clsFollicularMonitoring)((FollicularMonitoring)sender).DataContext);
                BizAction.FollicularMonitoringDetial.UnitID = ((clsFollicularMonitoring)dgFollicularMonitoring.SelectedItem).UnitID;
                BizAction.FollicularMonitoringDetial.Date = ((FollicularMonitoring)sender).dtpFollicularDate.SelectedDate.Value.Date;
                BizAction.FollicularMonitoringDetial.Date = BizAction.FollicularMonitoringDetial.Date.Value.Add(((FollicularMonitoring)sender).txtTime.Value.Value.TimeOfDay);
                BizAction.FollicularMonitoringDetial.AttachmentFileContent = ((FollicularMonitoring)sender).AttachedFileContents;
                BizAction.FollicularMonitoringDetial.PhysicianID = ((MasterListItem)((FollicularMonitoring)sender).cmbPhysician.SelectedItem).ID;
                foreach (clsFollicularMonitoringSizeDetails item in BizAction.FollicularMonitoringDetial.SizeList)
                {
                    if (string.IsNullOrEmpty(BizAction.FollicularMonitoringDetial.FollicularNoList))
                    {
                        BizAction.FollicularMonitoringDetial.FollicularNoList = item.FollicularNumber;
                    }
                    else
                    {
                        BizAction.FollicularMonitoringDetial.FollicularNoList = BizAction.FollicularMonitoringDetial.FollicularNoList + "," + item.FollicularNumber;
                    }
                    if (string.IsNullOrEmpty(BizAction.FollicularMonitoringDetial.LeftSizeList))
                    {
                        BizAction.FollicularMonitoringDetial.LeftSizeList =item.LeftSize;
                    }
                    else
                    {
                        BizAction.FollicularMonitoringDetial.LeftSizeList = BizAction.FollicularMonitoringDetial.LeftSizeList + "," + item.LeftSize;
                    }
                    if (string.IsNullOrEmpty(BizAction.FollicularMonitoringDetial.RightSizeList))
                    {
                        BizAction.FollicularMonitoringDetial.RightSizeList =item.RightSIze;
                   
                    }
                    else
                    {
                        BizAction.FollicularMonitoringDetial.RightSizeList = BizAction.FollicularMonitoringDetial.RightSizeList + "," + item.RightSIze;                   
                    }
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    
                    if (args.Error == null && args.Result != null)
                    {
                        wait.Close();                        
                        setupPage(((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).ID, (int)TherapyTabs.FollicularMonitoring);
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Follicular Monitoring Details Updated Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);                       
                        msgW1.Show();
                    }
                    else
                    {
                        wait.Close();
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
                throw ex;
            }
        }


        private void FollicularMonitoringAttachemntView_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((clsFollicularMonitoring)dgFollicularMonitoring.SelectedItem).AttachmentPath))
            {
                if (((clsFollicularMonitoring)dgFollicularMonitoring.SelectedItem).AttachmentFileContent != null)
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + ((clsFollicularMonitoring)dgFollicularMonitoring.SelectedItem).AttachmentPath });
                            AttachedFileNameList.Add(((clsFollicularMonitoring)dgFollicularMonitoring.SelectedItem).AttachmentPath);
                        }
                    };
                    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", ((clsFollicularMonitoring)dgFollicularMonitoring.SelectedItem).AttachmentPath, ((clsFollicularMonitoring)dgFollicularMonitoring.SelectedItem).AttachmentFileContent);
                }
            }
        }

        #endregion

        private void cmdEvent_Click(object sender, RoutedEventArgs e)
        {
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

                ((IInitiateCIMS)myData).Initiate("VISIT");
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void HyperlinkButton_Click_Male(object sender, RoutedEventArgs e)
        {
            if ((MaleAlert.Text.Trim().Length) > 0)
            {
                frmAttention PatientAlert = new frmAttention();
                PatientAlert.AlertsExpanded = CoupleDetails.MalePatient.Alerts;
                PatientAlert.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Attention Not Entered.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }   
        }
               
        private void HyperlinkButton_Click_Female(object sender, RoutedEventArgs e)
        {
            if (FemaleAlert.Text.Trim().Length > 0)
            {
                frmAttention PatientAlert = new frmAttention();
                PatientAlert.AlertsExpanded = CoupleDetails.FemalePatient.Alerts;
                PatientAlert.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Attention Not Entered.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        private void txtPlannedNoOfEmbryos_TextChanged(object sender, TextChangedEventArgs e)
        {
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

        private void txtPlannedNoOfEmbryos_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }
               
        private void chkPill_Checked(object sender, RoutedEventArgs e)
        {
            dtStartDate.IsEnabled = true;
            dtEndDate.IsEnabled = true;
        }

        private void btnSearchSurrogate_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch Win = new PatientSearch();
            Win.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            Win.PatientCategoryID = 10;
            Win.Show();
        }

        void Win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch ObjWin = (PatientSearch)sender;
            if (ObjWin.DialogResult == true)
            {
                if (ObjWin.PatientCategoryID == 10)
                    txtSurrogateCode.Text = ((clsPatientGeneralVO)ObjWin.dataGrid2.SelectedItem).MRNo;
                SurrogateNo = ((clsPatientGeneralVO)ObjWin.dataGrid2.SelectedItem).PatientID;
            }
        }

        private void CalendarForFemalePatient_Maximized(object sender, EventArgs e)
        {
        }
        private void CalendarForFemalePatient_Minimized(object sender, EventArgs e)
        {
        }
        private void CalendarForSurrogatePatient_Maximized(object sender, EventArgs e)
        {
        }
        private void CalendarForSurrogatePatient_Minimized(object sender, EventArgs e)
        {
        }
        private void chkPill_Unchecked(object sender, RoutedEventArgs e)
        {
            dtStartDate.IsEnabled = false;
            dtEndDate.IsEnabled = false;
        }

        private void cmdSaveDelivery_Click(object sender, RoutedEventArgs e)
        {
            if (dtdeliverydate.SelectedDate == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Delivery Date", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
            }
            else if (txtWeight.Text.Length == 0)
            {
                     MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Weight", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
            }

            else if (cmbBaby.SelectedItem == null && cmbBaby.SelectedItem == "")
            {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Baby", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
            }
            else if (cmbMode.SelectedItem == null && cmbMode.SelectedItem == "")
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Mode", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
            }
            else
            {
                string msgTitle = "Palash";
                string msgText = "Are You Sure \n You Want To Save Delivery Details";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += (result) =>
                {
                    if (result == MessageBoxResult.Yes)
                    {
                        saveUpdateGeneralDetails(((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).ID, (int)TherapyTabs.Deliverydetails, 0, "Delivery Details Saved Successfully", 0, 0);                
                    }           
                };
                msgWin.Show();
            }
        }
        
        private void cmdCancelDelivery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grdBackPanel.DataContext = new clsPlanTherapyVO();
                objAnimation.Invoke(RotationType.Backward);
                setupPage(0, 0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void txtWeight_TextChanged(object sender, TextChangedEventArgs e)
        {          
            if(!((TextBox)sender).Text.IsValueDouble() && textBefore !=null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }
       
        private void txtWeight_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {            
            try
            {
                if (dgANCList.ItemsSource != null)
                {
                    //obj = (ObservableCollection<clsTherapyANCVO>)dgANCList.ItemsSource;
                    var item1 = from r in ANCList
                                where ((r.ANCDate == dtANCDate.SelectedDate && r.POG == txtPOG.Text && r.Findings == txtFindings.Text && r.USGReproductive == txtUSGReproductive.Text && r.Investigation == txtInvestigation.Text && r.Remarks == txtRemarks.Text))
                                select new clsTherapyANCVO
                                {
                                    ANCDate = r.ANCDate,
                                    POG = r.POG,
                                    Findings = r.Findings,
                                    USGReproductive = r.USGReproductive,
                                    Investigation = r.Investigation,
                                    Remarks = r.Remarks
                                };
                    if (item1.ToList().Count == 0)
                    {
                        GetDetails();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW2 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Data Already Exist", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW2.Show();
                    }
                }
                else
                {
                    GetDetails();
                }
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "ANC Visit  Added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                //}
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void GetDetails()
        {
            clsTherapyANCVO tempDetails = new clsTherapyANCVO();

            tempDetails.ANCDate = dtANCDate.SelectedDate;
            tempDetails.POG = txtPOG.Text;
            tempDetails.Findings = txtFindings.Text;
            tempDetails.USGReproductive = txtUSGReproductive.Text;
            tempDetails.Investigation = txtInvestigation.Text;
            tempDetails.Remarks = txtRemarks.Text;
            tempDetails.TherapyID = TherapyID;
            tempDetails.ThearpyUnitID = TherapyID;

            ANCList.Add(tempDetails);     
            dgANCList.ItemsSource = ANCList;
            dgANCList.UpdateLayout();
            dgANCList.Focus();
         
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grdBackPanel.DataContext = new clsPlanTherapyVO();
                objAnimation.Invoke(RotationType.Backward);
                setupPage(0, 0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
         

            if (dgANCList.ItemsSource != null)
            {
                string msgTitle = "Palash";
                string msgText = "Are You Sure You Want To Save The ANC Visits.";
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                msgW.Show();
            }
            else
            {
                if (ANCList.Count == 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "You Can Not Save ANC Visit Without Details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                saveUpdateGeneralDetails(((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).ID, (int)TherapyTabs.ANCVisit, 0, "ANC Visits Updated Successfully", 0, 0);
            }
        }

        private void cmdDeleteSchedule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgANCList.SelectedItem != null)
                {
                    string msgText = "Are You Sure \n You Want To Delete The Selected ANC Visit ?";
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            ANCList.RemoveAt(dgANCList.SelectedIndex);
                        }
                    };
                    msgWD.Show();
                }
            }
            catch
            {
                throw;
            }
        }

        private void cmdEdit_Click(object sender, RoutedEventArgs e)
        {
            if (ANCList.Count > 0)
            {             
                    int var = dgANCList.SelectedIndex;
                    ANCList.RemoveAt(dgANCList.SelectedIndex);            
                     ANCList.Insert(var, new clsTherapyANCVO
                    {                     
                        ANCDate = dtANCDate.SelectedDate,
                        POG = txtPOG.Text,
                        Findings = txtFindings.Text,
                        USGReproductive = txtUSGReproductive.Text,
                        Investigation = txtInvestigation.Text,
                        Remarks = txtRemarks.Text,
                        ANCID = ANCID
                    }
                    );
                    dgANCList.ItemsSource = ANCList;
                   dgANCList.Focus();
                   dgANCList.UpdateLayout();
                   dgANCList.SelectedIndex = ANCList.Count - 1;
            }
        }

        private void hlbEditDetails_Click(object sender, RoutedEventArgs e)
        {
            if (dgANCList.SelectedItem != null)
            {
                dtANCDate.SelectedDate = ((clsTherapyANCVO)dgANCList.SelectedItem).ANCDate;
                txtPOG.Text = ((clsTherapyANCVO)dgANCList.SelectedItem).POG;
                txtFindings.Text = ((clsTherapyANCVO)dgANCList.SelectedItem).Findings;
                txtUSGReproductive.Text = ((clsTherapyANCVO)dgANCList.SelectedItem).USGReproductive;
                txtInvestigation.Text = ((clsTherapyANCVO)dgANCList.SelectedItem).Investigation;
                txtRemarks.Text = ((clsTherapyANCVO)dgANCList.SelectedItem).Remarks;               
                ANCID = ((clsTherapyANCVO)dgANCList.SelectedItem).ANCID;
            }
        }

        private void ClearData()
        {
            this.DataContext = new clsTherapyANCVO();
            dtANCDate.SelectedDate = null;
            txtPOG.Text = null;
            txtFindings.Text = null;
            txtUSGReproductive.Text = null;
            txtInvestigation.Text = null;
            txtRemarks.Text = null;
            ANCList = new ObservableCollection<clsTherapyANCVO>();
            dgANCList.ItemsSource = null;
        }

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
               
                case "Save":
                   cmdSave.IsEnabled = true;
                    cmdEdit.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;
                case "Edit":
                    cmdSave.IsEnabled = false;
                    cmdEdit.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    break;
                case "Cancel":
                     cmdSave.IsEnabled = false;
                     cmdEdit.IsEnabled = false;
                     cmdCancel.IsEnabled = false;
                    break;
                default:
                    break;
            }
        }

        private void FillModeofDelivery()
        {
            List<MasterListItem> lList = new List<MasterListItem>();
            MasterListItem Default = new MasterListItem(0, "- Select -");
            lList.Insert(0, Default);
            EnumToList(typeof(ModeofDelivery), lList);
            cmbMode.ItemsSource = lList;
            //cmbBaby.SelectedValue = (long)((clsPlanTherapyVO)this.DataContext).Baby;
            cmbMode.SelectedValue = (long)0;
        }

        private void FillBaby()
        {
            List<MasterListItem> lList = new List<MasterListItem>();
            MasterListItem Default = new MasterListItem(0, "- Select -");
            lList.Insert(0, Default);
            EnumToList(typeof(Genders), lList);
            cmbBaby.ItemsSource = lList;
            //cmbBaby.SelectedValue = (long)((clsPlanTherapyVO)this.DataContext).Baby;
            cmbBaby.SelectedValue = (long)0;            
        }

        public static void EnumToList(Type EnumType, List<MasterListItem> TheMasterList)
        {
            Array Values = GetValues(EnumType);
            foreach (int Value in Values)
            {
                string Display = Enum.GetName(EnumType, Value);
                MasterListItem Item = new MasterListItem(Value, Display);
                //if(Value !=0)
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

        private void cmdNewExeDrugSurrogate_Click(object sender, RoutedEventArgs e)
        {
            isDrugForSurrogate = true;
            AddDrug addDrug = new AddDrug();
            addDrug.IsEdit = false;
            addDrug.TherpayExeId = 0;
            addDrug.TherapyStartDate = SelectedTherapyDetails.TherapyDetails.TherapyStartDate;
            addDrug.DrugId = 0;
            addDrug.OnSaveButton_Click += new RoutedEventHandler(addDrug_OnSaveButton_Click);
            addDrug.Show();
        }     

        private void cmdDrugfemale_Click(object sender, RoutedEventArgs e)
        {          
            clsgetTherapyDrugDetailsBizActionVO BizAction = new clsgetTherapyDrugDetailsBizActionVO();
            BizAction.IsFromDelete = true;
            BizAction.TherapyDrugDetails = new clsTherapyDrug();
            BizAction.TherapyDrugDetails.IsSurrogate = false;
            //BizAction.PlanTherapyId = ((clsTherapyExecutionVO)((DataGrid)sender).SelectedItem).PlanTherapyId;
            BizAction.PlanTherapyId = TherapyID;    
            BizAction.TherapyTypeId = (int)TherapyGroup.Drug;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
             client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            DeleteDrug Drug = null;
                            Drug = new DeleteDrug();
                            List<clsTherapyDrug> TempDrugList = new List<clsTherapyDrug>();
                            TempDrugList = ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDetails;
                            if (TempDrugList != null && TempDrugList.Count > 0)
                                Drug.DrugList = TempDrugList;
                            Drug.OnCloseButton_Click += new RoutedEventHandler(cmdDrugFemale1_Click);
                            Drug.Show();
                         }
                    };
                    client.ProcessAsync(BizAction, new clsUserVO());
                    client.CloseAsync();
                }

        void cmdDrugFemale1_Click(object sender, RoutedEventArgs e)
        {

            clsAddPlanTherapyBizActionVO BizAction = new clsAddPlanTherapyBizActionVO();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            BizAction.TherapyDetails = new clsPlanTherapyVO();
            BizAction.TherapyDetails.ID = SelectedTherapyDetails.TherapyDetails.ID;
            BizAction.TherapyDetails.UnitID = SelectedTherapyDetails.TherapyDetails.UnitID;
            BizAction.TherapyDetails.IsSurrogateDrug = isDrugForSurrogate;
            BizAction.TherapyDetails.IsSurrogateCalendar = isDrugForSurrogate;
            BizAction.TherapyDetails.ThreapyExecutionId = 0;
            BizAction.TherapyDetails.SurrogateExecutionId = 0;

            BizAction.TherapyDetails.TherapyExeTypeID = (int)TherapyGroup.Drug;
            BizAction.TherapyDetails.TherapyStartDate = SelectedTherapyDetails.TherapyDetails.TherapyStartDate;
            BizAction.TherapyDetails.PhysicianId = SelectedTherapyDetails.TherapyDetails.PhysicianId;
            BizAction.TherapyDetails.TabID = (int)TherapyTabs.Execution;
            #region Service Call (Check Validation)

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    setupPage(((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).ID, (int)TherapyTabs.Execution);
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Drug Added Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
            #endregion
        }
      
        private void cmdDrugSurrogate_Click(object sender, RoutedEventArgs e)
        {
            clsgetTherapyDrugDetailsBizActionVO BizAction = new clsgetTherapyDrugDetailsBizActionVO();
            BizAction.IsFromDelete = true;
            BizAction.TherapyDrugDetails = new clsTherapyDrug();
            BizAction.TherapyDrugDetails.IsSurrogate =  true;
            BizAction.PlanTherapyId = TherapyID;
            BizAction.TherapyTypeId = (int)TherapyGroup.Drug;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    DeleteDrug Drug = null;
                    Drug = new DeleteDrug();
                    List<clsTherapyDrug> TempDrugList = new List<clsTherapyDrug>();
                    TempDrugList = ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDetails;

                    if (TempDrugList != null && TempDrugList.Count > 0)
                        Drug.DrugList = TempDrugList;
                    Drug.OnCloseButton_Click += new RoutedEventHandler(cmdDrugfemale_Click);
                    Drug.Show();
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void ChkAbortion_Click(object sender, RoutedEventArgs e)
        {
            chkMissed.Visibility = Visibility.Visible;
            chkIncomplete.Visibility = Visibility.Visible;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            chkMissed.Visibility = Visibility.Collapsed;
            chkIncomplete.Visibility = Visibility.Collapsed;
        }
    }
}
