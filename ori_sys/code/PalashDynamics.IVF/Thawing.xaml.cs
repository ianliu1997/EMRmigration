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
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Patient;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using DataDrivenApplication.Forms;

namespace PalashDynamics.IVF
{
    public partial class Thawing : UserControl, IInitiateCIMS
    {
        #region Variables
            public bool IsPatientExist;
            WaitIndicator wait = new WaitIndicator();
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

        private ObservableCollection<clsThawingDetailsVO> _ThawDetails = new ObservableCollection<clsThawingDetailsVO>();
        public ObservableCollection<clsThawingDetailsVO> ThawDetails
        {
            get { return _ThawDetails; }
            set { _ThawDetails = value; }
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


        #endregion

        #region Validation
        public Boolean Validation()
        {
           
             if (dtVitrificationDate.SelectedDate == null)
            {
                dtVitrificationDate.SetValidation("Please Select Thawing Date");
                dtVitrificationDate.RaiseValidationError();
                dtVitrificationDate.Focus();
                return false;
            }
            else if (txtTime.Value == null)
            {
                dtVitrificationDate.ClearValidationError();
                txtTime.SetValidation("Please Select Thawing Time");
                txtTime.RaiseValidationError();
                txtTime.Focus();
                return false;
            }
            else if (cmbLabPerson.SelectedItem== null)
            {
                
                dtVitrificationDate.ClearValidationError();
                txtTime.ClearValidationError();
                cmbLabPerson.TextBox.SetValidation("Please Select Lab Person");
                cmbLabPerson.TextBox.RaiseValidationError();
                cmbLabPerson.Focus();
                return false;
            }
            else if (ThawDetails.Count <= 0)
            {
                
                dtVitrificationDate.ClearValidationError();
                txtTime.ClearValidationError();
                cmbLabPerson.TextBox.ClearValidationError();

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Thawing Details Data Grid Cannot Be Empty", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                return false;
            }
            else
            {
                
                dtVitrificationDate.ClearValidationError();
                txtTime.ClearValidationError();
                cmbLabPerson.TextBox.ClearValidationError();
                return true;
            }

        }


        #endregion

        #region Constructor 
        public Thawing()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Thawing_Loaded);
            this.Unloaded += new RoutedEventHandler(Thawing_Unloaded);
        }
        #endregion

        #region Unloaded Event

        void Thawing_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                PalashDynamics.Service.DataTemplateServiceRef.DataTemplateServiceClient client = new PalashDynamics.Service.DataTemplateServiceRef.DataTemplateServiceClient("CustomBinding_DataTemplateService", address.AbsoluteUri);
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
        void Thawing_Loaded(object sender, RoutedEventArgs e)
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
                        SSemenMaster();
                        IsExpendedWindow = true;
                        LoadFURepeaterControl();
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
                        CanList = ((clsGetMasterListBizActionVO)args.Result).MasterList;
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
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        CellStage =((clsGetMasterListBizActionVO)args.Result).MasterList;
                        FillLabPerson();
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

        private void FillLabPerson()
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

                    cmbLabPerson.ItemsSource = null;
                    cmbLabPerson.ItemsSource = objList;
                    cmbLabPerson.SelectedItem = objList[0];
                    fillCoupleDetails();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        #endregion

        #region Fill Vitrification Details

