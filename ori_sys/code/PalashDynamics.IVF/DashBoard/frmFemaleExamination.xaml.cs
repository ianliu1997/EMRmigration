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
using PalashDynamics.Collections;
using CIMS;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects.EMR;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmFemaleExamination : ChildWindow
    {

        #region Paging

        public PagedSortableCollectionView<clsGeneralExaminationVO> DataList { get; private set; }


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

        #region Public Variables
        public bool IsPatientExist = false;
        public bool IsPageLoded = false;
        private SwivelAnimation objAnimation;
        public long SelectedRecord;
        public bool isModify = false;
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
        public bool IsCancel = true;
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        public string Action { get; set; }
        public clsUserVO loggedinUser { get; set; }
        public string ModuleName { get; set; }

        #endregion

        #region Save / Cancel Button
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        #endregion

        public frmFemaleExamination(long PID, long PUnitID)
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            //Paging
            DataList = new PagedSortableCollectionView<clsGeneralExaminationVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;

            this.DataContext = new clsGeneralExaminationVO();
            PatientID = PID;
            PatientUnitID = PUnitID;
        }

        //added by neena
        private void FIllvisit()
        {
            clsGetPatientAllVisitBizActionVO BizAction = new clsGetPatientAllVisitBizActionVO();
            BizAction.PatientID = PatientID;
            BizAction.UnitID = PatientUnitID;
            BizAction.IsPhysicalExamination = true;


            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient clientBizActionObjPatientHistoryData = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
            clientBizActionObjPatientHistoryData.ProcessCompleted += (s1, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    dgComplaintSummary.ItemsSource = ((clsGetPatientAllVisitBizActionVO)args.Result).VisitList;
                }

            };
            clientBizActionObjPatientHistoryData.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            clientBizActionObjPatientHistoryData.CloseAsync();
        }
        //

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FIllvisit();
            FillBuilt();
            //FillEyeColor();
            //FillHairColor();
            // FillSkinColor();
            if ((((IApplicationConfiguration)App.Current).SelectedPatient).PatientID != 0 || (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID != null)
            {
                //SetCommandButtonState("Load");
                this.DataContext = new clsGeneralExaminationVO();
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

        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonState(String strFormMode)
        {
            GeneralExamination.IsEnabled = true;
            PhysicalExamination.IsEnabled = true;
            Alterts.IsEnabled = true;

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
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = true;
                    CmdPrint.IsEnabled = true;
                    IsCancel = false;
                    GeneralExamination.IsEnabled = false;
                    PhysicalExamination.IsEnabled = false;
                    Alterts.IsEnabled = false;
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

        //private void FillEyeColor()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.IsActive = true;
        //    BizAction.MasterTable = MasterTableNameList.M_EyeColor;
        //    BizAction.MasterList = new List<MasterListItem>();

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            objList.Add(new MasterListItem(0, "- Select -", true));
        //            objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
        //            cmbEyeColor.ItemsSource = null;
        //            cmbEyeColor.ItemsSource = objList;
        //            cmbEyeColor.SelectedItem = objList[0];
        //        }
        //    };
        //    Client.ProcessAsync(BizAction, loggedinUser);
        //    Client.CloseAsync();
        //}
        //private void FillHairColor()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.IsActive = true;
        //    BizAction.MasterTable = MasterTableNameList.M_HairColor;
        //    BizAction.MasterList = new List<MasterListItem>();

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            objList.Add(new MasterListItem(0, "- Select -", true));
        //            objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
        //            cmbHairColor.ItemsSource = null;
        //            cmbHairColor.ItemsSource = objList;
        //            cmbHairColor.SelectedItem = objList[0];
        //        }
        //    };
        //    Client.ProcessAsync(BizAction, loggedinUser);
        //    Client.CloseAsync();
        //}
        //private void FillSkinColor()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.IsActive = true;
        //    BizAction.MasterTable = MasterTableNameList.M_SkinColor;
        //    BizAction.MasterList = new List<MasterListItem>();

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            objList.Add(new MasterListItem(0, "- Select -", true));
        //            objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
        //            cmbSkinColor.ItemsSource = null;
        //            cmbSkinColor.ItemsSource = objList;
        //            cmbSkinColor.SelectedItem = objList[0];
        //        }

        //    };
        //    Client.ProcessAsync(BizAction, loggedinUser);
        //    Client.CloseAsync();
        //}
        private void FillBuilt()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_IvfBuilt;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "- Select -", true));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbBuilt.ItemsSource = null;
                    cmbBuilt.ItemsSource = objList;
                    cmbBuilt.SelectedItem = objList[0];
                }
            };
            Client.ProcessAsync(BizAction, loggedinUser);
            Client.CloseAsync();
        }

        private bool Validation()
        {

            bool result = true;
            if (txtWeight.Text == "")
            {
                txtWeight.SetValidation("Please enter Weight");
                txtWeight.RaiseValidationError();
                txtWeight.Focus();
                result = false;
            }
            else if (txtWeight.Text == "0")
            {
                txtWeight.SetValidation("Please enter Weight");
                txtWeight.RaiseValidationError();
                txtWeight.Focus();
                result = false;
            }
            else
                txtWeight.ClearValidationError();

            if (txtHeight.Text == "")
            {
                txtHeight.SetValidation("Please enter Height");
                txtHeight.RaiseValidationError();
                txtHeight.Focus();
                result = false;
            }
            else if (txtHeight.Text == "0")
            {
                txtHeight.SetValidation("Please enter Height");
                txtHeight.RaiseValidationError();
                txtHeight.Focus();
                result = false;
            }
            else
                txtHeight.ClearValidationError();

            return result;
        }

        private void ClearFormData()
        {
            chkHIV.IsChecked = false;
        }

        private void FetchData()
        {
            clsGetGeneralExaminationFemaleBizActionVO BizAction = new clsGetGeneralExaminationFemaleBizActionVO();
            BizAction.List = new List<clsGeneralExaminationVO>();
            BizAction.PatientID = PatientID;
            BizAction.UnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {

                    if (((clsGetGeneralExaminationFemaleBizActionVO)args.Result).List != null)
                    {
                        clsGetGeneralExaminationFemaleBizActionVO result = args.Result as clsGetGeneralExaminationFemaleBizActionVO;

                        DataList.TotalItemCount = result.TotalRows;
                        if (result.List != null)
                        {
                            DataList.Clear();

                            foreach (var item in result.List)
                            {
                                DataList.Add(item);
                            }
                            dgExamination.ItemsSource = null;
                            dgExamination.ItemsSource = DataList;
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

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            this.SetCommandButtonState("New");
            ClearFormData();
            clsPatientGeneralVO FemalePatientDetails = new clsPatientGeneralVO();
            //  FemalePatientDetails = (clsPatientGeneralVO)Female.DataContext;
            clsGeneralExaminationVO obj = new clsGeneralExaminationVO();

            this.DataContext = obj;// new clsGeneralExaminationVO();
            txtBMI.Text = "";
            txtBPsystolic.Text = "";
            txtBPdystolic.Text = "";
            txtHeight.Text = "";
            txtPulse.Text = "";
            txtWeight.Text = "";
            // By  bhushan.. 
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

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to save the General examination?";
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
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
            clsAddGeneralExaminationFemaleBizActionVO BizAction = new clsAddGeneralExaminationFemaleBizActionVO();
            BizAction.Details = (clsGeneralExaminationVO)this.DataContext;

            BizAction.Details.PatientID = PatientID; // ByBHUSHAN(((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
            BizAction.Details.PatientUnitID = PatientUnitID;

            //By Anjali......

            if (AcenNo.IsChecked == true)
            {
                BizAction.Details.Acen1 = false;
            }
            else
            {
                BizAction.Details.Acen1 = true;
            }

            if (HirsutismNo.IsChecked == true)
            {
                BizAction.Details.Hirsutism1 = false;
            }
            else
            {
                BizAction.Details.Hirsutism1 = true;
            }
            BizAction.Details.BuiltID = ((MasterListItem)cmbBuilt.SelectedItem).ID;
            //  BizAction.Details.EyeColor1 = ((MasterListItem)cmbEyeColor.SelectedItem).ID;

            // BizAction.Details.HairColor1 = ((MasterListItem)cmbHairColor.SelectedItem).ID;
            //BizAction.Details.SkinColor1 = ((MasterListItem)cmbSkinColor.SelectedItem).ID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow(" ", " Female Examination Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    SetCommandButtonState("Save");
                    FetchData();
                    objAnimation.Invoke(RotationType.Backward);
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow(" ", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }


        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            if (IsCancel == false)
            {
                objAnimation.Invoke(RotationType.Backward);
                IsCancel = true;
            }
            else
                this.DialogResult = false;
        }

        private void CmdPrint_Click(object sender, RoutedEventArgs e)
        {
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/Patient/PatientHistory.aspx?Type=1&ID=" + SelectedRecord + "&PatientID=" + PatientID + "&PatientUnitID=" + PatientUnitID), "_blank");
        }


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

        private void hlbViewExamination_Click(object sender, RoutedEventArgs e)
        {
            if (dgExamination.SelectedItem != null)
            {
                SetCommandButtonState("Modify");
                this.DataContext = (clsGeneralExaminationVO)dgExamination.SelectedItem;
                SelectedRecord = ((clsGeneralExaminationVO)dgExamination.SelectedItem).ID;
                clsPatientGeneralVO FemalePatientDetails = new clsPatientGeneralVO();
                //  Female.DataContext = null;
                FemalePatientDetails = CoupleDetails.FemalePatient;
                FemalePatientDetails.Height = ((clsGeneralExaminationVO)dgExamination.SelectedItem).Height;
                FemalePatientDetails.Weight = ((clsGeneralExaminationVO)dgExamination.SelectedItem).Weight;

                //By Anjali
                cmbBuilt.SelectedValue = ((clsGeneralExaminationVO)dgExamination.SelectedItem).BuiltID;
                //cmbEyeColor.SelectedValue = ((clsGeneralExaminationVO)dgExamination.SelectedItem).EyeColor1;
                //cmbSkinColor.SelectedValue = ((clsGeneralExaminationVO)dgExamination.SelectedItem).SkinColor1;
                //cmbHairColor.SelectedValue = ((clsGeneralExaminationVO)dgExamination.SelectedItem).HairColor1;
                if (((clsGeneralExaminationVO)dgExamination.SelectedItem).Acen1 == true)
                {
                    AcenYes.IsChecked = true;
                }
                else
                {
                    AcenNo.IsChecked = true;
                }
                if (((clsGeneralExaminationVO)dgExamination.SelectedItem).Hirsutism1 == true)
                {
                    HirsutismYes.IsChecked = true;
                }
                else
                {
                    HirsutismNo.IsChecked = true;
                }
                if (((clsGeneralExaminationVO)dgExamination.SelectedItem).HIVPositive == true)
                {
                    chkHIV.IsChecked = true;
                }
                else
                {
                    chkHIV.IsChecked = true;
                }
                if (((clsGeneralExaminationVO)dgExamination.SelectedItem).HBVPositive == true)
                {
                    chkHBV.IsChecked = true;
                }
                else
                {
                    chkHBV.IsChecked = true;
                }
                if (((clsGeneralExaminationVO)dgExamination.SelectedItem).HCVPositive == true)
                {
                    chkHCV.IsChecked = true;
                }
                else
                {
                    chkHCV.IsChecked = true;
                }
                FemalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", ((clsGeneralExaminationVO)dgExamination.SelectedItem).BMI));
                //     Female.DataContext = FemalePatientDetails;
                CmdSave.IsEnabled = false;
                objAnimation.Invoke(RotationType.Forward);
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedRecord = ((clsGeneralExaminationVO)dgExamination.SelectedItem).ID;


            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/Patient/PatientHistory.aspx?Type=1&ID=" + SelectedRecord + "&PatientID=" + PatientID + "&PatientUnitID=" + PatientUnitID + "&PrintFomatID=" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID), "_blank");

        }

        private void txtWeight_TextChanged(object sender, TextChangedEventArgs e)
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

        private void txtWeight_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtHeight_TextChanged(object sender, TextChangedEventArgs e)
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

        private void txtHeight_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        public long PatientID;
        public long PatientUnitID;
        UIElement myData = null;

        private void txtWeight_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtWeight.Text) && !string.IsNullOrEmpty(txtHeight.Text))
            {
                double Height = Convert.ToDouble(txtHeight.Text.Trim());
                double Weight = Convert.ToDouble(txtWeight.Text.Trim());
                txtBMI.Text = Convert.ToString(CalculateBMI(Height, Weight));
            }
            else if (string.IsNullOrEmpty(txtWeight.Text))
                txtWeight.Focus();
            else if (string.IsNullOrEmpty(txtHeight.Text))
                txtHeight.Focus();
        }

        private void txtBPsystolic_TextChanged(object sender, TextChangedEventArgs e)
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

        private void txtBPdystolic_TextChanged(object sender, TextChangedEventArgs e)
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

        private void txtPulse_TextChanged(object sender, TextChangedEventArgs e)
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

        private void txtBPsystolic_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtBPdystolic_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtPulse_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;

        }
        long RoleID = ((clsUserRoleVO)((clsUserGeneralDetailVO)((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).UserGeneralDetailVO).RoleDetails).ID;
        long AdminRoleID = ((clsAppConfigVO)((IApplicationConfiguration)App.Current).ApplicationConfigurations).AdminRoleID;
        long NurseRoleID = ((clsAppConfigVO)((IApplicationConfiguration)App.Current).ApplicationConfigurations).NurseRoleID;
        long DoctorRoleID = ((clsAppConfigVO)((IApplicationConfiguration)App.Current).ApplicationConfigurations).DoctorRoleID;

        private void hlbViewExaminationNew_Click(object sender, RoutedEventArgs e)
        {
            IsCancel = false;
            frmIVFEMRPhysicalExam winPhysicalExam = new frmIVFEMRPhysicalExam();
            winPhysicalExam.CurrentVisit = ((clsVisitVO)dgComplaintSummary.SelectedItem);
            //winPhysicalExam.IsEnabledControl = this.CurrentVisit.VisitStatus;
            if (RoleID == NurseRoleID)
                winPhysicalExam.SelectedUser = "Nurse";

            if (RoleID == DoctorRoleID)
                winPhysicalExam.SelectedUser = "Doctor";

            if (RoleID == AdminRoleID)
                winPhysicalExam.SelectedUser = "Admin";
            winPhysicalExam.SelectedPatient = new Patient() { PatientId = PatientID, patientUnitID = PatientUnitID };
            EMR_RightCorner.Content = winPhysicalExam;
            objAnimation.Invoke(RotationType.Forward);
        }


    }
}

