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
using System.Windows.Controls.Primitives;
using PalashDynamics.IVF.TherpyExecution;
using System.Windows.Markup;
using System.Windows.Data;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;
using DataDrivenApplication.Forms;


namespace PalashDynamics.IVF.TherpyExecution
{
    public partial class IVFTherapyExecutionForSurrogacy : UserControl, IInitiateCIMS
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
        ListBox lstIVFBox;
        public bool IsExpendedWindow { get; set; }

        public string Action { get; set; }
        public string ModuleName { get; set; }
        UIElement myData = null;
        public Boolean a;
        public bool IsSaved = false;
        #endregion

        #region Constructor
        public IVFTherapyExecutionForSurrogacy()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(grdFrontPanel, grdBackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            this.Loaded += new RoutedEventHandler(IVFTherapyExecutionForSurrogacy_Loaded);
            SelectedTherapyDetails = new clsGetTherapyListBizActionVO();
            SelectedTherapyDetails.TherapyDetailsList = new List<clsPlanTherapyVO>();
            //App.TherapyExecution = this;

        }
        #endregion

        #region Load Event
        void IVFTherapyExecutionForSurrogacy_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsExpendedWindow)
            {
                try
                {

                    if (IsPatientExist == false)
                    {
                        //((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                        //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                        //((IInitiateCIMS)App.Current).Initiate("VISIT");
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
                        SelectedTherapyDetails.TherapyDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.UnitId;
                        if (((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).PatientID != 0)
                        {
                            if (((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).Gender.Equals("Male"))
                            {
                                changeFemalePatient.Visibility = Visibility.Visible;
                                changeMalePatient.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                changeFemalePatient.Visibility = Visibility.Collapsed;
                                changeMalePatient.Visibility = Visibility.Visible;
                            }
                        }
                    }
                    ExeGrid.KeyUp += new KeyEventHandler(ExeGrid_KeyUp);
                    IsExpendedWindow = true;
                }
                catch (Exception ex)
                {
                }
            }
        }


        #endregion

        #region Setup Page
        public void setupPage(long TherapyID, long TabId)
        {
            try
            {
                wait.Show();
                clsGetTherapyListBizActionVO BizAction = new clsGetTherapyListBizActionVO();
                BizAction.TherapyID = TherapyID;
                BizAction.TabID = TabId;
                if (SelectedTherapyDetails.TherapyDetails.UnitID == 0)
                {
                    BizAction.TherapyUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.UnitId;
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

                                dgDocumentGrid.SelectedIndex = 0;
                                SelectedTherapyDetails.TherapyDocument = ((clsGetTherapyListBizActionVO)args.Result).TherapyDocument;
                                SelectedTherapyDetails.TherapyExecutionList = ((clsGetTherapyListBizActionVO)args.Result).TherapyExecutionList;
                                SelectedTherapyDetails.FollicularMonitoringList = ((clsGetTherapyListBizActionVO)args.Result).FollicularMonitoringList;
                                this.DataContext = null;
                                this.DataContext = SelectedTherapyDetails;
                                InitExecutionGrid();
                                BindExecutionGrid();
                                wait.Close();
                            }
                            else
                            {
                                if (TabId == ((int)TherapyTabs.FollicularMonitoring))
                                {
                                    dgFollicularMonitoring.ItemsSource = null;
                                    dgFollicularMonitoring.ItemsSource = ((clsGetTherapyListBizActionVO)args.Result).FollicularMonitoringList;
                                    SelectedTherapyDetails.FollicularMonitoringList = ((clsGetTherapyListBizActionVO)args.Result).FollicularMonitoringList;
                                    this.DataContext = SelectedTherapyDetails;
                                    InitExecutionGrid();
                                    BindExecutionGrid();
                                    wait.Close();
                                }
                                else if (TabId == ((int)TherapyTabs.Documents))
                                {
                                    dgDocumentGrid.ItemsSource = null;
                                    dgDocumentGrid.ItemsSource = ((clsGetTherapyListBizActionVO)args.Result).TherapyDocument;
                                    SelectedTherapyDetails.TherapyDocument = ((clsGetTherapyListBizActionVO)args.Result).TherapyDocument;
                                    this.DataContext = SelectedTherapyDetails;
                                    InitExecutionGrid();
                                    BindExecutionGrid();
                                    wait.Close();
                                }
                                else if (TabId == ((int)TherapyTabs.Execution))
                                {
                                    SelectedTherapyDetails.TherapyExecutionList = ((clsGetTherapyListBizActionVO)args.Result).TherapyExecutionList;
                                    this.DataContext = SelectedTherapyDetails;
                                    InitExecutionGrid();
                                    BindExecutionGrid();

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
            catch (Exception ex)
            {
                wait.Close();
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

                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                    mElement.Text = " : " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).FirstName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).MiddleName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).LastName;
                    break;



                default:
                    break;
            }

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

                                //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                                //((IInitiateCIMS)App.Current).Initiate("VISIT");

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

                                //getEMRDetails(BizAction.CoupleDetails.FemalePatient, "F");
                                //getEMRDetails(BizAction.CoupleDetails.MalePatient, "M");
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

                            //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                            //((IInitiateCIMS)App.Current).Initiate("VISIT");

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
            catch (Exception ex)
            {
                wait.Close();
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
            }
        }
        #endregion

        #region Fill Combobox
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
            Item.Description = "--Select--";
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

        private void getEMRDetails(clsPatientGeneralVO PatientDetails, string Gender)
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

                    if (BizAction.EMRDetailsList != null || BizAction.EMRDetailsList.Count > 0)
                    {
                        for (int i = 0; i < BizAction.EMRDetailsList.Count; i++)
                        {
                            if (BizAction.EMRDetailsList[i].ControlCaption.Equals("Height"))
                            {
                                if (!string.IsNullOrEmpty(BizAction.EMRDetailsList[i].Value))
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
                                if (!string.IsNullOrEmpty(BizAction.EMRDetailsList[i].Value))
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
                                PatientDetails.BMI = Math.Round(CalculateBMI(height, weight), 2);
                                PatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", PatientDetails.BMI));
                                Female.DataContext = PatientDetails;
                            }
                            else
                            {
                                PatientDetails.Height = height;
                                PatientDetails.Weight = weight;
                                PatientDetails.BMI = Math.Round(CalculateBMI(height, weight), 2);
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
        private double CalculateBMI(Double Height, Double Weight)
        {
            try
            {
                if (Weight == 0)
                {
                    return 0.0;

                }
                else if (Height == 0)
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
            }
        }
        #endregion

        #region Save /Update Data(TabId is used which tab u want to upadate and save(According to TherapyTabs Enum(Like 1 for General Details .......))
        private void saveUpdateGeneralDetails(long ID, long TabID, long DocumentId, String Msg)
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


                        }
                        #endregion

                        if (TabID == 4)//When Document Is Added and Upadated
                        {
                            setupPage(((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).ID, TabID);//Fill Document Data Grid
                        }
                        else if (TabID == (int)TherapyTabs.General)
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
        }

        #endregion


        private void cmdGenerate_Click(object sender, RoutedEventArgs e)
        {
            //if (cmdGenerate.Visibility == Visibility.Visible)
            if(cmdGenerate.Visibility == Visibility.Collapsed)
            {
                if (GenValidation())
                {
                    string msgTitle = "Palash";
                    string msgText = "Are you sure you want to save Therapy General Details";
                    MessageBoxControl.MessageBoxChildWindow msgWin =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWin.OnMessageBoxClosed += (result) =>
                    {
                        if (result == MessageBoxResult.Yes)
                        {
                            saveUpdateGeneralDetails(0, (int)TherapyTabs.General, 0, "Therapy General Details Saved Successfully");
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
                    string msgText = "Are you sure you want to save Therapy General Details";
                    MessageBoxControl.MessageBoxChildWindow msgWin =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWin.OnMessageBoxClosed += (result) =>
                    {
                        if (result == MessageBoxResult.Yes)
                        {
                            saveUpdateGeneralDetails(((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).ID, (int)TherapyTabs.General, 0, "Therapy General Details Updated Successfully");
                        }
                    };
                    msgWin.Show();
                }

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
                    saveUpdateGeneralDetails(((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).ID, (int)TherapyTabs.LutealComments, 0, "Luteal Comment Updated Successfully");
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
                        saveUpdateGeneralDetails(((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).ID, (int)TherapyTabs.Outcome, 0, "OutCome Updated Successfully");
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
                AttachedFileName = openDialog.File.Name;
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
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
            }
        }

        private void cmdDeleteDoc_Click(object sender, RoutedEventArgs e)
        {
            string msgTitle = "Palash";
            string msgText = "Are you sure you want to Delete this Document ?";
            //string msgText = "Are you sure you want to save the Doctor Schedule";
            MessageBoxControl.MessageBoxChildWindow msgWin =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

            msgWin.OnMessageBoxClosed += (result) =>
            {
                if (result == MessageBoxResult.Yes)
                {
                    saveUpdateGeneralDetails(((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).ID, (int)TherapyTabs.Documents, ((PalashDynamics.ValueObjects.IVFPlanTherapy.clsTherapyDocumentsVO)dgDocumentGrid.SelectedItem).ID, "Therapy Document Deleted Successfully");

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
                        saveUpdateGeneralDetails(((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).ID, (int)TherapyTabs.Documents, 0, "Therapy Document Saved Successfully");
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
                setupPage(0, 0);
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



            #endregion

            #region Set Combo Box Values

            if (dgTherpyRptList.SelectedItem != null)
            {
                grdBackPanel.DataContext = ((clsPlanTherapyVO)dgTherpyRptList.SelectedItem);
                cmbPhysician.SelectedValue = (long)((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).PhysicianId;
                cmbMainIndication.SelectedValue = (long)((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).MainInductionID;
                cmbPlannedTreatment.SelectedValue = (long)((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).PlannedTreatmentID;
                cmbProtocol.SelectedValue = (long)((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).ProtocolTypeID;
                cmbSimulation.SelectedValue = (long)((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).ExternalSimulationID;
                cmbSpermCollection.SelectedValue = (long)((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).PlannedSpermCollectionID;
            }

            #endregion

            #region Set All Tabs
            SelectedTherapyDetails.TherapyDetails = new clsPlanTherapyVO();
            SelectedTherapyDetails.TherapyDetails = ((clsPlanTherapyVO)dgTherpyRptList.SelectedItem);
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
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Close the Previous Therapy ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW2.Show();
            }

            else
            {


                //cmdGenerate.Visibility = Visibility.Visible;
                cmdGenerate.Visibility = Visibility.Collapsed;
                cmdSaveGeneral.Visibility = Visibility.Collapsed;

                grdBackPanel.DataContext = new clsPlanTherapyVO();
                dtthreapystartdate.IsEnabled = true;
                #region Set Tab When Therapy is Generate
                TabEmbrology.Visibility = Visibility.Collapsed;
                TabDocuments.Visibility = Visibility.Collapsed;
                TabFolM.Visibility = Visibility.Collapsed;
                TabOutcome.Visibility = Visibility.Collapsed;
                ExeCalander.Visibility = Visibility.Collapsed;
                //GeneralDetail.Visibility = Visibility.Visible;
                GeneralDetail.Visibility = Visibility.Collapsed;
                tabControl2.SelectedIndex = 0;


                #endregion

                #region All Combo Boxes set Default Value
                //cmbPhysician.SelectedValue = (long)0;
                //cmbMainIndication.SelectedValue = (long)0;
                //cmbPlannedTreatment.SelectedValue = (long)0;
                //cmbProtocol.SelectedValue = (long)0;
                //cmbSimulation.SelectedValue = (long)0;
                //cmbSpermCollection.SelectedValue = (long)0;
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
            setupPage(0, 0);//Fill Therpy List
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
            AddDrug addDrug = new AddDrug();
            addDrug.IsEdit = false;
            addDrug.TherpayExeId = 0;
            addDrug.TherapyStartDate = SelectedTherapyDetails.TherapyDetails.TherapyStartDate;
            addDrug.DrugId = 0;
            addDrug.OnSaveButton_Click += new RoutedEventHandler(addDrug_OnSaveButton_Click);

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
                BizAction.TherapyDetails.ThreapyExecutionId = 0;
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
                BizAction.TherapyDetails.ThreapyExecutionId = ((AddDrug)sender).TherpayExeId;
                BizAction.TherapyDetails.TherapyExeTypeID = (int)TherapyGroup.Drug;
                BizAction.TherapyDetails.TherapyStartDate = SelectedTherapyDetails.TherapyDetails.TherapyStartDate;
                BizAction.TherapyDetails.PhysicianId = SelectedTherapyDetails.TherapyDetails.PhysicianId;
                BizAction.TherapyDetails.TabID = (int)TherapyTabs.Execution;
                BizAction.TherapyDetails.Day = kEY;
                BizAction.TherapyDetails.Value = VALUE;

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
        {

        }

        private void ExeGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }

        private void cmdDrugChart_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdFollicularMonitoring_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Data Grid Selection Changed

        private void dgTherpyRptList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //getEMRDetails(FemalePatient.FirstOrDefault(p => p.CoupleRegNo.Equals(((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).CoupleRegNo)), "F");
            //getEMRDetails(MalePatient.FirstOrDefault(p => p.CoupleRegNo.Equals(((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).CoupleRegNo)), "M");
            ////Male.DataContext = MalePatient.FirstOrDefault(p => p.CoupleRegNo.Equals(((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).CoupleRegNo));
            //Female.DataContext = FemalePatient.FirstOrDefault(p => p.CoupleRegNo.Equals(((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).CoupleRegNo));

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
            //    string xaml = @"<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' xmlns:Converter=""clr-namespace:IVF.Morpheus.WebClient;assembly=IVF.Morpheus.WebClient"">"
            //    + "<Converter:ExeExtendedGrid>"
            //    + "<Converter:ExeExtendedGrid.Resources>"
            //    + "<Converter:VisibilityConverter  x:Key='VisibilityConverter'></Converter:VisibilityConverter>"
            //    + "<Converter:StringToBoolConverter  x:Key='StringToBoolConverter'></Converter:StringToBoolConverter>"
            //     + "</Converter:ExeExtendedGrid.Resources>"
            //    + @"<TextBlock  Text='" + BindingExp + "' Visibility='Collapsed'/>"
            //     + "<TextBox Text='{Binding " + BindingExp + ",Mode=TwoWay}' Visibility='{Binding IsText,Converter={StaticResource VisibilityConverter}}'  VerticalAlignment='Center'  />"
            //        + "<CheckBox IsChecked='{Binding " + BindingExp + ",Mode=TwoWay,Converter={StaticResource StringToBoolConverter}}' Visibility='{Binding IsBool,Converter={StaticResource VisibilityConverter}}'  VerticalAlignment='Center' HorizontalAlignment='Center'></CheckBox>"
            //    + "</Converter:ExeExtendedGrid>"
            //+ "</DataTemplate>";

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

                // throw;
            }
        }

        void ExeGrid_KeyUp(object sender, KeyEventArgs e)
        {
            #region   Drug Row Deletion

            //clsTherapyExecutionVO selExeth = SelectedTherapyDetails.TherapyExecutionList.FirstOrDefault(p => p.PlanTherapyId == SelectedTherapyDetails.TherapyDetails.ID && p.PlanTherapyUnitId==SelectedTherapyDetails.TherapyDetails.UnitID );

            //if (selExeth!=null)
            //{
            //    SelectedTherapyDetails.TherapyDetails.ThearpyTypeDetailId = selExeth.ThearpyTypeDetailId;
            //    SelectedTherapyDetails.TherapyDetails.ThreapyExecutionId = selExeth.ID;
            //}



            if (e.Key == Key.Delete && e.PlatformKeyCode == 46 && ((clsTherapyExecutionVO)((DataGrid)sender).SelectedItem).ThearpyTypeDetailId != 0)
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to Delete Drug?";
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

                        }
                    }
                };
                msgWin.Show();

            }
            #endregion
        }

        #endregion

        #region TAb Selection Changed Event
        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((TabControl)sender).SelectedIndex == ((int)TherapyTabs.FollicularMonitoring) - 1)
            {
                setupPage(SelectedTherapyDetails.TherapyDetails.ID, ((int)TherapyTabs.FollicularMonitoring));//Fill Document Data Grid
            }
            //else if (tabControl1.SelectedIndex == 3)
            //{
            //    setupPage(((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).ID, 4);//Fill Document Data Grid
            //}
            //else if (tabControl1.SelectedIndex == 4)
            //{
            //    setupPage(((clsPlanTherapyVO)dgTherpyRptList.SelectedItem).ID, 2);//Fill Document Data Grid
            //}
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
                dtEndDate.DisplayDateStart = dtStartDate.SelectedDate.Value.AddDays(1);
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

        //bool Flag = false;
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
                        BizAction.FollicularMonitoringDetial.LeftSizeList = item.LeftSize;
                    }
                    else
                    {
                        BizAction.FollicularMonitoringDetial.LeftSizeList = BizAction.FollicularMonitoringDetial.LeftSizeList + "," + item.LeftSize;
                    }
                    if (string.IsNullOrEmpty(BizAction.FollicularMonitoringDetial.RightSizeList))
                    {
                        BizAction.FollicularMonitoringDetial.RightSizeList = item.RightSIze;

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
                throw;
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
                            new MessageBoxControl.MessageBoxChildWindow("", "Attention not entered.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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
                            new MessageBoxControl.MessageBoxChildWindow("", "Attention not entered.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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

        private void cmdANCSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdANCCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdSaveDelievery_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdCancelDelievery_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtWeight_TextChanged_1(object sender, TextChangedEventArgs e)
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

        private void txtWeight_KeyDown_1(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        //private void LoadArtRepeaterControl()
        //{
        //    lstIVFBox = new ListBox();

        //    if (IsSaved == false)
        //        IVFSetting = ((clsFemaleLabDay0VO)this.DataContext).IVFSetting = new List<IVFTreatment>();
        //    else
        //        IVFSetting = ((clsFemaleLabDay0VO)this.DataContext).IVFSetting;
        //    lstIVFBox.DataContext = ((clsFemaleLabDay0VO)this.DataContext).IVFSetting;
        //    if (IsSaved == false || ((clsFemaleLabDay0VO)this.DataContext).IVFSetting.Count == 0)
        //        IVFSetting.Add(new IVFTreatment() { CumulusSource = Cumulus, GradeSource = Grade, MOISource = MOI, PlanSource = Plan });

        //    for (int i = 0; i < IVFSetting.Count; i++)
        //    {
        //        IVFSetting[i].CumulusSource = Cumulus;
        //        IVFSetting[i].GradeSource = Grade;
        //        IVFSetting[i].MOISource = MOI;
        //        IVFSetting[i].PlanSource = Plan;

        //        IVFTreatmentPlanRepeterControlItem IVFTPrci = new IVFTreatmentPlanRepeterControlItem();
        //        IVFTPrci.OnAddRemoveClick += new RoutedEventHandler(IVFTPrci_OnAddRemoveClick);
        //        IVFTPrci.OnCmdMediaDetailClick += new RoutedEventHandler(IVFTPrci_OnCmdMediaDetailClick);
        //        IVFTPrci.OnchkProceedToDayClick += new RoutedEventHandler(IVFTPrci_OnchkProceedToDayClick);
        //        IVFTPrci.OnSelectionChanged += new SelectionChangedEventHandler(IVFTPrci_OnSelectionChanged);
        //        IVFTPrci.OnViewClick += new RoutedEventHandler(IVFTPrci_OnViewClick);
        //        IVFTPrci.OnBrowseClick += new RoutedEventHandler(IVFTPrci_OnBrowseClick);

        //        IVFSetting[i].Index = i;
        //        IVFSetting[i].Command = ((i == IVFSetting.Count - 1) ? "Add" : "Remove");
        //        IVFTPrci.DataContext = IVFSetting[i];
        //        lstIVFBox.Items.Add(IVFTPrci);
        //    }
        //    Grid.SetRow(lstIVFBox, 0);
        //    Grid.SetColumn(lstIVFBox, 0);
        //    IVFRepeater.Children.Add(lstIVFBox);
        //}
    }




















}