        public void fillVitrificationDetails()
        {
            try
            {

                clsGetVitrificationDetailsBizActionVO BizAction = new clsGetVitrificationDetailsBizActionVO();
                BizAction.Vitrification = new clsGetVitrificationVO();
                BizAction.IsEdit = true;
                BizAction.ID = 0;
                BizAction.UnitID = 0;
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
                        MasterListItem SS = new MasterListItem();
                        MasterListItem OS = new MasterListItem();
                        MasterListItem Gr = new MasterListItem();
                        MasterListItem PT = new MasterListItem(); ;
                        MasterListItem CS = new MasterListItem(); ;

                        if (((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.FUSetting.Count <= 0)
                        {
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.FUSetting.Add(new FileUpload());
                            FileUpLoadList = ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.FUSetting;
                        }
                        else
                        {
                            FileUpLoadList = ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.FUSetting;
                        }

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
                                if (SS.ID > 0)
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SOSemen = SS.Description;
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SOSemenID = SS.ID;
                                }
                                else
                                {
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SOSemen = "";
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SOSemenID = SS.ID;
                                }


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
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].ProtocolType ="";
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
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CellStange ="";
                                    ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CellStangeID = CS.ID;
                                }
                            }
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanIdList = CanList;
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i].CanID));

                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.ThawingDetails[i].CellStage = CellStage;
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.ThawingDetails[i].SelectedCellStage = CellStage.FirstOrDefault(p => p.ID ==0);

                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.ThawingDetails[i].Grade = Grade;
                            ((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.ThawingDetails[i].SelectedGrade = Grade.FirstOrDefault(p => p.ID == 0);

                            VitriDetails.Add(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.VitrificationDetails[i]);
                            ThawDetails.Add(((clsGetVitrificationDetailsBizActionVO)arg.Result).Vitrification.ThawingDetails[i]);
                            wait.Close();
                        }



                        dgVitrificationDetilsGrid.ItemsSource = VitriDetails;
                        dgThawingDetilsGrid.ItemsSource = ThawDetails;
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

        #region Fill Thawing Grid After Saving
        public void fillThawingDetails()
        {
            try
            {
                clsGetThawingDetailsBizActionVO BizAction = new clsGetThawingDetailsBizActionVO();
                BizAction.Thawing = new clsThawingVO();
                BizAction.IsEdit = true;
                BizAction.ID = ID;
                BizAction.UnitID = UnitID;
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
                        FileUpLoadList = ((clsGetThawingDetailsBizActionVO)arg.Result).Thawing.FUSetting;
                        dtVitrificationDate.SelectedDate = ((clsGetThawingDetailsBizActionVO)arg.Result).Thawing.Date;
                        txtTime.Value = ((clsGetThawingDetailsBizActionVO)arg.Result).Thawing.Date;
                        cmbLabPerson.SelectedValue = (long)((clsGetThawingDetailsBizActionVO)arg.Result).Thawing.LabPerseonID;
                        MasterListItem SS = new MasterListItem();
                        MasterListItem OS = new MasterListItem();
                        MasterListItem Gr = new MasterListItem();
                        MasterListItem PT = new MasterListItem(); ;
                        MasterListItem CS = new MasterListItem(); ;
                        for (int i = 0; i < ((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].SOSemen))
                            {
                                SS = SSemen.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].SOSemen));
                            }
                            if (!string.IsNullOrEmpty(((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].SOOcytes))
                            {
                                OS = SOctyes.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].SOOcytes));
                            }
                            if (!string.IsNullOrEmpty(((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].Grade))
                            {
                                Gr = Grade.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].Grade));
                            }
                            if (!string.IsNullOrEmpty(((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].ProtocolType))
                            {
                                PT = ProtocolType.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].ProtocolType));
                            }
                            if (!string.IsNullOrEmpty(((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].CellStange))
                            {
                                CS = CellStage.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].CellStange));
                            }

                            if (SS != null)
                            {
                                ((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].SOSemen = SS.Description;
                                ((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].SOSemenID = SS.ID;
                            }
                            if (OS != null)
                            {
                                ((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].SOOcytes = OS.Description;
                                ((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].SOOcytesID = OS.ID;
                            }
                            if (Gr != null)
                            {
                                if (Gr.ID > 0)
                                {
                                    ((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].Grade = Gr.Description;
                                    ((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].GradeID = Gr.ID;
                                }
                                else
                                {
                                    ((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].Grade = "";
                                    ((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].GradeID = Gr.ID;
                                }
                            }
                            if (PT != null)
                            {
                                if (PT.ID > 0)
                                {
                                    ((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].ProtocolType = PT.Description;
                                    ((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].ProtocolTypeID = PT.ID;
                                }
                                else
                                {
                                    ((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].ProtocolType ="";
                                    ((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].ProtocolTypeID = PT.ID;
                                }
                            }
                            if (CS != null)
                            {
                                if (CS.ID > 0)
                                {
                                    ((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].CellStange = CS.Description;
                                    ((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].CellStangeID = CS.ID;
                                }
                                else
                                {
                                    ((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].CellStange = "";
                                    ((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].CellStangeID = CS.ID;
                        
                                }
                            }
                            ((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].CanIdList = CanList;
                            ((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].SelectedCanId = CanList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i].CanID));

                            ((clsGetThawingDetailsBizActionVO)arg.Result).Thawing.ThawingDetails[i].CellStage = CellStage;
                            ((clsGetThawingDetailsBizActionVO)arg.Result).Thawing.ThawingDetails[i].SelectedCellStage = CellStage.FirstOrDefault(p => p.ID == ((clsGetThawingDetailsBizActionVO)arg.Result).Thawing.ThawingDetails[i].SelectedCellStage.ID);

                            ((clsGetThawingDetailsBizActionVO)arg.Result).Thawing.ThawingDetails[i].Grade = Grade;
                            ((clsGetThawingDetailsBizActionVO)arg.Result).Thawing.ThawingDetails[i].SelectedGrade = Grade.FirstOrDefault(p => p.ID == ((clsGetThawingDetailsBizActionVO)arg.Result).Thawing.ThawingDetails[i].SelectedGrade.ID);

                            VitriDetails.Add(((clsGetThawingDetailsBizActionVO)arg.Result).VitrificationDetails[i]);
                            ThawDetails.Add(((clsGetThawingDetailsBizActionVO)arg.Result).Thawing.ThawingDetails[i]);
                            wait.Close();
                        }



                        dgVitrificationDetilsGrid.ItemsSource = VitriDetails;
                        dgThawingDetilsGrid.ItemsSource = ThawDetails;
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

        //Added by Saily P on 25.09.12 to get the patient details in case of Donor
        private void LoadPatientHeader()
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient.Gender == Genders.Female.ToString())
            {
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
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            else
            {
                wait.Close();
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Thawing is Only For Male", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            }
        }
        
        private void fillCoupleDetails()
        {
            if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId == 0)
            {
                LoadPatientHeader();
                wait.Close();
                if (!IsEdit)
                {
                    fillVitrificationDetails();
                }
                else
                {
                    fillThawingDetails();
                }
                //wait.Close();
                //MessageBoxControl.MessageBoxChildWindow msgW1 =
                //     new MessageBoxControl.MessageBoxChildWindow("Palash", "Thawing is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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
                        if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails != null)
                        {
                            CoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails;
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
                            }
                        }
                        else
                            LoadPatientHeader();
                        if (!IsEdit)
                        {
                            fillVitrificationDetails();
                        }
                        else
                        {
                            fillThawingDetails();
                        }
                        wait.Close();
                    }
                    
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

                #region Commented by Saily P on 070512. Alerts for patient not reflecting.
                //CoupleDetails = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails;
                //clsPatientGeneralVO FemalePatientDetails = new clsPatientGeneralVO();
                //FemalePatientDetails = CoupleDetails.FemalePatient;
                //FemalePatientDetails.Height = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.Height;
                //FemalePatientDetails.Weight = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.Weight;
                //FemalePatientDetails.BMI = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.BMI;
                //FemalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", FemalePatientDetails.BMI));
                //FemalePatientDetails.Alerts = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.Alerts;
                //FemalePatientDetails.Photo = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.Photo;
                //Female.DataContext = FemalePatientDetails;
                
                //if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.Photo != null)
                //{
                //    WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto13.Width, (int)imgPhoto13.Height);
                //    bmp.FromByteArray(((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.Photo);
                //    imgPhoto13.Source = bmp;
                //    imgPhoto13.Visibility = System.Windows.Visibility.Visible;
                //}
                //else
                //{
                //    imgP1.Visibility = System.Windows.Visibility.Visible;

                //}


                //clsPatientGeneralVO MalePatientDetails = new clsPatientGeneralVO();
                //MalePatientDetails = CoupleDetails.MalePatient;
                //MalePatientDetails.Height = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient.Height;
                //MalePatientDetails.Weight = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient.Weight;
                //MalePatientDetails.BMI = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient.BMI;
                //MalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", MalePatientDetails.BMI));
                //MalePatientDetails.Alerts = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient.Alerts;
                //MalePatientDetails.Photo = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient.Photo;
                //Male.DataContext = MalePatientDetails;

                //if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient.Photo != null)
                //{
                //    WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto12.Width, (int)imgPhoto12.Height);
                //    bmp.FromByteArray(((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient.Photo);
                //    imgPhoto12.Source = bmp;
                //    imgPhoto12.Visibility = System.Windows.Visibility.Visible;
                //}
                //else
                //{
                //    imgP2.Visibility = System.Windows.Visibility.Visible;

                //}

                ////Female.DataContext = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient;
                ////Male.DataContext = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient;
                //if (!IsEdit)
                //{
                //    fillVitrificationDetails();
                //}
                //else
                //{
                //    fillThawingDetails();
                //}
                #endregion
            }
        }

            #region Fill Couple Details From EMR
            //try
            //{
            //    clsGetCoupleDetailsBizActionVO BizAction = new clsGetCoupleDetailsBizActionVO();
            //    BizAction.PatientID = ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
            //    BizAction.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
            //    BizAction.IsAllCouple = false;
            //    BizAction.CoupleDetails = new clsCoupleVO();

            //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            //    client.ProcessCompleted += (s, args) =>
            //    {
            //        if (args.Error == null && args.Result != null)
            //        {
            //            BizAction.CoupleDetails.MalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient;
            //            BizAction.CoupleDetails.FemalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient;
            //            CoupleDetails.MalePatient = new clsPatientGeneralVO();
            //            CoupleDetails.FemalePatient = new clsPatientGeneralVO();
            //            CoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails;
            //            if (BizAction.CoupleDetails.MalePatient == null || BizAction.CoupleDetails.FemalePatient == null || BizAction.CoupleDetails.FemalePatient.PatientID == 0 || BizAction.CoupleDetails.MalePatient.PatientID == 0)
            //            {
            //                MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot Create Thawing, Thawing is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //                msgW1.Show();

            //                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            //                wait.Close();
            //            }
            //            else
            //            {

            //                getEMRDetails(BizAction.CoupleDetails.FemalePatient, "F");
            //                getEMRDetails(BizAction.CoupleDetails.MalePatient, "M");
            //                if (!IsEdit)
            //                {
            //                    fillVitrificationDetails();
            //                }
            //                else
            //                {
            //                    fillThawingDetails();
            //                }

            //            }
            //        }
            //        else
            //        {
            //            wait.Close();
            //        }
            //    };
            //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //    client.CloseAsync();
            //}
            //catch (Exception ex)
            //{
            //    wait.Close();
            //}
            #endregion
        
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
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to save Thawing Details";
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


        void winImp_OnSaveClick(object sender, RoutedEventArgs e)
        {
            ImpressionWindow ObjImp = (ImpressionWindow)sender;
            Impression = ObjImp.Impression;
            SaveThawing();
        }

        private void SaveThawing()
        {
            try
            {

                wait.Show();
                ObservableCollection<clsGetVitrificationDetailsVO> newvit = VitriDetails;
                clsAddUpdateThawingBizActionVO BizAction = new clsAddUpdateThawingBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                BizAction.Thawing = new clsThawingVO();
                BizAction.Thawing.Impression = Impression;
                BizAction.ID = ID;
                BizAction.UintID = UnitID;
                BizAction.CoupleID = CoupleDetails.CoupleId;
                BizAction.CoupleUintID = CoupleDetails.CoupleUnitId;

                BizAction.Thawing.LabPerseonID = ((MasterListItem)cmbLabPerson.SelectedItem).ID;
                BizAction.Thawing.Date = dtVitrificationDate.SelectedDate.Value.Date;
                BizAction.Thawing.Date = BizAction.Thawing.Date.Value.Add(txtTime.Value.Value.TimeOfDay);
                
                BizAction.Thawing.ThawingDetails = ((List<clsThawingDetailsVO>)ThawDetails.ToList());
                BizAction.Thawing.FUSetting = FileUpLoadList;


                #region Service Call (Check Validation)

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        string txtmsg = "";
                        if (IsEdit == true)
                        {
                            txtmsg = "Thawing Details Updated Successfully";
                        }
                        else
                        {
                            txtmsg = "Thawing Details Saved Successfully";
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
            if (((clsThawingDetailsVO)((HyperlinkButton)sender).DataContext).MediaDetails == null)
                ((clsThawingDetailsVO)((HyperlinkButton)sender).DataContext).MediaDetails = new List<clsFemaleMediaDetailsVO>();
            Win.ItemList = GetCollection(((clsThawingDetailsVO)((HyperlinkButton)sender).DataContext).MediaDetails);
            Win.Tag = ((clsThawingDetailsVO)((HyperlinkButton)sender).DataContext).MediaDetails;
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

        #region Cell Stage and Grade Selection Changed Event

        private void cmbCellStage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < ThawDetails.Count; i++)
            {
                if (i == dgThawingDetilsGrid.SelectedIndex)
                {
                    if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                    {
                        ThawDetails[i].CellStangeID = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                    }
                }
            }
        }

        private void cmbGrade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < ThawDetails.Count; i++)
            {
                if (i == dgThawingDetilsGrid.SelectedIndex)
                {
                    if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                    {
                        ThawDetails[i].GradeID = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
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
                    PalashDynamics.Service.DataTemplateServiceRef.DataTemplateServiceClient client = new PalashDynamics.Service.DataTemplateServiceRef.DataTemplateServiceClient("CustomBinding_DataTemplateService", address.AbsoluteUri);
                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            System.Windows.Browser.HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + FullFile });
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
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "This File is not uploaded. Please upload the File then click on preview", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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
                    using (System.IO.Stream stream = openDialog.File.OpenRead())
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
