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
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.UserControls;
using PalashDynamics.Animations;
using System.Collections.ObjectModel;
using System.Globalization;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Browser;
using MessageBoxControl;
using System.IO;
using PalashDynamics.ValueObjects;
using System.Windows.Media.Imaging;
using DataDrivenApplication.Forms;

namespace PalashDynamics.IVF
{
    public partial class Vitrification : UserControl, IInitiateCIMS
    {
        #region Variable
        WaitIndicator wait = new WaitIndicator();
        public bool IsPatientExist;
        private SwivelAnimation objAnimation;
        System.Collections.ObjectModel.ObservableCollection<string> AttachedFileNameList = new System.Collections.ObjectModel.ObservableCollection<string>();
        ListBox lstFUBox;



        #endregion

        #region Properties
        public bool IsExpendedWindow { get; set; }
        public string Impression { get; set; }
        public Boolean IsEdit { get; set; }
        public Int64 ID { get; set; }
        public Int64 UnitID { get; set; }

        private ObservableCollection<clsGetVitrificationDetailsVO> _VitriDetails = new ObservableCollection<clsGetVitrificationDetailsVO>();
        public ObservableCollection<clsGetVitrificationDetailsVO> VitriDetails
        {
            get { return _VitriDetails; }
            set { _VitriDetails = value; }
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

        // Added by Saily P

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

        #endregion

        #region Validation
        public Boolean Validation()
        {
            //if (string.IsNullOrEmpty(txtVitriNo.Text.Trim()))
            //{
            //    txtVitriNo.SetValidation("Please Enter Vitrification Number");
            //    txtVitriNo.RaiseValidationError();
            //    txtVitriNo.Focus();
            //    return false;
            //}
            //else 
            if (dtVitrificationDate.SelectedDate == null)
            {
                //txtVitriNo.ClearValidationError();
                dtVitrificationDate.SetValidation("Please Select Vitrification Date");
                dtVitrificationDate.RaiseValidationError();
                dtVitrificationDate.Focus();
                return false;
            }
            else if (txtTime.Value == null)
            {
                //txtVitriNo.ClearValidationError();
                dtVitrificationDate.ClearValidationError();
                txtTime.SetValidation("Please Select Vitrification Time");
                txtTime.RaiseValidationError();
                txtTime.Focus();
                return false;
            }
            //else if(dtPickUpDate.SelectedDate==null)
            //{
            //    txtVitriNo.ClearValidationError();
            //    dtVitrificationDate.ClearValidationError();
            //    txtTime.ClearValidationError();

            //    dtPickUpDate.SetValidation("Please Select PickUp Date");
            //    dtPickUpDate.RaiseValidationError();
            //    dtPickUpDate.Focus();
            //    return false;
            //}
            else if (VitriDetails.Count <= 0)
            {
                //txtVitriNo.ClearValidationError();
                dtVitrificationDate.ClearValidationError();
                txtTime.ClearValidationError();
                dtPickUpDate.ClearValidationError();

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Vitrification Details Data Grid Cannot Be Empty", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                return false;
            }
            else
            {
                //txtVitriNo.ClearValidationError();
                dtVitrificationDate.ClearValidationError();
                txtTime.ClearValidationError();
                dtPickUpDate.ClearValidationError();
                return true;
            }

        }


        #endregion

        #region Constructor
        public Vitrification()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Vitrification_Loaded);
            this.Unloaded += new RoutedEventHandler(Vitrification_Unloaded);


            //objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

        }


        #endregion

        #region Unloaded Event
        void Vitrification_Unloaded(object sender, RoutedEventArgs e)
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

