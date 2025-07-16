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
    public partial class frmSDDT : UserControl, IInitiateCIMS
    {
        #region Property
        private SwivelAnimation objAnimation;
        public string Action { get; set; }
        public string ModuleName { get; set; }
        bool IsCancel = true;
        public bool IsPatientExist = false;
        long VisitID = 0;
        long VisitUnitID = 0;
        public long PatientID = 0;
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
        clsIVFSpermDefectTestVO objSDDT = new clsIVFSpermDefectTestVO();
        public long ID = 0;
        public long UnitID = 0;
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        Color myRgbColor;
        Color myRgbColor1;
        Color myRgbColor2;
        Color myRgbColor3;
        #endregion
        #region Pagging
        public PagedSortableCollectionView<clsIVFSpermDefectTestVO> DataList { get; private set; }

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
        public frmSDDT()
        {
            InitializeComponent();
            //Initiate("NEW");
            DataList = new PagedSortableCollectionView<clsIVFSpermDefectTestVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            SDDTListGridPager.Source = DataList;
            SDDTListGridPager.PageSize = DataListPageSize;
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            SetCommandButtonState("Load");
            FillAndrologist();
            FillSperDefectTestList();

            myRgbColor = new Color();
            myRgbColor.A = Convert.ToByte(120);
            myRgbColor.R = Convert.ToByte(250);
            myRgbColor.G = Convert.ToByte(0);
            myRgbColor.B = Convert.ToByte(0);
            
            myRgbColor1 = new Color();
            myRgbColor1.A = Convert.ToByte(120);
            myRgbColor1.R = Convert.ToByte(240);
            myRgbColor1.G = Convert.ToByte(230);
            myRgbColor1.B = Convert.ToByte(140);

            myRgbColor2 = new Color();
            myRgbColor2.A = Convert.ToByte(120);
            myRgbColor2.R = Convert.ToByte(32);
            myRgbColor2.G = Convert.ToByte(178);
            myRgbColor2.B = Convert.ToByte(170);


            myRgbColor3 = new Color();
            myRgbColor3.A = Convert.ToByte(120);
            myRgbColor3.R = Convert.ToByte(65);
            myRgbColor3.G = Convert.ToByte(105);
            myRgbColor3.B = Convert.ToByte(225);
        }


        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillSperDefectTestList();
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
                    if (((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).Gender == "MALE" || ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).Gender == "Male")
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
        public void FillSperDefectTestList()
        {
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();
            clsGetIVFSpermDefectTestBizActionVO objBizActionVO = new clsGetIVFSpermDefectTestBizActionVO();
            objBizActionVO.ClsIVFSpermDefectTest = new clsIVFSpermDefectTestVO();
            objBizActionVO.ClsIVFSpermDefectTest.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            objBizActionVO.ClsIVFSpermDefectTest.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            objBizActionVO.ClsIVFSpermDefectTest.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                Indicatior.Show();
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        clsGetIVFSpermDefectTestBizActionVO result = arg.Result as clsGetIVFSpermDefectTestBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;
                        if (result.ObjSpermDefectTestList != null)
                        {
                            DataList = new PagedSortableCollectionView<clsIVFSpermDefectTestVO>();
                            foreach (var item in result.ObjSpermDefectTestList)
                            {
                                DataList.Add(item);
                            }
                            dgSDDTList.ItemsSource = null;
                            dgSDDTList.ItemsSource = DataList;
                            dgSDDTList.CanUserSortColumns = true;
                            dgSDDTList.SelectedItem = null;
                            SDDTListGridPager.Source = null;
                            SDDTListGridPager.Source = DataList;
                            SDDTListGridPager.PageSize = objBizActionVO.MaximumRows;
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
                    SetCommandButtonState("Cancel");
                }

                objAnimation.Invoke(RotationType.Backward);
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
                clsAddUpdateIVFSpermDefectTestBizActionVO objBizAction = new clsAddUpdateIVFSpermDefectTestBizActionVO();
                objBizAction.ClsIVFSpermDefectTest = new clsIVFSpermDefectTestVO();
                try
                {
                    if (ID > 0)
                    {
                        objBizAction.ClsIVFSpermDefectTest.ID = ID;
                    }
                    objBizAction.ClsIVFSpermDefectTest.Date = DateTime.Now;
                    objBizAction.ClsIVFSpermDefectTest.VisitID = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;
                    objBizAction.ClsIVFSpermDefectTest.VisitUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitUnitId;
                    objBizAction.ClsIVFSpermDefectTest.AnnexinBinding = Convert.ToDouble(txtAnnexinBinding.Text);
                    objBizAction.ClsIVFSpermDefectTest.CaspaseActivity = Convert.ToDouble(txtCaspaseActivity.Text);
                    objBizAction.ClsIVFSpermDefectTest.AcrosinActivity = Convert.ToDouble(txtAcrosinActivity.Text);
                    objBizAction.ClsIVFSpermDefectTest.GlucosidaseActivity = Convert.ToDouble(txtGlucosidaseActivity.Text);
                    if ((MasterListItem)cmbAndrologist.SelectedItem != null)
                    {
                        objBizAction.ClsIVFSpermDefectTest.AndrologistID = ((MasterListItem)cmbAndrologist.SelectedItem).ID;
                    }
                    objBizAction.ClsIVFSpermDefectTest.Remarks = txtRemark.Text.ToString().Trim();
                    objBizAction.ClsIVFSpermDefectTest.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    objBizAction.ClsIVFSpermDefectTest.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    objBizAction.ClsIVFSpermDefectTest.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    objBizAction.ClsIVFSpermDefectTest.AddedDateTime = System.DateTime.Now;
                    objBizAction.ClsIVFSpermDefectTest.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    objBizAction.ClsIVFSpermDefectTest.IsFreezed = Convert.ToBoolean(chkFreezBill.IsChecked);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        Indicatior.Show();
                        if (arg.Error == null)
                        {
                            if (arg.Result != null)
                            {
                                clsAddUpdateIVFSpermDefectTestBizActionVO result1 = arg.Result as clsAddUpdateIVFSpermDefectTestBizActionVO;
                                DataList.TotalItemCount = result1.TotalRows;
                                if (result1.ClsIVFSpermDefectTest != null)
                                {

                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                    PrintReport(result1.ClsIVFSpermDefectTest.ID, result1.ClsIVFSpermDefectTest.UnitId);
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
                        SetCommandButtonState("Modify");
                        FillSperDefectTestList();
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
                if (dgSDDTList.SelectedItem != null)
                {
                    SolidColorBrush Blackbrush = new SolidColorBrush(Colors.Black);
                    txtAcrosinActivity.Foreground = Blackbrush;
                    txtAnnexinBinding.Foreground = Blackbrush;
                    txtCaspaseActivity.Foreground = Blackbrush;
                    txtGlucosidaseActivity.Foreground = Blackbrush;
                    SetCommandButtonState("View");
                    objSDDT = new clsIVFSpermDefectTestVO();
                    objSDDT = (clsIVFSpermDefectTestVO)dgSDDTList.SelectedItem;
                    ID = ((clsIVFSpermDefectTestVO)dgSDDTList.SelectedItem).ID;
                    UnitID = ((clsIVFSpermDefectTestVO)dgSDDTList.SelectedItem).UnitId;
                    dtpDate.Text = Convert.ToDateTime(((clsIVFSpermDefectTestVO)dgSDDTList.SelectedItem).Date).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture); ;                   
                    cmbAndrologist.SelectedValue = ((clsIVFSpermDefectTestVO)dgSDDTList.SelectedItem).AndrologistID;
                    cmbAndrologist.UpdateLayout();
                    txtAcrosinActivity.Text = Convert.ToString(((clsIVFSpermDefectTestVO)dgSDDTList.SelectedItem).AcrosinActivity);
                    txtAnnexinBinding.Text = Convert.ToString(((clsIVFSpermDefectTestVO)dgSDDTList.SelectedItem).AnnexinBinding);
                    txtCaspaseActivity.Text = Convert.ToString(((clsIVFSpermDefectTestVO)dgSDDTList.SelectedItem).CaspaseActivity);
                    txtGlucosidaseActivity.Text = Convert.ToString(((clsIVFSpermDefectTestVO)dgSDDTList.SelectedItem).GlucosidaseActivity);
                    txtRemark.Text = Convert.ToString(((clsIVFSpermDefectTestVO)dgSDDTList.SelectedItem).Remarks);
                    chkFreezBill.IsChecked = ((clsIVFSpermDefectTestVO)dgSDDTList.SelectedItem).IsFreezed;
                    if (((clsIVFSpermDefectTestVO)dgSDDTList.SelectedItem).IsFreezed == true)
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
            if (String.IsNullOrEmpty(txtAnnexinBinding.Text))
            {
                result = false;
                txtAnnexinBinding.SetValidation("Annexin Binding Is Required");
                txtAnnexinBinding.RaiseValidationError();
                return result;
            }
            else
            {
                txtAnnexinBinding.ClearValidationError();
                result = true;
            }
            if (String.IsNullOrEmpty(txtCaspaseActivity.Text))
            {
                result = false;
                txtCaspaseActivity.SetValidation("Caspase Activity Is Required");
                txtCaspaseActivity.RaiseValidationError();
                return result;
            }
            else
            {
                txtCaspaseActivity.ClearValidationError();
                result = true;
            }
            if (String.IsNullOrEmpty(txtAcrosinActivity.Text))
            {
                result = false;
                txtAcrosinActivity.SetValidation("Acrosin Activity Is Required");
                txtAcrosinActivity.RaiseValidationError();
                return result;
            }
            else
            {
                txtAcrosinActivity.ClearValidationError();
                result = true;
            }
            if (String.IsNullOrEmpty(txtGlucosidaseActivity.Text))
            {
                result = false;
                txtGlucosidaseActivity.SetValidation("Glucosidase Activity Is Required");
                txtGlucosidaseActivity.RaiseValidationError();
                return result;
            }
            else
            {
                txtGlucosidaseActivity.ClearValidationError();
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
                clsAddUpdateIVFSpermDefectTestBizActionVO objBizAction = new clsAddUpdateIVFSpermDefectTestBizActionVO();
                objBizAction.ClsIVFSpermDefectTest = new clsIVFSpermDefectTestVO();
                try
                {
                    objBizAction.ClsIVFSpermDefectTest.ID = 0;
                    objBizAction.ClsIVFSpermDefectTest.Date = DateTime.Now;
                    objBizAction.ClsIVFSpermDefectTest.VisitID = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;
                    objBizAction.ClsIVFSpermDefectTest.VisitUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitUnitId;
                    objBizAction.ClsIVFSpermDefectTest.AnnexinBinding = Convert.ToDouble(txtAnnexinBinding.Text);
                    objBizAction.ClsIVFSpermDefectTest.CaspaseActivity = Convert.ToDouble(txtCaspaseActivity.Text);
                    objBizAction.ClsIVFSpermDefectTest.AcrosinActivity = Convert.ToDouble(txtAcrosinActivity.Text);
                    objBizAction.ClsIVFSpermDefectTest.GlucosidaseActivity = Convert.ToDouble(txtGlucosidaseActivity.Text);
                    if ((MasterListItem)cmbAndrologist.SelectedItem != null)
                    {
                        objBizAction.ClsIVFSpermDefectTest.AndrologistID = ((MasterListItem)cmbAndrologist.SelectedItem).ID;
                    }
                    objBizAction.ClsIVFSpermDefectTest.Remarks = txtRemark.Text.ToString().Trim();
                    objBizAction.ClsIVFSpermDefectTest.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    objBizAction.ClsIVFSpermDefectTest.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    objBizAction.ClsIVFSpermDefectTest.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    objBizAction.ClsIVFSpermDefectTest.AddedDateTime = System.DateTime.Now;
                    objBizAction.ClsIVFSpermDefectTest.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    objBizAction.ClsIVFSpermDefectTest.IsFreezed = Convert.ToBoolean(chkFreezBill.IsChecked);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        Indicatior.Show();
                        if (arg.Error == null)
                        {
                            if (arg.Result != null)
                            {
                                clsAddUpdateIVFSpermDefectTestBizActionVO result1 = arg.Result as clsAddUpdateIVFSpermDefectTestBizActionVO;
                                DataList.TotalItemCount = result1.TotalRows;
                                if (result1.ClsIVFSpermDefectTest != null)
                                {

                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                    PrintReport(result1.ClsIVFSpermDefectTest.ID, result1.ClsIVFSpermDefectTest.UnitId);
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
                        FillSperDefectTestList();
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
            txtAcrosinActivity.Text = String.Empty;
            txtAnnexinBinding.Text = String.Empty;
            txtCaspaseActivity.Text = String.Empty;
            txtGlucosidaseActivity.Text = String.Empty;
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

        private void txtAnnexinBinding_TextChanged(object sender, TextChangedEventArgs e)
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
            SolidColorBrush Redbrush = new SolidColorBrush(Colors.Red);
            SolidColorBrush Blackbrush = new SolidColorBrush(Colors.Black);

            if (!String.IsNullOrEmpty(txtAnnexinBinding.Text))
            {
                double value = Convert.ToDouble(txtAnnexinBinding.Text);
                if (value >= 25 && value <= 40)
                {
                    txtAnnexinBinding.Foreground = Blackbrush;
                    txtAnnexinBinding.FontWeight = FontWeights.Normal;
                }
                else
                {
                    txtAnnexinBinding.Foreground = Redbrush;
                    txtAnnexinBinding.FontWeight = FontWeights.Bold;
                }
            }
        }

        private void txtAnnexinBinding_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtAnnexinBinding_LostFocus(object sender, RoutedEventArgs e)
        {
            SolidColorBrush Redbrush = new SolidColorBrush(Colors.Red);
            SolidColorBrush Blackbrush = new SolidColorBrush(Colors.Black);

            if (!String.IsNullOrEmpty(txtAnnexinBinding.Text))
            {
                double value = Convert.ToDouble(txtAnnexinBinding.Text);
                if (value >= 25 && value <= 40)
                {
                    txtAnnexinBinding.Foreground = Blackbrush;
                    txtAnnexinBinding.FontWeight = FontWeights.Normal;
                }
                else
                {
                    txtAnnexinBinding.Foreground = Redbrush;
                    txtAnnexinBinding.FontWeight = FontWeights.Bold;
                }
            }
        }

        private void txtCaspaseActivity_LostFocus(object sender, RoutedEventArgs e)
        {
            SolidColorBrush Redbrush = new SolidColorBrush(Colors.Red);
            SolidColorBrush Blackbrush = new SolidColorBrush(Colors.Black);
            if (!String.IsNullOrEmpty(txtCaspaseActivity.Text))
            {
                double value = Convert.ToDouble(txtCaspaseActivity.Text);
                if (value >= 0.1 && value <= 0.2)
                {
                    txtCaspaseActivity.Foreground = Blackbrush;
                    txtCaspaseActivity.FontWeight = FontWeights.Normal;
                }
                else
                {
                    txtCaspaseActivity.Foreground = Redbrush;
                    txtCaspaseActivity.FontWeight = FontWeights.Bold;
                }
            }
        }

        private void txtAcrosinActivity_LostFocus(object sender, RoutedEventArgs e)
        {
            SolidColorBrush Redbrush = new SolidColorBrush(Colors.Red);
            SolidColorBrush Blackbrush = new SolidColorBrush(Colors.Black);

            if (!String.IsNullOrEmpty(txtAcrosinActivity.Text))
            {
                double value = Convert.ToDouble(txtAcrosinActivity.Text);
                if (value >= 16)
                {
                    txtAcrosinActivity.Foreground = Blackbrush;
                    txtAcrosinActivity.FontWeight = FontWeights.Normal;
                }
                else
                {
                    txtAcrosinActivity.Foreground = Redbrush;
                    txtAcrosinActivity.FontWeight = FontWeights.Bold;
                }
            }
        }

        private void txtGlucosidaseActivity_LostFocus(object sender, RoutedEventArgs e)
        {
            SolidColorBrush Redbrush = new SolidColorBrush(Colors.Red);
            SolidColorBrush Blackbrush = new SolidColorBrush(Colors.Black);

            if (!String.IsNullOrEmpty(txtGlucosidaseActivity.Text))
            {
                double value = Convert.ToDouble(txtGlucosidaseActivity.Text);
                if (value >= 0.3)
                {
                    txtGlucosidaseActivity.Foreground = Blackbrush;
                     txtGlucosidaseActivity.FontWeight = FontWeights.Normal;
                }
                else
                {
                    txtGlucosidaseActivity.Foreground = Redbrush;
                    txtGlucosidaseActivity.FontWeight = FontWeights.Bold;
                }
            }
        }

        private void txtCaspaseActivity_TextChanged(object sender, TextChangedEventArgs e)
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
            SolidColorBrush Redbrush = new SolidColorBrush(Colors.Red);
            SolidColorBrush Blackbrush = new SolidColorBrush(Colors.Black);
            if (!String.IsNullOrEmpty(txtCaspaseActivity.Text))
            {
                double value = Convert.ToDouble(txtCaspaseActivity.Text);
                if (value >= 0.1 && value <= 0.2)
                {
                    txtCaspaseActivity.Foreground = Blackbrush;
                    txtCaspaseActivity.FontWeight = FontWeights.Normal;
                }
                else
                {
                    txtCaspaseActivity.Foreground = Redbrush;
                    txtCaspaseActivity.FontWeight = FontWeights.Bold;
                }
            }
        }

        private void txtAcrosinActivity_TextChanged(object sender, TextChangedEventArgs e)
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
            SolidColorBrush Redbrush = new SolidColorBrush(Colors.Red);
            SolidColorBrush Blackbrush = new SolidColorBrush(Colors.Black);

            if (!String.IsNullOrEmpty(txtAcrosinActivity.Text))
            {
                double value = Convert.ToDouble(txtAcrosinActivity.Text);
                if (value >= 16)
                {
                    txtAcrosinActivity.Foreground = Blackbrush;
                    txtAcrosinActivity.FontWeight = FontWeights.Normal;
                }
                else
                {
                    txtAcrosinActivity.Foreground = Redbrush;
                    txtAcrosinActivity.FontWeight = FontWeights.Bold;
                }
            }
        }

        private void txtGlucosidaseActivity_TextChanged(object sender, TextChangedEventArgs e)
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
            SolidColorBrush Redbrush = new SolidColorBrush(Colors.Red);
            SolidColorBrush Blackbrush = new SolidColorBrush(Colors.Black);

            if (!String.IsNullOrEmpty(txtGlucosidaseActivity.Text))
            {
                double value = Convert.ToDouble(txtGlucosidaseActivity.Text);
                if (value >= 0.3)
                {
                    txtGlucosidaseActivity.Foreground = Blackbrush;
                    txtGlucosidaseActivity.FontWeight = FontWeights.Normal;
                }
                else
                {
                    txtGlucosidaseActivity.Foreground = Redbrush;
                    txtGlucosidaseActivity.FontWeight = FontWeights.Bold;
                }
            }
        }

        private void PrintReport(long pID, long pUnitID)
        {
            if (pID > 0)
            {
                string URL = "../Reports/IVFDashboard/IVFDashboard_SpermDevelopmentalDefectTest.aspx?ID=" + pID + "&UnitID=" + pUnitID ;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgSDDTList.SelectedItem != null)
                {
                    ID = ((clsIVFSpermDefectTestVO)dgSDDTList.SelectedItem).ID;
                    UnitID = ((clsIVFSpermDefectTestVO)dgSDDTList.SelectedItem).UnitId;
                    PrintReport(ID, UnitID);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
