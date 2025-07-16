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
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.ComponentModel;
using PalashDynamics.Collections;
using System.Reflection;
using PalashDynamics.UserControls;

namespace PalashDynamics.Administration
{
    public partial class frmCashCounterMaster : UserControl
    {

         #region  Variables

        private SwivelAnimation objAnimation;
        WaitIndicator objProgress = new WaitIndicator();
        string msgTitle = "PALASHDYNAMICS";
        string msgText = "";
        private long BankBranchId;
        public PagedSortableCollectionView<clsCashCounterVO> MasterList { get; private set; }
        bool IsCancel = true;
        #endregion

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

        public frmCashCounterMaster()
        {
            InitializeComponent();
            objProgress.Show();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            //this.Loaded += new RoutedEventHandler(BankBranch_Loaded);
            SetCommandButtonState("Load");
            BankBranchId = 0;
            FillClinic();
            MasterList = new PagedSortableCollectionView<clsCashCounterVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdCityDetails.DataContext = MasterList;
            SetupPage();
        }

        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            SetupPage();
        }

        #region FillCombobox

        public void FillClinic()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
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

                    cmbClinic.ItemsSource = null;
                    cmbClinic.ItemsSource = objList;

                    cmbSearchCountry.ItemsSource = null;
                    cmbSearchCountry.ItemsSource = objList;
                }
                if (this.DataContext != null)
                {
                    //cmbClinic.SelectedValue = ((MasterListItem)this.DataContext).ID;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }


        #endregion FillCombobox

        void SetupPage()
        {
            try
            {
                clsGetCashCounterDetailsBizActionVO bizActionVO = new clsGetCashCounterDetailsBizActionVO();
                

                bizActionVO.Description = txtSearch.Text.Trim();              
                if (cmbSearchCountry.SelectedItem != null)
                    bizActionVO.ClinicId = ((MasterListItem)cmbSearchCountry.SelectedItem).ID;

                bizActionVO.SearchExpression = null;
                bizActionVO.PagingEnabled = true;
                bizActionVO.MaximumRows = MasterList.PageSize;
                bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;

                clsCashCounterVO objCashCOVo = new clsCashCounterVO();
                bizActionVO.ListCashCounterDetails = new List<clsCashCounterVO>();


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {

                    if (args.Error == null && args.Result != null)
                    {

                        bizActionVO.ListCashCounterDetails = (((clsGetCashCounterDetailsBizActionVO)args.Result).ListCashCounterDetails);
                        MasterList.Clear();
                        MasterList.TotalItemCount = (int)(((clsGetCashCounterDetailsBizActionVO)args.Result).TotalRows);
                        ///Setup Page Fill DataGrid
                        if (bizActionVO.ListCashCounterDetails.Count > 0)
                        {
                            foreach (clsCashCounterVO item in bizActionVO.ListCashCounterDetails)
                            {
                                MasterList.Add(item);
                            }
                        }
                        objProgress.Close();
                    }
                    else
                    {
                        objProgress.Close();
                    }

                };
                client.ProcessAsync(bizActionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                objProgress.Close();
            }

        }


        #region Validation

        public bool Validation()
        {
            bool retval = true;
            
            if (string.IsNullOrEmpty(txtCashCounterCode.Text.Trim()))
            {
                txtCashCounterCode.SetValidation("Please Enter Code");
                txtCashCounterCode.RaiseValidationError();
                txtCashCounterCode.Focus();
                retval = false;
            }
            else
                txtCashCounterCode.ClearValidationError();
            if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
            {
                txtDescription.SetValidation("Please Enter Cash Counter Name");
                txtDescription.RaiseValidationError();
                if (retval == true)
                    txtDescription.Focus();
                retval = false;
            }
            else
                txtDescription.ClearValidationError();
            if (retval == true)
            {
                if (cmbClinic.SelectedItem == null)
                {
                    cmbClinic.TextBox.SetValidation("Please Select Clinic Name");
                    cmbClinic.TextBox.RaiseValidationError();
                    if (retval == true)
                        cmbClinic.Focus();
                    retval = false;

                }
                else if (((MasterListItem)cmbClinic.SelectedItem).ID == 0)
                {
                    cmbClinic.TextBox.SetValidation("Please Select Clinic Name");
                    cmbClinic.TextBox.RaiseValidationError();
                    if (retval == true)
                        cmbClinic.Focus();
                    retval = false;

                }
                else
                    cmbClinic.TextBox.ClearValidationError();
                
            }
            
            return retval;
        }

        #endregion

        public void ClearUI()
        {
            txtDescription.Text = "";
            txtCashCounterCode.Text = "";
            if (cmbClinic.ItemsSource != null)
            {
                MasterListItem Defaultc = ((List<MasterListItem>)cmbClinic.ItemsSource).FirstOrDefault(s => s.ID == 0);
                cmbClinic.SelectedItem = Defaultc;
            }
        }

        #region Set Command Button State New/Save/Modify/Print

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

        #endregion

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            ClearUI();
            Validation();
            //Border border = new Border();
            //border.BorderBrush = new SolidColorBrush(Colors.Red);
            //border.Child = cmbClinic;
            SetCommandButtonState("New");
            grdBackPanel.DataContext = new clsBankBranchVO();
            
            try
            {
                objAnimation.Invoke(RotationType.Forward);
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                string msgTitle = "";
                string msgText = "Are You Sure You Want to Save Record ?";

                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);

                msgWin.Show();
            }
            else
            { Validation(); }
        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                string msgTitle = "";
                string msgText = "Are You Sure You Want to Update Record ?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();
            }
            else
            { Validation(); }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            txtCashCounterCode.ClearValidationError();
            txtDescription.ClearValidationError();
            SetCommandButtonState("Cancel");
            BankBranchId = 0;
            objAnimation.Invoke(RotationType.Backward);
            if (IsCancel == true)
            {
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Clinic Configuration";

                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmClinicConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

            }
            else
            {
                IsCancel = true;
            }
        }

        void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    objProgress.Show();
                    clsAddUpadateCashCounterBizActionVO BizActionVO = new clsAddUpadateCashCounterBizActionVO();
                    clsCashCounterVO objStatDet = new clsCashCounterVO();

                    objStatDet.Id = 0;
                    objStatDet.Description = txtDescription.Text.Trim();
                    objStatDet.Code = txtCashCounterCode.Text.Trim();
                    objStatDet.ClinicId = ((MasterListItem)cmbClinic.SelectedItem).ID;
                    objStatDet.Status = true;

                    objStatDet.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    objStatDet.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    objStatDet.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    objStatDet.AddedDateTime = System.DateTime.Now;
                    objStatDet.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    BizActionVO.ObjCashCounter = objStatDet;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpadateCashCounterBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                objProgress.Close();
                                msgText = "Record Saved Successfully";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                //After Insertion Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                BankBranchId = 0;
                                SetupPage();
                                SetCommandButtonState("Save");
                            }
                            else if (((clsAddUpadateCashCounterBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                objProgress.Close();
                                msgText = "Record cannot be save because CODE already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpadateCashCounterBizActionVO)args.Result).SuccessStatus == 3)
                            {
                                objProgress.Close();
                                msgText = "Record cannot be save because DESCRIPTION already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
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
                    msgText = "Record cannot be save because DESCRIPTION already exist!";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWindow.Show();
                }
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    objProgress.Show();
                    clsAddUpadateCashCounterBizActionVO BizActionVO = new clsAddUpadateCashCounterBizActionVO();
                    clsCashCounterVO objCashCODet = new clsCashCounterVO();

                    objCashCODet.Id = Convert.ToInt64(txtCashCounterCode.Tag);
                    objCashCODet.Description = txtDescription.Text.Trim();
                    objCashCODet.Code = txtCashCounterCode.Text.Trim();
                    objCashCODet.ClinicId = ((MasterListItem)cmbClinic.SelectedItem).ID;
                    objCashCODet.Status = true;

                    objCashCODet.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    objCashCODet.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    objCashCODet.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    objCashCODet.AddedDateTime = System.DateTime.Now;
                    objCashCODet.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    BizActionVO.ObjCashCounter = objCashCODet;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpadateCashCounterBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                objProgress.Close();
                                msgText = "Record Updated Successfully.";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                //After Insertion Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                BankBranchId = 0;
                                SetupPage();
                                SetCommandButtonState("Save");
                            }
                            else if (((clsAddUpadateCashCounterBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                objProgress.Close();
                                msgText = "Record cannot be save because CODE already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpadateCashCounterBizActionVO)args.Result).SuccessStatus == 3)
                            {
                                objProgress.Close();
                                msgText = "Record cannot be save because DESCRIPTION already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }

                        }
                        else
                        {
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
                    msgText = "Record cannot be save , Error Occured!";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWindow.Show();
                }
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            objProgress.Show();
            MasterList = new PagedSortableCollectionView<clsCashCounterVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdCityDetails.DataContext = MasterList;
            SetupPage();
        }

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {

            if (grdCityDetails.SelectedItem != null)
            {
                objProgress.Show();
                ClearUI();
                SetCommandButtonState("View");
                txtCashCounterCode.Tag = Convert.ToInt64(((clsCashCounterVO)grdCityDetails.SelectedItem).Id);
                txtCashCounterCode.Text = Convert.ToString(((clsCashCounterVO)grdCityDetails.SelectedItem).Code);
                txtDescription.Text = Convert.ToString(((clsCashCounterVO)grdCityDetails.SelectedItem).Description);
                cmbClinic.SelectedValue = Convert.ToInt64(((clsCashCounterVO)grdCityDetails.SelectedItem).ClinicId);
                cmdModify.IsEnabled = ((clsCashCounterVO)grdCityDetails.SelectedItem).Status;
                try
                {
                    objAnimation.Invoke(RotationType.Forward);
                }
                catch (Exception)
                {
                    objProgress.Close();
                    throw;
                }
                objProgress.Close();
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (grdCityDetails.SelectedItem != null)
            {
                try
                {
                  //  objProgress.Show();
                    clsAddUpadateCashCounterBizActionVO BizActionVO = new clsAddUpadateCashCounterBizActionVO();
                    clsCashCounterVO objCashCODet = new clsCashCounterVO();

                    objCashCODet.Id = Convert.ToInt64(((clsCashCounterVO)grdCityDetails.SelectedItem).Id);
                    objCashCODet.Description = ((clsCashCounterVO)grdCityDetails.SelectedItem).Description;
                    objCashCODet.Code = ((clsCashCounterVO)grdCityDetails.SelectedItem).Code;
                    objCashCODet.ClinicId = ((clsCashCounterVO)grdCityDetails.SelectedItem).ClinicId;
                    objCashCODet.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
                    objCashCODet.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    objCashCODet.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    objCashCODet.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    objCashCODet.AddedDateTime = System.DateTime.Now;
                    objCashCODet.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    BizActionVO.ObjCashCounter = objCashCODet;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpadateCashCounterBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                objProgress.Close();
                                msgText = "Status Updated Successfully.";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                SetupPage();
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
               

        private void txtDescription_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtDescription.Text.IsItUpperCase() != true)
                txtDescription.Text = txtDescription.Text.ToTitleCase();
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SetupPage();
                dataGrid2Pager.PageIndex = 0;
            }
        }

        private void txtCashCounterCode_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtCashCounterCode.Text.IsItUpperCase() == false)
                txtCashCounterCode.Text = txtCashCounterCode.Text.ToTitleCase();
        }
    }
}
