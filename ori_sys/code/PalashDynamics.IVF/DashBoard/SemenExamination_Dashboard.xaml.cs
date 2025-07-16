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
using System.Collections;
using CIMS;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Patient;
using OPDModule;
using System.Windows.Media.Imaging;
using System.IO;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using System.Reflection;
using CIMS.Forms;
using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
using OPDModule.Forms;
using PalashDynamics;
using System.Threading;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.ValueObjects.Administration.StaffMaster;
using System.Xml.Linq;
using System.Windows.Resources;
using PalashDynamics.ValueObjects.DashBoardVO;
using PalashDynamics.Animations;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class SemenExamination_Dashboard : ChildWindow, IInitiateCIMS
    {
        bool IsPatientExist = true;
        string ModuleName;
        string Action;
        public int mode = 0;
        public int CallType = 0;
        public string ListTitle { get; set; }
        public bool IsFromDashBoard = false;
        public void Initiate(string Mode)
        {
            //added by neena

            switch (Mode)
            {
                case "4":
                    first.Content = "List Of ASA Details";
                    validation();
                    break;
            }
        }

        private bool validation()
        {

            if (((IApplicationConfiguration)App.Current).SelectedPatient.IsSurrogate == true)
            {
                if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                {
                    // System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    IsPatientExist = false;

                }
            }
            else if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
            {
                //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                IsPatientExist = false;

            }
            else if (((IApplicationConfiguration)App.Current).SelectedPatient.VisitID == 0)
            {
                //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Mark Visit.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                IsPatientExist = false;

            }
            else if (((IApplicationConfiguration)App.Current).SelectedPatient.GenderID != 1)
            {
                //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                IsPatientExist = false;

            }

            if (IsPatientExist == true)
            {
                PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
                VisitID = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;
                UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                Title = "ASA (Name:- " + ((IApplicationConfiguration)App.Current).SelectedPatient.PatientName + ")";
            }

            return IsPatientExist;

        }

        #region Paging

        public PagedSortableCollectionView<cls_IVFDashboard_SemenVO> DataList { get; private set; }


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

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            // FetchData();

        }
        #endregion



        bool Flagref = false, IsModify = false;
        bool IsPageLoded = false;
        int ClickedFlag = 0;
        public bool IsCancel = true;

        public Int64 UnitID = 0;
        public Int64 PatientID = 0;
        public Int64 PatientUnitID = 0;
        public Int64 VisitID = 0;
        public decimal SumOfGrade = 0, SumOfMorphologyHeadTotal = 0, SumOfTotalMidPiece = 0, SumOfMidPieceTail = 0, SumOfMorphologicalabnormiilities = 0;

        //public decimal SumOfMorphologyHeadTotal = 0;
        //public decimal SumOfTotalMidPiece = 0; 

        private void FillEmbryologist()
        {
            clsGetEmbryologistBizActionVO BizAction = new clsGetEmbryologistBizActionVO();
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
                    objList.AddRange(((clsGetEmbryologistBizActionVO)arg.Result).MasterList);

                    cmbEmbroyologist.ItemsSource = null;
                    cmbEmbroyologist.ItemsSource = objList;
                    cmbEmbroyologist.SelectedItem = objList[0];
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }



        void SemenExamination_Dashboard_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                if (IsPatientExist == false)
                {
                    try
                    {
                        ModuleName = "OPDModule";
                        Action = "CIMS.Forms.QueueManagement";
                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        WebClient c2 = new WebClient();
                        c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                        c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }

                //FillReferralName();
                FillEmbryologist();
                FillMethodCollection();
                FillAbstience();
                FillColor();

                FillViscousRange();
                FillSemenRemarks();

                rbtCentre.IsChecked = true;
                rbtNormal.IsChecked = true;
                rbtNotPresent.IsChecked = true;

                if ((((IApplicationConfiguration)App.Current).SelectedPatient).PatientID != 0 || (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID != null)
                {
                    SetCommandButtonState("Load");
                    //this.DataContext = new cls_IVFDashboard_SemenVO();
                    FetchData();
                    //this.Title = "Female Examination :- (Name - " + (((IApplicationConfiguration)App.Current).SelectedCoupleDetails).FemalePatient.FirstName +
                    //  " " + (((IApplicationConfiguration)App.Current).SelectedCoupleDetails).FemalePatient.LastName + ")";
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Female Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                    msgW1.Show();
                }
            }
        }

        UIElement myData = null;
        UserControl rootPage = Application.Current.RootVisual as UserControl;
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
                this.Close();
                ((IInitiateCIMS)myData).Initiate("IsAndrology");
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
                TextBlock Header = (TextBlock)rootPage.FindName("SampleHeader");
                Header.Text = "Andrology Queue List";
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private void FetchData()
        {

            cls_GetIVFDashboard_SemenBizActionVO BizAction = new cls_GetIVFDashboard_SemenBizActionVO();

            BizAction.List = new List<cls_IVFDashboard_SemenVO>();
            BizAction.PatientID = PatientID;
            BizAction.PatientUnitID = PatientUnitID;


            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {

                    if (((cls_GetIVFDashboard_SemenBizActionVO)args.Result).List != null)
                    {
                        cls_GetIVFDashboard_SemenBizActionVO result = args.Result as cls_GetIVFDashboard_SemenBizActionVO;

                        DataList.TotalItemCount = result.TotalRows;
                        if (result.List != null)
                        {
                            DataList.Clear();

                            foreach (var item in result.List)
                            {
                                DataList.Add(item);
                            }

                            dgSemenExamination.ItemsSource = null;
                            dgSemenExamination.ItemsSource = DataList;
                            dgDataPager.Source = null;
                            dgDataPager.PageSize = BizAction.MaximumRows;
                            dgDataPager.Source = DataList;
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
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }


        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonState(String strFormMode)
        {
            //GeneralExamination.IsEnabled = true;
            //PhysicalExamination.IsEnabled = true;
            //Alterts.IsEnabled = true;

            switch (strFormMode)
            {
                case "Load":
                    cmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = true;
                    CmdPrint.IsEnabled = false;
                    IsCancel = true;
                    break;
                case "New":
                    cmdNew.IsEnabled = false;
                    CmdSave.IsEnabled = true;
                    CmdClose.IsEnabled = true;
                    CmdPrint.IsEnabled = false;
                    IsCancel = false;
                    break;
                case "Save":
                    cmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = true;
                    CmdPrint.IsChecked = false;
                    IsCancel = true;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = false;
                    CmdSave.IsEnabled = true;
                    CmdClose.IsEnabled = true;
                    CmdPrint.IsEnabled = true;
                    IsCancel = false;
                    //GeneralExamination.IsEnabled = false;
                    //PhysicalExamination.IsEnabled = false;
                    //Alterts.IsEnabled = false;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = true;
                    CmdPrint.IsEnabled = false;
                    break;
                default:
                    break;
            }
        }

        #endregion


        #region FILL Methods
        private void FillMethodCollection()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_IVFPlannedSpermCollection;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList.Where(x => x.ID != 24).ToList());

                    if (Flagref == true)
                    {
                        var results = from r in objList
                                      where r.ID == 10
                                      select r;
                        cmbCollection.ItemsSource = null;
                        cmbCollection.ItemsSource = results.ToList();
                        cmbCollection.SelectedItem = results.ToList()[0];
                    }
                    else
                    {
                        cmbCollection.ItemsSource = null;
                        cmbCollection.ItemsSource = objList;
                        cmbCollection.SelectedItem = objList[0];
                    }
                    //cmbCollection.ItemsSource = null;
                    //cmbCollection.ItemsSource = objList;
                }

                if (this.DataContext != null)
                {
                    cmbCollection.SelectedValue = ((cls_IVFDashboard_SemenVO)this.DataContext).MethodOfCollectionID;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillAbstience()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_Abstinence;
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

                    if (Flagref == true)
                    {
                        var results = from r in objList
                                      where r.ID == 10
                                      select r;
                        cmbAbstience.ItemsSource = null;
                        cmbAbstience.ItemsSource = results.ToList();
                        cmbAbstience.SelectedItem = results.ToList()[0];
                    }
                    else
                    {
                        cmbAbstience.ItemsSource = null;
                        cmbAbstience.ItemsSource = objList;
                        cmbAbstience.SelectedItem = objList[0];
                    }
                    //cmbAbstience.ItemsSource = null;
                    //cmbAbstience.ItemsSource = objList;
                }

                if (this.DataContext != null)
                {
                    cmbAbstience.SelectedValue = ((cls_IVFDashboard_SemenVO)this.DataContext).AbstinenceID;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillColor()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_SemenColorMaster;
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

                    if (Flagref == true)
                    {
                        var results = from r in objList
                                      where r.ID == 10
                                      select r;
                        cmbColor.ItemsSource = null;
                        cmbColor.ItemsSource = results.ToList();
                        cmbColor.SelectedItem = results.ToList()[0];
                    }
                    else
                    {
                        cmbColor.ItemsSource = null;
                        cmbColor.ItemsSource = objList;
                        cmbColor.SelectedItem = objList[0];
                    }
                    //cmbColor.ItemsSource = null;
                    //cmbColor.ItemsSource = objList;
                }

                if (this.DataContext != null)
                {
                    cmbColor.SelectedValue = ((cls_IVFDashboard_SemenVO)this.DataContext).ColorID;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillViscousRange()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_ViscousRange;
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

                    if (Flagref == true)
                    {
                        var results = from r in objList
                                      where r.ID == 10
                                      select r;
                        cmbViscousRange.ItemsSource = null;
                        cmbViscousRange.ItemsSource = results.ToList();
                        cmbViscousRange.SelectedItem = results.ToList()[0];
                    }
                    else
                    {
                        cmbViscousRange.ItemsSource = null;
                        cmbViscousRange.ItemsSource = objList;
                        cmbViscousRange.SelectedItem = objList[0];
                    }
                    //cmbColor.ItemsSource = null;
                    //cmbColor.ItemsSource = objList;
                }

                if (this.DataContext != null)
                {
                    cmbViscousRange.SelectedValue = ((cls_IVFDashboard_SemenVO)this.DataContext).RangeViscosityID;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillSemenRemarks()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_SemenRemarks;
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

                    if (Flagref == true)
                    {
                        var results = from r in objList
                                      where r.ID == 10
                                      select r;
                        cmbInterpretationsRemark.ItemsSource = null;
                        cmbInterpretationsRemark.ItemsSource = results.ToList();
                        cmbInterpretationsRemark.SelectedItem = results.ToList()[0];
                    }
                    else
                    {
                        cmbInterpretationsRemark.ItemsSource = null;
                        cmbInterpretationsRemark.ItemsSource = objList;
                        cmbInterpretationsRemark.SelectedItem = objList[0];

                    }
                    //cmbColor.ItemsSource = null;
                    //cmbColor.ItemsSource = objList;
                }

                if (this.DataContext != null)
                {
                    cmbInterpretationsRemark.SelectedValue = ((cls_IVFDashboard_SemenVO)this.DataContext).InterpretationsID;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        #endregion

        public SemenExamination_Dashboard()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            //Paging
            DataList = new PagedSortableCollectionView<cls_IVFDashboard_SemenVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;

            this.DataContext = null;
            // (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID = PatientID;
            // (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId = PatientUnitID;
        }

        public SemenExamination_Dashboard(long PatientID, long PatientUnitID)
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            //Paging
            DataList = new PagedSortableCollectionView<cls_IVFDashboard_SemenVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;

            this.DataContext = null;
            // (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID = PatientID;
            // (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId = PatientUnitID;
        }


        private bool CheckValidations()
        {
            bool result = true;
            try
            {
                if (string.IsNullOrEmpty(dtCollection.Text))
                {
                    dtCollection.SetValidation("Collection Date Is Required");
                    dtCollection.RaiseValidationError();
                    dtCollection.RaiseValidationError();
                    result = false;
                    dtCollection.Focus();
                    // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                }
                else
                {
                    dtCollection.ClearValidationError();
                }
                if ((MasterListItem)cmbCollection.SelectedItem == null)
                {
                    cmbCollection.TextBox.SetValidation("Collection Method Is Required");
                    cmbCollection.TextBox.RaiseValidationError();
                    cmbCollection.Focus();
                    //cmbCollection.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else if (((MasterListItem)cmbCollection.SelectedItem).ID == 0)
                {
                    cmbCollection.TextBox.SetValidation("Collection Method Is Required");
                    cmbCollection.TextBox.RaiseValidationError();
                    cmbCollection.Focus();
                    // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    cmbCollection.TextBox.ClearValidationError();

                //if ((MasterListItem)cmbPatientType.SelectedItem == null)
                //{
                //    cmbPatientType.TextBox.SetValidation("Patient Type Is Required");
                //    cmbPatientType.TextBox.RaiseValidationError();
                //    cmbPatientType.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else if (((MasterListItem)cmbPatientType.SelectedItem).ID == 0)
                //{
                //    cmbPatientType.TextBox.SetValidation("Patient Type Is Required");
                //    cmbPatientType.TextBox.RaiseValidationError();
                //    cmbPatientType.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    cmbPatientType.TextBox.ClearValidationError();
                //if ((MasterListItem)cmbPatientSource.SelectedItem == null)
                //{
                //    cmbPatientSource.TextBox.SetValidation("Patient Source Is Required");
                //    cmbPatientSource.TextBox.RaiseValidationError();
                //    cmbPatientSource.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else if (((MasterListItem)cmbPatientSource.SelectedItem).ID == 0)
                //{
                //    cmbPatientSource.TextBox.SetValidation("Patient Source Is Required");
                //    cmbPatientSource.TextBox.RaiseValidationError();
                //    cmbPatientSource.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    cmbPatientSource.TextBox.ClearValidationError();


                //if (((cls_IVFDashboard_SemenVO)this.DataContext).GeneralDetails.FirstName == null || ((cls_IVFDashboard_SemenVO)this.DataContext).GeneralDetails.FirstName.Trim() == "")
                //{
                //    txtFirstName.SetValidation("First Name Is Required.");
                //    txtFirstName.RaiseValidationError();
                //    txtFirstName.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    txtFirstName.ClearValidationError();

                //if (IsPageLoded)
                //{
                //    if (txtEmail.Text.Trim().Length > 0)
                //    {
                //        if (txtEmail.Text.IsEmailValid())
                //            txtEmail.ClearValidationError();
                //        else
                //        {
                //            txtEmail.SetValidation("Please Enter Valid Email");
                //            txtEmail.RaiseValidationError();
                //            txtEmail.Focus();
                //            PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //            result = false;
                //        }
                //    }
                //    if (((cls_IVFDashboard_SemenVO)this.DataContext).GeneralDetails.DateOfBirth == null)
                //    {
                //        dtpDOB.SetValidation("Birth Date Is Required");
                //        dtpDOB.RaiseValidationError();
                //        dtpDOB.Focus();
                //        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //        result = false;
                //    }
                //    else if (((cls_IVFDashboard_SemenVO)this.DataContext).GeneralDetails.DateOfBirth.Value.Date >= DateTime.Now.Date)
                //    {
                //        dtpDOB.SetValidation("Birth Date Can Not Be Greater Than Todays Date");
                //        dtpDOB.RaiseValidationError();
                //        dtpDOB.Focus();
                //        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //        result = false;
                //    }
                //    else
                //        dtpDOB.ClearValidationError();
                //    if (txtMM.Text != "")
                //    {
                //        if (Convert.ToInt16(txtMM.Text) > 12)
                //        {
                //            txtMM.SetValidation("Month Cannot Be Greater than 12");
                //            txtMM.RaiseValidationError();
                //            txtMM.Focus();
                //            result = false;
                //            PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //            ClickedFlag = 0;
                //        }
                //        else
                //            txtMM.ClearValidationError();
                //    }
                //    if (txtYY.Text != "")
                //    {
                //        if (Convert.ToInt16(txtYY.Text) > 120)
                //        {
                //            txtYY.SetValidation("Age Can Not Be Greater Than 121");
                //            txtYY.RaiseValidationError();
                //            txtYY.Focus();
                //            result = false;
                //            PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //            ClickedFlag = 0;
                //        }
                //        else
                //            txtYY.ClearValidationError();
                //    }
                //}
                ////Spouse
                //if (cmbPatientType.SelectedItem != null && ((MasterListItem)(cmbPatientType.SelectedItem)).ID == 7 && !PatientEditMode) // Couple
                //{
                //    if (string.IsNullOrEmpty(txtSpouseYY.Text) && string.IsNullOrEmpty(txtSpouseMM.Text) && string.IsNullOrEmpty(txtSpouseDD.Text))
                //    {
                //        txtSpouseYY.SetValidation("Age Is Required");
                //        txtSpouseYY.RaiseValidationError();
                //        txtSpouseMM.SetValidation("Age Is Required");
                //        txtSpouseMM.RaiseValidationError();
                //        txtSpouseDD.SetValidation("Age Is Required");
                //        txtSpouseDD.RaiseValidationError();
                //        result = false;
                //        txtSpouseYY.Focus();
                //        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                //    }
                //    else
                //    {
                //        txtSpouseYY.ClearValidationError();
                //        txtSpouseMM.ClearValidationError();
                //        txtSpouseDD.ClearValidationError();
                //    }
                //    if ((MasterListItem)cmbSpouseGender.SelectedItem == null)
                //    {
                //        cmbSpouseGender.TextBox.SetValidation("Gender Is Required");
                //        cmbSpouseGender.TextBox.RaiseValidationError();
                //        cmbSpouseGender.Focus();
                //        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                //        result = false;
                //    }
                //    else if (((MasterListItem)cmbSpouseGender.SelectedItem).ID == 0)
                //    {
                //        cmbSpouseGender.TextBox.SetValidation("Gender Is Required");
                //        cmbSpouseGender.TextBox.RaiseValidationError();
                //        cmbSpouseGender.Focus();
                //        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                //        result = false;
                //    }
                //    else
                //        cmbGender.TextBox.ClearValidationError();

                //    if ((MasterListItem)cmbPatientType.SelectedItem == null)
                //    {
                //        cmbPatientType.TextBox.SetValidation("Patient Type Is Required");
                //        cmbPatientType.TextBox.RaiseValidationError();
                //        cmbPatientType.Focus();
                //        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                //        result = false;
                //    }
                //    else if (((MasterListItem)cmbPatientType.SelectedItem).ID == 0)
                //    {
                //        cmbPatientType.TextBox.SetValidation("Patient Type Is Required");
                //        cmbPatientType.TextBox.RaiseValidationError();
                //        cmbPatientType.Focus();
                //        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                //        result = false;
                //    }
                //    else
                //        cmbPatientType.TextBox.ClearValidationError();

                //    if ((MasterListItem)cmbPatientSource.SelectedItem == null)
                //    {
                //        cmbPatientSource.TextBox.SetValidation("Patient Source Is Required");
                //        cmbPatientSource.TextBox.RaiseValidationError();
                //        cmbPatientSource.Focus();
                //        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                //        result = false;
                //    }
                //    else if (((MasterListItem)cmbPatientSource.SelectedItem).ID == 0)
                //    {
                //        cmbPatientSource.TextBox.SetValidation("Patient Source Is Required");
                //        cmbPatientSource.TextBox.RaiseValidationError();
                //        cmbPatientSource.Focus();
                //        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                //        result = false;
                //    }
                //    else
                //        cmbPatientSource.TextBox.ClearValidationError();



                //    if (((cls_IVFDashboard_SemenVO)this.DataContext).SpouseDetails.FirstName == null || ((cls_IVFDashboard_SemenVO)this.DataContext).SpouseDetails.FirstName.Trim() == "")
                //    {
                //        txtSpouseFirstName.SetValidation("First Name Is Required");
                //        txtSpouseFirstName.RaiseValidationError();
                //        txtSpouseFirstName.Focus();
                //        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                //        result = false;
                //    }
                //    else
                //        txtSpouseFirstName.ClearValidationError();

                //    if (IsPageLoded)
                //    {
                //        if (txtSpouseEmail.Text.Trim().Length > 0)
                //        {
                //            if (txtSpouseEmail.Text.IsEmailValid())
                //                txtSpouseEmail.ClearValidationError();
                //            else
                //            {
                //                txtSpouseEmail.SetValidation("Please Enter Valid Email");
                //                txtSpouseEmail.RaiseValidationError();
                //                txtSpouseEmail.Focus();
                //                PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                //                result = false;
                //            }
                //        }

                //        if (((cls_IVFDashboard_SemenVO)this.DataContext).SpouseDetails.DateOfBirth == null)
                //        {
                //            dtpSpouseDOB.SetValidation("Birth Date Is Required");
                //            dtpSpouseDOB.RaiseValidationError();
                //            dtpSpouseDOB.Focus();
                //            PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                //            result = false;
                //        }
                //        else if (((cls_IVFDashboard_SemenVO)this.DataContext).SpouseDetails.DateOfBirth.Value.Date >= DateTime.Now.Date)
                //        {
                //            dtpSpouseDOB.SetValidation("Birth Date Can Not Be Greater Than Todays Date");
                //            dtpSpouseDOB.RaiseValidationError();
                //            dtpSpouseDOB.Focus();
                //            PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                //            result = false;
                //        }
                //        else
                //            dtpSpouseDOB.ClearValidationError();

                //        if (txtSpouseYY.Text != "")
                //        {
                //            if (Convert.ToInt16(txtSpouseYY.Text) > 120)
                //            {
                //                txtSpouseYY.SetValidation("Age Can Not Be Greater Than 121");
                //                txtSpouseYY.RaiseValidationError();
                //                txtSpouseYY.Focus();
                //                result = false;
                //                PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                //                ClickedFlag = 0;
                //            }
                //            else
                //                txtSpouseYY.ClearValidationError();
                //        }
                //    }
                //}
            }
            catch (Exception)
            {
                // throw;
            }
            return result;
        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;

            if (ClickedFlag == 1)
            {
                bool saveDtls = true;

                saveDtls = CheckValidations();

                //if (saveDtls == true && visitwin != null)
                //{
                //    saveDtls = visitwin.CheckValidations();
                //    if (!saveDtls) PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Visit;
                //}

                if (saveDtls == true)
                {
                    string msgText = "";
                    msgText = "Are You Sure \n You Want To Save The Semen Examination Details?";
                    MessageBoxControl.MessageBoxChildWindow msgW =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                    msgW.Show();
                }
                else
                    ClickedFlag = 0;
            }
        }



        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                SaveSemenExamination();
            else
                ClickedFlag = 0;

        }

        private void SaveSemenExamination()
        {
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();


            cls_IVFDashboard_AddSemenBizActionVO BizAction = new cls_IVFDashboard_AddSemenBizActionVO();
            //BizAction.SemensExaminationDetails = (cls_IVFDashboard_SemenVO)this.DataContext;


            BizAction.SemensExaminationDetails.ID = ((cls_IVFDashboard_SemenVO)this.DataContext).ID;

            BizAction.SemensExaminationDetails.UnitID = UnitID;
            BizAction.SemensExaminationDetails.PatientID = PatientID;
            BizAction.SemensExaminationDetails.PatientUnitID = PatientUnitID;
            BizAction.SemensExaminationDetails.VisitID = VisitID;

            if (dtCollection.SelectedDate.Value.Date != null)
                BizAction.SemensExaminationDetails.CollectionDate = dtCollection.SelectedDate.Value.Date;

            if (CollectionTime.Value != null)
                BizAction.SemensExaminationDetails.CollectionDate = BizAction.SemensExaminationDetails.CollectionDate.Value.Add(CollectionTime.Value.Value.TimeOfDay);

            if (cmbCollection.SelectedItem != null)
                BizAction.SemensExaminationDetails.MethodOfCollectionID = ((MasterListItem)cmbCollection.SelectedItem).ID;

            if (cmbAbstience.SelectedItem != null)
                BizAction.SemensExaminationDetails.AbstinenceID = ((MasterListItem)cmbAbstience.SelectedItem).ID;

            if (SampRecTime.Value != null)
                BizAction.SemensExaminationDetails.TimeRecSampLab = SampRecTime.Value;

            if (rbtYes.IsChecked == true)
                BizAction.SemensExaminationDetails.Complete = true;
            else if (rbtNo.IsChecked == true)
                BizAction.SemensExaminationDetails.Complete = false;

            if (rbtCentre.IsChecked == true)
                BizAction.SemensExaminationDetails.CollecteAtCentre = true;
            else if (rbtOutSideCentre.IsChecked == true)
                BizAction.SemensExaminationDetails.CollecteAtCentre = false;

            if (cmbColor.SelectedItem != null)
                BizAction.SemensExaminationDetails.ColorID = ((MasterListItem)cmbColor.SelectedItem).ID;

            if (!string.IsNullOrEmpty(TxtQuantity.Text.Trim()))
                BizAction.SemensExaminationDetails.Quantity = float.Parse(TxtQuantity.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPH.Text.Trim()))
                BizAction.SemensExaminationDetails.PH = float.Parse(TxtPH.Text.Trim());

            //BizAction.SemensExaminationDetails.LiquificationTime = LiquiTime.Value;
            BizAction.SemensExaminationDetails.LiquificationTime = TxtLiquiTime.Text;

            if (rbtNormal.IsChecked == true)
            {
                BizAction.SemensExaminationDetails.Viscosity = true;
                cmbViscousRange.Visibility = Visibility.Collapsed;
            }
            else if (rbtViscous.IsChecked == true)
            {
                BizAction.SemensExaminationDetails.Viscosity = false;
                cmbViscousRange.Visibility = Visibility.Visible;
            }


            if (cmbViscousRange.SelectedItem != null)
                BizAction.SemensExaminationDetails.RangeViscosityID = ((MasterListItem)cmbViscousRange.SelectedItem).ID;

            if (rbtPresent.IsChecked == true)
                BizAction.SemensExaminationDetails.Odour = true;
            else if (rbtNotPresent.IsChecked == true)
                BizAction.SemensExaminationDetails.Odour = false;

            if (!string.IsNullOrEmpty(TxtSpermCount.Text.Trim()))
                BizAction.SemensExaminationDetails.SpermCount = float.Parse(TxtSpermCount.Text.Trim());

            if (!string.IsNullOrEmpty(TxtTotCount.Text.Trim()))
                BizAction.SemensExaminationDetails.TotalSpermCount = float.Parse(TxtTotCount.Text.Trim());

            if (!string.IsNullOrEmpty(TxtMotility.Text.Trim()))
                BizAction.SemensExaminationDetails.Motility = float.Parse(TxtMotility.Text.Trim());

            //if (!string.IsNullOrEmpty(TxtNonMotility.Text.Trim()))
            //BizAction.SemensExaminationDetails.NonMotility = float.Parse(TxtNonMotility.Text.Trim());

            if (!string.IsNullOrEmpty(TxtTotalMotility.Text.Trim()))
                BizAction.SemensExaminationDetails.TotalMotility = float.Parse(TxtTotalMotility.Text.Trim());

            if (!string.IsNullOrEmpty(TxtGradeI.Text.Trim()))
                BizAction.SemensExaminationDetails.MotilityGradeI = float.Parse(TxtGradeI.Text.Trim());

            if (!string.IsNullOrEmpty(TxtGradeII.Text.Trim()))
                BizAction.SemensExaminationDetails.MotilityGradeII = float.Parse(TxtGradeII.Text.Trim());

            if (!string.IsNullOrEmpty(TxtGradeIII.Text.Trim()))
                BizAction.SemensExaminationDetails.MotilityGradeIII = float.Parse(TxtGradeIII.Text.Trim());

            if (!string.IsNullOrEmpty(TxtGradeIV.Text.Trim()))
                BizAction.SemensExaminationDetails.MotilityGradeIV = float.Parse(TxtGradeIV.Text.Trim());

            if (!string.IsNullOrEmpty(TxtAmorphus.Text.Trim()))
                BizAction.SemensExaminationDetails.Amorphus = float.Parse(TxtAmorphus.Text.Trim());

            if (!string.IsNullOrEmpty(TxtNeckappendages.Text.Trim()))
                BizAction.SemensExaminationDetails.NeckAppendages = float.Parse(TxtNeckappendages.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPyriform.Text.Trim()))
                BizAction.SemensExaminationDetails.Pyriform = float.Parse(TxtPyriform.Text.Trim());

            if (!string.IsNullOrEmpty(TxtMacrocefalic.Text.Trim()))
                BizAction.SemensExaminationDetails.Macrocefalic = float.Parse(TxtMacrocefalic.Text.Trim());

            if (!string.IsNullOrEmpty(TxtMicrocefalic.Text.Trim()))
                BizAction.SemensExaminationDetails.Microcefalic = float.Parse(TxtMicrocefalic.Text.Trim());

            if (!string.IsNullOrEmpty(TxtBrokenneck.Text.Trim()))
                BizAction.SemensExaminationDetails.BrockenNeck = float.Parse(TxtBrokenneck.Text.Trim());

            if (!string.IsNullOrEmpty(TxtRoundHead.Text.Trim()))
                BizAction.SemensExaminationDetails.RoundHead = float.Parse(TxtRoundHead.Text.Trim());

            if (!string.IsNullOrEmpty(TxtDoubleHead.Text.Trim()))
                BizAction.SemensExaminationDetails.DoubleHead = float.Parse(TxtDoubleHead.Text.Trim());

            if (!string.IsNullOrEmpty(TxtTotal.Text.Trim()))
                BizAction.SemensExaminationDetails.Total = float.Parse(TxtTotal.Text.Trim());

            if (!string.IsNullOrEmpty(TxtMorphologicalabnormiilities.Text.Trim()))
                BizAction.SemensExaminationDetails.MorphologicalAbnormilities = float.Parse(TxtMorphologicalabnormiilities.Text.Trim());

            if (!string.IsNullOrEmpty(TxtNormalMorphology.Text.Trim()))
                BizAction.SemensExaminationDetails.NormalMorphology = float.Parse(TxtNormalMorphology.Text.Trim());

            //if (!string.IsNullOrEmpty(TxtPyriform.Text.Trim()))
            //    BizAction.SemensExaminationDetails.Pyriform = float.Parse(TxtPyriform.Text.Trim());

            if (!string.IsNullOrEmpty(TxtComment.Text.Trim()))
                BizAction.SemensExaminationDetails.Comment = TxtComment.Text.Trim();

            if (!string.IsNullOrEmpty(TxtCytoplasmicDroplet.Text.Trim()))
                BizAction.SemensExaminationDetails.CytoplasmicDroplet = float.Parse(TxtCytoplasmicDroplet.Text.Trim());

            if (!string.IsNullOrEmpty(TxtOthersMidPiece.Text.Trim()))
                BizAction.SemensExaminationDetails.Others = float.Parse(TxtOthersMidPiece.Text.Trim());

            //if (!string.IsNullOrEmpty(TxtPyriform.Text.Trim()))
            //    BizAction.SemensExaminationDetails.Pyriform = float.Parse(TxtPyriform.Text.Trim());


            if (!string.IsNullOrEmpty(TxtTotalMidPiece.Text.Trim()))
                BizAction.SemensExaminationDetails.MidPieceTotal = float.Parse(TxtTotalMidPiece.Text.Trim());

            if (!string.IsNullOrEmpty(TxtCoiledTail.Text.Trim()))
                BizAction.SemensExaminationDetails.CoiledTail = float.Parse(TxtCoiledTail.Text.Trim());

            if (!string.IsNullOrEmpty(TxtShortTail.Text.Trim()))
                BizAction.SemensExaminationDetails.ShortTail = float.Parse(TxtShortTail.Text.Trim());

            if (!string.IsNullOrEmpty(TxtHairPinTail.Text.Trim()))
                BizAction.SemensExaminationDetails.HairpinTail = float.Parse(TxtHairPinTail.Text.Trim());

            if (!string.IsNullOrEmpty(TxtDoubleTail.Text.Trim()))
                BizAction.SemensExaminationDetails.DoubleTail = float.Parse(TxtDoubleTail.Text.Trim());

            if (!string.IsNullOrEmpty(TxtOtherTail.Text.Trim()))
                BizAction.SemensExaminationDetails.TailOthers = float.Parse(TxtOtherTail.Text.Trim());

            if (!string.IsNullOrEmpty(TxtTotalTail.Text.Trim()))
                BizAction.SemensExaminationDetails.TailTotal = float.Parse(TxtTotalTail.Text.Trim());

            if (!string.IsNullOrEmpty(TxtHeadToHead.Text.Trim()))
                BizAction.SemensExaminationDetails.HeadToHead = TxtHeadToHead.Text.Trim();

            if (!string.IsNullOrEmpty(TxtTailToTail.Text.Trim()))
                BizAction.SemensExaminationDetails.TailToTail = TxtTailToTail.Text.Trim();

            if (!string.IsNullOrEmpty(TxtHeadToTail.Text.Trim()))
                BizAction.SemensExaminationDetails.HeadToTail = TxtHeadToTail.Text.Trim();


            if (!string.IsNullOrEmpty(TxtSpermAgreagtion.Text.Trim()))
                BizAction.SemensExaminationDetails.SpermToOther = TxtSpermAgreagtion.Text.Trim();

            if (!string.IsNullOrEmpty(TxtPusCells.Text.Trim()))
                BizAction.SemensExaminationDetails.PusCells = TxtPusCells.Text.Trim();

            if (!string.IsNullOrEmpty(TxtRoundCells.Text.Trim()))
                BizAction.SemensExaminationDetails.RoundCells = TxtRoundCells.Text.Trim();

            if (!string.IsNullOrEmpty(TxtEpithelialCells.Text.Trim()))
                BizAction.SemensExaminationDetails.EpithelialCells = TxtEpithelialCells.Text.Trim();

            if (!string.IsNullOrEmpty(TxtInfections.Text.Trim()))
                BizAction.SemensExaminationDetails.Infections = TxtInfections.Text.Trim();

            if (!string.IsNullOrEmpty(TxtOtherFindings.Text.Trim()))
                BizAction.SemensExaminationDetails.OtherFindings = TxtOtherFindings.Text.Trim();

            if (cmbInterpretationsRemark.SelectedItem != null)
                BizAction.SemensExaminationDetails.InterpretationsID = ((MasterListItem)cmbInterpretationsRemark.SelectedItem).ID;

            if (cmbEmbroyologist.SelectedItem != null)
                BizAction.SemensExaminationDetails.EmbryologistID = ((MasterListItem)cmbEmbroyologist.SelectedItem).ID;

            //added by neena
            if (!string.IsNullOrEmpty(TxtComments.Text.Trim()))
                BizAction.SemensExaminationDetails.AllComments = TxtComments.Text.Trim();
            if (!string.IsNullOrEmpty(TxtSperm5thPercentile.Text.Trim()))
                BizAction.SemensExaminationDetails.Sperm5thPercentile = float.Parse(TxtSperm5thPercentile.Text.Trim());
            if (!string.IsNullOrEmpty(TxtSperm75thPercentile.Text.Trim()))
                BizAction.SemensExaminationDetails.Sperm75thPercentile = float.Parse(TxtSperm75thPercentile.Text.Trim());
            if (!string.IsNullOrEmpty(TxtEjaculate5thPercentile.Text.Trim()))
                BizAction.SemensExaminationDetails.Ejaculate5thPercentile = float.Parse(TxtEjaculate5thPercentile.Text.Trim());
            if (!string.IsNullOrEmpty(TxtEjaculate75thPercentile.Text.Trim()))
                BizAction.SemensExaminationDetails.Ejaculate75thPercentile = float.Parse(TxtEjaculate75thPercentile.Text.Trim());
            if (!string.IsNullOrEmpty(TxtTotalMotility5thPercentile.Text.Trim()))
                BizAction.SemensExaminationDetails.TotalMotility5thPercentile = float.Parse(TxtTotalMotility5thPercentile.Text.Trim());
            if (!string.IsNullOrEmpty(TxtTotalMotility75thPercentile.Text.Trim()))
                BizAction.SemensExaminationDetails.TotalMotility75thPercentile = float.Parse(TxtTotalMotility75thPercentile.Text.Trim());
            if (!string.IsNullOrEmpty(TxtRapidProgressive.Text.Trim()))
                BizAction.SemensExaminationDetails.RapidProgressive = float.Parse(TxtRapidProgressive.Text.Trim());
            if (!string.IsNullOrEmpty(TxtSlowProgressive.Text.Trim()))
                BizAction.SemensExaminationDetails.SlowProgressive = float.Parse(TxtSlowProgressive.Text.Trim());
            if (!string.IsNullOrEmpty(TxtSpermMorphology5thPercentile.Text.Trim()))
                BizAction.SemensExaminationDetails.SpermMorphology5thPercentile = float.Parse(TxtSpermMorphology5thPercentile.Text.Trim());
            if (!string.IsNullOrEmpty(TxtSpermMorphology75thPercentile.Text.Trim()))
                BizAction.SemensExaminationDetails.SpermMorphology75thPercentile = float.Parse(TxtSpermMorphology75thPercentile.Text.Trim());
            if (!string.IsNullOrEmpty(TxtNormalFormsComments.Text.Trim()))
                BizAction.SemensExaminationDetails.NormalFormsComments = TxtNormalFormsComments.Text.Trim();
            if (!string.IsNullOrEmpty(TxtOverAllDefectsComments.Text.Trim()))
                BizAction.SemensExaminationDetails.OverAllDefectsComments = TxtOverAllDefectsComments.Text.Trim();
            if (!string.IsNullOrEmpty(TxtHeadDefectsComments.Text.Trim()))
                BizAction.SemensExaminationDetails.HeadDefectsComments = TxtHeadDefectsComments.Text.Trim();
            if (!string.IsNullOrEmpty(TxtMidPieceNeckDefectsComments.Text.Trim()))
                BizAction.SemensExaminationDetails.MidPieceNeckDefectsComments = TxtMidPieceNeckDefectsComments.Text.Trim();
            if (!string.IsNullOrEmpty(TxtTailDefectsComments.Text.Trim()))
                BizAction.SemensExaminationDetails.TailDefectsComments = TxtTailDefectsComments.Text.Trim();
            if (!string.IsNullOrEmpty(TxtExcessiveResidualComments.Text.Trim()))
                BizAction.SemensExaminationDetails.ExcessiveResidualComments = TxtExcessiveResidualComments.Text.Trim();
            if (!string.IsNullOrEmpty(TxtSpermMorphologySubNormal.Text.Trim()))
                BizAction.SemensExaminationDetails.SpermMorphologySubNormal = TxtSpermMorphologySubNormal.Text.Trim();
            if (!string.IsNullOrEmpty(TxtTotalAdvanceMotility.Text.Trim()))
                BizAction.SemensExaminationDetails.TotalAdvanceMotility = float.Parse(TxtTotalAdvanceMotility.Text.Trim());
            //

            //if (!string.IsNullOrEmpty(TxtInterpretationsRemark.Text.Trim()))
            //    BizAction.SemensExaminationDetails.Interpretations = TxtInterpretationsRemark.Text.Trim();  

            //======================================================
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {

                    Indicatior.Close();

                    if (IsModify == true)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient Semen Examation Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.OnMessageBoxClosed += (re) =>
                        {
                            // No Code 
                            objAnimation.Invoke(RotationType.Backward);
                            SetCommandButtonState("Load");
                            CmdSave.Content = "Save";
                            FetchData();
                        };
                        msgW1.Show();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient Semen Examation Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.OnMessageBoxClosed += (re) =>
                        {
                            // No Code 
                            objAnimation.Invoke(RotationType.Backward);
                            SetCommandButtonState("Load");
                            CmdSave.Content = "Save";
                            FetchData();
                        };
                        msgW1.Show();
                    }
                }
                else
                {
                    //CmdSave.IsEnabled = true;
                    ClickedFlag = 0;
                    Indicatior.Close();
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding patient semen examination.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //SetCommandButtonState("Load");
            //objAnimation.Invoke(RotationType.Backward);
            //ClearAll();

            if (IsCancel == true)
            {
                this.DialogResult = false;
            }
            else
            {
                objAnimation.Invoke(RotationType.Backward);
                IsCancel = true;
                CmdSave.Content = "Save";
                FetchData();
            }
            SetCommandButtonState("Cancel");
        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
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

        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }
        private SwivelAnimation objAnimation;
        public long SelectedRecord;

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient.IsClinicVisited == true || IsFromDashBoard == true)
            {
                this.SetCommandButtonState("New");
                // ClearFormData();
                //  clsPatientGeneralVO FemalePatientDetails = new clsPatientGeneralVO();
                //  FemalePatientDetails = (clsPatientGeneralVO)Female.DataContext;


                cls_IVFDashboard_SemenVO obj = new cls_IVFDashboard_SemenVO();
                this.DataContext = obj;     // new clsGeneralExaminationVO();

                IsModify = false;
                ClickedFlag = 0;
                ClearAll();


                ((cls_IVFDashboard_SemenVO)this.DataContext).VolumeRange = "<1.5 - Low, 2.0 - Normal, >2.0 - High";
                ((cls_IVFDashboard_SemenVO)this.DataContext).PHRange = "<7.0 - Acidic, >=7.2 - Normal";
                ((cls_IVFDashboard_SemenVO)this.DataContext).PusCellsRange = "<04 - Not Significant";
                ((cls_IVFDashboard_SemenVO)this.DataContext).MorphologyAbnormilityRange = "<4% - Abnormal forms";
                ((cls_IVFDashboard_SemenVO)this.DataContext).NormalMorphologyRange = ">=4 Normal forms";
                ((cls_IVFDashboard_SemenVO)this.DataContext).SpermConcentrationRange = ">15 mill/ml - Normal";
                ((cls_IVFDashboard_SemenVO)this.DataContext).EjaculateRange = ">= 39 millions - Normal";
                ((cls_IVFDashboard_SemenVO)this.DataContext).TotalMotilityRange = ">40% - Normal";

                grpPhysicalExam.DataContext = (cls_IVFDashboard_SemenVO)this.DataContext;
                grpMicroscopicExam.DataContext = (cls_IVFDashboard_SemenVO)this.DataContext;
                grpGradeMotality.DataContext = (cls_IVFDashboard_SemenVO)this.DataContext;
                grpMorphologyHead.DataContext = (cls_IVFDashboard_SemenVO)this.DataContext;
                grpOtherParameters.DataContext = (cls_IVFDashboard_SemenVO)this.DataContext;



                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                      && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AllowClinicalTransaction == true)
                    {
                        //do nothing
                    }
                    else
                        CmdSave.IsEnabled = false;
                }
                objAnimation.Invoke(RotationType.Forward);
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                new MessageBoxControl.MessageBoxChildWindow("Palash", "Current Visit is Closed", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        private void ClearAll()
        {
            this.BackPanel.DataContext = new cls_IVFDashboard_SemenVO();

            SampRecTime.Value = null;

            this.DataContext = new cls_IVFDashboard_SemenVO();
            grpContactDetails.DataContext = null;
            grpPhysicalExam.DataContext = null;
            grpMicroscopicExam.DataContext = null;
            grpGradeMotality.DataContext = null;
            grpMorphologyHead.DataContext = null;
            grpMidPiece.DataContext = null;
            grpMidPieceTail.DataContext = null;
            grpOtherParameters.DataContext = null;
            grpInterpretations.DataContext = null;
            grpAgulation.DataContext = null;


            dtCollection.SelectedDate = null;
            cmbCollection.SelectedValue = (long)0;
            cmbAbstience.SelectedValue = (long)0;
            cmbColor.SelectedValue = (long)0;

            dtCollection.SelectedDate = null;
            SampRecTime.Value = null;
            CollectionTime.Value = null;
            //LiquiTime.Value = null;

            rbtCentre.IsChecked = true;
            rbtNormal.IsChecked = true;
            rbtNotPresent.IsChecked = true;

            ////rbtYes.IsChecked
            ////rbtNo.IsChecked
            TxtLiquiTime.Text = "";
            TxtQuantity.Text = "0";
            TxtPH.Text = "0";
            //LiquiTime.Value
            //rbtNormal.IsChecked
            //rbtViscous.IsChecked
            TxtSpermCount.Text = "0";
            TxtTotCount.Text = "0";
            TxtMotility.Text = "0";
            //TxtNonMotility.Text = "";
            TxtTotalMotility.Text = "0";
            TxtGradeI.Text = "0";
            TxtGradeII.Text = "0";
            TxtGradeIII.Text = "0";
            TxtGradeIV.Text = "0";
            TxtAmorphus.Text = "0";
            TxtNeckappendages.Text = "0";
            TxtPyriform.Text = "0";
            TxtMacrocefalic.Text = "0";
            TxtMicrocefalic.Text = "0";
            TxtBrokenneck.Text = "0";
            TxtRoundHead.Text = "0";
            TxtDoubleHead.Text = "0";
            TxtTotal.Text = "0";
            TxtMorphologicalabnormiilities.Text = "0";
            TxtNormalMorphology.Text = "0";
            TxtPyriform.Text = "0";
            TxtComment.Text = "";
            TxtComments.Text = "";
            TxtCytoplasmicDroplet.Text = "0";
            TxtOthersMidPiece.Text = "0";
            TxtTotalMidPiece.Text = "0";
            TxtCoiledTail.Text = "0";
            TxtShortTail.Text = "0";
            TxtHairPinTail.Text = "0";
            TxtDoubleTail.Text = "0";
            TxtOtherTail.Text = "0";
            TxtTotalTail.Text = "0";
            TxtHeadToHead.Text = "";
            TxtTailToTail.Text = "";
            TxtHeadToTail.Text = "";
            TxtSpermAgreagtion.Text = "";

            TxtPusCells.Text = "";
            TxtRoundCells.Text = "";
            TxtEpithelialCells.Text = "";
            TxtInfections.Text = "";
            TxtOtherFindings.Text = "";
            //TxtInterpretationsRemark.Text = "";
            cmbInterpretationsRemark.SelectedValue = (long)0;
        }


        private void hlbViewExamination_Click(object sender, RoutedEventArgs e)
        {
            if (dgSemenExamination.SelectedItem != null)
            {
                SetCommandButtonState("Modify");
                ClickedFlag = 0;
                ClearAll();

                this.DataContext = new cls_IVFDashboard_SemenVO();
                this.DataContext = (cls_IVFDashboard_SemenVO)dgSemenExamination.SelectedItem;
                SelectedRecord = ((cls_IVFDashboard_SemenVO)dgSemenExamination.SelectedItem).ID;

                grpContactDetails.DataContext = (cls_IVFDashboard_SemenVO)this.DataContext;
                grpPhysicalExam.DataContext = (cls_IVFDashboard_SemenVO)this.DataContext;
                grpMicroscopicExam.DataContext = (cls_IVFDashboard_SemenVO)this.DataContext;
                grpGradeMotality.DataContext = (cls_IVFDashboard_SemenVO)this.DataContext;
                grpMorphologyHead.DataContext = (cls_IVFDashboard_SemenVO)this.DataContext;
                grpMidPiece.DataContext = (cls_IVFDashboard_SemenVO)this.DataContext;
                grpMidPieceTail.DataContext = (cls_IVFDashboard_SemenVO)this.DataContext;
                grpOtherParameters.DataContext = (cls_IVFDashboard_SemenVO)this.DataContext;
                grpInterpretations.DataContext = (cls_IVFDashboard_SemenVO)this.DataContext;
                grpAgulation.DataContext = (cls_IVFDashboard_SemenVO)this.DataContext;
                grpAdvanceGradeMotality.DataContext = (cls_IVFDashboard_SemenVO)this.DataContext;
                if (((cls_IVFDashboard_SemenVO)this.DataContext).AllComments != null)
                    TxtComments.Text = ((cls_IVFDashboard_SemenVO)this.DataContext).AllComments;
                //grpEmb.DataContext = (cls_IVFDashboard_SemenVO)this.DataContext;
                if (((cls_IVFDashboard_SemenVO)this.DataContext).EmbryologistID != null)
                    cmbEmbroyologist.SelectedValue = ((cls_IVFDashboard_SemenVO)this.DataContext).EmbryologistID;


                if (((cls_IVFDashboard_SemenVO)this.DataContext).Complete == true)
                {
                    rbtYes.IsChecked = true;
                }
                else
                {
                    rbtNo.IsChecked = true;
                }

                if (((cls_IVFDashboard_SemenVO)this.DataContext).CollecteAtCentre == true)
                {
                    rbtCentre.IsChecked = true;
                }
                else
                {
                    rbtOutSideCentre.IsChecked = true;
                }

                if (((cls_IVFDashboard_SemenVO)this.DataContext).Viscosity == true)
                {
                    rbtNormal.IsChecked = true;
                }
                else
                {
                    rbtViscous.IsChecked = true;
                }
                if (((cls_IVFDashboard_SemenVO)this.DataContext).Odour == true)
                {
                    rbtPresent.IsChecked = true;
                }
                else
                {
                    rbtNotPresent.IsChecked = true;
                }




                objAnimation.Invoke(RotationType.Forward);

                CmdSave.Content = "Modify";

                IsModify = true;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }




        private void Total_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //TextBox txtbox = (TextBox)sender;
                //App.GenericMethod.IsValidEntry(txtbox, ValidationType.IsDecimalDigitsOnly);


                decimal varpreRapid = 0;
                decimal varpreSlow = 0;
                decimal varpreNon = 0;


                if (TxtGradeI.Text == "" || TxtGradeI.Text == null)
                    varpreRapid = 0;
                else
                    varpreRapid = Convert.ToDecimal(TxtGradeI.Text.Trim());


                if (TxtGradeII.Text == "" || TxtGradeII.Text == null)
                    varpreSlow = 0;
                else
                    varpreSlow = Convert.ToDecimal(TxtGradeII.Text.Trim());

                if (TxtGradeIII.Text == "" || TxtGradeIII.Text == null)
                    varpreNon = 0;
                else
                    varpreNon = Convert.ToDecimal(TxtGradeIII.Text.Trim());

                SumOfGrade = (Convert.ToDecimal(varpreRapid)) + (Convert.ToDecimal(varpreSlow)) + (Convert.ToDecimal(varpreNon));

                TxtTotalMotility.Text = SumOfGrade.ToString();
                //txtSpermConcentration.Text = SumOfGrade.ToString();

                TxtGradeIV.Text = (100 - SumOfGrade).ToString();

                if (Convert.ToInt64(TxtTotalMotility.Text) < 40)
                    TxtTotalMotility.Foreground = new SolidColorBrush(Colors.Red);
                else
                    TxtTotalMotility.Foreground = new SolidColorBrush(Colors.Black);

            }
            catch (Exception ex)
            {
                //App.ErrorLog(ex);

            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            long ID, UnitID;
            if (dgSemenExamination.SelectedItem != null)
            {
                ID = ((cls_IVFDashboard_SemenVO)dgSemenExamination.SelectedItem).ID;
                UnitID = ((cls_IVFDashboard_SemenVO)dgSemenExamination.SelectedItem).UnitID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/IVFDashboard/SemenExamination.aspx?ID=" + ID + "&UnitID=" + UnitID + "&PrintFomatID=" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID), "_blank");
            }
        }


        private void rbtNormal_Checked(object sender, RoutedEventArgs e)
        {
            if (rbtNormal.IsChecked == true)
            {
                cmbViscousRange.Visibility = Visibility.Collapsed;
            }
            else if (rbtViscous.IsChecked == true)
            {
                cmbViscousRange.Visibility = Visibility.Visible;
            }
        }

        private void txtFloatNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValueDouble() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtFloatNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }



        private void MorphologyHeadTotal_TextChanged(object sender, TextChangedEventArgs e)
        {
            //TxtTotal.Text
            try
            {
                decimal varAmp = 0;
                decimal varNeck = 0;
                decimal varPyr = 0;
                decimal varMacr = 0;
                decimal varMicr = 0;
                decimal varBrok = 0;
                decimal varRoun = 0;
                decimal varDoub = 0;


                if (TxtAmorphus.Text == "" || TxtAmorphus.Text == null)
                    varAmp = 0;
                else
                    varAmp = Convert.ToDecimal(TxtAmorphus.Text.Trim());


                if (TxtNeckappendages.Text == "" || TxtNeckappendages.Text == null)
                    varNeck = 0;
                else
                    varNeck = Convert.ToDecimal(TxtNeckappendages.Text.Trim());

                if (TxtPyriform.Text == "" || TxtPyriform.Text == null)
                    varPyr = 0;
                else
                    varPyr = Convert.ToDecimal(TxtPyriform.Text.Trim());


                if (TxtMacrocefalic.Text == "" || TxtMacrocefalic.Text == null)
                    varMacr = 0;
                else
                    varMacr = Convert.ToDecimal(TxtMacrocefalic.Text.Trim());


                if (TxtMicrocefalic.Text == "" || TxtMicrocefalic.Text == null)
                    varMicr = 0;
                else
                    varMicr = Convert.ToDecimal(TxtMicrocefalic.Text.Trim());


                if (TxtBrokenneck.Text == "" || TxtBrokenneck.Text == null)
                    varBrok = 0;
                else
                    varBrok = Convert.ToDecimal(TxtBrokenneck.Text.Trim());

                if (TxtRoundHead.Text == "" || TxtRoundHead.Text == null)
                    varRoun = 0;
                else
                    varRoun = Convert.ToDecimal(TxtRoundHead.Text.Trim());


                if (TxtDoubleHead.Text == "" || TxtDoubleHead.Text == null)
                    varDoub = 0;
                else
                    varDoub = Convert.ToDecimal(TxtDoubleHead.Text.Trim());


                SumOfMorphologyHeadTotal = (Convert.ToDecimal(varAmp)) + (Convert.ToDecimal(varNeck)) + (Convert.ToDecimal(varPyr)) + (Convert.ToDecimal(varMacr)) + (Convert.ToDecimal(varMicr)) + (Convert.ToDecimal(varBrok)) + (Convert.ToDecimal(varRoun)) + (Convert.ToDecimal(varDoub));


                TxtTotal.Text = SumOfMorphologyHeadTotal.ToString();


                // TxtGradeIV.Text = (100 - SumOfGrade).ToString();

            }
            catch (Exception ex)
            {
                //App.ErrorLog(ex);

            }





        }

        private void MidPieceTotal_TextChanged(object sender, TextChangedEventArgs e)
        {

            try
            {
                decimal varCytdrp = 0;
                decimal varOthMid = 0;
                //decimal varpreNon = 0;


                if (TxtCytoplasmicDroplet.Text == "" || TxtCytoplasmicDroplet.Text == null)
                    varCytdrp = 0;
                else
                    varCytdrp = Convert.ToDecimal(TxtCytoplasmicDroplet.Text.Trim());


                if (TxtOthersMidPiece.Text == "" || TxtOthersMidPiece.Text == null)
                    varOthMid = 0;
                else
                    varOthMid = Convert.ToDecimal(TxtOthersMidPiece.Text.Trim());


                //if (TxtGradeIII.Text == "" || TxtGradeIII.Text == null)
                //    varpreNon = 0;
                //else
                //    varpreNon = Convert.ToDecimal(TxtGradeIII.Text.Trim());

                SumOfGrade = (Convert.ToDecimal(varCytdrp)) + (Convert.ToDecimal(varOthMid));

                TxtTotalMidPiece.Text = SumOfGrade.ToString();
                //txtSpermConcentration.Text = SumOfGrade.ToString();

                //TxtGradeIV.Text = (100 - SumOfGrade).ToString();

            }
            catch (Exception ex)
            {
                //App.ErrorLog(ex);

            }
        }



        private void MidPieceTail_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal varCoil = 0;
                decimal varShort = 0;
                decimal varHrPin = 0;
                decimal varDoub = 0;
                decimal varOthe = 0;

                if (TxtCoiledTail.Text == "" || TxtCoiledTail.Text == null)
                    varCoil = 0;
                else
                    varCoil = Convert.ToDecimal(TxtCoiledTail.Text.Trim());


                if (TxtShortTail.Text == "" || TxtShortTail.Text == null)
                    varShort = 0;
                else
                    varShort = Convert.ToDecimal(TxtShortTail.Text.Trim());


                if (TxtHairPinTail.Text == "" || TxtHairPinTail.Text == null)
                    varHrPin = 0;
                else
                    varHrPin = Convert.ToDecimal(TxtHairPinTail.Text.Trim());


                if (TxtDoubleTail.Text == "" || TxtDoubleTail.Text == null)
                    varDoub = 0;
                else
                    varDoub = Convert.ToDecimal(TxtDoubleTail.Text.Trim());


                if (TxtOtherTail.Text == "" || TxtOtherTail.Text == null)
                    varOthe = 0;
                else
                    varOthe = Convert.ToDecimal(TxtOtherTail.Text.Trim());

                SumOfMidPieceTail = (Convert.ToDecimal(varCoil)) + (Convert.ToDecimal(varShort)) + (Convert.ToDecimal(varHrPin)) + (Convert.ToDecimal(varDoub)) + (Convert.ToDecimal(varOthe));

                TxtTotalTail.Text = SumOfMidPieceTail.ToString();
                //txtSpermConcentration.Text = SumOfGrade.ToString();

                //TxtGradeIV.Text = (100 - SumOfGrade).ToString();

            }
            catch (Exception ex)
            {
                //App.ErrorLog(ex);

            }
        }

        private void mainTotal_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal varHeadTotal = 0;
                decimal varMidPieceTotal = 0;
                decimal varMidPieceTailTotal = 0;


                if (TxtTotal.Text == "" || TxtTotal.Text == null)
                    varHeadTotal = 0;
                else
                    varHeadTotal = Convert.ToDecimal(TxtTotal.Text.Trim());


                if (TxtTotalMidPiece.Text == "" || TxtTotalMidPiece.Text == null)
                    varMidPieceTotal = 0;
                else
                    varMidPieceTotal = Convert.ToDecimal(TxtTotalMidPiece.Text.Trim());


                if (TxtTotalTail.Text == "" || TxtTotalTail.Text == null)
                    varMidPieceTailTotal = 0;
                else
                    varMidPieceTailTotal = Convert.ToDecimal(TxtTotalTail.Text.Trim());

                SumOfMorphologicalabnormiilities = (Convert.ToDecimal(varHeadTotal)) + (Convert.ToDecimal(varMidPieceTotal)) + (Convert.ToDecimal(varMidPieceTailTotal));

                TxtMorphologicalabnormiilities.Text = SumOfMorphologicalabnormiilities.ToString();
                //txtSpermConcentration.Text = SumOfGrade.ToString();

                TxtNormalMorphology.Text = (100 - SumOfMorphologicalabnormiilities).ToString();

                if (Convert.ToInt64(TxtMorphologicalabnormiilities.Text) < 4)
                    TxtMorphologicalabnormiilities.Foreground = new SolidColorBrush(Colors.Red);
                else
                    TxtMorphologicalabnormiilities.Foreground = new SolidColorBrush(Colors.Black);

                if (Convert.ToInt64(TxtNormalMorphology.Text) < 4)
                    TxtNormalMorphology.Foreground = new SolidColorBrush(Colors.Red);
                else
                    TxtNormalMorphology.Foreground = new SolidColorBrush(Colors.Black);


            }
            catch (Exception ex)
            {
                //App.ErrorLog(ex);

            }
        }

        private void TxtPusCells_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TxtPusCells_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt64(TxtPusCells.Text.Trim()) < 4)
                TxtPusCells.Foreground = new SolidColorBrush(Colors.Red);
            else
                TxtPusCells.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void TxtSpermCount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt64(TxtSpermCount.Text.Trim()) < 15)
                TxtSpermCount.Foreground = new SolidColorBrush(Colors.Red);
            else
                TxtSpermCount.Foreground = new SolidColorBrush(Colors.Black);

            if (TxtSpermCount.Text.Trim() != "" && TxtQuantity.Text.Trim() != "")
            {
                double value = (Convert.ToInt64(TxtSpermCount.Text)) * (Convert.ToDouble(TxtQuantity.Text));
                TxtTotCount.Text = value.ToString();
            }
        }

        private void TxtTotCount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt64(TxtTotCount.Text.Trim()) < 39)
                TxtTotCount.Foreground = new SolidColorBrush(Colors.Red);
            else
                TxtTotCount.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void TxtQuantity_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Convert.ToDouble(TxtQuantity.Text.Trim()) == 2)
                TxtQuantity.Foreground = new SolidColorBrush(Colors.Black);
            else
                TxtQuantity.Foreground = new SolidColorBrush(Colors.Red);

            if (TxtSpermCount.Text.Trim() != "" && TxtQuantity.Text.Trim() != "")
            {
                double value = (Convert.ToDouble(TxtSpermCount.Text)) * (Convert.ToDouble(TxtQuantity.Text));
                TxtTotCount.Text = value.ToString();
            }
        }

        private void TxtPH_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt64(TxtPH.Text.Trim()) >= 7.2)
                TxtPH.Foreground = new SolidColorBrush(Colors.Black);
            else
                TxtPH.Foreground = new SolidColorBrush(Colors.Red);
        }

        private void TxtLiquiTime_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TxtLiquiTime.Text.Trim() != "" && Convert.ToInt64(TxtLiquiTime.Text.Trim()) == 30)
                TxtLiquiTime.Foreground = new SolidColorBrush(Colors.Black);
            else
                TxtLiquiTime.Foreground = new SolidColorBrush(Colors.Red);
        }

        private void TxtRapidProgressive_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtRapidProgressive.Text.Trim() != "" && TxtSlowProgressive.Text.Trim() != "")
            {
                double value = (Convert.ToDouble(TxtRapidProgressive.Text)) + (Convert.ToDouble(TxtSlowProgressive.Text));
                TxtTotalAdvanceMotility.Text = value.ToString();
            }
        }



    }
}

