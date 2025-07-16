//Created Date:19/August/2012
//Created By: Nilesh Raut
//Specification: To Add,Update the State Master

//Review By:
//Review Date:

//Modified By:
//Modified Date: 

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
    public partial class frmStateMaster : UserControl
    {

        #region  Variables

        private SwivelAnimation objAnimation;
        WaitIndicator objProgress = new WaitIndicator();
        string msgTitle = "PALASHDYNAMICS";
        string msgText = "";
        private long BankBranchId;
        public PagedSortableCollectionView<clsStateVO> MasterList { get; private set; }
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
        public frmStateMaster()
        {
            InitializeComponent();
            objProgress.Show();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            //this.Loaded += new RoutedEventHandler(BankBranch_Loaded);
            SetCommandButtonState("Load");
            BankBranchId = 0;
            FillCountry();
            MasterList = new PagedSortableCollectionView<clsStateVO>();
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

        public void FillCountry()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_CountryMaster;
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

                    cmbCountry.ItemsSource = null;
                    cmbCountry.ItemsSource = objList;

                    cmbSearchCountry.ItemsSource = null;
                    cmbSearchCountry.ItemsSource = objList;
                    cmbSearchCountry.SelectedItem = objList[0];
                    //cmbSearchCountry

                }

                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
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
                clsGetStateDetailsBizActionVO bizActionVO = new clsGetStateDetailsBizActionVO();

                bizActionVO.Description = txtSearch.Text.Trim();
                //bizActionVO.Code = txtCityCode.Text.Trim();
                if (cmbSearchCountry.SelectedItem != null)
                    bizActionVO.CountryId = ((MasterListItem)cmbSearchCountry.SelectedItem).ID;

                bizActionVO.SearchExpression = null;
                bizActionVO.PagingEnabled = true;
                bizActionVO.MaximumRows = MasterList.PageSize;
                bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;

                clsStateVO objStateVo = new clsStateVO();
                bizActionVO.ListStateDetails = new List<clsStateVO>();


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {

                    if (args.Error == null && args.Result != null)
                    {

                        bizActionVO.ListStateDetails = (((clsGetStateDetailsBizActionVO)args.Result).ListStateDetails);
                        MasterList.Clear();
                        MasterList.TotalItemCount = (int)(((clsGetStateDetailsBizActionVO)args.Result).TotalRows);
                        ///Setup Page Fill DataGrid
                        if (bizActionVO.ListStateDetails.Count > 0)
                        {
                            foreach (clsStateVO item in bizActionVO.ListStateDetails)
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
            if (string.IsNullOrEmpty(txtCityCode.Text))
            {
                txtCityCode.SetValidation("Please Enter Code");
                txtCityCode.RaiseValidationError();
                txtCityCode.Focus();
                retval = false;
            }
            else
                txtCityCode.ClearValidationError();
            if (string.IsNullOrEmpty(txtDescription.Text))
            {
                txtDescription.SetValidation("Please Enter State Name");
                txtDescription.RaiseValidationError();
                if (retval == true)
                    txtDescription.Focus();
                retval = false;
            }
            else
                txtDescription.ClearValidationError();
            
            return retval;
        }

        public bool Validation1()
        {
            bool retval = true;
            if (cmbCountry.SelectedItem == null)
            {
                cmbCountry.TextBox.SetValidation("Please Select Country");
                cmbCountry.TextBox.RaiseValidationError();
                if (retval == true)
                    cmbCountry.Focus();
                retval = false;

            }
            else if (((MasterListItem)cmbCountry.SelectedItem).ID == 0)
            {
                cmbCountry.TextBox.SetValidation("Please Select Country");
                cmbCountry.TextBox.RaiseValidationError();
                if (retval == true)
                    cmbCountry.Focus();
                retval = false;

            }
            else
            {
                cmbCountry.TextBox.ClearValidationError();
                retval = true;
            }
            return retval;
        }

        #endregion
        public void ClearUI()
        {
            txtDescription.Text = "";
            txtCityCode.Text = "";
            if (cmbCountry.ItemsSource != null)
            {
                MasterListItem Defaultc = ((List<MasterListItem>)cmbCountry.ItemsSource).FirstOrDefault(s => s.ID == 0);
                cmbCountry.SelectedItem = Defaultc;
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
                    txtCityCode.RaiseValidationError();              
                    txtCityCode.Focus();
                    txtDescription.RaiseValidationError();
                    txtDescription.Focus();
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
            SetCommandButtonState("New");
            grdBackPanel.DataContext = new clsBankBranchVO();
            Validation();
            ClearUI();
          
           
            try
            {
                objAnimation.Invoke(RotationType.Forward);
                txtCityCode.Focus();
                txtDescription.Focus();

            }
            catch (Exception)
            {

                throw;
            }

        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation() && Validation1())
            {
                string msgTitle = "";
                string msgText = "ARE YOU SURE YOU WANT TO SAVE THE RECORD ?";

                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);

                msgWin.Show();
            }
        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (Validation() && Validation1())
            {
                string msgTitle = "";
                string msgText = "ARE YOU SURE YOU  WANT TO UPDATE THE RECORD ?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();

            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            txtCityCode.ClearValidationError();
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
                    clsAddUpadateStateBizActionVO BizActionVO = new clsAddUpadateStateBizActionVO();
                    clsStateVO objStatDet = new clsStateVO();

                    objStatDet.Id = 0;
                    objStatDet.Description = txtDescription.Text.Trim();
                    objStatDet.Code = txtCityCode.Text.Trim();
                    objStatDet.CountryId = ((MasterListItem)cmbCountry.SelectedItem).ID;
                    objStatDet.Status = true;

                    objStatDet.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    objStatDet.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    objStatDet.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    objStatDet.AddedDateTime = System.DateTime.Now;
                    objStatDet.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    BizActionVO.ObjState = objStatDet;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpadateStateBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                objProgress.Close();
                                msgText = "RECORD SAVED SUCCESSFULLY";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                //After Insertion Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                BankBranchId = 0;
                                SetupPage();
                                SetCommandButtonState("Save");
                            }
                            else if (((clsAddUpadateStateBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                objProgress.Close();
                                msgText = "Record cannot be save because CODE already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpadateStateBizActionVO)args.Result).SuccessStatus == 3)
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
                    clsAddUpadateStateBizActionVO BizActionVO = new clsAddUpadateStateBizActionVO();
                    clsStateVO objStatDet = new clsStateVO();

                    objStatDet.Id = Convert.ToInt64(txtCityCode.Tag);
                    objStatDet.Description = txtDescription.Text.Trim();
                    objStatDet.Code = txtCityCode.Text.Trim();
                    objStatDet.CountryId = ((MasterListItem)cmbCountry.SelectedItem).ID;
                    objStatDet.Status = true;

                    objStatDet.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    objStatDet.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    objStatDet.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    objStatDet.AddedDateTime = System.DateTime.Now;
                    objStatDet.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    BizActionVO.ObjState = objStatDet;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpadateStateBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                objProgress.Close();
                                msgText = "RECORD UPDATED SUCCESSFULLY.";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                //After Insertion Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                BankBranchId = 0;
                                SetupPage();
                                SetCommandButtonState("Save");
                            }
                            else if (((clsAddUpadateStateBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                objProgress.Close();
                                msgText = "Record cannot be save because CODE already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpadateStateBizActionVO)args.Result).SuccessStatus == 3)
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
            MasterList = new PagedSortableCollectionView<clsStateVO>();
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
                txtCityCode.Tag = Convert.ToInt64(((clsStateVO)grdCityDetails.SelectedItem).Id);
                txtCityCode.Text = Convert.ToString(((clsStateVO)grdCityDetails.SelectedItem).Code);
                txtDescription.Text = Convert.ToString(((clsStateVO)grdCityDetails.SelectedItem).Description);
                cmbCountry.SelectedValue = Convert.ToInt64(((clsStateVO)grdCityDetails.SelectedItem).CountryId);
                cmdModify.IsEnabled = ((clsStateVO)grdCityDetails.SelectedItem).Status;
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
                    objProgress.Show();
                    clsAddUpadateStateBizActionVO BizActionVO = new clsAddUpadateStateBizActionVO();
                    clsStateVO objStatDet = new clsStateVO();

                    objStatDet.Id = Convert.ToInt64(((clsStateVO)grdCityDetails.SelectedItem).Id);
                    objStatDet.Description = ((clsStateVO)grdCityDetails.SelectedItem).Description;
                    objStatDet.Code = ((clsStateVO)grdCityDetails.SelectedItem).Code;
                    objStatDet.CountryId = ((clsStateVO)grdCityDetails.SelectedItem).CountryId;
                    objStatDet.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
                    objStatDet.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    objStatDet.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    objStatDet.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    objStatDet.AddedDateTime = System.DateTime.Now;
                    objStatDet.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    BizActionVO.ObjState = objStatDet;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpadateStateBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                objProgress.Close();
                                msgText = "STATUS UPDATED SUCCESSFULLY !";
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

        private void txtCityCode_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtCityCode.Text.IsItUpperCase() == false)
                txtCityCode.Text = txtCityCode.Text.ToTitleCase();
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
            }
        }
    }
}
