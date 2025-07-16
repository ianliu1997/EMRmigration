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
using CIMS;
using PalashDynamics.UserControls;
using System.IO;
using PalashDynamics.ValueObjects.Administration;
using System.Collections.ObjectModel;
using PalashDynamics.Collections;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.Animations;
using System.Reflection;
using System.Windows.Browser;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using PalashDynamics.Service.EmailServiceReference;

namespace PalashDynamics.Administration
{
    public partial class frmEmailTemplate : UserControl
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
        FileInfo [] Attachment;
        FileInfo Attachment1;
        byte[] Attachment1data;
        byte[] Attachment2data;
        byte[] Attachment3data;
        byte[] [] data;
        byte[] data1;
        public string[] fileName;
        public string fileName1;
        public long NoofAttachment;
        public string ClinicEmailId;
        public long AttachmentId;
        public ObservableCollection<clsEmailTemplateVO> EmailAddedItems { get; set; }
        public bool IsNew = false;
        bool IsCancel = true;
        long FileAttachmentNo = 0;
       // List<string> AttachmentFiles;
        public ObservableCollection<string> AttachmentFiles { get; set; }
       
        #endregion

        #region Pagging

        public PagedSortableCollectionView<clsEmailTemplateVO> DataList { get; private set; }
        public ObservableCollection<clsEmailAttachmentVO> AddedFiles { get; set; }
        public List<clsEmailAttachmentVO> AttachmentList { get; set; }
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

