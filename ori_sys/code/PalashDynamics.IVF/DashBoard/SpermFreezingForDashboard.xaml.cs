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
using System.Windows.Browser;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class SpermFreezingForDashboard : ChildWindow, IInitiateCIMS
    {
        public long PatientID = 0;
        public long PatientUnitID = 0;
        public long UnitID = 0;
        public long VisitID = 0;
        public bool IsFromDashBoard;

        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "Clinical":

                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        return;
                    }
                    else if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        return;
                    }
                    if (((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).Gender == "Male")
                    {
                        IsPatientExist = true;

                        PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                        PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;

                        break;

                    }
                    else
                    {
                        IsPatientExist = false;
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW5.Show();
                        return;
                    }

                default:
                    break;

            }

        }

        public SpermFreezingForDashboard()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
        }

        public clsCoupleVO CoupleDetails;

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
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
            else
            {
                Statusdetails = true;
                FillSpremCollection();
                this.SetCommandButtonState("Load");

                if (IsFromDashBoard == true)
                {
                    if (CoupleDetails != null)
                        this.Title = "Self Sperm Freezing  :-(Name- " + CoupleDetails.MalePatient.FirstName +
                              " " + CoupleDetails.MalePatient.LastName + ")";
                }
                else
                {
                    if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
                    {
                        this.Title = "Self Sperm Freezing  :-(Name- " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).FirstName +
                              " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).LastName + ")";
                    }
                }
            }
        }
        // private void getCycleCode() 
        //{
        //    GetCycleCodeForPatientBizActionVO BizAction = new GetCycleCodeForPatientBizActionVO();

        //    BizAction.CoupleUnitID = CoupleDetails.CoupleUnitId;
        //    BizAction.CoupleID = CoupleDetails.CoupleId;

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            CmbCycleCode.ItemsSource = null;
        //            List<string> objList = new List<string>();
        //            objList.Add("--Select--");
        //            objList.AddRange(((GetCycleCodeForPatientBizActionVO)e.Result).List.DeepCopyd());
        //            CmbCycleCode.ItemsSource = objList;


        //            if (cyclecode != string.Empty)
        //                CmbCycleCode.SelectedItem = cyclecode;
        //            else
        //                CmbCycleCode.SelectedItem = objList[0];


        //        }
        //    };
        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        //}
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
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

        //string textBefore = null;
        //int selectionStart = 0;
        //double selectionStartDecml = 0;
        //int selectionLength = 0;



        #endregion

        // Grid Add click

        bool ISEdit = false;
        #region Validation
        public Boolean Validation()
        {
            bool result = true;

            if (SpremFreezDate.SelectedDate == null)
            {
                SpremFreezDate.SetValidation("Please Select Freezing Date");
                SpremFreezDate.RaiseValidationError();
                SpremFreezDate.Focus();
                result = false;     //return false;
            }
            else
                SpremFreezDate.ClearValidationError();

            if (SpremFreezTime.Value == null)
            {
                //SpremFreezDate.ClearValidationError();
                SpremFreezTime.SetValidation("Please Select Freezing Time");
                SpremFreezTime.RaiseValidationError();
                SpremFreezTime.Focus();
                result = false;     //return false;
            }
            else
                SpremFreezTime.ClearValidationError();

            DateTime? FreezingDate = new DateTime();
            DateTime? ExpiryDate = new DateTime();
            DateTime CurrentDate = DateTime.Now.Date;

            FreezingDate = SpremFreezDate.SelectedDate.Value.Date;
            //FreezingDate = FreezingDate.Value.Add(SpremFreezTime.Value.Value.TimeOfDay);

            ExpiryDate = dtExpiryDate.SelectedDate.Value.Date;
            //ExpiryDate = ExpiryDate.Value.Add(txtExpiryTime.Value.Value.TimeOfDay);

            if (FreezingDate > ExpiryDate)
            {
                dtExpiryDate.SetValidation("Expiry Date Should Be Greater Than Freezing Date");
                dtExpiryDate.RaiseValidationError();
                dtExpiryDate.Focus();
                result = false;
            }
            else
                dtExpiryDate.ClearValidationError();

            if (FreezingDate > CurrentDate)
            {
                SpremFreezDate.SetValidation("Freezing Date Should Not Be Future Date");
                SpremFreezDate.RaiseValidationError();
                SpremFreezDate.Focus();
                result = false;
            }
            else
                SpremFreezDate.ClearValidationError();


            if (TxtSpermCount.Text == null || TxtSpermCount.Text.Trim() == "")
            {
                //SpremFreezDate.ClearValidationError();
                //SpremFreezTime.ClearValidationError();
                //cmbEmbryologist.TextBox.ClearValidationError();
                TxtSpermCount.SetValidation("Sperm Concentration  Is Required");
                TxtSpermCount.RaiseValidationError();
                TxtSpermCount.Focus();
                result = false;     //return false; //result = false;
            }
            else
                TxtSpermCount.ClearValidationError();


            if (txtGradeA.Text == null || txtGradeA.Text.Trim() == "")
            {
                //SpremFreezDate.ClearValidationError();
                //SpremFreezTime.ClearValidationError();
                //cmbEmbryologist.TextBox.ClearValidationError();
                txtGradeA.SetValidation("Progressive Motility  Is Required");
                txtGradeA.RaiseValidationError();
                txtGradeA.Focus();
                result = false;     //return false; //result = false;
            }
            else
                txtGradeA.ClearValidationError();

            if (txtGradeB.Text == null || txtGradeB.Text.Trim() == "")
            {
                //SpremFreezDate.ClearValidationError();
                //SpremFreezTime.ClearValidationError();
                //cmbEmbryologist.TextBox.ClearValidationError();
                txtGradeB.SetValidation("NonProgressive Motility  Is Required");
                txtGradeB.RaiseValidationError();
                txtGradeB.Focus();
                result = false;     //return false; //result = false;
            }
            else
                txtGradeB.ClearValidationError();

            if (cmbEmbryologist.SelectedItem == null || ((MasterListItem)cmbEmbryologist.SelectedItem).ID == 0)
            {
                //SpremFreezDate.ClearValidationError();
                //SpremFreezTime.ClearValidationError();
                cmbEmbryologist.TextBox.SetValidation("Please Select Andrologist");    //cmbEmbryologist.TextBox.SetValidation("Please Select Embryologist");
                cmbEmbryologist.TextBox.RaiseValidationError();
                cmbEmbryologist.Focus();
                result = false;     //return false;
            }
            else
                cmbEmbryologist.TextBox.ClearValidationError();

            if (txtvolume.Text == null || txtvolume.Text.Trim() == "")
            {
                //SpremFreezDate.ClearValidationError();
                //SpremFreezTime.ClearValidationError();
                //cmbEmbryologist.TextBox.ClearValidationError();
                txtvolume.SetValidation("Volume  Is Required");
                txtvolume.RaiseValidationError();
                txtvolume.Focus();
                result = false;     //return false; //result = false;
            }
            else if (Convert.ToDouble(txtvolume.Text) >= 10)
            {
                //SpremFreezDate.ClearValidationError();
                //SpremFreezTime.ClearValidationError();
                //cmbEmbryologist.TextBox.ClearValidationError();
                txtvolume.SetValidation("Volume Should Be less than 10 ");
                txtvolume.RaiseValidationError();
                txtvolume.Focus();

                result = false;     //return false; //result = false;
            }
            else
                txtvolume.ClearValidationError();

            if (txtNoofVials.Text == null || txtNoofVials.Text == string.Empty)
            {
                //SpremFreezDate.ClearValidationError();
                //SpremFreezTime.ClearValidationError();
                //cmbEmbryologist.TextBox.ClearValidationError();
                ////cmbDoctorName.TextBox.ClearValidationError();     // commented on 15052017
                //txtvolume.ClearValidationError();
                txtNoofVials.SetValidation("Please Enter Number of Vails");
                txtNoofVials.RaiseValidationError();
                txtNoofVials.Focus();
                result = false;     //return false;
            }
            else if (Convert.ToInt16(txtNoofVials.Text.Trim()) <= 0)
            {
                txtNoofVials.SetValidation("Number of Vails should be greater than zero");
                txtNoofVials.RaiseValidationError();
                txtNoofVials.Focus();
                return false;
            }
            else
            {
                //SpremFreezDate.ClearValidationError();
                //SpremFreezTime.ClearValidationError();
                //cmbEmbryologist.TextBox.ClearValidationError();
                ////cmbDoctorName.TextBox.ClearValidationError();     // commented on 15052017
                //txtvolume.ClearValidationError();
                txtNoofVials.ClearValidationError();
                //result = false;     //return true;
            }

            if (SpremFreezingDetails.Count <= 0)
            {
                //SpremFreezDate.ClearValidationError();
                //SpremFreezTime.ClearValidationError();
                //cmbEmbryologist.TextBox.ClearValidationError();
                ////cmbDoctorName.TextBox.ClearValidationError();     // commented on 15052017
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Freezing Details Data Grid Cannot Be Empty", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                result = false;     //return false;
            }

            return result;

            #region Commented

            //if (SpremFreezDate.SelectedDate == null)
            //{
            //    SpremFreezDate.SetValidation("Please Select Freezing Date");
            //    SpremFreezDate.RaiseValidationError();
            //    SpremFreezDate.Focus();
            //    return false;
            //}
            //else if (SpremFreezTime.Value == null)
            //{
            //    SpremFreezDate.ClearValidationError();
            //    SpremFreezTime.SetValidation("Please Select Freezing Time");
            //    SpremFreezTime.RaiseValidationError();
            //    SpremFreezTime.Focus();
            //    return false;
            //}
            //else if (cmbEmbryologist.SelectedItem == null || ((MasterListItem)cmbEmbryologist.SelectedItem).ID == 0)
            //{
            //    SpremFreezDate.ClearValidationError();
            //    SpremFreezTime.ClearValidationError();
            //    cmbEmbryologist.TextBox.SetValidation("Please Select Andrologist");    //cmbEmbryologist.TextBox.SetValidation("Please Select Embryologist");
            //    cmbEmbryologist.TextBox.RaiseValidationError();
            //    cmbEmbryologist.Focus();
            //    return false;
            //}

            //// commented on 15052017
            ////else if (cmbDoctorName.SelectedItem == null || ((MasterListItem)cmbDoctorName.SelectedItem).ID == 0)
            ////{
            ////    SpremFreezDate.ClearValidationError();
            ////    SpremFreezTime.ClearValidationError();
            ////    cmbEmbryologist.TextBox.ClearValidationError();

            ////    cmbDoctorName.TextBox.SetValidation("Please Select Anesthetist");
            ////    cmbDoctorName.TextBox.RaiseValidationError();
            ////    cmbDoctorName.Focus();
            ////    return false;
            ////}

            //else if (SpremFreezingDetails.Count <= 0)
            //{
            //    SpremFreezDate.ClearValidationError();
            //    SpremFreezTime.ClearValidationError();
            //    cmbEmbryologist.TextBox.ClearValidationError();
            //    //cmbDoctorName.TextBox.ClearValidationError();     // commented on 15052017
            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                new MessageBoxControl.MessageBoxChildWindow("Palash", "Freezing Details Data Grid Cannot Be Empty", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    msgW1.Show();
            //    return false;
            //}
            ////else if (txtvolume.Text == null)
            ////{
            ////    SpremFreezDate.ClearValidationError();
            ////    SpremFreezTime.ClearValidationError();
            ////    cmbEmbryologist.TextBox.ClearValidationError();
            ////    //cmbDoctorName.TextBox.ClearValidationError();     // commented on 15052017
            ////    txtvolume.SetValidation("Please Enter Volume .");
            ////    txtvolume.RaiseValidationError();
            ////    txtvolume.Focus();
            ////    return false;
            ////}
            //else if (txtvolume.Text == null || txtvolume.Text.Trim() == "")
            //{
            //    SpremFreezDate.ClearValidationError();
            //    SpremFreezTime.ClearValidationError();
            //    cmbEmbryologist.TextBox.ClearValidationError();
            //    txtvolume.SetValidation("Volume  Is Required");
            //    txtvolume.RaiseValidationError();
            //    txtvolume.Focus();
            //    return false; //result = false;
            //}
            //else if (Convert.ToDouble(txtvolume.Text) > 10)
            //{
            //    SpremFreezDate.ClearValidationError();
            //    SpremFreezTime.ClearValidationError();
            //    cmbEmbryologist.TextBox.ClearValidationError();
            //    txtvolume.SetValidation("Volume Should Be less than 10 ");
            //    txtvolume.RaiseValidationError();
            //    txtvolume.Focus();

            //    return false; //result = false;
            //}
            //else if (txtNoofVials.Text == string.Empty)
            //{
            //    SpremFreezDate.ClearValidationError();
            //    SpremFreezTime.ClearValidationError();
            //    cmbEmbryologist.TextBox.ClearValidationError();
            //    //cmbDoctorName.TextBox.ClearValidationError();     // commented on 15052017
            //    txtvolume.ClearValidationError();
            //    txtNoofVials.SetValidation("Please Enter Number of Vails");
            //    txtNoofVials.RaiseValidationError();
            //    txtNoofVials.Focus();
            //    return false;
            //}
            //else if (Convert.ToInt16(txtNoofVials.Text.Trim()) <= 0)
            //{
            //    txtNoofVials.SetValidation("Number of Vails should be greater than zero");
            //    txtNoofVials.RaiseValidationError();
            //    txtNoofVials.Focus();
            //    return false;
            //}
            //else
            //{
            //    SpremFreezDate.ClearValidationError();
            //    SpremFreezTime.ClearValidationError();
            //    cmbEmbryologist.TextBox.ClearValidationError();
            //    //cmbDoctorName.TextBox.ClearValidationError();     // commented on 15052017
            //    txtvolume.ClearValidationError();
            //    txtNoofVials.ClearValidationError();
            //    txtvolume.ClearValidationError();
            //    return true;
            //}

            #endregion

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

                    //FillFrontGrid();  // commented on 15/05/2017 For Andrology Flow
                    //// FillGrid();

                    FillAbstience();  // added on 15/05/2017 For Andrology Flow

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

        #region For Andrology Flow added on 15/05/2017

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

                    //if (Flagref == true)
                    //{
                    //    var results = from r in objList
                    //                  where r.ID == 10
                    //                  select r;
                    //    cmbAbstience.ItemsSource = null;
                    //    cmbAbstience.ItemsSource = results.ToList();
                    //    cmbAbstience.SelectedItem = results.ToList()[0];
                    //}
                    //else
                    //{
                    txtAbstinence.ItemsSource = null;
                    txtAbstinence.ItemsSource = objList;
                    txtAbstinence.SelectedItem = objList[0];
                    //}
                    //cmbAbstience.ItemsSource = null;
                    //cmbAbstience.ItemsSource = objList;
                }

                //if (this.DataContext != null)
                //{
                //    //cmbAbstience.SelectedValue = ((cls_IVFDashboard_SemenVO)this.DataContext).AbstinenceID;
                //}

                FillResultMaster();

            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
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

                    FillFrontGrid();    // For Andrology Flow added on 15/05/2017 

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

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {

        }

        //private void txtTotalSpremCount_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (!((TextBox)sender).Text.IsValidTwoDigit() && textBefore != null)
        //    {
        //        ((TextBox)sender).Text = textBefore;
        //        ((TextBox)sender).SelectionStart = selectionStart;
        //        ((TextBox)sender).SelectionLength = selectionLength;
        //        textBefore = "";
        //        selectionStart = 0;
        //        selectionLength = 0;
        //    }
        //}

        //private void txtTotalSpremCount_KeyDown(object sender, KeyEventArgs e)
        //{
        //    textBefore = ((TextBox)sender).Text;
        //    selectionStart = ((TextBox)sender).SelectionStart;
        //    selectionLength = ((TextBox)sender).SelectionLength;
        //}

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

        string textBeforeGradeA = null;
        int selectionStartGradeA = 0;
        double selectionStartDecmlGradeA = 0;
        int selectionLengthGradeA = 0;

        //private void txtGradeA_KeyDown(object sender, KeyEventArgs e)
        //{
        //    textBeforeGradeA = ((TextBox)sender).Text;
        //    selectionStartGradeA = ((TextBox)sender).SelectionStart;
        //    selectionLengthGradeA = ((TextBox)sender).SelectionLength;
        //}

        //private void txtGradeA_TextChanged(object sender, TextChangedEventArgs e)
        //{
            

        //    if (!((TextBox)sender).Text.IsValidTwoDigit() && !(string.IsNullOrEmpty(textBeforeGradeA)))  //textBeforeGradeA != null
        //    {
        //        ((TextBox)sender).Text = textBeforeGradeA;
        //        ((TextBox)sender).SelectionStart = selectionStartGradeA;
        //        ((TextBox)sender).SelectionLength = selectionLengthGradeA;
        //        textBeforeGradeA = "";
        //        selectionStartGradeA = 0;
        //        selectionLengthGradeA = 0;
        //    }
        //    else
        //    {
        //        Total_TextChanged();
        //    }
        //}

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


                if (txtGradeA.Text == "" || txtGradeA.Text == null)               // if (TxtPreGradeI.Text == "" || TxtPreGradeI.Text == null)
                    varpreRapid = 0;
                else
                    varpreRapid = Convert.ToDecimal(txtGradeA.Text.Trim());      // Convert.ToDecimal(TxtPreGradeI.Text.Trim());


                if (txtGradeB.Text == "" || txtGradeB.Text == null)             // if (TxtPreGradeII.Text == "" || TxtPreGradeII.Text == null)
                    varpreSlow = 0;
                else
                    varpreSlow = Convert.ToDecimal(txtGradeB.Text.Trim());  // Convert.ToDecimal(TxtPreGradeII.Text.Trim());

                //if (TxtPreGradeIII.Text == "" || TxtPreGradeIII.Text == null)
                //    varpreNon = 0;
                //else
                //    varpreNon = Convert.ToDecimal(TxtPreGradeIII.Text.Trim());

                PreSumOfGrade = (Convert.ToDecimal(varpreRapid)) + (Convert.ToDecimal(varpreSlow)) + (Convert.ToDecimal(varpreNon));

                //TxtTotalMotility.Text = PreSumOfGrade.ToString();
                txtMotility.Text = PreSumOfGrade.ToString();

                ////txtSpermConcentration.Text = SumOfGrade.ToString();

                //TxtPreGradeIV.Text = (100 - PreSumOfGrade).ToString();

                if (PreSumOfGrade > 0)
                    txtGradeC.Text = (100 - PreSumOfGrade).ToString();
            }
            catch (Exception ex)
            {
                //App.ErrorLog(ex);

            }
        }

        string textBeforeGradeB = null;
        int selectionStartGradeB = 0;
        double selectionStartDecmlGradeB = 0;
        int selectionLengthGradeB = 0;

        //private void txtGradeB_KeyDown(object sender, KeyEventArgs e)
        //{
        //    textBeforeGradeB = ((TextBox)sender).Text;
        //    selectionStartGradeB = ((TextBox)sender).SelectionStart;
        //    selectionLengthGradeB = ((TextBox)sender).SelectionLength;
        //}

        //private void txtGradeB_TextChanged(object sender, TextChangedEventArgs e)
        //{
            

        //    if (!((TextBox)sender).Text.IsValidTwoDigit() && !(string.IsNullOrEmpty(textBeforeGradeB)))    //textBeforeGradeB != null
        //    {
        //        ((TextBox)sender).Text = textBeforeGradeB;
        //        ((TextBox)sender).SelectionStart = selectionStartGradeB;
        //        ((TextBox)sender).SelectionLength = selectionLengthGradeB;
        //        textBeforeGradeB = "";
        //        selectionStartGradeB = 0;
        //        selectionLengthGradeB = 0;
        //    }
        //    else
        //    {
        //        Total_TextChanged();
        //    }
        //}

        string textBeforeGradeC = null;
        int selectionStartGradeC = 0;
        double selectionStartDecmlGradeC = 0;
        int selectionLengthGradeC = 0;

        //private void txtGradeC_KeyDown(object sender, KeyEventArgs e)
        //{
        //    textBeforeGradeC = ((TextBox)sender).Text;
        //    selectionStartGradeC = ((TextBox)sender).SelectionStart;
        //    selectionLengthGradeC = ((TextBox)sender).SelectionLength;
        //}

        //private void txtGradeC_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (!((TextBox)sender).Text.IsValueDouble() && textBeforeGradeC != null)
        //    {
        //        ((TextBox)sender).Text = textBeforeGradeC;
        //        ((TextBox)sender).SelectionStart = selectionStartGradeC;
        //        ((TextBox)sender).SelectionLength = selectionLengthGradeC;
        //        textBeforeGradeC = "";
        //        selectionStartGradeC = 0;
        //        selectionLengthGradeC = 0;
        //    }
        //    else
        //    {
        //        Total_TextChanged();
        //    }
        //}

        private void dgSpremFreezingDetailsGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
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
        public bool IsPatientExist;
        public string Action { get; set; }
        public string ModuleName { get; set; }
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
                ((IInitiateCIMS)myData).Initiate("IsAndrology");    //((IInitiateCIMS)myData).Initiate("VISIT");
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
                TextBlock Header = (TextBlock)rootPage.FindName("SampleHeader");
                Header.Text = "Andrology Queue List";

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void FillFrontGrid()
        {
            try
            {

                cls_NewGetListSpremFreezingBizActionVO BizActionObj = new cls_NewGetListSpremFreezingBizActionVO();

                BizActionObj.MalePatientID = PatientID;         //CoupleDetails.MalePatient.PatientID;
                BizActionObj.MalePatientUnitID = PatientUnitID;  // CoupleDetails.MalePatient.UnitId;

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
                    FillTypeSperm();
                };
                client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

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

                    var results = from r in objList
                                  where r.ID == 0 || r.ID == 1 || r.ID == 2 || r.ID == 3 || r.ID == 5 || r.ID == 6 || r.ID == 8
                                  select r;

                    cmbSpermType.ItemsSource = null;
                    cmbSpermType.ItemsSource = results.ToList();
                    cmbSpermType.SelectedItem = results.ToList()[0];

                    //if (Flagref == true)
                    //{
                    //    var results = from r in objList
                    //                  where r.ID == 10
                    //                  select r;
                    //    cmbSpermType.ItemsSource = null;
                    //    cmbSpermType.ItemsSource = results.ToList();
                    //    cmbSpermType.SelectedItem = results.ToList()[0];
                    //}
                    //else
                    //{
                    //    cmbSpermType.ItemsSource = null;
                    //    cmbSpermType.ItemsSource = objList;
                    //    cmbSpermType.SelectedItem = objList[0];
                    //}
                }

                if (this.DataContext != null)
                {
                    //cmbSpermType.SelectedValue = ((cls_IVFDashboard_SemenWashVO)this.DataContext).SpermTypeID;
                }

            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
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
                    chkFreeze.IsEnabled = true;
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
                    CmdSave.IsEnabled = true;
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
            clearUI();
            cyclecode = ((cls_NewSpremFreezingMainVO)dgfreezing.SelectedItem).CycleCode;
            objSparmVO = new cls_NewSpremFreezingMainVO();
            txtNoofVials.IsEnabled = false;
            //  CmbCycleCode.IsEnabled = false;
            isnew = false;
            //getCycleCode();
            FillGrid();
            objAnimation.Invoke(RotationType.Forward);

        }
        private SwivelAnimation objAnimation;
        bool isnew = false;
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient.IsClinicVisited == true)
            {
                this.SetCommandButtonState("New");
                clearUI();
                objSparmVO = new cls_NewSpremFreezingMainVO();
                // CmbCycleCode.IsEnabled = true;
                txtNoofVials.IsEnabled = true;
                //clearUI();
                cyclecode = string.Empty;
                // getCycleCode();
                //FillGrid();
                ISEdit = false;
                isnew = true;
                CmdSave.Content = "Save";
                objAnimation.Invoke(RotationType.Forward);
            }
            else if (IsFromDashBoard==true)
            {
                this.SetCommandButtonState("New");
                clearUI();
                objSparmVO = new cls_NewSpremFreezingMainVO();
                // CmbCycleCode.IsEnabled = true;
                txtNoofVials.IsEnabled = true;
                //clearUI();
                cyclecode = string.Empty;
                // getCycleCode();
                //FillGrid();
                ISEdit = false;
                isnew = true;
                CmdSave.Content = "Save";
                objAnimation.Invoke(RotationType.Forward);
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                new MessageBoxControl.MessageBoxChildWindow("Palash", "Current Visit is Closed", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
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
        public cls_NewSpremFreezingMainVO objSparmVO;
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
                        //BizActionObj.SpremFreezingDetails[i].GobletSizeId = SpremFreezingDetails[i].selectedGlobletSizeListVO.ID;
                        //BizActionObj.SpremFreezingDetails[i].GobletColorID = SpremFreezingDetails[i].selectedColorList.ID;
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
                    BizActionObj.MalePatientID = PatientID;         // CoupleDetails.MalePatient.PatientID;
                    BizActionObj.MalePatientUnitID = PatientUnitID;     // CoupleDetails.MalePatient.UnitId;

                    //........................................................
                    //BizActionObj.SpremFreezingMainVO.TherapyID = ((clsPlanTherapyVO)TherapyDetails.DataContext).ID;
                    //BizActionObj.SpremFreezingMainVO.TherapyUnitID= ((clsPlanTherapyVO)TherapyDetails.DataContext).UnitID;
                    //  BizActionObj.SpremFreezingMainVO.CycleCode = CmbCycleCode.SelectedItem.ToString();
                    //........................................................
                    BizActionObj.SpremFreezingMainVO.BatchID = objSparmVO.BatchID;
                    BizActionObj.SpremFreezingMainVO.BatchUnitID = objSparmVO.BatchUnitID;
                    if (ISEdit == true)
                    {
                        BizActionObj.ID = objSparmVO.ID;
                    }
                    else
                    {
                        BizActionObj.ID = 0;
                    }
                    if (SpremFreezDate.SelectedDate != null)
                        BizActionObj.SpremFreezingMainVO.SpremFreezingDate = SpremFreezDate.SelectedDate.Value.Date;
                    if (SpremFreezTime.Value != null)
                        BizActionObj.SpremFreezingMainVO.SpremFreezingDate = BizActionObj.SpremFreezingMainVO.SpremFreezingDate.Value.Add(SpremFreezTime.Value.Value.TimeOfDay);

                    if (dtExpiryDate.SelectedDate != null)
                        BizActionObj.SpremFreezingMainVO.SpremExpiryDate = dtExpiryDate.SelectedDate.Value.Date;
                    if (txtExpiryTime.Value != null)
                        BizActionObj.SpremFreezingMainVO.SpremExpiryDate = BizActionObj.SpremFreezingMainVO.SpremExpiryDate.Value.Add(txtExpiryTime.Value.Value.TimeOfDay);

                    // Commented on 15052017
                    //if (((MasterListItem)cmbDoctorName.SelectedItem) != null)
                    //    BizActionObj.SpremFreezingMainVO.DoctorID = ((MasterListItem)cmbDoctorName.SelectedItem).ID;
                    //if (((MasterListItem)cmbEmbryologist.SelectedItem) != null)
                    //    BizActionObj.SpremFreezingMainVO.EmbryologistID = ((MasterListItem)cmbEmbryologist.SelectedItem).ID;
                    //if (((MasterListItem)cmbSpremCollecionMethod.SelectedItem) != null)
                    //    BizActionObj.SpremFreezingMainVO.CollectionMethodID = ((MasterListItem)cmbSpremCollecionMethod.SelectedItem).ID;
                    //BizActionObj.SpremFreezingMainVO.CollectionProblem = txtcollectnprblm.Text;

                    //BizActionObj.SpremFreezingMainVO.Abstience = txtAbstinence.Text;      // For Andrology Flow commented on 15052017

                    if (txtvolume.Text != string.Empty)
                        BizActionObj.SpremFreezingMainVO.Volume = Convert.ToSingle(txtvolume.Text);

                    // Commented on 15052017
                    //if (((MasterListItem)cmbViscosity.SelectedItem) != null)
                    //    BizActionObj.SpremFreezingMainVO.ViscosityID = ((MasterListItem)cmbViscosity.SelectedItem).ID;

                    if (txtGradeA.Text != string.Empty)
                        BizActionObj.SpremFreezingMainVO.GradeA = Convert.ToDecimal(txtGradeA.Text);
                    if (txtGradeB.Text != string.Empty)
                        BizActionObj.SpremFreezingMainVO.GradeB = Convert.ToDecimal(txtGradeB.Text);
                    if (txtGradeC.Text != string.Empty)
                        BizActionObj.SpremFreezingMainVO.GradeC = Convert.ToDecimal(txtGradeC.Text);
                    if (txtTotalSpremCount.Text != string.Empty)
                        BizActionObj.SpremFreezingMainVO.TotalSpremCount = Convert.ToInt64(txtTotalSpremCount.Text.Trim());
                    if (txtMotility.Text != string.Empty)
                        BizActionObj.SpremFreezingMainVO.Motility = Convert.ToDecimal(txtMotility.Text);

                    BizActionObj.SpremFreezingMainVO.Other = txtOtherDetails.Text;
                    BizActionObj.SpremFreezingMainVO.Comments = txtComments.Text;
                    BizActionObj.SpremFreezingMainVO.Status = true;

                    BizActionObj.SpremFreezingMainVO.BatchCode = "--";
                    BizActionObj.SpremFreezingMainVO.LabID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.InhouseLabID;

                    if (txtNoofVials.Text != null)
                        BizActionObj.SpremFreezingMainVO.NoOfVails = Convert.ToSingle(txtNoofVials.Text);

                    if (TxtSampleCode.Text != null && !string.IsNullOrEmpty(TxtSampleCode.Text))
                    {
                        BizActionObj.SpremFreezingVO.SampleLinkID = Convert.ToInt64(TxtSampleCode.Text);
                    }

                    if (cmbSpermType.SelectedItem != null)
                        BizActionObj.SpremFreezingMainVO.SpermTypeID = ((MasterListItem)cmbSpermType.SelectedItem).ID;
                    BizActionObj.SpremFreezingMainVO.SampleCode = TxtSampleCode.Text.Trim();

                    #region For Andrology Flow added on 15052017

                    if (!string.IsNullOrEmpty(txtSpillage.Text.Trim()))
                        BizActionObj.SpremFreezingMainVO.Spillage = Convert.ToString(txtSpillage.Text);

                    if (TxtSpermCount.Text != string.Empty)
                        BizActionObj.SpremFreezingMainVO.SpermCount = Convert.ToDecimal(TxtSpermCount.Text);

                    if (txtDFI.Text != string.Empty)
                        BizActionObj.SpremFreezingMainVO.DFI = Convert.ToDecimal(txtDFI.Text);

                    if (txtROS.Text != string.Empty)
                        BizActionObj.SpremFreezingMainVO.ROS = Convert.ToDecimal(txtROS.Text);

                    if (cmbHIV.SelectedItem != null && ((MasterListItem)cmbHIV.SelectedItem).ID > 0)
                        BizActionObj.SpremFreezingMainVO.HIV = ((MasterListItem)cmbHIV.SelectedItem).ID;

                    if (cmbHBSAG.SelectedItem != null && ((MasterListItem)cmbHBSAG.SelectedItem).ID > 0)
                        BizActionObj.SpremFreezingMainVO.HBSAG = ((MasterListItem)cmbHBSAG.SelectedItem).ID;

                    if (cmbVDRL.SelectedItem != null && ((MasterListItem)cmbVDRL.SelectedItem).ID > 0)
                        BizActionObj.SpremFreezingMainVO.VDRL = ((MasterListItem)cmbVDRL.SelectedItem).ID;

                    if (cmbHCV.SelectedItem != null && ((MasterListItem)cmbHCV.SelectedItem).ID > 0)
                        BizActionObj.SpremFreezingMainVO.HCV = ((MasterListItem)cmbHCV.SelectedItem).ID;

                    if (cmbEmbryologist.SelectedItem != null && ((MasterListItem)cmbEmbryologist.SelectedItem).ID > 0)
                        BizActionObj.SpremFreezingMainVO.CheckedByDoctorID = ((MasterListItem)cmbEmbryologist.SelectedItem).ID;

                    if (chkConsentCheck != null)
                        BizActionObj.SpremFreezingMainVO.IsConsentCheck = Convert.ToBoolean(chkConsentCheck.IsChecked);

                    if (chkFreeze != null)
                        BizActionObj.SpremFreezingMainVO.IsFreezed = Convert.ToBoolean(chkFreeze.IsChecked);

                    //BizActionObj.SpremFreezingMainVO.Abstience = txtAbstinence.Text;
                    if (txtAbstinence.SelectedItem != null && ((MasterListItem)txtAbstinence.SelectedItem).ID > 0)
                        BizActionObj.SpremFreezingMainVO.AbstienceID = ((MasterListItem)txtAbstinence.SelectedItem).ID;

                    if (!string.IsNullOrEmpty(TxtPusCells.Text.Trim()))
                        BizActionObj.SpremFreezingMainVO.PusCells = Convert.ToString(TxtPusCells.Text);

                    if (!string.IsNullOrEmpty(TxtRoundCells.Text.Trim()))
                        BizActionObj.SpremFreezingMainVO.RoundCells = Convert.ToString(TxtRoundCells.Text);

                    if (!string.IsNullOrEmpty(TxtEpithelialCells.Text.Trim()))
                        BizActionObj.SpremFreezingMainVO.EpithelialCells = Convert.ToString(TxtEpithelialCells.Text);

                    if (!string.IsNullOrEmpty(TxtOtherCells.Text.Trim()))
                        BizActionObj.SpremFreezingMainVO.OtherCells = Convert.ToString(TxtOtherCells.Text);

                    if (IsFromDashBoard == false)
                    {
                        if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
                        {
                            BizActionObj.SpremFreezingMainVO.VisitID = ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).VisitID;
                            BizActionObj.SpremFreezingMainVO.VisitUnitID = ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).VisitUnitID;
                        }
                    }


                    #endregion

                    //added by neena                   
                    if (!string.IsNullOrEmpty(TxtSperm5thPercentile.Text.Trim()))
                        BizActionObj.SpremFreezingMainVO.Sperm5thPercentile = float.Parse(TxtSperm5thPercentile.Text.Trim());
                    if (!string.IsNullOrEmpty(TxtSperm75thPercentile.Text.Trim()))
                        BizActionObj.SpremFreezingMainVO.Sperm75thPercentile = float.Parse(TxtSperm75thPercentile.Text.Trim());
                    if (!string.IsNullOrEmpty(TxtEjaculate5thPercentile.Text.Trim()))
                        BizActionObj.SpremFreezingMainVO.Ejaculate5thPercentile = float.Parse(TxtEjaculate5thPercentile.Text.Trim());
                    if (!string.IsNullOrEmpty(TxtEjaculate75thPercentile.Text.Trim()))
                        BizActionObj.SpremFreezingMainVO.Ejaculate75thPercentile = float.Parse(TxtEjaculate75thPercentile.Text.Trim());
                    if (!string.IsNullOrEmpty(TxtTotalMotility5thPercentile.Text.Trim()))
                        BizActionObj.SpremFreezingMainVO.TotalMotility5thPercentile = float.Parse(TxtTotalMotility5thPercentile.Text.Trim());
                    if (!string.IsNullOrEmpty(TxtTotalMotility75thPercentile.Text.Trim()))
                        BizActionObj.SpremFreezingMainVO.TotalMotility75thPercentile = float.Parse(TxtTotalMotility75thPercentile.Text.Trim());
                    //

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", " Freezing Details Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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
                this.DialogResult = false;
            }
            else
            {
                objAnimation.Invoke(RotationType.Backward);
                IsCancel = true;
                CmdSave.Content = "Save";
                //  FetchData();
            }
            clearUI();
            SetCommandButtonState("Cancel");
        }
        //private void CmbCycleCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (CmbCycleCode.SelectedItem !=null)
        //        if (CmbCycleCode.SelectedItem.ToString() != "--Select--")
        //        {
        //            GetCycleDetailsFromCycleCodeBizActionVO BizAction = new GetCycleDetailsFromCycleCodeBizActionVO();

        //            BizAction.CycleCode = CmbCycleCode.SelectedItem.ToString();

        //            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //            Client.ProcessCompleted += (s, args) =>
        //            {
        //                if (args.Error == null && args.Result != null)
        //                {
        //                    TherapyDetails.DataContext = ((GetCycleDetailsFromCycleCodeBizActionVO)args.Result).TherapyDetails;
        //                   // FillSpremCollection();
        //                    //FillColor();

        //                         FillGrid();
        //                    CmdSave.IsEnabled = true;
        //                    //DateTimeSF.IsEnabled = true;
        //                    //dgSpremFreezingDetailsGrid.IsEnabled = true;
        //                    //DoctorSF.IsEnabled = true;
        //                    //FreezingDetailsSF.IsEnabled = true;
        //                    //GradeSF.IsEnabled = true;
        //                    //DetailsSF.IsEnabled = true;

        //                }
        //            };
        //            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //            Client.CloseAsync();
        //        }
        //        else
        //        {
        //            TherapyDetails.DataContext = null;
        //            CmdSave.IsEnabled = false;
        //            SpremFreezingDetails = new ObservableCollection<clsNew_SpremFreezingVO>();
        //            //DateTimeSF.IsEnabled = false;
        //            //dgSpremFreezingDetailsGrid.IsEnabled = false;
        //            //DoctorSF.IsEnabled = false;
        //            //FreezingDetailsSF.IsEnabled = false;
        //            //GradeSF.IsEnabled = false;
        //            //DetailsSF.IsEnabled = false;
        //        }
        //}
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
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList.Where(x => x.ID != 24).ToList());

                        // commented on 15052017
                        //cmbSpremCollecionMethod.ItemsSource = null;
                        //cmbSpremCollecionMethod.ItemsSource = objList;
                        //cmbSpremCollecionMethod.SelectedItem = objList[0];


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

                        // commented on 15052017
                        //cmbViscosity.ItemsSource = null;
                        //cmbViscosity.ItemsSource = objList;
                        //cmbViscosity.SelectedItem = objList[0];
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

                        // commented on 15052017
                        //cmbDoctorName.ItemsSource = null;
                        //cmbDoctorName.ItemsSource = objList;
                        //cmbDoctorName.SelectedValue = (long)0;
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
            try
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
            txtNoofVials.Text = string.Empty;
            //cmbDoctorName.SelectedValue = (long)0;    // commented on 15052017
            cmbEmbryologist.SelectedValue = (long)0;
            dtExpiryDate.SelectedDate = null;
            txtExpiryTime.Value = null;
            //txtAbstinence.Text = string.Empty;        // commented on 16052017
            //txtcollectnprblm.Text = string.Empty;     // commented on 15052017
            txtComments.Text = string.Empty;
            //txtGradeA.Text = "0";
            //txtGradeB.Text = "0";
            //txtGradeC.Text = "0";
            txtGradeA.Text = string.Empty;
            txtGradeB.Text = string.Empty;
            txtGradeC.Text = string.Empty;

            txtMotility.Text = string.Empty;
            txtOtherDetails.Text = string.Empty;
            txtTotalSpremCount.Text = string.Empty;
            txtvolume.Text = string.Empty;
            //cmbSpremCollecionMethod.SelectedValue = (long)0;  // commented on 15052017
            //cmbViscosity.SelectedValue = (long)0;             // commented on 15052017

            #region  For Andrology Flow Added on 16052017

            txtSpillage.Text = string.Empty;
            txtDFI.Text = string.Empty;
            txtROS.Text = string.Empty;

            TxtPusCells.Text = string.Empty;
            TxtRoundCells.Text = string.Empty;
            TxtEpithelialCells.Text = string.Empty;
            TxtOtherCells.Text = string.Empty;

            TxtSpermCount.Text = string.Empty;
            txtMotility.Text = string.Empty;

            cmbHIV.SelectedValue = (long)0;
            cmbHBSAG.SelectedValue = (long)0;
            cmbVDRL.SelectedValue = (long)0;
            cmbHCV.SelectedValue = (long)0;
            txtAbstinence.SelectedValue = (long)0;

            chkConsentCheck.IsChecked = false;
            chkFreeze.IsChecked = false;

            #endregion

            SpremFreezingDetails = new ObservableCollection<clsNew_SpremFreezingVO>();
            dgSpremFreezingDetailsGrid.ItemsSource = null;

        }
        void FillGrid()
        {
            wait.Show();
            try
            {
                if (isnew != true)
                {
                    cls_NewGetSpremFreezingBizActionVO BizActionObj = new cls_NewGetSpremFreezingBizActionVO();

                    //BizActionObj.MalePatientID = CoupleDetails.MalePatient.PatientID;
                    //BizActionObj.MalePatientUnitID = CoupleDetails.MalePatient.UnitId;
                    if ((cls_NewSpremFreezingMainVO)dgfreezing.SelectedItem != null)
                    {
                        BizActionObj.ID = ((cls_NewSpremFreezingMainVO)dgfreezing.SelectedItem).ID;
                        BizActionObj.UintID = ((cls_NewSpremFreezingMainVO)dgfreezing.SelectedItem).UnitID;
                    }


                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Status == true)
                            {
                                objSparmVO = ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO;

                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.SpermTypeID != null)
                                    cmbSpermType.SelectedValue = ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.SpermTypeID;
                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.SampleCode != null)
                                    TxtSampleCode.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.SampleCode);
                                
                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.SpremFreezingDate != null)
                                    SpremFreezDate.SelectedDate = ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.SpremFreezingDate;
                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.SpremFreezingTime != null)
                                    SpremFreezTime.Value = ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.SpremFreezingTime;

                                // commented on 15052017
                                //if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.DoctorID != null)
                                //    cmbDoctorName.SelectedValue = ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.DoctorID;

                                //if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.EmbryologistID != null)
                                //    cmbEmbryologist.SelectedValue = ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.EmbryologistID;        // For Andrology Flow Commented on 16052017

                                //if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Abstience != null)
                                //    txtAbstinence.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Abstience);    // For Andrology Flow Commented on 15052017

                                // commented on 15052017
                                //if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.CollectionProblem != null)
                                //    txtcollectnprblm.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.CollectionProblem);

                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.SpermCount != null)
                                    TxtSpermCount.Text = Convert.ToString(Convert.ToInt64(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.SpermCount));

                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Comments != null)
                                    txtComments.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Comments);
                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.GradeA != null)
                                    txtGradeA.Text = Convert.ToString(Convert.ToInt64(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.GradeA));
                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.GradeB != null)
                                    txtGradeB.Text = Convert.ToString(Convert.ToInt64(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.GradeB));
                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.GradeC != null)
                                    txtGradeC.Text = Convert.ToString(Convert.ToInt64(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.GradeC));
                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Motility != null)
                                    txtMotility.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Motility);
                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Other != null)
                                    txtOtherDetails.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Other);
                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.TotalSpremCount != null)
                                    txtTotalSpremCount.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.TotalSpremCount);
                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Volume != null)
                                    txtvolume.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Volume);

                                // commented on 15052017
                                //if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.CollectionMethodID != null)
                                //    cmbSpremCollecionMethod.SelectedValue = ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.CollectionMethodID;
                                //if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.ViscosityID != null)
                                //    cmbViscosity.SelectedValue = ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.ViscosityID;

                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.NoOfVails != null)
                                    txtNoofVials.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.NoOfVails);
                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.SpremExpiryDate != null)
                                    dtExpiryDate.SelectedDate = ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.SpremExpiryDate;
                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.SpremExpiryDate != null)
                                    txtExpiryTime.Value = ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.SpremExpiryDate;

                                #region  For Andrology Flow added on 15052017

                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Spillage != null)
                                    txtSpillage.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Spillage);                                                              

                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.DFI != null)
                                    txtDFI.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.DFI);

                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.ROS != null)
                                    txtROS.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.ROS);

                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.HIV != null)
                                    cmbHIV.SelectedValue = ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.HIV;

                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.HBSAG != null)
                                    cmbHBSAG.SelectedValue = ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.HBSAG;

                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.VDRL != null)
                                    cmbVDRL.SelectedValue = ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.VDRL;

                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.HCV != null)
                                    cmbHCV.SelectedValue = ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.HCV;

                                chkConsentCheck.IsChecked = ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.IsConsentCheck;
                                chkFreeze.IsChecked = ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.IsFreezed;

                                //if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Abstience != null)
                                //    txtAbstinence.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Abstience);

                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.AbstienceID != null)
                                    txtAbstinence.SelectedValue = ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.AbstienceID;

                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.PusCells != null)
                                    TxtPusCells.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.PusCells);

                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.RoundCells != null)
                                    TxtRoundCells.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.RoundCells);

                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.EpithelialCells != null)
                                    TxtEpithelialCells.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.EpithelialCells);

                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.OtherCells != null)
                                    TxtOtherCells.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.OtherCells);

                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.EmbryologistID != null)
                                    cmbEmbryologist.SelectedValue = ((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.CheckedByDoctorID;

                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.IsFreezed == true)
                                {
                                    CmdSave.IsEnabled = false;
                                    chkFreeze.IsEnabled = false;
                                }
                                else
                                {
                                    CmdSave.IsEnabled = true;
                                    chkFreeze.IsEnabled = true;
                                }
                                

                                #endregion

                                #region Region Freezing  Details

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

                                #endregion

                                //added by neena
                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Sperm5thPercentile != null)
                                    TxtSperm5thPercentile.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Sperm5thPercentile);
                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Sperm75thPercentile != null)
                                    TxtSperm75thPercentile.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Sperm75thPercentile);
                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Ejaculate5thPercentile != null)
                                    TxtEjaculate5thPercentile.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Ejaculate5thPercentile);
                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Ejaculate75thPercentile != null)
                                    TxtEjaculate75thPercentile.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.Ejaculate75thPercentile);
                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.TotalMotility5thPercentile != null)
                                    TxtTotalMotility5thPercentile.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.TotalMotility5thPercentile);
                                if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.TotalMotility75thPercentile != null)
                                    TxtTotalMotility75thPercentile.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.TotalMotility75thPercentile);
                                //

                                //if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.GradeA != null)
                                //    txtGradeA.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.GradeA);
                                //if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.GradeB != null)
                                //    txtGradeB.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.GradeB);
                                //if (((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.SpermCount != null)
                                //    TxtSpermCount.Text = Convert.ToString(((cls_NewGetSpremFreezingBizActionVO)arg.Result).SpremFreezingMainVO.SpermCount);

                            }
                            wait.Close();
                        }
                    };
                    client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }

            }
            catch (Exception)
            {
                wait.Close();
                throw;
            }
        }

        private void txtNoofVials_TextChanged(object sender, TextChangedEventArgs e)
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
            else
            {
                if (isnew == true)
                {
                    if (txtNoofVials.Text != string.Empty && txtNoofVials.Text != "0")
                    {
                        dgSpremFreezingDetailsGrid.ItemsSource = null;
                        SpremFreezingDetails = new ObservableCollection<clsNew_SpremFreezingVO>();
                        for (int i = 0; i < Convert.ToInt32(txtNoofVials.Text); i++)
                        {
                            SpremFreezingDetails.Add(new clsNew_SpremFreezingVO() { SpremNostr = "Auto Generated", GobletColorID = 0, ColorListVO = ColorList, StrawId = 0, StrawListVO = StrawList, GobletShapeId = 0, GlobletShapeListVO = GlobletShapeList, GobletSizeId = 0, GlobletSizeListVO = GlobletSizeList, CanID = 0, CanListVO = CanList, CanisterId = 0, CanisterListVO = CanisterList, TankId = 0, TankListVO = TankList, Comments = "", IsThaw = false });

                        }
                        dgSpremFreezingDetailsGrid.ItemsSource = SpremFreezingDetails;
                    }
                }
            }
        }

        private void txtNoofVials_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void cmbSpermType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TxtSampleCode.Text = "";


            if (((MasterListItem)cmbSpermType.SelectedItem).ID != 3 && ((MasterListItem)cmbSpermType.SelectedItem).ID != 6 && ((MasterListItem)cmbSpermType.SelectedItem).ID != 8 && ((MasterListItem)cmbSpermType.SelectedItem).ID != 10)
            {
                CmdSelectSample.IsEnabled = false;
            }
            else
            {
                CmdSelectSample.IsEnabled = true;
            }
        }

        private void CmdSelectSample_Click(object sender, RoutedEventArgs e)
        {
            if (((MasterListItem)cmbSpermType.SelectedItem).ID == 3 || (((MasterListItem)cmbSpermType.SelectedItem).ID == 6) || (((MasterListItem)cmbSpermType.SelectedItem).ID == 8))
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

                    //((TesaPesaTeseList)sender).TesaPesa.ID;
                }

                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("", "Please Select Sample!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                    msgW1.Show();
                }
            }
        }

        private void hlbViewLinking_Click(object sender, RoutedEventArgs e)
        {
            long ID, UnitID;
            if (dgfreezing.SelectedItem != null)
            {
                ID = ((cls_NewSpremFreezingMainVO)dgfreezing.SelectedItem).ID;
                UnitID = ((cls_NewSpremFreezingMainVO)dgfreezing.SelectedItem).UnitID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/IVFDashboard/IVFDashboard_SpermFreezing.aspx?ID=" + ID + "&UnitID=" + UnitID + "&PrintFomatID=" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID), "_blank");
            }
        }

        private void SpremFreezDate_LostFocus(object sender, RoutedEventArgs e)
        {
            DateTime? ExpiryDate = null;
            if (SpremFreezTime.Value != null)
            {
                ExpiryDate = SpremFreezDate.SelectedDate.Value.Date;
                ExpiryDate = ExpiryDate.Value.Add(SpremFreezTime.Value.Value.TimeOfDay);
                dtExpiryDate.SelectedDate = ExpiryDate.Value.AddYears(1);
                txtExpiryTime.Value = ExpiryDate.Value.AddYears(1);
            }
            else
            {
                dtExpiryDate.SelectedDate = SpremFreezDate.SelectedDate.Value.AddYears(1);
            }
        }

        //string textBeforeNo = null;
        //int selectionStartNo = 0;
        //double selectionStartDecmlNo = 0;
        //int selectionLengthNo = 0;

        //private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (!((TextBox)sender).Text.IsValidThreeDigit() && !(string.IsNullOrEmpty(textBeforeNo)))  //textBeforeNo != null
        //    {
        //        ((TextBox)sender).Text = textBeforeNo;
        //        ((TextBox)sender).SelectionStart = selectionStartNo;
        //        ((TextBox)sender).SelectionLength = selectionLengthNo;
        //        textBeforeNo = "";
        //        selectionStartNo = 0;
        //        selectionLengthNo = 0;
        //    }
        //}

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

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            long ID, UnitID;
            if (dgfreezing.SelectedItem != null)
            {
                ID = ((cls_NewSpremFreezingMainVO)dgfreezing.SelectedItem).ID;
                UnitID = ((cls_NewSpremFreezingMainVO)dgfreezing.SelectedItem).UnitID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/IVFDashboard/IVFDashboard_SpermFreezing.aspx?ID=" + ID + "&UnitID=" + UnitID + "&PrintFomatID=" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID), "_blank");
            }
        }

        private void txtDFI_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtDFI_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValidTwoDigitWithTwoDecimal() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtROS_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtROS_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValidThreeDigitWithTwoDecimal() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtvolume_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TxtSpermCount.Text.Trim() != "" && txtvolume.Text.Trim() != "")
            {
                double value = (Convert.ToDouble(TxtSpermCount.Text)) * (Convert.ToDouble(txtvolume.Text));
                txtTotalSpremCount.Text = value.ToString();
            }
        }

        //private void Total_TextChanged()
        //{

        //}

    }

}
