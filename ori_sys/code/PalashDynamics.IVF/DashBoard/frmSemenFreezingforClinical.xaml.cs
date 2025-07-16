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
using PalashDynamics.ValueObjects;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.DashBoardVO;
using CIMS;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Patient;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Reflection;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Animations;
using PalashDynamics.UserControls;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmSemenFreezingforClinical : UserControl, IInitiateCIMS
    {
        public frmSemenFreezingforClinical()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((IApplicationConfiguration)App.Current).SelectedPatient.Gender != Genders.Female.ToString())
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
                        Statusdetails = true;
                        fillCoupleDetails();
                        this.SetCommandButtonState("Load");
                    }
                }
                else
                {
                    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                    ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Male Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void getCycleCode() 
        {
            GetCycleCodeForPatientBizActionVO BizAction = new GetCycleCodeForPatientBizActionVO();

            BizAction.CoupleUnitID = CoupleDetails.CoupleUnitId;
            BizAction.CoupleID = CoupleDetails.CoupleId;
          
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    CmbCycleCode.ItemsSource = null;
                    List<string> objList = new List<string>();
                    objList.Add("--Select--");
                    objList.AddRange(((GetCycleCodeForPatientBizActionVO)e.Result).List.DeepCopyd());
                    CmbCycleCode.ItemsSource = objList;
                   

                    if (cyclecode != string.Empty)
                        CmbCycleCode.SelectedItem = cyclecode;
                    else
                        CmbCycleCode.SelectedItem = objList[0];

                    
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private ObservableCollection<clsNew_SpremFreezingVO> _SpremFreezingDetails = new ObservableCollection<clsNew_SpremFreezingVO>();
        public ObservableCollection<clsNew_SpremFreezingVO> SpremFreezingDetails
        {
            get { return _SpremFreezingDetails; }
            set { _SpremFreezingDetails = value; }
        }

        private ObservableCollection<clsNew_SpremFreezingVO> _RemoveSpremFreezingDetails = new ObservableCollection<clsNew_SpremFreezingVO>();
        public ObservableCollection<clsNew_SpremFreezingVO> RemoveSpremFreezingDetails
        {
            get { return _RemoveSpremFreezingDetails; }
            set { _RemoveSpremFreezingDetails = value; }
        }

        private List<MasterListItem> _ColorList = new List<MasterListItem>();
        public List<MasterListItem> ColorList
        {
            get
            {
                return _ColorList;
            }
            set
            {
                _ColorList = value;
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

        private List<MasterListItem> _StrawList = new List<MasterListItem>();
        public List<MasterListItem> StrawList
        {
            get
            {
                return _StrawList;
            }
            set
            {
                _StrawList = value;
            }
        }

        private List<MasterListItem> _GlobletShapeList = new List<MasterListItem>();
        public List<MasterListItem> GlobletShapeList
        {
            get
            {
                return _GlobletShapeList;
            }
            set
            {
                _GlobletShapeList = value;
            }
        }

        private List<MasterListItem> _CanisterList = new List<MasterListItem>();
        public List<MasterListItem> CanisterList
        {
            get
            {
                return _CanisterList;
            }
            set
            {
                _CanisterList = value;
            }
        }

        private List<MasterListItem> _GlobletSizeList = new List<MasterListItem>();
        public List<MasterListItem> GlobletSizeList
        {
            get
            {
                return _GlobletSizeList;
            }
            set
            {
                _GlobletSizeList = value;
            }
        }

        private List<MasterListItem> _TankList = new List<MasterListItem>();
        public List<MasterListItem> TankList
        {
            get
            {
                return _TankList;
            }
            set
            {
                _TankList = value;
            }
        }
        private void cmbGrade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((AutoCompleteBox)sender).Name.Equals("cmbcolor"))
            {
                for (int i = 0; i < SpremFreezingDetails.Count; i++)
                {
                    if (SpremFreezingDetails[i] == ((clsNew_SpremFreezingVO)dgSpremFreezingDetailsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            SpremFreezingDetails[i].GobletColorID = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbStraw"))
            {
                for (int i = 0; i < SpremFreezingDetails.Count; i++)
                {
                    if (SpremFreezingDetails[i] == ((clsNew_SpremFreezingVO)dgSpremFreezingDetailsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            SpremFreezingDetails[i].StrawId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbGlobletShape"))
            {
                for (int i = 0; i < SpremFreezingDetails.Count; i++)
                {
                    if (SpremFreezingDetails[i] == ((clsNew_SpremFreezingVO)dgSpremFreezingDetailsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            SpremFreezingDetails[i].GobletShapeId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbGlobletSize"))
            {
                for (int i = 0; i < SpremFreezingDetails.Count; i++)
                {
                    if (SpremFreezingDetails[i] == ((clsNew_SpremFreezingVO)dgSpremFreezingDetailsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            SpremFreezingDetails[i].GobletSizeId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbCane"))
            {
                for (int i = 0; i < SpremFreezingDetails.Count; i++)
                {
                    if (SpremFreezingDetails[i] == ((clsNew_SpremFreezingVO)dgSpremFreezingDetailsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            SpremFreezingDetails[i].CanID = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbCanister"))
            {
                for (int i = 0; i < SpremFreezingDetails.Count; i++)
                {
                    if (SpremFreezingDetails[i] == ((clsNew_SpremFreezingVO)dgSpremFreezingDetailsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            SpremFreezingDetails[i].CanisterId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
            else if (((AutoCompleteBox)sender).Name.Equals("cmbTank"))
            {
                for (int i = 0; i < SpremFreezingDetails.Count; i++)
                {
                    if (SpremFreezingDetails[i] == ((clsNew_SpremFreezingVO)dgSpremFreezingDetailsGrid.SelectedItem))
                    {
                        if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                        {
                            SpremFreezingDetails[i].TankId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                        }
                    }
                }
            }
        }

        #region Lost Focus

        string textBefore = null;
        int selectionStart = 0;
        double selectionStartDecml = 0;
        int selectionLength = 0;



        #endregion
        #region Check Patient Is Selected/Not
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
        // Grid Add click
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (SpremFreezingDetails == null)
            {
                SpremFreezingDetails = new ObservableCollection<clsNew_SpremFreezingVO>();
            }
            SpremFreezingDetails.Add(new clsNew_SpremFreezingVO() { SpremNostr = "Auto Generated", GobletColorID = 0, ColorListVO = ColorList, StrawId = 0, StrawListVO = StrawList, GobletShapeId = 0, GlobletShapeListVO = GlobletShapeList, GobletSizeId = 0, GlobletSizeListVO = GlobletSizeList, CanID = 0, CanListVO = CanList, CanisterId = 0, CanisterListVO = CanisterList, TankId = 0, TankListVO = TankList, Comments = "", IsThaw = false, IsNewlyAdded = true });

            dgSpremFreezingDetailsGrid.ItemsSource = SpremFreezingDetails;
        }
        bool ISEdit = false;
        #region Validation
        public Boolean Validation()
        {
            if (SpremFreezDate.SelectedDate == null)
            {
                SpremFreezDate.SetValidation("Please Select Freezing Date");
                SpremFreezDate.RaiseValidationError();
                SpremFreezDate.Focus();
                return false;
            }
            else if (SpremFreezTime.Value == null)
            {
                SpremFreezDate.ClearValidationError();
                SpremFreezTime.SetValidation("Please Select Freezing Time");
                SpremFreezTime.RaiseValidationError();
                SpremFreezTime.Focus();
                return false;
            }
            else if (cmbEmbryologist.SelectedItem == null || ((MasterListItem)cmbEmbryologist.SelectedItem).ID == 0)
            {
                SpremFreezDate.ClearValidationError();
                SpremFreezTime.ClearValidationError();
                cmbEmbryologist.TextBox.SetValidation("Please Select Embryologist");
                cmbEmbryologist.TextBox.RaiseValidationError();
                cmbEmbryologist.Focus();
                return false;
            }


            else if (cmbDoctorName.SelectedItem == null || ((MasterListItem)cmbDoctorName.SelectedItem).ID == 0)
            {
                SpremFreezDate.ClearValidationError();
                SpremFreezTime.ClearValidationError();
                cmbEmbryologist.TextBox.ClearValidationError();

                cmbDoctorName.TextBox.SetValidation("Please Select Anesthetist");
                cmbDoctorName.TextBox.RaiseValidationError();
                cmbDoctorName.Focus();
                return false;
            }

            else if (SpremFreezingDetails.Count <= 0)
            {
                SpremFreezDate.ClearValidationError();
                SpremFreezTime.ClearValidationError();
                cmbEmbryologist.TextBox.ClearValidationError();
                cmbDoctorName.TextBox.ClearValidationError();
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Freezing Details Data Grid Cannot Be Empty", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                return false;
            }
            else if (txtvolume.Text == null)
            {
                SpremFreezDate.ClearValidationError();
                SpremFreezTime.ClearValidationError();
                cmbEmbryologist.TextBox.ClearValidationError();
                cmbDoctorName.TextBox.ClearValidationError();
                txtvolume.SetValidation("Please Enter Volume .");
                txtvolume.RaiseValidationError();
                txtvolume.Focus();
                return false;
            }
            else
            {
                SpremFreezDate.ClearValidationError();
                SpremFreezTime.ClearValidationError();
                cmbEmbryologist.TextBox.ClearValidationError();
                cmbDoctorName.TextBox.ClearValidationError();
                txtvolume.ClearValidationError();
                return true;
            }
        }
        #endregion
        //Grid Delete .  .  . . . .  . 
        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgSpremFreezingDetailsGrid.SelectedItem != null)
            {
                clsNew_SpremFreezingVO objVO = (clsNew_SpremFreezingVO)dgSpremFreezingDetailsGrid.SelectedItem;
                if (ISEdit != true)
                {
                    if (SpremFreezingDetails.Count > 1)
                    {
                        // Statusdetails = false;
                        SpremFreezingDetails[dgSpremFreezingDetailsGrid.SelectedIndex].SpremNostr = "-1";
                        if (SpremFreezingDetails[dgSpremFreezingDetailsGrid.SelectedIndex].ID != 0 && !SpremFreezingDetails[dgSpremFreezingDetailsGrid.SelectedIndex].SpremNostr.Equals("Auto Generated"))
                        {
                            RemoveSpremFreezingDetails.Add(SpremFreezingDetails[dgSpremFreezingDetailsGrid.SelectedIndex]);
                        }
                        SpremFreezingDetails.RemoveAt(dgSpremFreezingDetailsGrid.SelectedIndex);
                    }
                    dgSpremFreezingDetailsGrid.ItemsSource = SpremFreezingDetails;
                }
                else if (objVO.IsNewlyAdded == true)
                {
                    SpremFreezingDetails.Remove(objVO);
                    dgSpremFreezingDetailsGrid.ItemsSource = null;
                    dgSpremFreezingDetailsGrid.ItemsSource = SpremFreezingDetails;
                    dgSpremFreezingDetailsGrid.UpdateLayout();
                }
                else
                {
                    if (Validation())
                    {
                        try
                        {
                            // Statusdetails = false;
                            string msgTitle = "Palash";
                            string msgText = "Are You Sure \n You Want To Delete Semen Freezing Details";
                            MessageBoxControl.MessageBoxChildWindow msgWin =
                                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosedforDelete);
                            msgWin.Show();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
        }
        void msgW_OnMessageBoxClosedforDelete(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    cls_NewDeleteSpremFreezingBizActionVO BizAction = new cls_NewDeleteSpremFreezingBizActionVO();

                    //BizAction.MalePatientID = PatientID1;
                    //BizAction.MalePatientUnitID = PatientUnitID1;
                    BizAction.SpremID = ((clsNew_SpremFreezingVO)dgSpremFreezingDetailsGrid.SelectedItem).ID;
                    BizAction.Status = false;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            if (arg.Error == null && arg.Result != null)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "Semen Freezing Details Deleted Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();

                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW1.Show();
                            }
                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        private void FillColor()
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
                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        ColorList = null;
                        ColorList = objList;
                    }
                    FillStraw();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
                wait.Close();
            }
        }
        private void FillStraw()
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
                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        StrawList = null;
                        StrawList = objList;
                    }
                    FillGlobletShape();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
                wait.Close();
            }
        }
        private void FillGlobletShape()
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
                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        GlobletShapeList = null;
                        GlobletShapeList = objList;
                    }
                    FillGlobletSize();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
                wait.Close();
            }
        }
        private void FillGlobletSize()
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
                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        GlobletSizeList = null;
                        GlobletSizeList = objList;
                    }
                    FillCane();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
                wait.Close();
            }
        }
        private void FillCane()
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
                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        CanList = null;
                        CanList = objList;
                    }
                    FillCanister();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
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
                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        CanisterList = null;
                        CanisterList = objList;
                    }
                    FillTank();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
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
                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        TankList = null;
                        TankList = objList;
                    }
                    FillFrontGrid();
                   // FillGrid();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
                wait.Close();
            }
            catch (Exception ex)
            {
                throw ex;
                wait.Close();
            }
        }
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtTotalSpremCount_TextChanged(object sender, TextChangedEventArgs e)
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

        private void txtTotalSpremCount_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtvolume_TextChanged(object sender, TextChangedEventArgs e)
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

        private void txtvolume_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtGradeA_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtGradeA_TextChanged(object sender, TextChangedEventArgs e)
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

        private void txtGradeB_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtGradeB_TextChanged(object sender, TextChangedEventArgs e)
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

        private void txtGradeC_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtGradeC_TextChanged(object sender, TextChangedEventArgs e)
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

        private void dgSpremFreezingDetailsGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            //clsSpermThawingDetailsVO ThawRow = (clsSpermThawingDetailsVO)e.Row.DataContext;
            //if (ThawRow.IsFreezed == true)
            //    e.Row.IsEnabled = false;
            //else
            //    e.Row.IsEnabled = true;
            DataGridRow row = e.Row;
            if (((clsNew_SpremFreezingVO)(row.DataContext)).IsThaw == true)
            {
                e.Row.IsEnabled = false;
            }
            else
                e.Row.IsEnabled = true;
        }

        private void txtMotility_TextChanged(object sender, TextChangedEventArgs e)
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

        private void txtMotility_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }
        private bool IsPatientExist;
        public string Action { get; set; }
        public string ModuleName { get; set; }
        UIElement myData = null;
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
       
        #region FillCouple Details
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
        private void fillCoupleDetails()
        {
            clsGetCoupleDetailsBizActionVO BizAction = new clsGetCoupleDetailsBizActionVO();
            BizAction.PatientID = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
            BizAction.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
            BizAction.CoupleDetails = new clsCoupleVO();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails != null && ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.CoupleId > 0)
                    {
                        
                        CoupleInfo.Visibility = Visibility.Visible;
                        BizAction.CoupleDetails.MalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient;
                        BizAction.CoupleDetails.FemalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient;
                        CoupleDetails.MalePatient = new clsPatientGeneralVO();
                        CoupleDetails.FemalePatient = new clsPatientGeneralVO();
                        CoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails;
                        //if (CoupleDetails.CoupleId == 0)
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //         new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot Create Examination, Examination is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //    msgW1.Show();
                        //    //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                        //    //((IInitiateCIMS)App.Current).Initiate("VISIT");
                        //    ModuleName = "PalashDynamics";
                        //    Action = "CIMS.Forms.PatientList";
                        //    UserControl rootPage = Application.Current.RootVisual as UserControl;
                        //    WebClient c2 = new WebClient();
                        //    c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                        //    c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                        //}
                        //else
                        //{
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
                        // }
                    }
                    else
                    {
                       
                        #region Commented by Saily P on 260912. purpose, the form is applicable to donor patient
                        //MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //         new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot Create Examination, Examination is Only For Active Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //msgW1.Show();
                        ////((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                        ////((IInitiateCIMS)App.Current).Initiate("VISIT");

                        //ModuleName = "PalashDynamics";
                        //Action = "CIMS.Forms.PatientList";
                        //UserControl rootPage = Application.Current.RootVisual as UserControl;

                        //WebClient c2 = new WebClient();
                        //c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                        //c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                        #endregion
                    }
                   // FillFrontGrid();
                    FillSpremCollection();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
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
        private void FillFrontGrid() 
        {
            try
            {

             cls_NewGetListSpremFreezingBizActionVO BizActionObj = new cls_NewGetListSpremFreezingBizActionVO();

                BizActionObj.MalePatientID = CoupleDetails.MalePatient.PatientID;
                BizActionObj.MalePatientUnitID = CoupleDetails.MalePatient.UnitId;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((cls_NewGetListSpremFreezingBizActionVO)arg.Result).SpremFreezingMainList.Count > 0)
                        {
                            dgfreezing.ItemsSource = null;
                            dgfreezing.ItemsSource = ((cls_NewGetListSpremFreezingBizActionVO)arg.Result).SpremFreezingMainList;
                            dgfreezing.UpdateLayout();
                        }
                    }
                };
                client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsCancel = true;
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "New":
                    cmdNew.IsEnabled = false;
                    CmdSave.IsEnabled = true;
                    CmdClose.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Save":
                    cmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = false;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = true;
                 
                    break;
                default:
                    break;
            }
        }
  
        string cyclecode;
        private void hlbViewSpermfreezing_Click(object sender, RoutedEventArgs e)
        {
            this.SetCommandButtonState("Modify");
            cyclecode = ((cls_NewSpremFreezingMainVO)dgfreezing.SelectedItem).CycleCode;
            CmbCycleCode.IsEnabled = false;
            isnew = false;
            getCycleCode();
          //  FillGrid();
            objAnimation.Invoke(RotationType.Forward);

        }
        private SwivelAnimation objAnimation;
        bool isnew = false;
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            this.SetCommandButtonState("New");
            CmbCycleCode.IsEnabled = true;
            clearUI();
            cyclecode = string.Empty;
            getCycleCode();
            ISEdit = false;
            isnew = true;
            //FillColor();
            //fillInitailFrezingDetails();
            CmdSave.Content = "Save";
            objAnimation.Invoke(RotationType.Forward);
        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                try
                {
                    string msgTitle = "Palash";
                    string msgText = "Are You Sure \n You Want To Save Sperm Freezing Details";
                    MessageBoxControl.MessageBoxChildWindow msgWin =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                    msgWin.Show();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        WaitIndicator wait = new WaitIndicator();
        public bool Statusdetails;
        public cls_NewSpremFreezingMainVO objSparmVO = new cls_NewSpremFreezingMainVO();
        int j = 0;
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                wait.Show();
                try
                {
                    cls_NewAddUpdateSpremFreezingBizActionVO BizActionObj = new cls_NewAddUpdateSpremFreezingBizActionVO();

                    BizActionObj.SpremFreezingVO = new clsNew_SpremFreezingVO();
                    BizActionObj.SpremFreezingMainVO = new cls_NewSpremFreezingMainVO();

                    // Grid Details
                    BizActionObj.SpremFreezingDetails = SpremFreezingDetails;
                    for (int i = 0; i < SpremFreezingDetails.Count; i++)
                    {
                        if (SpremFreezingDetails[i].ID != 0)
                        {
                            BizActionObj.SpremFreezingDetails[i].ID = SpremFreezingDetails[i].ID;
                        }
                        else
                        {
                            BizActionObj.SpremFreezingDetails[i].ID = 0;
                        }
                        BizActionObj.SpremFreezingDetails[i].GobletSizeId = SpremFreezingDetails[i].selectedGlobletSizeListVO.ID;
                        BizActionObj.SpremFreezingDetails[i].GobletColorID = SpremFreezingDetails[i].selectedColorList.ID;
                        BizActionObj.SpremFreezingDetails[i].GobletShapeId = SpremFreezingDetails[i].selectedGlobletShapeListVO.ID;
                        BizActionObj.SpremFreezingDetails[i].CanID = SpremFreezingDetails[i].selectedCanListVO.ID;
                        BizActionObj.SpremFreezingDetails[i].CanisterId = SpremFreezingDetails[i].selectedCanisterListVO.ID;
                        BizActionObj.SpremFreezingDetails[i].StrawId = SpremFreezingDetails[i].selectedStrawListVO.ID;
                        BizActionObj.SpremFreezingDetails[i].TankId = SpremFreezingDetails[i].selectedTankListVO.ID;
                        BizActionObj.SpremFreezingDetails[i].Status = SpremFreezingDetails[i].Status;
                        BizActionObj.SpremFreezingDetails[i].IsThaw = SpremFreezingDetails[i].IsThaw;
                        BizActionObj.SpremFreezingDetails[i].IsModify = ISEdit;
                        BizActionObj.SpremFreezingDetails[i].Status = Statusdetails;
                        if (SpremFreezingDetails[i].SpremNostr.Equals("Auto Generated"))
                        {
                            SpremFreezingDetails[i].SpremNostr = "0";
                        }
                        //By Anjali............
                        j = i + 1;
                        BizActionObj.SpremFreezingDetails[i].SpremNostr = Convert.ToString(j);
                    }

                    // Main Table Details. .
                    BizActionObj.MalePatientID = CoupleDetails.MalePatient.PatientID;
                    BizActionObj.MalePatientUnitID = CoupleDetails.MalePatient.UnitId;

                    //........................................................
                    BizActionObj.SpremFreezingMainVO.TherapyID = ((clsPlanTherapyVO)TherapyDetails.DataContext).ID;
                    BizActionObj.SpremFreezingMainVO.TherapyUnitID= ((clsPlanTherapyVO)TherapyDetails.DataContext).UnitID;
                    BizActionObj.SpremFreezingMainVO.CycleCode = CmbCycleCode.SelectedItem.ToString();
                    //........................................................

                    if (ISEdit==true)
                    {
                        BizActionObj.ID = objSparmVO.ID;
                    }
                    else
                    {
                        BizActionObj.ID = 0;
                    }

                    BizActionObj.SpremFreezingMainVO.SpremFreezingDate = SpremFreezDate.SelectedDate.Value;
                    BizActionObj.SpremFreezingMainVO.SpremFreezingTime = BizActionObj.SpremFreezingMainVO.SpremFreezingDate.Value.Add(SpremFreezTime.Value.Value.TimeOfDay);
                    BizActionObj.SpremFreezingMainVO.DoctorID = ((MasterListItem)cmbDoctorName.SelectedItem).ID;
                    BizActionObj.SpremFreezingMainVO.EmbryologistID = ((MasterListItem)cmbEmbryologist.SelectedItem).ID;

                    BizActionObj.SpremFreezingMainVO.CollectionMethodID = ((MasterListItem)cmbSpremCollecionMethod.SelectedItem).ID;
                    BizActionObj.SpremFreezingMainVO.CollectionProblem = Convert.ToString(txtcollectnprblm.Text.Trim());
                    BizActionObj.SpremFreezingMainVO.Abstience = Convert.ToString(txtAbstinence.Text.Trim());
                    BizActionObj.SpremFreezingMainVO.Volume = Convert.ToSingle(txtvolume.Text.Trim());

                    BizActionObj.SpremFreezingMainVO.ViscosityID = ((MasterListItem)cmbViscosity.SelectedItem).ID;
                    BizActionObj.SpremFreezingMainVO.GradeA = Convert.ToDecimal(txtGradeA.Text.Trim());
                    BizActionObj.SpremFreezingMainVO.GradeB = Convert.ToDecimal(txtGradeB.Text.Trim());
                    BizActionObj.SpremFreezingMainVO.GradeC = Convert.ToDecimal(txtGradeC.Text.Trim());

                    BizActionObj.SpremFreezingMainVO.TotalSpremCount = Convert.ToInt64(txtTotalSpremCount.Text.Trim());
                    BizActionObj.SpremFreezingMainVO.Motility = Convert.ToDecimal(txtMotility.Text.Trim());
                    BizActionObj.SpremFreezingMainVO.Other = Convert.ToString(txtOtherDetails.Text.Trim());
                    BizActionObj.SpremFreezingMainVO.Comments = Convert.ToString(txtComments.Text.Trim());
                    BizActionObj.SpremFreezingMainVO.Status = true;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Semen Freezing Details Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.OK)
                                {
                                    FillFrontGrid();
                                    objAnimation.Invoke(RotationType.Backward);
                                    this.SetCommandButtonState("Load");
                                }
                            };
                            msgW1.Show();
                            
                            
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                    };
                    client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();

                    wait.Close();
                }
                catch (Exception ex)
                {
                    wait.Show();
                    throw ex;
                }
            }
        }
        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            if (IsCancel == true)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                ModuleName = "PalashDynamics";
                Action = "CIMS.Forms.PatientList";
                UserControl rootPage = Application.Current.RootVisual as UserControl;

                WebClient c2 = new WebClient();
                c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                objAnimation.Invoke(RotationType.Backward);
                IsCancel = true;
                CmdSave.Content = "Save";
              //  FetchData();
            }
            SetCommandButtonState("Cancel");
        }
        private void CmbCycleCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbCycleCode.SelectedItem !=null)
                if (CmbCycleCode.SelectedItem.ToString() != "--Select--")
                {
                    GetCycleDetailsFromCycleCodeBizActionVO BizAction = new GetCycleDetailsFromCycleCodeBizActionVO();

                    BizAction.CycleCode = CmbCycleCode.SelectedItem.ToString();

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    Client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            TherapyDetails.DataContext = ((GetCycleDetailsFromCycleCodeBizActionVO)args.Result).TherapyDetails;
                           // FillSpremCollection();
                            //FillColor();
                           
                                 FillGrid();
                            CmdSave.IsEnabled = true;
                            DateTimeSF.IsEnabled = true;
                            dgSpremFreezingDetailsGrid.IsEnabled = true;
                            DoctorSF.IsEnabled = true;
                            FreezingDetailsSF.IsEnabled = true;
                            GradeSF.IsEnabled = true;
                            DetailsSF.IsEnabled = true;

                        }
                    };
                    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client.CloseAsync();
                }
                else
                {
                    TherapyDetails.DataContext = null;
                    CmdSave.IsEnabled = false;
                    SpremFreezingDetails = new ObservableCollection<clsNew_SpremFreezingVO>();
                    DateTimeSF.IsEnabled = false;
                    dgSpremFreezingDetailsGrid.IsEnabled = false;
                    DoctorSF.IsEnabled = false;
                    FreezingDetailsSF.IsEnabled = false;
                    GradeSF.IsEnabled = false;
                    DetailsSF.IsEnabled = false;
                }
        }
        void FillSpremCollection()
        {
            wait.Show();
            try
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
                        cmbSpremCollecionMethod.ItemsSource = null;
                        cmbSpremCollecionMethod.ItemsSource = objList;
                        cmbSpremCollecionMethod.SelectedItem = objList[0];
                    }
                    FillViscosity();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
                wait.Close();
            }
            
        }

        void FillViscosity()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVF_Viscosity;
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
                        cmbViscosity.ItemsSource = null;
                        cmbViscosity.ItemsSource = objList;
                        cmbViscosity.SelectedItem = objList[0];
                    }
                    if (this.DataContext != null)
                    {
                        //cmbSrcTreatmentPlan.SelectedValue = ((clsVO)this.DataContext).;
                    }
                    FillDoctor();
                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
                wait.Close();
            }
        }

        void FillDoctor()
        {
            try
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

                        cmbDoctorName.ItemsSource = null;
                        cmbDoctorName.ItemsSource = objList;
                        cmbDoctorName.SelectedValue = (long)0;
                    }
                    FillEmbryologist();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
                wait.Close();
            }
        }

        void FillEmbryologist()
        {
           try{
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

                    cmbEmbryologist.ItemsSource = null;
                    cmbEmbryologist.ItemsSource = objList;
                    cmbEmbryologist.SelectedItem = objList[0];
                }
                 FillColor();
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        catch (Exception ex)
            {
                throw ex;
                wait.Close();
            }
        }

        private void clearUI() 
        {
            SpremFreezDate.SelectedDate = null;
            SpremFreezTime.Value = null;
            cmbDoctorName.SelectedValue = (long)0;
            cmbEmbryologist.SelectedValue= (long)0;
            txtAbstinence.Text = string.Empty;
            txtcollectnprblm.Text= string.Empty;
            txtComments.Text = string.Empty;
            txtGradeA.Text= string.Empty;
            txtGradeB.Text= string.Empty;
            txtGradeC.Text= string.Empty;
            txtMotility.Text= string.Empty;
            txtOtherDetails.Text= string.Empty;
            txtTotalSpremCount.Text= string.Empty;
            txtvolume.Text= string.Empty;
            cmbSpremCollecionMethod.SelectedValue=(long)0;
            cmbViscosity.SelectedValue=(long)0;
            SpremFreezingDetails = new ObservableCollection<clsNew_SpremFreezingVO>();
            dgSpremFreezingDetailsGrid.ItemsSource = null;

        }
        void FillGrid()
        {
            wait.Show();
            try
            {
                if(isnew!=true)
                {
                    cls_NewGetSpremFreezingBizActionVO BizActionObj = new cls_NewGetSpremFreezingBizActionVO();

                //BizActionObj.MalePatientID = CoupleDetails.MalePatient.PatientID;
                //BizActionObj.MalePatientUnitID = CoupleDetails.MalePatient.UnitId;
                BizActionObj.ID = ((cls_NewSpremFreezingMainVO)dgfreezing.SelectedItem).ID;
                BizActionObj.UintID = ((cls_NewSpremFreezingMainVO)dgfreezing.SelectedItem).UnitID;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Status == true)
                        {
                            objSparmVO = ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO;
                            SpremFreezDate.SelectedDate = ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.SpremFreezingDate;
                            SpremFreezTime.Value = ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.SpremFreezingTime;
                            cmbDoctorName.SelectedValue = (long)((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.DoctorID;
                            cmbEmbryologist.SelectedValue = (long)((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.EmbryologistID;

                            txtAbstinence.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Abstience);
                            txtcollectnprblm.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.CollectionProblem);
                            txtComments.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Comments);
                            txtGradeA.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.GradeA);
                            txtGradeB.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.GradeB);
                            txtGradeC.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.GradeC);
                            txtMotility.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Motility);
                            txtOtherDetails.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Other);
                            txtTotalSpremCount.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.TotalSpremCount);
                            txtvolume.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Volume);

                            cmbSpremCollecionMethod.SelectedValue = (long)((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.CollectionMethodID;

                            cmbViscosity.SelectedValue = (long)((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.ViscosityID;
                            SpremFreezingDetails = new ObservableCollection<clsNew_SpremFreezingVO>();

                            for (int i = 0; i < ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails.Count; i++)
                            {
                                ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].CanListVO = CanList;
                                ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].CanisterListVO = CanisterList;
                                ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].ColorListVO = ColorList;
                                ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].StrawListVO = StrawList;
                                ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].TankListVO = TankList;
                                ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].GlobletShapeListVO = GlobletShapeList;
                                ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].GlobletSizeListVO = GlobletSizeList;

                                if ((((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].CanID) >= 0)
                                {
                                    if (Convert.ToInt64(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].CanID) > 0)
                                    {
                                        ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].selectedCanListVO = CanList.FirstOrDefault(p => p.ID == Convert.ToInt64(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].CanID));
                                    }
                                    else
                                    {
                                        ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].selectedCanListVO = CanList.FirstOrDefault(p => p.ID == 0);
                                    }
                                }

                                //canister
                                if ((((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].CanisterId) >= 0)
                                {
                                    if (Convert.ToInt64(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].CanisterId) > 0)
                                    {
                                        ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].selectedCanisterListVO = CanisterList.FirstOrDefault(p => p.ID == Convert.ToInt64(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].CanisterId));
                                    }
                                    else
                                    {
                                        ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].selectedCanisterListVO = CanisterList.FirstOrDefault(p => p.ID == 0);
                                    }
                                }
                                //tank
                                if ((((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].TankId) >= 0)
                                {
                                    if (Convert.ToInt64(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].TankId) > 0)
                                    {
                                        ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].selectedTankListVO = TankList.FirstOrDefault(p => p.ID == Convert.ToInt64(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].TankId));
                                    }
                                    else
                                    {
                                        ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].selectedTankListVO = TankList.FirstOrDefault(p => p.ID == 0);
                                    }

                                }
                                //color
                                if ((((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].GobletColorID) >= 0)
                                {
                                    if (Convert.ToInt64(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].GobletColorID) > 0)
                                    {
                                        ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].selectedColorList = ColorList.FirstOrDefault(p => p.ID == Convert.ToInt64(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].GobletColorID));
                                    }
                                    else
                                    {
                                        ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].selectedColorList = ColorList.FirstOrDefault(p => p.ID == 0);
                                    }
                                }

                                //straw
                                if ((((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].StrawId) >= 0)
                                {
                                    if (Convert.ToInt64(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].StrawId) > 0)
                                    {
                                        ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].selectedStrawListVO = StrawList.FirstOrDefault(p => p.ID == Convert.ToInt64(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].StrawId));
                                    }
                                    else
                                    {
                                        ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].selectedStrawListVO = StrawList.FirstOrDefault(p => p.ID == 0);
                                    }
                                }
                                //shape
                                if ((((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].GobletShapeId) >= 0)
                                {
                                    if (Convert.ToInt64(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].GobletShapeId) > 0)
                                    {
                                        ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].selectedGlobletShapeListVO = GlobletShapeList.FirstOrDefault(p => p.ID == Convert.ToInt64(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].GobletShapeId));
                                    }
                                    else
                                    {
                                        ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].selectedGlobletShapeListVO = GlobletShapeList.FirstOrDefault(p => p.ID == 0);
                                    }
                                }

                                //size
                                if ((((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].GobletSizeId) >= 0)
                                {
                                    if (Convert.ToInt64(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].GobletSizeId) > 0)
                                    {
                                        ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].selectedGlobletSizeListVO = GlobletSizeList.FirstOrDefault(p => p.ID == Convert.ToInt64(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].GobletSizeId));
                                    }
                                    else
                                    {
                                        ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].selectedGlobletSizeListVO = GlobletSizeList.FirstOrDefault(p => p.ID == 0);
                                    }
                                }

                                if ((((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].IsThaw) == true)
                                {
                                    ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].IsThaw = true;
                                }
                                else
                                {
                                    ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i].IsThaw = false;
                                }
                                CmdSave.Content = "Modify";
                                ISEdit = true;
                                SpremFreezingDetails.Add(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingDetails[i]);
                                dgSpremFreezingDetailsGrid.ItemsSource = SpremFreezingDetails;
                            }
                            wait.Close();
                        }
                    }
                     };
                client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                    }
               else
                {
                    fillInitailFrezingDetails();

                  // BizActionObj.ISModify = false;
                    wait.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void fillInitailFrezingDetails()
        {
            SpremFreezingDetails.Add(new clsNew_SpremFreezingVO() { SpremNostr = "Auto Generated", GobletColorID = 0, ColorListVO = ColorList, StrawId = 0, StrawListVO = StrawList, GobletShapeId = 0, GlobletShapeListVO = GlobletShapeList, GobletSizeId = 0, GlobletSizeListVO = GlobletSizeList, CanID = 0, CanListVO = CanList, CanisterId = 0, CanisterListVO = CanisterList, TankId = 0, TankListVO = TankList, Comments = "", IsThaw = false });
            dgSpremFreezingDetailsGrid.ItemsSource = SpremFreezingDetails;
            wait.Close();
        }
    }

}
