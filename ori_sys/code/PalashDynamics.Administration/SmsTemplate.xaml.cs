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
using PalashDynamics.ValueObjects;
using PalashDynamics.UserControls;
using PalashDynamics.Animations;
using CIMS;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Data;
using System.IO;
using PalashDynamics.Collections;
using System.Collections.ObjectModel;
using System.Reflection;


namespace PalashDynamics.Administration
{
    public partial class SmsTemplate : UserControl
    {
        #region Variable Declaration

        PalashDynamics.Animations.SwivelAnimation _flip = null;
        clsUserVO user = null;
        WaitIndicator waiting = new WaitIndicator();
        bool ISPageLoaded = false;
        bool isFormValid = true;
        public string msgText = "";
        public string msgTitle = "";
        public bool txtCodeValid = true;
        public bool txtNameValid = true;
        public bool txtEngValid = true;
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        public string FileName;
        public string FilePath;
        public ObservableCollection<clsSMSTemplateVO> SMSAddedItems { get; set; }
        public bool IsNew = false;
        bool IsCancel = true;
        #endregion

        #region Pagging

        public PagedSortableCollectionView<clsSMSTemplateVO> DataList { get; private set; }

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

        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillSMSTemplateList();
        }

        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        /// 
        #endregion


        public SmsTemplate()
        {
            InitializeComponent();
            _flip = new PalashDynamics.Animations.SwivelAnimation(LayoutRoot1, LayoutRoot2, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsSMSTemplateVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;
            //======================================================
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!ISPageLoaded)
            {
                this.DataContext = null;
                FillSMSTemplateList();
                GetLocalLanguage();
                SetCommandButtonState("Load");
            }
            ISPageLoaded = true;
        }

