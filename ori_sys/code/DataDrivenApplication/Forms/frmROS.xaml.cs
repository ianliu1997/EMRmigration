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
using PalashDynamics.ValueObjects.Patient;
using System.Windows.Controls.Primitives;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects.EMR;
using System.Xml.Linq;
using System.Reflection;
using System.Windows.Resources;
using System.IO;
using System.Windows.Media.Imaging;
using System.ServiceModel.Channels;
using System.Windows.Data;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.DashBoardVO;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.Collections;
using System.Globalization;

namespace DataDrivenApplication.Forms
{
    public partial class frmROS : UserControl, IInitiateCIMS
    {
        #region Property
        public string Action { get; set; }
        public string ModuleName { get; set; }
        bool IsCancel = true;
        public bool IsPatientExist = false;
        long VisitID = 0;
        long VisitUnitID = 0;
        public long PatientID = 0;

        clsIVFROSEstimationVO objSDDT = new clsIVFROSEstimationVO();
        public long ID = 0;
        public long UnitID = 0;
        private SwivelAnimation objAnimation;
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        #endregion
        #region Pagging
        public PagedSortableCollectionView<clsIVFROSEstimationVO> DataList { get; private set; }

        public int DataListPageSize
        {
            get { return DataList.PageSize; }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
            }
        }
        #endregion
        public frmROS()
        {
            InitializeComponent();
            NormalRange.Text = "Normal Range : <250RLU/sec/million sperm";
            //Initiate("NEW");
            DataList = new PagedSortableCollectionView<clsIVFROSEstimationVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            ROSEstimationGridPager.Source = DataList;
            ROSEstimationGridPager.PageSize = DataListPageSize;
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            SetCommandButtonState("Load");
            FillAndrologist();
            FillROSEstimationList();
        }


        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillROSEstimationList();
        }
        public void Initiate(string Mode)
        {
            switch (Mode.ToUpper())
            {
                case "NEW":

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
                    if (((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).Gender == "MALE")
                    {
                        IsPatientExist = true;
                        PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                        mElement.Text = " : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                            " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                            ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;
                        mElement.Text += " - " + ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;

                        txtFirstName.Text = Convert.ToString(((IApplicationConfiguration)App.Current).SelectedPatient.FirstName);
                        txtMiddleName.Text = Convert.ToString(((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName);
                        txtLastName.Text = Convert.ToString(((IApplicationConfiguration)App.Current).SelectedPatient.LastName);
                        txtMRNo.Text = Convert.ToString(((IApplicationConfiguration)App.Current).SelectedPatient.MRNo);
                        txtDateOfBirth.Text = Convert.ToDateTime(((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        txtWeight.Text = Convert.ToString(((IApplicationConfiguration)App.Current).SelectedPatient.Weight);
                        txtHeight.Text = Convert.ToString(((IApplicationConfiguration)App.Current).SelectedPatient.Height);
                        txtMBMI.Text = Convert.ToString(((IApplicationConfiguration)App.Current).SelectedPatient.BMI);
                        txtAlerts.Text = Convert.ToString(((IApplicationConfiguration)App.Current).SelectedPatient.Alerts);
                        txtAge.Text = CalculateYourAge(Convert.ToDateTime(((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth));
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

                case "VISIT":

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
                    PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    UserControl rootPage2 = Application.Current.RootVisual as UserControl;
                    TextBlock mElement3 = (TextBlock)rootPage2.FindName("SampleSubHeader");
                    mElement3.Text = " : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                        " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                        ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;
                    mElement3.Text += " - " + ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                    break;

                default:
                    break;
            }
        }
        static string CalculateYourAge(DateTime Dob)
        {
            DateTime Now = DateTime.Now;
            int Years = new DateTime(DateTime.Now.Subtract(Dob).Ticks).Year - 1;
            DateTime PastYearDate = Dob.AddYears(Years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (PastYearDate.AddMonths(i) == Now)
                {
                    Months = i;
                    break;
                }
                else if (PastYearDate.AddMonths(i) >= Now)
                {
                    Months = i - 1;
                    break;
                }
            }
            int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;
            int Hours = Now.Subtract(PastYearDate).Hours;
            int Minutes = Now.Subtract(PastYearDate).Minutes;
            int Seconds = Now.Subtract(PastYearDate).Seconds;
            return Years + "" + " Years " + Months + " Months";
        }
        public void FillROSEstimationList()
        {
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();
            clsGetIVFROSEstimationBizActionVO objBizActionVO = new clsGetIVFROSEstimationBizActionVO();
            objBizActionVO.clsIVFROSEstimation = new clsIVFROSEstimationVO();
            objBizActionVO.clsIVFROSEstimation.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            objBizActionVO.clsIVFROSEstimation.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            objBizActionVO.clsIVFROSEstimation.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                Indicatior.Show();
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        clsGetIVFROSEstimationBizActionVO result = arg.Result as clsGetIVFROSEstimationBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;
                        if (result.ObjROSEstimationList != null)
                        {
                            DataList = new PagedSortableCollectionView<clsIVFROSEstimationVO>();
                            foreach (var item in result.ObjROSEstimationList)
                            {
                                DataList.Add(item);
                            }
                            dgROSList.ItemsSource = null;
                            dgROSList.ItemsSource = DataList;
                            dgROSList.CanUserSortColumns = true;
                            dgROSList.SelectedItem = null;
                            ROSEstimationGridPager.Source = null;
                            ROSEstimationGridPager.Source = DataList;
                            ROSEstimationGridPager.PageSize = objBizActionVO.MaximumRows;
                        }
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "ERROR Occured While Processing... ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
                objAnimation.Invoke(RotationType.Backward);
                Indicatior.Close();
            };
            client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void dgThawingDetilsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dgThawingDetilsGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmbPlanForSperms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbLabIncharge_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsCancel == true)
                {
                    ModuleName = "OPDModule";
                    Action = "CIMS.Forms.QueueManagement";

                    UserControl rootPage = Application.Current.RootVisual as UserControl;

                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                    mElement.Text = "Andrology Queue List";

                    WebClient c2 = new WebClient();
                    c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                    c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                    //SampleHeader.Text = "Andrology Queue List";
                }
                else
                {

                    ClearData();
                    IsCancel = true;
                    objAnimation.Invoke(RotationType.Backward);
                }
                SetCommandButtonState("Cancel");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient.IsClinicVisited)
            {
                ClearData();
                SetCommandButtonState("New");
                FillAndrologist();
                try
                {
                    objAnimation.Invoke(RotationType.Forward);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Current Visit is Closed", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                WaitIndicator Indicatior = new WaitIndicator();
                Indicatior.Show();
                clsAddUpdateIVFROSEstimationBizActionVO objBizAction = new clsAddUpdateIVFROSEstimationBizActionVO();
                objBizAction.clsIVFROSEstimation = new clsIVFROSEstimationVO();
                try
                {
                    if (ID > 0)
                    {
                        objBizAction.clsIVFROSEstimation.ID = ID;
                    }
                    objBizAction.clsIVFROSEstimation.Date = DateTime.Now;
                    objBizAction.clsIVFROSEstimation.VisitID = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;
                    objBizAction.clsIVFROSEstimation.VisitUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitUnitId;
                    objBizAction.clsIVFROSEstimation.ROSLevel = Convert.ToDouble(txtROSLevel.Text);
                    objBizAction.clsIVFROSEstimation.TotalAntioxidantCapacity = Convert.ToDouble(txtTAC.Text);
                    objBizAction.clsIVFROSEstimation.ROSTACScore = Convert.ToDouble(txtROSTAC.Text);
                    if ((MasterListItem)cmbAndrologist.SelectedItem != null)
                    {
                        objBizAction.clsIVFROSEstimation.AndrologistID = ((MasterListItem)cmbAndrologist.SelectedItem).ID;
                    }
                    objBizAction.clsIVFROSEstimation.Remarks = txtRemark.Text.ToString().Trim();
                    objBizAction.clsIVFROSEstimation.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    objBizAction.clsIVFROSEstimation.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    objBizAction.clsIVFROSEstimation.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    objBizAction.clsIVFROSEstimation.AddedDateTime = System.DateTime.Now;
                    objBizAction.clsIVFROSEstimation.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    objBizAction.clsIVFROSEstimation.IsFreezed = Convert.ToBoolean(chkFreezBill.IsChecked);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        Indicatior.Show();
                        if (arg.Error == null)
                        {
                            if (arg.Result != null)
                            {
                                clsAddUpdateIVFROSEstimationBizActionVO result1 = arg.Result as clsAddUpdateIVFROSEstimationBizActionVO;
                                DataList.TotalItemCount = result1.TotalRows;
                                if (result1.clsIVFROSEstimation != null)
                                {

                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                    PrintReport(result1.clsIVFROSEstimation.ID, result1.clsIVFROSEstimation.UnitId);
                                }

                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "ERROR Occured While Processing... ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                        SetCommandButtonState("Modify");
                        objAnimation.Invoke(RotationType.Backward);
                        FillROSEstimationList();
                        Indicatior.Close();
                    };
                    client.ProcessAsync(objBizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void hlbView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgROSList.SelectedItem != null)
                {
                    SetCommandButtonState("View");
                    objSDDT = new clsIVFROSEstimationVO();
                    objSDDT = (clsIVFROSEstimationVO)dgROSList.SelectedItem;
                    ID = ((clsIVFROSEstimationVO)dgROSList.SelectedItem).ID;
                    UnitID = ((clsIVFROSEstimationVO)dgROSList.SelectedItem).UnitId;
                    dtpDate.Text = Convert.ToDateTime(((clsIVFROSEstimationVO)dgROSList.SelectedItem).Date).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture); ;
                    cmbAndrologist.SelectedValue = ((clsIVFROSEstimationVO)dgROSList.SelectedItem).AndrologistID;
                    cmbAndrologist.UpdateLayout();
                    txtROSLevel.Text = Convert.ToString(((clsIVFROSEstimationVO)dgROSList.SelectedItem).ROSLevel);
                    txtTAC.Text = Convert.ToString(((clsIVFROSEstimationVO)dgROSList.SelectedItem).TotalAntioxidantCapacity);
                    txtROSTAC.Text = Convert.ToString(((clsIVFROSEstimationVO)dgROSList.SelectedItem).ROSTACScore);
                    txtRemark.Text = Convert.ToString(((clsIVFROSEstimationVO)dgROSList.SelectedItem).Remarks);
                    chkFreezBill.IsChecked = ((clsIVFROSEstimationVO)dgROSList.SelectedItem).IsFreezed;
                    if (((clsIVFROSEstimationVO)dgROSList.SelectedItem).IsFreezed == true)
                    {
                        cmdModify.IsEnabled = false;
                        chkFreezBill.IsEnabled = false;
                    }
                    else
                    {
                        cmdModify.IsEnabled = true;
                        chkFreezBill.IsEnabled = true;
                    }
                    objAnimation.Invoke(RotationType.Forward);
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
        }

        private void cmdSave_Click_1(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Save ?";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);
                msgWin.Show();

            }
        }
        #region Set Command Button State New/Save/Modify/Cancel
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdAdd.IsEnabled = true;
                    IsCancel = true;
                    break;

                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdAdd.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    chkFreezBill.IsEnabled = true;
                    chkFreezBill.IsChecked = false;
                    break;

                case "Save":
                    cmdAdd.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;

                case "Modify":
                    cmdAdd.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Cancel":
                    cmdAdd.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;

                case "FrontPanel":

                    cmdAdd.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    IsCancel = true;

                    break;
                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdAdd.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region Validation
        public bool Validation()
        {
            bool result = true;
            if (String.IsNullOrEmpty(txtROSLevel.Text))
            {
                result = false;
                txtROSLevel.SetValidation("ROS Level Is Required");
                txtROSLevel.RaiseValidationError();
                return result;
            }
            else
            {
                txtROSLevel.ClearValidationError();
                result = true;
            }
            if (String.IsNullOrEmpty(txtTAC.Text))
            {
                result = false;
                txtTAC.SetValidation("Total Antioxidant capacity Is Required");
                txtTAC.RaiseValidationError();
                return result;
            }
            else
            {
                txtTAC.ClearValidationError();
                result = true;
            }
            if (String.IsNullOrEmpty(txtROSTAC.Text))
            {
                result = false;
                txtROSTAC.SetValidation("ROS-TAC Score Is Required");
                txtROSTAC.RaiseValidationError();
                return result;
            }
            else
            {
                txtROSTAC.ClearValidationError();
                result = true;
            }
            if ((MasterListItem)cmbAndrologist.SelectedItem == null)
            {
                result = false;
                cmbAndrologist.TextBox.SetValidation("Andrologist Is Required");
                cmbAndrologist.TextBox.RaiseValidationError();
                cmbAndrologist.Focus();
                return result;
            }
            else if (((MasterListItem)cmbAndrologist.SelectedItem).ID == 0)
            {
                result = false;
                cmbAndrologist.TextBox.SetValidation("Andrologist Is Required");
                cmbAndrologist.TextBox.RaiseValidationError();
                cmbAndrologist.Focus();
                return result;
            }
            else
            {
                cmbAndrologist.TextBox.ClearValidationError();
                result = true;
            }
            return result;
        }

        #endregion

        void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                WaitIndicator Indicatior = new WaitIndicator();
                Indicatior.Show();
                clsAddUpdateIVFROSEstimationBizActionVO objBizAction = new clsAddUpdateIVFROSEstimationBizActionVO();
                objBizAction.clsIVFROSEstimation = new clsIVFROSEstimationVO();
                try
                {
                    objBizAction.clsIVFROSEstimation.ID = 0;
                    objBizAction.clsIVFROSEstimation.Date = DateTime.Now;
                    objBizAction.clsIVFROSEstimation.VisitID = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;
                    objBizAction.clsIVFROSEstimation.VisitUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitUnitId;
                    objBizAction.clsIVFROSEstimation.ROSLevel = Convert.ToDouble(txtROSLevel.Text);
                    objBizAction.clsIVFROSEstimation.TotalAntioxidantCapacity = Convert.ToDouble(txtTAC.Text);
                    objBizAction.clsIVFROSEstimation.ROSTACScore = Convert.ToDouble(txtROSTAC.Text);
                    if ((MasterListItem)cmbAndrologist.SelectedItem != null)
                    {
                        objBizAction.clsIVFROSEstimation.AndrologistID = ((MasterListItem)cmbAndrologist.SelectedItem).ID;
                    }
                    objBizAction.clsIVFROSEstimation.Remarks = txtRemark.Text.ToString().Trim();
                    objBizAction.clsIVFROSEstimation.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    objBizAction.clsIVFROSEstimation.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    objBizAction.clsIVFROSEstimation.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    objBizAction.clsIVFROSEstimation.AddedDateTime = System.DateTime.Now;
                    objBizAction.clsIVFROSEstimation.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    objBizAction.clsIVFROSEstimation.IsFreezed = Convert.ToBoolean(chkFreezBill.IsChecked);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        Indicatior.Show();
                        if (arg.Error == null)
                        {
                            if (arg.Result != null)
                            {
                                clsAddUpdateIVFROSEstimationBizActionVO result1 = arg.Result as clsAddUpdateIVFROSEstimationBizActionVO;
                                DataList.TotalItemCount = result1.TotalRows;
                                if (result1.clsIVFROSEstimation != null)
                                {

                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                    PrintReport(result1.clsIVFROSEstimation.ID, result1.clsIVFROSEstimation.UnitId);
                                }

                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "ERROR Occured While Processing... ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                        SetCommandButtonState("Save");
                        objAnimation.Invoke(RotationType.Backward);
                        FillROSEstimationList();
                        Indicatior.Close();
                    };
                    client.ProcessAsync(objBizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                catch (Exception ex)
                {

                }
            }
        }

        public void ClearData()
        {
            txtROSLevel.Text = String.Empty;
            txtTAC.Text = String.Empty;
            txtROSTAC.Text = String.Empty;
            txtRemark.Text = String.Empty;
            dtpDate.Text = DateTime.Now.ToString();

        }

        public void FillAndrologist()
        {
            clsGetDoctorListForComboBizActionVO BizAction = new clsGetDoctorListForComboBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetDoctorListForComboBizActionVO)arg.Result).MasterList.Where(p => p.SpecializationID == 5).ToList()); //HardCode By Bhushan For AndrologyDoctor

                    cmbAndrologist.ItemsSource = null;
                    cmbAndrologist.ItemsSource = objList;
                    cmbAndrologist.SelectedItem = objList[0];
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
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

        private void txtROSLevel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValidFourDigitWithTwoDecimal() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
            SolidColorBrush Redbrush = new SolidColorBrush(Colors.Red);
            SolidColorBrush Blackbrush = new SolidColorBrush(Colors.Black);

            if (!String.IsNullOrEmpty(txtROSLevel.Text))
            {
                double value = Convert.ToDouble(txtROSLevel.Text);
                if (value < 250)
                {
                    txtROSLevel.Foreground = Blackbrush;
                    txtROSLevel.FontWeight = FontWeights.Normal;
                }
                else
                {
                    txtROSLevel.Foreground = Redbrush;
                    txtROSLevel.FontWeight = FontWeights.Bold;
                }
            }
        }

        private void txtROSLevel_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtROSLevel_LostFocus(object sender, RoutedEventArgs e)
        {
            SolidColorBrush Redbrush = new SolidColorBrush(Colors.Red);
            SolidColorBrush Blackbrush = new SolidColorBrush(Colors.Black);

            if (!String.IsNullOrEmpty(txtROSLevel.Text))
            {
                double value = Convert.ToDouble(txtROSLevel.Text);
                if (value < 250)
                {
                    txtROSLevel.Foreground = Blackbrush;
                    txtROSLevel.FontWeight = FontWeights.Normal;
                }
                else
                {
                    txtROSLevel.Foreground = Redbrush;
                    txtROSLevel.FontWeight = FontWeights.Bold;
                }
            }
        }

        private void txtTAC_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValidFourDigitWithTwoDecimal() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
            SolidColorBrush Redbrush = new SolidColorBrush(Colors.Red);
            SolidColorBrush Blackbrush = new SolidColorBrush(Colors.Black);

            if (!String.IsNullOrEmpty(txtTAC.Text))
            {
                double value = Convert.ToDouble(txtTAC.Text);
                //if (value > 25 && value < 40)
                //{
                //    txtTAC.Foreground = Blackbrush;
                //    txtTAC.FontWeight = FontWeights.Normal;
                //}
                //else
                //{
                //    txtTAC.Foreground = Redbrush;
                //    txtTAC.FontWeight = FontWeights.Bold;
                //}
            }
        }

        private void txtTAC_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtTAC_LostFocus(object sender, RoutedEventArgs e)
        {
            SolidColorBrush Redbrush = new SolidColorBrush(Colors.Red);
            SolidColorBrush Blackbrush = new SolidColorBrush(Colors.Black);

            if (!String.IsNullOrEmpty(txtTAC.Text))
            {
                double value = Convert.ToDouble(txtTAC.Text);
                //if (value > 25 && value < 40)
                //{
                //    txtTAC.Foreground = Blackbrush;
                //    txtTAC.FontWeight = FontWeights.Normal;
                //}
                //else
                //{
                //    txtTAC.Foreground = Redbrush;
                //    txtTAC.FontWeight = FontWeights.Bold;
                //}
            }
        }

        private void txtROSTAC_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValidFourDigitWithTwoDecimal() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
            SolidColorBrush Redbrush = new SolidColorBrush(Colors.Red);
            SolidColorBrush Blackbrush = new SolidColorBrush(Colors.Black);

            if (!String.IsNullOrEmpty(txtROSTAC.Text))
            {
                double value = Convert.ToDouble(txtROSTAC.Text);
                //if (value > 25 && value < 40)
                //{
                //    txtROSTAC.Foreground = Blackbrush;
                //    txtROSTAC.FontWeight = FontWeights.Normal;
                //}
                //else
                //{
                //    txtROSTAC.Foreground = Redbrush;
                //    txtROSTAC.FontWeight = FontWeights.Bold;
                //}
            }
        }

        private void txtROSTAC_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtROSTAC_LostFocus(object sender, RoutedEventArgs e)
        {
            SolidColorBrush Redbrush = new SolidColorBrush(Colors.Red);
            SolidColorBrush Blackbrush = new SolidColorBrush(Colors.Black);

            if (!String.IsNullOrEmpty(txtROSTAC.Text))
            {
                double value = Convert.ToDouble(txtROSTAC.Text);
                //if (value > 25 && value < 40)
                //{
                //    txtROSTAC.Foreground = Blackbrush;
                //    txtROSTAC.FontWeight = FontWeights.Normal;
                //}
                //else
                //{
                //    txtROSTAC.Foreground = Redbrush;
                //    txtROSTAC.FontWeight = FontWeights.Bold;
                //}
            }
        }

        private void PrintReport(long pID, long pUnitID)
        {
            if (pID > 0)
            {
                string URL = "../Reports/IVFDashboard/IVFDashboard_ROSEstimation.aspx?ID=" + pID + "&UnitID=" + pUnitID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgROSList.SelectedItem != null)
                {
                    ID = ((clsIVFROSEstimationVO)dgROSList.SelectedItem).ID;
                    UnitID = ((clsIVFROSEstimationVO)dgROSList.SelectedItem).UnitId;
                    PrintReport(ID, UnitID);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
