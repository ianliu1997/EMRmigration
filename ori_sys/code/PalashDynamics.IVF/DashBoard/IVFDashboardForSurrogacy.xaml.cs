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
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.ValueObjects.DashBoardVO;
using PalashDynamics.Animations;
using PalashDynamics.UserControls;
using System.Windows.Data;
using PalashDynamics.ValueObjects.Patient;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Reflection;
using PalashDynamics.ValueObjects.Administration;
using System.Windows.Markup;
using PalashDynamics.IVF.PatientList;
using System.Text;
using PalashDynamics.IVF.TherpyExecution;
using System.Collections.ObjectModel;
using OPDModule;
using System.Windows.Browser;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class IVFDashboardForSurrogacy : UserControl, IInitiateCIMS
    {
        public IVFDashboardForSurrogacy()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(ARTFrontPanel, ARTBackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            App.TherapyExecutionForSurrogacyPatient = this;
            App.TherapyExecutionForSurrogate = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsPatientExist == false)
            {


                ModuleName = "PalashDynamics";
                Action = "PalashDynamics.Forms.PatientView.PatientListForSurrogacy";
                UserControl rootPage = Application.Current.RootVisual as UserControl;

                WebClient c2 = new WebClient();
                c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));


                //return;
            }
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
            {
                fillProtocolType();
                FillBabyType();
                dtStartDate.IsEnabled = false;
                dtEndDate.IsEnabled = false;
                if (chkPill.IsChecked == true)
                {
                    dtStartDate.IsEnabled = true;
                    dtEndDate.IsEnabled = true;
                }
                else
                {
                    dtStartDate.IsEnabled = false;
                    dtEndDate.IsEnabled = false;
                }
            }

        }

        #region Variables and properties
        bool IsCancel = true;
        DateTime? startDate = null;
        bool IsClosed = false;
        public bool IsExpendedWindow { get; set; }
        public string Action { get; set; }
        public string ModuleName { get; set; }
        UIElement myData = null;
        public Boolean a = true;
        private bool IsPatientExist = false;
        private SwivelAnimation objAnimation;
        WaitIndicator wait = new WaitIndicator();
        PagedCollectionView Execollection;
        PagedCollectionView Exe1collection;
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

        private clsIVFDashboard_GetTherapyListBizActionVO _TherapyDetails = new clsIVFDashboard_GetTherapyListBizActionVO();
        public clsIVFDashboard_GetTherapyListBizActionVO SelectedTherapyDetails
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

        frmGraphicalRepresentationofLabDays grpRep = null;
      //  FrmIUI frmIUI = null;
        frmIUIComeSemenWash frmIUI = null;
        frmBirth Birth = null;
        frmTransfer ET = null;
        frmOPU FrOPU = null;
        frmCryoPreservation frmCryo = null;
        frmOnlyET onlyET = null;
        frmDocumentforDashTherapy Doc = null;
        string billno;
        long billID = 0;
        long billUnitID = 0;
        string msgTitle = "";
        string msgText = "";
        long ChildID;
        DateTime? PregnancyAchivedDate;
        bool? IsPregnancyAchived;
        long FollicularID = 0;
        long FollicularUnitID = 0;

        private List<MasterListItem> _SSemen = new List<MasterListItem>();
        public List<MasterListItem> SSemen
        {
            get
            {
                return _SSemen;
            }
            set
            {
                _SSemen = value;
            }
        }
        private List<MasterListItem> _SOctyes = new List<MasterListItem>();
        public List<MasterListItem> SOctyes
        {
            get
            {
                return _SOctyes;
            }
            set
            {
                _SOctyes = value;
            }
        }
        private List<MasterListItem> _Grade = new List<MasterListItem>();
        public List<MasterListItem> Grade
        {
            get
            {
                return _Grade;
            }
            set
            {
                _Grade = value;
            }
        }

        private List<MasterListItem> _CanList = new List<MasterListItem>();
        public List<MasterListItem> CanList
        {
            get
            {
                return _CanList;
            }
            set
            {
                _CanList = value;
            }
        }
        private List<MasterListItem> _ProtocolType = new List<MasterListItem>();
        public List<MasterListItem> ProtocolType
        {
            get
            {
                return _ProtocolType;
            }
            set
            {
                _ProtocolType = value;
            }
        }
        private List<MasterListItem> _CellStage = new List<MasterListItem>();
        public List<MasterListItem> CellStage
        {
            get
            {
                return _CellStage;
            }
            set
            {
                _CellStage = value;
            }
        }

        private List<MasterListItem> _Straw = new List<MasterListItem>();
        public List<MasterListItem> Straw
        {
            get
            {
                return _Straw;
            }
            set
            {
                _Straw = value;
            }
        }

        private List<MasterListItem> _GobletShape = new List<MasterListItem>();
        public List<MasterListItem> GobletShape
        {
            get
            {
                return _GobletShape;
            }
            set
            {
                _GobletShape = value;
            }
        }

        private List<MasterListItem> _GobletSize = new List<MasterListItem>();
        public List<MasterListItem> GobletSize
        {
            get
            {
                return _GobletSize;
            }
            set
            {
                _GobletSize = value;
            }
        }

        private List<MasterListItem> _GobletColor = new List<MasterListItem>();
        public List<MasterListItem> GobletColor
        {
            get
            {
                return _GobletColor;
            }
            set
            {
                _GobletColor = value;
            }
        }

        private List<MasterListItem> _Canister = new List<MasterListItem>();
        public List<MasterListItem> Canister
        {
            get
            {
                return _Canister;
            }
            set
            {
                _Canister = value;
            }
        }

        private List<MasterListItem> _Tank = new List<MasterListItem>();
        public List<MasterListItem> Tank
        {
            get
            {
                return _Tank;
            }
            set
            {
                _Tank = value;
            }
        }

        private ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO> _VitriDetails = new ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO>();
        public ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO> VitriDetails
        {
            get { return _VitriDetails; }
            set { _VitriDetails = value; }
        }
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        #endregion

        #region fillCombox
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
                    cmbAdsercchProtocol.ItemsSource = null;
                    cmbAdsercchProtocol.ItemsSource = objList;
                    cmbAdsercchProtocol.SelectedValue = (long)0;

                }
                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }
                fillMainIndication();
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
                fillSpermCollection();
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
                fillPlannedTreatment();
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

                    cmbAdsercchPlannedTreatment.ItemsSource = null;
                    cmbAdsercchPlannedTreatment.ItemsSource = objList;
                    cmbAdsercchPlannedTreatment.SelectedValue = (long)0;

                }
                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }
                fillExternalStimulation();
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
            fillDoctor();
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

                    cmbAdsercchDoctor.ItemsSource = null;
                    cmbAdsercchDoctor.ItemsSource = objList;
                    cmbAdsercchDoctor.SelectedValue = (long)0;


                }
                fillCoupleDetails();
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }


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

                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        #endregion

        #region IInitiateCIMS Members


        public void Initiate(string Mode)
        {
            switch (Mode.ToUpper())
            {
                case "NEW":

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

                                ModuleName = "PalashDynamics";
                                Action = "PalashDynamics.Forms.PatientView.PatientListForSurrogacy";
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
                            Action = "PalashDynamics.Forms.PatientView.PatientListForSurrogacy";
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
                throw ex;
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
                ((IInitiateCIMS)myData).Initiate("REG");
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            catch (Exception ex)
            {
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


        public void SSemenMaster()
        {
            try
            {
                wait.Show();
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVF_SourceSemenMaster;
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
                        SSemen = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        fillGrade();
                    }
                    //wait.Close();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        private void fillGrade()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVF_GradeMaster;
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        Grade = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        fillOocyteSource();
                    }

                    if (this.DataContext != null)
                    {
                        //cmbSrcTreatmentPlan.SelectedValue = ((clsVO)this.DataContext).;
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

        private void fillOocyteSource()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVF_SourceOocyteMaster;
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
                        SOctyes = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        fillCanID();
                    }
                    //wait.Close();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }

        }

        private void fillCanID()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFCanMaster;
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        CanList = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        FillStrawList();
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

        private void FillStrawList()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFStrawMaster;
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        Straw = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        FillGobletshape();
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

        private void FillGobletshape()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFGobletShapeMaster;
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        GobletShape = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        FillGobletSize();
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

        private void FillGobletSize()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFGobletSizeMaster;
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        GobletSize = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        fillGlobletColor();
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

        private void fillGlobletColor()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFGobletColor;
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        GobletColor = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        FillCanister();
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

        private void FillCanister()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFCanisterMaster;
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        Canister = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        FillTank();
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

        private void FillTank()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFTankMaster;
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        Tank = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        fillPlan();
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

        private void fillPlan()
        {
            try
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
                        ProtocolType = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        FillCellStage();
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

        private void FillCellStage()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVF_FertilizationStageMaster;
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
                        CellStage = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        fillOnlyVitrificationDetails();
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

        private void txtOutβhCGValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                if (!((TextBox)sender).Text.IsItNumber() && textBefore != null)
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
        }

        private void txtOutβhCGValue_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtCount_TextChanged(object sender, TextChangedEventArgs e)
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

        private void txtCount_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        #region Only Vitrification
        private void chkUseownoocyte_Click(object sender, RoutedEventArgs e)
        {
            if (chkUseownoocyte.IsChecked == true)
            {
                GetEmbfromVitrification();
            }
            else
            {
                GetEmbfromVitrificationWithoutUsingOwnOocyte();
            }
        }

        private void GetEmbfromVitrificationWithoutUsingOwnOocyte()
        {
            try
            {
                clsIVFDashboard_GetPreviousVitrificationBizActionVO BizAction = new clsIVFDashboard_GetPreviousVitrificationBizActionVO();
                BizAction.VitrificationMain = new clsIVFDashBoard_VitrificationVO();
                if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails != null)
                {
                    BizAction.VitrificationMain.PatientID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.PatientID;
                    BizAction.VitrificationMain.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.UnitId;
                }
                BizAction.VitrificationMain.IsOnlyVitrification = true;
                BizAction.VitrificationMain.UsedOwnOocyte = false;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        VitriDetails = new ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO>();

                        MasterListItem Gr = new MasterListItem();
                        MasterListItem PT = new MasterListItem(); ;
                        MasterListItem CS = new MasterListItem(); ;
                        for (int i = 0; i < ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList.Count; i++)
                        {
                            if (((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID != null)
                            {
                                Gr = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID));
                            }
                            if (((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID != null)
                            {
                                CS = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID));
                            }
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanIdList = CanList;
                            if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanId) > 0)
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == 0);
                            }

                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ColorSelectorList = GobletColor;
                            if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ColorCodeID) > 0)
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedColorSelector = GobletColor.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ColorCodeID));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedColorSelector = GobletColor.FirstOrDefault(p => p.ID == 0);
                            }

                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].StrawIdList = Straw;
                            if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].StrawId) > 0)
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].StrawId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == 0);
                            }
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletShapeList = GobletShape;
                            if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletShapeId) > 0)
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletShapeId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == 0);
                            }
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletSizeList = GobletSize;
                            {
                                if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletSizeId) > 0)
                                {
                                    ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletSizeId));
                                }
                                else
                                {
                                    ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == 0);
                                }
                            }
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanisterIdList = Canister;
                            if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ConistorNo) > 0)
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ConistorNo));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == 0);
                            }



                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TankList = Tank;
                            if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TankId) > 0)
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TankId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == 0);
                            }

                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeList = Grade;
                            if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID) > 0)
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGrade = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGrade = Grade.FirstOrDefault(p => p.ID == 0);
                            }
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageList = CellStage;
                            if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID) > 0)
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCellStage = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCellStage = CellStage.FirstOrDefault(p => p.ID == 0);
                            }

                            VitriDetails.Add(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i]);

                        }

                        dgonlyVitrificationDetilsGrid.ItemsSource = null;
                        dgonlyVitrificationDetilsGrid.ItemsSource = VitriDetails;

                        wait.Close();
                        if (VitriDetails.Count == 0)
                        {
                            fillInitailOnlyVitrificationDetails();
                        }
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                wait.Close();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        private void GetEmbfromVitrification()
        {
            try
            {
                clsIVFDashboard_GetPreviousVitrificationBizActionVO BizAction = new clsIVFDashboard_GetPreviousVitrificationBizActionVO();
                BizAction.VitrificationMain = new clsIVFDashBoard_VitrificationVO();
                if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails != null)
                {
                    BizAction.VitrificationMain.PatientID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.PatientID;
                    BizAction.VitrificationMain.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.UnitId;
                }
                BizAction.VitrificationMain.IsOnlyVitrification = true;
                BizAction.VitrificationMain.UsedOwnOocyte = true;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        VitriDetails = new ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO>();

                        MasterListItem Gr = new MasterListItem();
                        MasterListItem PT = new MasterListItem(); ;
                        MasterListItem CS = new MasterListItem(); ;
                        for (int i = 0; i < ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList.Count; i++)
                        {
                            if (((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID != null)
                            {
                                Gr = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID));
                            }
                            if (((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID != null)
                            {
                                CS = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID));
                            }
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanIdList = CanList;
                            if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanId) > 0)
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == 0);
                            }

                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ColorSelectorList = GobletColor;
                            if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ColorCodeID) > 0)
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedColorSelector = GobletColor.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ColorCodeID));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedColorSelector = GobletColor.FirstOrDefault(p => p.ID == 0);
                            }

                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].StrawIdList = Straw;
                            if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].StrawId) > 0)
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].StrawId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == 0);
                            }
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletShapeList = GobletShape;
                            if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletShapeId) > 0)
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletShapeId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == 0);
                            }
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletSizeList = GobletSize;
                            {
                                if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletSizeId) > 0)
                                {
                                    ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletSizeId));
                                }
                                else
                                {
                                    ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == 0);
                                }
                            }
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanisterIdList = Canister;
                            if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ConistorNo) > 0)
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ConistorNo));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == 0);
                            }



                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TankList = Tank;
                            if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TankId) > 0)
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TankId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == 0);
                            }

                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeList = Grade;
                            if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID) > 0)
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGrade = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGrade = Grade.FirstOrDefault(p => p.ID == 0);
                            }
                            ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageList = CellStage;
                            if (Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID) > 0)
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCellStage = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCellStage = CellStage.FirstOrDefault(p => p.ID == 0);
                            }

                            VitriDetails.Add(((clsIVFDashboard_GetPreviousVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i]);

                        }

                        dgonlyVitrificationDetilsGrid.ItemsSource = null;
                        dgonlyVitrificationDetilsGrid.ItemsSource = VitriDetails;
                        wait.Close();
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                wait.Close();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (VitriDetails == null)
            {
                VitriDetails = new ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO>();
            }
            VitriDetails.Add(new clsIVFDashBoard_VitrificationDetailsVO() { CanId = 0, StrawId = 0, GobletShapeId = 0, ColorCodeID = 0, GobletSizeId = 0, ConistorNo = 0, TankId = 0, CanIdList = CanList, StrawIdList = Straw, GobletSizeList = GobletSize, GobletShapeList = GobletShape, CanisterIdList = Canister, TankList = Tank, ColorSelectorList = GobletColor, EmbNumber = 0, GradeList = Grade, CellStageList = CellStage });

            dgonlyVitrificationDetilsGrid.ItemsSource = VitriDetails;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (dgonlyVitrificationDetilsGrid.SelectedItem != null)
            {
                VitriDetails.RemoveAt(dgonlyVitrificationDetilsGrid.SelectedIndex);
            }
            dgonlyVitrificationDetilsGrid.ItemsSource = VitriDetails;
        }
        private bool ValidateOnlyVtri()
        {
            if (dtonlyVitrificationDate.SelectedDate == null)
            {
                dtonlyVitrificationDate.SetValidation("Please Select Vitrification Date");
                dtonlyVitrificationDate.RaiseValidationError();
                dtonlyVitrificationDate.Focus();
                return false;
            }
            else if (txtonlyVitTime.Value == null)
            {
                dtonlyVitrificationDate.ClearValidationError();
                txtonlyVitTime.SetValidation("Please Select Vitrification Time");
                txtonlyVitTime.RaiseValidationError();
                txtonlyVitTime.Focus();
                return false;
            }
            else if (VitriDetails.Count <= 0)
            {

                dtonlyVitrificationDate.ClearValidationError();
                txtonlyVitTime.ClearValidationError();
                dtonlyvitPickUpDate.ClearValidationError();

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Vitrification Details Data Grid Cannot Be Empty", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                return false;
            }
            else
            {
                dtonlyVitrificationDate.ClearValidationError();
                txtonlyVitTime.ClearValidationError();
                dtonlyvitPickUpDate.ClearValidationError();
                return true;
            }
        }

        private void cmdSaveOnlyVit_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateOnlyVtri())
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosedonlyVitri);
                msgW.Show();
            }
        }

        void msgW_OnMessageBoxClosedonlyVitri(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                SaveOnlyVtri();
        }

        private void SaveOnlyVtri()
        {
            clsIVFDashboard_AddUpdateVitrificationBizActionVO BizAction = new clsIVFDashboard_AddUpdateVitrificationBizActionVO();
            BizAction.VitrificationMain = new clsIVFDashBoard_VitrificationVO();
            BizAction.VitrificationDetailsList = new List<clsIVFDashBoard_VitrificationDetailsVO>();
            BizAction.VitrificationMain.ID = ((clsIVFDashboard_GetVitrificationBizActionVO)this.DataContext).VitrificationMain.ID;
            BizAction.VitrificationMain.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.VitrificationMain.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.VitrificationMain.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
            BizAction.VitrificationMain.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
            BizAction.VitrificationMain.DateTime = dtonlyVitrificationDate.SelectedDate.Value.Date;
            BizAction.VitrificationMain.DateTime = BizAction.VitrificationMain.DateTime.Value.Add(txtonlyVitTime.Value.Value.TimeOfDay);
            BizAction.VitrificationMain.PickUpDate = dtonlyvitPickUpDate.SelectedDate.Value.Date;
            BizAction.VitrificationMain.IsOnlyVitrification = true;

            for (int i = 0; i < VitriDetails.Count; i++)
            {
                VitriDetails[i].CanId = VitriDetails[i].SelectedCanId.ID;
                VitriDetails[i].StrawId = VitriDetails[i].SelectedStrawId.ID;
                VitriDetails[i].GobletShapeId = VitriDetails[i].SelectedGobletShape.ID;
                VitriDetails[i].GobletSizeId = VitriDetails[i].SelectedGobletSize.ID;
                VitriDetails[i].ConistorNo = VitriDetails[i].SelectedCanisterId.ID;
                VitriDetails[i].TankId = VitriDetails[i].SelectedTank.ID;
                VitriDetails[i].ColorCodeID = VitriDetails[i].SelectedColorSelector.ID;
                VitriDetails[i].GradeID = VitriDetails[i].SelectedGrade.ID;
                VitriDetails[i].CellStageID = VitriDetails[i].SelectedCellStage.ID;
                VitriDetails[i].EmbNumber = i + 1;
            }
            BizAction.VitrificationDetailsList = ((List<clsIVFDashBoard_VitrificationDetailsVO>)VitriDetails.ToList());
            if (chkonlyvitFreeze.IsChecked == true)
                BizAction.VitrificationMain.IsFreezed = true;
            else
                BizAction.VitrificationMain.IsFreezed = false;

            if (rdoYes.IsChecked == true)
                BizAction.VitrificationMain.ConsentForm = true;
            else
                BizAction.VitrificationMain.ConsentForm = false;
            if (chkUseownoocyte.IsChecked == true)
                BizAction.VitrificationMain.UsedOwnOocyte = true;
            else
                BizAction.VitrificationMain.UsedOwnOocyte = false;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                    fillOnlyVitrificationDetails();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        public void fillOnlyVitrificationDetails()
        {
            try
            {
                clsIVFDashboard_GetVitrificationBizActionVO BizAction = new clsIVFDashboard_GetVitrificationBizActionVO();
                BizAction.VitrificationMain = new clsIVFDashBoard_VitrificationVO();
                BizAction.VitrificationMain.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                BizAction.VitrificationMain.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails != null)
                {
                    BizAction.VitrificationMain.PatientID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.PatientID;
                    BizAction.VitrificationMain.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.UnitId;
                }
                BizAction.VitrificationMain.IsOnlyVitrification = true;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        VitriDetails.Clear();
                        this.DataContext = null;
                        this.DataContext = (clsIVFDashboard_GetVitrificationBizActionVO)arg.Result;
                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.IsFreezed == true)
                        {
                            cmdSaveOnlyVit.IsEnabled = false;
                            chkUseownoocyte.IsEnabled = false;
                        }
                        if (IsClosed == true)
                        {
                            cmdSaveOnlyVit.IsEnabled = false;
                        }
                        chkonlyvitFreeze.IsChecked = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.IsFreezed;
                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.DateTime != null)
                        {
                            dtonlyVitrificationDate.SelectedDate = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.DateTime;
                            txtonlyVitTime.Value = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.DateTime;
                        }
                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.VitrificationNo != null)
                        {
                            txtonlyVitriNo.Text = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.VitrificationNo;
                        }
                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.PickUpDate != null)
                        {
                            dtonlyvitPickUpDate.SelectedDate = ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.PickUpDate;
                        }
                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.ConsentForm == true)
                        {
                            rdoYes.IsChecked = true;
                        }
                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.ConsentForm == false)
                        {
                            rdoNo.IsChecked = true;
                        }
                        if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationMain.UsedOwnOocyte == true)
                        {
                            chkUseownoocyte.IsChecked = true;
                        }
                        MasterListItem Gr = new MasterListItem();
                        MasterListItem PT = new MasterListItem(); ;
                        MasterListItem CS = new MasterListItem(); ;
                        for (int i = 0; i < ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList.Count; i++)
                        {
                            if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID != null)
                            {
                                Gr = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID));
                            }
                            if (((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID != null)
                            {
                                CS = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID));
                            }
                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanIdList = CanList;
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanId) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == 0);
                            }

                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ColorSelectorList = GobletColor;
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ColorCodeID) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedColorSelector = GobletColor.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ColorCodeID));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedColorSelector = GobletColor.FirstOrDefault(p => p.ID == 0);
                            }

                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].StrawIdList = Straw;
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].StrawId) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].StrawId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == 0);
                            }
                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletShapeList = GobletShape;
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletShapeId) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletShapeId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == 0);
                            }
                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletSizeList = GobletSize;
                            {
                                if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletSizeId) > 0)
                                {
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GobletSizeId));
                                }
                                else
                                {
                                    ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == 0);
                                }
                            }
                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CanisterIdList = Canister;
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ConistorNo) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].ConistorNo));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == 0);
                            }



                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TankList = Tank;
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TankId) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].TankId));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == 0);
                            }

                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeList = Grade;
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGrade = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].GradeID));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedGrade = Grade.FirstOrDefault(p => p.ID == 0);
                            }
                            ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageList = CellStage;
                            if (Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID) > 0)
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCellStage = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].CellStageID));
                            }
                            else
                            {
                                ((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i].SelectedCellStage = CellStage.FirstOrDefault(p => p.ID == 0);
                            }

                            VitriDetails.Add(((clsIVFDashboard_GetVitrificationBizActionVO)arg.Result).VitrificationDetailsList[i]);

                        }

                        dgonlyVitrificationDetilsGrid.ItemsSource = null;
                        dgonlyVitrificationDetilsGrid.ItemsSource = VitriDetails;

                        if (VitriDetails.Count == 0)
                        {
                            fillInitailOnlyVitrificationDetails();
                        }

                        wait.Close();
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                wait.Close();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        public void fillInitailOnlyVitrificationDetails()
        {
            VitriDetails = new ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO>();
            VitriDetails.Add(new clsIVFDashBoard_VitrificationDetailsVO() { CanId = 0, StrawId = 0, GobletShapeId = 0, ColorCodeID = 0, GobletSizeId = 0, ConistorNo = 0, TankId = 0, CanIdList = CanList, StrawIdList = Straw, GobletSizeList = GobletSize, GobletShapeList = GobletShape, CanisterIdList = Canister, TankList = Tank, ColorSelectorList = GobletColor, EmbNumber = 0, GradeList = Grade, CellStageList = CellStage });
            dgonlyVitrificationDetilsGrid.ItemsSource = VitriDetails;
            wait.Close();
        }
        #endregion



        #region Setup Page
        public void setupPage(long TherapyID, long TabId)
        {
            try
            {
                wait.Show();
                clsIVFDashboard_GetTherapyListBizActionVO BizAction = new clsIVFDashboard_GetTherapyListBizActionVO();
                BizAction.TherapyID = TherapyID;
                BizAction.TabID = TabId;
                BizAction.Flag = true;

                if (SelectedTherapyDetails.TherapyDetails.UnitID == 0)
                {
                    BizAction.TherapyUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }
                else
                {
                    BizAction.TherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                }

                if (CoupleDetails != null)
                {
                    BizAction.PatientID = CoupleDetails.FemalePatient.PatientID;
                    BizAction.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                }
                if (((MasterListItem)cmbAdsercchDoctor.SelectedItem).ID != 0)
                {
                    BizAction.PhysicianId = ((MasterListItem)cmbAdsercchDoctor.SelectedItem).ID;
                }
                if (((MasterListItem)cmbAdsercchProtocol.SelectedItem).ID != 0)
                {
                    BizAction.ProtocolTypeID = ((MasterListItem)cmbAdsercchProtocol.SelectedItem).ID;
                }
                if (((MasterListItem)cmbAdsercchPlannedTreatment.SelectedItem).ID != 0)
                {
                    BizAction.PlannedTreatmentID = ((MasterListItem)cmbAdsercchPlannedTreatment.SelectedItem).ID;
                }
                BizAction.rdoSuccessful = rdoSuccessful.IsChecked;
                BizAction.rdoAll = rdoAll.IsChecked;
                BizAction.rdoUnsuccessful = rdoUnsuccessful.IsChecked;
                BizAction.rdoClosed = rdoClosed.IsChecked;
                BizAction.rdoActive = rdoActive.IsChecked;
                BizAction.AttachedSurrogate = true;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (TherapyID == 0)
                        {
                            this.DataContext = null;
                            dgtheropyList.ItemsSource = null;
                            dgtheropyList.ItemsSource = ((clsIVFDashboard_GetTherapyListBizActionVO)args.Result).TherapyDetailsList;

                            dgtheropyList.SelectedIndex = 0;
                            TherapyDetailsList = new List<clsPlanTherapyVO>();
                            TherapyDetailsList = ((clsIVFDashboard_GetTherapyListBizActionVO)args.Result).TherapyDetailsList;


                            wait.Close();
                        }
                        else
                        {
                            if (TabId == 0)
                            {
                                //    this.DataContext = null;
                                SelectedTherapyDetails.TherapyExecutionList = ((clsIVFDashboard_GetTherapyListBizActionVO)args.Result).TherapyExecutionList;
                                SelectedTherapyDetails.FollicularMonitoringList = ((clsIVFDashboard_GetTherapyListBizActionVO)args.Result).FollicularMonitoringList;
                                SelectedTherapyDetails.TherapyExecutionListSurrogate = ((clsIVFDashboard_GetTherapyListBizActionVO)args.Result).TherapyExecutionListSurrogate;

                                this.DataContext = SelectedTherapyDetails;



                                // For Birthdetails Tab
                                ChildID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).BabyTypeID;
                                PregnancyAchivedDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PregnanacyConfirmDate;
                                IsPregnancyAchived = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsPregnancyAchieved;


                                if ((ChildID != null && ChildID != 0) && PregnancyAchivedDate != null && IsPregnancyAchived == true)
                                {
                                    Tabbirthdetails.Visibility = Visibility.Visible;
                                }

                                //...............................................
                                if (SelectedTherapyDetails.TherapyExecutionList[0].IsOocyteDonorExists == true && SelectedTherapyDetails.TherapyExecutionList[0].IsSemenDonorExists == true)
                                    AccItem.Header = "Therapy OverView " + "Oocyte Donor(" + SelectedTherapyDetails.TherapyExecutionList[0].OoctyDonorMrNo + ") Semen Donor(" + SelectedTherapyDetails.TherapyExecutionList[0].SemenDonorMrNo + ")";
                                else if (SelectedTherapyDetails.TherapyExecutionList[0].IsOocyteDonorExists == true && SelectedTherapyDetails.TherapyExecutionList[0].IsSemenDonorExists == false)
                                    AccItem.Header = "Therapy OverView " + "Oocyte Donor(" + SelectedTherapyDetails.TherapyExecutionList[0].OoctyDonorMrNo + ")";
                                else if (SelectedTherapyDetails.TherapyExecutionList[0].IsOocyteDonorExists == false && SelectedTherapyDetails.TherapyExecutionList[0].IsSemenDonorExists == true)
                                    AccItem.Header = "Therapy OverView " + "Semen Donor(" + SelectedTherapyDetails.TherapyExecutionList[0].SemenDonorMrNo + ")";
                                else if (SelectedTherapyDetails.TherapyExecutionList[0].IsOocyteDonorExists == false && SelectedTherapyDetails.TherapyExecutionList[0].IsSemenDonorExists == false)
                                    AccItem.Header = "Therapy OverView ";


                                InitExecutionGrid();
                                InitExecutionGridSurrogate();
                                BindExecutionGrid();
                                BindExecutionGridSurrogate();
                                wait.Close();
                            }
                            else
                            {
                                if (TabId == 2)
                                {
                                    SelectedTherapyDetails.TherapyExecutionList = ((clsIVFDashboard_GetTherapyListBizActionVO)args.Result).TherapyExecutionList;
                                    SelectedTherapyDetails.TherapyExecutionListSurrogate = ((clsIVFDashboard_GetTherapyListBizActionVO)args.Result).TherapyExecutionListSurrogate;
                                    this.DataContext = SelectedTherapyDetails;
                                    InitExecutionGrid();
                                    InitExecutionGridSurrogate();
                                    BindExecutionGrid();
                                    BindExecutionGridSurrogate();
                                    wait.Close();
                                }
                                if (TabId == 3)
                                {

                                    SelectedTherapyDetails.FollicularMonitoringList = ((clsIVFDashboard_GetTherapyListBizActionVO)args.Result).FollicularMonitoringList;
                                    this.DataContext = SelectedTherapyDetails;
                                    InitExecutionGrid();
                                    InitExecutionGridSurrogate();
                                    BindExecutionGrid();
                                    BindExecutionGridSurrogate();
                                    wait.Close();
                                }

                            }
                            dgtheropyList.SelectedItem = TherapyDetailsList.FirstOrDefault(p => p.ID == TherapyID);
                        }

                        wait.Close();

                    }
                    else
                    {
                        wait.Close();
                    }
                    wait.Close();
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
        #endregion

        #region Bill History
        void serviceSearch_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            billID = ((OPDModule.frmBillListPatientWise)(sender)).BillID;
            billUnitID = ((OPDModule.frmBillListPatientWise)(sender)).BillUnitID;
            DateTime billDate = ((OPDModule.frmBillListPatientWise)(sender)).BillDate;
            billno = ((OPDModule.frmBillListPatientWise)(sender)).BillNo;
        }
        private void CmdBillHistory_Click(object sender, RoutedEventArgs e)
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
            {
                long patientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                long PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                frmBillListPatientWise serviceSearch = null;
                serviceSearch = new frmBillListPatientWise(patientID, PatientUnitID);
                serviceSearch.Show();
                serviceSearch.OnSaveButton_Click += new RoutedEventHandler(serviceSearch_OnAddButton_Click);
            }
            else
            {
                msgText = "Please Select The Patient";
                MessageBoxControl.MessageBoxChildWindow msgWindow =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                msgWindow.Show();
            }
        }
        #endregion

        #region Cancel event
        private void CmdCancleMain_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    objAnimation.Invoke(RotationType.Backward);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            if (IsCancel == true)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                ((IApplicationConfiguration)App.Current).OpenMainContent("PalashDynamics.Forms.PatientView.PatientListForSurrogacy");
            }
            else
            {
                objAnimation.Invoke(RotationType.Backward);
                IsCancel = true;
                setupPage(0, 0);
            }
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
                DateTime? drsd;
                if (((clsIVFDashboard_GetTherapyListBizActionVO)this.DataContext).TherapyDetails.TherapyStartDate != null)
                    drsd = ((clsIVFDashboard_GetTherapyListBizActionVO)this.DataContext).TherapyDetails.TherapyStartDate;
                else
                    drsd = startDate;
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
            + "<Converter:ExeExtendedGridForSurrogacyPatient>"
            + "<Converter:ExeExtendedGridForSurrogacyPatient.Resources>"
            + "<Converter:VisibilityConverter  x:Key='VisibilityConverter'></Converter:VisibilityConverter>"
            + "<Converter:StringToBoolConverter  x:Key='StringToBoolConverter'></Converter:StringToBoolConverter>"
            + "<Converter:TherapyToTimeConverter  x:Key='TherapyToTimeConverter'></Converter:TherapyToTimeConverter>"
            + "<Converter:TherapyToDateTimeConverter  x:Key='TherapyToDateTimeConverter'></Converter:TherapyToDateTimeConverter>"
            + "<Converter:TherapyToVisibilityConverter  x:Key='TherapyToVisibilityConverter'></Converter:TherapyToVisibilityConverter>"
            + "</Converter:ExeExtendedGridForSurrogacyPatient.Resources>"
            + @"<TextBlock  Text='" + BindingExp + "' Visibility='Collapsed'/>"
            + "<TextBox " + ReadOnly + " Text='{Binding " + BindingExp + ",Mode=TwoWay}' MaxLength='6' Visibility='{Binding IsText,Converter={StaticResource VisibilityConverter}}' VerticalAlignment='Center' Width='40'><ToolTipService.ToolTip><ToolTip  DataContext='{Binding CurrentTherapy,Converter={StaticResource TherapyToDateTimeConverter},ConverterParameter=" + BindingExp + "}' Content='{Binding TimeOfDay,Converter={StaticResource TherapyToTimeConverter}}'></ToolTip></ToolTipService.ToolTip></TextBox>"
            + "<CheckBox " + Enabled + " IsChecked='{Binding " + BindingExp + ",Mode=TwoWay, Converter={StaticResource StringToBoolConverter}}' Visibility='{Binding IsBool,Converter={StaticResource VisibilityConverter}}'  VerticalAlignment='Center' HorizontalAlignment='Center' ></CheckBox>"
            + "</Converter:ExeExtendedGridForSurrogacyPatient>"
        + "</DataTemplate>";
            dt = (DataTemplate)XamlReader.Load(xaml);
            return dt;
        }

        private DataTemplate CreateExe(string BindingExp)
        {
            DataTemplate dt = null;

            string xaml = @"<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' xmlns:Converter=""clr-namespace:PalashDynamics.IVF;assembly=PalashDynamics.IVF"">"
           + "<Converter:ExeExtendedGridForSurrogacyPatient>"
           + "<Converter:ExeExtendedGridForSurrogacyPatient.Resources>"
           + "<Converter:VisibilityConverter  x:Key='VisibilityConverter'></Converter:VisibilityConverter>"
           + "<Converter:StringToBoolConverter  x:Key='StringToBoolConverter'></Converter:StringToBoolConverter>"
           + "<Converter:TherapyToTimeConverter  x:Key='TherapyToTimeConverter'></Converter:TherapyToTimeConverter>"
           + "<Converter:TherapyToDateTimeConverter  x:Key='TherapyToDateTimeConverter'></Converter:TherapyToDateTimeConverter>"
           + "<Converter:ToggleBooleanValueConverter  x:Key='ToggleBooleanValueConverter'></Converter:ToggleBooleanValueConverter>"
           + "<Converter:TherapyToVisibilityConverter  x:Key='TherapyToVisibilityConverter'></Converter:TherapyToVisibilityConverter>"
           + "<Converter:NumberToBrushConverter  x:Key='NumberToBrushConverter' Id='{Binding Id}'></Converter:NumberToBrushConverter>"
            + "</Converter:ExeExtendedGridForSurrogacyPatient.Resources>"
           + @"<TextBlock  Text='" + BindingExp + "' Visibility='Collapsed'/>"
            + "<TextBox Background='{Binding CurrentTherapy, Converter={StaticResource NumberToBrushConverter},ConverterParameter=" + BindingExp + "}' Text='{Binding " + BindingExp + ",Mode=TwoWay}' Visibility='{Binding IsText,Converter={StaticResource VisibilityConverter}}' VerticalAlignment='Center' MaxLength='6'><ToolTipService.ToolTip><ToolTip  DataContext='{Binding CurrentTherapy,Converter={StaticResource TherapyToDateTimeConverter},ConverterParameter=" + BindingExp + "}' Content='{Binding TimeOfDay,Converter={StaticResource TherapyToTimeConverter}}'></ToolTip></ToolTipService.ToolTip></TextBox>"
               + "<CheckBox  IsChecked='{Binding " + BindingExp + ",Mode=TwoWay,Converter={StaticResource StringToBoolConverter}}' Visibility='{Binding IsBool,Converter={StaticResource VisibilityConverter}}' IsEnabled='{Binding IsBool,Converter={StaticResource ToggleBooleanValueConverter}}' VerticalAlignment='Center' HorizontalAlignment='Center'></CheckBox>"
                //+ "<CheckBox  IsChecked='{Binding " + BindingExp + ",Mode=TwoWay,Converter={StaticResource StringToBoolConverter}}' IsEnabled='{Binding "+ BindingExp +",Converter={StaticResource NegateValueConverter}}' VerticalAlignment='Center' HorizontalAlignment='Center'></CheckBox>"
               + "</Converter:ExeExtendedGridForSurrogacyPatient>"
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
                ExeGrid.SelectedIndex = -1;
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
                ExeGrid1.SelectedIndex = -1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ExeGrid_KeyUp(object sender, KeyEventArgs e)
        {
            #region   Drug Row Deletion
            if (e.Key == Key.Delete && e.PlatformKeyCode == 46 && ((clsTherapyExecutionVO)((DataGrid)sender).SelectedItem).ThearpyTypeDetailId != 0)
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
                            BizAction.TherapyDetails.ThearpyTypeDetailId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ThearpyTypeDetailId;
                            #region Service Call (Check Validation)

                            client.ProcessCompleted += (s, args) =>
                            {
                                if (args.Error == null && args.Result != null)
                                {
                                    App.TherapyExecutionForSurrogacyPatient.setupPage(((clsTherapyExecutionVO)((DataGrid)sender).SelectedItem).PlanTherapyId, (int)TherapyTabs.Execution);
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
                            BizAction.TherapyDetails.ThearpyTypeDetailId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ThearpyTypeDetailId;
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
                DateTime? drsd1;
                if (((clsIVFDashboard_GetTherapyListBizActionVO)this.DataContext).TherapyDetails.TherapyStartDate != null)
                    drsd1 = ((clsIVFDashboard_GetTherapyListBizActionVO)this.DataContext).TherapyDetails.TherapyStartDate;
                else
                    drsd1 = startDate;
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
            + "<Converter:ExtendedGridForSurrogate>"
            + "<Converter:ExtendedGridForSurrogate.Resources>"
            + "<Converter:VisibilityConverter  x:Key='VisibilityConverter'></Converter:VisibilityConverter>"
            + "<Converter:StringToBoolConverter  x:Key='StringToBoolConverter'></Converter:StringToBoolConverter>"
            + "<Converter:TherapyToTimeConverter  x:Key='TherapyToTimeConverter'></Converter:TherapyToTimeConverter>"
            + "<Converter:TherapyToDateTimeConverter  x:Key='TherapyToDateTimeConverter'></Converter:TherapyToDateTimeConverter>"
            + "<Converter:TherapyToVisibilityConverter  x:Key='TherapyToVisibilityConverter'></Converter:TherapyToVisibilityConverter>"
            + "</Converter:ExtendedGridForSurrogate.Resources>"
            + @"<TextBlock  Text='" + BindingExpSurrogate + "' Visibility='Collapsed'/>"
            + "<TextBox " + ReadOnly + " Text='{Binding " + BindingExpSurrogate + ",Mode=TwoWay}' MaxLength='6' Visibility='{Binding IsText,Converter={StaticResource VisibilityConverter}}' VerticalAlignment='Center' Width='40'><ToolTipService.ToolTip><ToolTip  DataContext='{Binding CurrentTherapy,Converter={StaticResource TherapyToDateTimeConverter},ConverterParameter=" + BindingExpSurrogate + "}' Content='{Binding TimeOfDay,Converter={StaticResource TherapyToTimeConverter}}'></ToolTip></ToolTipService.ToolTip></TextBox>"
            + "<CheckBox " + Enabled + " IsChecked='{Binding " + BindingExpSurrogate + ",Mode=TwoWay, Converter={StaticResource StringToBoolConverter}}' Visibility='{Binding IsBool,Converter={StaticResource VisibilityConverter}}'  VerticalAlignment='Center' HorizontalAlignment='Center' ></CheckBox>"
            + "</Converter:ExtendedGridForSurrogate>"
        + "</DataTemplate>";
            dt = (DataTemplate)XamlReader.Load(xaml);
            return dt;
        }

        private DataTemplate CreateExeSurrogate(string BindingExpSurrogate)
        {
            DataTemplate dt = null;
            string xaml = @"<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' xmlns:Converter=""clr-namespace:PalashDynamics.IVF;assembly=PalashDynamics.IVF"">"
              + "<Converter:ExtendedGridForSurrogate>"
              + "<Converter:ExtendedGridForSurrogate.Resources>"
              + "<Converter:VisibilityConverter  x:Key='VisibilityConverter'></Converter:VisibilityConverter>"
              + "<Converter:StringToBoolConverter  x:Key='StringToBoolConverter'></Converter:StringToBoolConverter>"
              + "<Converter:TherapyToTimeConverter  x:Key='TherapyToTimeConverter'></Converter:TherapyToTimeConverter>"
              + "<Converter:TherapyToDateTimeConverter  x:Key='TherapyToDateTimeConverter'></Converter:TherapyToDateTimeConverter>"
              + "<Converter:ToggleBooleanValueConverter  x:Key='ToggleBooleanValueConverter'></Converter:ToggleBooleanValueConverter>"
              + "<Converter:TherapyToVisibilityConverter  x:Key='TherapyToVisibilityConverter'></Converter:TherapyToVisibilityConverter>"
              + "<Converter:NumberToBrushConverter  x:Key='NumberToBrushConverter' Id='{Binding Id}'></Converter:NumberToBrushConverter>"
               + "</Converter:ExtendedGridForSurrogate.Resources>"
              + @"<TextBlock  Text='" + BindingExpSurrogate + "' Visibility='Collapsed'/>"
               + "<TextBox Background='{Binding CurrentTherapy, Converter={StaticResource NumberToBrushConverter},ConverterParameter=" + BindingExpSurrogate + "}' Text='{Binding " + BindingExpSurrogate + ",Mode=TwoWay}' Visibility='{Binding IsText,Converter={StaticResource VisibilityConverter}}' VerticalAlignment='Center' MaxLength='6'><ToolTipService.ToolTip><ToolTip  DataContext='{Binding CurrentTherapy,Converter={StaticResource TherapyToDateTimeConverter},ConverterParameter=" + BindingExpSurrogate + "}' Content='{Binding TimeOfDay,Converter={StaticResource TherapyToTimeConverter}}'></ToolTip></ToolTipService.ToolTip></TextBox>"
                  + "<CheckBox  IsChecked='{Binding " + BindingExpSurrogate + ",Mode=TwoWay,Converter={StaticResource StringToBoolConverter}}' Visibility='{Binding IsBool,Converter={StaticResource VisibilityConverter}}' IsEnabled='{Binding IsBool,Converter={StaticResource ToggleBooleanValueConverter}}' VerticalAlignment='Center' HorizontalAlignment='Center'></CheckBox>"
                //+ "<CheckBox  IsChecked='{Binding " + BindingExp + ",Mode=TwoWay,Converter={StaticResource StringToBoolConverter}}' IsEnabled='{Binding "+ BindingExp +",Converter={StaticResource NegateValueConverter}}' VerticalAlignment='Center' HorizontalAlignment='Center'></CheckBox>"
                  + "</Converter:ExtendedGridForSurrogate>"
          + "</DataTemplate>";
            //NumberToBrushConverter
            dt = (DataTemplate)XamlReader.Load(xaml);
            return dt;
        }
        #endregion
        private void TherpayExeRowDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExeGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }

        private void ExeGrid_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {

        }

        private void ExeGrid_CurrentCellChanged(object sender, EventArgs e)
        {

        }
        #region follicular monitoring click
        private void cmdFollicularMonitoring_Click(object sender, RoutedEventArgs e)
        {
            frmFollicularMonitoring FMWin = new frmFollicularMonitoring();
            FMWin.Show();
            FMWin.Title = "Follicular Monitoring Details : " + CoupleDetails.FemalePatient.FirstName + " " + CoupleDetails.FemalePatient.LastName + " (Cyclecode :" + ((clsPlanTherapyVO)dgtheropyList.SelectedItem).Cyclecode + ")";
            FMWin.dgFollicularMonitoring.ItemsSource = null;
            FMWin.dgFollicularMonitoring.ItemsSource = SelectedTherapyDetails.FollicularMonitoringList;
            FMWin.TherapyId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
            FMWin.TherapyUnitId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
            FMWin.FollicularMonitoringAttachemntView_ChildClick += new RoutedEventHandler(WinFollicularMonitoringAttachemntView_ChildClick);
        }
        void WinFollicularMonitoringAttachemntView_ChildClick(object sender, RoutedEventArgs e)
        {
            clsIVFDashboard_GetFollicularMonitoringSizeDetailsBizActionVO BizAction = new clsIVFDashboard_GetFollicularMonitoringSizeDetailsBizActionVO();
            BizAction.FollicularMonitoringSizeList = new List<clsFollicularMonitoringSizeDetails>();
            BizAction.FollicularID = ((clsFollicularMonitoring)((frmFollicularMonitoring)sender).dgFollicularMonitoring.SelectedItem).ID;
            BizAction.UnitID = ((clsFollicularMonitoring)((frmFollicularMonitoring)sender).dgFollicularMonitoring.SelectedItem).UnitID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {

                    ((clsFollicularMonitoring)((frmFollicularMonitoring)sender).dgFollicularMonitoring.SelectedItem).SizeList = new List<clsFollicularMonitoringSizeDetails>();
                    ((clsFollicularMonitoring)((frmFollicularMonitoring)sender).dgFollicularMonitoring.SelectedItem).SizeList = ((clsIVFDashboard_GetFollicularMonitoringSizeDetailsBizActionVO)arg.Result).FollicularMonitoringSizeList;

                    ((frmFollicularMonitoring)sender).DialogResult = false;
                    ARTFollicularMonitoring addFollic = new ARTFollicularMonitoring();
                    //.....................
                    addFollic.DataContext = null;
                    //..........................
                    addFollic.fillDoctor(((clsFollicularMonitoring)((frmFollicularMonitoring)sender).dgFollicularMonitoring.SelectedItem).PhysicianID);
                    addFollic.DataContext = ((clsFollicularMonitoring)((frmFollicularMonitoring)sender).dgFollicularMonitoring.SelectedItem);
                    addFollic.AttachedFileContents = ((clsFollicularMonitoring)((frmFollicularMonitoring)sender).dgFollicularMonitoring.SelectedItem).AttachmentFileContent;
                    addFollic.dtpFollicularDate.IsEnabled = false;
                    FollicularID = ((clsFollicularMonitoring)((frmFollicularMonitoring)sender).dgFollicularMonitoring.SelectedItem).ID;
                    FollicularUnitID = ((clsFollicularMonitoring)((frmFollicularMonitoring)sender).dgFollicularMonitoring.SelectedItem).UnitID;
                    addFollic.OnSaveButton_Click += new RoutedEventHandler(addFollic_OnSaveButton_Click);
                    addFollic.Show();

                    addFollic.Closed += new EventHandler(addFollic_Closing);

                    SelectedTherapyDetails.TherapyDetails = new clsPlanTherapyVO();
                    if ((((clsPlanTherapyVO)dgtheropyList.SelectedItem)).IsClosed)
                        addFollic.OKButton.IsEnabled = (a == true) ? false : true;
                    // End
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void addFollic_Closing(object sender, EventArgs e)
        {

            frmFollicularMonitoring FMWin = new frmFollicularMonitoring();
            FMWin.Show();
            FMWin.Title = "Follicular Monitoring Details : " + CoupleDetails.FemalePatient.FirstName + " " + CoupleDetails.FemalePatient.LastName + " (Cyclecode :" + ((clsPlanTherapyVO)dgtheropyList.SelectedItem).Cyclecode + ")";
            FMWin.dgFollicularMonitoring.ItemsSource = null;
            FMWin.dgFollicularMonitoring.ItemsSource = SelectedTherapyDetails.FollicularMonitoringList;
            FMWin.TherapyId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
            FMWin.TherapyUnitId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
            FMWin.FollicularMonitoringAttachemntView_ChildClick += new RoutedEventHandler(WinFollicularMonitoringAttachemntView_ChildClick);
        }

        void addFollic_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wait.Show();
                clsIVFDashboard_UpdateFollicularMonitoringBizActionVO BizAction = new clsIVFDashboard_UpdateFollicularMonitoringBizActionVO();
                BizAction.FollicularID = FollicularID;
                BizAction.FollicularMonitoringDetial = ((clsFollicularMonitoring)((ARTFollicularMonitoring)sender).DataContext);
                BizAction.FollicularMonitoringDetial.UnitID = FollicularUnitID;
                BizAction.FollicularMonitoringDetial.Date = ((ARTFollicularMonitoring)sender).dtpFollicularDate.SelectedDate.Value.Date;
                BizAction.FollicularMonitoringDetial.Date = BizAction.FollicularMonitoringDetial.Date.Value.Add(((ARTFollicularMonitoring)sender).txtTime.Value.Value.TimeOfDay);
                BizAction.FollicularMonitoringDetial.AttachmentFileContent = ((ARTFollicularMonitoring)sender).AttachedFileContents;
                BizAction.FollicularMonitoringDetial.PhysicianID = ((MasterListItem)((ARTFollicularMonitoring)sender).cmbPhysician.SelectedItem).ID;
                BizAction.FollicularMonitoringDetial.FollicularNoList = string.Empty;
                BizAction.FollicularMonitoringDetial.LeftSizeList = string.Empty;
                BizAction.FollicularMonitoringDetial.RightSizeList = string.Empty;
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
                        //setupPage(((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID, 3);
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Follicular Monitoring Details Updated Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        // setupPage(((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID, 2);
                        // setupPage(SelectedTherapyDetails.TherapyDetails.ID, 0);
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
        #endregion

        #region New Drug
        private void cmdNewExeDrug_Click(object sender, RoutedEventArgs e)
        {
            isDrugForSurrogate = false;
            AddDrug addDrug = new AddDrug();
            addDrug.IsEdit = false;
            addDrug.TherpayExeId = 0;
            addDrug.TherapyStartDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TherapyStartDate;
            addDrug.DrugId = 0;
            addDrug.OnSaveButton_Click += new RoutedEventHandler(addDrug_OnSaveButton_Click);
            addDrug.Show();
        }
        public void addDrug_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (((AddDrug)sender).IsEdit == false)
            {

                if (isDrugForSurrogate == true)
                {
                    var result = from c in SelectedTherapyDetails.TherapyExecutionListSurrogate
                                 where c.ThearpyTypeDetailId == ((MasterListItem)((AddDrug)sender).CmbDrug.SelectedItem).ID
                                 select c as clsTherapyExecutionVO;

                    if (result.ToList().Count == 0)
                    {
                        string kEY = ((AddDrug)sender).txtForDays.Text.Trim();
                        string VALUE = ((AddDrug)sender).txtDosage.Text.Trim();
                        clsIVFDashboard_AddPlanTherapyBizActionVO BizAction = new clsIVFDashboard_AddPlanTherapyBizActionVO();
                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        BizAction.TherapyDetails = new clsPlanTherapyVO();
                        BizAction.TherapyDetails.ID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                        BizAction.TherapyDetails.UnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                        BizAction.TherapyDetails.ThreapyExecutionId = 0;
                        BizAction.TherapyDetails.IsSurrogateDrug = isDrugForSurrogate;
                        BizAction.TherapyDetails.IsSurrogateCalendar = isDrugForSurrogate;
                        BizAction.TherapyDetails.TherapyExeTypeID = (int)TherapyGroup.Drug;
                        BizAction.TherapyDetails.TherapyStartDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TherapyStartDate;
                        BizAction.TherapyDetails.PhysicianId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PhysicianId;
                        BizAction.TherapyDetails.TabID = 2;
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
                                setupPage(((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID, 2);
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Drug Added Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();

                            }
                        };
                        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client.CloseAsync();
                        #endregion
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Drug Already exists", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();

                    }
                }
                else
                {
                    var result = from c in SelectedTherapyDetails.TherapyExecutionList
                                 where c.ThearpyTypeDetailId == ((MasterListItem)((AddDrug)sender).CmbDrug.SelectedItem).ID
                                 select c as clsTherapyExecutionVO;

                    if (result.ToList().Count == 0)
                    {
                        string kEY = ((AddDrug)sender).txtForDays.Text.Trim();
                        string VALUE = ((AddDrug)sender).txtDosage.Text.Trim();
                        clsIVFDashboard_AddPlanTherapyBizActionVO BizAction = new clsIVFDashboard_AddPlanTherapyBizActionVO();
                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        BizAction.TherapyDetails = new clsPlanTherapyVO();
                        BizAction.TherapyDetails.ID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                        BizAction.TherapyDetails.UnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                        BizAction.TherapyDetails.ThreapyExecutionId = 0;
                        BizAction.TherapyDetails.IsSurrogateDrug = isDrugForSurrogate;
                        BizAction.TherapyDetails.IsSurrogateCalendar = isDrugForSurrogate;
                        BizAction.TherapyDetails.TherapyExeTypeID = (int)TherapyGroup.Drug;
                        BizAction.TherapyDetails.TherapyStartDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TherapyStartDate;
                        BizAction.TherapyDetails.PhysicianId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PhysicianId;
                        BizAction.TherapyDetails.TabID = 2;
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
                                setupPage(((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID, 2);
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Drug Added Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();

                            }
                        };
                        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client.CloseAsync();
                        #endregion
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Drug Already exists", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();

                    }
                }
            }
            else if (((AddDrug)sender).IsEdit == true)
            {
                string kEY = ((AddDrug)sender).txtForDays.Text.Trim();
                string VALUE = ((AddDrug)sender).txtDosage.Text.Trim();
                clsIVFDashboard_AddPlanTherapyBizActionVO BizAction = new clsIVFDashboard_AddPlanTherapyBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                BizAction.TherapyDetails = new clsPlanTherapyVO();
                BizAction.TherapyDetails.ID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                BizAction.TherapyDetails.UnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                BizAction.TherapyDetails.ThreapyExecutionId = ((AddDrug)sender).TherpayExeId;
                BizAction.TherapyDetails.TherapyExeTypeID = (int)TherapyGroup.Drug;
                BizAction.TherapyDetails.TherapyStartDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TherapyStartDate;
                BizAction.TherapyDetails.PhysicianId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PhysicianId;
                BizAction.TherapyDetails.TabID = 2;
                BizAction.TherapyDetails.Day = kEY;
                BizAction.TherapyDetails.Value = VALUE;
                BizAction.TherapyDetails.IsSurrogateDrug = ((AddDrug)sender).IsSurrogateDrug;
                BizAction.TherapyDetails.IsSurrogateCalendar = ((AddDrug)sender).IsSurrogateCalendar;
                BizAction.TherapyDetails.DrugTime = ((AddDrug)sender).dtthreapystartdate.SelectedDate.Value.Date;
                BizAction.TherapyDetails.DrugTime = BizAction.TherapyDetails.DrugTime.Value.Add(((AddDrug)sender).txtTime.Value.Value.TimeOfDay);
                BizAction.TherapyDetails.DrugNotes = ((AddDrug)sender).txtDrugNotes.Text.Trim();
                BizAction.TherapyDetails.ThearpyTypeDetailId = ((MasterListItem)((AddDrug)sender).CmbDrug.SelectedItem).ID;
                #region Service Call (Check Validation)

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        setupPage(((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID, 2);

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

        #region Modify Therapy Details
        public bool GenValidation()
        {
            //if()
            if (cmbPlannedTreatment.SelectedItem == null)
            {

                cmbPlannedTreatment.TextBox.SetValidation("Please Select Planned Treatment");
                cmbPlannedTreatment.TextBox.RaiseValidationError();
                cmbPlannedTreatment.Focus();
                return false;
            }
            else if (((MasterListItem)cmbPlannedTreatment.SelectedItem).ID == 0)
            {

                cmbPlannedTreatment.TextBox.SetValidation("Please Select Planned Treatment");
                cmbPlannedTreatment.TextBox.RaiseValidationError();
                cmbPlannedTreatment.Focus();
                return false;
            }
            else if (cmbMainIndication.SelectedItem == null)
            {
                cmbPlannedTreatment.ClearValidationError();
                cmbMainIndication.TextBox.SetValidation("Please Select Main Indication");
                cmbMainIndication.TextBox.RaiseValidationError();
                cmbMainIndication.Focus();
                return false;
            }

            else if (((MasterListItem)cmbMainIndication.SelectedItem).ID == 0)
            {
                cmbPlannedTreatment.ClearValidationError();
                cmbMainIndication.TextBox.SetValidation("Please Select Main Indication");
                cmbMainIndication.TextBox.RaiseValidationError();
                cmbMainIndication.Focus();
                return false;
            }
            else if (cmbPhysician.SelectedItem == null)
            {
                cmbPlannedTreatment.ClearValidationError();
                cmbMainIndication.ClearValidationError();
                cmbPhysician.TextBox.SetValidation("Please Select Physician");
                cmbPhysician.TextBox.RaiseValidationError();
                cmbPhysician.Focus();
                return false;
            }
            else if (((MasterListItem)cmbPhysician.SelectedItem).ID == 0)
            {
                cmbPlannedTreatment.ClearValidationError();

                cmbMainIndication.ClearValidationError();
                cmbPhysician.TextBox.SetValidation("Please Select Physician");
                cmbPhysician.TextBox.RaiseValidationError();
                cmbPhysician.Focus();
                return false;
            }
            else if (chkSurrogate.IsChecked == true && txtSurrogateCode.Text == string.Empty)
            {
                string msgTitle = "Palash";
                string msgText = "Please Link surrogate ";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWin.Show();
                return false;
            }
            else if (((MasterListItem)cmbPlannedTreatment.SelectedItem).ID == 1 || ((MasterListItem)cmbPlannedTreatment.SelectedItem).ID == 2 || ((MasterListItem)cmbPlannedTreatment.SelectedItem).ID == 3 || ((MasterListItem)cmbPlannedTreatment.SelectedItem).ID == 11)
            {
                if (dtOPUDate.SelectedDate == null)
                {
                    cmbPlannedTreatment.ClearValidationError();
                    cmbMainIndication.ClearValidationError();
                    cmbPhysician.ClearValidationError();
                    dtOPUDate.SetValidation("Please Select OPU Date");
                    dtOPUDate.RaiseValidationError();
                    dtOPUDate.Focus();
                    return false;
                }
                else
                {
                    return true;
                }

            }
            else
            {
                cmbPlannedTreatment.ClearValidationError();
                cmbMainIndication.ClearValidationError();
                cmbPhysician.ClearValidationError();
                dtOPUDate.ClearValidationError();
                return true;
            }
        }

        private void cmdSaveTherapy_Click(object sender, RoutedEventArgs e)
        {
            if (GenValidation())
            {
                string msgTitle = "Palash";
                string msgText = "Are You Sure \n  You Want To Modify Therapy General Details?";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += (result) =>
                {
                    if (result == MessageBoxResult.Yes)
                    {
                        saveUpdateGeneralDetails(((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID, (long)IVFDashBoardTab.TabOverview, 0, "Therapy General Details Modified Successfully");
                    }

                };
                msgWin.Show();
            }
        }

        void frmARTGeneralDetails_OnSaveButton_Click(object sender, EventArgs e)
        {
            setupPage(0, 0);
        }
        private void saveUpdateGeneralDetails(long ID, long TabID, long DocumentId, String Msg)
        {
            try
            {
                clsIVFDashboard_AddPlanTherapyBizActionVO BizAction = new clsIVFDashboard_AddPlanTherapyBizActionVO();
                BizAction.TherapyDetails = new clsPlanTherapyVO();
                BizAction.TherapyDetails = ((clsPlanTherapyVO)grdBackPanel.DataContext);
                BizAction.TherapyDetails.ID = ID;
                BizAction.TherapyDetails.TabID = TabID;
                BizAction.TherapyDetails.PatientUintId = CoupleDetails.FemalePatient.UnitId;
                BizAction.TherapyDetails.CoupleId = CoupleDetails.CoupleId;
                BizAction.TherapyDetails.CoupleUnitId = CoupleDetails.CoupleUnitId;

                if (chkSurrogate.IsChecked == true)
                    BizAction.TherapyDetails.AttachedSurrogate = true;
                else
                    BizAction.TherapyDetails.AttachedSurrogate = false;
                BizAction.TherapyDetails.SurrogateID = SurrogateID;
                BizAction.TherapyDetails.SurrogateUnitID = SurrogateUnitID;
                BizAction.TherapyDetails.SurrogateMRNo = txtSurrogateCode.Text;
                BizAction.TherapyDetails.PatientId = CoupleDetails.FemalePatient.PatientID;
                BizAction.TherapyDetails.PatientUintId = CoupleDetails.FemalePatient.UnitId;

                #region Data Used For Therapy General Details
                if (TabID == 1)
                {
                    BizAction.TherapyDetails.OPUtDate = dtOPUDate.SelectedDate;

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

                    if (txtPlannedNoOfEmbryos.Text != null)
                    {
                        BizAction.TherapyDetails.PlannedEmbryos = txtPlannedNoOfEmbryos.Text.Trim();
                    }
                    if (txtLongtermMedication.Text != null)
                    {
                        BizAction.TherapyDetails.LongtermMedication = txtLongtermMedication.Text.Trim();
                    }
                    if (cmbSimulation.SelectedItem != null)
                    {
                        BizAction.TherapyDetails.ExternalSimulationID = ((MasterListItem)cmbSimulation.SelectedItem).ID;
                    }
                    if (cmbSpermCollection.SelectedItem != null)
                    {
                        BizAction.TherapyDetails.PlannedSpermCollectionID = ((MasterListItem)cmbSpermCollection.SelectedItem).ID;
                    }
                }
                #endregion

                #region Service Call (Check Validation)
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", Msg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

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
        #endregion


        #region pill check uncheck event
        private void chkPill_Checked(object sender, RoutedEventArgs e)
        {
            if (chkPill.IsChecked == true)
            {
                dtStartDate.IsEnabled = true;
                dtEndDate.IsEnabled = true;
            }
        }

        private void chkPill_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chkPill.IsChecked == false)
            {
                dtStartDate.IsEnabled = false;
                dtEndDate.IsEnabled = false;
            }
        }
        #endregion

        #region Tab Selection Changed
        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TabOverview != null)
            {
                if (TabOverview.IsSelected)
                {
                    setupPage(((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID, 0);
                }
            }

            if (TabOPU != null)
            {
                if (TabOPU.IsSelected)
                {
                    UIElement mydata5 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmOPU") as UIElement;
                    FrOPU = new frmOPU();
                    FrOPU = (frmOPU)mydata5;
                    FrOPU.CoupleDetails = CoupleDetails;
                    //FrOPU.PlanTherapyID = SelectedTherapyDetails.TherapyDetails.ID;
                    // FrOPU.PlanTherapyUnitID = SelectedTherapyDetails.TherapyDetails.UnitID;
                    FrOPU.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                    FrOPU.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                    //FrOPU.OPUDate = Convert.ToDateTime(SelectedTherapyDetails.TherapyDetails.OPUtDate);
                    FrOPU.OPUDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).OPUtDate;
                    FrOPU.PlannedTreatmentID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID;
                    FrOPU.IsClosed = IsClosed;
                    FrOPU.OnSaveButton_Click += new RoutedEventHandler(OnSaveButtonOPU_Click);
                    OPUContent.Content = FrOPU;
                }
            }
            if (TabCryoPreservation != null)
            {
                if (TabCryoPreservation.IsSelected)
                {
                    UIElement mydata5 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmCryoPreservation") as UIElement;
                    frmCryo = new frmCryoPreservation();
                    frmCryo = (frmCryoPreservation)mydata5;
                    frmCryo.CoupleDetails = CoupleDetails;
                    frmCryo.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                    frmCryo.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                    frmCryo.PlanTherapyVO = ((clsPlanTherapyVO)dgtheropyList.SelectedItem);
                    frmCryo.IsClosed = IsClosed;
                    frmCryo.IsEmbryoDonation = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsEmbryoDonation;
                    CryopreservationContent.Content = frmCryo;
                }
            }

            //if (TabIUI != null)
            //{
            //    if (TabIUI.IsSelected)
            //    {
            //        UIElement mydata2 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.FrmIUI") as UIElement;
            //        frmIUI = new FrmIUI();
            //        frmIUI = (FrmIUI)mydata2;
            //        frmIUI.CoupleDetails = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails;
            //        frmIUI.PlanTherapyID = SelectedTherapyDetails.TherapyDetails.ID;
            //        frmIUI.PlanTherapyUnitID = SelectedTherapyDetails.TherapyDetails.UnitID;
            //        frmIUI.IsClosed = IsClosed;
            //        IUIContent.Content = frmIUI;



            //UIElement mydata2 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmIUIComeSemenWash") as UIElement;
            //frmIUI = new frmIUIComeSemenWash();
            //frmIUI = (frmIUIComeSemenWash)mydata2;
            //frmIUI.CoupleDetails = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails;
            //frmIUI.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
            //frmIUI.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
            //frmIUI.IsClosed = IsClosed;
            //IUIContent.Content = frmIUI;
            //    }
            //}
            if (TabLutualPhase != null)
            {
                if (TabLutualPhase.IsSelected)
                {
                    fillLutealPhaseDetails();
                }
            }
            if (Taboutcome != null)
            {
                if (Taboutcome.IsSelected)
                {
                    fillOutcomeDetails();
                }
            }
            if (TabGraphicalRepresentation != null)
            {
                if (TabGraphicalRepresentation.IsSelected)
                {
                    UIElement mydata1 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmGraphicalRepresentationofLabDays") as UIElement;
                    grpRep = new frmGraphicalRepresentationofLabDays();
                    grpRep = (frmGraphicalRepresentationofLabDays)mydata1;
                    grpRep.CoupleDetails = CoupleDetails;
                    grpRep.StartDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TherapyStartDate.Value.Date;
                    grpRep.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                    grpRep.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                    grpRep.PlannedTreatmentID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID;

                    // grpRep.PlannedNoOfEmb = ((clsIVFDashboard_OPUVO)this.DataContext).OocyteRetrived;                  
                    AdmissionContent.Content = grpRep;
                }
            }
            if (Tabtransfer != null)
            {
                if (Tabtransfer.IsSelected)
                {
                    UIElement mydata4 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmTransfer") as UIElement;
                    ET = new frmTransfer();
                    ET = (frmTransfer)mydata4;
                    ET.CoupleDetails = CoupleDetails;
                    ET.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                    ET.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                    ET.IsClosed = IsClosed;
                    TransferContent.Content = ET;
                }
            }
            if (TabOnlyTransfer != null)
            {
                if (TabOnlyTransfer.IsSelected)
                {
                    UIElement mydata4 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmOnlyET") as UIElement;
                    onlyET = new frmOnlyET();
                    onlyET = (frmOnlyET)mydata4;
                    onlyET.CoupleDetails = CoupleDetails;
                    onlyET.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                    onlyET.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                    onlyET.IsClosed = IsClosed;
                    OnlyTransferContent.Content = onlyET;
                }
            }
            if (TabOnlyVitrification != null)
            {
                if (TabOnlyVitrification.IsSelected)
                {
                    SSemenMaster();
                }
            }
            // For Birth Details
            if (Tabbirthdetails != null)
            {
                if (Tabbirthdetails.IsSelected != null)
                {
                    UIElement mydata3 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmBirth") as UIElement;
                    Birth = new frmBirth();
                    Birth = (frmBirth)mydata3;

                    Birth.CoupleDetails = CoupleDetails;
                    Birth.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                    Birth.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                    Birth.ChildID = ChildID;
                    Birth.IsPregnancyAchived = IsPregnancyAchived;
                    Birth.PregnancyAchivedDate = PregnancyAchivedDate;

                    BirthContent.Content = Birth;
                }
            }
            if (TabDocument != null)
            {
                if (TabDocument.IsSelected)
                {
                    UIElement mydata2 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmDocumentforDashTherapy") as UIElement;
                    Doc = new frmDocumentforDashTherapy();
                    Doc = (frmDocumentforDashTherapy)mydata2;
                    Doc.CoupleDetails = CoupleDetails;
                    Doc.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                    Doc.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
                    Doc.IsClosed = IsClosed;
                    Document.Content = Doc;

                }
            }
        }
        private void OnSaveButtonOPU_Click(object sender, RoutedEventArgs e)
        {
            tabItems.Add(TabGraphicalRepresentation);
            tabItems.Add(TabCryoPreservation);
            RequestXML("IVFDashboardSubmenu");
        }

        #endregion

        #region Luteal Phase
        private void fillLutealPhaseDetails()
        {
            clsIVFDashboard_GetLutealPhaseBizActionVO BizAction = new clsIVFDashboard_GetLutealPhaseBizActionVO();
            BizAction.Details = new cls_IVFDashboard_LutualPhaseVO();
            BizAction.Details.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
            BizAction.Details.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
            if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails != null)
            {
                BizAction.Details.PatientID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.PatientID;
                BizAction.Details.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.UnitId;
            }
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsIVFDashboard_GetLutealPhaseBizActionVO)arg.Result).Details != null)
                    {
                        this.DataContext = ((clsIVFDashboard_GetLutealPhaseBizActionVO)arg.Result).Details;
                        if (IsClosed == true)
                        {
                            cmdNewLP.IsEnabled = false;
                        }
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void cmdNewLP_Click(object sender, RoutedEventArgs e)
        {
            if (txtLutealSupport.Text != string.Empty || txtLutealRemarks.Text != string.Empty)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                msgW.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please enter the details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                msgW.Show();

            }
        }
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Save();
        }
        private void Save()
        {
            clsIVFDashboard_AddUpdateLutealPhaseBizActionVO BizAction = new clsIVFDashboard_AddUpdateLutealPhaseBizActionVO();
            BizAction.LutualPhaseDetails = new cls_IVFDashboard_LutualPhaseVO();
            BizAction.LutualPhaseDetails.ID = ((cls_IVFDashboard_LutualPhaseVO)this.DataContext).ID;
            BizAction.LutualPhaseDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.PatientID;
            BizAction.LutualPhaseDetails.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.UnitId;
            BizAction.LutualPhaseDetails.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
            BizAction.LutualPhaseDetails.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
            BizAction.LutualPhaseDetails.LutealSupport = txtLutealSupport.Text;
            BizAction.LutualPhaseDetails.LutealRemark = txtLutealRemarks.Text;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Luteal Phase Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }
        #endregion

        #region Donor Details click
        private void cmddonorDetails_Click(object sender, RoutedEventArgs e)
        {
            SemenDetails_DashBoard win = new SemenDetails_DashBoard();
            win.coupleDetails = CoupleDetails;
            win.PatientID = CoupleDetails.FemalePatient.PatientID;
            win.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            win.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
            win.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
            win.PlannedtreatmentID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID;
            win.SelectedTherapyDetails = ((clsPlanTherapyVO)dgtheropyList.SelectedItem);
            win.IsClosed = IsClosed;
            win.Save_ChildClick += new RoutedEventHandler(SaveDonorChild_Click);
            win.Show();

        }
        private void SaveDonorChild_Click(object sender, RoutedEventArgs e)
        {
            setupPage(((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID, 0);
        }
        #endregion
        private void AccordionItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void acc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnSearchTherapy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rdoActive_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rdoClosed_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rdoSuccessful_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rdoUnsuccessful_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rdoAll_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtPatientName_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtPatientName_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void txtPatientName_TextChanged(object sender, RoutedEventArgs e)
        {

        }

        private void btnPatientName_Click(object sender, RoutedEventArgs e)
        {

        }
        #region Calling EMR
        // Templte For Female History = 19
        private void cmdhistoryFemale_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.FemalePatient != null && CoupleDetails.FemalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                ModuleName = "DataDrivenApplication";
                Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Female Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                msgW1.Show();
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
                ((IInitiateCIMS)myData).Initiate("122");
                ((ChildWindow)myData).Title = "Female History";
                ((ChildWindow)myData).Show();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Templte For Male History = 22
        private void cmdhistorymale_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                ModuleName = "DataDrivenApplication";
                Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_MaleHistory);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                msgW1.Show();
            }

        }
        void c_OpenReadCompleted_MaleHistory(object sender, OpenReadCompletedEventArgs e)
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
                ((IInitiateCIMS)myData).Initiate("22");
                ((ChildWindow)myData).Title = "Male History";
                ((ChildWindow)myData).Show();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // Templte For Female Finding = 27
        private void cmdFemaleFindings_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.FemalePatient != null && CoupleDetails.FemalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                ModuleName = "DataDrivenApplication";
                Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_FeamleFinding);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Female Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void c_OpenReadCompleted_FeamleFinding(object sender, OpenReadCompletedEventArgs e)
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
                ((IInitiateCIMS)myData).Initiate("27");
                ((ChildWindow)myData).Title = "Female Findings";
                ((ChildWindow)myData).Show();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Templte For male Finding=43
        private void cmdmaleFindings_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                ModuleName = "DataDrivenApplication";
                Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_MaleFinding);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void c_OpenReadCompleted_MaleFinding(object sender, OpenReadCompletedEventArgs e)
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
                ((IInitiateCIMS)myData).Initiate("43");
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                ((ChildWindow)myData).Title = "Male Findings";
                ((ChildWindow)myData).Show();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FemaleUSG=28
        private void cmdFemaleUSG_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.FemalePatient != null && CoupleDetails.FemalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                ModuleName = "DataDrivenApplication";
                Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_FemaleUSG);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Female Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void c_OpenReadCompleted_FemaleUSG(object sender, OpenReadCompletedEventArgs e)
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
                ((IInitiateCIMS)myData).Initiate("28");
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                ((ChildWindow)myData).Title = "USG";
                ((ChildWindow)myData).Show();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FemaleHystroscopy=23
        private void cmdFemaleHystroscopy_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.FemalePatient != null && CoupleDetails.FemalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                ModuleName = "DataDrivenApplication";
                Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_FemaleHystroscopy);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Female Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void c_OpenReadCompleted_FemaleHystroscopy(object sender, OpenReadCompletedEventArgs e)
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
                ((IInitiateCIMS)myData).Initiate("23");
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                ((ChildWindow)myData).Title = "Hysteroscopy";
                ((ChildWindow)myData).Show();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //FemaleLaproscopy=24
        private void cmdFemaleLaproscopy_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.FemalePatient != null && CoupleDetails.FemalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                ModuleName = "DataDrivenApplication";
                Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_FemaleLaproscopy);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Female Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void c_OpenReadCompleted_FemaleLaproscopy(object sender, OpenReadCompletedEventArgs e)
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
                ((IInitiateCIMS)myData).Initiate("24");
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                ((ChildWindow)myData).Title = "Laproscopy";
                ((ChildWindow)myData).Show();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //FemaleTBPCR=30
        private void cmdFemaleTBPCR_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.FemalePatient != null && CoupleDetails.FemalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                ModuleName = "DataDrivenApplication";
                Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_FemaleTBPCR);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Female Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void c_OpenReadCompleted_FemaleTBPCR(object sender, OpenReadCompletedEventArgs e)
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
                ((IInitiateCIMS)myData).Initiate("30");
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                ((ChildWindow)myData).Title = "TBPCR";
                ((ChildWindow)myData).Show();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //FemalePCT=24
        private void cmdFemalePCT_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.FemalePatient != null && CoupleDetails.FemalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                ModuleName = "DataDrivenApplication";
                Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_FemalePCT);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Female Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void c_OpenReadCompleted_FemalePCT(object sender, OpenReadCompletedEventArgs e)
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
                ((IInitiateCIMS)myData).Initiate("44");
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                ((ChildWindow)myData).Title = "PCT";
                ((ChildWindow)myData).Show();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //FemaleHSG=62
        private void cmdFemaleHSG_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.FemalePatient != null && CoupleDetails.FemalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                ModuleName = "DataDrivenApplication";
                Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_FemaleHSG);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Female Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void c_OpenReadCompleted_FemaleHSG(object sender, OpenReadCompletedEventArgs e)
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
                ((IInitiateCIMS)myData).Initiate("62");
                ((ChildWindow)myData).Title = "HSG";
                ((ChildWindow)myData).Show();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //SemenWash=37
        private void cmdSemenWash_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                ModuleName = "DataDrivenApplication";
                Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_SemenWash);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void c_OpenReadCompleted_SemenWash(object sender, OpenReadCompletedEventArgs e)
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
                ((IInitiateCIMS)myData).Initiate("37");
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                ((ChildWindow)myData).Title = "Semen Wash";
                ((ChildWindow)myData).Show();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //maleSemenCultureNSentity=39
        private void cmdmaleSemenCultureNSentity_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                ModuleName = "DataDrivenApplication";
                Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_SemenCultureNSentity);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void c_OpenReadCompleted_SemenCultureNSentity(object sender, OpenReadCompletedEventArgs e)
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
                ((IInitiateCIMS)myData).Initiate("39");
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                ((ChildWindow)myData).Title = "Semen Culture And Sensitivity";
                ((ChildWindow)myData).Show();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //maleSemenExam=40
        private void cmdmaleSemenExam_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                ModuleName = "DataDrivenApplication";
                Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_maleSemenExam);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void c_OpenReadCompleted_maleSemenExam(object sender, OpenReadCompletedEventArgs e)
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
                ((IInitiateCIMS)myData).Initiate("40");
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                ((ChildWindow)myData).Title = "Semen Examination";
                ((ChildWindow)myData).Show();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //SemenSurvival=38
        private void cmdmaleSemenSurvival_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                ModuleName = "DataDrivenApplication";
                Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_maleSemenSurvival);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void c_OpenReadCompleted_maleSemenSurvival(object sender, OpenReadCompletedEventArgs e)
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
                ((IInitiateCIMS)myData).Initiate("38");
                ((ChildWindow)myData).Title = "Semen Survival";
                ((ChildWindow)myData).Show();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // maleTESE ID- 57
        private void cmdmaleTESE_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                ModuleName = "DataDrivenApplication";
                Action = "DataDrivenApplication.Forms.frmChildWinEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_TESE);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void c_OpenReadCompleted_TESE(object sender, OpenReadCompletedEventArgs e)
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
                ((IInitiateCIMS)myData).Initiate("63");
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                ((ChildWindow)myData).Title = "TESE";
                ((ChildWindow)myData).Show();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Calling Form

        private void cmdFemaleEmbryoBank_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.FemalePatient.PatientID != null && CoupleDetails.FemalePatient.PatientID != 0)
            {
                long PatientID = CoupleDetails.FemalePatient.PatientID;
                frmEmbryoBank win = new frmEmbryoBank();
                win.MRNO = CoupleDetails.FemalePatient.MRNo;
                win.Title = "Embryo Bank ( " + CoupleDetails.FemalePatient.FirstName + " " + CoupleDetails.FemalePatient.LastName + " )";
                win.Show();
            }
            else
            {

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Female Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }

        }

        private void cmdMaleEmbryoBank_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient.PatientID != null && CoupleDetails.MalePatient.PatientID != 0)
            {
                long PatientID = CoupleDetails.FemalePatient.PatientID;
                frmSpermBank win = new frmSpermBank();
                win.MRNO = CoupleDetails.MalePatient.MRNo;
                win.Title = "Sprem Bank ( " + CoupleDetails.MalePatient.FirstName + " " + CoupleDetails.MalePatient.LastName + " )";
                win.Show();
            }
            else
            {

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }

        }



        // Lab Test For male
        private void cmdmaleLabtest_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID != 0)
            {
                long PatientID = CoupleDetails.MalePatient.PatientID;
                long PatientUnitID = CoupleDetails.MalePatient.UnitId;
                frmLabTestMale invFemale = new frmLabTestMale(PatientID, PatientUnitID);
                invFemale.Title = "Male Investigation (Name:- " + CoupleDetails.MalePatient.FirstName +
                     " " + CoupleDetails.MalePatient.LastName + ")";
                invFemale.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        // Lab Test For Female
        private void cmdFemaleLabtest_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.FemalePatient != null && CoupleDetails.FemalePatient.PatientID != 0)
            {
                long PatientID = CoupleDetails.FemalePatient.PatientID;
                long PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                frmLabTestFemale invFemale = new frmLabTestFemale(PatientID, PatientUnitID);
                invFemale.Title = "Female Investigation (Name:- " + CoupleDetails.FemalePatient.FirstName +
                         " " + CoupleDetails.FemalePatient.LastName + ")";
                invFemale.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Female Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                msgW1.Show();
            }
        }

        // Form maleSemenExamination
        private void cmdmaleSemenExamination_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID != 0)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                ModuleName = "DataDrivenApplication";
                Action = "DataDrivenApplication.Forms.PatientEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_MaleFinding);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        // Form maleSemenThawing
        private void cmdmaleSemenThawing_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID > 0)
            {
                //long PatientID = CoupleDetails.MalePatient.PatientID;
                //long PatientUnitID = CoupleDetails.MalePatient.UnitId;
                //frmSpremThawing objwin = new frmSpremThawing(PatientID, PatientUnitID);
                //objwin.Show();
                SpermThawingForDashboard win = new SpermThawingForDashboard();
                win.CoupleDetails = CoupleDetails;
                win.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        // Form SemenFreezing
        private void cmdmaleSemenFreezing_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID > 0)
            {
                //long PatientID = CoupleDetails.MalePatient.PatientID;
                //long PatientUnitID = CoupleDetails.MalePatient.UnitId;

                //frmSpremFrezing objWin = new frmSpremFrezing(PatientID, PatientUnitID, CoupleDetails);
                //objWin.Show();
                SpermFreezingForDashboard win = new SpermFreezingForDashboard();
                win.CoupleDetails = CoupleDetails;
                win.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        // For maleConsent . . 
        private void cmdmaleConsent_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID != 0)
            {
                long PatientID = CoupleDetails.MalePatient.PatientID;
                long PatientUnitID = CoupleDetails.MalePatient.UnitId;
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
                frmMaleConsent obj = new frmMaleConsent(PatientID, PatientUnitID, CoupleDetails);
                obj.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        //for FemaleConsent
        private void cmdFemaleConsent_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.FemalePatient != null && CoupleDetails.FemalePatient.PatientID != 0)
            {
                long PatientID = CoupleDetails.FemalePatient.PatientID;
                long PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                frmFemaleConsent obj = new frmFemaleConsent(PatientID, PatientUnitID, CoupleDetails);
                obj.PatientID = PatientID;
                obj.PatientUnitID = PatientUnitID;
                obj.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Female Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        //FemaleExamination
        private void cmdFemaleExamination_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.FemalePatient != null && CoupleDetails.FemalePatient.PatientID != 0)
            {
                long PatientID = CoupleDetails.FemalePatient.PatientID;
                long PatientUnitID = CoupleDetails.FemalePatient.UnitId;

                frmFemaleExamination obj = new frmFemaleExamination(PatientID, PatientUnitID);
                obj.Title = "Female Examination :- (Name - " + CoupleDetails.FemalePatient.FirstName +
                  " " + CoupleDetails.FemalePatient.LastName + ")";
                obj.Closed += new EventHandler(OnExaminationClosed);
                obj.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Female Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        //maleExamination
        private void cmdmaleExamination_Click(object sender, RoutedEventArgs e)
        {
            if (CoupleDetails.MalePatient != null && CoupleDetails.MalePatient.PatientID != 0)
            {
                long PatientID = CoupleDetails.MalePatient.PatientID;
                long PatientUnitID = CoupleDetails.MalePatient.UnitId;

                frmMaleExamination_Dashboard obj = new frmMaleExamination_Dashboard(PatientID, PatientUnitID);
                obj.Title = "Male Examination :- (Name - " + CoupleDetails.MalePatient.FirstName +
                " " + CoupleDetails.MalePatient.LastName + ")";
                obj.Closed += new EventHandler(OnExaminationClosed);
                obj.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }

        }

        #endregion
        private void OnExaminationClosed(object sender, EventArgs e)
        {
            fillCoupleDetails();
        }
        private void btnSearchpatient_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtMrno_TextChanged(object sender, RoutedEventArgs e)
        {

        }

        #region set tab therapywise
        private void SetTabPlanTherapyWise(long pPlanTherapyID)
        {

            TabOverview.Visibility = Visibility.Collapsed;
            //TabIUI.Visibility = Visibility.Collapsed;

            TabOPU.Visibility = Visibility.Collapsed;
            //TabTableRepresentation.Visibility = Visibility.Collapsed;
            TabGraphicalRepresentation.Visibility = Visibility.Collapsed;
            //TabSummedRepresentation.Visibility = Visibility.Collapsed;
            TabCryoPreservation.Visibility = Visibility.Collapsed;
            TabOnlyVitrification.Visibility = Visibility.Collapsed;
            TabOnlyTransfer.Visibility = Visibility.Collapsed;
            Tabtransfer.Visibility = Visibility.Collapsed;
            TabLutualPhase.Visibility = Visibility.Collapsed;
            Taboutcome.Visibility = Visibility.Collapsed;
            Tabbirthdetails.Visibility = Visibility.Collapsed;

            switch (pPlanTherapyID)
            {
                case 1://IVF
                    TabOverview.Visibility = Visibility.Visible;

                    //    //TabIUI.Visibility = Visibility.Collapsed;
                    //    //TabOPU.Visibility = Visibility.Visible;
                    //    //TabTableRepresentation.Visibility = Visibility.Collapsed;
                    //    //TabGraphicalRepresentation.Visibility = Visibility.Visible;
                    //    //TabSummedRepresentation.Visibility = Visibility.Collapsed;
                    //    //TabCryoPreservation.Visibility = Visibility.Visible;
                    //    //TabOnlyVitrification.Visibility = Visibility.Collapsed;
                    //    //TabOnlyTransfer.Visibility = Visibility.Collapsed;
                    //    //Tabtransfer.Visibility = Visibility.Visible;
                    TabLutualPhase.Visibility = Visibility.Visible;
                    Taboutcome.Visibility = Visibility.Visible;
                    //Tabbirthdetails.Visibility = Visibility.Collapsed;
                    tabItems = new List<TabItem>();

                    tabItems.Add(TabOPU);
                    tabItems.Add(TabGraphicalRepresentation);
                    tabItems.Add(TabCryoPreservation);
                    tabItems.Add(Tabtransfer);

                    break;

                case 2://ICSI
                    TabOverview.Visibility = Visibility.Visible;

                    //TabIUI.Visibility = Visibility.Collapsed;
                    //TabOPU.Visibility = Visibility.Visible;
                    //TabTableRepresentation.Visibility = Visibility.Collapsed;
                    //TabGraphicalRepresentation.Visibility = Visibility.Visible;
                    //TabCryoPreservation.Visibility = Visibility.Visible;
                    //TabSummedRepresentation.Visibility = Visibility.Collapsed;
                    //TabOnlyVitrification.Visibility = Visibility.Collapsed;
                    //Tabtransfer.Visibility = Visibility.Visible;
                    //TabOnlyTransfer.Visibility = Visibility.Collapsed;
                    TabLutualPhase.Visibility = Visibility.Visible;
                    Taboutcome.Visibility = Visibility.Visible;
                    //Tabbirthdetails.Visibility = Visibility.Collapsed;
                    tabItems = new List<TabItem>();
                    tabItems.Add(TabOPU);
                    tabItems.Add(TabGraphicalRepresentation);
                    tabItems.Add(TabCryoPreservation);
                    tabItems.Add(Tabtransfer);
                    break;
                case 3://IVF - ICSI
                    TabOverview.Visibility = Visibility.Visible;

                    //TabIUI.Visibility = Visibility.Collapsed;
                    //TabOPU.Visibility = Visibility.Visible;
                    //TabTableRepresentation.Visibility = Visibility.Collapsed;
                    //TabGraphicalRepresentation.Visibility = Visibility.Visible;
                    //TabSummedRepresentation.Visibility = Visibility.Collapsed;
                    //TabCryoPreservation.Visibility = Visibility.Visible;
                    //TabOnlyVitrification.Visibility = Visibility.Collapsed;
                    //Tabtransfer.Visibility = Visibility.Visible;
                    //TabOnlyTransfer.Visibility = Visibility.Collapsed;
                    TabLutualPhase.Visibility = Visibility.Visible;
                    Taboutcome.Visibility = Visibility.Visible;
                    //Tabbirthdetails.Visibility = Visibility.Collapsed;
                    tabItems = new List<TabItem>();
                    tabItems.Add(TabOPU);
                    tabItems.Add(TabGraphicalRepresentation);
                    tabItems.Add(TabCryoPreservation);
                    tabItems.Add(Tabtransfer);
                    break;

                case 5://Thaw/Transfer
                    TabOverview.Visibility = Visibility.Visible;
                    //TabIUI.Visibility = Visibility.Collapsed;
                    //TabOPU.Visibility = Visibility.Collapsed;
                    //TabTableRepresentation.Visibility = Visibility.Collapsed;
                    //TabGraphicalRepresentation.Visibility = Visibility.Collapsed;
                    //TabCryoPreservation.Visibility = Visibility.Visible;
                    //TabSummedRepresentation.Visibility = Visibility.Collapsed;
                    //Tabtransfer.Visibility = Visibility.Visible;
                    //TabOnlyVitrification.Visibility = Visibility.Collapsed;
                    //TabOnlyTransfer.Visibility = Visibility.Collapsed;
                    TabLutualPhase.Visibility = Visibility.Visible;
                    Taboutcome.Visibility = Visibility.Visible;
                    //Tabbirthdetails.Visibility = Visibility.Collapsed;
                    tabItems = new List<TabItem>();
                    tabItems.Add(TabCryoPreservation);
                    tabItems.Add(Tabtransfer);

                    break;

                case 11://OocyteDonation
                    TabOverview.Visibility = Visibility.Visible;
                    //TabIUI.Visibility = Visibility.Collapsed;
                    //TabOPU.Visibility = Visibility.Visible;
                    //TabTableRepresentation.Visibility = Visibility.Collapsed;
                    //TabGraphicalRepresentation.Visibility = Visibility.Collapsed;
                    //TabCryoPreservation.Visibility = Visibility.Collapsed;
                    //TabSummedRepresentation.Visibility = Visibility.Collapsed;
                    //Tabtransfer.Visibility = Visibility.Collapsed;
                    //TabOnlyVitrification.Visibility = Visibility.Visible;
                    //TabOnlyTransfer.Visibility = Visibility.Visible;
                    TabLutualPhase.Visibility = Visibility.Visible;
                    Taboutcome.Visibility = Visibility.Visible;
                    //Tabbirthdetails.Visibility = Visibility.Collapsed;
                    tabItems = new List<TabItem>();
                    tabItems.Add(TabOPU);
                    //  tabItems.Add(TabOnlyVitrification);
                    if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsEmbryoDonation == true)
                    {
                        tabItems.Add(TabGraphicalRepresentation);
                        tabItems.Add(TabCryoPreservation);
                    }
                    break;
                case 12://Oocyte Receipant
                    TabOverview.Visibility = Visibility.Visible;

                    //TabIUI.Visibility = Visibility.Collapsed;
                    //TabOPU.Visibility = Visibility.Collapsed;
                    //TabTableRepresentation.Visibility = Visibility.Collapsed;
                    //TabGraphicalRepresentation.Visibility = Visibility.Collapsed;
                    //TabCryoPreservation.Visibility = Visibility.Collapsed;
                    //TabSummedRepresentation.Visibility = Visibility.Collapsed;
                    //Tabtransfer.Visibility = Visibility.Collapsed;
                    //TabOnlyVitrification.Visibility = Visibility.Collapsed;
                    //TabOnlyTransfer.Visibility = Visibility.Visible;
                    TabLutualPhase.Visibility = Visibility.Visible;
                    Taboutcome.Visibility = Visibility.Visible;
                    //Tabbirthdetails.Visibility = Visibility.Collapsed;
                    tabItems = new List<TabItem>();
                    //tabItems.Add(TabOnlyTransfer);
                    tabItems.Add(TabGraphicalRepresentation);
                    tabItems.Add(TabCryoPreservation);
                    tabItems.Add(Tabtransfer);
                    break;

                //case 15: // IUI(H)
                //       TabOverview.Visibility = Visibility.Visible;
                //       //TabIUI.Visibility = Visibility.Visible;
                //       //TabOPU.Visibility = Visibility.Collapsed;
                //       //TabTableRepresentation.Visibility = Visibility.Collapsed;
                //       //TabGraphicalRepresentation.Visibility = Visibility.Collapsed;
                //       //TabCryoPreservation.Visibility = Visibility.Collapsed;
                //       //TabSummedRepresentation.Visibility = Visibility.Collapsed;
                //       //Tabtransfer.Visibility = Visibility.Collapsed;
                //       //TabOnlyVitrification.Visibility = Visibility.Collapsed;
                //       //TabOnlyTransfer.Visibility = Visibility.Collapsed;
                //       TabLutualPhase.Visibility = Visibility.Visible;
                //       Taboutcome.Visibility = Visibility.Visible;
                //       //Tabbirthdetails.Visibility = Visibility.Collapsed;
                //       tabItems = new List<TabItem>();
                //       tabItems.Add(TabIUI);

                //       break;

                //case 16: // IUI(D)
                //       TabOverview.Visibility = Visibility.Visible;
                //       //TabIUI.Visibility = Visibility.Visible;
                //       //TabOPU.Visibility = Visibility.Collapsed;
                //       //TabTableRepresentation.Visibility = Visibility.Collapsed;
                //       //TabGraphicalRepresentation.Visibility = Visibility.Collapsed;
                //       //TabCryoPreservation.Visibility = Visibility.Collapsed;
                //       //TabSummedRepresentation.Visibility = Visibility.Collapsed;
                //       //TabOnlyVitrification.Visibility = Visibility.Collapsed;
                //       //Tabtransfer.Visibility = Visibility.Collapsed;
                //       //TabOnlyTransfer.Visibility = Visibility.Collapsed;
                //       TabLutualPhase.Visibility = Visibility.Visible;
                //       Taboutcome.Visibility = Visibility.Visible;
                //       //Tabbirthdetails.Visibility = Visibility.Collapsed;
                //       tabItems = new List<TabItem>();
                //       tabItems.Add(TabIUI);
                //       break;
                //// BY bHUSHSN
                case 14: // Embryo Recepient
                    TabOverview.Visibility = Visibility.Visible;
                    // TabIUI.Visibility = Visibility.Visible;
                    //TabOPU.Visibility = Visibility.Collapsed;
                    //TabTableRepresentation.Visibility = Visibility.Collapsed;
                    //TabGraphicalRepresentation.Visibility = Visibility.Collapsed;
                    //TabCryoPreservation.Visibility = Visibility.Collapsed;
                    //TabSummedRepresentation.Visibility = Visibility.Collapsed;
                    //TabOnlyVitrification.Visibility = Visibility.Collapsed;
                    //Tabtransfer.Visibility = Visibility.Collapsed;
                    //TabOnlyTransfer.Visibility = Visibility.Collapsed;
                    TabLutualPhase.Visibility = Visibility.Visible;
                    Taboutcome.Visibility = Visibility.Visible;
                    Tabbirthdetails.Visibility = Visibility.Collapsed;
                    tabItems = new List<TabItem>();
                    //tabItems.Add(TabOnlyTransfer);
                    tabItems.Add(TabGraphicalRepresentation);
                    tabItems.Add(TabCryoPreservation);
                    tabItems.Add(Tabtransfer);
                    // tabItems.Add(Tabbirthdetails);
                    break;

                default:
                    TabOverview.Visibility = Visibility.Visible;
                    //TabIUI.Visibility = Visibility.Collapsed;
                    //TabOPU.Visibility = Visibility.Collapsed;
                    //TabTableRepresentation.Visibility = Visibility.Collapsed;
                    //TabGraphicalRepresentation.Visibility = Visibility.Collapsed;
                    //TabCryoPreservation.Visibility = Visibility.Collapsed;
                    //TabSummedRepresentation.Visibility = Visibility.Collapsed;
                    //Tabtransfer.Visibility = Visibility.Collapsed;
                    //TabOnlyVitrification.Visibility = Visibility.Collapsed;
                    //TabOnlyTransfer.Visibility = Visibility.Collapsed;
                    TabLutualPhase.Visibility = Visibility.Visible;
                    Taboutcome.Visibility = Visibility.Visible;
                    // Tabbirthdetails.Visibility = Visibility.Collapsed;
                    break;


            }
            RequestXML("IVFDashboardSubmenu");
        }
        List<clsMenuVO> MenuList = new List<clsMenuVO>();
        private List<TabItem> tabItems = new List<TabItem>();

        public void RequestXML(string Parent)
        {
            if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.RoleDetails.MenuList.Count > 0)
            {
                var i1 = tabControl1.Items;

                MenuList = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.RoleDetails.MenuList;
                var Menus = from r in MenuList
                            where r.Parent == Parent && r.Status == true
                            orderby r.MenuOrder
                            select r;

                foreach (var s in tabItems)
                {
                    if (s.Name != "TabOverview" || s.Name != "TabDocument" || s.Name != "TabLutualPhase" || s.Name != "Taboutcome")
                    {
                        var h = from u in Menus
                                where u.Title == s.Name
                                select u;
                        if (h.ToList().Count > 0)
                        {
                            s.Visibility = Visibility.Visible;
                        }

                    }
                }
            }
        }
        #endregion

        #region View CycleDetails
        private void cmdCycleDetails_Click(object sender, RoutedEventArgs e)
        {
            if (dgtheropyList.SelectedItem != null)
            {

                try
                {
                    SelectedTherapyDetails = new clsIVFDashboard_GetTherapyListBizActionVO();
                    objAnimation.Invoke(RotationType.Forward);
                    if (TabOverview != null)
                        TabOverview.IsSelected = true;
                    grdBackPanel.DataContext = ((clsPlanTherapyVO)dgtheropyList.SelectedItem);
                    SelectedTherapyDetails.TherapyDetails = ((clsPlanTherapyVO)dgtheropyList.SelectedItem);
                    startDate = SelectedTherapyDetails.TherapyDetails.TherapyStartDate;
                    cmbPhysician.SelectedValue = (long)((clsPlanTherapyVO)dgtheropyList.SelectedItem).PhysicianId;
                    cmbMainIndication.SelectedValue = (long)((clsPlanTherapyVO)dgtheropyList.SelectedItem).MainInductionID;
                    cmbPlannedTreatment.SelectedValue = (long)((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID;
                    txtPlannedNoOfEmbryos.Text = Convert.ToString(((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedEmbryos);
                    cmbProtocol.SelectedValue = (long)((clsPlanTherapyVO)dgtheropyList.SelectedItem).ProtocolTypeID;
                    cmbSimulation.SelectedValue = (long)((clsPlanTherapyVO)dgtheropyList.SelectedItem).ExternalSimulationID;
                    cmbSpermCollection.SelectedValue = (long)((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedSpermCollectionID;
                    dtOPUDate.SelectedDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).OPUtDate;
                    SurrogateUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).SurrogateUnitID;
                    SurrogateID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).SurrogateID;
                    if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).AttachedSurrogate != null)
                        chkSurrogate.IsChecked = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).AttachedSurrogate;
                    txtSurrogateCode.Text = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).SurrogateMRNo;

                    setupPage(((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID, 0);
                    IsCancel = false;
                    if (chkPill.IsChecked == true)
                    {
                        dtStartDate.IsEnabled = true;
                        dtEndDate.IsEnabled = true;
                    }
                    else
                    {
                        dtStartDate.IsEnabled = false;
                        dtEndDate.IsEnabled = false;
                    }
                    if (SelectedTherapyDetails.TherapyDetails.IsClosed == true)
                    {
                        IsClosed = true;
                        cmdSaveTherapy.IsEnabled = false;
                        cmdNewExeDrug.IsEnabled = false;
                        cmdNewExeDrugSurrogate.IsEnabled = false;
                    }
                    else
                    {
                        IsClosed = false;
                        cmdSaveTherapy.IsEnabled = true;
                        cmdNewExeDrug.IsEnabled = true;
                        cmdNewExeDrugSurrogate.IsEnabled = true;
                    }
                    if (((clsPlanTherapyVO)dgtheropyList.SelectedItem).IsClosed)
                    {
                        cmdNewExeDrug.IsEnabled = false;
                    }
                    else { cmdNewExeDrug.IsEnabled = true; }
                    LoadPatientHeader();
                    SetTabPlanTherapyWise((long)((clsPlanTherapyVO)dgtheropyList.SelectedItem).PlannedTreatmentID);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region outcome
        private void fillOutcomeDetails()
        {
            clsIVFDashboard_GetOutcomeBizActionVO BizAction = new clsIVFDashboard_GetOutcomeBizActionVO();
            BizAction.Details = new clsIVFDashboard_OutcomeVO();
            BizAction.Details.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
            BizAction.Details.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
            if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails != null)
            {
                BizAction.Details.PatientID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.PatientID;
                BizAction.Details.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.UnitId;
            }
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details != null)
                    {
                        this.DataContext = ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details;
                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.BHCGAss1Date != null && ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.BHCGAss1Date != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                        {
                            BHCGAss1Date.SelectedDate = ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.BHCGAss1Date;
                        }
                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.BHCGAss2Date != null && ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.BHCGAss2Date != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                        {
                            BHCGAss2Date.SelectedDate = ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.BHCGAss2Date;
                        }
                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.PregnanacyConfirmDate != null && ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.PregnanacyConfirmDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                        {
                            dtpPregnancy.SelectedDate = ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.PregnanacyConfirmDate;
                        }
                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.FetalDate != null && ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.FetalDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                        {
                            dtFetalDate.SelectedDate = ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.FetalDate;
                        }
                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.BabyTypeID != null)
                        {
                            cmbBabyType.SelectedValue = ((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.BabyTypeID;
                        }
                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.BHCGAss1IsBSCG == true)
                        {
                            rdoBSCGAss1Positve.IsChecked = true;
                        }
                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.BHCGAss1IsBSCG == false)
                        {
                            rdoBSCGAss1Negative.IsChecked = true;
                        }
                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.BHCGAss2IsBSCG == true)
                        {
                            rdoBSCGAss2Positve.IsChecked = true;
                        }
                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.BHCGAss2IsBSCG == false)
                        {
                            rdoBSCGAss2Negative.IsChecked = true;
                        }
                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.IsPregnancyAchieved == true)
                        {
                            rdoPregnancyAchievedYes.IsChecked = true;
                        }
                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.IsPregnancyAchieved == false)
                        {
                            rdoPregnancyAchievedNo.IsChecked = true;
                        }
                        if (((clsIVFDashboard_GetOutcomeBizActionVO)arg.Result).Details.IsClosed == true)
                        {
                            chkIsclosed.IsChecked = true;
                            cmdSaveOut.IsEnabled = false;
                        }
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            bool saveDtls = true;
            saveDtls = validateOutcome();
            if (saveDtls == true)
            {
                string msgTitle = "Palash";
                string msgText = "Are You Sure \n You Want To Save OutCome Details";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += (result) =>
                {
                    if (result == MessageBoxResult.Yes)
                    {
                        SaveOutcome();
                    }
                };
                msgWin.Show();
            }
        }
        private void SaveOutcome()
        {
            clsIVFDashboard_AddUpdateOutcomeBizActionVO BizAction = new clsIVFDashboard_AddUpdateOutcomeBizActionVO();
            BizAction.OutcomeDetails = new clsIVFDashboard_OutcomeVO();
            BizAction.OutcomeDetails.ID = ((clsIVFDashboard_OutcomeVO)this.DataContext).ID;
            BizAction.OutcomeDetails.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.OutcomeDetails.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.OutcomeDetails.PlanTherapyID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
            BizAction.OutcomeDetails.PlanTherapyUnitID = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
            if (rdoBSCGAss1Negative.IsChecked != null)
            {
                if (rdoBSCGAss1Negative.IsChecked == true)
                {
                    BizAction.OutcomeDetails.BHCGAss1IsBSCG = false;
                }
            }
            if (rdoBSCGAss1Positve.IsChecked != null)
            {
                if (rdoBSCGAss1Positve.IsChecked == true)
                {
                    BizAction.OutcomeDetails.BHCGAss1IsBSCG = true;
                }
            }
            if (rdoBSCGAss2Negative.IsChecked != null)
            {
                if (rdoBSCGAss2Negative.IsChecked == true)
                {
                    BizAction.OutcomeDetails.BHCGAss2IsBSCG = false;
                }
            }
            if (rdoBSCGAss2Positve.IsChecked != null)
            {
                if (rdoBSCGAss2Positve.IsChecked == true)
                {
                    BizAction.OutcomeDetails.BHCGAss2IsBSCG = true;
                }
            }
            if (rdoPregnancyAchievedNo.IsChecked != null)
            {
                if (rdoPregnancyAchievedNo.IsChecked == true)
                {
                    BizAction.OutcomeDetails.IsPregnancyAchieved = false;
                }
            }
            if (rdoPregnancyAchievedYes.IsChecked != null)
            {
                if (rdoPregnancyAchievedYes.IsChecked == true)
                {
                    BizAction.OutcomeDetails.IsPregnancyAchieved = true;
                }
            }

            if (cmbBabyType.SelectedItem != null)
            {
                BizAction.OutcomeDetails.BabyTypeID = ((MasterListItem)cmbBabyType.SelectedItem).ID;
            }

            if (txtOHSSremark.Text != null && txtOHSSremark.Text != string.Empty)
            {
                BizAction.OutcomeDetails.OHSSRemark = txtOHSSremark.Text.Trim();
            }
            if (txtOutComeRemarks.Text != null && txtOutComeRemarks.Text != string.Empty)
            {
                BizAction.OutcomeDetails.OutComeRemarks = txtOutComeRemarks.Text.Trim();
            }
            if (txtUSG.Text != null && txtUSG.Text != string.Empty)
            {
                BizAction.OutcomeDetails.BHCGAss2USG = txtUSG.Text.Trim();
            }

            if (txtCount.Text != null && txtCount.Text != string.Empty)
            {
                BizAction.OutcomeDetails.Count = txtCount.Text.Trim();
            }

            if (txtOutβhCGValue.Text != null && txtOutβhCGValue.Text != string.Empty)
            {
                BizAction.OutcomeDetails.BHCGAss1BSCGValue = txtOutβhCGValue.Text.Trim();
            }
            if (txtSrProgest.Text != null && txtSrProgest.Text != string.Empty)
            {
                BizAction.OutcomeDetails.BHCGAss1SrProgest = txtSrProgest.Text.Trim();
            }
            if (txtOutAssment2βhCGValue.Text != null && txtOutAssment2βhCGValue.Text != string.Empty)
            {
                BizAction.OutcomeDetails.BHCGAss2BSCGValue = txtOutAssment2βhCGValue.Text.Trim();
            }
            if (txtOutAssment2βhCGValue.Text != null && txtOutAssment2βhCGValue.Text != string.Empty)
            {
                BizAction.OutcomeDetails.BHCGAss2BSCGValue = txtOutAssment2βhCGValue.Text.Trim();
            }
            if (dtFetalDate.SelectedDate != null)
            {
                BizAction.OutcomeDetails.FetalDate = dtFetalDate.SelectedDate;
            }
            if (BHCGAss1Date.SelectedDate != null)
            {
                BizAction.OutcomeDetails.BHCGAss1Date = BHCGAss1Date.SelectedDate;
            }
            if (BHCGAss2Date.SelectedDate != null)
            {
                BizAction.OutcomeDetails.BHCGAss2Date = BHCGAss2Date.SelectedDate;
            }
            if (dtpPregnancy.SelectedDate != null)
            {
                BizAction.OutcomeDetails.PregnanacyConfirmDate = dtpPregnancy.SelectedDate;
            }
            if (chkOHSSEarly.IsChecked != null)
            {
                if (chkOHSSEarly.IsChecked == true)
                {
                    BizAction.OutcomeDetails.OHSSEarly = true;
                }
            }
            if (chkOHSSLate.IsChecked != null)
            {
                if (chkOHSSLate.IsChecked == true)
                {
                    BizAction.OutcomeDetails.OHSSLate = true;
                }
            }
            if (chkChemicalPregancy.IsChecked != null)
            {
                if (chkChemicalPregancy.IsChecked == true)
                {
                    BizAction.OutcomeDetails.IsChemicalPregnancy = true;
                }
            }
            if (chkPreTermPregnancy.IsChecked != null)
            {
                if (chkPreTermPregnancy.IsChecked == true)
                {
                    BizAction.OutcomeDetails.PretermDelivery = true;
                }
            }
            if (chkFullTermPregnancy.IsChecked != null)
            {
                if (chkFullTermPregnancy.IsChecked == true)
                {
                    BizAction.OutcomeDetails.IsFullTermDelivery = true;
                }
            }
            if (chkOHSSLate.IsChecked != null)
            {
                if (chkOHSSLate.IsChecked == true)
                {
                    BizAction.OutcomeDetails.OHSSLate = true;
                }
            }
            if (chkOHSSmild.IsChecked != null)
            {
                if (chkOHSSmild.IsChecked == true)
                {
                    BizAction.OutcomeDetails.OHSSMild = true;
                }
            }
            if (chkOHSSmod.IsChecked != null)
            {
                if (chkOHSSmod.IsChecked == true)
                {
                    BizAction.OutcomeDetails.OHSSMode = true;
                }
            }
            if (chkOHSSsevere.IsChecked != null)
            {
                if (chkOHSSsevere.IsChecked == true)
                {
                    BizAction.OutcomeDetails.OHSSSereve = true;
                }
            }
            if (rdoBSCGAss1Negative.IsChecked != null)
            {
                if (rdoBSCGAss1Negative.IsChecked == true)
                {
                    BizAction.OutcomeDetails.BHCGAss1IsBSCG = false;
                }
            }
            if (rdoBSCGAss1Positve.IsChecked != null)
            {
                if (rdoBSCGAss1Positve.IsChecked == true)
                {
                    BizAction.OutcomeDetails.BHCGAss1IsBSCG = true;
                }
            }
            if (rdoBSCGAss2Negative.IsChecked != null)
            {
                if (rdoBSCGAss2Negative.IsChecked == true)
                {
                    BizAction.OutcomeDetails.BHCGAss2IsBSCG = false;
                }
            }
            if (rdoBSCGAss2Positve.IsChecked != null)
            {
                if (rdoBSCGAss2Positve.IsChecked == true)
                {
                    BizAction.OutcomeDetails.BHCGAss2IsBSCG = true;
                }
            }
            if (rdoPregnancyAchievedNo.IsChecked != null)
            {
                if (rdoPregnancyAchievedNo.IsChecked == true)
                {
                    BizAction.OutcomeDetails.IsPregnancyAchieved = false;
                }
            }
            if (rdoPregnancyAchievedYes.IsChecked != null)
            {
                if (rdoPregnancyAchievedYes.IsChecked == true)
                {
                    BizAction.OutcomeDetails.IsPregnancyAchieved = true;
                }
            }
            if (chkOHSSsevere.IsChecked != null)
            {
                if (chkOHSSsevere.IsChecked == true)
                {
                    BizAction.OutcomeDetails.OHSSSereve = true;
                }
            }
            if (BiochemPregnancy.IsChecked != null)
            {
                if (BiochemPregnancy.IsChecked == true)
                {
                    BizAction.OutcomeDetails.BiochemPregnancy = true;
                }
            }
            if (Abortion.IsChecked != null)
            {
                if (Abortion.IsChecked == true)
                {
                    BizAction.OutcomeDetails.Abortion = true;
                }
            }
            if (FetalHeartSound.IsChecked != null)
            {
                if (FetalHeartSound.IsChecked == true)
                {
                    BizAction.OutcomeDetails.FetalHeartSound = true;
                }
            }
            if (IUD.IsChecked != null)
            {
                if (IUD.IsChecked == true)
                {
                    BizAction.OutcomeDetails.IUD = true;
                }
            }
            if (LiveBirth.IsChecked != null)
            {
                if (LiveBirth.IsChecked == true)
                {
                    BizAction.OutcomeDetails.LiveBirth = true;
                }
            }
            if (Congenitalabnormality.IsChecked != null)
            {
                if (Congenitalabnormality.IsChecked == true)
                {
                    BizAction.OutcomeDetails.Congenitalabnormality = true;
                }
            }
            if (Ectopic.IsChecked != null)
            {
                if (Ectopic.IsChecked == true)
                {
                    BizAction.OutcomeDetails.Ectopic = true;
                }
            }
            if (chkMissed.IsChecked != null)
            {
                if (chkMissed.IsChecked == true)
                {
                    BizAction.OutcomeDetails.Missed = true;
                }
            }
            if (chkIncomplete.IsChecked != null)
            {
                if (chkIncomplete.IsChecked == true)
                {
                    BizAction.OutcomeDetails.Incomplete = true;
                }
            }
            if (chkIsclosed.IsChecked != null)
            {
                if (chkIsclosed.IsChecked == true)
                {
                    BizAction.OutcomeDetails.IsClosed = true;
                }
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Outcome Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    //  setupPage(((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID, 0);
                    fillOutcomeDetails();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private bool validateOutcome()
        {
            bool result = true;
            if (rdoPregnancyAchievedYes.IsChecked == true)
            {
                if (string.IsNullOrEmpty(dtpPregnancy.Text))
                {
                    dtpPregnancy.SetValidation("Pregnancy Confirm Date Is Required");
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
        #endregion

        #region checkbox click

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
        #endregion

        #region Media Click
        private void Cmdmedia_Click(object sender, RoutedEventArgs e)
        {
            MediaDetails_New Win = new MediaDetails_New(((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID, ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID, CoupleDetails, "OnlyVitrification");
            Win.Show();
        }
        #endregion

        private void cmbCanID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbSimulation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbProtocol_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbSpermCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        long TherapyId, TherapyUnitId;
        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgtheropyList.SelectedItem != null)
            {
                TherapyId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).ID;
                TherapyUnitId = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).UnitID;
            }
            if (CoupleDetails.FemalePatient != null && CoupleDetails.FemalePatient.PatientID > 0)
            {
                long PatientID = CoupleDetails.FemalePatient.PatientID;
                long PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/IVFDashboard/IVFDashboard_PatientARTReport.aspx?TherapyId=" + TherapyId + "&TherapyUnitId=" + TherapyUnitId + "&PatientID=" + PatientID + "&PatientUnitID=" + PatientUnitID), "_blank");
            }
        }

        private void ExeGrid_RowEditEnded(object sender, DataGridRowEditEndedEventArgs e)
        {

        }

        private void CalendarForSurrogatePatient_Maximized(object sender, EventArgs e)
        {

        }

        private void CalendarForSurrogatePatient_Minimized(object sender, EventArgs e)
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

        private void TherpayExe1RowDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExeGrid1_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }
        bool isDrugForSurrogate;
        private void cmdNewExeDrugSurrogate_Click(object sender, RoutedEventArgs e)
        {
            isDrugForSurrogate = true;
            AddDrug addDrug = new AddDrug();
            addDrug.IsEdit = false;
            addDrug.TherpayExeId = 0;
            addDrug.TherapyStartDate = ((clsPlanTherapyVO)dgtheropyList.SelectedItem).TherapyStartDate;
            addDrug.DrugId = 0;
            addDrug.OnSaveButton_Click += new RoutedEventHandler(addDrug_OnSaveButton_Click);
            addDrug.Show();
        }

        private void cmdDrugfemale_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdDrugSurrogate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CalendarForFemalePatient_Maximized(object sender, EventArgs e)
        {

        }

        private void CalendarForFemalePatient_Minimized(object sender, EventArgs e)
        {

        }

        private void cmdOutComeCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void chkSurrogate_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnSearchSurrogate_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch Win = new PatientSearch();
            Win.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            Win.PatientCategoryID = 10;
            Win.IsFromSurrogacyModule = true;
            Win.Show();
        }
        long SurrogateID = 0;
        long SurrogateUnitID = 0;
        void Win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch ObjWin = (PatientSearch)sender;
            if (ObjWin.DialogResult == true)
            {
                if (ObjWin.PatientCategoryID == 10)
                    txtSurrogateCode.Text = ((clsPatientGeneralVO)ObjWin.dataGrid2.SelectedItem).MRNo;
                SurrogateID = ((clsPatientGeneralVO)ObjWin.dataGrid2.SelectedItem).PatientID;
                SurrogateUnitID = ((clsPatientGeneralVO)ObjWin.dataGrid2.SelectedItem).UnitId;
            }
        }

        private void ExeGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((clsTherapyExecutionVO)ExeGrid.SelectedItem) != null)
            {
                if (((clsTherapyExecutionVO)ExeGrid.SelectedItem).TherapyTypeId == 1)
                {
                    FrmExecutionCalender win = new FrmExecutionCalender();
                    win.Title = "Date of LP ( " + CoupleDetails.FemalePatient.FirstName +
                         " " + CoupleDetails.FemalePatient.LastName + " )";
                    win.ExeCal.SelectionMode = CalendarSelectionMode.SingleDate;
                    win.ExeCal.DisplayDateStart = startDate;
                    win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(60);
                    if (win.selecteddates != null)
                        win.selecteddates.Clear();
                    win.selecteddates = SetValueToCal(((clsTherapyExecutionVO)ExeGrid.SelectedItem));
                    win.OnSaveButton_Click += new RoutedEventHandler(ExecutionCalender_OnSaveButton_Click);
                    win.Show();
                }
                else if (((clsTherapyExecutionVO)ExeGrid.SelectedItem).TherapyTypeId == 3)
                {
                    FrmExecutionCalender win = new FrmExecutionCalender();
                    win.Title = "Follicular US ( " + CoupleDetails.FemalePatient.FirstName +
                         " " + CoupleDetails.FemalePatient.LastName + " )";
                    win.ExeCal.SelectionMode = CalendarSelectionMode.MultipleRange;
                    win.ExeCal.DisplayDateStart = startDate;
                    win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(60);
                    win.RemarkLabel.Visibility = Visibility.Visible;
                    //foreach(var r in SetValueToCal())
                    //{
                    //    win.ExeCal.SelectedDate=r.Date;
                    //}
                    //win.ExeCal.SelectedDates=(SetValueToCal())


                    //SelectedDatesCollection selecteddates = win.ExeCal.SelectedDates;
                    if (win.selecteddates != null)
                        win.selecteddates.Clear();
                    win.selecteddates = SetValueToCal(((clsTherapyExecutionVO)ExeGrid.SelectedItem));

                    win.OnSaveButton_Click += new RoutedEventHandler(ExecutionCalender_OnSaveButton_Click);
                    win.Show();
                }
                else if (((clsTherapyExecutionVO)ExeGrid.SelectedItem).TherapyTypeId == 4)
                {
                    FrmExecutionCalender win = new FrmExecutionCalender();
                    win.Title = "OPU ( " + CoupleDetails.FemalePatient.FirstName +
                         " " + CoupleDetails.FemalePatient.LastName + " )";
                    win.ExeCal.SelectionMode = CalendarSelectionMode.SingleDate;
                    win.ExeCal.DisplayDateStart = startDate;
                    win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(60);
                    if (win.selecteddates != null)
                        win.selecteddates.Clear();
                    win.selecteddates = SetValueToCal(((clsTherapyExecutionVO)ExeGrid.SelectedItem));
                    win.OnSaveButton_Click += new RoutedEventHandler(ExecutionCalender_OnSaveButton_Click);
                    win.Show();
                }
                else if (((clsTherapyExecutionVO)ExeGrid.SelectedItem).TherapyTypeId == 5)
                {
                    FrmExecutionCalender win = new FrmExecutionCalender();
                    win.Title = "ET ( " + CoupleDetails.FemalePatient.FirstName +
                         " " + CoupleDetails.FemalePatient.LastName + " )";
                    win.ExeCal.SelectionMode = CalendarSelectionMode.SingleDate;
                    win.ExeCal.DisplayDateStart = startDate;
                    win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(60);
                    if (win.selecteddates != null)
                        win.selecteddates.Clear();
                    win.selecteddates = SetValueToCal(((clsTherapyExecutionVO)ExeGrid.SelectedItem));
                    win.OnSaveButton_Click += new RoutedEventHandler(ExecutionCalender_OnSaveButton_Click);
                    win.Show();
                }
            }
        }
        StringBuilder Day = new StringBuilder();
        private SelectedDatesCollection SetValueToCal(clsTherapyExecutionVO ExeVO)
        {
            Calendar WinCal = new Calendar();
            WinCal.SelectionMode = CalendarSelectionMode.MultipleRange;
            SelectedDatesCollection SelectedDates = WinCal.SelectedDates;

            if (ExeVO.Day1 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(0));
            }
            if (ExeVO.Day2 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(1));
            }
            if (ExeVO.Day3 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(2));
            }
            if (ExeVO.Day4 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(3));
            }
            if (ExeVO.Day5 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(4));
            }
            if (ExeVO.Day6 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(5));
            }
            if (ExeVO.Day7 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(6));
            }
            if (ExeVO.Day8 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(7));
            }
            if (ExeVO.Day9 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(8));
            }
            if (ExeVO.Day10 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(8));
            }
            if (ExeVO.Day11 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(10));
            }
            if (ExeVO.Day12 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(11));
            }
            if (ExeVO.Day13 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(12));
            }
            if (ExeVO.Day14 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(13));
            }
            if (ExeVO.Day15 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(14));
            }
            if (ExeVO.Day16 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(15));
            }
            if (ExeVO.Day17 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(16));
            }
            if (ExeVO.Day18 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(17));
            }
            if (ExeVO.Day19 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(18));
            }
            if (ExeVO.Day20 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(19));
            }
            if (ExeVO.Day21 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(20));
            }
            if (ExeVO.Day22 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(21));
            }
            if (ExeVO.Day23 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(22));
            }
            if (ExeVO.Day24 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(23));
            }
            if (ExeVO.Day25 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(24));
            }
            if (ExeVO.Day26 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(25));
            }
            if (ExeVO.Day27 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(26));
            }
            if (ExeVO.Day28 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(27));
            }
            if (ExeVO.Day29 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(28));
            }
            if (ExeVO.Day30 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(29));
            }
            if (ExeVO.Day31 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(30));
            }
            if (ExeVO.Day32 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(31));
            }
            if (ExeVO.Day33 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(32));
            }
            if (ExeVO.Day34 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(33));
            }
            if (ExeVO.Day35 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(34));
            }
            if (ExeVO.Day36 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(35));
            }
            if (ExeVO.Day37 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(36));
            }
            if (ExeVO.Day38 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(37));
            }
            if (ExeVO.Day39 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(38));
            }
            if (ExeVO.Day40 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(39));
            }
            if (ExeVO.Day41 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(40));
            }
            if (ExeVO.Day42 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(41));
            }
            if (ExeVO.Day43 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(42));
            }
            if (ExeVO.Day44 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(43));
            }
            if (ExeVO.Day45 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(44));
            }
            if (ExeVO.Day46 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(45));
            }
            if (ExeVO.Day47 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(46));
            }
            if (ExeVO.Day48 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(47));
            }
            if (ExeVO.Day49 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(48));
            }
            if (ExeVO.Day50 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(49));
            }
            if (ExeVO.Day51 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(50));
            }
            if (ExeVO.Day52 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(51));
            }
            if (ExeVO.Day53 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(52));
            }
            if (ExeVO.Day54 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(53));
            }
            if (ExeVO.Day55 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(54));
            }
            if (ExeVO.Day56 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(55));
            }
            if (ExeVO.Day57 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(56));
            }
            if (ExeVO.Day58 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(57));
            }
            if (ExeVO.Day59 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(58));
            }
            if (ExeVO.Day60 == "True")
            {
                SelectedDates.Add(startDate.Value.AddDays(59));
            }
            return SelectedDates;
        }
        void ExecutionCalender_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                SelectedDatesCollection DateValues = ((FrmExecutionCalender)sender).ExeCal.SelectedDates;
                clsUpdateTherapyExecutionBizActionVO BizActionVO = new clsUpdateTherapyExecutionBizActionVO();
                BizActionVO.TherapyExecutionDetial = new clsTherapyExecutionVO();
                BizActionVO.TherapyExecutionDetial = SetValueOfVO(((clsTherapyExecutionVO)ExeGrid.SelectedItem));
                double dayNo = 0;
                Day = new StringBuilder();

                foreach (var r in DateValues)
                {

                    dayNo = (r.Date - startDate.Value.Date).TotalDays + 1;
                    if (Day.Length == 0)
                    {
                        Day.Append(dayNo);
                    }
                    else
                    {
                        Day.Append(",");
                        Day.Append(dayNo);
                    }
                    RetriveValue(BizActionVO.TherapyExecutionDetial, (int)dayNo);
                }

                if (Day != null)
                    BizActionVO.TherapyExecutionDetial.Day = Day.ToString();
                BizActionVO.TherapyExecutionDetial.IsSurrogate = false;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsUpdateTherapyExecutionBizActionVO)args.Result).SuccessStatus == 3)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "follicular monitoring details already added for selection so cannot save the selection.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                            setupPage(((clsTherapyExecutionVO)ExeGrid.SelectedItem).PlanTherapyId, 0);
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Details Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                            setupPage(((clsTherapyExecutionVO)ExeGrid.SelectedItem).PlanTherapyId, 0);
                        }
                    }
                };
                client.ProcessAsync(BizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        void ExecutionCalenderSurrogate_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                SelectedDatesCollection DateValues = ((FrmExecutionCalender)sender).ExeCal.SelectedDates;
                clsUpdateTherapyExecutionBizActionVO BizActionVO = new clsUpdateTherapyExecutionBizActionVO();
                BizActionVO.TherapyExecutionDetial = new clsTherapyExecutionVO();

                BizActionVO.TherapyExecutionDetial = SetValueOfVO(((clsTherapyExecutionVO)ExeGrid1.SelectedItem));
                double dayNo = 0;
                Day = new StringBuilder();

                foreach (var r in DateValues)
                {

                    dayNo = (r.Date - startDate.Value.Date).TotalDays + 1;
                    if (Day.Length == 0)
                    {
                        Day.Append(dayNo);
                    }
                    else
                    {
                        Day.Append(",");
                        Day.Append(dayNo);
                    }
                    RetriveValue(BizActionVO.TherapyExecutionDetial, (int)dayNo);
                }

                if (Day != null)
                    BizActionVO.TherapyExecutionDetial.Day = Day.ToString();
                BizActionVO.TherapyExecutionDetial.IsSurrogate = true;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsUpdateTherapyExecutionBizActionVO)args.Result).SuccessStatus == 3)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "follicular monitoring details already added for selection so cannot save the selection.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                            setupPage(((clsTherapyExecutionVO)ExeGrid1.SelectedItem).PlanTherapyId, 0);
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Details Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                            setupPage(((clsTherapyExecutionVO)ExeGrid1.SelectedItem).PlanTherapyId, 0);
                        }
                    }
                };
                client.ProcessAsync(BizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public clsTherapyExecutionVO SetValueOfVO(clsTherapyExecutionVO ExeVO)
        {
            ExeVO.Day1 = "False";
            ExeVO.Day2 = "False";
            ExeVO.Day3 = "False";
            ExeVO.Day4 = "False";
            ExeVO.Day5 = "False";
            ExeVO.Day6 = "False";
            ExeVO.Day7 = "False";
            ExeVO.Day8 = "False";
            ExeVO.Day9 = "False";
            ExeVO.Day10 = "False";
            ExeVO.Day11 = "False";
            ExeVO.Day12 = "False";
            ExeVO.Day13 = "False";
            ExeVO.Day14 = "False";
            ExeVO.Day15 = "False";
            ExeVO.Day16 = "False";
            ExeVO.Day17 = "False";
            ExeVO.Day18 = "False";
            ExeVO.Day19 = "False";
            ExeVO.Day20 = "False";
            ExeVO.Day21 = "False";
            ExeVO.Day22 = "False";
            ExeVO.Day23 = "False";
            ExeVO.Day24 = "False";
            ExeVO.Day25 = "False";
            ExeVO.Day26 = "False";
            ExeVO.Day27 = "False";
            ExeVO.Day28 = "False";
            ExeVO.Day29 = "False";
            ExeVO.Day30 = "False";
            ExeVO.Day31 = "False";
            ExeVO.Day32 = "False";
            ExeVO.Day33 = "False";
            ExeVO.Day34 = "False";
            ExeVO.Day35 = "False";
            ExeVO.Day36 = "False";
            ExeVO.Day37 = "False";
            ExeVO.Day38 = "False";
            ExeVO.Day39 = "False";
            ExeVO.Day40 = "False";
            ExeVO.Day41 = "False";
            ExeVO.Day42 = "False";
            ExeVO.Day43 = "False";
            ExeVO.Day44 = "False";
            ExeVO.Day45 = "False";
            ExeVO.Day46 = "False";
            ExeVO.Day47 = "False";
            ExeVO.Day48 = "False";
            ExeVO.Day49 = "False";
            ExeVO.Day50 = "False";
            ExeVO.Day51 = "False";
            ExeVO.Day52 = "False";
            ExeVO.Day53 = "False";
            ExeVO.Day54 = "False";
            ExeVO.Day55 = "False";
            ExeVO.Day56 = "False";
            ExeVO.Day57 = "False";
            ExeVO.Day58 = "False";
            ExeVO.Day59 = "False";
            ExeVO.Day60 = "False";


            return ExeVO;
        }
        public clsTherapyExecutionVO RetriveValue(clsTherapyExecutionVO ExeVO, int DayNo)
        {
            switch (DayNo)
            {
                case 1:
                    ExeVO.Day1 = "True";

                    break;
                case 2:
                    ExeVO.Day2 = "True";
                    break;
                case 3:
                    ExeVO.Day3 = "True";
                    break;
                case 4:
                    ExeVO.Day4 = "True";
                    break;
                case 5:
                    ExeVO.Day5 = "True";
                    break;
                case 6:
                    ExeVO.Day6 = "True";
                    break;
                case 7:
                    ExeVO.Day7 = "True";
                    break;
                case 8:
                    ExeVO.Day8 = "True";
                    break;
                case 9:
                    ExeVO.Day9 = "True";
                    break;
                case 10:
                    ExeVO.Day10 = "True";
                    break;
                case 11:
                    ExeVO.Day11 = "True";
                    break;
                case 12:
                    ExeVO.Day12 = "True";
                    break;
                case 13:
                    ExeVO.Day13 = "True";
                    break;
                case 14:
                    ExeVO.Day14 = "True";
                    break;
                case 15:
                    ExeVO.Day15 = "True";
                    break;
                case 16:
                    ExeVO.Day16 = "True";
                    break;
                case 17:
                    ExeVO.Day17 = "True";
                    break;
                case 18:
                    ExeVO.Day18 = "True";
                    break;
                case 19:
                    ExeVO.Day19 = "True";
                    break;
                case 20:
                    ExeVO.Day20 = "True";
                    break;
                case 21:
                    ExeVO.Day21 = "True";
                    break;
                case 22:
                    ExeVO.Day22 = "True";
                    break;
                case 23:
                    ExeVO.Day23 = "True";
                    break;
                case 24:
                    ExeVO.Day24 = "True";
                    break;
                case 25:
                    ExeVO.Day25 = "True";
                    break;
                case 26:
                    ExeVO.Day26 = "True";
                    break;
                case 27:
                    ExeVO.Day27 = "True";
                    break;
                case 28:
                    ExeVO.Day28 = "True";
                    break;
                case 29:
                    ExeVO.Day29 = "True";
                    break;
                case 30:
                    ExeVO.Day30 = "True";
                    break;
                case 31:
                    ExeVO.Day31 = "True";
                    break;
                case 32:
                    ExeVO.Day32 = "True";
                    break;
                case 33:
                    ExeVO.Day33 = "True";
                    break;
                case 34:
                    ExeVO.Day34 = "True";
                    break;
                case 35:
                    ExeVO.Day35 = "True";
                    break;
                case 36:
                    ExeVO.Day36 = "True";
                    break;
                case 37:
                    ExeVO.Day37 = "True";
                    break;
                case 38:
                    ExeVO.Day38 = "True";
                    break;
                case 39:
                    ExeVO.Day39 = "True";
                    break;
                case 40:
                    ExeVO.Day40 = "True";
                    break;
                case 41:
                    ExeVO.Day41 = "True";
                    break;
                case 42:
                    ExeVO.Day42 = "True";
                    break;
                case 43:
                    ExeVO.Day43 = "True";
                    break;
                case 44:
                    ExeVO.Day44 = "True";
                    break;
                case 45:
                    ExeVO.Day45 = "True";
                    break;
                case 46:
                    ExeVO.Day46 = "True";
                    break;
                case 47:
                    ExeVO.Day47 = "True";
                    break;
                case 48:
                    ExeVO.Day48 = "True";
                    break;
                case 49:
                    ExeVO.Day49 = "True";
                    break;
                case 50:
                    ExeVO.Day50 = "True";
                    break;
                case 51:
                    ExeVO.Day51 = "True";
                    break;
                case 52:
                    ExeVO.Day52 = "True";
                    break;
                case 53:
                    ExeVO.Day53 = "True";
                    break;
                case 54:
                    ExeVO.Day54 = "True";
                    break;
                case 55:
                    ExeVO.Day55 = "True";
                    break;
                case 56:
                    ExeVO.Day56 = "True";
                    break;
                case 57:
                    ExeVO.Day57 = "True";
                    break;
                case 58:
                    ExeVO.Day58 = "True";
                    break;
                case 59:
                    ExeVO.Day59 = "True";
                    break;
                case 60:
                    ExeVO.Day60 = "True";
                    break;
                default:
                    break;
            }
            return ExeVO;
        }

        private void ExeGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((clsTherapyExecutionVO)ExeGrid1.SelectedItem) != null)
            {
                if (((clsTherapyExecutionVO)ExeGrid1.SelectedItem).TherapyTypeId == 1)
                {
                    FrmExecutionCalender win = new FrmExecutionCalender();
                    win.Title = "Date of LP ( " + lblSurrogate.Text + " )";
                    win.ExeCal.SelectionMode = CalendarSelectionMode.SingleDate;
                    win.ExeCal.DisplayDateStart = startDate;
                    win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(60);
                    if (win.selecteddates != null)
                        win.selecteddates.Clear();
                    win.selecteddates = SetValueToCal(((clsTherapyExecutionVO)ExeGrid1.SelectedItem));
                    win.OnSaveButton_Click += new RoutedEventHandler(ExecutionCalenderSurrogate_OnSaveButton_Click);
                    win.Show();
                }
                else if (((clsTherapyExecutionVO)ExeGrid1.SelectedItem).TherapyTypeId == 5)
                {
                    FrmExecutionCalender win = new FrmExecutionCalender();
                    win.Title = "ET ( " + lblSurrogate.Text + " )";
                    win.ExeCal.SelectionMode = CalendarSelectionMode.SingleDate;
                    win.ExeCal.DisplayDateStart = startDate;
                    win.ExeCal.DisplayDateEnd = startDate.Value.AddDays(60);
                    if (win.selecteddates != null)
                        win.selecteddates.Clear();
                    win.selecteddates = SetValueToCal(((clsTherapyExecutionVO)ExeGrid1.SelectedItem));
                    win.OnSaveButton_Click += new RoutedEventHandler(ExecutionCalenderSurrogate_OnSaveButton_Click);
                    win.Show();
                }
            }
        }
        private void LoadPatientHeader()
        {
            clsGetPatientBizActionVO BizAction = new PalashDynamics.ValueObjects.Patient.clsGetPatientBizActionVO();
            BizAction.PatientDetails = new PalashDynamics.ValueObjects.Patient.clsPatientVO();
            BizAction.SurrogateID = SurrogateID;
            BizAction.PatientDetails.GeneralDetails.UnitId = SurrogateUnitID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.PatientDetails.GeneralDetails = ((clsGetPatientBizActionVO)args.Result).PatientDetails.GeneralDetails;
                    lblSurrogate.Text = ((clsGetPatientBizActionVO)args.Result).PatientDetails.GeneralDetails.PatientName;
                    lblFemale.Text = CoupleDetails.FemalePatient.PatientName;
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void cmdhistorySurrogate_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID = SurrogateID;
            ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId = SurrogateUnitID;
            ModuleName = "DataDrivenApplication";
            Action = "DataDrivenApplication.Forms.frmChildWinEMR";
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            WebClient c = new WebClient();
            c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompletedSurrogate);
            c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));


        }
        void c_OpenReadCompletedSurrogate(object sender, OpenReadCompletedEventArgs e)
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
                ((IInitiateCIMS)myData).Initiate("19");
                ((ChildWindow)myData).Title = "Surrogate History:- (Name - " + lblSurrogate.Text + ")";
                ((ChildWindow)myData).Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cmdSurrogateExamination_Click(object sender, RoutedEventArgs e)
        {
            long PatientID = SurrogateID;
            long PatientUnitID = SurrogateUnitID;

            frmFemaleExamination obj = new frmFemaleExamination(PatientID, PatientUnitID);
            obj.Title = "Female Examination :- (Name - " + lblSurrogate.Text + ")";
            //obj.Closed += new EventHandler(OnExaminationClosed);
            obj.Show();
        }

        private void cmdSurrogateFindings_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID = SurrogateID;
            ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId = SurrogateUnitID;
            ModuleName = "DataDrivenApplication";
            Action = "DataDrivenApplication.Forms.frmChildWinEMR";
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            WebClient c = new WebClient();
            c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_FeamleFindingSurrogate);
            c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
        }
        void c_OpenReadCompleted_FeamleFindingSurrogate(object sender, OpenReadCompletedEventArgs e)
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
                ((IInitiateCIMS)myData).Initiate("27");
                ((ChildWindow)myData).Title = "Surrogate Findings:- (Name - " + lblSurrogate.Text + ")";
                ((ChildWindow)myData).Show();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cmdSurrogateUSG_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID = SurrogateID;
            ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId = SurrogateUnitID;
            ModuleName = "DataDrivenApplication";
            Action = "DataDrivenApplication.Forms.frmChildWinEMR";
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            WebClient c = new WebClient();
            c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_FemaleUSGSurrogate);
            c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));

        }
        void c_OpenReadCompleted_FemaleUSGSurrogate(object sender, OpenReadCompletedEventArgs e)
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
                ((IInitiateCIMS)myData).Initiate("28");
                ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.FemalePatient;
                ((ChildWindow)myData).Title = "USG :- (Name - " + lblSurrogate.Text + ")";
                ((ChildWindow)myData).Show();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cmdSurrogateLabtest_Click(object sender, RoutedEventArgs e)
        {
            long PatientID = SurrogateID;
            long PatientUnitID = SurrogateUnitID;
            frmLabTestFemale invFemale = new frmLabTestFemale(PatientID, PatientUnitID);
            invFemale.Title = "Female Investigation (Name:- " + lblSurrogate.Text + ")";
            invFemale.Show();
        }

    }
}