        public frmEmailTemplate()
        {
            InitializeComponent();
            _flip = new PalashDynamics.Animations.SwivelAnimation(LayoutRoot1, LayoutRoot2, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            //this.Unloaded += new RoutedEventHandler(UserControl_Unloaded);
            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsEmailTemplateVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;
            //======================================================
            GetPatientConfigFields();
            AddedFiles = new ObservableCollection<clsEmailAttachmentVO>();
            AttachmentList = new List<clsEmailAttachmentVO>();
            AttachmentFiles = new ObservableCollection<string>();
        }
        
        /// <summary>
        /// This Function is Used To Fill The Template List On The Front Panel.
        /// </summary>
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

        private void GetPatientConfigFields()
        {
            try
            {
                clsGetPatientConfigFieldsBizActionVO bizActionVO = new clsGetPatientConfigFieldsBizActionVO();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<clsPatientFieldsConfigVO> objList = new List<clsPatientFieldsConfigVO>();
                        //objList = 
                        //objList.AddRange(((clsGetPatientConfigFieldsBizActionVO)args.Result).OtPateintConfigFieldsMatserDetails);
                        clsPatientFieldsConfigVO obj = new clsPatientFieldsConfigVO();
                        obj.ID = 0;
                        obj.FieldName = "--Select--";
                        objList.Add(obj);
                        objList.AddRange(((clsGetPatientConfigFieldsBizActionVO)args.Result).OtPateintConfigFieldsMatserDetails);

                        CmbOtTheatre.ItemsSource = null;
                        CmbOtTheatre.ItemsSource = objList;
                        CmbOtTheatre.SelectedItem = objList[0];
                    }
                };
                client.ProcessAsync(bizActionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!ISPageLoaded)
            {
                this.DataContext = null;
                FillEmailTemplateList();
                GetEmailfromConfig();
                SetCommandButtonState("Load");
                AddedFiles = new ObservableCollection<clsEmailAttachmentVO>();
                AttachmentFiles = new ObservableCollection<string>();
                dgEmailAttachmentList.ItemsSource = AddedFiles;
                AttachmentList = new List<clsEmailAttachmentVO>();
                //AttachmentFiles = null;
            }
            ISPageLoaded = true;
            //AttachmentList.Clear();
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
                    //if (isText == true)
                    //    txtTemplateText.Focus();
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
                        GetPatientConfigFields();
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

        private clsEmailTemplateVO CreateFormData()
        {
            clsEmailTemplateVO objTemp = new clsEmailTemplateVO();

            objTemp.Code = txtCode.Text.Trim();
            objTemp.Description = txtName.Text.Trim();
            objTemp.Subject = txtSubject.Text.Trim();
            objTemp.Text = TextEditor.Html;
          //  objTemp.Text = TextEditor.SelectedText;
            //objTemp.Text = txtTemplateText.Text.Trim();

            //if (!string.IsNullOrEmpty(txtFilePath.Text.Trim()))
            //{
            //    objTemp.AttachmentNos = 1;
            //    NoofAttachment = 1;
            //    objTemp.AttachmentDetails.AttachmentFileName = txtFilePath.Text.Trim();
            //    objTemp.AttachmentDetails.Attachment = data;
            //}
            //else
            //{
            //    objTemp.AttachmentNos = 0;
            //    NoofAttachment = 0;
            //}
            if (FileAttachmentNo > 0)
            {
                objTemp.AttachmentNos = FileAttachmentNo ;
                if (objTemp.AttachmentNos > 0)
                {
                    //objTemp.AttachmentDetails = 
                   // objTemp.AttachmentDetails= AddedFiles;
                    objTemp.AttachmentDetails = AttachmentList;
                    //for (int i = 0; i < FileAttachmentNo; i++)
                    //{
                    //    AttachmentFiles.Add(objTemp.AttachmentDetails[i].AttachmentFileName);
                    //}
                    //for (int i = 0; i <= objTemp.AttachmentNos; i++)
                    //{
                    //    objTemp.AttachmentDetails[i].AttachmentFileName = //((clsEmailAttachmentVO)dgEmailAttachmentList. //fileName[i];
                    //    objTemp.AttachmentDetails[i].Attachment = data[i];
                    //}
                }
            }
            else
                objTemp.AttachmentNos = 0;
            //if (rBtnText.IsChecked == true)
            //    objTemp.EmailFormat = true;
            //else
            //    objTemp.EmailFormat = false;
            return objTemp;
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
                    //if (isText == true)
                    //    txtTemplateText.Focus();
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
               // SetCommandButtonState("Modify");

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
                        GetPatientConfigFields();
                        ClearFormData();
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
                           // hlbView.IsEnabled = true;
                            //objTemp.SourceURL=
                            //if (objTemp.EmailFormat == true)
                            //    rBtnText.IsChecked = true;
                            //else
                            //    rBtnHTML.IsChecked = true;

                            txtCode.Text = objTemp.Code;
                            txtName.Text = objTemp.Description;
                            txtSubject.Text = objTemp.Subject;
                            TextEditor.Html = objTemp.Text;
                            FileAttachmentNo = objTemp.AttachmentNos;
                            if (objTemp.AttachmentNos > 0)
                            {
                                dgEmailAttachmentList.ItemsSource = null;
                                dgEmailAttachmentList.ItemsSource = objTemp.AttachmentDetails;                                
                                AddedFiles.Clear();
                                AttachmentList.Clear();
                                AttachmentFiles = new ObservableCollection<string>();
                                for (int i = 0; i < objTemp.AttachmentNos; i++)
                                {
                                    AddedFiles.Add(objTemp.AttachmentDetails[i]);
                                    AttachmentList.Add(objTemp.AttachmentDetails[i]);
                                    AttachmentFiles.Add(objTemp.AttachmentDetails[i].AttachmentFileName);
                                }
                            }
                            else
                                dgEmailAttachmentList.ItemsSource = null;
                            #region Commented
                            // txtTemplateText.Text = objTemp.Text;
                            //if (objTemp.EmailFormat == true)
                            //    rBtnText.IsChecked = true;
                            //else
                            //    rBtnHTML.IsChecked = true;

                            //if (objTemp.AttachmentNos > 0)
                            //{
                            //    NoofAttachment = objTemp.AttachmentNos;

                            //    if (NoofAttachment >= 1)
                            //    {
                            //        hblFile1.Content = objTemp.AttachmentDetails[0].AttachmentFileName;
                            //        data[0] = objTemp.AttachmentDetails[0].Attachment;
                            //        fileName[0] = objTemp.AttachmentDetails[0].AttachmentFileName;

                            //        if (NoofAttachment >= 2)
                            //        {
                            //            hblFile2.Content = objTemp.AttachmentDetails[1].AttachmentFileName;
                            //            data[1] = objTemp.AttachmentDetails[1].Attachment;
                            //            fileName[1] = objTemp.AttachmentDetails[1].AttachmentFileName;

                            //            if (NoofAttachment >= 3)
                            //            {
                            //                hblFile3.Content = objTemp.AttachmentDetails[2].AttachmentFileName;
                            //                data[2] = objTemp.AttachmentDetails[2].Attachment;
                            //                fileName[2] = objTemp.AttachmentDetails[2].AttachmentFileName;
                            //            }
                            //        }

                            //    }                                

                            //    //AttachmentId = objTemp.AttachmentDetails.ID;
                            //    //txtFilePath.Text = objTemp.AttachmentDetails.AttachmentFileName;
                            //    //data = objTemp.AttachmentDetails.Attachment;
                            //    //fileName = objTemp.AttachmentDetails.AttachmentFileName;


                            //}
                            //else
                            //{
                            //    NoofAttachment = 0;
                            //   // txtFilePath.Text = " ";
                            //    data = null;
                            //    fileName = null;
                            //}
                            #endregion
                            
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
            GetPatientConfigFields();
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
                        txtEmailId.Text = "";
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

        private void cmdBrowse_Click(object sender, RoutedEventArgs e)
        {
            AttachFile();
        }

        private void AttachFile()
        {
                       
        }

        //private void cmdAdd_Click(object sender, RoutedEventArgs e)
        //{
        //    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
        //    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

        //    client.AttachmentFileCompleted += (s, args) =>
        //    {
        //        if (args.Error == null)
        //        {
        //            MessageBoxControl.MessageBoxChildWindow msgW1 =
        //            new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Attachment added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

        //            msgW1.Show();

        //            hlbView.IsEnabled = true;
        //        }
        //        else
        //        {
        //            MessageBoxControl.MessageBoxChildWindow msgW1 =
        //            new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Error Occured while adding the attachment.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

        //            msgW1.Show();
        //        }
        //    };
        //    client.AttachmentFileAsync(txtFilePath.Text, data);
        //}

        private void cmdTest_Click(object sender, RoutedEventArgs e)
        {
            txtEmailId.Focus();
            cmdSend.IsEnabled = true;
        }

        private void hlbView_Click(object sender, RoutedEventArgs e)
        {
            //get the attachment details
           // clsEmailTemplateVO objTemplate = CreateFormData();
            //ShowEmailTemplateDetails()
            
            // if (!string.IsNullOrEmpty(txtFilePath.Text.Trim()))
            if (IsNew == true)
            {
                if (!string.IsNullOrEmpty(fileName1))
                {
                   // HtmlPage.Window.Invoke("openAttachment", new string[] { txtFilePath.Text });
                    HtmlPage.Window.Invoke("openAttachment", new string[] { fileName1 });
                }
            }
            else
            {
                fileName1 = ((clsEmailAttachmentVO)dgEmailAttachmentList.SelectedItem).AttachmentFileName;
                data1 = ((clsEmailAttachmentVO)dgEmailAttachmentList.SelectedItem).Attachment;
                //if (((clsEmailTemplateVO)dgTest.SelectedItem).IsCompleted == true)
                if (!string.IsNullOrEmpty(fileName1))
                {
                    //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

                    client.UploadETemplateFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("openAttachment", new string[] { fileName1 });
                           // HtmlPage.Window.Invoke("openAttachment", new string[] { txtFilePath.Text });// { ((clsEmailTemplateVO)dgEmailTemplateList.SelectedItem).SourceURL}); //.SelectedItem).SourceURL });
                            // listOfReports.Add(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SourceURL);
                        }
                    };
                    client.UploadETemplateFileAsync(fileName1, data1);
                }
                else
                {
                    //MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "This test's report is not uploaded. Please upload the report then click on preview", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    //mgbx.Show();
                }
            }
        }

