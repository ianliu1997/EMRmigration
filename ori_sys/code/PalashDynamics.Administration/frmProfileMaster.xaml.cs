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
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Administration.PatientSourceMaster;
using System.Reflection;
using System.IO;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Browser;
using PalashDynamics.Collections;
using System.ComponentModel;

using PalashDynamics.UserControls;


namespace PalashDynamics.Administration
{
    public partial class frmProfileMaster : UserControl
    {
       
        #region VariableDeclaration
      
        public clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        WaitIndicator objProgress = new WaitIndicator();
        bool IsViewRec = false;
        public PagedSortableCollectionView<clsPreffixMasterVO> MasterList { get; private set; }
        private SwivelAnimation objAnimation;
        bool isnew = false;
        string msgText = "";
        string msgTitle = "PALASH";
        public bool j;
        bool IsCancel = true;
        #endregion

        #region From Constructor
        public frmProfileMaster()
        {
            InitializeComponent();
            FillApplcableGender();
            MasterList = new PagedSortableCollectionView<clsPreffixMasterVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.dgPreffixMaster.DataContext = MasterList;
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

        }
        #endregion

        #region Page Refresh
        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            SetupPage();
        }
        #endregion

        #region Set Button State
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Save":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;                    
                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Fill Gender List Cmb
        public void FillApplcableGender()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_GenderMaster;
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

                    objList.Add(new MasterListItem(0, "Applicable To Both"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);            
                    cmbApplicableGenderBackpnl.ItemsSource = null;
                    cmbApplicableGenderBackpnl.ItemsSource = objList;
                    cmbApplicableGenderBackpnl.SelectedItem = objList[0];

                }

