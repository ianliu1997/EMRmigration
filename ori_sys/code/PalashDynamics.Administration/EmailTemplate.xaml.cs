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
using PalashDynamics.Service.EmailServiceReference;
using System.Windows.Data;
using System.IO;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Browser;
using System.Collections.ObjectModel;
using PalashDynamics.Collections;
using System.Reflection;


namespace PalashDynamics.Administration
{
    public partial class EmailTemplate : UserControl
    {
        #region Variable Declaration

        PalashDynamics.Animations.SwivelAnimation _flip = null;       
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        WaitIndicator waiting = new WaitIndicator();
        bool ISPageLoaded = false;
        public string msgText = "";
        public string msgTitle = "Palash";
        public bool isCodeValid = true;
        public bool isNameValid = true;
        public bool isSubjectValid = true;
        public bool isText = true;
        FileInfo Attachment;
        byte[] data;
        public string fileName;
        public long NoofAttachment;
        public string ClinicEmailId;
        public long AttachmentId;
        public ObservableCollection<clsEmailTemplateVO> EmailAddedItems { get; set; }
        public bool IsNew = false;
        bool IsCancel = true;
        #endregion

        #region Pagging

        public PagedSortableCollectionView<clsEmailTemplateVO> DataList { get; private set; }

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
            }
        }

        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillEmailTemplateList();
        }

        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        /// 
        #endregion

        public EmailTemplate()
        {
            InitializeComponent();
            _flip = new PalashDynamics.Animations.SwivelAnimation(LayoutRoot1, LayoutRoot2, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            this.Unloaded += new RoutedEventHandler(UserControl_Unloaded);
            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsEmailTemplateVO>();
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
                FillEmailTemplateList();
                GetEmailfromConfig();
                SetCommandButtonState("Load");
            }

            ISPageLoaded = true;
        }

        private void GetEmailfromConfig()
        {
            try
            {
                clsAppConfigVO objAppcongif = new clsAppConfigVO();
                objAppcongif.ID = 1;

                clsGetAppConfigBizActionVO objApp = new clsGetAppConfigBizActionVO();
                objApp.AppConfig = objAppcongif;
                objApp.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        clsAppConfigVO objAppEmail = new clsAppConfigVO();
                        objAppEmail = ((clsGetAppConfigBizActionVO)ea.Result).AppConfig;
                        ClinicEmailId = objAppEmail.Email;
                    }
                };
                client.ProcessAsync(objApp, new clsUserVO());
                client.CloseAsync();
                client = null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (NoofAttachment == 1)
            {
                //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                
                Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                client.DeleteAttachmentFileCompleted += (s1, args1) =>
                {
                    if (args1.Error == null)
                    {
                    }
                };
                client.DeleteAttachmentFileAsync(fileName);
            }
        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            ClearFormData();
            FormValidation();
            IsNew = true;
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "New Email Template";

            _flip.Invoke(RotationType.Forward);

            this.SetCommandButtonState("New");
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            waiting.Show();
            try
            {
                if (((clsEmailTemplateVO)dgEmailTemplateList.SelectedItem).Status == true)
                {
                    if (dgEmailTemplateList.SelectedItem != null)
                    {
                        IsNew = false;
                        ShowEmailTemplateDetails(((clsEmailTemplateVO)dgEmailTemplateList.SelectedItem).ID);
                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                        mElement.Text = " : " + ((clsEmailTemplateVO)dgEmailTemplateList.SelectedItem).Code;

                        _flip.Invoke(RotationType.Forward);
                    }
                    SetCommandButtonState("View");
                    waiting.Close();
                }
                else
                {
                    waiting.Close();
                    msgText = "Cannot view the Details, The Template is disabled";

                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
            }
            catch (Exception ex)
            {
                waiting.Close();
                throw;
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            waiting.Show();
            try
            {
                FormValidation();
                if (isCodeValid == true && isNameValid == true && isSubjectValid == true && isText == true)
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
                    if (isCodeValid == true)
                        txtCode.Focus();
                    if (isNameValid == true)
                        txtName.Focus();
                    if (isSubjectValid == true)
                        txtSubject.Focus();
                    if (isText == true)
                        txtTemplateText.Focus();
                }
            }
            catch (Exception ex)
            {
                waiting.Close();
                throw;
            }

        }

        private void msgWindowSave_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (CheckDuplicasy())
                {
                    Save();
                    SetCommandButtonState("Save");
                }
            }
        }

        void Save()
        {
            try
            {
                clsEmailTemplateVO objTemplate = CreateFormData();
                clsAddEmailTemplateBizActionVO objEmailTemplate = new clsAddEmailTemplateBizActionVO();
                objEmailTemplate.EmailTemplate = objTemplate;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        FillEmailTemplateList();
                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                        mElement.Text = "";

                        _flip.Invoke(RotationType.Backward);

                        string msgText = "Email Template Details Added Successfully";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow.Show();
                    }
                    else
                    {
                        string msgText = "Record cannot be added Please check the Details";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                    }
                };
                Client.ProcessAsync(objEmailTemplate, User);
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
                if (isCodeValid == true && isNameValid == true && isSubjectValid == true && isText == true)
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
                    if (isCodeValid == true)
                        txtCode.Focus();
                    if (isNameValid == true)
                        txtName.Focus();
                    if (isSubjectValid == true)
                        txtSubject.Focus();
                    if (isText == true)
                        txtTemplateText.Focus();
                }
            }
            catch (Exception ex)
            {
                waiting.Close();
                throw;
            }
        }

        private void msgWindowModify_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (CheckDuplicasy())
                {
                    Modify();
                    SetCommandButtonState("Modify");
                }
            }
            else
            {
                if (dgEmailTemplateList.SelectedItem != null)
                    ShowEmailTemplateDetails(((clsEmailTemplateVO)dgEmailTemplateList.SelectedItem).ID);

                SetCommandButtonState("Modify");

                _flip.Invoke(RotationType.Forward);
            }
        }

        void Modify()
        {
            try
            {
                clsEmailTemplateVO objTemplate = CreateFormData();
                clsEmailTemplateVO objEmailTemplateSelected = (clsEmailTemplateVO)dgEmailTemplateList.SelectedItem;
                clsAddEmailTemplateBizActionVO objEmailTemplate = new clsAddEmailTemplateBizActionVO();
                objEmailTemplate.EmailTemplate = objTemplate;
                objEmailTemplate.EmailTemplate.ID = objEmailTemplateSelected.ID;
               // objEmailTemplate.EmailTemplate.AttachmentDetails.ID = AttachmentId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        FillEmailTemplateList();

                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                        mElement.Text = "";

                        _flip.Invoke(RotationType.Backward);

                        string msgText = "Email Template Details Modified Successfully";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow.Show();
                    }
                    else
                    {

                        string msgText = "Record cannot be added Please check the Details";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                    }
                };
                Client.ProcessAsync(objEmailTemplate, User);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool CheckDuplicasy()
        {
            clsEmailTemplateVO Item;
            clsEmailTemplateVO Item1;
            if (IsNew)
            {
                Item = ((PagedSortableCollectionView<clsEmailTemplateVO>)dgEmailTemplateList.ItemsSource).FirstOrDefault(p => p.Code.ToUpper().Equals(txtCode.Text.ToUpper()));
                Item1 = ((PagedSortableCollectionView<clsEmailTemplateVO>)dgEmailTemplateList.ItemsSource).FirstOrDefault(p => p.Description.ToUpper().Equals(txtName.Text.ToUpper()));
            }
            else
            {
                Item = ((PagedSortableCollectionView<clsEmailTemplateVO>)dgEmailTemplateList.ItemsSource).FirstOrDefault(p => p.Code.ToUpper().Equals(txtCode.Text.ToUpper()) && p.ID != ((clsEmailTemplateVO)dgEmailTemplateList.SelectedItem).ID);
                Item1 = ((PagedSortableCollectionView<clsEmailTemplateVO>)dgEmailTemplateList.ItemsSource).FirstOrDefault(p => p.Description.ToUpper().Equals(txtName.Text.ToUpper()) && p.ID != ((clsEmailTemplateVO)dgEmailTemplateList.SelectedItem).ID);
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
            FillEmailTemplateList();

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

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FillEmailTemplateList();
        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {
            clsUpdateEmailTemplateStatusBizActionVO objUpdateStatus = new clsUpdateEmailTemplateStatusBizActionVO();
            objUpdateStatus.EmailTempStatus = new clsEmailTemplateVO();
            objUpdateStatus.EmailTempStatus.ID = (((clsEmailTemplateVO)dgEmailTemplateList.SelectedItem).ID);
            objUpdateStatus.EmailTempStatus.Status = (((clsEmailTemplateVO)dgEmailTemplateList.SelectedItem).Status);

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if ((ea.Error == null) && (ea.Result != null))
                {
                    if (objUpdateStatus.EmailTempStatus.Status == false)
                    {
                        msgText = "Email Template Deactivated Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow.Show();
                    }
                    else
                    {
                        msgText = "Email Template Activated Successfully.";
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
            client.ProcessAsync(objUpdateStatus, new clsUserVO());
            client.CloseAsync();
            client = null;
        }

        private void dgEmailTemplateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void dgEmailTemplateList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
        }

        private void dgEmailTemplateList_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
        }

        private void FillEmailTemplateList()
        {
            try
            {
                clsGetEmailTemplateListBizActionVO objTempList = new clsGetEmailTemplateListBizActionVO();
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
                        clsGetEmailTemplateListBizActionVO result = ea.Result as clsGetEmailTemplateListBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;

                        if (result.EmailList != null)
                        {
                            DataList.Clear();
                            foreach (var item in result.EmailList)
                            {
                                DataList.Add(item);
                            }

                            dgEmailTemplateList.ItemsSource = null;
                            dgEmailTemplateList.ItemsSource = DataList;

                            dgDataPager.Source = null;
                            dgDataPager.PageSize = objTempList.MaximumRows;
                            dgDataPager.Source = DataList;
                        }
                    }
                    else
                    {
                        msgText = "An Error Occured while filling the Email Template List";

                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                    }
                };
                client.ProcessAsync(objTempList, new clsUserVO());
                client.CloseAsync();
                client = null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void FormValidation()
        {
            isCodeValid = true;
            isNameValid = true;
            isSubjectValid = true;
            isText = true;

            if (string.IsNullOrEmpty(txtTemplateText.Text.Trim()))
            {
                txtTemplateText.SetValidation("Email Template cannot be blank.");
                txtTemplateText.RaiseValidationError();
                isText = false;
            }
            else
                txtTemplateText.ClearValidationError();

            if (string.IsNullOrEmpty(txtSubject.Text.Trim()))
            {
                txtSubject.SetValidation("Email Template Subject cannot be blank.");
                txtSubject.RaiseValidationError();
                isSubjectValid = false;
            }
            else
                txtSubject.ClearValidationError();

            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                txtName.SetValidation("Email Template Name cannot be blank.");
                txtName.RaiseValidationError();
                isNameValid = false;
            }
            else
                txtName.ClearValidationError();

            if (string.IsNullOrEmpty(txtCode.Text.Trim()))
            {
                txtCode.SetValidation("Email Template Code Cannot be blank.");
                txtCode.RaiseValidationError();
                isCodeValid = false;
            }
            else
                txtCode.ClearValidationError();
        }

        private clsEmailTemplateVO CreateFormData()
        {
            clsEmailTemplateVO objTemp = new clsEmailTemplateVO();

            objTemp.Code = txtCode.Text.Trim();
            objTemp.Description = txtName.Text.Trim();
            objTemp.Subject = txtSubject.Text.Trim();
            objTemp.Text = txtTemplateText.Text.Trim();
            if (!string.IsNullOrEmpty(txtFilePath.Text.Trim()))
            {
                objTemp.AttachmentNos = 1;
                NoofAttachment = 1;
                //objTemp.AttachmentDetails.AttachmentFileName = txtFilePath.Text.Trim();
                //objTemp.AttachmentDetails.Attachment = data;
            }
            else
            {
                objTemp.AttachmentNos = 0;
                NoofAttachment = 0;
            }
            if (rBtnText.IsChecked == true)
                objTemp.EmailFormat = true;
            else
                objTemp.EmailFormat = false;
            return objTemp;
        }

        private void ShowEmailTemplateDetails(long EmailTemplateId)
        {
            try
            {
                clsGetEmailTemplateBizActionVO obj = new clsGetEmailTemplateBizActionVO();
                obj.ID = EmailTemplateId;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Error == null && ea.Result != null)
                    {
                        clsEmailTemplateVO objTemp = new clsEmailTemplateVO();
                        objTemp = ((clsGetEmailTemplateBizActionVO)ea.Result).EmailDetails;

                        if (objTemp.AttachmentDetails != null)
                        {
                            hlbView.IsEnabled = true;
                            //objTemp.SourceURL=
                            txtCode.Text = objTemp.Code;
                            txtName.Text = objTemp.Description;
                            txtSubject.Text = objTemp.Subject;
                            txtTemplateText.Text = objTemp.Text;

                            if (objTemp.EmailFormat == true)
                                rBtnText.IsChecked = true;
                            else
                                rBtnHTML.IsChecked = true;

                            if (objTemp.AttachmentNos == 1)
                            {
                                NoofAttachment = 1;
                                //AttachmentId = objTemp.AttachmentDetails.ID;
                                //txtFilePath.Text = objTemp.AttachmentDetails.AttachmentFileName;
                                //data = objTemp.AttachmentDetails.Attachment;
                                //fileName = objTemp.AttachmentDetails.AttachmentFileName;
                            }
                            else
                            {
                                NoofAttachment = 0;
                                txtFilePath.Text = " ";
                                data = null;
                                fileName = null;
                            }
                        }
                        else
                        {
                            msgText = "An Error Occured while Retrieving the Email Template.";

                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                        }
                    }
                };
                client.ProcessAsync(obj, new clsUserVO());
                client.CloseAsync();
                client = null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

       private void ClearFormData()
       {
           txtCode.Text = string.Empty;
           txtName.Text = string.Empty;
           txtSubject.Text = string.Empty;
           txtTemplateText.Text = string.Empty;
           //txtEmailFormat.Text = string.Empty;           
           txtFilePath.Text = string.Empty;
           txtEmailId.Text = string.Empty;

           cmdAdd.IsEnabled = false;
           //cmdTest.Visibility = Visibility.Collapsed;
           hlbView.IsEnabled = false;
           //lblEmail.IsEnabled = false;
           //txtEmailId.IsEnabled = false;
           cmdSend.IsEnabled = false;

           txtCode.ClearValidationError();
           txtName.ClearValidationError();
           txtSubject.ClearValidationError();
           txtTemplateText.ClearValidationError();
           //txtEmailFormat.ClearValidationError();
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

       private void cmdBrowse_Click(object sender, RoutedEventArgs e)
       {
           OpenFileDialog OpenFile = new OpenFileDialog();
           
           if (OpenFile.ShowDialog() == true)
           {
               try
               {
                   using (Stream stream = OpenFile.File.OpenRead())
                    {
                        // Don't allow really big files (more than 5 MB).
                        if (stream.Length < 5120000)
                        {
                            data = new byte[stream.Length];
                            stream.Read(data, 0, (int)stream.Length);
                            Attachment = OpenFile.File;

                            txtFilePath.Text = OpenFile.File.Name;
                            fileName = txtFilePath.Text;
                            cmdAdd.IsEnabled = true;                      
                        }
                        else
                        {
                            MessageBox.Show("File must be less than 5 MB");
                        }
                    }
               }
               catch (Exception ex)
               {                   
                   //throw;
               }
           }
       }
               
       private void cmdTest_Click(object sender, RoutedEventArgs e)
       {          
           txtEmailId.Focus();

           cmdSend.IsEnabled = true;
       }

       private void cmdSend_Click(object sender, RoutedEventArgs e)
       {
           FormValidation();
           waiting.Show();
           if (isCodeValid == true && isNameValid == true && isSubjectValid == true && isText == true)
           {               
               if (string.IsNullOrEmpty(txtFilePath.Text))
               {
                   Uri address1 = new Uri(Application.Current.Host.Source, "../EmailService.svc"); // this url will work both in dev and after deploy
                   EmailServiceClient EmailClient = new EmailServiceClient("CustomBinding_EmailService", address1.AbsoluteUri);
                   EmailClient.SendEmailCompleted += (ea, args) =>
                   {
                       waiting.Close();
                       if (args.Error == null)
                       {

                           MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Email Request Sent To The Server Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                           msgW1.Show();
                       }
                       else
                       {
                           MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Error Occured while sending the message.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                           msgW1.Show();
                       }
                   };
                   EmailClient.SendEmailAsync(ClinicEmailId, txtEmailId.Text, txtSubject.Text, txtTemplateText.Text);
                   EmailClient.CloseAsync();

               }
               //else
               //{
               //    Uri address1 = new Uri(Application.Current.Host.Source, "../EmailService.svc"); // this url will work both in dev and after deploy
               //    EmailServiceClient EmailClient = new EmailServiceClient("CustomBinding_EmailService", address1.AbsoluteUri);
               //    EmailClient.SendEmailwithAttachmentCompleted += (ea, args) =>
               //    {
               //        waiting.Close();
               //        if (args.Error == null)
               //        {
               //            MessageBoxControl.MessageBoxChildWindow msgW1 =
               //            new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Email Request Sent To The Server Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

               //            msgW1.Show();
               //        }
               //        else
               //        {
               //            MessageBoxControl.MessageBoxChildWindow msgW1 =
               //            new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Error Occured while sending the message.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

               //            msgW1.Show();
               //        }
               //    };
               //    EmailClient.SendEmailwithAttachmentAsync(ClinicEmailId, txtEmailId.Text, txtSubject.Text, txtTemplateText.Text, txtFilePath.Text);
               //    EmailClient.CloseAsync();
               //}
           }
           else
           {
               waiting.Close();
               MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Cannot Send Email. Data is Incomplete.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

               msgW1.Show();
               if (isCodeValid == true)
                   txtCode.Focus();
               if (isNameValid == true)
                   txtName.Focus();
               if (isSubjectValid == true)
                   txtSubject.Focus();
               if (isText == true)
                   txtTemplateText.Focus();
           }           
       }
              
       private void cmdAdd_Click(object sender, RoutedEventArgs e)
       {  
           //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
           //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
           
           Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
           DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

           client.AttachmentFileCompleted += (s, args) =>
           {
               if (args.Error == null)
               {
                   MessageBoxControl.MessageBoxChildWindow msgW1 =
                   new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Attachment added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                   msgW1.Show();

                   hlbView.IsEnabled = true;
               }
               else
               {
                   MessageBoxControl.MessageBoxChildWindow msgW1 =
                   new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Error Occured while adding the attachment.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                   msgW1.Show();
               }
           };
           client.AttachmentFileAsync(txtFilePath.Text, data);
       }
        
       private void hlbView_Click(object sender, RoutedEventArgs e)
       {
           //get the attachment details
            clsEmailTemplateVO objTemplate = CreateFormData();
           //ShowEmailTemplateDetails()

          // if (!string.IsNullOrEmpty(txtFilePath.Text.Trim()))
            if (IsNew == true)
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    HtmlPage.Window.Invoke("openAttachment", new string[] { txtFilePath.Text });
                }
            }
            else
            {
                //if (((clsEmailTemplateVO)dgTest.SelectedItem).IsCompleted == true)
                if(!string.IsNullOrEmpty(fileName))
                {
                    //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    
                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

                    client.UploadETemplateFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("openAttachment", new string[] { txtFilePath.Text });// { ((clsEmailTemplateVO)dgEmailTemplateList.SelectedItem).SourceURL}); //.SelectedItem).SourceURL });
                           // listOfReports.Add(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SourceURL);
                        }
                    };
                    client.UploadETemplateFileAsync(txtFilePath.Text, data);
                    //client.UploadETemplateFileAsync(((clsEmailTemplateVO)dgEmailTemplateList.SelectedItem).SourceURL, ((clsEmailTemplateVO)dgEmailTemplateList.SelectedItem).Report);
                }
                else
                {
                    //MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "This test's report is not uploaded. Please upload the report then click on preview", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    //mgbx.Show();
                }
            }
           
       }

    }

      
}