        private void cmdSend_Click(object sender, RoutedEventArgs e)
        {
            FormValidation();
            waiting.Show();
            clsEmailTemplateVO objAttachment = new clsEmailTemplateVO();
            objAttachment.AttachmentDetails = AttachmentList;
            if (isCodeValid == true && isNameValid == true && isSubjectValid == true && isText == true)
            {
                if (FileAttachmentNo>=1)
                {
                    Uri address1 = new Uri(Application.Current.Host.Source, "../EmailService.svc"); // this url will work both in dev and after deploy
                    EmailServiceClient EmailClient = new EmailServiceClient("CustomBinding_EmailService", address1.AbsoluteUri);
                    EmailClient.SendEmailwithAttachmentCompleted += (ea, args) =>
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
                    EmailClient.SendEmailwithAttachmentAsync(ClinicEmailId, txtEmailId.Text, txtSubject.Text, TextEditor.Html, FileAttachmentNo, AttachmentFiles);//txtFilePath.Text);
                    EmailClient.CloseAsync();
                }
                else
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
                    EmailClient.SendEmailAsync(ClinicEmailId, txtEmailId.Text, txtSubject.Text, TextEditor.Html);
                    EmailClient.CloseAsync();
                }
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
                //if (isText == true)
                //    txtTemplateText.Focus();
            }   
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