        private void GetLocalLanguage()
        {
            clsGetAppConfigBizActionVO objGetDetails = new clsGetAppConfigBizActionVO();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsAppConfigVO objApp = new clsAppConfigVO();
                    objApp = ((clsGetAppConfigBizActionVO)ea.Result).AppConfig;
                    FileName = objApp.FileName;
                    FilePath = objApp.FilePath;

                    Stream LocalFont = this.GetType().Assembly.GetManifestResourceStream(FilePath);
                    txtLocal.FontSource = new FontSource(LocalFont);
                    txtLocal.FontFamily = new FontFamily(FileName);
                    txtLocal.FontSize = 20;
                }
            };
            client.ProcessAsync(objGetDetails, User); //new clsUserVO());
            client.CloseAsync();
        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            ClearFormData();
            FormValidation();
            IsNew = true;
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "New SMS Template";

            _flip.Invoke(RotationType.Forward);

            this.SetCommandButtonState("New");
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (dgSMSTemplateList.SelectedItem != null)
            {
                if (((clsSMSTemplateVO)dgSMSTemplateList.SelectedItem).Status == true)
                {
                    try
                    {
                        IsNew = false;

                        ShowSMSTemplateDetails(((clsSMSTemplateVO)dgSMSTemplateList.SelectedItem).ID);


                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                        mElement.Text = " : " + ((clsSMSTemplateVO)dgSMSTemplateList.SelectedItem).Code;

                        _flip.Invoke(RotationType.Forward);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
                else
                {
                    msgText = "Cannot view the Details, The Template is disabled.";

                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
                SetCommandButtonState("View");
            }
            else
            {
                msgText = "Cannot view the Details.";

                MessageBoxControl.MessageBoxChildWindow msgWindow =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            waiting.Show();
            try
            {
                FormValidation();
                if (txtCodeValid == true && txtNameValid == true && txtEngValid == true)
                {
                    waiting.Close();
                    msgText = "Are you sure you want to Save the Record";
                    MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                      new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWindowUpdate.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindowSave_OnMessageBoxClosed);

                    msgWindowUpdate.Show();
                }
                else
                {
                    waiting.Close();
                    if (txtCodeValid == false)
                        txtCode.Focus();
                    if (txtNameValid == false)
                        txtName.Focus();
                    if (txtEngValid == false)
                        txtEnglish.Focus();
                }
            }
            catch (Exception ex)
            {
                waiting.Close();
                //throw;
            }
        }

        private void msgWindowSave_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    if (CheckDuplicasy())
                    {
                        Save();
                        SetCommandButtonState("Save");
                    }
                }
                catch (Exception ex)
                {
                    //   throw;
                }
            }
        }

        void Save()
        {
            try
            {
                clsSMSTemplateVO objTemplate = CreateFormData();
                clsAddSMSTemplateBizActionVO objSMSTemplate = new clsAddSMSTemplateBizActionVO();
                objSMSTemplate.SMSTemplate = objTemplate;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        ClearFormData();
                        FillSMSTemplateList();

                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                        mElement.Text = "";

                        _flip.Invoke(RotationType.Backward);

                        string msgText = "SMS Template Details Added Successfully";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow.Show();
                    }
                    else if (((clsAddSMSTemplateBizActionVO)ea.Result).SuccessStatus == 2)
                    {
                        msgText = "Record cannot be save because CODE already exist!";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                    }
                    else if (((clsAddSMSTemplateBizActionVO)ea.Result).SuccessStatus == 3)
                    {
                        msgText = "Record cannot be save because Template Name already exist!";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                    }
                };
                Client.ProcessAsync(objSMSTemplate, User);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            waiting.Show();
            try
            {
                FormValidation();
                if (txtCodeValid == true && txtNameValid == true && txtEngValid == true)
                {
                    waiting.Close();
                    msgText = "Are you sure you want to Modify the Record";
                    MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                      new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWindowUpdate.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindowModify_OnMessageBoxClosed);

                    msgWindowUpdate.Show();
                }
                else
                {
                    waiting.Close();
                    if (txtCodeValid == false)
                        txtCode.Focus();
                    if (txtNameValid == false)
                        txtName.Focus();
                    if (txtEngValid == false)
                        txtEnglish.Focus();
                }
            }
            catch (Exception ex)
            {
                waiting.Close();
                //throw;
            }
        }

        private void msgWindowModify_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    if (CheckDuplicasy())
                    {
                        Modify();
                        SetCommandButtonState("Modify");
                    }
                }
                catch (Exception ex)
                {
                    //   throw;
                }
            }
            else
            {
                try
                {
                    if (dgSMSTemplateList.SelectedItem != null)
                    {
                        ShowSMSTemplateDetails(((clsSMSTemplateVO)dgSMSTemplateList.SelectedItem).ID);
                    }
                    SetCommandButtonState("Modify");

                    _flip.Invoke(RotationType.Forward);
                }
                catch (Exception ex)
                {
                    //   throw;
                }
            }
        }

        void Modify()
        {
            try
            {
                clsSMSTemplateVO objTemplate = CreateFormData();
                clsSMSTemplateVO objTemplateSelected = (clsSMSTemplateVO)dgSMSTemplateList.SelectedItem;
                clsAddSMSTemplateBizActionVO objSMSTemplate = new clsAddSMSTemplateBizActionVO();
                objSMSTemplate.SMSTemplate = objTemplate;
                objSMSTemplate.SMSTemplate.ID = objTemplateSelected.ID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        string msgText = "SMS Template Details Modified Successfully";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow.Show();

                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                        mElement.Text = "";

                        _flip.Invoke(RotationType.Backward);
                        ClearFormData();
                    }
                    //else if (((clsAddSMSTemplateBizActionVO)ea.Result).SuccessStatus == 2)
                    //{
                    //    msgText = "Record cannot be save because CODE already exist!";
                    //    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //    msgWindow.Show();
                    //}
                    //else if (((clsAddSMSTemplateBizActionVO)ea.Result).SuccessStatus == 3)
                    //{
                    //    msgText = "Record cannot be save because DESCRIPTION already exist!";
                    //    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //    msgWindow.Show();
                    //}
                    else
                    {
                        string msgText = "Record cannot be added Please check the Details";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                    }
                    FillSMSTemplateList();

                };
                Client.ProcessAsync(objSMSTemplate, User);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool CheckDuplicasy()
        {
            clsSMSTemplateVO Item;
            clsSMSTemplateVO Item1;
            if (IsNew)
            {
                Item = ((PagedSortableCollectionView<clsSMSTemplateVO>)dgSMSTemplateList.ItemsSource).FirstOrDefault(p => p.Code.ToUpper().Equals(txtCode.Text.ToUpper()));
                Item1 = ((PagedSortableCollectionView<clsSMSTemplateVO>)dgSMSTemplateList.ItemsSource).FirstOrDefault(p => p.Description.ToUpper().Equals(txtName.Text.ToUpper()));
            }
            else
            {
                Item = ((PagedSortableCollectionView<clsSMSTemplateVO>)dgSMSTemplateList.ItemsSource).FirstOrDefault(p => p.Code.ToUpper().Equals(txtCode.Text.ToUpper()) && p.ID != ((clsSMSTemplateVO)dgSMSTemplateList.SelectedItem).ID);
                Item1 = ((PagedSortableCollectionView<clsSMSTemplateVO>)dgSMSTemplateList.ItemsSource).FirstOrDefault(p => p.Description.ToUpper().Equals(txtName.Text.ToUpper()) && p.ID != ((clsSMSTemplateVO)dgSMSTemplateList.SelectedItem).ID);
            }
            if (Item != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because CODE already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return false;
            }
            else if (Item1 != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because Template Name already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return false;
            }
            else
            {
                return true;
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearFormData();
                FillSMSTemplateList();

                _flip.Invoke(RotationType.Backward);

                SetCommandButtonState("Cancel");
                if (IsCancel == true)
                {
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                    mElement.Text = "Alerts & Notifications Configuration";
                                       
                    UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmAlertsandNotifications") as UIElement;
                    ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

                }
                else
                {
                    IsCancel = true;
                }
            }
            catch (Exception ex)
            {
                //   throw;
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FillSMSTemplateList();
        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {
            clsUpdateTemplatestatusBizActionVO objUpdateStatus = new clsUpdateTemplatestatusBizActionVO();
            objUpdateStatus.SMSTempStatus = new clsSMSTemplateVO();
            objUpdateStatus.SMSTempStatus.ID = (((clsSMSTemplateVO)dgSMSTemplateList.SelectedItem).ID);
            objUpdateStatus.SMSTempStatus.Status = (((clsSMSTemplateVO)dgSMSTemplateList.SelectedItem).Status);

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if ((ea.Error == null) && (ea.Result != null))
                {
                    if (objUpdateStatus.SMSTempStatus.Status == false)
                    {
                        msgText = "SMS Template Deactivated Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow.Show();
                    }
                    else
                    {
                        msgText = "SMS Template Activated Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow.Show();
                    }
                }
                else
                {
                    string msgText = "An Error Occured";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWindow.Show();
                }
            };
            client.ProcessAsync(objUpdateStatus, User); //new clsUserVO());
            client.CloseAsync();
            client = null;
        }

        private void FillSMSTemplateList()
        {
            try
            {
                this.DataContext = null;
                clsGetListSMSTemplateListBizActionVO objTempList = new clsGetListSMSTemplateListBizActionVO();
                objTempList.IsPagingEnabled = true;
                objTempList.MaximumRows = DataList.PageSize;
                objTempList.StartRowIndex = DataList.PageIndex * DataList.PageSize;

                objTempList.SearchExpression = txtSearchCriteria.Text.Trim();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        clsGetListSMSTemplateListBizActionVO result = ea.Result as clsGetListSMSTemplateListBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;

                        if (result.SMSList != null)
                        {
                            DataList.Clear();
                            foreach (var item in result.SMSList)
                            {
                                DataList.Add(item);
                            }

                            dgSMSTemplateList.ItemsSource = null;
                            dgSMSTemplateList.ItemsSource = DataList;

                            dgDataPager.Source = null;
                            dgDataPager.PageSize = objTempList.MaximumRows;
                            dgDataPager.Source = DataList;
                        }
                    }
                    else
                    {
                        msgText = "An Error Occured while filling the SMS Template List";

                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                    }
                };
                client.ProcessAsync(objTempList, User); //new clsUserVO());
                client.CloseAsync();
                client = null;
            }
            catch (Exception ex)
            {
                //   throw;
            }
        }

        void FormValidation()
        {
            txtCodeValid = true;
            txtNameValid = true;
            txtEngValid = true;

            if (string.IsNullOrEmpty(txtEnglish.Text.Trim()))
            {
                txtEnglish.SetValidation("SMS Template English Text cannot be blank.");
                txtEnglish.RaiseValidationError();
                txtEngValid = false;
            }
            else
                txtEnglish.ClearValidationError();

            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                txtName.SetValidation("SMS Template Name cannot be blank.");
                txtName.RaiseValidationError();
                txtNameValid = false;
            }
            else
                txtName.ClearValidationError();

            if (string.IsNullOrEmpty(txtCode.Text.Trim()))
            {
                txtCode.SetValidation("SMS Template Code Cannot be blank.");
                txtCode.RaiseValidationError();
                txtCodeValid = false;
            }
            else
                txtCode.ClearValidationError();
        }

        private clsSMSTemplateVO CreateFormData()
        {
            clsSMSTemplateVO TemplateObj = new clsSMSTemplateVO();
            TemplateObj.Code = txtCode.Text.Trim();
            TemplateObj.Description = txtName.Text.Trim();
            TemplateObj.EnglishText = txtEnglish.Text.Trim();
            TemplateObj.LocalText = txtLocal.Text.Trim();

            return TemplateObj;
        }

        private void dgSMSTemplateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void dgSMSTemplateList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
        }

        private void dgSMSTemplateList_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
        }

        void ShowSMSTemplateDetails(long SMSTemplateId)
        {
            clsGetSMSTemplateDetailsBizActionVO obj = new clsGetSMSTemplateDetailsBizActionVO();
            obj.ID = SMSTemplateId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            try
            {
                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Error == null && ea.Result != null)
                    {
                        clsSMSTemplateVO objTemp = new clsSMSTemplateVO();
                        objTemp = ((clsGetSMSTemplateDetailsBizActionVO)ea.Result).SMSDetails;
                        if (objTemp != null)
                        {
                            txtCode.Text = objTemp.Code;
                            txtName.Text = objTemp.Description;
                            txtEnglish.Text = objTemp.EnglishText;
                            txtLocal.Text = objTemp.LocalText;
                        }
                        else
                        {
                            msgText = "An Error Occured while retrieving the Template Data.";

                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                        }
                    }
                    else
                    { }
                };
                client.ProcessAsync(obj, User); //new clsUserVO());
                client.CloseAsync();
                client = null;
            }
            catch (Exception ex)
            {
                // waiting.Close();
                throw;
            }
           
        }

        void ClearFormData()
        {
            txtCode.Text = string.Empty;
            txtName.Text = string.Empty;
            txtEnglish.Text = string.Empty;
            txtLocal.Text = string.Empty;

            txtCode.ClearValidationError();
            txtName.ClearValidationError();
            txtEnglish.ClearValidationError();
            txtLocal.ClearValidationError();
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
 
    }
}
