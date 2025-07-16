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
using System.Text.RegularExpressions;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class SemenWash_Dashboard : ChildWindow, IInitiateCIMS
    {
        bool IsPatientExist = true;
        string ModuleName;
        string Action;
        public int mode = 0;
        public int CallType = 0;
        public string ListTitle { get; set; }
        public void Initiate(string Mode)
        {
            //added by neena

            switch (Mode)
            {
                case "1":

                    TransactionTypeID = 1;
                    // SemenExamination_Dashboard frm = new SemenExamination_Dashboard(CoupleDetails.MalePatient.PatientID, CoupleDetails.MalePatient.PatientUnitID);
                    mode = 1;
                    first.Content = "List Of AIH Processing Details";
                    validation();
                    break;

                case "2":
                    TransactionTypeID = 2;
                    mode = 2;
                    first.Content = "List Of AIH Mg Bead Processing Details";
                    validation();
                    break;

                case "3":
                    TransactionTypeID = 3;
                    mode = 3;
                    first.Content = "List Of Cryo Frozen Sperm Processing Details";
                    validation();
                    break;

            }

        }

        private bool validation()
        {
            if (CallType == 0) //MENU CALL
            {

                for (int i = 0; i < 2; i++)
                {

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.IsSurrogate == true)
                        //Flagref = true;
                        //

                        if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                        {
                            // System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            IsPatientExist = false;
                            break;
                        }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        break;
                    }
                    if (((IApplicationConfiguration)App.Current).SelectedPatient.VisitID == 0)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Mark Visit.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        break;
                    }
                    if (((IApplicationConfiguration)App.Current).SelectedPatient.GenderID != 1)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Male Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        break;
                    }

                    //if (((IApplicationConfiguration)App.Current).SelectedPatient.IsClinicVisited != true)
                    //{
                    //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    //    new MessageBoxControl.MessageBoxChildWindow("Palash", "Current Visit is Closed", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //    msgW1.Show();
                    //    IsPatientExist = false;
                    //    break;
                    //}

                }
                if (IsPatientExist == true)
                {
                    PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
                    VisitID = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;

                    //Title = "Semen Preparation (Name:- " + CoupleDetails.MalePatient.FirstName +
                    //   " " + CoupleDetails.MalePatient.LastName + ")";
                    if (mode == 1)
                    {
                        Title = "AIH Processing (Name:- " + ((IApplicationConfiguration)App.Current).SelectedPatient.PatientName + ")";

                    }
                    else if (mode == 2)
                    {

                        Title = "AIH Mg Bead Processing (Name:- " + ((IApplicationConfiguration)App.Current).SelectedPatient.PatientName + ")";


                    }
                    else if (mode == 3)
                    {

                        Title = "Cryo Frozen Sperm Processing (Name:- " + ((IApplicationConfiguration)App.Current).SelectedPatient.PatientName + ")";

                    }

                }
            }
            return IsPatientExist;

        }
        #region Paging

        public PagedSortableCollectionView<cls_IVFDashboard_SemenWashVO> DataList { get; private set; }


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
        public int ClickedFlag = 0;
        public bool IsCancel = true, IsFromDashBoard = false;

        public Int64 UnitID = 0;
        public Int64 PatientID = 0;
        public Int64 PatientUnitID = 0;
        public Int64 VisitID = 0;

        private SwivelAnimation objAnimation;
        public long SelectedRecord;
        public clsCoupleVO CoupleDetails;
        public int TransactionTypeID;
        clsPatientGeneralVO SelectedPatient = ((IApplicationConfiguration)App.Current).SelectedPatient;
        public SemenWash_Dashboard()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            //Paging
            DataList = new PagedSortableCollectionView<cls_IVFDashboard_SemenWashVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;

            this.DataContext = null;

        }

        public SemenWash_Dashboard(long PatientID, long PatientUnitID)
        {
            InitializeComponent();

            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            //Paging
            DataList = new PagedSortableCollectionView<cls_IVFDashboard_SemenWashVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;

            this.DataContext = null;
            //(((IApplicationConfiguration)App.Current).SelectedPatient).PatientID = PatientID;
            //(((IApplicationConfiguration)App.Current).SelectedPatient).UnitId = PatientUnitID;

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
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
                    chkFreeze.IsEnabled = true;
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
        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void SemenWash_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                // FillReferralName();
                //FillMethodCollection();
                //FillAbstience();
                //FillColor();
                //FillEmbryologist();

                //FillViscousRange();
                //FillSemenRemarks();
                dtCollection.DisplayDateEnd = DateTime.Today;

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

                rbtFresh.IsChecked = true;
                rbtCentre.IsChecked = true;
                rbtNormal.IsChecked = true;
                rbtNotPresent.IsChecked = true;

                if ((((IApplicationConfiguration)App.Current).SelectedPatient).PatientID != 0 || (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID != null)
                {
                    SetCommandButtonState("Load");
                    FillMethodCollection();
                    // FetchData();                   
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Female Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                    msgW1.Show();
                }
                if (TransactionTypeID == 3)
                {
                    grpPrePostProcessingTest.Visibility = Visibility.Visible;
                    TxtSpermRecoverdFromTitle.Visibility = Visibility.Collapsed;
                    TxtSpermRecoverdFrom.Visibility = Visibility.Collapsed;

                    if (IsFromDashBoard == true)
                    {
                        Title = "Cryo Frozen Sperm Processing (Name:- " + CoupleDetails.MalePatient.FirstName + " " + CoupleDetails.MalePatient.LastName + ")";
                    }
                    else
                    {
                        Title = "Cryo Frozen Sperm Processing (Name:- " + ((IApplicationConfiguration)App.Current).SelectedPatient.PatientName + ")";
                    }
                }
                else if (TransactionTypeID == 2)
                {
                    grpPrePostProcessingTest.Visibility = Visibility.Collapsed;
                    TxtSpermRecoverdFromTitle.Visibility = Visibility.Visible;
                    TxtSpermRecoverdFrom.Visibility = Visibility.Visible;

                    if (IsFromDashBoard == true)
                    {
                        Title = "AIH Mg Bead Processing (Name:- " + CoupleDetails.MalePatient.FirstName +" " + CoupleDetails.MalePatient.LastName + ")";
                    }
                    else
                    {
                        Title = "AIH Mg Bead Processing (Name:- " + ((IApplicationConfiguration)App.Current).SelectedPatient.PatientName + ")";
                    }
                }
                else if (TransactionTypeID == 1)
                {
                    grpPrePostProcessingTest.Visibility = Visibility.Collapsed;
                    TxtSpermRecoverdFromTitle.Visibility = Visibility.Collapsed;
                    TxtSpermRecoverdFrom.Visibility = Visibility.Collapsed;

                    if (IsFromDashBoard == true)
                    {
                        Title = "AIH Processing (Name:- " + CoupleDetails.MalePatient.FirstName +" " + CoupleDetails.MalePatient.LastName + ")";
                    }
                    else
                    {
                        Title = "AIH Processing (Name:- " + ((IApplicationConfiguration)App.Current).SelectedPatient.PatientName + ")";
                    }
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

            //cls_GetIVFDashboard_SemenBizActionVO BizAction = new cls_GetIVFDashboard_SemenBizActionVO();
            cls_GetIVFDashboard_SemenWashBizActionVO BizAction = new cls_GetIVFDashboard_SemenWashBizActionVO();

            BizAction.List = new List<cls_IVFDashboard_SemenWashVO>();
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

                    if (((cls_GetIVFDashboard_SemenWashBizActionVO)args.Result).List != null)
                    {
                        cls_GetIVFDashboard_SemenWashBizActionVO result = args.Result as cls_GetIVFDashboard_SemenWashBizActionVO;

                        DataList.TotalItemCount = result.TotalRows;
                        if (result.List != null)
                        {
                            DataList.Clear();

                            foreach (var item in result.List)
                            {
                                if (item.TransacationTypeID == TransactionTypeID)
                                {
                                    DataList.Add(item);

                                }
                            }


                            dgSemenWash.ItemsSource = null;
                            dgSemenWash.ItemsSource = DataList;
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
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

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
                    //cmbCollection.SelectedValue = ((cls_IVFDashboard_SemenVO)this.DataContext).MethodOfCollectionID;
                }
                FillAbstience();
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
                    //cmbAbstience.SelectedValue = ((cls_IVFDashboard_SemenVO)this.DataContext).AbstinenceID;
                }
                FillColor();
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
                    // cmbColor.SelectedValue = ((cls_IVFDashboard_SemenVO)this.DataContext).ColorID;
                }
                FillEmbryologist();
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillEmbryologist()
        {
            //wait.Show();

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

                    //cmbCheckedBy.ItemsSource = null;
                    //cmbCheckedBy.ItemsSource = objList;
                    //cmbCheckedBy.SelectedItem = objList[0];

                    cmbAndrologist.ItemsSource = null;
                    cmbAndrologist.ItemsSource = objList;
                    cmbAndrologist.SelectedItem = objList[0];

                }
                FillViscousRange();
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
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
                    //cmbViscousRange.SelectedValue = ((cls_IVFDashboard_SemenVO)this.DataContext).RangeViscosityID;
                }
                FillSemenRemarks();
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
                    //cmbInterpretationsRemark.SelectedValue = ((cls_IVFDashboard_SemenVO)this.DataContext).InterpretationsID;
                }
                FillTypeSperm();
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        //added by neena
        private void FillTypeSperm()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_IVF_TypeOfSperm;  // M_Abstinence;
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
                        cmbSpermType.ItemsSource = null;
                        cmbSpermType.ItemsSource = results.ToList();
                        cmbSpermType.SelectedItem = results.ToList()[0];
                    }
                    else
                    {
                        //////////////NEW CODE BY YK 301116////////////
                        var results = from r in objList
                                      where r.ID != 4 && r.ID != 7 && r.ID != 9
                                      select r;

                        cmbSpermType.ItemsSource = null;
                        cmbSpermType.ItemsSource = results.ToList();
                        cmbSpermType.SelectedItem = results.ToList()[0];
                        //////////////END////////////////////////////


                        /////////OLD CODE/////////////////

                        //cmbSpermType.ItemsSource = null;
                        //cmbSpermType.ItemsSource = objList;
                        //cmbSpermType.SelectedItem = objList[0];

                        ///////////END////////////
                    }
                }

                if (this.DataContext != null)
                {
                    //cmbSpermType.SelectedValue = ((cls_IVFDashboard_SemenWashVO)this.DataContext).SpermTypeID;
                }
                // commented by devidas 9/5/2017 For Andrology Flow
                FillMOSP();
                // FetchData();// added by devidas 9/5/2017 For Andrology Flow
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        //

        private void FillMOSP()     // added by devidas 9/5/2017 For Andrology Flow
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_MethodOfSpermPreparationMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbSemenProcess.ItemsSource = null;
                    cmbSemenProcess.ItemsSource = objList;
                    cmbSemenProcess.SelectedItem = objList[0];

                    //if (Details.MethodOfSpermPreparation != null && Details.MethodOfSpermPreparation != 0)
                    //{
                    //    cmbMethodOfSpermpreparation.SelectedValue = Details.MethodOfSpermPreparation;
                    //}

                }
                //fillNeedleSource();
                FillBloodGroup();   // added by devidas 9/5/2017 For Andrology Flow


            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillBloodGroup()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_BloodGroupMaster;
                BizAction.MasterList = new List<MasterListItem>();
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

                        cmbBloodGroop.ItemsSource = null;
                        cmbBloodGroop.ItemsSource = objList.DeepCopy();

                        cmbBloodGroop.ItemsSource = null;
                        cmbBloodGroop.ItemsSource = objList.DeepCopy();
                    }

                    if (this.DataContext != null)
                    {
                        //cmbBloodGroop.SelectedValue = ((clsPatientVO)this.DataContext).BloodGroupID;
                        //cmbBloodGroop.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.BloodGroupID;

                        cmbBloodGroop.SelectedValue = ((cls_IVFDashboard_SemenWashVO)this.DataContext).BloodGroupID;
                        //cmbBloodGroop.SelectedValue = ((cls_IVFDashboard_SemenWashVO)this.DataContext).SpouseDetails.BloodGroupID;
                        
                    }
                    //FillGender();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

                FillResultMaster();  // added by devidas 11/5/2017 For Andrology Flow

            }
            catch (Exception ex)
            {
                //  wait.Close();
                throw;
            }
        }

        void FillResultMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ResultTypeMaster;
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

                        var results = from r in objList
                                      where r.ID == 0 || r.ID == 5 || r.ID == 4
                                      select r;
                        // objList.Clear();
                        objList = results.ToList();

                        //cmbPatientType.ItemsSource = null;
                        //cmbPatientType.ItemsSource = results.ToList();
                        //cmbPatientType.SelectedItem = results.ToList()[0];

                        cmbHIV.ItemsSource = null;
                        cmbHIV.ItemsSource = objList.DeepCopy();
                        cmbHIV.SelectedItem = results.ToList()[0];

                        cmbHCV.ItemsSource = null;
                        cmbHCV.ItemsSource = objList.DeepCopy();
                        cmbHCV.SelectedItem = results.ToList()[0];

                        cmbVDRL.ItemsSource = null;
                        cmbVDRL.ItemsSource = objList.DeepCopy();
                        cmbVDRL.SelectedItem = results.ToList()[0];

                        cmbHBSAG.ItemsSource = null;
                        cmbHBSAG.ItemsSource = objList.DeepCopy();
                        cmbHBSAG.SelectedItem = results.ToList()[0];
                    }

                    FetchData(); // // added by devidas 11/5/2017 For Andrology Flow

                    //if (this.DataContext != null)
                    //{
                    //    cmbResultType.SelectedValue = ((clsPathoResultEntryVO)this.DataContext).ResultType;
                    //}
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


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
                if (TxtSpillage.Text != string.Empty && TxtSpillage.Text.Trim().Length > 0)
                {
                    // string pattern = @"([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$"; //To Check PAN NO Regular Expression Date 21/9/2016
                    var regexItem = new Regex("^[a-zA-Z0-9 ]*$");

                    if (!regexItem.IsMatch(TxtSpillage.Text))
                    {
                        TxtSpillage.SetValidation("Not Allowed Special Character ");
                        TxtSpillage.RaiseValidationError();
                        TxtSpillage.Focus();
                        //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                        result = false;
                    }
                    else
                        TxtSpillage.ClearValidationError();
                }
                if (TxtMediaUsed.Text != string.Empty && TxtMediaUsed.Text.Trim().Length > 0)
                {
                    var regexItem = new Regex("^[a-zA-Z0-9 ]*$");

                    if (!regexItem.IsMatch(TxtMediaUsed.Text))
                    {
                        TxtMediaUsed.SetValidation("Not Allowed Special Character ");
                        TxtMediaUsed.RaiseValidationError();
                        TxtMediaUsed.Focus();
                        //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                        result = false;
                    }
                    else
                        TxtMediaUsed.ClearValidationError();
                }
                //if (TxtMediaUsed.Text != string.Empty && TxtMediaUsed.Text.Trim().Length > 0)
                //{
                //    string pattern = @"([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$"; //To Check PAN NO Regular Expression Date 21/9/2016
                //    Match m = Regex.Match(TxtMediaUsed.Text.Trim(), pattern);
                //    if (!m.Success)
                //    {
                //        TxtMediaUsed.SetValidation("Not Allowed Special Character ");
                //        TxtMediaUsed.RaiseValidationError();
                //        TxtMediaUsed.Focus();
                //        //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //        result = false;
                //    }
                //    else
                //        TxtSpillage.ClearValidationError();

                //}

                if (TxtQuantity.Text == null || Convert.ToDouble(TxtQuantity.Text.Trim()) == 0)
                {
                    TxtQuantity.SetValidation("Volume  Is Required");
                    TxtQuantity.RaiseValidationError();
                    TxtQuantity.Focus();
                    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else if (Convert.ToDouble(TxtQuantity.Text) > 10)
                {
                    TxtQuantity.SetValidation("Volume Should Be less than 10 ");
                    TxtQuantity.RaiseValidationError();
                    TxtQuantity.Focus();
                    // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    TxtQuantity.ClearValidationError();


                if (TxtPreSpermCount.Text == null || Convert.ToInt64(TxtPreSpermCount.Text.Trim()) == 0)
                {
                    TxtPreSpermCount.SetValidation(" Pre Sperm Concentration Is Required");
                    TxtPreSpermCount.RaiseValidationError();
                    TxtPreSpermCount.Focus();
                    result = false;
                }
                else
                    TxtPreSpermCount.ClearValidationError();


                if (TxtPostSpermCount.Text == null || Convert.ToInt64(TxtPostSpermCount.Text.Trim()) == 0)
                {

                    TxtPostSpermCount.SetValidation(" Post Sperm Concentration Is Required");
                    TxtPostSpermCount.RaiseValidationError();
                    TxtPostSpermCount.Focus();
                    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    TxtPostSpermCount.ClearValidationError();


                //if (TxtPreTotCount.Text == null || Convert.ToInt64(TxtPreTotCount.Text.Trim()) == 0)
                //{
                //    TxtPreTotCount.SetValidation("Pre Total Ejaculated Sperm Count Is Required");
                //    TxtPreTotCount.RaiseValidationError();
                //    TxtPreTotCount.Focus();
                //    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    TxtPreTotCount.ClearValidationError();


                //if (TxtPostTotCount.Text == null || Convert.ToInt64(TxtPostTotCount.Text.Trim()) == 0)
                //{
                //    TxtPostTotCount.SetValidation("Post Total Ejaculated Sperm Count Is Required");
                //    TxtPostTotCount.RaiseValidationError();
                //    TxtPostTotCount.Focus();
                //    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    TxtPostTotCount.ClearValidationError();


                if (TxtPreGradeI.Text == null || Convert.ToInt64(TxtPreGradeI.Text.Trim()) == 0)
                {
                    TxtPreGradeI.SetValidation("Pre Grade Progressive Is Required");
                    TxtPreGradeI.RaiseValidationError();
                    TxtPreGradeI.Focus();
                    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    TxtPreGradeI.ClearValidationError();



                if (TxtPostGradeI.Text == null || Convert.ToInt64(TxtPostGradeI.Text.Trim()) == 0)
                {
                    TxtPostGradeI.SetValidation("Post Grade Progressive Is Required");
                    TxtPostGradeI.RaiseValidationError();
                    TxtPostGradeI.Focus();
                    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    TxtPostGradeI.ClearValidationError();



                if (TxtPostGradeII.Text == null || Convert.ToInt64(TxtPostGradeII.Text.Trim()) == 0)
                {
                    TxtPostGradeII.SetValidation(" Post Grade NonProgressive Is Required");
                    TxtPostGradeII.RaiseValidationError();
                    TxtPostGradeII.Focus();
                    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    TxtPostGradeII.ClearValidationError();


                if (TxtPreGradeII.Text == null || Convert.ToInt64(TxtPreGradeII.Text.Trim()) == 0)
                {
                    TxtPreGradeII.SetValidation(" Pre Grade NonProgressive Is Required");
                    TxtPreGradeII.RaiseValidationError();
                    TxtPreGradeII.Focus();
                    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    TxtPreGradeII.ClearValidationError();

                //  Check for the ASCII for the Numeric

                //if ((MasterListItem)cmbCollection.SelectedItem == null)
                //{
                //    cmbCollection.TextBox.SetValidation("Collection Method Is Required");
                //    cmbCollection.TextBox.RaiseValidationError();
                //    cmbCollection.Focus();
                //   // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else if (((MasterListItem)cmbCollection.SelectedItem).ID == 0)
                //{
                //    cmbCollection.TextBox.SetValidation("Collection Method Is Required");
                //    cmbCollection.TextBox.RaiseValidationError();
                //    cmbCollection.Focus();
                //   // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    cmbCollection.TextBox.ClearValidationError();

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

                if (cmbAndrologist.SelectedItem == null || ((MasterListItem)cmbAndrologist.SelectedItem).ID == 0)
                {
                    cmbAndrologist.TextBox.SetValidation("Please Select Andrologist");
                    cmbAndrologist.TextBox.RaiseValidationError();
                    cmbAndrologist.Focus();
                    result = false;
                }
                else
                    cmbAndrologist.TextBox.ClearValidationError();

            }
            catch (Exception)
            {
                // throw;
            }
            return result;
        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;

            if (ClickedFlag == 1)
            {
                bool saveDtls = true;

                saveDtls = CheckValidations();

                if (saveDtls == true)
                {
                    string msgText = "";
                    msgText = "Are You Sure \n You Want To Save The Details?";
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
                SaveSemenWash();
            else
                ClickedFlag = 0;

        }


        private void SaveSemenWash()
        {
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();


            cls_IVFDashboard_AddUpdateSemenWashBizActionVO BizAction = new cls_IVFDashboard_AddUpdateSemenWashBizActionVO();

            //BizAction.SemensWashDetails = (cls_IVFDashboard_SemenVO)this.DataContext;

            BizAction.SemensWashDetails.ID = ((cls_IVFDashboard_SemenWashVO)this.DataContext).ID;

            BizAction.SemensWashDetails.UnitID = UnitID;
            BizAction.SemensWashDetails.PatientID = PatientID;
            BizAction.SemensWashDetails.PatientUnitID = PatientUnitID;
            BizAction.SemensWashDetails.VisitID = VisitID;

            if (cmbSpermType.SelectedItem != null)
                BizAction.SemensWashDetails.SpermTypeID = ((MasterListItem)cmbSpermType.SelectedItem).ID;

            BizAction.SemensWashDetails.SampleCode = TxtSampleCode.Text.Trim();

            if (TxtSampleCode.Tag == "")
            {
                BizAction.SemensWashDetails.SampleLinkID = "";
            }
            else
            {

                BizAction.SemensWashDetails.SampleLinkID = TxtSampleCode.Tag.ToString();
            }

            if (dtCollection.SelectedDate.Value.Date != null)
                BizAction.SemensWashDetails.CollectionDate = dtCollection.SelectedDate.Value.Date;

            if (CollectionTime.Value != null)
                BizAction.SemensWashDetails.CollectionDate = BizAction.SemensWashDetails.CollectionDate.Value.Add(CollectionTime.Value.Value.TimeOfDay);

            if (cmbCollection.SelectedItem != null)
                BizAction.SemensWashDetails.MethodOfCollectionID = ((MasterListItem)cmbCollection.SelectedItem).ID;

            if (cmbAbstience.SelectedItem != null)
                BizAction.SemensWashDetails.AbstinenceID = ((MasterListItem)cmbAbstience.SelectedItem).ID;

            BizAction.SemensWashDetails.ISFromIUI = false;

            if (SampRecTime.Value != null)
                BizAction.SemensWashDetails.TimeRecSampLab = SampRecTime.Value;

            if (rbtFresh.IsChecked == true)
                BizAction.SemensWashDetails.Complete = true;
            else if (rbtISFrozen.IsChecked == true)
                BizAction.SemensWashDetails.Complete = false;

            if (rbtCentre.IsChecked == true)
                BizAction.SemensWashDetails.CollecteAtCentre = true;
            else if (rbtOutSideCentre.IsChecked == true)
                BizAction.SemensWashDetails.CollecteAtCentre = false;

            if (cmbColor.SelectedItem != null)
                BizAction.SemensWashDetails.ColorID = ((MasterListItem)cmbColor.SelectedItem).ID;

            if (!string.IsNullOrEmpty(TxtQuantity.Text.Trim()))
                BizAction.SemensWashDetails.Quantity = float.Parse(TxtQuantity.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPH.Text.Trim()))
                BizAction.SemensWashDetails.PH = float.Parse(TxtPH.Text.Trim());

            BizAction.SemensWashDetails.LiquificationTime = TxtLiquiTime.Text;

            if (rbtNormal.IsChecked == true)
            {
                BizAction.SemensWashDetails.Viscosity = true;
                cmbViscousRange.Visibility = Visibility.Collapsed;
            }
            else if (rbtViscous.IsChecked == true)
            {
                BizAction.SemensWashDetails.Viscosity = false;
                cmbViscousRange.Visibility = Visibility.Visible;
            }

            if (cmbViscousRange.SelectedItem != null)
                BizAction.SemensWashDetails.RangeViscosityID = ((MasterListItem)cmbViscousRange.SelectedItem).ID;

            if (rbtPresent.IsChecked == true)
                BizAction.SemensWashDetails.Odour = true;
            else if (rbtNotPresent.IsChecked == true)
                BizAction.SemensWashDetails.Odour = false;

            if (!string.IsNullOrEmpty(TxtPreSpermCount.Text.Trim()))
                BizAction.SemensWashDetails.PreSpermCount = float.Parse(TxtPreSpermCount.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPreTotCount.Text.Trim()))
                BizAction.SemensWashDetails.PreTotalSpermCount = float.Parse(TxtPreTotCount.Text.Trim());

            //if (!string.IsNullOrEmpty(TxtPreMotility.Text.Trim())) TxtPreSpermCount
            //    BizAction.SemensWashDetails.PreMotility = float.Parse(TxtPreMotility.Text.Trim());

            //if (!string.IsNullOrEmpty(TxtPreNonMotility.Text.Trim()))
            //    BizAction.SemensWashDetails.PreNonMotility = float.Parse(TxtPreNonMotility.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPreTotalMotility.Text.Trim()))
                BizAction.SemensWashDetails.PreTotalMotility = float.Parse(TxtPreTotalMotility.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPreGradeI.Text.Trim()))
                BizAction.SemensWashDetails.PreMotilityGradeI = float.Parse(TxtPreGradeI.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPreGradeII.Text.Trim()))
                BizAction.SemensWashDetails.PreMotilityGradeII = float.Parse(TxtPreGradeII.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPreGradeIII.Text.Trim()))
                BizAction.SemensWashDetails.PreMotilityGradeIII = float.Parse(TxtPreGradeIII.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPreGradeIV.Text.Trim()))
                BizAction.SemensWashDetails.PreMotilityGradeIV = float.Parse(TxtPreGradeIV.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPreNormalMorphology.Text.Trim()))
                BizAction.SemensWashDetails.PreNormalMorphology = float.Parse(TxtPreNormalMorphology.Text.Trim());



            //
            if (!string.IsNullOrEmpty(TxtPostSpermCount.Text.Trim()))
                BizAction.SemensWashDetails.PostSpermCount = float.Parse(TxtPostSpermCount.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPostTotCount.Text.Trim()))
                BizAction.SemensWashDetails.PostTotalSpermCount = float.Parse(TxtPostTotCount.Text.Trim());

            //if (!string.IsNullOrEmpty(TxtPostMotility.Text.Trim()))
            //    BizAction.SemensWashDetails.PostMotility = float.Parse(TxtPostMotility.Text.Trim());

            //if (!string.IsNullOrEmpty(TxtPostNonMotility.Text.Trim()))
            //    BizAction.SemensWashDetails.PostNonMotility = float.Parse(TxtPostNonMotility.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPostTotalMotility.Text.Trim()))
                BizAction.SemensWashDetails.PostTotalMotility = float.Parse(TxtPostTotalMotility.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPostGradeI.Text.Trim()))
                BizAction.SemensWashDetails.PostMotilityGradeI = float.Parse(TxtPostGradeI.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPostGradeII.Text.Trim()))
                BizAction.SemensWashDetails.PostMotilityGradeII = float.Parse(TxtPostGradeII.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPostGradeIII.Text.Trim()))
                BizAction.SemensWashDetails.PostMotilityGradeIII = float.Parse(TxtPostGradeIII.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPostGradeIV.Text.Trim()))
                BizAction.SemensWashDetails.PostMotilityGradeIV = float.Parse(TxtPostGradeIV.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPostNormalMorphology.Text.Trim()))
                BizAction.SemensWashDetails.PostNormalMorphology = float.Parse(TxtPostNormalMorphology.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPusCells.Text.Trim()))
                BizAction.SemensWashDetails.PusCells = TxtPusCells.Text.Trim();

            if (!string.IsNullOrEmpty(TxtRoundCells.Text.Trim()))
                BizAction.SemensWashDetails.RoundCells = TxtRoundCells.Text.Trim();

            if (!string.IsNullOrEmpty(TxtEpithelialCells.Text.Trim()))
                BizAction.SemensWashDetails.EpithelialCells = TxtEpithelialCells.Text.Trim();

            if (!string.IsNullOrEmpty(TxtOtherCells.Text.Trim()))
                BizAction.SemensWashDetails.AnyOtherCells = TxtOtherCells.Text.Trim();


            if (cmbAndrologist.SelectedItem != null)
                BizAction.SemensWashDetails.CheckedByDoctorID = ((MasterListItem)cmbAndrologist.SelectedItem).ID;

            //if (!string.IsNullOrEmpty(TxtRemark.Text.Trim()))
            //    BizAction.SemensWashDetails.Comment = TxtRemark.Text.Trim();

            if (cmbInterpretationsRemark.SelectedItem != null)
                BizAction.SemensWashDetails.CommentID = ((MasterListItem)cmbInterpretationsRemark.SelectedItem).ID;

            //if (!string.IsNullOrEmpty(TxtPyriform.Text.Trim()))
            //    BizAction.SemensWashDetails.Pyriform = float.Parse(TxtPyriform.Text.Trim());           

            //if (!string.IsNullOrEmpty(TxtPyriform.Text.Trim()))
            //    BizAction.SemensWashDetails.Pyriform = float.Parse(TxtPyriform.Text.Trim());

            // add by devidas 9/may/2017
            if (cmbSemenProcess.SelectedItem != null)
                BizAction.SemensWashDetails.SemenProcessingMethodID = ((MasterListItem)cmbSemenProcess.SelectedItem).ID;
            if (!string.IsNullOrEmpty(TxtMediaUsed.Text.Trim()))
                BizAction.SemensWashDetails.MediaUsed = TxtMediaUsed.Text;
            if (!string.IsNullOrEmpty(TxtSpillage.Text.Trim()))
            {
                BizAction.SemensWashDetails.Spillage = TxtSpillage.Text;
            }
            BizAction.SemensWashDetails.HBSAG = ((MasterListItem)cmbHBSAG.SelectedItem).ID; ;

            BizAction.SemensWashDetails.HIV = ((MasterListItem)cmbHIV.SelectedItem).ID; ;
            BizAction.SemensWashDetails.VDRL = ((MasterListItem)cmbVDRL.SelectedItem).ID; ;
            BizAction.SemensWashDetails.HCV = ((MasterListItem)cmbHCV.SelectedItem).ID;

            if (!string.IsNullOrEmpty(TxtSpermRecoverdFrom.Text.Trim()))
                BizAction.SemensWashDetails.SpermRecoveredFrom = TxtSpermRecoverdFrom.Text;

            if (cmbBloodGroop.SelectedItem != null)
                BizAction.SemensWashDetails.BloodGroupID = ((MasterListItem)cmbBloodGroop.SelectedItem).ID;
            BizAction.SemensWashDetails.TransacationTypeID = TransactionTypeID;

            if (chkFreeze != null)
                BizAction.SemensWashDetails.IsFreezed = Convert.ToBoolean(chkFreeze.IsChecked);

            //added by neena
            if (!string.IsNullOrEmpty(TxtComments.Text.Trim()))
                BizAction.SemensWashDetails.AllComments = TxtComments.Text.Trim();
            if (!string.IsNullOrEmpty(TxtSperm5thPercentile.Text.Trim()))
                BizAction.SemensWashDetails.Sperm5thPercentile = float.Parse(TxtSperm5thPercentile.Text.Trim());
            if (!string.IsNullOrEmpty(TxtSperm75thPercentile.Text.Trim()))
                BizAction.SemensWashDetails.Sperm75thPercentile = float.Parse(TxtSperm75thPercentile.Text.Trim());
            if (!string.IsNullOrEmpty(TxtEjaculate5thPercentile.Text.Trim()))
                BizAction.SemensWashDetails.Ejaculate5thPercentile = float.Parse(TxtEjaculate5thPercentile.Text.Trim());
            if (!string.IsNullOrEmpty(TxtEjaculate75thPercentile.Text.Trim()))
                BizAction.SemensWashDetails.Ejaculate75thPercentile = float.Parse(TxtEjaculate75thPercentile.Text.Trim());
            if (!string.IsNullOrEmpty(TxtTotalMotility5thPercentile.Text.Trim()))
                BizAction.SemensWashDetails.TotalMotility5thPercentile = float.Parse(TxtTotalMotility5thPercentile.Text.Trim());
            if (!string.IsNullOrEmpty(TxtTotalMotility75thPercentile.Text.Trim()))
                BizAction.SemensWashDetails.TotalMotility75thPercentile = float.Parse(TxtTotalMotility75thPercentile.Text.Trim());
            //

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
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding Details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        bool IsView = false;
        private void hlbViewExamination_Click(object sender, RoutedEventArgs e)
        {
            if (dgSemenWash.SelectedItem != null)
            {
                SetCommandButtonState("Modify");
                ClickedFlag = 0;
                ClearAll();
                IsView = true;
                this.DataContext = new cls_IVFDashboard_SemenWashVO();
                this.DataContext = (cls_IVFDashboard_SemenWashVO)dgSemenWash.SelectedItem;
                SelectedRecord = ((cls_IVFDashboard_SemenWashVO)dgSemenWash.SelectedItem).ID;

                grpContactDetails.DataContext = (cls_IVFDashboard_SemenWashVO)this.DataContext;
                grpPhysicalExam.DataContext = (cls_IVFDashboard_SemenWashVO)this.DataContext;
                // grpPreWash.DataContext = (cls_IVFDashboard_SemenWashVO)this.DataContext;
                //  grpPostWash.DataContext = (cls_IVFDashboard_SemenWashVO)this.DataContext;
                grpCellInfo.DataContext = (cls_IVFDashboard_SemenWashVO)this.DataContext;
                grpInterpretations.DataContext = (cls_IVFDashboard_SemenWashVO)this.DataContext;
                grpGeneralSpermInformation.DataContext = (cls_IVFDashboard_SemenWashVO)this.DataContext;
                grpProcessingDet.DataContext = (cls_IVFDashboard_SemenWashVO)this.DataContext;
                grpPrePostProcessingTest.DataContext = (cls_IVFDashboard_SemenWashVO)this.DataContext;
                grpPrePostProcessingTerms.DataContext = (cls_IVFDashboard_SemenWashVO)this.DataContext;
                if (((cls_IVFDashboard_SemenWashVO)this.DataContext).IsFrozenSample == true)
                {
                    rbtFresh.IsChecked = true;
                }
                else
                {
                    rbtISFrozen.IsChecked = true;
                }

                if (((cls_IVFDashboard_SemenWashVO)this.DataContext).CollecteAtCentre == true)
                {
                    rbtCentre.IsChecked = true;
                }
                else
                {
                    rbtOutSideCentre.IsChecked = true;
                }

                if (((cls_IVFDashboard_SemenWashVO)this.DataContext).Viscosity == true)
                {
                    rbtNormal.IsChecked = true;
                }
                else
                {
                    rbtViscous.IsChecked = true;
                }
                if (((cls_IVFDashboard_SemenWashVO)this.DataContext).Odour == true)
                {
                    rbtPresent.IsChecked = true;
                }
                else
                {
                    rbtNotPresent.IsChecked = true;
                }

                if(((cls_IVFDashboard_SemenWashVO)this.DataContext).IsFreezed==true)
                {
                    CmdSave.IsEnabled = false;
                    chkFreeze.IsEnabled = false;
                }
                else
                {
                    CmdSave.IsEnabled = true;
                    chkFreeze.IsEnabled = true;
                }

                objAnimation.Invoke(RotationType.Forward);

                CmdSave.Content = "Modify";

                IsModify = true;
            }

        }

        private void ClearAll()
        {
            this.BackPanel.DataContext = new cls_IVFDashboard_SemenWashVO();

            SampRecTime.Value = null;
            this.DataContext = new cls_IVFDashboard_SemenWashVO();
            grpContactDetails.DataContext = null;
            grpPhysicalExam.DataContext = null;
            // grpPreWash.DataContext = null;
            //grpPostWash.DataContext = null;
            grpCellInfo.DataContext = null;
            grpInterpretations.DataContext = null;
            TxtQuantity.Text = "0";

            dtCollection.SelectedDate = null;
            cmbCollection.SelectedValue = (long)0;
            cmbAbstience.SelectedValue = (long)0;
            cmbColor.SelectedValue = (long)0;
            cmbAndrologist.SelectedValue = (long)0;

            SampRecTime.Value = null;
            CollectionTime.Value = null;
            //rbtYes.IsChecked
            //rbtNo.IsChecked
            //cmbColor.SelectedItem

            rbtFresh.IsChecked = true;
            rbtCentre.IsChecked = true;
            rbtNormal.IsChecked = true;
            rbtNotPresent.IsChecked = true;

            TxtPH.Text = "0";
            TxtLiquiTime.Text = "";
            //rbtNormal.IsChecked
            //rbtViscous.IsChecked

            //Pre
            TxtPreSpermCount.Text = "0";
            TxtPreTotCount.Text = "0";
            TxtPreTotalMotility.Text = "0";
            TxtPreGradeI.Text = "0";
            TxtPreGradeII.Text = "0";
            TxtPreGradeIII.Text = "0";
            TxtPreGradeIV.Text = "0";
            TxtPreNormalMorphology.Text = "0";
            //TxtPreMotility.Text = "";
            //TxtPreNonMotility.Text = "";            
            //Post
            TxtPostSpermCount.Text = "0";
            TxtPostTotCount.Text = "0";
            TxtPostTotalMotility.Text = "0";
            TxtPostGradeI.Text = "0";
            TxtPostGradeII.Text = "0";
            TxtPostGradeIII.Text = "0";
            TxtPostGradeIV.Text = "0";
            TxtPostNormalMorphology.Text = "0";

            TxtPusCells.Text = "";
            TxtRoundCells.Text = "";
            TxtEpithelialCells.Text = "";
            TxtOtherCells.Text = "";
            //TxtRemark.Text = "";         
            cmbInterpretationsRemark.SelectedValue = (long)0;
            cmbSpermType.SelectedValue = (long)0;
            TxtSampleCode.Text = "";
            TxtSpillage.Text = "";
            TxtMediaUsed.Text = "";
            TxtSpermRecoverdFrom.Text = "";
            cmbBloodGroop.SelectedValue = (long)0;
            cmbHCV.SelectedValue = (long)0;
            cmbHBSAG.SelectedValue = (long)0;
            cmbHIV.SelectedValue = (long)0;
            cmbVDRL.SelectedValue = (long)0;
            cmbSemenProcess.SelectedValue = (long)0;
            chkFreeze.IsChecked = false;
        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient.IsClinicVisited == true)
            {

                this.SetCommandButtonState("New");
                SelectedRecord = 0;
                // ClearFormData();
                //  clsPatientGeneralVO FemalePatientDetails = new clsPatientGeneralVO();
                //  FemalePatientDetails = (clsPatientGeneralVO)Female.DataContext;

                cls_IVFDashboard_SemenWashVO obj = new cls_IVFDashboard_SemenWashVO();
                this.DataContext = obj;     // new clsGeneralExaminationVO();

                IsModify = false;
                ClickedFlag = 0;
                ClearAll();

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

                CmdSave.Content = "Save";
                IsModify = false;
            }
            else if (IsFromDashBoard == true)
            {
                this.SetCommandButtonState("New");
                SelectedRecord = 0;
                // ClearFormData();
                //  clsPatientGeneralVO FemalePatientDetails = new clsPatientGeneralVO();
                //  FemalePatientDetails = (clsPatientGeneralVO)Female.DataContext;

                cls_IVFDashboard_SemenWashVO obj = new cls_IVFDashboard_SemenWashVO();
                this.DataContext = obj;     // new clsGeneralExaminationVO();

                IsModify = false;
                ClickedFlag = 0;
                ClearAll();

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

                CmdSave.Content = "Save";
                IsModify = false;
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                new MessageBoxControl.MessageBoxChildWindow("Palash", "Current Visit is Closed", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }

        }

        decimal PreSumOfGrade = 0;
        private void Total_TextChanged(object sender, TextChangedEventArgs e)
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

            try
            {
                //TextBox txtbox = (TextBox)sender;
                //App.GenericMethod.IsValidEntry(txtbox, ValidationType.IsDecimalDigitsOnly);

                decimal varpreRapid = 0;
                decimal varpreSlow = 0;
                decimal varpreNon = 0;


                if (TxtPreGradeI.Text == "" || TxtPreGradeI.Text == null)
                    varpreRapid = 0;
                else
                    varpreRapid = Convert.ToDecimal(TxtPreGradeI.Text.Trim());


                if (TxtPreGradeII.Text == "" || TxtPreGradeII.Text == null)
                    varpreSlow = 0;
                else
                    varpreSlow = Convert.ToDecimal(TxtPreGradeII.Text.Trim());

                if (TxtPreGradeIII.Text == "" || TxtPreGradeIII.Text == null)
                    varpreNon = 0;
                else
                    varpreNon = Convert.ToDecimal(TxtPreGradeIII.Text.Trim());

                PreSumOfGrade = (Convert.ToDecimal(varpreRapid)) + (Convert.ToDecimal(varpreSlow)) + (Convert.ToDecimal(varpreNon));

                TxtPreTotalMotility.Text = PreSumOfGrade.ToString();
                //txtSpermConcentration.Text = SumOfGrade.ToString();

                TxtPreGradeIV.Text = (100 - PreSumOfGrade).ToString();


            }
            catch (Exception ex)
            {
                //App.ErrorLog(ex);

            }
        }

        decimal PostSumOfGrade = 0;

        private void PostTotal_TextChanged(object sender, TextChangedEventArgs e)
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

            try
            {
                //TextBox txtbox = (TextBox)sender;
                //App.GenericMethod.IsValidEntry(txtbox, ValidationType.IsDecimalDigitsOnly);

                decimal varpreRapid = 0;
                decimal varpreSlow = 0;
                decimal varpreNon = 0;


                if (TxtPostGradeI.Text == "" || TxtPostGradeI.Text == null)
                    varpreRapid = 0;
                else
                    varpreRapid = Convert.ToDecimal(TxtPostGradeI.Text.Trim());


                if (TxtPostGradeII.Text == "" || TxtPostGradeII.Text == null)
                    varpreSlow = 0;
                else
                    varpreSlow = Convert.ToDecimal(TxtPostGradeII.Text.Trim());

                if (TxtPostGradeIII.Text == "" || TxtPostGradeIII.Text == null)
                    varpreNon = 0;
                else
                    varpreNon = Convert.ToDecimal(TxtPostGradeIII.Text.Trim());

                PostSumOfGrade = (Convert.ToDecimal(varpreRapid)) + (Convert.ToDecimal(varpreSlow)) + (Convert.ToDecimal(varpreNon));

                TxtPostTotalMotility.Text = PostSumOfGrade.ToString();
                //txtSpermConcentration.Text = SumOfGrade.ToString();

                TxtPostGradeIV.Text = (100 - PostSumOfGrade).ToString();
            }
            catch (Exception ex)
            {
                //App.ErrorLog(ex);

            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            long ID, UnitID;
            if (dgSemenWash.SelectedItem != null)
            {
                ID = ((cls_IVFDashboard_SemenWashVO)dgSemenWash.SelectedItem).ID;
                UnitID = ((cls_IVFDashboard_SemenWashVO)dgSemenWash.SelectedItem).UnitID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/IVFDashboard/SemenWashAndIUI.aspx?ID=" + ID + "&UnitID=" + UnitID + "&IsFromIUI=" + false + "&PrintFomatID=" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID), "_blank");
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

        private void cmbSpermType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TxtSampleCode.Text = "";
            TxtSampleCode.Tag = "";

            if ((MasterListItem)cmbSpermType.SelectedItem != null)
            {
                if (((MasterListItem)cmbSpermType.SelectedItem).ID != 3 && ((MasterListItem)cmbSpermType.SelectedItem).ID != 6 && ((MasterListItem)cmbSpermType.SelectedItem).ID != 8 && ((MasterListItem)cmbSpermType.SelectedItem).ID != 10)
                {
                    CmdSelectSample.IsEnabled = false;
                }
                else
                {
                    CmdSelectSample.IsEnabled = true;
                }
            }
        }

        private void CmdSelectSample_Click(object sender, RoutedEventArgs e)
        {
            if (((MasterListItem)cmbSpermType.SelectedItem).ID == 10)
            {
                SpermThawingForDashboard frm = new SpermThawingForDashboard();
                frm.IsView = IsView;
                frm.CoupleDetails = CoupleDetails;
                frm.SemenWashID = SelectedRecord;
                frm.IsFromSemenPreparation = true;
                frm.OKButtonCode_Click += new RoutedEventHandler(SpermThwa_Ok_Click);
                frm.Show();
            }
            else if (((MasterListItem)cmbSpermType.SelectedItem).ID == 3 || (((MasterListItem)cmbSpermType.SelectedItem).ID == 6) || (((MasterListItem)cmbSpermType.SelectedItem).ID == 8))
            {
                TesaPesaTeseList win = new TesaPesaTeseList();
                win.CoupleDetails = CoupleDetails;
                win.DescriptionValue = ((MasterListItem)cmbSpermType.SelectedItem).Description;
                win.SelectTesaCode_Click += new RoutedEventHandler(win_SelectTesaCode_Click);
                win.Show();
            }
        }

        public void win_SelectTesaCode_Click(object sender, RoutedEventArgs e)
        {
            if (((TesaPesaTeseList)sender).DialogResult == true)
            {
                if (((TesaPesaTeseList)sender).TesaPesa != null)
                {
                    TxtSampleCode.Text = ((TesaPesaTeseList)sender).TesaPesa.tesapesacode;
                    TxtSampleCode.Tag = ((TesaPesaTeseList)sender).TesaPesa.tesapesacode;
                }
            }
        }

        public void SpermThwa_Ok_Click(object sender, RoutedEventArgs e)
        {
            //if (((SpermThawingForDashboard)sender).DialogResult == true)
            //{
            //    if (((SpermThawingForDashboard)sender).SpermThaw != null)
            //    {
            if (((SpermThawingForDashboard)sender).SampleID != null)
            {
                TxtSampleCode.Text = ((SpermThawingForDashboard)sender).SampleID.ToString();
                TxtSampleCode.Tag = ((SpermThawingForDashboard)sender).SampleID;
            }
            //TxtSampleCode.Text = ((SpermThawingForDashboard)sender).SpermThaw.spermFrezingCode;
            //TxtSampleCode.Tag = ((SpermThawingForDashboard)sender).SpermThaw.ThawingID;
            //SpermThaw.spermFrezingCode;
            //TxtSampleCode.Text = ((SpermThawingForDashboard)sender).SpermThaw.ThawList.c;
            //    }
            //}
        }

        private void hlbViewLinking_Click(object sender, RoutedEventArgs e)
        {

        }



        private void checkHIV_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void txtvolume_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (!((TextBox)sender).Text.IsValidOneDigitWithTwoDecimal() && textBefore != null)
            if (!((TextBox)sender).Text.IsValidOneDigitWithOneDecimal() && textBefore != null)   
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtvolume_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void TxtQuantity_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TxtPreSpermCount.Text.Trim() != "" && TxtQuantity.Text.Trim() != "")
            {
                double value = (Convert.ToDouble(TxtPreSpermCount.Text)) * (Convert.ToDouble(TxtQuantity.Text));
                TxtPreTotCount.Text = value.ToString();
            }

            if (TxtPostSpermCount.Text.Trim() != "" && TxtQuantity.Text.Trim() != "")
            {
                double value = (Convert.ToDouble(TxtPostSpermCount.Text)) * (Convert.ToDouble(TxtQuantity.Text));
                TxtPostTotCount.Text = value.ToString();
            }

        }

    }
}