                if (this.DataContext != null)
                {
                    //cmbapplicableGender.SelectedValue = ((clsPreffixMasterVO)this.DataContext).ID;
                }


            };

            client.ProcessAsync(BizAction, User); // new clsUserVO());
            client.CloseAsync();
        }
        #endregion

        #region ClearUI  Method
        public void ClearUI()
        {
            txtCode.Text = string.Empty;
            txtdescription.Text = string.Empty;
            txtCodeBackpnl.Text = string.Empty;
            txtDescriptionBackpnl.Text = string.Empty;
            //cmbapplicableGender.SelectedValue = ((List<MasterListItem>)cmbapplicableGender.ItemsSource)[0].ID;
           //.Text = string.Empty;
            //txtDescription.Text = string.Empty;
            //CmbCategory.SelectedValue = ((List<MasterListItem>)CmbCategory.ItemsSource)[0].ID;
            if (cmbApplicableGenderBackpnl.ItemsSource != null)
                cmbApplicableGenderBackpnl.SelectedValue = ((List<MasterListItem>)cmbApplicableGenderBackpnl.ItemsSource)[0].ID;
            //if (cmbapplicableGender.ItemsSource != null)
            //    cmbapplicableGender.SelectedValue = ((List<MasterListItem>)cmbapplicableGender.ItemsSource)[0].ID;
        }
        #endregion

        #region Button Click Event

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            isnew = false;
            if (dgPreffixMaster.SelectedItem != null)
            {
                objProgress.Show();
                ClearUI();
                SetCommandButtonState("View");
                IsViewRec = true;
                isnew = false;                
                txtCodeBackpnl.Text = Convert.ToString(((clsPreffixMasterVO)dgPreffixMaster.SelectedItem).Code);
                txtDescriptionBackpnl.Text = Convert.ToString(((clsPreffixMasterVO)dgPreffixMaster.SelectedItem).Description);
                cmbApplicableGenderBackpnl.SelectedValue = Convert.ToInt64(((clsPreffixMasterVO)dgPreffixMaster.SelectedItem).ApplicableGender);
                cmdModify.IsEnabled = ((clsPreffixMasterVO)dgPreffixMaster.SelectedItem).Status;
                j = Convert.ToBoolean(((clsPreffixMasterVO)dgPreffixMaster.SelectedItem).Status);                
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

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("New");
            Validation();
            this.DataContext = new clsPreffixMasterVO();
            isnew = true;
            ClearUI();
            IsCancel = false;
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
                if (CheckDuplicasynew())
                {
                    SetCommandButtonState("Save");
                    string msgTitle = "Palash";
                    msgText = "Are you sure you want to Save ?";
                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosedSavePriffixMaster);
                    msgW.Show();
                }
            }
        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            
            if (Validation())
            {
                if (CheckDuplicasyModify())
                {
                    SetCommandButtonState("Modify");
                    string msgTitle = "";
                    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    //{
                    //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("UpdateVerification_Msg");
                    //}
                    //else
                    //{
                    msgText = "Are you sure you want to Update ?";
                    //}
                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosedModifyPreffixMaster);

                    msgW.Show();
                }
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            this.DataContext = null;
            SetupPage();
            isnew = false;
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            dgPreffixMaster.Visibility = Visibility.Visible;
            mElement.Text = "";
            objAnimation.Invoke(RotationType.Backward);
            if (IsCancel == true)
            {
                mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Patient Configuration";
                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmPatientConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            else
            {
                IsCancel = true;
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {            
                SetupPage();
                dataGrid2Pager.PageIndex = 0;
                //MasterList = new PagedSortableCollectionView<clsPreffixMasterVO>();
                //PageSize = 15;
                //this.dataGrid2Pager.DataContext = MasterList;
                //this.dgPreffixMaster.DataContext = MasterList;
            
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (dgPreffixMaster.SelectedItem != null)
            {
                try
                {
                    objProgress.Show();
                    clsAddUpdatePreffixMasterBizActionVO BizActionVO = new clsAddUpdatePreffixMasterBizActionVO();
                    clsPreffixMasterVO obj = new clsPreffixMasterVO();
                    obj.ID = Convert.ToInt64(((clsPreffixMasterVO)dgPreffixMaster.SelectedItem).ID);
                    obj.UnitID = Convert.ToInt64(((clsPreffixMasterVO)dgPreffixMaster.SelectedItem).UnitID);
                    obj.Code = (((clsPreffixMasterVO)dgPreffixMaster.SelectedItem).Code);
                    obj.Description = (((clsPreffixMasterVO)dgPreffixMaster.SelectedItem).Description);
                    obj.ApplicableGender = (((clsPreffixMasterVO)dgPreffixMaster.SelectedItem).ApplicableGender);
                    obj.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
                    BizActionVO.PreffixMasterDetails.Add(obj);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpdatePreffixMasterBizActionVO)args.Result).SuccessStatus == 1)
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
        #endregion

        #region Validation
        private bool Validation()
        {
            if (string.IsNullOrEmpty(txtDescriptionBackpnl.Text))//txtCodeBackpnl
            {


                msgText = "Please enter description.";
                txtDescriptionBackpnl.SetValidation(msgText);
                txtDescriptionBackpnl.RaiseValidationError();
                txtDescriptionBackpnl.Focus();
                if (string.IsNullOrWhiteSpace(txtCodeBackpnl.Text))//txtDescriptionBackpnl
                {
                    msgText = "Please enter code.";
                    txtCodeBackpnl.SetValidation(msgText);
                    txtCodeBackpnl.RaiseValidationError();
                    txtCodeBackpnl.Focus();
                    return false;
                }
                return false;

            }
            else if (string.IsNullOrWhiteSpace(txtDescriptionBackpnl.Text))
            {               
                msgText = "Please enter description.";
                txtDescriptionBackpnl.SetValidation(msgText);
                txtDescriptionBackpnl.RaiseValidationError();
                txtDescriptionBackpnl.Focus();
                return false;
            }            
            else
            {
                txtCode.ClearValidationError();
                txtdescription.ClearValidationError();
                return true;
            }
        }

        //private bool ValidationModify()
        //{
        //    if (string.IsNullOrEmpty(txtCode.Text))
        //    {
        //        //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
        //        //{
        //        //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("CodeValidation_Msg");
        //        //}
        //        //else
        //        //{
        //        msgText = "Please enter code.";
        //        //}
        //        txtCode.SetValidation(msgText);
        //        txtCode.RaiseValidationError();
        //        txtCode.Focus();
        //        return false;
        //    }
        //    else if (string.IsNullOrWhiteSpace(txtdescription.Text))
        //    {

        //        msgText = "Please enter description.";

        //        txtdescription.SetValidation(msgText);
        //        txtdescription.RaiseValidationError();
        //        txtdescription.Focus();
        //        return false;
        //    }
        //    //else if (!string.IsNullOrEmpty(txtDescription.Text))
        //    //{
        //    else if (((MasterListItem)cmbapplicableGender.SelectedItem) == null)
        //    {
        //        //if (((MasterListItem)CmbCategory.SelectedItem).ID == 0)
        //        //{
        //        //msgText = "Please select Category.";
        //        //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
        //        //{
        //        //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("CategoryValidation_Msg");
        //        //}
        //        //else
        //        //{
        //        msgText = "Please select category.";
        //        //}
        //        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //        msgWindow.Show();
        //        cmbapplicableGender.Focus();
        //        return false;
        //    }

        //        //}
        //    //}

        //    //else if (!string.IsNullOrEmpty(txtDescription.Text))
        //    //{
        //    else if (((MasterListItem)cmbapplicableGender.SelectedItem) == null)
        //    {
        //        //if (((MasterListItem)CmbSubCategory.SelectedItem).ID == 0)
        //        //{
        //        // msgText = "Please select SubCategory.";
        //        //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
        //        //{
        //        //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("SubCategoryValidation_Msg");
        //        //}
        //        //else
        //        //{
        //        // msgText = "Please select sub category.";
        //        //}
        //        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //        msgWindow.Show();
        //        cmbApplicableGenderBackpnl.Focus();
        //        return false;
        //    }

        //        //}
        //    //}
        //    else
        //    {
        //        txtCode.ClearValidationError();
        //        txtdescription.ClearValidationError();
        //        return true;
        //    }


        //}

        private bool ValidationSearch()
        {
            if (string.IsNullOrEmpty(txtCode.Text) && string.IsNullOrEmpty(txtdescription.Text))
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Insert Code OR Description", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                return false;
            }
            else 
            { 
                return true; 
            }
        }

        #endregion

        #region SaveMethod Message
        void msgW_OnMessageBoxClosedSavePriffixMaster(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                SavePriffixMaster();
            }
            else
            {
                SetCommandButtonState("New");
            }
        }
        #endregion

        #region Save preffixMaster Method
        private void SavePriffixMaster()
        {
            clsAddUpdatePreffixMasterBizActionVO BizAction = new clsAddUpdatePreffixMasterBizActionVO();
            clsPreffixMasterVO addpriffixmasterVO = new clsPreffixMasterVO();
            BizAction.PreffixMasterDetails = new List<clsPreffixMasterVO>();
            addpriffixmasterVO.ID = 0;
            addpriffixmasterVO.Code = txtCodeBackpnl.Text.Trim();
            addpriffixmasterVO.Description = txtDescriptionBackpnl.Text.Trim();
            addpriffixmasterVO.ApplicableGender = ((MasterListItem)cmbApplicableGenderBackpnl.SelectedItem).ID;
            addpriffixmasterVO.Status = true;
            BizAction.PreffixMasterDetails.Add(addpriffixmasterVO);
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
              
                    if (arg.Error == null)
                    {
                        SetupPage();
                        ClearUI();
                        objAnimation.Invoke(RotationType.Backward);
                        if (BizAction.SuccessStatus == 0)
                        {
                            msgText = "Record saved successfully.";
                            isnew = false;
                        }
                        else
                            msgText = "Record already exist.";
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        SetCommandButtonCheckList("Save");
                    }
                    else
                    {
                        msgText = "Error occured while processing.";
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
               
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        #endregion

        #region Get Preffix Master

        private void SetupPage()
        {
            clsGetPreffixMasterBizActionVO BizAction = new clsGetPreffixMasterBizActionVO();           
            if (txtCode.Text != string.Empty)
                BizAction.Code = txtCode.Text;
            if (txtdescription.Text != string.Empty)
                BizAction.Description = txtdescription.Text;
            BizAction.IsPagingEnabled = true;
            BizAction.MaximumRows = MasterList.PageSize;
            BizAction.StartIndex = MasterList.PageIndex * MasterList.PageSize;            
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {

                    if (args.Error == null && args.Result != null)
                    {
                        BizAction.PreffixMasterDetails = (((clsGetPreffixMasterBizActionVO)args.Result).PreffixMasterDetails);
                        MasterList.Clear();
                        MasterList.TotalItemCount = (int)(((clsGetPreffixMasterBizActionVO)args.Result).TotalRows);
                        foreach (clsPreffixMasterVO item in BizAction.PreffixMasterDetails)
                        {
                            MasterList.Add(item);
                        }
                    }

                };
                client.ProcessAsync(BizAction, User);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                
            }
        }

        #endregion

        #region ModifyMethod Message
        void msgW_OnMessageBoxClosedModifyPreffixMaster(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    ModifyPreffixMaster();
                    SetCommandButtonCheckList("Modify");
                }

                catch (Exception ex)
                {
                    throw;
                }
            }
            else 
            {
                SetCommandButtonCheckList("View");
            }
        }
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

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonCheckList(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    break;

                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;
                case "Save":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;
                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;
                default:
                    break;
            }
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
        
        #region Modify Preffix Master
        private void ModifyPreffixMaster()
        {
            clsAddUpdatePreffixMasterBizActionVO BizAction = new clsAddUpdatePreffixMasterBizActionVO();
            clsPreffixMasterVO UpdtpreffixtMaster = new clsPreffixMasterVO();
            BizAction.PreffixMasterDetails = new List<clsPreffixMasterVO>();
            try
            {                
                UpdtpreffixtMaster.ID = ((clsPreffixMasterVO)dgPreffixMaster.SelectedItem).ID;
                UpdtpreffixtMaster.Code = txtCodeBackpnl.Text;
                UpdtpreffixtMaster.Description = txtDescriptionBackpnl.Text;
                UpdtpreffixtMaster.ApplicableGender = ((MasterListItem)cmbApplicableGenderBackpnl.SelectedItem).ID;
                UpdtpreffixtMaster.Status = j;
                Int64 i = ((MasterListItem)cmbApplicableGenderBackpnl.SelectedItem).ID;
                BizAction.PreffixMasterDetails.Add(UpdtpreffixtMaster);
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        ClearUI();
                        objAnimation.Invoke(RotationType.Backward);                       
                        msgText = "Record updated successfully.";                       
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        SetCommandButtonCheckList("Modify");
                        SetupPage();
                    }
                    else
                    {                       
                        msgText = "Error occured while processing.";                    
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Form Load
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Load");
            SetupPage();

        }
        #endregion

        #region Check Duplicasy Record

        private bool CheckDuplicasynew()
        {
            clsPreffixMasterVO PreffixItem;
            clsPreffixMasterVO PreffixItem1;
            if (isnew)
            {
                PreffixItem = MasterList.FirstOrDefault(p => p.Code.ToUpper().Equals(txtCodeBackpnl.Text.ToUpper()));
                PreffixItem1 = MasterList.FirstOrDefault(p => p.Description.ToUpper().Equals(txtDescriptionBackpnl.Text.ToUpper()));

            }
            else
            {
                PreffixItem = MasterList.FirstOrDefault(p => p.Code.ToUpper().Equals(txtCodeBackpnl.Text.ToUpper()) && p.ID != ((clsPreffixMasterVO)dgPreffixMaster.SelectedItem).ID);
                PreffixItem1 = MasterList.FirstOrDefault(p => p.Description.ToUpper().Equals(txtDescriptionBackpnl.Text.ToUpper()) && p.ID != ((clsPreffixMasterVO)dgPreffixMaster.SelectedItem).ID);
            }
            if (PreffixItem != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because CODE already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                SetCommandButtonState("New");
                return false;
            }
            else if (PreffixItem1 != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because Description already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                SetCommandButtonState("New");
                return false;
                
            }
            else
            {
                return true;
            }
        }

        private bool CheckDuplicasyModify()
        {
            clsPreffixMasterVO PreffixItem;
            clsPreffixMasterVO PreffixItem1;
            if (isnew)
            {
                PreffixItem = MasterList.FirstOrDefault(p => p.Code.ToUpper().Equals(txtCodeBackpnl.Text.ToUpper()));
                PreffixItem1 = MasterList.FirstOrDefault(p => p.Description.ToUpper().Equals(txtDescriptionBackpnl.Text.ToUpper()));
            }
            else
            {
                PreffixItem = MasterList.FirstOrDefault(p => p.Code.ToUpper().Equals(txtCodeBackpnl.Text.ToUpper()) && p.ID != ((clsPreffixMasterVO)dgPreffixMaster.SelectedItem).ID);
                PreffixItem1 = MasterList.FirstOrDefault(p => p.Description.ToUpper().Equals(txtDescriptionBackpnl.Text.ToUpper()) && p.ID != ((clsPreffixMasterVO)dgPreffixMaster.SelectedItem).ID);
            }
            if (PreffixItem != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because CODE already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                SetCommandButtonState("View");
                return false;
            }
            else if (PreffixItem1 != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because Description already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                SetCommandButtonState("View");
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region TextBox Key Down Event
        private void cmdtext_keyDown(object sender, KeyEventArgs e)
        {
            SetupPage();
        }
        #endregion
    }
}