        private void FormValidation()
        {
            isCodeValid = true;
            isNameValid = true;
            isSubjectValid = true;
            isText = true;

            //if (string.IsNullOrEmpty(txtTemplateText.Text.Trim()))
            //{
            //    txtTemplateText.SetValidation("Email Template cannot be blank.");
            //    txtTemplateText.RaiseValidationError();
            //    isText = false;
            //}
            //else
            //    txtTemplateText.ClearValidationError();

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

        private void ClearFormData()
        {
            txtCode.Text = string.Empty;
            txtName.Text = string.Empty;
            txtSubject.Text = string.Empty;
            TextEditor.Html = string.Empty;            
            txtEmailId.Text = string.Empty;
            cmdSend.IsEnabled = false;
            txtCode.ClearValidationError();
            txtName.ClearValidationError();
            txtSubject.ClearValidationError();
            AttachmentList = new List<clsEmailAttachmentVO>();
            AttachmentList.Clear();
            AddedFiles = new ObservableCollection<clsEmailAttachmentVO>();
            AddedFiles.Clear();            
            dgEmailAttachmentList.ItemsSource = null;
            AttachmentFiles = new ObservableCollection<string>();
            FileAttachmentNo = 0;
           // AttachmentFiles = new ObservableCollection<string>();
           // AttachmentFiles.Clear();
            #region Commented
            //txtTemplateText.Text = string.Empty;
            //txtEmailFormat.Text = string.Empty;           
            //txtFilePath.Text = string.Empty;
            //txtTemplateText.ClearValidationError();
            //txtEmailFormat.ClearValidationError();
            //cmdAdd.IsEnabled = false;
            //cmdTest.Visibility = Visibility.Collapsed;
            //hlbView.IsEnabled = false;
            //lblEmail.IsEnabled = false;
            //txtEmailId.IsEnabled = false;
            #endregion            
        }

        private void GetEmailfromConfig()
        {
            try
            {
                clsAppConfigVO objAppcongif = new clsAppConfigVO();
                objAppcongif.ID = 1;

                clsGetAppConfigBizActionVO objApp = new clsGetAppConfigBizActionVO();
                objApp.AppConfig = objAppcongif;
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

        /// <summary>
        /// gets richtext editor
        /// </summary>
        public Liquid.RichTextEditor TextEditor
        {
            get { return richTextEditor; }
        }

        /// <summary>
        /// gets richtext box
        /// </summary>
        public Liquid.RichTextBox rt
        {
            get { return richTextEditor.TextBox; }
        }
        
        /// <summary>
        /// adds patient config fields to the rich text area
        /// </summary>
        /// <param name="sender">add button </param>
        /// <param name="e">add button  click</param>
        private void lnkAddItems_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CmbOtTheatre.SelectedItem != null && ((clsPatientFieldsConfigVO)CmbOtTheatre.SelectedItem).ID != 0)
                {
                    Point str = TextEditor.CusrsorPosition;

                    TextEditor.SelectAll();
                    string str1 = TextEditor.SelectedText;
                    //string str2 = "<span style=" + "color:#999999;" + ">" + " {" + CmbOtTheatre.SelectedItem.ToString() + "}</span>";
                    string str2 = " {" + CmbOtTheatre.SelectedItem.ToString() + "}";

                    TextEditor.SelectedText = String.Empty;

                    String html = TextEditor.Html;
                    string s = TextEditor.TextBox.Text;

                    //Liquid.RichTextBox rt = TextEditor.TextBox;

                    if (TextEditor.Html != null)
                    {
                        if (TextEditor.Html == "")
                            TextEditor.Html = "<p style=\"margin:0px;\"><span class=\"Normal\">" + str2 + "</span></p>\r";
                        else
                        {
                            string find = html.Insert(html.LastIndexOf("</span>") + 7, str2);
                            TextEditor.Html = find;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        private void hblFileRemove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void hblFile_Click(long FileNo, string FileName)
        {
            //get the attachment details
            
            if (IsNew == true)
            {
                if (!string.IsNullOrEmpty(fileName[FileNo]))
                {
                    HtmlPage.Window.Invoke("openAttachment", new string[] { FileName });
                }
            }
            else
            {                
                if (!string.IsNullOrEmpty(fileName[FileNo]))
                {
                    //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

                    client.UploadETemplateFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("openAttachment", new string[] { FileName });// { ((clsEmailTemplateVO)dgEmailTemplateList.SelectedItem).SourceURL}); //.SelectedItem).SourceURL });
                            // listOfReports.Add(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SourceURL);
                        }
                    };
                    client.UploadETemplateFileAsync(FileName, data[FileNo]);
                    //client.UploadETemplateFileAsync(((clsEmailTemplateVO)dgEmailTemplateList.SelectedItem).SourceURL, ((clsEmailTemplateVO)dgEmailTemplateList.SelectedItem).Report);
                }
                else
                {
                    //MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "This test's report is not uploaded. Please upload the report then click on preview", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    //mgbx.Show();
                }
            }
        }

        #region
        private void hblFile1_Click(object sender, RoutedEventArgs e)
        {
            //long FileNo = 0;
            //string FileName = Convert.ToString(hblFile1.Content);
            //hblFile_Click(FileNo,FileName);
        }

        private void hblFile2_Click(object sender, RoutedEventArgs e)
        {
            //long FileNo = 1;
            //string FileName = Convert.ToString(hblFile2.Content);
            //hblFile_Click(FileNo, FileName);
        }

        private void hblFile3_Click(object sender, RoutedEventArgs e)
        {
            //long FileNo = 2;
            //string FileName = Convert.ToString(hblFile3.Content);
            //hblFile_Click(FileNo, FileName);
        }

        private void hblAttach_Click(object sender, RoutedEventArgs e)
        {            
            OpenFileDialog OpenFile = new OpenFileDialog();

            if (OpenFile.ShowDialog() == true)
            {
                try
                {
                    clsEmailAttachmentVO AttachmentDetails = new clsEmailAttachmentVO();

                    using (Stream stream = OpenFile.File.OpenRead())
                    {
                        // Don't allow really big files (more than 5 MB).
                        if (stream.Length < 5120000)
                        {
                            #region Commented
                            //if(FileAttachmentNo==1)
                            //{    Attachment1data = new byte[stream.Length];
                            //    stream.Read(Attachment1data, 0, (int)stream.Length);
                            //}
                            //else if (FileAttachmentNo == 2)
                            //{
                            //    Attachment2data = new byte[stream.Length];
                            //    stream.Read(Attachment2data, 0, (int)stream.Length);
                            //}
                            //else if (FileAttachmentNo == 3)
                            //{
                            //    Attachment3data = new byte[stream.Length];
                            //    stream.Read(Attachment3data, 0, (int)stream.Length);
                            //}
                            //txtFilePath.Text = OpenFile.File.Name;
                            //fileName = txtFilePath.Text;
                            //cmdAdd.IsEnabled = true;
                            #endregion
                            long i = FileAttachmentNo;
                            data1 = new byte[stream.Length];
                            stream.Read(data1, 0, (int)stream.Length);
                            Attachment1 = OpenFile.File;
                            fileName1 = OpenFile.File.Name;

                            AttachmentDetails.Attachment = data1;
                            AttachmentDetails.AttachmentFileName = fileName1;

                            AttachmentList.Add(AttachmentDetails);
                            AddedFiles.Add(AttachmentDetails);
                            AttachmentFiles.Add(AttachmentDetails.AttachmentFileName);
                            dgEmailAttachmentList.ItemsSource = null;
                            dgEmailAttachmentList.ItemsSource = AddedFiles;

                            FileAttachmentNo = FileAttachmentNo + 1;
                           // AttachmentFiles.Add(AttachmentDetails.AttachmentFileName);
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
            //AttachFile();
            #region Commented
            //// FileAttachmentNo = 1;
            // if (FileAttachmentNo < 3)
            // {
            //     AttachFile();
            //     if (FileAttachmentNo == 0)
            //     {
            //         hblFile1.Content = fileName[0];
            //         hblFileRemove1.Visibility = Visibility.Visible;
            //     }
            //     if (FileAttachmentNo == 1)
            //     {
            //         hblFile2.Content = fileName[1];
            //         hblFileRemove2.Visibility = Visibility.Visible;
            //     }
            //     if (FileAttachmentNo == 2)
            //     {
            //         hblFile3.Content = fileName[2];
            //         hblFileRemove3.Visibility = Visibility.Visible;
            //         hblAttach.IsEnabled = false;
            //     }
            //     FileAttachmentNo = FileAttachmentNo + 1;
            // }
            #endregion
        }
        #endregion
        private void cmdDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddedFiles.RemoveAt(dgEmailAttachmentList.SelectedIndex);
                AttachmentList.RemoveAt(dgEmailAttachmentList.SelectedIndex);
                dgEmailAttachmentList.ItemsSource = null;
                dgEmailAttachmentList.ItemsSource = AddedFiles;                
                dgEmailAttachmentList.UpdateLayout();
                AttachmentFiles = new ObservableCollection<string>();
                foreach (var item in AttachmentList)
                {
                    AttachmentFiles.Add(item.AttachmentFileName);
                }
                FileAttachmentNo = FileAttachmentNo - 1;
            }
            catch (Exception ex)
            {
                throw;
            }
        }        
    }
}
