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
    public partial class frmIUID : ChildWindow, IInitiateCIMS
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
                case "5":
                    TransactionTypeID = 5;
                    first.Content = "List Of IUI D Details";
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
                Title = "IUI D (Name:- " + ((IApplicationConfiguration)App.Current).SelectedPatient.PatientName + ")";
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

        public frmIUID()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            //Paging
            DataList = new PagedSortableCollectionView<cls_IVFDashboard_SemenWashVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;

            this.DataContext = null;
        }

        public frmIUID(long PatientID, long PatientUnitID)
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
        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
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
                    //chkFreeze.IsEnabled = true;
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


        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
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

                //cmbInseminatedBy.IsEnabled = false;
                rbtExisting.IsChecked = true;
                //  CollectionTime.SetValue = 

                FillInseminatedBy();

                dtIUIDate.SelectedDate = DateTime.Now.Date;
                IUITime.Value = DateTime.Now.Date;
                dtCollection.SelectedDate = DateTime.Now;
                CollectionTime.Value = DateTime.Now.Date;
                //dtThawDate.SelectedDate = DateTime.Now;
                //ThawTime.Value = DateTime.Now.Date;
                dtpreperation.SelectedDate = DateTime.Now;
                PreperationTime.Value = DateTime.Now.Date;

                //if (PlannedSpermCollection == 4 || PlannedSpermCollection == 11 || PlannedSpermCollection == 13 || PlannedSpermCollection == 17 || PlannedSpermCollection == 18)
                if (PlannedSpermCollection == 24)
                {
                    dtThawDate.IsEnabled = true;
                    ThawTime.IsEnabled = true;
                    SelectSample.IsEnabled = true;
                }
                if (IsClosed)
                    cmdNew.IsEnabled = false;
                else
                    cmdNew.IsEnabled = true;
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

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
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

        private void ClearAll()
        {
            this.DataContext = new cls_IVFDashboard_SemenWashVO();
            gb1.DataContext = null;
            grpIUID.DataContext = null;
            grpPhysicalCharacteristics.DataContext = null;
            grpClassification.DataContext = null;
            grpCellInfo.DataContext = null;
            AccDonor.DataContext = null;
            grpContactDetails.DataContext = null;

            dtIUIDate.SelectedDate = null;
            IUITime.Value = null;
            dtCollection.SelectedDate = null;
            CollectionTime.Value = null;
            dtpreperation.SelectedDate = null;
            PreperationTime.Value = null;
            cmbCollection.SelectedValue = (long)0;
            cmbInseminatedBy.SelectedValue = (long)0;
            cmbWitnessedBy.SelectedValue = (long)0;
            txtInseminated.Text = "";
            TxtPreAmount.Text = "";
            TxtPostAmount.Text = "";
            TxtPreSpermCount.Text = "";
            TxtPostSpermCount.Text = "";
            TxtPreTotCount.Text = "";
            TxtPostTotCount.Text = "";
            TxtPreProgMotility.Text = "";
            TxtPostProgMotility.Text = "";
            TxtPreNonProgressive.Text = "";
            TxtPostNonProgressive.Text = "";
            TxtPreNonMotile.Text = "";
            TxtPostNonMotile.Text = "";
            TxtPreTotalMotility.Text = "";
            TxtPostTotalMotility.Text = "";
            TxtPreNormalForms.Text = "";
            TxtPostNormalForms.Text = "";
            TxtRemark.Text = "";
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


        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;

            if (ClickedFlag == 1)
            {
                bool saveDtls = true;

                saveDtls = Validate();

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
                SaveIUI();
            else
                ClickedFlag = 0;

        }

        long DonorID = 0, DonorUnitID = 0, BatchID = 0, BatchUnitID = 0;
        long IUIID = 0;
        private void SaveIUI()
        {
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();


            cls_IVFDashboard_AddUpdateSemenWashBizActionVO BizAction = new cls_IVFDashboard_AddUpdateSemenWashBizActionVO();
            BizAction.SemensWashDetails.UnitID = UnitID;
            BizAction.SemensWashDetails.PatientID = PatientID;
            BizAction.SemensWashDetails.PatientUnitID = PatientUnitID;
            BizAction.SemensWashDetails.VisitID = VisitID;

            //BizAction.SemensWashDetails.PatientID = CoupleDetails.FemalePatient.PatientID;
            //BizAction.SemensWashDetails.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.SemensWashDetails.PlanTherapyID = 0;
            BizAction.SemensWashDetails.PlanTherapyUnitID = 0;

            BizAction.SemensWashDetails.BatchID = BatchID;
            BizAction.SemensWashDetails.BatchUnitID = BatchUnitID;

            BizAction.SemensWashDetails.BatchCode = txtBatchCode.Text;

            BizAction.SemensWashDetails.ISFromIUI = false;
            BizAction.SemensWashDetails.ID = IUIID;

            //For IUI
            BizAction.SemensWashDetails.TransacationTypeID = TransactionTypeID;
            if (dtIUIDate.SelectedDate != null)
                BizAction.SemensWashDetails.IUIDate = dtIUIDate.SelectedDate.Value.Date;
            if (IUITime.Value != null)
                BizAction.SemensWashDetails.IUIDate = BizAction.SemensWashDetails.IUIDate.Value.Add(IUITime.Value.Value.TimeOfDay);

            if (dtCollection.SelectedDate != null)
                BizAction.SemensWashDetails.CollectionDate = dtCollection.SelectedDate.Value.Date;
            if (CollectionTime.Value != null)
                BizAction.SemensWashDetails.CollectionDate = BizAction.SemensWashDetails.CollectionDate.Value.Add(CollectionTime.Value.Value.TimeOfDay);

            if (dtThawDate.SelectedDate != null)
                BizAction.SemensWashDetails.ThawDate = dtThawDate.SelectedDate.Value.Date;
            if (ThawTime.Value != null)
                BizAction.SemensWashDetails.ThawDate = BizAction.SemensWashDetails.ThawDate.Value.Add(ThawTime.Value.Value.TimeOfDay); //added by neena

            if (dtpreperation.SelectedDate != null)
                BizAction.SemensWashDetails.PreperationDate = dtpreperation.SelectedDate.Value.Date;
            if (PreperationTime.Value != null)
                BizAction.SemensWashDetails.PreperationDate = BizAction.SemensWashDetails.PreperationDate.Value.Add(PreperationTime.Value.Value.TimeOfDay);

            if (cmbCollection.SelectedItem != null)
                BizAction.SemensWashDetails.MethodOfCollectionID = ((MasterListItem)cmbCollection.SelectedItem).ID;

            BizAction.SemensWashDetails.Inseminated = txtInseminated.Text.Trim();

            if (cmbInseminatedBy.SelectedItem != null)
                BizAction.SemensWashDetails.InSeminatedByID = ((MasterListItem)cmbInseminatedBy.SelectedItem).ID;
            if (cmbWitnessedBy.SelectedItem != null)
                BizAction.SemensWashDetails.WitnessByID = ((MasterListItem)cmbWitnessedBy.SelectedItem).ID;

            if (!string.IsNullOrEmpty(TxtPreAmount.Text.Trim()))
                BizAction.SemensWashDetails.PreAmount = float.Parse(TxtPreAmount.Text.Trim());
            if (!string.IsNullOrEmpty(TxtPostAmount.Text.Trim()))
                BizAction.SemensWashDetails.PostAmount = float.Parse(TxtPostAmount.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPreSpermCount.Text.Trim()))
                BizAction.SemensWashDetails.PreSpermCount = float.Parse(TxtPreSpermCount.Text.Trim());
            if (!string.IsNullOrEmpty(TxtPostSpermCount.Text.Trim()))
                BizAction.SemensWashDetails.PostSpermCount = float.Parse(TxtPostSpermCount.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPreTotCount.Text.Trim()))
                BizAction.SemensWashDetails.PreTotalSpermCount = float.Parse(TxtPreTotCount.Text.Trim());
            if (!string.IsNullOrEmpty(TxtPostTotCount.Text.Trim()))
                BizAction.SemensWashDetails.PostTotalSpermCount = float.Parse(TxtPostTotCount.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPreProgMotility.Text.Trim()))
                BizAction.SemensWashDetails.PreProgMotility = float.Parse(TxtPreProgMotility.Text.Trim());
            if (!string.IsNullOrEmpty(TxtPostProgMotility.Text.Trim()))
                BizAction.SemensWashDetails.PostProgMotility = float.Parse(TxtPostProgMotility.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPreNonProgressive.Text.Trim()))
                BizAction.SemensWashDetails.PreNonProgressive = float.Parse(TxtPreNonProgressive.Text.Trim());
            if (!string.IsNullOrEmpty(TxtPostNonProgressive.Text.Trim()))
                BizAction.SemensWashDetails.PostNonProgressive = float.Parse(TxtPostNonProgressive.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPreNonMotile.Text.Trim()))
                BizAction.SemensWashDetails.PreNonMotile = float.Parse(TxtPreNonMotile.Text.Trim());
            if (!string.IsNullOrEmpty(TxtPostNonMotile.Text.Trim()))
                BizAction.SemensWashDetails.PostNonMotile = float.Parse(TxtPostNonMotile.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPreMotileSpermCount.Text.Trim()))
                BizAction.SemensWashDetails.PreMotileSpermCount = float.Parse(TxtPreMotileSpermCount.Text.Trim());
            if (!string.IsNullOrEmpty(TxtPostMotileSpermCount.Text.Trim()))
                BizAction.SemensWashDetails.PostMotileSpermCount = float.Parse(TxtPostMotileSpermCount.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPreTotalMotility.Text.Trim()))
                BizAction.SemensWashDetails.PreTotalMotility = float.Parse(TxtPreTotalMotility.Text.Trim());
            if (!string.IsNullOrEmpty(TxtPostTotalMotility.Text.Trim()))
                BizAction.SemensWashDetails.PostTotalMotility = float.Parse(TxtPostTotalMotility.Text.Trim());

            if (!string.IsNullOrEmpty(TxtPreNormalForms.Text.Trim()))
                BizAction.SemensWashDetails.PreNormalForms = float.Parse(TxtPreNormalForms.Text.Trim());
            if (!string.IsNullOrEmpty(TxtPostNormalForms.Text.Trim()))
                BizAction.SemensWashDetails.PostNormalForms = float.Parse(TxtPostNormalForms.Text.Trim());

            if (!string.IsNullOrEmpty(TxtRemark.Text.Trim()))
                BizAction.SemensWashDetails.Comment = TxtRemark.Text.Trim();

            if (!string.IsNullOrEmpty(txtSampleID.Text.Trim()))
                BizAction.SemensWashDetails.SampleID = txtSampleID.Text;

            if (!string.IsNullOrEmpty(txtDonorCode.Text.Trim()))
            {
                BizAction.SemensWashDetails.DonorID = DonorID;
                BizAction.SemensWashDetails.DonorUnitID = DonorUnitID;
            }



            if (cmbnseminationLocation.SelectedItem != null)
                BizAction.SemensWashDetails.InSeminationLocationID = ((MasterListItem)cmbnseminationLocation.SelectedItem).ID;
            if (cmbInseminationMethod.SelectedItem != null)
                BizAction.SemensWashDetails.InSeminationMethodID = ((MasterListItem)cmbInseminationMethod.SelectedItem).ID;
            //BizAction.SemensWashDetails.SampleID = txtSemenBankSampleID.Text;
            //BizAction.SemensWashDetails.DonorID = DonorID;
            //BizAction.SemensWashDetails.DonorUnitID = DonorUnitID;

            if (!string.IsNullOrEmpty(TxtOtherCells.Text.Trim()))
                BizAction.SemensWashDetails.AnyOtherCells = TxtOtherCells.Text.Trim();
            //..........................

            if (cmbAbstience.SelectedItem != null)
                BizAction.SemensWashDetails.AbstinenceID = ((MasterListItem)cmbAbstience.SelectedItem).ID;
            BizAction.SemensWashDetails.TimeRecSampLab = SampRecTime.Value;

            if (rbtCentre.IsChecked == true)
                BizAction.SemensWashDetails.CollecteAtCentre = true;
            else if (rbtOutSideCentre.IsChecked == true)
                BizAction.SemensWashDetails.CollecteAtCentre = false;
            if (cmbColor.SelectedItem != null)
                BizAction.SemensWashDetails.ColorID = ((MasterListItem)cmbColor.SelectedItem).ID;
            if (!string.IsNullOrEmpty(txtVolume.Text.Trim()))
                BizAction.SemensWashDetails.Quantity = float.Parse(txtVolume.Text.Trim());
            if (!string.IsNullOrEmpty(txtpH.Text.Trim()))
                BizAction.SemensWashDetails.PH = float.Parse(txtpH.Text.Trim());
            BizAction.SemensWashDetails.LiquificationTime = txtLiqueficationTime.Text;
            if (rbtNormal.IsChecked == true)
                BizAction.SemensWashDetails.Viscosity = true;
            else if (rbtViscous.IsChecked == true)
                BizAction.SemensWashDetails.Viscosity = false;
            if (rbtPresent.IsChecked == true)
                BizAction.SemensWashDetails.Odour = true;
            else if (rbtNotPresent.IsChecked == true)
                BizAction.SemensWashDetails.Odour = false;

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
            BizAction.SemensWashDetails.MotileSperm = txtMotile.Text.Trim();
            if (!string.IsNullOrEmpty(TxtRoundCells.Text.Trim()))
                BizAction.SemensWashDetails.RoundCells = TxtRoundCells.Text.Trim();
            if (!string.IsNullOrEmpty(TxtEpithelialCells.Text.Trim()))
                BizAction.SemensWashDetails.EpithelialCells = TxtEpithelialCells.Text.Trim();

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


                gb1.DataContext = (cls_IVFDashboard_SemenWashVO)this.DataContext;
                //gb2.DataContext = (cls_IVFDashboard_SemenWashVO)this.DataContext;
                grpIUID.DataContext = (cls_IVFDashboard_SemenWashVO)this.DataContext;
                grpPhysicalCharacteristics.DataContext = (cls_IVFDashboard_SemenWashVO)this.DataContext;
                grpClassification.DataContext = (cls_IVFDashboard_SemenWashVO)this.DataContext;
                grpCellInfo.DataContext = (cls_IVFDashboard_SemenWashVO)this.DataContext;
                AccDonor.DataContext = (cls_IVFDashboard_SemenWashVO)this.DataContext;
                grpContactDetails.DataContext = (cls_IVFDashboard_SemenWashVO)this.DataContext;

                IUIID = ((cls_IVFDashboard_SemenWashVO)this.DataContext).ID;

                if (((cls_IVFDashboard_SemenWashVO)this.DataContext).MethodOfCollectionID != null && ((cls_IVFDashboard_SemenWashVO)this.DataContext).MethodOfCollectionID > 0)
                    cmbCollection.SelectedValue = ((cls_IVFDashboard_SemenWashVO)this.DataContext).MethodOfCollectionID;
                if (((cls_IVFDashboard_SemenWashVO)this.DataContext).InSeminatedByID != null && ((cls_IVFDashboard_SemenWashVO)this.DataContext).InSeminatedByID > 0)
                    cmbInseminatedBy.SelectedValue = ((cls_IVFDashboard_SemenWashVO)this.DataContext).InSeminatedByID;
                if (((cls_IVFDashboard_SemenWashVO)this.DataContext).WitnessByID != null)
                    cmbWitnessedBy.SelectedValue = ((cls_IVFDashboard_SemenWashVO)this.DataContext).WitnessByID;


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

                if (((cls_IVFDashboard_SemenWashVO)this.DataContext).IsFreezed == true)
                {
                    CmdSave.IsEnabled = false;
                    // chkFreeze.IsEnabled = false;
                }
                else
                {
                    CmdSave.IsEnabled = true;
                    //chkFreeze.IsEnabled = true;
                }

                objAnimation.Invoke(RotationType.Forward);

                CmdSave.Content = "Modify";

                IsModify = true;
            }

        }

        #region FillCombox
        public long PlanTherapyID;
        public long PlanTherapyUnitID;
        public bool IsClosed;
        public long PlannedSpermCollection;
        public long PlannedTreatmentID;

        private void FillInseminatedBy()
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

                    cmbInseminatedBy.ItemsSource = null;
                    cmbInseminatedBy.ItemsSource = objList;
                    cmbInseminatedBy.SelectedValue = (long)0;


                    //by neena
                    if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID > 0)
                        cmbInseminatedBy.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
                    //

                    cmbWitnessedBy.ItemsSource = null;
                    cmbWitnessedBy.ItemsSource = objList;
                    cmbWitnessedBy.SelectedItem = objList[0];
                    //   fillfillInseminationmethod();
                    FillCollectionMethod();   //  as per the requirements of Milann
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void fillfillInseminationmethod()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_InseminationmethodMaster;
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

                    cmbInseminationMethod.ItemsSource = null;
                    cmbInseminationMethod.ItemsSource = objList;
                    cmbInseminationMethod.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {

                    }
                    fillInseminationLocationMaster();
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void fillInseminationLocationMaster()
        {

            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_InseminationLocationMaster;
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

                    cmbnseminationLocation.ItemsSource = null;
                    cmbnseminationLocation.ItemsSource = objList;
                    cmbnseminationLocation.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {

                    }
                    FillCollectionMethod();
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void FillCollectionMethod()
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

                    cmbCollection.ItemsSource = null;
                    cmbCollection.ItemsSource = objList;
                    cmbCollection.SelectedItem = objList[0];

                    //cmbCollection.SelectedValue = (long)PlannedSpermCollection;
                    cmbCollection.SelectedItem = objList.Where(x => x.ID == Convert.ToInt64(PlannedSpermCollection)).FirstOrDefault();


                    if (this.DataContext != null)
                    {

                    }
                    FetchData();
                    //fillDetails();
                    //FillAbstience();
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void FillAbstience()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();

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


                    cmbAbstience.ItemsSource = null;
                    cmbAbstience.ItemsSource = objList;
                    cmbAbstience.SelectedItem = objList[0];
                }

                if (this.DataContext != null)
                {

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
                    cmbColor.ItemsSource = null;
                    cmbColor.ItemsSource = objList;
                    cmbColor.SelectedItem = objList[0];
                }
                if (this.DataContext != null)
                {

                }
                FetchData();
                //fillDetails();
                //fillPlannedTreatment();
                // fillFinalPlannedTreatment();

            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }


        #endregion

        private bool Validate()
        {
            bool result = true;
            if (dtIUIDate.SelectedDate == null)
            {
                dtIUIDate.SetValidation("Please Select IUI Date");
                dtIUIDate.RaiseValidationError();
                dtIUIDate.Focus();
                result = false;
            }
            else
                dtIUIDate.ClearValidationError();
            if (IUITime.Value == null)
            {
                IUITime.SetValidation("Please Select IUI Time");
                IUITime.RaiseValidationError();
                IUITime.Focus();
                result = false;
            }
            else
                IUITime.ClearValidationError();

            if (dtCollection.SelectedDate == null)
            {
                dtCollection.SetValidation("Please Select Collection Date");
                dtCollection.RaiseValidationError();
                dtCollection.Focus();
                result = false;
            }
            else
                dtCollection.ClearValidationError();

            if (CollectionTime.Value == null)
            {
                CollectionTime.SetValidation("Please Select Time of Collection");
                CollectionTime.RaiseValidationError();
                CollectionTime.Focus();
                result = false;
            }
            else
                CollectionTime.ClearValidationError();



            if (PlannedSpermCollection == 4 || PlannedSpermCollection == 11 || PlannedSpermCollection == 13 || PlannedSpermCollection == 17 || PlannedSpermCollection == 18)
            {
                if (dtThawDate.SelectedDate == null)
                {
                    dtThawDate.SetValidation("Please Select Thaw Date");
                    dtThawDate.RaiseValidationError();
                    dtThawDate.Focus();
                    result = false;
                }
                else
                    dtThawDate.ClearValidationError();

                if (ThawTime.Value == null)
                {
                    ThawTime.SetValidation("Please Select Time of Thaw");
                    ThawTime.RaiseValidationError();
                    ThawTime.Focus();
                    result = false;
                }
                else
                    ThawTime.ClearValidationError();

                if (txtSampleID.Text.Trim() == string.Empty)
                {
                    txtSampleID.SetValidation("Please Select Sample ID");
                    txtSampleID.RaiseValidationError();
                    txtSampleID.Focus();
                    result = false;
                }
                else
                    txtSampleID.ClearValidationError();

                if (PlannedTreatmentID == 16)
                {
                    if (txtDonorCode.Text.Trim() == string.Empty)
                    {
                        txtDonorCode.SetValidation("Please Select Donor Code");
                        txtDonorCode.RaiseValidationError();
                        txtDonorCode.Focus();
                        result = false;
                    }
                    else
                        txtDonorCode.ClearValidationError();
                }

            }

            if (dtpreperation.SelectedDate == null)
            {
                dtpreperation.SetValidation("Please Select Preperation Date");
                dtpreperation.RaiseValidationError();
                dtpreperation.Focus();
                result = false;
            }
            else
                dtpreperation.ClearValidationError();

            if (PreperationTime.Value == null)
            {
                PreperationTime.SetValidation("Please Select Time of Preperation");
                PreperationTime.RaiseValidationError();
                PreperationTime.Focus();
                result = false;
            }
            else
                PreperationTime.ClearValidationError();

            if (cmbCollection.SelectedItem == null)
            {
                cmbCollection.TextBox.SetValidation("Please select Collection Method");
                cmbCollection.TextBox.RaiseValidationError();
                cmbCollection.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbCollection.SelectedItem).ID == 0)
            {
                cmbCollection.TextBox.SetValidation("Please select Collection Method");
                cmbCollection.TextBox.RaiseValidationError();
                cmbCollection.Focus();
                result = false;
            }
            else
                cmbCollection.TextBox.ClearValidationError();


            if (cmbInseminatedBy.SelectedItem == null)
            {
                cmbInseminatedBy.TextBox.SetValidation("Please select Inseminated By");
                cmbInseminatedBy.TextBox.RaiseValidationError();
                cmbInseminatedBy.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbInseminatedBy.SelectedItem).ID == 0)
            {
                cmbInseminatedBy.TextBox.SetValidation("Please select Inseminated By");
                cmbInseminatedBy.TextBox.RaiseValidationError();
                cmbInseminatedBy.Focus();
                result = false;
            }
            else
                cmbInseminatedBy.TextBox.ClearValidationError();

            if (cmbWitnessedBy.SelectedItem == null)
            {
                cmbWitnessedBy.TextBox.SetValidation("Please select Witnessed By");
                cmbWitnessedBy.TextBox.RaiseValidationError();
                cmbWitnessedBy.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbWitnessedBy.SelectedItem).ID == 0)
            {
                cmbWitnessedBy.TextBox.SetValidation("Please select Witnessed By");
                cmbWitnessedBy.TextBox.RaiseValidationError();
                cmbWitnessedBy.Focus();
                result = false;
            }
            else
                cmbWitnessedBy.TextBox.ClearValidationError();

            DateTime? CollectionDate = new DateTime();
            DateTime? ThawDate = new DateTime();
            DateTime? PreparationDate = new DateTime();
            DateTime? IUIDate = new DateTime();

            if (dtCollection.SelectedDate != null && CollectionTime.Value != null)
            {
                CollectionDate = dtCollection.SelectedDate.Value.Date;
                CollectionDate = CollectionDate.Value.Add(CollectionTime.Value.Value.TimeOfDay);
            }

            if (dtThawDate.SelectedDate != null && ThawTime.Value != null)
            {
                ThawDate = dtThawDate.SelectedDate.Value.Date;
                ThawDate = ThawDate.Value.Add(ThawTime.Value.Value.TimeOfDay);
            }

            if (dtpreperation.SelectedDate != null && PreperationTime.Value != null)
            {
                PreparationDate = dtpreperation.SelectedDate.Value.Date;
                PreparationDate = PreparationDate.Value.Add(PreperationTime.Value.Value.TimeOfDay);
            }

            if (dtIUIDate.SelectedDate != null && IUITime.Value != null)
            {
                IUIDate = dtIUIDate.SelectedDate.Value.Date;
                IUIDate = IUIDate.Value.Add(IUITime.Value.Value.TimeOfDay);
            }


            if (CollectionDate > PreparationDate)
            {
                dtCollection.SetValidation("Collection Date Cannot Be Greater Than Preperation Date");
                dtCollection.RaiseValidationError();
                dtCollection.Focus();
                result = false;
            }
            else if (CollectionDate > IUIDate)
            {
                dtCollection.SetValidation("Collection Date Cannot Be Greater Than IUI Date");
                dtCollection.RaiseValidationError();
                dtCollection.Focus();
                result = false;
            }
            else if (PreparationDate > IUIDate)
            {
                dtpreperation.SetValidation("Preperation Date Cannot Be Greater Than IUI Date");
                dtpreperation.RaiseValidationError();
                dtpreperation.Focus();
                result = false;
            }
            else if (dtThawDate.SelectedDate != null)
            {
                if (CollectionDate > ThawDate)
                {
                    dtCollection.SetValidation("Collection Date Cannot Be Greater Than Thaw Date");
                    dtCollection.RaiseValidationError();
                    dtCollection.Focus();
                    result = false;
                }
                else if (ThawDate > PreparationDate)
                {
                    dtThawDate.SetValidation("Thaw Date Cannot Be Greater Than Preperation Date");
                    dtThawDate.RaiseValidationError();
                    dtThawDate.Focus();
                    result = false;
                }
                else if (ThawDate > IUIDate)
                {
                    dtThawDate.SetValidation("Thaw Date Cannot Be Greater Than IUI Date");
                    dtThawDate.RaiseValidationError();
                    dtThawDate.Focus();
                    result = false;
                }
                else
                {
                    dtCollection.ClearValidationError();
                    dtThawDate.ClearValidationError();
                }
            }
            else
            {
                dtCollection.ClearValidationError();
                dtpreperation.ClearValidationError();
            }



            if (TxtPostProgMotility.Text != null && TxtPostNonProgressive.Text != null && TxtPostNonMotile.Text != null && TxtPreProgMotility.Text != null && TxtPreNonProgressive.Text != null && TxtPreNonMotile.Text != null)
            {
                double valuePost = (Convert.ToDouble(TxtPostProgMotility.Text) + Convert.ToDouble(TxtPostNonProgressive.Text) + Convert.ToDouble(TxtPostNonMotile.Text));
                double valuePre = (Convert.ToDouble(TxtPreProgMotility.Text) + Convert.ToDouble(TxtPreNonProgressive.Text) + Convert.ToDouble(TxtPreNonMotile.Text));

                if (valuePre == 100)
                {
                    TxtPreProgMotility.ClearValidationError();
                    TxtPreNonProgressive.ClearValidationError();
                    TxtPreNonMotile.ClearValidationError();
                }

                if (valuePost == 100)
                {
                    TxtPostProgMotility.ClearValidationError();
                    TxtPostNonProgressive.ClearValidationError();
                    TxtPostNonMotile.ClearValidationError();
                }

                if (valuePost < 100 && valuePre < 100)
                {
                    TxtPreProgMotility.SetValidation("Sum of Progressive,Non Progressive,Non motile can not be less than 100");
                    TxtPreProgMotility.RaiseValidationError();
                    TxtPreProgMotility.Focus();

                    TxtPreNonProgressive.SetValidation("Sum of Progressive,Non Progressive,Non motile can not be less than 100");
                    TxtPreNonProgressive.RaiseValidationError();
                    TxtPreNonProgressive.Focus();

                    TxtPreNonMotile.SetValidation("Sum of Progressive,Non Progressive,Non motile can not be less than 100");
                    TxtPreNonMotile.RaiseValidationError();
                    TxtPreNonMotile.Focus();

                    //TxtPreNonProgressive.SetValidation("Sum of Progressive,Non Progressive,Non motile can not be less than 100");
                    //TxtPreNonProgressive.RaiseValidationError();
                    //TxtPreNonProgressive.Focus();
                    //TxtPreNonMotile.SetValidation("Sum of Progressive,Non Progressive,Non motile can not be less than 100");
                    //TxtPreNonMotile.RaiseValidationError();
                    //TxtPreNonMotile.Focus();
                    //MessageBoxControl.MessageBoxChildWindow msgW1 =
                    //           new MessageBoxControl.MessageBoxChildWindow("", " For Pre and Post Wash Sum of Progressive,Non Progressive,Non motile can not be less than 100.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //msgW1.Show();
                    result = false;
                }
                else if (valuePre < 100)
                {
                    TxtPreProgMotility.SetValidation("Sum of Progressive,Non Progressive,Non motile can not be less than 100");
                    TxtPreProgMotility.RaiseValidationError();
                    TxtPreProgMotility.Focus();

                    TxtPreNonProgressive.SetValidation("Sum of Progressive,Non Progressive,Non motile can not be less than 100");
                    TxtPreNonProgressive.RaiseValidationError();
                    TxtPreNonProgressive.Focus();

                    TxtPreNonMotile.SetValidation("Sum of Progressive,Non Progressive,Non motile can not be less than 100");
                    TxtPreNonMotile.RaiseValidationError();
                    TxtPreNonMotile.Focus();

                    //TxtPreProgMotility.RaiseValidationError();
                    //TxtPreProgMotility.Focus();
                    //TxtPreNonProgressive.RaiseValidationError();
                    //TxtPreNonProgressive.Focus();
                    //TxtPreNonMotile.RaiseValidationError();
                    //TxtPreNonMotile.Focus();
                    //MessageBoxControl.MessageBoxChildWindow msgW1 =
                    //           new MessageBoxControl.MessageBoxChildWindow("", " For Pre Wash Sum of Progressive,Non Progressive,Non motile can not be less than 100.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //msgW1.Show();


                    result = false;
                }
                else if (valuePost < 100)
                {
                    TxtPostProgMotility.SetValidation("Sum of Progressive,Non Progressive,Non motile can not be less than 100");
                    TxtPostProgMotility.RaiseValidationError();
                    TxtPostProgMotility.Focus();

                    TxtPostNonProgressive.SetValidation("Sum of Progressive,Non Progressive,Non motile can not be less than 100");
                    TxtPostNonProgressive.RaiseValidationError();
                    TxtPostNonProgressive.Focus();

                    TxtPostNonMotile.SetValidation("Sum of Progressive,Non Progressive,Non motile can not be less than 100");
                    TxtPostNonMotile.RaiseValidationError();
                    TxtPostNonMotile.Focus();

                    //TxtPostProgMotility.RaiseValidationError();
                    //TxtPostProgMotility.Focus();
                    //TxtPostNonProgressive.RaiseValidationError();
                    //TxtPostNonProgressive.Focus();
                    //TxtPostNonMotile.RaiseValidationError();
                    //TxtPostNonMotile.Focus();
                    //MessageBoxControl.MessageBoxChildWindow msgW1 =
                    //           new MessageBoxControl.MessageBoxChildWindow("", " For POst Wash Sum of Progressive,Non Progressive,Non motile can not be less than 100.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //msgW1.Show();


                    result = false;
                }
                else
                {
                    TxtPreProgMotility.ClearValidationError();
                    TxtPreNonProgressive.ClearValidationError();
                    TxtPreNonMotile.ClearValidationError();
                    TxtPostProgMotility.ClearValidationError();
                    TxtPostNonProgressive.ClearValidationError();
                    TxtPostNonMotile.ClearValidationError();
                }

            }

            return result;

            //if (SampRecTime.Value == null)
            //{
            //    SampRecTime.SetValidation("Please Select Time of Receiving");
            //    SampRecTime.RaiseValidationError();
            //    SampRecTime.Focus();
            //    result = false;
            //}
            //else
            //    SampRecTime.ClearValidationError();



        }

        decimal PreSumOfGrade = 0;
        private void PreTotal_TextChanged(object sender, TextChangedEventArgs e)
        {
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

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void cmbSimulation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PrescriptionOvarian_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            long ID, UnitID, TransacationTypeID;
            if (dgSemenWash.SelectedItem != null)
            {
                ID = ((cls_IVFDashboard_SemenWashVO)dgSemenWash.SelectedItem).ID;
                UnitID = ((cls_IVFDashboard_SemenWashVO)dgSemenWash.SelectedItem).UnitID;
                TransacationTypeID = ((cls_IVFDashboard_SemenWashVO)dgSemenWash.SelectedItem).TransacationTypeID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/IVFDashboard/SemenWashAndIUI.aspx?ID=" + ID + "&UnitID=" + UnitID + "&IsFromIUI=" + false + "&TransacationTypeID=" + TransacationTypeID + "&PrintFomatID=" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID), "_blank");
            }
        }

        private void TxtPreAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValidOneDigitWithTwoDecimal() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void TxtPreAmount_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void TxtPreTotCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValidThreeDigit() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void TxtPreTotCount_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void TxtPreProgMotility_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValidTwoDigit() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void TxtPreProgMotility_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void TxtPreAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TxtPreAmount.Text != "0" && TxtPreAmount.Text.Trim() != null && TxtPreAmount.Text.Trim() != "")
            {
                if (TxtPreSpermCount.Text != "0" && TxtPreSpermCount.Text.Trim() != null && TxtPreSpermCount.Text.Trim() != "")
                {
                    double value = Convert.ToDouble(TxtPreAmount.Text) * Convert.ToDouble(TxtPreSpermCount.Text);
                    //double PreTot = Math.Round(value, 0);
                    TxtPreTotCount.Text = value.ToString();
                }
            }

            if (TxtPreMotileSpermCount.Text != "0" && TxtPreMotileSpermCount.Text.Trim() != null && TxtPreMotileSpermCount.Text.Trim() != "")
            {
                if (TxtPreTotCount.Text != "0" && TxtPreTotCount.Text.Trim() != null && TxtPreMotileSpermCount.Text.Trim() != "")
                {
                    double value = (Convert.ToDouble(TxtPreMotileSpermCount.Text) / Convert.ToDouble(TxtPreTotCount.Text)) * 100;
                   // double PreMot = Math.Round(value, 0);
                    TxtPreTotalMotility.Text = value.ToString();

                }
            }
        }

        private void TxtPostAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TxtPostAmount.Text != "0" && TxtPostAmount.Text.Trim() != null && TxtPostAmount.Text.Trim() != "")
            {
                if (TxtPostSpermCount.Text != "0" && TxtPostSpermCount.Text.Trim() != null && TxtPostSpermCount.Text.Trim() != "")
                {
                    double value = Convert.ToDouble(TxtPostAmount.Text) * Convert.ToDouble(TxtPostSpermCount.Text);
                    double PostTot = Math.Round(value, 0);
                    TxtPostTotCount.Text = PostTot.ToString();
                }
            }

            if (TxtPostMotileSpermCount.Text != "0" && TxtPostMotileSpermCount.Text.Trim() != null && TxtPostMotileSpermCount.Text.Trim() != "")
            {
                if (TxtPostTotCount.Text != "0" && TxtPostTotCount.Text.Trim() != null && TxtPostMotileSpermCount.Text.Trim() != "")
                {
                    double value = (Convert.ToDouble(TxtPostMotileSpermCount.Text) / Convert.ToDouble(TxtPostTotCount.Text)) * 100;
                    double PostMot = Math.Round(value, 0);
                    TxtPostTotalMotility.Text = PostMot.ToString();

                }
            }
        }

        private void TxtPreProgMotility_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (TxtPreProgMotility.Text.Trim() != null && TxtPreProgMotility.Text.Trim() != "" && TxtPreNonProgressive.Text.Trim() != null && TxtPreNonProgressive.Text.Trim() != "")
            //{
            //    double valuePre = (Convert.ToDouble(TxtPreProgMotility.Text) + Convert.ToDouble(TxtPreNonProgressive.Text));
            //    TxtPreTotalMotility.Text = valuePre.ToString();
            //    TxtPreNonMotile.Text = (100 - valuePre).ToString();
            //}

            decimal varpreRapid = 0;
            decimal varpreSlow = 0;
            decimal varpreNon = 0;


            if (TxtPreProgMotility.Text == "" || TxtPreProgMotility.Text == null)
                varpreRapid = 0;
            else
                varpreRapid = Convert.ToDecimal(TxtPreProgMotility.Text.Trim());

            if (TxtPreNonProgressive.Text == "" || TxtPreNonProgressive.Text == null)
                varpreSlow = 0;
            else
                varpreSlow = Convert.ToDecimal(TxtPreNonProgressive.Text.Trim());

            //if (TxtPreGradeIII.Text == "" || TxtPreGradeIII.Text == null)
            //    varpreNon = 0;
            //else
            //    varpreNon = Convert.ToDecimal(TxtPreGradeIII.Text.Trim());

            PreSumOfGrade = (Convert.ToDecimal(varpreRapid)) + (Convert.ToDecimal(varpreSlow)) ;

            TxtPreTotalMotility.Text = PreSumOfGrade.ToString();
            //txtSpermConcentration.Text = SumOfGrade.ToString();

            TxtPreNonMotile.Text = (100 - PreSumOfGrade).ToString();

        }

        private void TxtPostProgMotility_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (TxtPostProgMotility.Text.Trim() != null && TxtPostProgMotility.Text.Trim() != "" && TxtPostNonProgressive.Text.Trim() != null && TxtPostNonProgressive.Text.Trim() != "")
            //{
            //    double valuePost = (Convert.ToDouble(TxtPostProgMotility.Text) + Convert.ToDouble(TxtPostNonProgressive.Text));
            //    TxtPostTotalMotility.Text = valuePost.ToString();
            //    TxtPostNonMotile.Text = (100 - valuePost).ToString();
            //}

            decimal varpreRapid = 0;
            decimal varpreSlow = 0;
            decimal varpreNon = 0;


            if (TxtPostProgMotility.Text == "" || TxtPostProgMotility.Text == null)
                varpreRapid = 0;
            else
                varpreRapid = Convert.ToDecimal(TxtPostProgMotility.Text.Trim());

            if (TxtPostNonProgressive.Text == "" || TxtPostNonProgressive.Text == null)
                varpreSlow = 0;
            else
                varpreSlow = Convert.ToDecimal(TxtPostNonProgressive.Text.Trim());

            //if (TxtPreGradeIII.Text == "" || TxtPreGradeIII.Text == null)
            //    varpreNon = 0;
            //else
            //    varpreNon = Convert.ToDecimal(TxtPreGradeIII.Text.Trim());

            PreSumOfGrade = (Convert.ToDecimal(varpreRapid)) + (Convert.ToDecimal(varpreSlow));

            TxtPostTotalMotility.Text = PreSumOfGrade.ToString();
            //txtSpermConcentration.Text = SumOfGrade.ToString();

            TxtPostNonMotile.Text = (100 - PreSumOfGrade).ToString();
        }

        private void TxtPreNonProgressive_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void TxtPostNonProgressive_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void TxtPreNonMotile_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void TxtPostNonMotile_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void rbtOutSideCentre_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rbtYes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rbtNo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rbtExisting_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rbtNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rbtNormal_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rbtViscous_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rbtNotPresent_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rbtPresent_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rbtCentre_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdLinkDonor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void acc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AccordionItem_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
}

