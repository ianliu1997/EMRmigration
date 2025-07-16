

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
    public partial class frmCountryMaster : UserControl
    {
        #region  Variables

        private SwivelAnimation objAnimation;
        WaitIndicator objProgress = new WaitIndicator();
        public PagedSortableCollectionView<clsCountryVO> MasterList { get; private set; }
        bool IsCancel = true;
        string msgTitle = "PALASHDYNAMICS";
        string msgText = "";
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

        public frmCountryMaster()
        {
            InitializeComponent();
            objProgress.Show();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            SetCommandButtonState("Load");

            MasterList = new PagedSortableCollectionView<clsCountryVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdCountryDetails.DataContext = MasterList;
            SetupPage();
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
                txtDescription.SetValidation("Please Enter Country Name");
                txtDescription.RaiseValidationError();
                if (retval == true)
                    txtDescription.Focus();
                retval = false;
            }
            else
                txtDescription.ClearValidationError();

            if (string.IsNullOrEmpty(txtNationality.Text))
            {
                txtNationality.SetValidation("Please Enter Nationality");
                txtNationality.RaiseValidationError();
                if (retval == true)
                    txtNationality.Focus();
                retval = false;
            }
            else
                txtNationality.ClearValidationError();
            return retval;
        }

        #endregion
        public void ClearUI()
        {
            txtDescription.Text = "";
            txtCityCode.Text = "";
            //MasterListItem Defaultc = ((List<MasterListItem>)cmbCountry.ItemsSource).FirstOrDefault(s => s.ID == 0);
            //cmbCountry.SelectedItem = Defaultc;
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
        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            SetupPage();
        }
        void SetupPage()
        {
            try
            {

                clsGetCountryDetailsBizActionVO bizActionVO = new clsGetCountryDetailsBizActionVO();

                bizActionVO.Description = txtSearch.Text.Trim();
                //bizActionVO.Code = txtCityCode.Text.Trim();
                //if (cmbSearchCountry.SelectedItem != null)
                //    bizActionVO.CountryId = ((MasterListItem)cmbSearchCountry.SelectedItem).ID;

                bizActionVO.SearchExpression = null;
                bizActionVO.PagingEnabled = true;
                bizActionVO.MaximumRows = MasterList.PageSize;
                bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;

                clsStateVO objStateVo = new clsStateVO();
                bizActionVO.ListCountryDetails = new List<clsCountryVO>();


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {

                    if (args.Error == null && args.Result != null)
                    {

                        bizActionVO.ListCountryDetails = (((clsGetCountryDetailsBizActionVO)args.Result).ListCountryDetails);
                        MasterList.Clear();
                        MasterList.TotalItemCount = (int)(((clsGetCountryDetailsBizActionVO)args.Result).TotalRows);
                        ///Setup Page Fill DataGrid
                        if (bizActionVO.ListCountryDetails.Count > 0)
                        {
                            foreach (clsCountryVO item in bizActionVO.ListCountryDetails)
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
        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("New");
            grdBackPanel.DataContext = new clsBankBranchVO();
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
                string msgTitle = "";
                string msgText = "Are you sure you want to Save ?";

                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);

                msgWin.Show();
            }

        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Update ?";

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
                    clsAddUpadateCountryBizActionVO BizActionVO = new clsAddUpadateCountryBizActionVO();
                    clsCountryVO objCountryDet = new clsCountryVO();

                    objCountryDet.Id = 0;
                    objCountryDet.Description = txtDescription.Text.Trim();
                    objCountryDet.Code = txtCityCode.Text.Trim();
                    objCountryDet.Nationality = txtNationality.Text;
                    // objStatDet.CountryId = ((MasterListItem)cmbCountry.SelectedItem).ID;
                    objCountryDet.Status = true;

                    objCountryDet.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    objCountryDet.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    objCountryDet.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    objCountryDet.AddedDateTime = System.DateTime.Now;
                    objCountryDet.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    BizActionVO.ObjCountry = objCountryDet;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpadateCountryBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                objProgress.Close();
                                msgText = "RECORD SAVED SUCCESSFULLY.";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                //After Insertion Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                SetupPage();
                                SetCommandButtonState("Save");
                            }
                            else if (((clsAddUpadateCountryBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                objProgress.Close();
                                msgText = "Record cannot be save because CODE already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpadateCountryBizActionVO)args.Result).SuccessStatus == 3)
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
                    clsAddUpadateCountryBizActionVO BizActionVO = new clsAddUpadateCountryBizActionVO();
                    clsCountryVO objCountryDet = new clsCountryVO();

                    objCountryDet.Id = Convert.ToInt64(txtCityCode.Tag);
                    objCountryDet.Description = txtDescription.Text.Trim();
                    objCountryDet.Code = txtCityCode.Text.Trim();
                    objCountryDet.Nationality = txtNationality.Text;
                    objCountryDet.Status = true;

                    objCountryDet.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    objCountryDet.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    objCountryDet.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    objCountryDet.AddedDateTime = System.DateTime.Now;
                    objCountryDet.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    BizActionVO.ObjCountry = objCountryDet;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpadateCountryBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                objProgress.Close();
                                msgText = "RECORD UPDATED SUCCESSFULLY.";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                //After Insertion Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                SetupPage();
                                SetCommandButtonState("Save");
                            }
                            else if (((clsAddUpadateCountryBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                objProgress.Close();
                                msgText = "Record cannot be save because CODE already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpadateCountryBizActionVO)args.Result).SuccessStatus == 3)
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
                    msgText = "Record cannot be save , Error Occured!";
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

        private void txtNationality_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtNationality.Text.IsItUpperCase() != true)
                txtNationality.Text = txtNationality.Text.ToTitleCase();
        }

        private void txtCityCode_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtCityCode.Text.IsItUpperCase() != true)
                txtCityCode.Text = txtCityCode.Text.ToTitleCase();
        }

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {

            if (grdCountryDetails.SelectedItem != null)
            {
                objProgress.Show();
                ClearUI();
                SetCommandButtonState("View");
                txtCityCode.Tag = Convert.ToInt64(((clsCountryVO)grdCountryDetails.SelectedItem).Id);
                txtCityCode.Text = Convert.ToString(((clsCountryVO)grdCountryDetails.SelectedItem).Code);
                txtDescription.Text = Convert.ToString(((clsCountryVO)grdCountryDetails.SelectedItem).Description);
                txtNationality.Text = Convert.ToString(((clsCountryVO)grdCountryDetails.SelectedItem).Nationality);
                cmdModify.IsEnabled = ((clsCountryVO)grdCountryDetails.SelectedItem).Status;
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
            if (grdCountryDetails.SelectedItem != null)
            {
                try
                {
                    objProgress.Show();
                    clsAddUpadateCountryBizActionVO BizActionVO = new clsAddUpadateCountryBizActionVO();
                    clsCountryVO objStatDet = new clsCountryVO();

                    objStatDet.Id = Convert.ToInt64(((clsCountryVO)grdCountryDetails.SelectedItem).Id);
                    objStatDet.Description = ((clsCountryVO)grdCountryDetails.SelectedItem).Description;
                    objStatDet.Code = ((clsCountryVO)grdCountryDetails.SelectedItem).Code;
                    objStatDet.Nationality = ((clsCountryVO)grdCountryDetails.SelectedItem).Nationality;
                    objStatDet.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
                    objStatDet.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    objStatDet.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    objStatDet.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    objStatDet.AddedDateTime = System.DateTime.Now;
                    objStatDet.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    BizActionVO.ObjCountry = objStatDet;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpadateCountryBizActionVO)args.Result).SuccessStatus == 1)
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
                            msgText = "Record cannot be Updated ,Error occured.";
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
                    msgText = "Record cannot be Updated ,Error occured.";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWindow.Show();
                }
            }
        }

        private void CheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            CheckBox_Checked(sender, e);
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            objProgress.Show();
            SetupPage();
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
