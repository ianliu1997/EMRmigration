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
using CIMS;
using PalashDynamics.UserControls;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Browser;
using System.IO;
using MessageBoxControl;
using System.Windows.Media.Imaging;
using DataDrivenApplication.Forms;
using PalashDynamics.IVF.PatientList;

namespace PalashDynamics.IVF
{
    public partial class OnlyET : UserControl, IInitiateCIMS
    {
        #region Variables
        WaitIndicator wait = new WaitIndicator();
        public bool IsPatientExist;
        byte[] AttachedFileContents;
        string AttachedFileName;
        System.Collections.ObjectModel.ObservableCollection<string> AttachedFileNameList = new System.Collections.ObjectModel.ObservableCollection<string>();
        ListBox lstFUBox;
        #endregion

        #region Properties
        public bool IsExpendedWindow { get; set; }
        public string Impression { get; set; }
        public Boolean IsEdit { get; set; }
        public Int64 ID { get; set; }
        public Int64 UnitID { get; set; }

        private ObservableCollection<ETDetailsVO> _ETDetails = new ObservableCollection<ETDetailsVO>();
        public ObservableCollection<ETDetailsVO> ETDetails
        {
            get { return _ETDetails; }
            set { _ETDetails = value; }
        }

        private ObservableCollection<ETDetailsVO> _RemoveETDetails = new ObservableCollection<ETDetailsVO>();
        public ObservableCollection<ETDetailsVO> RemoveETDetails
        {
            get { return _RemoveETDetails; }
            set { _RemoveETDetails = value; }
        }

        private List<FileUpload> _FileUpLoadList = new List<FileUpload>();
        public List<FileUpload> FileUpLoadList
        {
            get
            {
                return _FileUpLoadList;
            }
            set
            {
                _FileUpLoadList = value;
            }
        }

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

        private List<MasterListItem> _Plan = new List<MasterListItem>();
        public List<MasterListItem> Plan
        {
            get
            {
                return _Plan;
            }
            set
            {
                _Plan = value;
            }
        }
        #endregion  

        #region Validation
        public Boolean Validation()
        {
            if (dtETDate.SelectedDate == null)
            {
                dtETDate.SetValidation("Please Select ET Date");
                dtETDate.RaiseValidationError();
                dtETDate.Focus();
                return false;
            }
            else if (txtTime.Value == null)
            {
                dtETDate.ClearValidationError();
                txtTime.SetValidation("Please Select ET Time");
                txtTime.RaiseValidationError();
                txtTime.Focus();
                return false;
            }
            else if (cmbEmbryologist.SelectedItem == null || ((MasterListItem)cmbEmbryologist.SelectedItem).ID == 0)
            {
                dtETDate.ClearValidationError();
                txtTime.ClearValidationError();
                cmbEmbryologist.TextBox.SetValidation("Please Select Embryologist");
                cmbEmbryologist.TextBox.RaiseValidationError();
                cmbEmbryologist.Focus();
                return false;
            }
            else if (cmbAssistantEmbryologist.SelectedItem == null || ((MasterListItem)cmbAssistantEmbryologist.SelectedItem).ID == 0)
            {
                dtETDate.ClearValidationError();
                txtTime.ClearValidationError();
                cmbEmbryologist.TextBox.ClearValidationError();
                cmbAssistantEmbryologist.TextBox.SetValidation("Please Select Assistant Embryologist");
                cmbAssistantEmbryologist.TextBox.RaiseValidationError();
                cmbAssistantEmbryologist.Focus();
                return false;
            }

            else if (cmbAnesthetist.SelectedItem == null || ((MasterListItem)cmbAnesthetist.SelectedItem).ID == 0)
            {
                dtETDate.ClearValidationError();
                txtTime.ClearValidationError();
                cmbEmbryologist.TextBox.ClearValidationError();
                cmbAssistantEmbryologist.TextBox.ClearValidationError();
                cmbAnesthetist.TextBox.SetValidation("Please Select Anesthetist");
                cmbAnesthetist.TextBox.RaiseValidationError();
                cmbAnesthetist.Focus();
                return false;
            }
            else if (cmbAssistantAnesthetist.SelectedItem == null || ((MasterListItem)cmbAssistantAnesthetist.SelectedItem).ID == 0)
            {
                dtETDate.ClearValidationError();
                txtTime.ClearValidationError();
                cmbEmbryologist.TextBox.ClearValidationError();
                cmbAssistantEmbryologist.TextBox.ClearValidationError();
                cmbAnesthetist.TextBox.ClearValidationError();
                cmbAssistantAnesthetist.TextBox.SetValidation("Please Select Assistant Anesthetist");
                cmbAssistantAnesthetist.TextBox.RaiseValidationError();
                cmbAssistantAnesthetist.Focus();
                return false;
            }


            else if (ETDetails.Count <= 0)
            {
                dtETDate.ClearValidationError();
                txtTime.ClearValidationError();
                cmbEmbryologist.TextBox.ClearValidationError();
                cmbAssistantEmbryologist.TextBox.ClearValidationError();
                cmbAnesthetist.TextBox.ClearValidationError();
                cmbAssistantAnesthetist.TextBox.ClearValidationError();

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "ET Details Data Grid Cannot Be Empty", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                return false;
            }
            else if (cmbCatheterType.SelectedItem == null || ((MasterListItem)cmbCatheterType.SelectedItem).ID == 0)
            {
                dtETDate.ClearValidationError();
                txtTime.ClearValidationError();
                cmbEmbryologist.TextBox.ClearValidationError();
                cmbAssistantEmbryologist.TextBox.ClearValidationError();
                cmbAnesthetist.TextBox.ClearValidationError();
                cmbAssistantAnesthetist.TextBox.ClearValidationError();
                cmbCatheterType.TextBox.SetValidation("Please Select Catheter Type");
                cmbCatheterType.TextBox.RaiseValidationError();
                cmbCatheterType.Focus();

                return false;
            }
            else if ((ChkIsDiificulty.IsChecked == true) && (cmbDifficultyType.SelectedItem == null || ((MasterListItem)cmbDifficultyType.SelectedItem).ID == 0))
            {
                dtETDate.ClearValidationError();
                txtTime.ClearValidationError();
                cmbEmbryologist.TextBox.ClearValidationError();
                cmbAssistantEmbryologist.TextBox.ClearValidationError();
                cmbAnesthetist.TextBox.ClearValidationError();
                cmbAssistantAnesthetist.TextBox.ClearValidationError();
                cmbCatheterType.TextBox.ClearValidationError();
                cmbDifficultyType.TextBox.SetValidation("Please Select Difficulty Type");
                cmbDifficultyType.TextBox.RaiseValidationError();
                cmbDifficultyType.Focus();

                return false;
            }
            else
            {
                dtETDate.ClearValidationError();
                txtTime.ClearValidationError();
                cmbEmbryologist.TextBox.ClearValidationError();
                cmbAssistantEmbryologist.TextBox.ClearValidationError();
                cmbAnesthetist.TextBox.ClearValidationError();
                cmbAssistantAnesthetist.TextBox.ClearValidationError();
                cmbCatheterType.TextBox.ClearValidationError();
                cmbCatheterType.ClearValidationError();
                return true;
            }

        }
        #endregion

