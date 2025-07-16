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
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using System.Reflection;
using CIMS;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;

namespace PalashDynamics.Administration
{
    public partial class FrmCleavageGradeMaster : UserControl
    {

        #region Properties
        public int PageSize
        {
            get
            {
                return MasterList.PageSize;
            }
            set
            {
                if (value == MasterList.PageSize) return;
                MasterList.PageSize = value;
                //OnPropertyChanged("PageSize");
            }
        }
        #endregion

        #region  Variables

        private SwivelAnimation objAnimation;
        WaitIndicator objProgress = new WaitIndicator();
        string msgTitle = "PALASHDYNAMICS";
        string msgText = "";
        public PagedSortableCollectionView<clsSurrogateAgencyMasterVO> MasterList { get; private set; }
        bool IsCancel = true;
        bool IsViewRec = false;
        #endregion

        public FrmCleavageGradeMaster()
        {
            InitializeComponent();
            objProgress.Show();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            SetCommandButtonState("Load");
            MasterList = new PagedSortableCollectionView<clsSurrogateAgencyMasterVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdCleavageGrade.DataContext = MasterList;
            FillApplyTo();
            FillFlag();
            SetupPage();
        }

        private void FillApplyTo()
        {
            List<MasterListItem> objApplyToList = new List<MasterListItem>();
            objApplyToList.Add(new MasterListItem(0, "- Select -"));
            objApplyToList.Add(new MasterListItem(1, "2"));
            objApplyToList.Add(new MasterListItem(2, "3"));
            objApplyToList.Add(new MasterListItem(3, "4"));
            cmbApplyTo.ItemsSource = null;
            cmbApplyTo.ItemsSource = objApplyToList;
            cmbApplyTo.SelectedItem = objApplyToList[0];
        }

        private void FillFlag()
        {
            List<MasterListItem> objFlagList = new List<MasterListItem>();
            objFlagList.Add(new MasterListItem(0, "- Select -"));
            objFlagList.Add(new MasterListItem(1, "Good"));
            objFlagList.Add(new MasterListItem(2, "Fair"));
            objFlagList.Add(new MasterListItem(3, "Poor"));
            cmbFlag.ItemsSource = null;
            cmbFlag.ItemsSource = objFlagList;
            cmbFlag.SelectedItem = objFlagList[0];
        }

        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            SetupPage();
        }

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

        bool isnew = false;
        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("New");
            isnew = true;
            grdBackPanel.DataContext = new clsSurrogateAgencyMasterVO();           
            ClearUI();
            try
            {
                objAnimation.Invoke(RotationType.Forward);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void ClearUI()
        {
            txtCode.Text = string.Empty;
            txtName.Text = string.Empty;
            txtDescription.Text = string.Empty;           
            cmbApplyTo.SelectedValue = (long)0;
            cmbFlag.SelectedValue = (long)0;            
        }

        #region Validation
        public bool Validation()
        {
            bool retVal = true;
            if (string.IsNullOrEmpty(txtCode.Text.Trim()))
            {
                txtCode.SetValidation("Please Enter Code");
                txtCode.RaiseValidationError();
                txtCode.Focus();
                retVal = false;
            }
            else
                txtCode.ClearValidationError();
            if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
            {
                txtDescription.SetValidation("Please Enter Description");
                txtDescription.RaiseValidationError();
                if (retVal == true)
                    txtDescription.Focus();
                retVal = false;
            }
            else
                txtDescription.ClearValidationError();
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                txtDescription.SetValidation("Please Enter Name");
                txtDescription.RaiseValidationError();
                if (retVal == true)
                    txtDescription.Focus();
                retVal = false;
            }
            else
                txtName.ClearValidationError();

            if (cmbApplyTo.SelectedItem == null)
            {
                cmbApplyTo.TextBox.SetValidation("Please Select Apply To Day");
                cmbApplyTo.TextBox.RaiseValidationError();
                if (retVal == true)
                    cmbApplyTo.Focus();
                retVal = false;

            }
            else if (((MasterListItem)cmbApplyTo.SelectedItem).ID == 0)
            {

                cmbApplyTo.TextBox.SetValidation("Please Select Referral Type");
                cmbApplyTo.TextBox.RaiseValidationError();
                if (retVal == true)
                    cmbApplyTo.Focus();
                retVal = false;

            }
            else
                cmbApplyTo.TextBox.ClearValidationError();



            if (cmbFlag.SelectedItem == null)
            {
                cmbFlag.TextBox.SetValidation("Please Select Flag");
                cmbFlag.TextBox.RaiseValidationError();
                if (retVal == true)
                    cmbFlag.Focus();
                retVal = false;

            }
            else if (((MasterListItem)cmbFlag.SelectedItem).ID == 0)
            {

                cmbFlag.TextBox.SetValidation("Please Select Flag");
                cmbFlag.TextBox.RaiseValidationError();
                if (retVal == true)
                    cmbFlag.Focus();
                retVal = false;

            }
            else
                cmbFlag.TextBox.ClearValidationError();

            return retVal;


        }

