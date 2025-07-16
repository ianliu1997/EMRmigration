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
    public partial class FrmSurrogateAgencyMaster : UserControl
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
        public FrmSurrogateAgencyMaster()
        {
            InitializeComponent();
            objProgress.Show();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            SetCommandButtonState("Load");
            FillReferralName();
            MasterList = new PagedSortableCollectionView<clsSurrogateAgencyMasterVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdAgencyDetails.DataContext = MasterList;
            SetupPage();
        }

        private void FillReferralName()
        {
            
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_ReferralTypeMaster;
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

                    cmbReferralName.ItemsSource = null;
                    cmbReferralName.ItemsSource = objList;
                    cmbReferralName.SelectedItem = objList[0];
                    

                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        void SetupPage() 
        {

            clsGetSurrogactAgencyMasterBizActionVO BizAction = new clsGetSurrogactAgencyMasterBizActionVO();
            BizAction.AgencyDetailsList = new List<clsSurrogateAgencyMasterVO>();
            BizAction.AgencyDetails = new clsSurrogateAgencyMasterVO();
            if (txtSearch.Text != null)
                BizAction.AgencyDetails.Description = txtSearch.Text;
   
            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = MasterList.PageIndex * MasterList.PageSize;
            BizAction.MaximumRows = MasterList.PageSize;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //dgDoctorList.ItemsSource = null;
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result !=null)
                {
                    if (((clsGetSurrogactAgencyMasterBizActionVO)arg.Result).AgencyDetailsList != null)
                    {
                       
                        clsGetSurrogactAgencyMasterBizActionVO result = arg.Result as clsGetSurrogactAgencyMasterBizActionVO;

                        if (result.AgencyDetailsList != null)
                        {
                            MasterList.Clear();
                            MasterList.TotalItemCount = ((clsGetSurrogactAgencyMasterBizActionVO)arg.Result).TotalRows;
                            foreach (clsSurrogateAgencyMasterVO item in result.AgencyDetailsList)
                            {
                                MasterList.Add(item);

                            }


                        }
                     //   grdAgencyDetails.ItemSource = null;
                       // grdAgencyDetails.ItemSource = MasterList;

           

                    }
                }

                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }
              

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

            objProgress.Close();

        }
        private void DateYear_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            if (Affilatedyear != null)
            {
                Affilatedyear.DisplayMode = CalendarMode.Decade;

            }
        }
        bool isnew = false;
        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("New");
            isnew = true;
            grdBackPanel.DataContext = new clsSurrogateAgencyMasterVO();
            Affilatedyear.DisplayDate = DateTime.Now;
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
            txtDescription.Text = string.Empty;
            txtAffilatedBy.Text = string.Empty;
            Affilatedyear.DisplayDate = DateTime.Now;
            txtSourceContactNo.Text = string.Empty;
            txtSourceEmailID.Text = string.Empty;
            txtSourceAddress.Text = string.Empty;
            cmbReferralName.SelectedValue = (long)0;
            txtRegistrationNo.Text = string.Empty;
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
                txtDescription.SetValidation("Please Enter Description/Agency Name");
                txtDescription.RaiseValidationError();
                if (retVal == true)
                    txtDescription.Focus();
                retVal = false;
            }
            else
                txtDescription.ClearValidationError();
            if (cmbReferralName.SelectedItem == null)
            {
                cmbReferralName.TextBox.SetValidation("Please Select Referral Type");
                cmbReferralName.TextBox.RaiseValidationError();
                if (retVal == true)
                    cmbReferralName.Focus();
                retVal = false;

            }
            else if (((MasterListItem)cmbReferralName.SelectedItem).ID == 0)
            {

                cmbReferralName.TextBox.SetValidation("Please Select Referral Type");
                cmbReferralName.TextBox.RaiseValidationError();
                if (retVal == true)
                    cmbReferralName.Focus();
                retVal = false;

            }
            else
                cmbReferralName.TextBox.ClearValidationError();
            
            return retVal;


        }

        #endregion
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                string msgTitle = "";
                string msgText = "ARE YOU SURE YOU WANT TO SAVE THE RECORD ?";

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
                        clsAddUpdateSurrogactAgencyMasterBizActionVO BizActionVO = new clsAddUpdateSurrogactAgencyMasterBizActionVO();
                        BizActionVO.AgencyDetails = (clsSurrogateAgencyMasterVO)grdBackPanel.DataContext;



                        BizActionVO.AgencyDetails.Description = txtDescription.Text.Trim();
                        BizActionVO.AgencyDetails.Code = txtCode.Text.Trim();
                        if (txtRegistrationNo.Text != string.Empty)
                            BizActionVO.AgencyDetails.RegistrationNo = txtRegistrationNo.Text;
                        if (txtSourceEmailID.Text != string.Empty)
                            BizActionVO.AgencyDetails.SourceEmail = txtSourceEmailID.Text;
                        if (txtSourceContactNo.Text != string.Empty)
                            BizActionVO.AgencyDetails.SourceContactNo = txtSourceContactNo.Text;
                        if (txtAffilatedBy.Text != string.Empty)
                            BizActionVO.AgencyDetails.AffilatedBy = txtAffilatedBy.Text;
                        if (txtSourceAddress.Text != string.Empty)
                            BizActionVO.AgencyDetails.SourceAddress = txtSourceAddress.Text;
                        if (Affilatedyear.DisplayDate != null)
                            BizActionVO.AgencyDetails.Affilatedyear = Affilatedyear.DisplayDate.Date.Date;
                        if (cmbReferralName.SelectedItem != null)
                            BizActionVO.AgencyDetails.ReferralTypeID = ((MasterListItem)cmbReferralName.SelectedItem).ID;
                        if (((clsSurrogateAgencyMasterVO)grdBackPanel.DataContext).ID != 0)
                            BizActionVO.AgencyDetails.Status = ((clsSurrogateAgencyMasterVO)grdAgencyDetails.SelectedItem).Status;
                        else
                            BizActionVO.AgencyDetails.Status = true;



                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, args) =>
                        {

                            if (args.Error == null && args.Result != null)
                            {
                                if (((clsAddUpdateSurrogactAgencyMasterBizActionVO)args.Result).SuccessStatus == 1)
                                {
                                    objProgress.Close();
                                    msgText = "RECORD SAVED SUCCESSFULLY";

                                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgWindow.Show();
                                    //After Insertion Back to BackPanel and Setup Page
                                    objAnimation.Invoke(RotationType.Backward);
                                    ClearUI();
                                    SetupPage();
                                    SetCommandButtonState("Save");
                                    isnew = false;
                                }
                                else if (((clsAddUpdateSurrogactAgencyMasterBizActionVO)args.Result).SuccessStatus == 2)
                                {
                                    objProgress.Close();
                                    msgText = "RECORD UPDATED SUCCESSFULLY";
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
                string msgText = "ARE YOU SURE YOU WANT TO UPDATE THE RECORD ?";

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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (grdAgencyDetails.SelectedItem != null)
            {
                try
                {
                    objProgress.Show();
                    clsUpdateStatusSurrogactAgencyMasterBizActionVO BizActionVO = new clsUpdateStatusSurrogactAgencyMasterBizActionVO();
                    clsSurrogateAgencyMasterVO obj = new clsSurrogateAgencyMasterVO();
                    obj.ID = Convert.ToInt64(((clsSurrogateAgencyMasterVO)grdAgencyDetails.SelectedItem).ID);
                    obj.UnitID = Convert.ToInt64(((clsSurrogateAgencyMasterVO)grdAgencyDetails.SelectedItem).UnitID);
                    obj.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
                    BizActionVO.AgencyDetailsList.Add(obj);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsUpdateStatusSurrogactAgencyMasterBizActionVO)args.Result).SuccessStatus == 2)
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
       
        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            if (grdAgencyDetails.SelectedItem != null)
            {
                objProgress.Show();
                ClearUI();
                SetCommandButtonState("View");
                IsViewRec = true;
                isnew = false;
                //rohinee 12/10/15 for refreshing data
                grdBackPanel.DataContext = ((clsSurrogateAgencyMasterVO)grdAgencyDetails.SelectedItem).DeepCopy();
                //
                txtCode.Text = Convert.ToString(((clsSurrogateAgencyMasterVO)grdAgencyDetails.SelectedItem).Code);
                txtDescription.Text = Convert.ToString(((clsSurrogateAgencyMasterVO)grdAgencyDetails.SelectedItem).Description);
                cmbReferralName.SelectedValue = Convert.ToInt64(((clsSurrogateAgencyMasterVO)grdAgencyDetails.SelectedItem).ReferralTypeID);
                txtRegistrationNo.Text = Convert.ToString(((clsSurrogateAgencyMasterVO)grdAgencyDetails.SelectedItem).RegistrationNo);
                txtSourceEmailID.Text = Convert.ToString(((clsSurrogateAgencyMasterVO)grdAgencyDetails.SelectedItem).SourceEmail);
                txtSourceContactNo.Text = Convert.ToString(((clsSurrogateAgencyMasterVO)grdAgencyDetails.SelectedItem).SourceContactNo);
                txtAffilatedBy.Text = Convert.ToString(((clsSurrogateAgencyMasterVO)grdAgencyDetails.SelectedItem).AffilatedBy);
                Affilatedyear.DisplayDate = Convert.ToDateTime(((clsSurrogateAgencyMasterVO)grdAgencyDetails.SelectedItem).Affilatedyear);
                txtSourceAddress.Text = Convert.ToString(((clsSurrogateAgencyMasterVO)grdAgencyDetails.SelectedItem).SourceAddress);
                //by rohinee 10/10/2015
                if (((clsSurrogateAgencyMasterVO)grdAgencyDetails.SelectedItem).Status == true)
                {
                    cmdModify.IsEnabled = true;
                }
                else
                {
                    cmdModify.IsEnabled = false;
                }
                //
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

        private void txtCode_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtDescription_LostFocus(object sender, RoutedEventArgs e)
        {

        }
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        private void txtSourceEmailID_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtSourceEmailID.Text.Length > 0)
            {
                if (txtSourceEmailID.Text.IsEmailValid())
                    txtSourceEmailID.ClearValidationError();
                else
                {
                    txtSourceEmailID.SetValidation("Please enter valid Email");
                    txtSourceEmailID.RaiseValidationError();
                }
            }
        }

        private void txtSourceContactNo_TextChanged(object sender, TextChangedEventArgs e)
        {
          
                if (!string.IsNullOrEmpty(((TextBox)sender).Text))
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
                    else if (((TextBox)sender).Text.Length > 10)
                    {
                        ((TextBox)sender).Text = textBefore;
                        ((TextBox)sender).SelectionStart = selectionStart;
                        ((TextBox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                }
            
        }

        private void txtSourceContactNo_KeyDown(object sender, KeyEventArgs e)
        {

            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            SetupPage();
        }
        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
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
            this.grdAgencyDetails.DataContext = MasterList;
            SetupPage();
        }

        private bool CheckDuplicasy()
        {
            clsSurrogateAgencyMasterVO CompanyItem;
            clsSurrogateAgencyMasterVO CompanyItem1;
            if (isnew)
            {
                CompanyItem = MasterList.FirstOrDefault(p => p.Code.ToUpper().Equals(txtCode.Text.ToUpper()));
                CompanyItem1 = MasterList.FirstOrDefault(p => p.Description.ToUpper().Equals(txtDescription.Text.ToUpper()));

            }
            else
            {
                CompanyItem = MasterList.FirstOrDefault(p => p.Code.ToUpper().Equals(txtCode.Text.ToUpper()) && p.ID != ((clsSurrogateAgencyMasterVO)grdAgencyDetails.SelectedItem).ID);
                CompanyItem1 = MasterList.FirstOrDefault(p => p.Description.ToUpper().Equals(txtDescription.Text.ToUpper()) && p.ID != ((clsSurrogateAgencyMasterVO)grdAgencyDetails.SelectedItem).ID);
            }
            if (CompanyItem != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because CODE already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return false;
            }
            else if (CompanyItem1 != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because Description already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return false;
            }
            else
            {
                return true;
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                
                SetupPage();
            }
        }
    }
}