        #region Loaded Event
        void Vitrification_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsExpendedWindow)
            {
                try
                {
                    txtVitriNo.Text = "Auto Generate";
                    if (IsPatientExist == false)
                    {
                        ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                        ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                    }
                    else
                    {
                        SSemenMaster();
                        IsExpendedWindow = true;
                        LoadFURepeaterControl();

                        if (IsEdit)
                        {
                            Print.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            Print.Visibility = Visibility.Collapsed;
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
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
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Female Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        return;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Female Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        return;
                    }
                    if (((IApplicationConfiguration)App.Current).SelectedPatient.Gender == Genders.Male.ToString())
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Female Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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

        #region Fill Master Item

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
                        fillCoupleDetails();
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

        #region Fill Vitrification Details

        public void fillVitrificationDetails()
        {
            try
            {

                clsGetVitrificationDetailsBizActionVO BizAction = new clsGetVitrificationDetailsBizActionVO();
                BizAction.Vitrification = new clsGetVitrificationVO();
                BizAction.IsEdit = IsEdit;
                BizAction.ID = ID;
                BizAction.UnitID = UnitID;
                BizAction.FromID = (int)IVFLabWorkForm.Vitrification;
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
                        if (IsEdit)
                        {
                            VitrificationMainGrid.DataContext = ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification;
                            if (((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.IsFreezed == true)
                            {
                                cmdSave.IsEnabled = false;
                            }
                            chkFreeze.IsChecked = ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.IsFreezed;
                        }
                        else
                        {
                            rdoYes.IsChecked = true;
                        }
                        if (((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.FUSetting.Count <= 0)
                        {
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.FUSetting.Add(new FileUpload());
                            FileUpLoadList = ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.FUSetting;
                        }
                        else
                        {
                            FileUpLoadList = ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.FUSetting;
                        }
                        MasterListItem SS = new MasterListItem();
                        MasterListItem OS = new MasterListItem();
                        MasterListItem Gr = new MasterListItem();
                        MasterListItem PT = new MasterListItem(); ;
                        MasterListItem CS = new MasterListItem(); ;
                        for (int i = 0; i < ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SOSemen))
                            {
                                SS = SSemen.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SOSemen));
                            }
                            if (!string.IsNullOrEmpty(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SOOcytes))
                            {
                                OS = SOctyes.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SOOcytes));
                            }
                            if (!string.IsNullOrEmpty(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].Grade))
                            {
                                Gr = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].Grade));
                            }
                            if (!string.IsNullOrEmpty(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].ProtocolType))
                            {
                                PT = ProtocolType.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].ProtocolType));
                            }
                            if (!string.IsNullOrEmpty(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CellStange))
                            {
                                CS = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CellStange));
                            }

                            if (SS != null)
                            {
                                ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SOSemen = SS.Description;
                                ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SOSemenID = SS.ID;
                            }
                            if (OS != null)
                            {
                                if (OS.ID > 0)
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SOOcytes = OS.Description;
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SOOcytesID = OS.ID;
                                }
                                else
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SOOcytes = "";
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SOOcytesID = OS.ID;
                                }
                            }
                            if (Gr != null)
                            {
                                if (Gr.ID > 0)
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].Grade = Gr.Description;
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GradeID = Gr.ID;
                                }
                                else
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].Grade = "";
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GradeID = Gr.ID;
                                }
                            }
                            if (PT != null)
                            {
                                if (PT.ID > 0)
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].ProtocolType = PT.Description;
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].ProtocolTypeID = PT.ID;
                                }
                                else
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].ProtocolType = "";
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].ProtocolTypeID = PT.ID;
                                }
                            }
                            if (CS != null)
                            {
                                if (CS.ID > 0)
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CellStange = CS.Description;
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CellStangeID = CS.ID;
                                }
                                else
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CellStange = "";
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CellStangeID = CS.ID;
                                }
                            }
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanIdList = CanList;

                            if (!string.IsNullOrEmpty(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanID))
                            {
                                if (Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanID) > 0)
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanID));
                                }
                                else
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == 0);
                                }
                            }

                            //Added by Saily P 
                            //Straw
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].StrawIdList = Straw;
                            if (!string.IsNullOrEmpty(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].StrawId))
                            {
                                if (Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].StrawId) > 0)
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].StrawId));
                                }
                                else
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedStrawId = Straw.FirstOrDefault(p => p.ID == 0);
                                }
                            }
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].StrawId = ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].StrawId;

                            //Goblet Shape
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletShapeList = GobletShape;
                            if (!string.IsNullOrEmpty(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletShapeId))
                            {
                                if (Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletShapeId) > 0)
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletShapeId));
                                }
                                else
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedGobletShape = GobletShape.FirstOrDefault(p => p.ID == 0);
                                }
                            }
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletShapeId = ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletShapeId;

                            //Goblet Size
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletSizeList = GobletSize;
                            if (!string.IsNullOrEmpty(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletSizeId))
                            {
                                if (Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletSizeId) > 0)
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletSizeId));
                                }
                                else
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedGobletSize = GobletSize.FirstOrDefault(p => p.ID == 0);
                                }
                            }
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletSizeId = ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].GobletSizeId;

                            //Canister
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanisterIdList = Canister;
                            if (!string.IsNullOrEmpty(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanisterId))
                            {
                                if (Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanisterId) > 0)
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanisterId));
                                }
                                else
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedCanisterId = Canister.FirstOrDefault(p => p.ID == 0);
                                }
                            }
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanisterId = ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanisterId;

                            //Tank
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].TankList = Tank;
                            if (!string.IsNullOrEmpty(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].TankId))
                            {
                                if (Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].TankId) > 0)
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].TankId));
                                }
                                else
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedTank = Tank.FirstOrDefault(p => p.ID == 0);
                                }
                            }
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].TankId = ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].TankId;

                            VitriDetails.Add(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i]);
                            wait.Close();
                        }



                        dgVitrificationDetilsGrid.ItemsSource = VitriDetails;
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

        #endregion

        #region Fill Couple Details
        private void fillCoupleDetails()
        {
            #region
            //if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId == 0)
            //{
            //    wait.Close();
            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //         new MessageBoxControl.MessageBoxChildWindow("Palash", "Vitrification is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    msgW1.Show();

            //    ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");

            //}
            //else
            //{
            //    CoupleDetails = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails;

            //    clsPatientGeneralVO FemalePatientDetails = new clsPatientGeneralVO();
            //    FemalePatientDetails = CoupleDetails.FemalePatient;
            //    FemalePatientDetails.Height = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.Height;
            //    FemalePatientDetails.Weight = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.Weight;
            //    FemalePatientDetails.BMI = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.BMI;
            //    FemalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", FemalePatientDetails.BMI));
            //    FemalePatientDetails.Alerts = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.Alerts;
            //    FemalePatientDetails.Photo = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.Photo;
            //    //FemalePatientDetails.FirstName = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.PatientName;
            //    //FemalePatientDetails.PatientName = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.PatientName;

            //    Female.DataContext = FemalePatientDetails;

            //    if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.Photo != null)
            //    {
            //        WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto13.Width, (int)imgPhoto13.Height);
            //        bmp.FromByteArray(((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.Photo);
            //        imgPhoto13.Source = bmp;
            //        imgPhoto13.Visibility = System.Windows.Visibility.Visible;
            //    }
            //    else
            //    {
            //        imgP1.Visibility = System.Windows.Visibility.Visible;

            //    }

            //    clsPatientGeneralVO MalePatientDetails = new clsPatientGeneralVO();
            //    MalePatientDetails = CoupleDetails.MalePatient;
            //    MalePatientDetails.Height = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient.Height;
            //    MalePatientDetails.Weight = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient.Weight;
            //    MalePatientDetails.BMI = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient.BMI;
            //    MalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", MalePatientDetails.BMI));
            //    MalePatientDetails.Alerts = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient.Alerts;
            //    MalePatientDetails.PatientName = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient.PatientName;
            //    MalePatientDetails.Photo = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient.Photo;
            //    Male.DataContext = MalePatientDetails;

            //    if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient.Photo != null)
            //    {
            //        WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto12.Width, (int)imgPhoto12.Height);
            //        bmp.FromByteArray(((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient.Photo);
            //        imgPhoto12.Source = bmp;
            //        imgPhoto12.Visibility = System.Windows.Visibility.Visible;
            //    }
            //    else
            //    {
            //        imgP2.Visibility = System.Windows.Visibility.Visible;

            //    }
            //    wait.Close();

            //    //Female.DataContext = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient;
            //    //Male.DataContext = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient;

            //}
            #endregion

            #region Fill Couple Details From EMR Commented
            try
            {
                if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId == 0)
                {
                    wait.Close();
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Vitrification is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();

                    ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");

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
                            if (CoupleDetails.CoupleId == 0)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot Create Therapy, Therapy is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                            }
                            else
                            {
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
                            }
                        }
                        fillVitrificationDetails();
                        wait.Close();
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                wait.Close();
            }
            #endregion
        }
        #endregion

        #region Get Patient EMR Details(Height and Weight)
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
                                //FemalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", BizAction.CoupleDetails.FemalePatient.BMI));
                                //FemalePatientDetails.Alerts = BizAction.CoupleDetails.FemalePatient.Alerts;

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
                    txtBMI.Text = String.Format("{0:0.00}", TotalBMI);

                    return TotalBMI;

                }
            }
            catch (Exception ex)
            {
                return 0.0;
            }
        }
        #endregion

        #region Cancel Event

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            LabDaysSummary LabSumm = new LabDaysSummary();
            LabSumm.IsPatientExist = true;
            ((IApplicationConfiguration)App.Current).OpenMainContent(LabSumm);
        }

        #endregion

        #region Media Click

        private void MediaCilck_Click(object sender, RoutedEventArgs e)
        {
            MediaDetails Win = new MediaDetails();
            if (((clsGetVitrificationDetailsVO)((HyperlinkButton)sender).DataContext).MediaDetails == null)
                ((clsGetVitrificationDetailsVO)((HyperlinkButton)sender).DataContext).MediaDetails = new List<clsFemaleMediaDetailsVO>();
            Win.ItemList = GetCollection(((clsGetVitrificationDetailsVO)((HyperlinkButton)sender).DataContext).MediaDetails);
            Win.Tag = ((clsGetVitrificationDetailsVO)((HyperlinkButton)sender).DataContext).MediaDetails;
            Win.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            Win.Show();
        }

        void Win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            MediaDetails ObjWin = (MediaDetails)sender;
            ((List<clsFemaleMediaDetailsVO>)ObjWin.Tag).Clear();
            if (ObjWin.DialogResult == true)
            {
                if (ObjWin.ItemList != null)
                {
                    foreach (var item in ObjWin.ItemList)
                    {
                        clsFemaleMediaDetailsVO objItem = new clsFemaleMediaDetailsVO();

                        objItem.Date = item.Date;
                        objItem.ItemID = item.ItemID;
                        objItem.ItemName = item.ItemName;
                        objItem.BatchID = item.BatchID;
                        objItem.BatchCode = item.BatchCode;
                        objItem.ExpiryDate = item.ExpiryDate;
                        objItem.StoreID = item.StoreID;
                        objItem.PH = item.PH;
                        objItem.OSM = item.OSM;
                        objItem.SelectedStatus = item.SelectedStatus;
                        objItem.VolumeUsed = item.VolumeUsed;

                        ((List<clsFemaleMediaDetailsVO>)ObjWin.Tag).Add(objItem);
                        //MediaList.Add(objItem);
                    }
                }
            }
        }

        private ObservableCollection<clsFemaleMediaDetailsVO> GetCollection(List<clsFemaleMediaDetailsVO> list)
        {
            ObservableCollection<clsFemaleMediaDetailsVO> ob = new ObservableCollection<clsFemaleMediaDetailsVO>();
            foreach (var i in list)
            {
                ob.Add(i);
            }
            return ob;
        }

        #endregion

        #region Save Event
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to save Vitrification Details";
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
        }
        private void SaveVitrification()
        {
            try
            {

                wait.Show();
                ObservableCollection<clsGetVitrificationDetailsVO> newvit = VitriDetails;
                clsAddUpdateVitrificationBizActionVO BizAction = new clsAddUpdateVitrificationBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                BizAction.Vitrification = new clsGetVitrificationVO();
                BizAction.Vitrification.Impression = Impression;
                BizAction.ID = ID;
                BizAction.UintID = UnitID;
                BizAction.CoupleID = CoupleDetails.CoupleId;
                BizAction.CoupleUintID = CoupleDetails.CoupleUnitId;
                BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                if (rdoNo.IsChecked == true)
                {
                    BizAction.Vitrification.ConsentForm = false;
                }
                else
                {
                    BizAction.Vitrification.ConsentForm = true;
                }
                BizAction.Vitrification.IsFreezed = (bool)chkFreeze.IsChecked;

                if (dtPickUpDate.SelectedDate != null)
                    BizAction.Vitrification.PickupDate = dtPickUpDate.SelectedDate.Value;

                BizAction.Vitrification.VitrificationDate = dtVitrificationDate.SelectedDate.Value.Date;
                BizAction.Vitrification.VitrificationDate = BizAction.Vitrification.VitrificationDate.Value.Add(txtTime.Value.Value.TimeOfDay);
                BizAction.Vitrification.VitrificationNo = txtVitriNo.Text.Trim();
                //Added by Saily P
                for (int i = 0; i < VitriDetails.Count; i++)
                {
                    VitriDetails[i].CellStangeID = VitriDetails[i].SelectedCellStage.ID;
                    VitriDetails[i].GradeID = VitriDetails[i].SelectedGrade.ID;
                    VitriDetails[i].CanID = VitriDetails[i].SelectedCanId.ID.ToString();
                    VitriDetails[i].StrawId = VitriDetails[i].SelectedStrawId.ID.ToString();
                    VitriDetails[i].GobletShapeId = VitriDetails[i].SelectedGobletShape.ID.ToString();
                    VitriDetails[i].GobletSizeId = VitriDetails[i].SelectedGobletSize.ID.ToString();
                    VitriDetails[i].CanisterId = VitriDetails[i].SelectedCanisterId.ID.ToString();
                    VitriDetails[i].TankId = VitriDetails[i].SelectedTank.ID.ToString();
                }
                BizAction.Vitrification.VitrificationDetails = ((List<clsGetVitrificationDetailsVO>)VitriDetails.ToList());
                BizAction.Vitrification.FUSetting = FileUpLoadList;

                #region Service Call (Check Validation)

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        string txtmsg = "";
                        if (IsEdit == true)
                        {
                            txtmsg = "Vitrification Details Updated Successfully";
                        }
                        else
                        {
                            txtmsg = "Vitrification Details Saved Successfully";
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

        #region Color Selector Selection Changed Event
        private void Colorsel_SelectionChanged(object sender, EventArgs e)
        {
            //((clsGetVitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem).SelectesColor //= ((Liquid.ColorSelector)sender).Selected;
            for (int i = 0; i < VitriDetails.Count; i++)
            {
                if (VitriDetails[i] == ((clsGetVitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem))
                {
                    VitriDetails[i].SelectesColor = ((Liquid.ColorSelector)sender).Selected;
                    VitriDetails[i].ColorCode = ((Liquid.ColorSelector)sender).Selected.ToString();

                }
            }


        }
        #endregion

        #region CanID Selecion Changed
        private void cmbCanID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < VitriDetails.Count; i++)
            {
                if (VitriDetails[i] == ((clsGetVitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem))
                {
                    if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                    {
                        VitriDetails[i].CanID = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID.ToString();
                    }
                }

            }
        }
        #endregion

        #region All Combo Selection Changed
        private void cmbCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((AutoCompleteBox)sender).Name.Equals("cmbStraw"))
            {
                for (int i = 0; i < VitriDetails.Count; i++)
                {
                    if (VitriDetails[i] == ((clsGetVitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            VitriDetails[i].StrawId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID.ToString();
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbGobletShape"))
            {
                for (int i = 0; i < VitriDetails.Count; i++)
                {
                    if (VitriDetails[i] == ((clsGetVitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            VitriDetails[i].GobletShapeId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID.ToString();
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbGobletSize"))
            {
                for (int i = 0; i < VitriDetails.Count; i++)
                {
                    if (VitriDetails[i] == ((clsGetVitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            VitriDetails[i].GobletSizeId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID.ToString();
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbCanisterId"))
            {
                for (int i = 0; i < VitriDetails.Count; i++)
                {
                    if (VitriDetails[i] == ((clsGetVitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            VitriDetails[i].CanisterId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID.ToString();
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbTankId"))
            {
                for (int i = 0; i < VitriDetails.Count; i++)
                {
                    if (VitriDetails[i] == ((clsGetVitrificationDetailsVO)dgVitrificationDetilsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            VitriDetails[i].TankId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID.ToString();
                        }
                    }
                }
            }
        }
        #endregion

        #region Upload File
        private void LoadFURepeaterControl()
        {

            lstFUBox = new ListBox();
            if (FileUpLoadList == null || FileUpLoadList.Count == 0)
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
                MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "This File is not uploaded. Please upload the File then click on preview", MessageBoxButtons.Ok, MessageBoxIcon.Information);
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
                    string msgText = "Error while reading file.";

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
                                                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select File.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

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

        #region Print

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            long _UnitID = 0;
            long _ID = 0;
            _ID = ID;
            _UnitID = UnitID;


            // HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source,  "../Reports/InventoryPharmacy/GRNPrint.aspx?"+Parameters , "_blank");

            string URL = "../Reports/IVF/VitrificationReport.aspx?ID=" + _ID + "&UnitID=" + _UnitID + "&FormId=" + (int)IVFLabWorkForm.Vitrification;

            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

        }

        #endregion Print

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


    }
}