        #endregion

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to save?";

                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);

                msgWin.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Required Fields To Save The Record", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }

        }

        void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (CheckDuplicasy())
                {
                    try
                    {
                        objProgress.Show();
                        clsAddUpdateCleavageGradeMasterBizActionVO BizActionVO = new clsAddUpdateCleavageGradeMasterBizActionVO();                     
                        BizActionVO.CleavageDetails = (clsCleavageGradeMasterVO)grdBackPanel.DataContext;

                        BizActionVO.CleavageDetails.Description = txtDescription.Text.Trim();
                        BizActionVO.CleavageDetails.Code = txtCode.Text.Trim();
                        BizActionVO.CleavageDetails.Name = txtName.Text.Trim();

                        if (cmbApplyTo.SelectedItem != null)
                            BizActionVO.CleavageDetails.ApplyTo = ((MasterListItem)cmbApplyTo.SelectedItem).ID;
                        if (cmbFlag.SelectedItem != null)
                            BizActionVO.CleavageDetails.Flag = ((MasterListItem)cmbFlag.SelectedItem).Description;
                        if (((clsCleavageGradeMasterVO)grdBackPanel.DataContext).ID != 0)
                            BizActionVO.CleavageDetails.Status = ((clsCleavageGradeMasterVO)grdCleavageGrade.SelectedItem).Status;
                        else
                            BizActionVO.CleavageDetails.Status = true;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, args) =>
                        {

                            if (args.Error == null && args.Result != null)
                            {
                                if (((clsAddUpdateCleavageGradeMasterBizActionVO)args.Result).SuccessStatus == 1)
                                {
                                    objProgress.Close();
                                    msgText = "Record Saved Successfully";
                                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgWindow.Show();
                                    //After Insertion Back to BackPanel and Setup Page
                                    objAnimation.Invoke(RotationType.Backward);
                                    ClearUI();
                                    SetupPage();
                                    SetCommandButtonState("Save");
                                    isnew = false;
                                }
                                else if (((clsAddUpdateCleavageGradeMasterBizActionVO)args.Result).SuccessStatus == 2)
                                {
                                    objProgress.Close();
                                    msgText = "Record Updated Successfully";
                                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgWindow.Show();
                                    objAnimation.Invoke(RotationType.Backward);
                                    ClearUI();
                                    SetupPage();
                                    SetCommandButtonState("Modify");
                                    isnew = false;
                                }
                            }
                            else
                            {
                                objProgress.Close();
                                msgText = "Record cannot be save ,Error occured.";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();

                            }

                        };
                        client.ProcessAsync(BizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client.CloseAsync();
                    }
                    catch (Exception)
                    {
                        objProgress.Close();
                        msgText = "Record cannot be save ,Error occured.";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                    }
                }
            }
        }



        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to update?";

                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);

                msgWin.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Required Fields To Update The Record", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            objAnimation.Invoke(RotationType.Backward);
            isnew = false;
            if (IsCancel == true)
            {
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "IVF Configuration";

                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmIVFConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

            }
            else
            {
                IsCancel = true;
            }
        }

        private void SetupPage()
        {
            
        }

        private bool CheckDuplicasy()
        {
            return true;
        }

        private void txtName_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            SetupPage();
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            objProgress.Show();
            MasterList = new PagedSortableCollectionView<clsSurrogateAgencyMasterVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdCleavageGrade.DataContext = MasterList;
            SetupPage();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                SetupPage();
            }
        }

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void txtCode_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtDescription_LostFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}
