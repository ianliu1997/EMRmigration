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
using System.ComponentModel;
using CIMS;
using PalashDynamics.Animations;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Administration.IPD;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using System.Reflection;
using PalashDynamics.ValueObjects.IPD;

namespace PalashDynamics.Administration
{
    public partial class frmBedReleseCheckList : UserControl
    {

        #region IPreInitiateCIMS Members

        PalashDynamics.ValueObjects.Administration.clsMenuVO _SelfMenuDetails = null;

        public void PreInitiate(PalashDynamics.ValueObjects.Administration.clsMenuVO _MenuDetails)
        {
            _SelfMenuDetails = _MenuDetails;
        }

        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion



        #region  Variables
        private SwivelAnimation objAnimation;
        string msgTitle = "PALASHDYNAMICS";
        string msgText = "";
        private long UnitId;
        int PageIndex = 0;
        public PagedSortableCollectionView<clsIPDBedReleaseCheckListVO> MasterList { get; private set; }
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
                OnPropertyChanged("PageSize");
            }
        }
        #endregion
        public frmBedReleseCheckList()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(frmBedReleaseCheckList_Loaded);
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            SetCommandButtonState("Load");
            MasterList = new PagedSortableCollectionView<clsIPDBedReleaseCheckListVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdBedReleaseCheckListDetails.DataContext = MasterList;
            SetupPage();
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


        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            PageIndex = dataGrid2Pager.PageIndex;
            SetupPage();
        }

        #region Load Event
        void frmBedReleaseCheckList_Loaded(object sender, RoutedEventArgs e)
        {
            if (grdBedReleaseCheckListDetails != null)
            {
                if (grdBedReleaseCheckListDetails.Columns.Count > 0)
                {
                    grdBedReleaseCheckListDetails.Columns[0].Header = "View";
                    grdBedReleaseCheckListDetails.Columns[1].Header = "Code";
                    grdBedReleaseCheckListDetails.Columns[2].Header = "Description";
                    grdBedReleaseCheckListDetails.Columns[3].Header = "Is Mandatory";
                    grdBedReleaseCheckListDetails.Columns[4].Header = "Status";
                }
            }
        }
        #endregion Load Event


        public void ClearUI()
        {
            txtDescription.Text = "";
            txtCode.Text = "";
            chkIsMandantory.IsChecked = false;
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            grdBackPanel.DataContext = new clsIPDBedReleaseCheckListVO();
            SetCommandButtonState("New");
            Validation();
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


        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                msgText = "Are you sure you want to Save ?";
                MessageBoxControl.MessageBoxChildWindow SavemsgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                SavemsgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(SavemsgWin_OnMessageBoxClosed);
                SavemsgWin.Show();
            }
        }


        void SavemsgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsAddUpdateBedReleaseCheckListDetailsBizActionVO bizactionVO = new clsAddUpdateBedReleaseCheckListDetailsBizActionVO();
                clsIPDBedReleaseCheckListVO addNewDepartmentVO = new clsIPDBedReleaseCheckListVO();
                try
                {
                    addNewDepartmentVO.Id = 0;
                    addNewDepartmentVO.Code = txtCode.Text.Trim();

                    addNewDepartmentVO.Description = txtDescription.Text.Trim();
                    addNewDepartmentVO.IsMandantory = (bool)chkIsMandantory.IsChecked;
                    addNewDepartmentVO.Status = true;
                    addNewDepartmentVO.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewDepartmentVO.AddUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewDepartmentVO.By = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewDepartmentVO.On = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewDepartmentVO.DateTime = System.DateTime.Now;
                    // addNewDepartmentVO.WindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.ItemMatserDetails.Add(addNewDepartmentVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpdateBedReleaseCheckListDetailsBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                msgText = "Record Saved Successfully.";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                //After Insertion Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                UnitId = 0;
                                SetupPage();
                                SetCommandButtonState("Save");
                            }
                            else if (((clsAddUpdateBedReleaseCheckListDetailsBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                msgText = "Record cannot be save because CODE already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpdateBedReleaseCheckListDetailsBizActionVO)args.Result).SuccessStatus == 3)
                            {
                                msgText = "Record cannot be save because DESCRIPTION already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                        }
                    };
                    client.ProcessAsync(bizactionVO, new clsUserVO());
                    client.CloseAsync();
                }
                catch (Exception ex)
                {

                }
            }
        }

        public void SetupPage()
        {
            clsGetBedReleaseCheckListBizActionVO bizActionVO = new clsGetBedReleaseCheckListBizActionVO();
            bizActionVO.SearchExpression = txtSearch.Text;
            bizActionVO.PagingEnabled = true;
            bizActionVO.MaximumRows = MasterList.PageSize;
            bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;

            clsIPDBedReleaseCheckListVO getDepartmentinfo = new clsIPDBedReleaseCheckListVO();
            bizActionVO.ItemMatserDetails = new List<clsIPDBedReleaseCheckListVO>();
            try
            {

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {

                    if (args.Error == null && args.Result != null)
                    {


                        bizActionVO.ItemMatserDetails = (((clsGetBedReleaseCheckListBizActionVO)args.Result).ItemMatserDetails);

                        ///Setup Page Fill DataGrid
                        if (UnitId == 0 && bizActionVO.ItemMatserDetails.Count > 0)
                        {
                            MasterList.Clear();
                            MasterList.TotalItemCount = (int)(((clsGetBedReleaseCheckListBizActionVO)args.Result).TotalRows);
                            foreach (clsIPDBedReleaseCheckListVO item in bizActionVO.ItemMatserDetails)
                            {
                                MasterList.Add(item);
                            }

                        }

                    }

                };
                client.ProcessAsync(bizActionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {

            }

        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                msgText = "Are you sure you want to Update ?";
                MessageBoxControl.MessageBoxChildWindow ModifymsgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                ModifymsgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(ModifymsgWin_OnMessageBoxClosed);
                ModifymsgWin.Show();
            }
        }

        void ModifymsgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsAddUpdateBedReleaseCheckListDetailsBizActionVO bizactionVO = new clsAddUpdateBedReleaseCheckListDetailsBizActionVO();
                clsIPDBedReleaseCheckListVO addNewDepartmentVO = new clsIPDBedReleaseCheckListVO();
                try
                {
                    if (grdBedReleaseCheckListDetails.SelectedItem != null)
                    {
                        addNewDepartmentVO.Id = ((clsIPDBedReleaseCheckListVO)grdBedReleaseCheckListDetails.SelectedItem).Id;
                        addNewDepartmentVO.Code = txtCode.Text;

                        addNewDepartmentVO.Description = txtDescription.Text;
                        addNewDepartmentVO.IsMandantory = (bool)chkIsMandantory.IsChecked;
                        addNewDepartmentVO.Status = true;
                        addNewDepartmentVO.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        addNewDepartmentVO.AddUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        addNewDepartmentVO.By = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        addNewDepartmentVO.On = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                        addNewDepartmentVO.DateTime = System.DateTime.Now;
                        // addNewDepartmentVO.WindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                        bizactionVO.ItemMatserDetails.Add(addNewDepartmentVO);
                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, args) =>
                        {

                            if (args.Error == null && args.Result != null)
                            {
                                if (((clsAddUpdateBedReleaseCheckListDetailsBizActionVO)args.Result).SuccessStatus == 1)
                                {
                                    msgText = "Record Updated Successfully.";
                                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgWindow.Show();
                                    //After Insertion Back to BackPanel and Setup Page
                                    objAnimation.Invoke(RotationType.Backward);
                                    UnitId = 0;
                                    SetupPage();
                                    SetCommandButtonState("Modify");


                                }
                                else if (((clsAddUpdateBedReleaseCheckListDetailsBizActionVO)args.Result).SuccessStatus == 2)
                                {
                                    msgText = "Record cannot be save because CODE already exist!";
                                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    msgWindow.Show();
                                }
                                else if (((clsAddUpdateBedReleaseCheckListDetailsBizActionVO)args.Result).SuccessStatus == 3)
                                {
                                    msgText = "Record cannot be save because DESCRIPTION already exist!";
                                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    msgWindow.Show();
                                }
                            }
                        };
                        client.ProcessAsync(bizactionVO, new clsUserVO());
                        client.CloseAsync();
                    }
                }
                catch (Exception ex)
                {

                }
            }

        }

        #region Validation
        public bool Validation()
        {
            bool result = true;
            if (string.IsNullOrEmpty(txtCode.Text))
            {
                msgText = "Please Enter Code";
                txtCode.SetValidation(msgText);
                txtCode.RaiseValidationError();
                txtCode.Focus();
                result = false;
            }
            else
                txtCode.ClearValidationError();

            if (string.IsNullOrEmpty(txtDescription.Text) || string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                msgText = "Please Enter Description";
                txtDescription.SetValidation(msgText);
                txtDescription.RaiseValidationError();
                txtDescription.Focus();
                result = false;
            }

            else

                txtDescription.ClearValidationError();
            return result;
        }

        #endregion

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            txtCode.ClearValidationError();
            txtDescription.ClearValidationError();
            SetCommandButtonState("Cancel");
            SetupPage();
            UnitId = 0;
            objAnimation.Invoke(RotationType.Backward);

            if (IsCancel == true)
            {
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Admission Configuration";

                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmIPDAdmissionConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

            }
            else
            {
                IsCancel = true;
            }


        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            MasterList = new PagedSortableCollectionView<clsIPDBedReleaseCheckListVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdBedReleaseCheckListDetails.DataContext = MasterList;

            SetupPage();

        }

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {

            SetCommandButtonState("View");
            cmdModify.IsEnabled = ((clsIPDBedReleaseCheckListVO)grdBedReleaseCheckListDetails.SelectedItem).Status;
            Validation();
            UnitId = ((clsIPDBedReleaseCheckListVO)grdBedReleaseCheckListDetails.SelectedItem).Id;
            grdBackPanel.DataContext = ((clsIPDBedReleaseCheckListVO)grdBedReleaseCheckListDetails.SelectedItem).DeepCopy(); ;

            try
            {
                objAnimation.Invoke(RotationType.Forward);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (grdBedReleaseCheckListDetails.SelectedItem != null)
            {
                try
                {
                    clsAddUpdateBedReleaseCheckListDetailsBizActionVO bizactionVO = new clsAddUpdateBedReleaseCheckListDetailsBizActionVO();
                    clsIPDBedReleaseCheckListVO addStatusVO = new clsIPDBedReleaseCheckListVO();
                    addStatusVO.Id = ((clsIPDBedReleaseCheckListVO)grdBedReleaseCheckListDetails.SelectedItem).Id;
                    addStatusVO.Code = ((clsIPDBedReleaseCheckListVO)grdBedReleaseCheckListDetails.SelectedItem).Code;
                    addStatusVO.IsMandantory = ((clsIPDBedReleaseCheckListVO)grdBedReleaseCheckListDetails.SelectedItem).IsMandantory;
                    addStatusVO.Description = ((clsIPDBedReleaseCheckListVO)grdBedReleaseCheckListDetails.SelectedItem).Description;
                    addStatusVO.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
                    addStatusVO.UnitId = ((clsIPDBedReleaseCheckListVO)grdBedReleaseCheckListDetails.SelectedItem).UnitId;
                    addStatusVO.AddUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addStatusVO.By = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addStatusVO.On = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addStatusVO.DateTime = System.DateTime.Now;
                    bizactionVO.ItemMatserDetails.Add(addStatusVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpdateBedReleaseCheckListDetailsBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                UnitId = 0;
                                SetupPage();
                                msgText = "Status Updated Successfully.";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                objAnimation.Invoke(RotationType.Backward);
                                SetCommandButtonState("Modify");
                            }
                        }
                    };
                    client.ProcessAsync(bizactionVO, new clsUserVO());
                    client.CloseAsync();
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void txtCode_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
            {
                if (!((TextBox)e.OriginalSource).Text.IsItSpecialChar())
                {
                    ((TextBox)e.OriginalSource).Text = string.Empty;
                    ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Red);
                    msgText = "Special characters are not allowed.";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
                else
                {
                    ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Gray);
                    txtCode.Text = txtCode.Text.ToTitleCase();

                }
            }
        }

        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = CIMS.Comman.HandleAlphnumericAndSpecialChar(sender, e, false);
        }

        private void txtDescription_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = CIMS.Comman.HandleAlphnumericAndSpecialChar(sender, e, true);
        }

        private void txtDescription_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
            {
                if (!((TextBox)e.OriginalSource).Text.AllowSpecialChar())
                {
                    ((TextBox)e.OriginalSource).Text = string.Empty;
                    ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Red);
                    msgText = "Only & ,.,- and space special characters are allowed.";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
                else
                {
                    ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Gray);
                    txtDescription.Text = txtDescription.Text.ToTitleCase();
                }
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                MasterList = new PagedSortableCollectionView<clsIPDBedReleaseCheckListVO>();
                MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                PageSize = 15;
                this.dataGrid2Pager.DataContext = MasterList;
                this.grdBedReleaseCheckListDetails.DataContext = MasterList;

                SetupPage();
            }
        }

    }
}