        #region Constructor

        public OnlyET()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(OnlyET_Loaded);
            this.Unloaded += new RoutedEventHandler(OnlyET_Unloaded);
        }

        #endregion

        #region Loaded Event
        void OnlyET_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsExpendedWindow)
            {
                try
                {
                    if (IsPatientExist == false)
                    {
                        ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                        ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                    }
                    else
                    {
                        fillSemenSource();
                        fillOocyteSource();
                        fillDoctor();
                        fillPattern();
                        IsExpendedWindow = true;
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        #endregion 

        #region Unload Event

        void OnlyET_Unloaded(object sender, RoutedEventArgs e)
        {
            try
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
            catch (Exception Exception) { }
        }

        #endregion

        private void fillSemenSource()
        {
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
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    cmbSrcSemen.ItemsSource = null;
                    cmbSrcSemen.ItemsSource = objList;
                    cmbSrcSemen.SelectedValue = objList[0]; ;

                    //if (this.DataContext != null)
                    //{
                    //    cmbSrcSemen.SelectedValue = ((clsFemaleLabDay0VO)this.DataContext).SrcOfSemenID;
                    //}
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void fillOocyteSource()
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
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    cmbSrcOocyte.ItemsSource = null;
                    cmbSrcOocyte.ItemsSource = objList;
                    cmbSrcOocyte.SelectedValue = objList[0];

                    //if (this.DataContext != null)
                    //{
                    //    cmbSrcOocyte.SelectedValue = ((clsFemaleLabDay0VO)this.DataContext).SrcOfOocyteID;
                    //}
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

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

        #region Fill Master Items

        private void fillDoctor()
        {
            try
            {
                wait.Show();
                clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
                BizAction.MasterList = new List<MasterListItem>();
                BizAction.UnitId = 0;
                BizAction.DepartmentId = 0;
                BizAction.SpecializationID = 0;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);

                        cmbEmbryologist.ItemsSource = null;
                        cmbEmbryologist.ItemsSource = objList;
                        cmbEmbryologist.SelectedValue = (long)0;


                        cmbAnesthetist.ItemsSource = null;
                        cmbAnesthetist.ItemsSource = objList;
                        cmbAnesthetist.SelectedValue = (long)0;

                        cmbAssistantAnesthetist.ItemsSource = null;
                        cmbAssistantAnesthetist.ItemsSource = objList;
                        cmbAssistantAnesthetist.SelectedValue = (long)0;

                        cmbAssistantEmbryologist.ItemsSource = null;
                        cmbAssistantEmbryologist.ItemsSource = objList;
                        cmbAssistantEmbryologist.SelectedValue = (long)0;

                        if (this.DataContext != null)
                        {
                            cmbEmbryologist.SelectedValue = ((clsFemaleLabDay0VO)this.DataContext).EmbryologistID;
                            cmbAnesthetist.SelectedValue = ((clsFemaleLabDay0VO)this.DataContext).AnesthetistID;
                        }
                        fillCatheterType();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
            }
        }

        private void fillCatheterType()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_CatheterTypeMaster;
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

                    cmbCatheterType.ItemsSource = null;
                    cmbCatheterType.ItemsSource = objList;
                    cmbCatheterType.SelectedValue = (long)0;

                    if (this.DataContext != null)
                    {
                        cmbCatheterType.SelectedValue = ((clsEmbryoTransferVO)this.DataContext).CatheterTypeID;
                    }
                    fillDifficultyType();
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void fillDifficultyType()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_DifficultyTypeMaster;
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

                    cmbDifficultyType.ItemsSource = null;
                    cmbDifficultyType.ItemsSource = objList;
                    cmbDifficultyType.SelectedValue = (long)0;

                    if (this.DataContext != null)
                    {
                        cmbDifficultyType.SelectedValue = ((clsEmbryoTransferVO)this.DataContext).DifficultyTypeID;
                    }
                    fillGrade();
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem(0, "--Select--"));
                        Grade = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                    }
                    if (this.DataContext != null)
                    {
                        //cmbSrcTreatmentPlan.SelectedValue = ((clsVO)this.DataContext).;
                    }
                    FillCellStage();
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem(0, "--Select--"));
                        CellStage = ((clsGetMasterListBizActionVO)args.Result).MasterList;
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
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_PlanMaster;
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
                    Plan = null;
                    Plan = objList;
                }
                fillCoupleDetails();
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        /*Added By Sudhir on 13/09/2013 As Mr. Bhushan told */
        private void fillPattern()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_ETPattern;
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
                    cmbPattern.ItemsSource = null;
                    cmbPattern.ItemsSource = objList;
                    cmbPattern.SelectedValue = (long)0;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        #endregion

        #region  Fill ET Details

        public void fillOnlyETDetails()
        {
            try
            {
                clsGetETDetailsBizActionVO BizAction = new clsGetETDetailsBizActionVO();
                BizAction.ET = new ETVO();
                BizAction.IsEdit = IsEdit;
                BizAction.ID = ID;
                BizAction.UnitID = UnitID;
                BizAction.FromID = (int)IVFLabWorkForm.OnlyET;
                if (CoupleDetails != null)
                {
                    BizAction.CoupleID = CoupleDetails.CoupleId;
                    BizAction.CoupleUintID = CoupleDetails.CoupleUnitId;
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (IsEdit == true)
                        {
                            dtETDate.SelectedDate = ((clsGetETDetailsBizActionVO)arg.Result).ET.Date;
                            txtTime.Value = ((clsGetETDetailsBizActionVO)arg.Result).ET.Date;
                            cmbAnesthetist.SelectedValue = (long)((clsGetETDetailsBizActionVO)arg.Result).ET.AnasthesistID;
                            cmbAssistantAnesthetist.SelectedValue = ((clsGetETDetailsBizActionVO)arg.Result).ET.AssAnasthesistID;
                            cmbEmbryologist.SelectedValue = (long)((clsGetETDetailsBizActionVO)arg.Result).ET.EmbryologistID;
                            cmbAssistantEmbryologist.SelectedValue = ((clsGetETDetailsBizActionVO)arg.Result).ET.AssEmbryologistID;

                            /* Added By Sudhir on 13/09/2013 */
                            txtThickNess.Text = ((clsGetETDetailsBizActionVO)arg.Result).ET.EndometriumThickness.ToString();
                            cmbPattern.SelectedValue = ((clsGetETDetailsBizActionVO)arg.Result).ET.ETPattern;
                          //  txtClrDoppler.Text = ((clsGetETDetailsBizActionVO)arg.Result).ET.ColorDopper;


                            //By Anjali
                            chkUterinePI.IsChecked = ((clsGetETDetailsBizActionVO)arg.Result).ET.IsUterinePI;
                            chkUterineRI.IsChecked = ((clsGetETDetailsBizActionVO)arg.Result).ET.IsUterineRI;
                            chkUterineSD.IsChecked = ((clsGetETDetailsBizActionVO)arg.Result).ET.IsUterineSD;
                            chkEndometerialPI.IsChecked = ((clsGetETDetailsBizActionVO)arg.Result).ET.IsEndometerialPI;
                            chkEndometerialRI.IsChecked = ((clsGetETDetailsBizActionVO)arg.Result).ET.IsEndometerialRI;
                            chkEndometerialSD.IsChecked = ((clsGetETDetailsBizActionVO)arg.Result).ET.IsEndometerialSD;
                            txtDistanceFromFundus.Text = Convert.ToString(((clsGetETDetailsBizActionVO)arg.Result).ET.DistanceFromFundus);

                            // BY bHUSHAN .
                            cmbSrcOocyte.SelectedValue = ((clsGetETDetailsBizActionVO)arg.Result).ET.SrcOoctyID;
                            cmbSrcSemen.SelectedValue = ((clsGetETDetailsBizActionVO)arg.Result).ET.SrcSemenID;
                            txtOocyteDonorCode.Text = ((clsGetETDetailsBizActionVO)arg.Result).ET.SrcOoctyCode;
                            txtSemenDonorCode.Text = ((clsGetETDetailsBizActionVO)arg.Result).ET.SrcSemenCode;

                            cmbCatheterType.SelectedValue = ((clsGetETDetailsBizActionVO)arg.Result).ET.CatheterTypeID;
                            cmbDifficultyType.SelectedValue = ((clsGetETDetailsBizActionVO)arg.Result).ET.DifficultyTypeID;
                            ChkIsDiificulty.IsChecked = ((clsGetETDetailsBizActionVO)arg.Result).ET.Difficulty;
                            if (ChkIsDiificulty.IsChecked == true)
                                cmbDifficultyType.Visibility = Visibility.Visible;
                            chkTreatment.IsChecked = ((clsGetETDetailsBizActionVO)arg.Result).ET.IsTreatmentUnderGA;
                            chkIsFreezed.IsChecked = ((clsGetETDetailsBizActionVO)arg.Result).ET.IsFreezed;
                            if (chkIsFreezed.IsChecked == true)
                            {
                                CmdSave.IsEnabled = false;
                            }
                        }
                        FileUpLoadList = ((clsGetETDetailsBizActionVO)arg.Result).ET.FUSetting;                        
                        for (int i = 0; i < ((clsGetETDetailsBizActionVO)arg.Result).ET.ETDetails.Count; i++)
                        {
                            ((clsGetETDetailsBizActionVO)arg.Result).ET.ETDetails[i].FertilizationList = CellStage;
                            ((clsGetETDetailsBizActionVO)arg.Result).ET.ETDetails[i].GradeList = Grade;
                            ((clsGetETDetailsBizActionVO)arg.Result).ET.ETDetails[i].PlanList = Plan;
                            if (!string.IsNullOrEmpty(((clsGetETDetailsBizActionVO)arg.Result).ET.ETDetails[i].Grade))
                            {
                                if (Convert.ToInt64(((clsGetETDetailsBizActionVO)arg.Result).ET.ETDetails[i].Grade) > 0)
                                {
                                    ((clsGetETDetailsBizActionVO)arg.Result).ET.ETDetails[i].SelectedGrade = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetETDetailsBizActionVO)arg.Result).ET.ETDetails[i].Grade));
                                }
                                else
                                {
                                    ((clsGetETDetailsBizActionVO)arg.Result).ET.ETDetails[i].SelectedGrade = Grade.FirstOrDefault(p => p.ID == 0);
                                }
                            }
                            if (!string.IsNullOrEmpty(((clsGetETDetailsBizActionVO)arg.Result).ET.ETDetails[i].FertilizationStage))
                            {
                                if(Convert.ToInt64(((clsGetETDetailsBizActionVO)arg.Result).ET.ETDetails[i].FertilizationStage)>0)
                                {
                                    ((clsGetETDetailsBizActionVO)arg.Result).ET.ETDetails[i].SelectedFertilizationStage = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetETDetailsBizActionVO)arg.Result).ET.ETDetails[i].FertilizationStage));
                                }
                                else
                                {
                                    ((clsGetETDetailsBizActionVO)arg.Result).ET.ETDetails[i].SelectedFertilizationStage = CellStage.FirstOrDefault(p => p.ID == 0);
                                }
                            }
                            if (((clsGetETDetailsBizActionVO)arg.Result).ET.ETDetails[i].PlanID>=0)
                            {
                                ((clsGetETDetailsBizActionVO)arg.Result).ET.ETDetails[i].SelectedPlan = Plan.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetETDetailsBizActionVO)arg.Result).ET.ETDetails[i].PlanID));
                            }
                            else
                            {
                                ((clsGetETDetailsBizActionVO)arg.Result).ET.ETDetails[i].SelectedPlan = Plan.FirstOrDefault(p => p.ID == 0);
                            }

                            ETDetails.Add(((clsGetETDetailsBizActionVO)arg.Result).ET.ETDetails[i]);
                           
                            wait.Close();
                        }
                        dgETDetilsGrid.ItemsSource = ETDetails;
                        LoadFURepeaterControl();
                        wait.Close();
                    }
                    wait.Close();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        public void fillInitailOnlyETDetails()
        {
            ETDetails.Add(new ETDetailsVO() { EmbNO = "Auto Generated", FertilizationStage = "0", FertilizationStageID = 0, Grade = "0", GradeID = 0, FertilizationList = CellStage, GradeList = Grade, PlanList = Plan, SerialOccyteNo = "Auto Generated" });
            dgETDetilsGrid.ItemsSource = ETDetails;
            LoadFURepeaterControl();
            wait.Close();
        }

        #endregion

        #region Fill Couple Details

        //Added by Saily P on 25.09.12 to get the patient details in case of Donor
        private void LoadPatientHeader()
        {
            if ((((IApplicationConfiguration)App.Current).SelectedPatient).Gender == Genders.Female.ToString())
            {
                wait.Close();
                clsGetPatientBizActionVO BizAction = new PalashDynamics.ValueObjects.Patient.clsGetPatientBizActionVO();
                BizAction.PatientDetails = new PalashDynamics.ValueObjects.Patient.clsPatientVO();
                BizAction.PatientDetails.GeneralDetails = (clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        BizAction.PatientDetails.GeneralDetails = ((clsGetPatientBizActionVO)args.Result).PatientDetails.GeneralDetails;
                        BizAction.PatientDetails.SpouseDetails = ((clsGetPatientBizActionVO)args.Result).PatientDetails.SpouseDetails;
                        PatientInfo.Visibility = Visibility.Visible;
                        CoupleInfo.Visibility = Visibility.Collapsed;
                        Patient.DataContext = BizAction.PatientDetails.GeneralDetails;
                        #region Commented by Saily P on 25.09.12
                        //if (BizAction.PatientDetails.GenderID == 1)
                        //{
                        //    Male.DataContext = BizAction.PatientDetails.GeneralDetails;
                        //    if (BizAction.PatientDetails.SpouseDetails != null && BizAction.PatientDetails.SpouseDetails.ID != 0)
                        //    {
                        //        Female.DataContext = BizAction.PatientDetails.SpouseDetails;
                        //        CoupleInfo.Visibility = Visibility.Visible;
                        //        PatientInfo.Visibility = Visibility.Collapsed;
                        //    }
                        //    else
                        //    {
                        //        PatientInfo.Visibility = Visibility.Visible;
                        //        CoupleInfo.Visibility = Visibility.Collapsed;
                        //        Patient.DataContext = BizAction.PatientDetails.GeneralDetails;
                        //    }
                        //}
                        //else
                        //{
                        //    Female.DataContext = BizAction.PatientDetails.GeneralDetails;
                        //    if (BizAction.PatientDetails.SpouseDetails != null && BizAction.PatientDetails.SpouseDetails.ID != 0)
                        //    {
                        //        Male.DataContext = BizAction.PatientDetails.SpouseDetails;
                        //        CoupleInfo.Visibility = Visibility.Visible;
                        //        PatientInfo.Visibility = Visibility.Collapsed;
                        //    }
                        //    else
                        //    {
                        //        Patient.DataContext = BizAction.PatientDetails.GeneralDetails;
                        //        PatientInfo.Visibility = Visibility.Visible;
                        //        CoupleInfo.Visibility = Visibility.Collapsed;
                        //    }
                        //}
                        #endregion

                        if (IsEdit == true)
                        {
                            fillOnlyETDetails();
                        }
                        else
                        {
                            fillInitailOnlyETDetails();
                        }

                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            else
            {
                wait.Close();
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Only ET is Only For Female.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            }
        }

        private void fillCoupleDetails()
        {
            if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId == 0)
            {
                LoadPatientHeader();              
                //wait.Close();
                //MessageBoxControl.MessageBoxChildWindow msgW1 =
                //     new MessageBoxControl.MessageBoxChildWindow("Palash", "Only ET is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //msgW1.Show();
                //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            }
            else
            {
                clsGetCoupleDetailsBizActionVO BizAction = new clsGetCoupleDetailsBizActionVO();
                BizAction.PatientID = ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
                BizAction.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
                BizAction.CoupleDetails = new clsCoupleVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        BizAction.CoupleDetails.MalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient;
                        BizAction.CoupleDetails.FemalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient;
                        CoupleDetails.MalePatient = new clsPatientGeneralVO();
                        CoupleDetails.FemalePatient = new clsPatientGeneralVO();
                        CoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails;

                        if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails != null)
                        {
                            if (CoupleDetails.CoupleId == 0)
                            {
                                LoadPatientHeader();
                                //MessageBoxControl.MessageBoxChildWindow msgW1 =
                                //     new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot Create Therapy, Therapy is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                //msgW1.Show();
                                //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                            }
                            else
                            {
                                PatientInfo.Visibility = Visibility.Collapsed;
                                CoupleInfo.Visibility = Visibility.Visible;
                                //getEMRDetails(BizAction.CoupleDetails.FemalePatient, "F");
                                //getEMRDetails(BizAction.CoupleDetails.MalePatient, "M");
                                GetHeightAndWeight(BizAction.CoupleDetails);
                                //added by priti
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
                                //Female.DataContext = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient;
                                //Male.DataContext = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient;
                                if (IsEdit == true)
                                {
                                    fillOnlyETDetails();
                                }
                                else
                                {
                                    fillInitailOnlyETDetails();
                                }
                            }
                        }
                        else
                            LoadPatientHeader();
                       
                    }
                    wait.Close();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
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
                        //FemalePatientDetails.BMI = BizAction.CoupleDetails.FemalePatient.BMI;
                        FemalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", BizAction.CoupleDetails.FemalePatient.BMI));
                        FemalePatientDetails.Alerts = BizAction.CoupleDetails.FemalePatient.Alerts;
                        Female.DataContext = FemalePatientDetails;

                        clsPatientGeneralVO MalePatientDetails = new clsPatientGeneralVO();
                        MalePatientDetails = CoupleDetails.MalePatient;
                        MalePatientDetails.Height = BizAction.CoupleDetails.MalePatient.Height;
                        MalePatientDetails.Weight = BizAction.CoupleDetails.MalePatient.Weight;
                        //MalePatientDetails.BMI = BizAction.CoupleDetails.MalePatient.BMI;
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
                                Female.DataContext = PatientDetails;
                            }
                            else
                            {
                                PatientDetails.Height = height;
                                PatientDetails.Weight = weight;
                                PatientDetails.BMI = Math.Round(CalculateBMI(height, weight), 2);
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

        #region Save Event
        int j = 0;
        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                try
                {
                    string msgTitle = "Palash";
                    string msgText = "Are You Sure You Want To Save Only ET Details";
                    MessageBoxControl.MessageBoxChildWindow msgWin =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWin.OnMessageBoxClosed += (result) =>
                    {
                        if (result == MessageBoxResult.Yes)
                        {
                            ImpressionWindow winImp = new ImpressionWindow();
                            winImp.Day = true;
                            winImp.Impression = Impression;
                            winImp.OnSaveClick += new RoutedEventHandler(winImp_OnSaveClick);
                            winImp.Show();
                        }
                    };
                    msgWin.Show();
                }
                catch (Exception ex)
                {

                }
            }
        }
        private void SaveVitrification()
        {
            try
            {
                wait.Show();

                clsAddUpdateETDetailsBizActionVO BizAction = new clsAddUpdateETDetailsBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                BizAction.ET = new ETVO();
                BizAction.ET.IsOnlyET = true;
                BizAction.ET.Impression = Impression;
                BizAction.ID = ID;
                BizAction.UintID = UnitID;
                BizAction.CoupleID = CoupleDetails.CoupleId;
                BizAction.CoupleUintID = CoupleDetails.CoupleUnitId;

                if (ChkIsDiificulty.IsChecked == true)
                {
                    BizAction.ET.Difficulty = true;
                    BizAction.ET.DifficultyTypeID = ((MasterListItem)cmbDifficultyType.SelectedItem).ID;
                }
                else
                {
                    BizAction.ET.Difficulty = false;
                }
                BizAction.ET.Date = dtETDate.SelectedDate.Value;
                BizAction.ET.Date = BizAction.ET.Date.Value.Add(txtTime.Value.Value.TimeOfDay);
                BizAction.ET.IsFreezed = (bool)chkIsFreezed.IsChecked;
                BizAction.ET.AnasthesistID = ((MasterListItem)cmbAnesthetist.SelectedItem).ID;
                BizAction.ET.AssAnasthesistID = ((MasterListItem)cmbAssistantAnesthetist.SelectedItem).ID;
                BizAction.ET.AssEmbryologistID = ((MasterListItem)cmbAssistantEmbryologist.SelectedItem).ID;
                BizAction.ET.EmbryologistID = ((MasterListItem)cmbEmbryologist.SelectedItem).ID;

                /* Added By Sudhir on 13/09/2013 */
                BizAction.ET.ETPattern =  ((MasterListItem)cmbPattern.SelectedItem).ID;
                BizAction.ET.EndometriumThickness = Convert.ToInt64(txtThickNess.Text);
             // BizAction.ET.ColorDopper = Convert.ToString(txtClrDoppler.Text);

                //By Anjali
                BizAction.ET.IsEndometerialPI =(bool)chkEndometerialPI.IsChecked;
                BizAction.ET.IsEndometerialRI = (bool)chkEndometerialRI.IsChecked;
                BizAction.ET.IsEndometerialSD = (bool)chkEndometerialSD.IsChecked;
                BizAction.ET.IsUterinePI = (bool)chkUterinePI.IsChecked;
                BizAction.ET.IsUterineRI = (bool)chkUterineRI.IsChecked;
                BizAction.ET.IsUterineSD = (bool)chkUterineSD.IsChecked;
                BizAction.ET.DistanceFromFundus = txtDistanceFromFundus.Text;

                // By BHUSHAN . . . . 
                BizAction.ET.SrcSemenCode = txtSemenDonorCode.Text;
                if (cmbSrcSemen.SelectedItem != null)
                {
                    BizAction.ET.SrcSemenID = ((MasterListItem)cmbSrcSemen.SelectedItem).ID;
                }
                BizAction.ET.SrcOoctyCode = txtOocyteDonorCode.Text;
                if (cmbSrcOocyte.SelectedItem != null)
                {
                    BizAction.ET.SrcOoctyID = ((MasterListItem)cmbSrcOocyte.SelectedItem).ID;
                }

                BizAction.ET.CatheterTypeID = ((MasterListItem)cmbCatheterType.SelectedItem).ID;
                BizAction.ET.IsTreatmentUnderGA = (bool)chkTreatment.IsChecked;

                for (int i = 0; i < ETDetails.Count; i++)
                {
                   
                    ETDetails[i].FertilizationStageID = ETDetails[i].SelectedFertilizationStage.ID;
                    ETDetails[i].GradeID = ETDetails[i].SelectedGrade.ID;
                    ETDetails[i].PlanID = ETDetails[i].SelectedPlan.ID;
                   // ETDetails[i].TransferDate = ETDetails[i].TransferDate;
                    if (ETDetails[i].EmbNO.Equals("Auto Generated"))
                    {
                        ETDetails[i].EmbNO = "0";
                    }
                    //By Anjali............
                    j = i + 1;
                    ETDetails[i].SerialOccyteNo = Convert.ToString(j);
                    //................................
                }
                for (int i = 0; i < RemoveETDetails.Count; i++)
                {
                    ETDetails.Add(RemoveETDetails[i]);
                }

                BizAction.ET.ETDetails = ((List<ETDetailsVO>)ETDetails.ToList());
                BizAction.ET.FUSetting = FileUpLoadList;

                #region Service Call (Check Validation)

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        string txtmsg = "";
                        if (IsEdit == true)
                        {
                            txtmsg = "Only ET Details Updated Successfully";
                        }
                        else
                        {
                            txtmsg = "Only ET Details Saved Successfully";
                        }

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", txtmsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        LabDaysSummary LabSumm = new LabDaysSummary();
                        LabSumm.IsPatientExist = true;
                        ((IApplicationConfiguration)App.Current).OpenMainContent(LabSumm);
                        wait.Close();

                    }
                    else
                    {
                        wait.Close();
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

        void winImp_OnSaveClick(object sender, RoutedEventArgs e)
        {
            ImpressionWindow ObjImp = (ImpressionWindow)sender;
            Impression = ObjImp.Impression;
            SaveVitrification();
        }
        #endregion

        #region Cancel Event
        private void CmdCancel_Click(object sender, RoutedEventArgs e)
        {
            LabDaysSummary LabSumm = new LabDaysSummary();
            LabSumm.IsPatientExist = true;
            ((IApplicationConfiguration)App.Current).OpenMainContent(LabSumm);
        }
        #endregion
        
        #region Checkbox Click Event
        private void ChkIsDiificulty_Click(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                cmbDifficultyType.Visibility = Visibility.Visible;
            }
            else
            {
                cmbDifficultyType.Visibility = Visibility.Collapsed;
            }
        }
        #endregion

        #region Add and Remove Only Vitrification

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (ETDetails == null)
            {
                ETDetails = new ObservableCollection<ETDetailsVO>();
            }
            ETDetails.Add(new ETDetailsVO() { EmbNO = "Auto Generated", FertilizationStage = "0", FertilizationStageID = 0, Grade = "0", GradeID = 0, FertilizationList = CellStage, GradeList = Grade, PlanList = Plan, SerialOccyteNo = "Auto Generated" });
           
            dgETDetilsGrid.ItemsSource = ETDetails;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (dgETDetilsGrid.SelectedItem != null)
            {
                if (ETDetails.Count > 1)
                {
                    ETDetails[dgETDetilsGrid.SelectedIndex].EmbNO = "-1";
                    if (ETDetails[dgETDetilsGrid.SelectedIndex].ID != 0 && !ETDetails[dgETDetilsGrid.SelectedIndex].EmbNO.Equals("Auto Generated"))
                    {
                        RemoveETDetails.Add(ETDetails[dgETDetilsGrid.SelectedIndex]);
                    }
                    ETDetails.RemoveAt(dgETDetilsGrid.SelectedIndex);
                }
            }

            dgETDetilsGrid.ItemsSource = ETDetails;
        }

        #endregion

        #region Grade Selection Changed Event
        private void cmbGrade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             if (((AutoCompleteBox)sender).Name.Equals("cmbCellStage"))
            {
                for (int i = 0; i < ETDetails.Count; i++)
                {
                    if (ETDetails[i] == ((ETDetailsVO)dgETDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            ETDetails[i].FertilizationStageID = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbGrade"))
            {
                for (int i = 0; i < ETDetails.Count; i++)
                {
                    if (ETDetails[i] == ((ETDetailsVO)dgETDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            ETDetails[i].GradeID = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
             else if (((AutoCompleteBox)sender).Name.Equals("cmbPlan"))
             {
                 for (int i = 0; i < ETDetails.Count; i++)
                 {
                     if (ETDetails[i] == ((ETDetailsVO)dgETDetilsGrid.SelectedItem))
                     {
                         if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                         {
                             ETDetails[i].PlanID = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                         }
                     }
                 }
             }
        }
        #endregion
        
        #region File Attachment Events
        private void CmdBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() == true)
            {
                AttachedFileName = openDialog.File.Name;
                ETDetails[dgETDetilsGrid.SelectedIndex].FileName=openDialog.File.Name;
               
                try
                {
                    using (Stream stream = openDialog.File.OpenRead())
                    {
                        AttachedFileContents = new byte[stream.Length];
                        stream.Read(AttachedFileContents, 0, (int)stream.Length);
                        ETDetails[dgETDetilsGrid.SelectedIndex].FileContents = AttachedFileContents;
                    }
                    
                }
                catch (Exception ex)
                {
                    string msgText = "Error While Reading File.";

                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow("Palsh", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
            }
        }
        
        private void hprlnkViewFile_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((ETDetailsVO)dgETDetilsGrid.SelectedItem).FileName))
            {
                if (((ETDetailsVO)dgETDetilsGrid.SelectedItem).FileContents != null)
                {


                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + ((ETDetailsVO)dgETDetilsGrid.SelectedItem).FileName });
                            AttachedFileNameList.Add(((ETDetailsVO)dgETDetilsGrid.SelectedItem).FileName);
                        }
                    };
                    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", ((ETDetailsVO)dgETDetilsGrid.SelectedItem).FileName, ((ETDetailsVO)dgETDetilsGrid.SelectedItem).FileContents);
                }
            }
        }
        #endregion

        #region Upload File
        private void LoadFURepeaterControl()
        {

            lstFUBox = new ListBox();
            if (FileUpLoadList == null || FileUpLoadList.Count==0)
            {
                FileUpLoadList = new List<FileUpload>();
                FileUpLoadList.Add(new FileUpload());
            }

            lstFUBox.DataContext = FileUpLoadList;


            if (FileUpLoadList != null)
            {
                for (int i = 0; i < FileUpLoadList.Count; i++)
                {
                    FileUploadRepeterControlItem FUrci = new FileUploadRepeterControlItem();
                    FUrci.OnAddRemoveClick += new RoutedEventHandler(FUrci_OnAddRemoveClick);
                    FUrci.OnBrowseClick += new RoutedEventHandler(FUrci_OnBrowseClick);
                    FUrci.OnViewClick += new RoutedEventHandler(FUrci_OnViewClick);

                    FileUpLoadList[i].Index = i;
                    FileUpLoadList[i].Command = ((i == FileUpLoadList.Count - 1) ? "Add" : "Remove");

                    FUrci.DataContext = FileUpLoadList[i];
                    lstFUBox.Items.Add(FUrci);
                }
            }
            Grid.SetRow(lstFUBox, 0);
            Grid.SetColumn(lstFUBox, 0);
            GridUploadFile.Children.Add(lstFUBox);
        }

        void FUrci_OnViewClick(object sender, RoutedEventArgs e)
        {
            if (((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).FileName != null && ((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).FileName != "")
            {
                if (((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).Data != null)
                {
                    string FullFile = "ET" + ((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).Index + ((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).FileName;

                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + FullFile });
                            AttachedFileNameList.Add(FullFile);
                        }
                    };
                    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", FullFile, ((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).Data);
                }
                else
                {

                }
            }
            else
            {
                MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "This File Is Not Uploaded. Please Upload The File Then Click On Preview", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                mgbx.Show();
            }
        }
        void FUrci_OnBrowseClick(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() == true)
            {
                ((ValueObjects.IVFPlanTherapy.FileUpload)((Button)sender).DataContext).FileName = openDialog.File.Name;
                ((TextBox)((Grid)((Button)sender).Parent).FindName("FileName")).Text = openDialog.File.Name;
                try
                {
                    using (Stream stream = openDialog.File.OpenRead())
                    {
                        ((ValueObjects.IVFPlanTherapy.FileUpload)((Button)sender).DataContext).Data = new byte[stream.Length];
                        stream.Read(((ValueObjects.IVFPlanTherapy.FileUpload)((Button)sender).DataContext).Data, 0, (int)stream.Length);

                    }
                }
                catch (Exception ex)
                {
                    string msgText = "Error While Reading File.";

                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow("Palsh", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
            }
        }

        void FUrci_OnAddRemoveClick(object sender, RoutedEventArgs e)
        {
            if (((HyperlinkButton)sender).TargetName.ToString() == "Remove")
            {
                FileUpLoadList.RemoveAt(((ValueObjects.IVFPlanTherapy.FileUpload)((HyperlinkButton)sender).DataContext).Index);
            }
            if (((HyperlinkButton)sender).TargetName.ToString() == "Add")
            {
                if (FileUpLoadList.Where(Items => Items.FileName == null).Any() == true)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW13 =
                                                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select File.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW13.Show();


                }
                else
                {
                    FileUpLoadList.Add(new ValueObjects.IVFPlanTherapy.FileUpload());
                }
            }
            lstFUBox.Items.Clear();
            for (int i = 0; i < FileUpLoadList.Count; i++)
            {
                FileUploadRepeterControlItem FUrci = new FileUploadRepeterControlItem();
                FUrci.OnAddRemoveClick += new RoutedEventHandler(FUrci_OnAddRemoveClick);
                FUrci.OnBrowseClick += new RoutedEventHandler(FUrci_OnBrowseClick);
                FUrci.OnViewClick += new RoutedEventHandler(FUrci_OnViewClick);


                FileUpLoadList[i].Index = i;
                FileUpLoadList[i].Command = ((i == FileUpLoadList.Count - 1) ? "Add" : "Remove");

                FUrci.DataContext = FileUpLoadList[i];
                lstFUBox.Items.Add(FUrci);
            }
        }
        #endregion

        #region Other Clicked and Selection Events

        private void HyperlinkButton_Click_Male(object sender, RoutedEventArgs e)
        {
            if (MaleAlert.Text.Trim().Length > 0)
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

        private void cmbPattern_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dtETDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        #endregion

        private void cmbSrcSemen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSrcSemen.SelectedItem != null)
            {
                if (((MasterListItem)cmbSrcSemen.SelectedItem).ID == 0)
                {
                    btnSearchSemenDonor.IsEnabled = false;
                    txtSemenDonorCode.Text = "";
                    txtSemenDonorCode.IsEnabled = false;
                    //By Anjali
                    //   txtSemenDonorID.IsEnabled = false;
                }
                else if (((MasterListItem)cmbSrcSemen.SelectedItem).ID == 1)
                {
                    btnSearchSemenDonor.IsEnabled = false;
                    txtSemenDonorCode.Text = "";
                    txtSemenDonorCode.IsEnabled = false;
                    //By Anjali
                    //    txtSemenDonorID.IsEnabled = false;
                }
                else
                {
                    btnSearchSemenDonor.IsEnabled = true;
                    //  txtSemenDonorID.IsEnabled = true;
                    txtSemenDonorCode.IsEnabled = true;
                }
            }

        }

        private void cmbSrcOocyte_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSrcOocyte.SelectedItem != null)
            {
                if (((MasterListItem)cmbSrcOocyte.SelectedItem).ID == 0)
                {
                    btnSearchOocyteDonor.IsEnabled = false;
                    txtOocyteDonorCode.Text = "";
                    txtOocyteDonorCode.IsEnabled = false;                    
                    //By Anjali
                    // txtOocyteDonorID.IsEnabled = false;

                }
                else if (((MasterListItem)cmbSrcOocyte.SelectedItem).ID == 1)
                {
                    btnSearchOocyteDonor.IsEnabled = false;
                    txtOocyteDonorCode.Text = "";
                    txtOocyteDonorCode.IsEnabled = false;
                    //By Anjali
                    // txtOocyteDonorID.IsEnabled = false;
                }
                else
                {
                    btnSearchOocyteDonor.IsEnabled = true;
                    //  txtOocyteDonorID.IsEnabled = true;
                    txtOocyteDonorCode.IsEnabled = true;
                }
            }
        }

        private void btnSearchOocyteDonor_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch Win = new PatientSearch();
            Win.OnSaveButton_Click += new RoutedEventHandler(PatientSearch_OnSaveButton_Click);
            Win.PatientCategoryID = 8;
            Win.Show();

        }

        private void btnSearchSemenDonor_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch Win = new PatientSearch();
            Win.OnSaveButton_Click += new RoutedEventHandler(PatientSearch_OnSaveButton_Click);
            Win.PatientCategoryID = 9;
            Win.Show();
        }

        void PatientSearch_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch ObjWin = (PatientSearch)sender;
            if (ObjWin.DialogResult == true)
            {
                if (ObjWin.PatientCategoryID == 8)
                    txtOocyteDonorCode.Text = ((clsPatientGeneralVO)ObjWin.dataGrid2.SelectedItem).MRNo;
                else
                    txtSemenDonorCode.Text = ((clsPatientGeneralVO)ObjWin.dataGrid2.SelectedItem).MRNo;
            }
        }


    }
}
