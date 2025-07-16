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
using System.Reflection;
using PalashDynamics.Animations;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using System.ComponentModel;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Administration.DischargeTemplateMaster;
using System.Text;
using C1.Silverlight;
using C1.Silverlight.RichTextBox;
using C1.Silverlight.SpellChecker;
using C1.Silverlight.RichTextBox.Documents;
using System.Windows.Printing;

namespace PalashDynamics.Administration
{
    public partial class frmDischargeTemplate : UserControl
    {
        #region IPreInitiateCIMS Members

        PalashDynamics.ValueObjects.Administration.clsMenuVO _SelfMenuDetails = null;

        public void PreInitiate(PalashDynamics.ValueObjects.Administration.clsMenuVO _MenuDetails)
        {
            _SelfMenuDetails = _MenuDetails;
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Implemts the INotifyPropertyChanged interface.
        /// </summary>
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

        public PagedSortableCollectionView<clsDischargeTemplateMasterVO> MasterList { get; private set; }
        private SwivelAnimation objAnimation;
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        bool IsCancel = true;
        string msgTitle = "";
        string msgText = "";
        bool IsModify;
        List<clsDischargeTemplateDetailsVO> objDischargeDetailsList = null;
        String Htmlrichtexbox;
        List<MasterListItem> BindingControlList = new List<MasterListItem>();

        public frmDischargeTemplate()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(grdDischargeList, grdDischargeDetails, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            this.Loaded += new RoutedEventHandler(frmDischargeTemplate_Loaded);
            Htmlrichtexbox = richTextBox.Html;
        }

        void frmDischargeTemplate_Loaded(object sender, RoutedEventArgs e)
        {
            FillParameter();
            FillFontComboBox();
            FillBindingControl();

            objDischargeDetailsList = new List<clsDischargeTemplateDetailsVO>();
            objDischargeModify = new clsDischargeTemplateMasterVO();

            MasterList = new PagedSortableCollectionView<clsDischargeTemplateMasterVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGridPager.DataContext = MasterList;
            this.dgDischargeTemplateDetailsList.DataContext = MasterList;

            BindGridList();
            SetCommandButtonState("Load");
            if (dgDischargeTemplateDetailsList != null)
            {
                if (dgDischargeTemplateDetailsList.Columns.Count > 0)
                {

                    dgDischargeTemplateDetailsList.Columns[0].Header = "View";
                    dgDischargeTemplateDetailsList.Columns[1].Header = "Code";
                    dgDischargeTemplateDetailsList.Columns[2].Header = "Name";
                    dgDischargeTemplateDetailsList.Columns[3].Header = "Status";
                }
            }
            if (dgDischargeDetails != null)
            {
                if (dgDischargeDetails.Columns.Count > 0)
                {
                    dgDischargeDetails.Columns[0].Header = "Field Name";
                    dgDischargeDetails.Columns[1].Header = "Parameter";
                    dgDischargeDetails.Columns[2].Header = "Font";
                    dgDischargeDetails.Columns[3].Header = "Control Binding";
                }
            }

        }

        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            BindGridList();
        }

        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdNew.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    txtNameDisCharge.Focus();
                    IsCancel = true;
                    break;
                case "New":
                    cmdSave.IsEnabled = true;
                    cmdAdd.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    cmdModify.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    break;
                case "Save":
                    cmdAdd.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdNew.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Modify":
                    cmdAdd.IsEnabled = false;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Cancel":
                    cmdAdd.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;
                case "Add":
                    cmdAdd.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    break;
                case "View":
                    cmdModify.IsEnabled = true;
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdAdd.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                default:
                    break;
            }
        }

        #endregion

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            this.DataContext = null;
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = "";
            ClearControl();
            objAnimation.Invoke(RotationType.Backward);
            if (IsCancel == true)
            {
                mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Admission Configuration";
                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmIPDAdmissionConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            else
            {
                IsCancel = true;
            }
        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("New");
            objAnimation.Invoke(RotationType.Forward);
            IsCancel = false;
            IsModify = false;
            ClearControl();
            CheckValidations();
            txtCode.Focus();
            dgDischargeDetails.ItemsSource = null;
            objDischargeDetailsList = new List<clsDischargeTemplateDetailsVO>();
        }

        private void ClearControl()
        {
            txtCode.Text = string.Empty;
            txtName.Text = string.Empty;
            txtFieldName.Text = string.Empty;
            cmbParameter.SelectedItem = ((List<MasterListItem>)cmbParameter.ItemsSource)[0];
            dgDischargeDetails.ItemsSource = null;
            List<clsDischargeTemplateDetailsVO> objDischargeDetailsList = new List<clsDischargeTemplateDetailsVO>();
            richTextBox.Html = Htmlrichtexbox;
            richTextBox.Html = string.Empty;
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFieldName.Text))
            {
                if (CheckDuplicy())
                {
                    txtFieldName.ClearValidationError();

                    clsDischargeTemplateDetailsVO objDischarge = new clsDischargeTemplateDetailsVO();
                    objDischarge.FieldName = txtFieldName.Text.Trim();
                    if (((MasterListItem)cmbParameter.SelectedItem).ID != 0)
                        objDischarge.ParameterName = cmbParameter.Text.Trim();
                    else
                        objDischarge.ParameterName = string.Empty;
                    objDischarge.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;

                    if (cmbFont.SelectedItem != null)
                    {
                        if (((MasterListItem)cmbFont.SelectedItem).ID > 0)
                        {
                            objDischarge.ApplicableFont = ((MasterListItem)cmbFont.SelectedItem).Description;
                        }
                    }

                    objDischarge.BindingControl = ((MasterListItem)cmbBindingControl.SelectedItem).ID;

                    if (objDischargeDetailsList == null)
                        objDischargeDetailsList = new List<clsDischargeTemplateDetailsVO>();

                    objDischargeDetailsList.Add(objDischarge);
                    dgDischargeDetails.ItemsSource = null;
                    dgDischargeDetails.ItemsSource = objDischargeDetailsList;

                    txtFieldName.Text = string.Empty;
                    cmbParameter.SelectedItem = ((List<MasterListItem>)cmbParameter.ItemsSource)[0];
                }
                else
                {
                    msgText = "Parameter Name is already exist.";
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            }
            else
            {
                msgText = "Please enter Field Name.";
                txtFieldName.SetValidation(msgText);
                txtFieldName.RaiseValidationError();
                txtFieldName.Focus();
            }
        }

        public bool CheckDuplicy()
        {
            bool result = true;

            if (cmbParameter.SelectedItem != null)
            {
                if (((MasterListItem)cmbParameter.SelectedItem).ID != 0)
                {
                    if (objDischargeDetailsList != null)
                    {
                        if (objDischargeDetailsList.SingleOrDefault(S => S.ParameterName.Equals(cmbParameter.Text.Trim())) == null)
                        {
                        }
                        else
                        {
                            result = false;
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(txtFieldName.Text))
            {
                if (objDischargeDetailsList.SingleOrDefault(S => S.FieldName.Equals(txtFieldName.Text.Trim())) == null)
                {
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }

        private void FillParameter()
        {
            try
            {
                List<MasterListItem> objList = new List<MasterListItem>();
                objList.Add(new MasterListItem(0, "- Select -"));
                objList.Add(new MasterListItem((int)DischargeParameter.Clinical_Findings, DischargeParameter.Clinical_Findings.ToString()));
                objList.Add(new MasterListItem((int)DischargeParameter.Diagnosis, DischargeParameter.Diagnosis.ToString()));
                objList.Add(new MasterListItem((int)DischargeParameter.Medication, DischargeParameter.Medication.ToString()));
                objList.Add(new MasterListItem((int)DischargeParameter.Advise, DischargeParameter.Advise.ToString()));
                objList.Add(new MasterListItem((int)DischargeParameter.Operating_Notes, DischargeParameter.Operating_Notes.ToString()));
                objList.Add(new MasterListItem((int)DischargeParameter.Note, DischargeParameter.Note.ToString()));
                objList.Add(new MasterListItem((int)DischargeParameter.Investigation, DischargeParameter.Investigation.ToString()));
                objList.Add(new MasterListItem((int)DischargeParameter.FollowUp, DischargeParameter.FollowUp.ToString()));
                objList.Add(new MasterListItem((int)DischargeParameter.Remark, DischargeParameter.Remark.ToString()));

                cmbParameter.ItemsSource = null;
                cmbParameter.ItemsSource = objList;

                cmbParameter.SelectedValue = objList[0].ID;
            }
            catch (Exception)
            {
                ///Indicatior.Close();
                // throw;
            }
        }

        public void FillFontComboBox()
        {
            List<MasterListItem> FontList = new List<MasterListItem>();
            FontList.Add(new MasterListItem(0, "- Select -"));
            FontList.Add(new MasterListItem(1, "Arial"));
            FontList.Add(new MasterListItem(2, "Times New Roman"));
            FontList.Add(new MasterListItem(3, "Verdana"));
            FontList.Add(new MasterListItem(4, "Kruti Dev 010"));
            FontList.Add(new MasterListItem(5, "Shivaji05"));

            cmbFont.ItemsSource = null;
            cmbFont.ItemsSource = FontList;
            cmbFont.SelectedItem = FontList[0];
        }

        private void FillBindingControl()
        {
            Type typeDepartment = typeof(BindingControl);
            FieldInfo[] arrDeptFiledValues = typeDepartment.GetFields(BindingFlags.Public | BindingFlags.Static);

            BindingControlList.Add(new MasterListItem(0, "--Select--"));

            foreach (var test in arrDeptFiledValues)
            {
                BindingControl TT = (BindingControl)Enum.Parse(typeof(BindingControl), Convert.ToString(test.GetValue(null)), true);
                BindingControlList.Add(new MasterListItem((int)TT, test.GetValue(null).ToString()));
            }
            cmbBindingControl.ItemsSource = null;
            cmbBindingControl.ItemsSource = BindingControlList;
            cmbBindingControl.SelectedItem = BindingControlList[0];
        }

        private void cmbBindingControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbBindingControl.SelectedItem != null)
            {
                if (((MasterListItem)cmbBindingControl.SelectedItem).ID == 3)
                {
                    cmbParameter.IsEnabled = true;
                    cmbFont.IsEnabled = true;
                }
                else
                {
                    cmbParameter.IsEnabled = false;
                    cmbFont.IsEnabled = false;
                }
            }
        }

        private void hlbDeleteField_Click(object sender, RoutedEventArgs e)
        {
            msgText = "Are you sure you want to Delete the Field ?";
            MessageBoxControl.MessageBoxChildWindow msgW =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
            msgW.Show();
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsDischargeTemplateDetailsVO objDischarge = (clsDischargeTemplateDetailsVO)dgDischargeDetails.SelectedItem;
                objDischargeDetailsList.Remove(objDischarge);
                dgDischargeDetails.ItemsSource = null;
                dgDischargeDetails.ItemsSource = objDischargeDetailsList;
            }
        }

        private void txtFieldName_LostFocus(object sender, RoutedEventArgs e)
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
                }
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidations())
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Are you sure you want to save ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnSaveMessageBoxClosed);

                msgW.Show();
            }
        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidations())
            {
                msgText = "Are you sure you want to update ?";
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnSaveMessageBoxClosed);

                msgW.Show();
            }
        }

        void msgW_OnSaveMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                Save();
            }
        }

        private void Save()
        {
            //if (chkIsTextTemplate.IsChecked == true)
            //{
            string str = richTextBox.Html;
            //}

            clsAddDischargeTemplateMasterBizActionVO bizActionVO = new clsAddDischargeTemplateMasterBizActionVO();
            bizActionVO.DischargeTemplateMaster = new clsDischargeTemplateMasterVO();
            bizActionVO.DischargeTemplateDetailsList = new List<clsDischargeTemplateDetailsVO>();

            bizActionVO.DischargeTemplateMaster.Code = txtCode.Text.Trim();
            bizActionVO.DischargeTemplateMaster.Description = txtName.Text.Trim();
            //bizActionVO.DischargeTemplateMaster.IsTextTemplate = (bool)chkIsTextTemplate.IsChecked;
            bizActionVO.DischargeTemplateMaster.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            bizActionVO.DischargeTemplateMaster.Status = true;
            bizActionVO.IsCheckBox = false;
            if (IsModify == false)
            {
                bizActionVO.IsModify = false;
                bizActionVO.DischargeTemplateMaster.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                bizActionVO.DischargeTemplateMaster.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                bizActionVO.DischargeTemplateMaster.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                bizActionVO.DischargeTemplateMaster.AddedDateTime = System.DateTime.Now;
                bizActionVO.DischargeTemplateMaster.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
            }
            else if (IsModify == true)
            {
                bizActionVO.IsModify = true;
                bizActionVO.DischargeTemplateMaster.ID = objDischargeModify.ID;
                bizActionVO.DischargeTemplateMaster.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                bizActionVO.DischargeTemplateMaster.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                bizActionVO.DischargeTemplateMaster.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                bizActionVO.DischargeTemplateMaster.UpdatedDateTime = System.DateTime.Now;
                bizActionVO.DischargeTemplateMaster.UpdatedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
            }
            //if (chkIsTextTemplate.IsChecked.Equals(true))
            //{
            clsDischargeTemplateDetailsVO objItem = new clsDischargeTemplateDetailsVO();
            objItem.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
            objItem.TextData = richTextBox.Html;
            bizActionVO.DischargeTemplateDetailsList.Add(objItem);
            //}
            //else if (chkIsTextTemplate.IsChecked.Equals(false))
            //{
            //    if (objDischargeDetailsList != null)
            //    {
            //        bizActionVO.DischargeTemplateDetailsList = objDischargeDetailsList;
            //    }
            //}
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (IsModify == false)
                    {
                        if (((clsAddDischargeTemplateMasterBizActionVO)arg.Result).SuccessStatus == 1)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Record Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                            SetCommandButtonState("Save");
                            ClearControl();
                            BindGridList();
                            objAnimation.Invoke(RotationType.Backward);
                        }
                        else if (((clsAddDischargeTemplateMasterBizActionVO)arg.Result).SuccessStatus == 2)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because CODE already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                            msgW1.Show();
                        }
                        else if (((clsAddDischargeTemplateMasterBizActionVO)arg.Result).SuccessStatus == 3)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because DESCRIPTION already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                            msgW1.Show();
                        }

                    }
                    else if (IsModify == true)
                    {
                        if (((clsAddDischargeTemplateMasterBizActionVO)arg.Result).SuccessStatus == 1)
                        {
                            SetCommandButtonState("Modify");
                            ClearControl();
                            BindGridList();
                            objAnimation.Invoke(RotationType.Backward);
                            msgText = "Record Updated Successfully.";
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            objAnimation.Invoke(RotationType.Backward);
                        }
                        else if (((clsAddDischargeTemplateMasterBizActionVO)arg.Result).SuccessStatus == 2)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because CODE already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                            msgW1.Show();
                        }
                        else if (((clsAddDischargeTemplateMasterBizActionVO)arg.Result).SuccessStatus == 3)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because DESCRIPTION already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                            msgW1.Show();
                        }
                    }
                }
                else
                {
                    SetCommandButtonState("New");
                    msgText = "Error occurred while processing.";
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }
            };
            client.ProcessAsync(bizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private bool CheckValidations()
        {
            bool result = true;
            if (string.IsNullOrEmpty(txtName.Text))
            {
                msgText = "Please enter Name.";
                txtName.SetValidation(msgText);
                txtName.RaiseValidationError();
                txtName.Focus();
                result = false;
            }
            else
            {
                txtName.ClearValidationError();
            }

            if (string.IsNullOrEmpty(txtCode.Text))
            {
                msgText = "Please enter code.";
                txtCode.SetValidation(msgText);
                txtCode.RaiseValidationError();
                txtCode.Focus();
                result = false;
            }
            else
            {
                txtCode.ClearValidationError();
            }
            if (richTextBox.Html == null || richTextBox.Html.Equals(string.Empty))
            {
                msgText = "Please add data to Template";
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                result = false;
            }

            return result;
        }

        clsDischargeTemplateMasterVO objDischargeModify = null;
        private void cmdViewDischarge_Click(object sender, RoutedEventArgs e)
        {
            ClearControl();
            CheckValidations();
            IsModify = true;
            SetCommandButtonState("View");
            cmdModify.IsEnabled = ((clsDischargeTemplateMasterVO)dgDischargeTemplateDetailsList.SelectedItem).Status;

            objDischargeModify = (clsDischargeTemplateMasterVO)dgDischargeTemplateDetailsList.SelectedItem;
            txtCode.Text = objDischargeModify.Code;
            txtName.Text = objDischargeModify.Description;

            //if (objDischargeModify.IsTextTemplate == true)
            //{
            //chkIsTextTemplate.IsChecked = true;
            grdRichTextBox.Visibility = Visibility.Visible;
            //grdFieldName.Visibility = Visibility.Collapsed;
            richTextBox.Html = objDischargeModify.DischargeTemplateDetails.TextData;
            //}
            //else if (objDischargeModify.IsTextTemplate == false)
            //{
            //    chkIsTextTemplate.IsChecked = false;
            //    grdRichTextBox.Visibility = Visibility.Collapsed;
            //    grdFieldName.Visibility = Visibility.Visible;
            //    objDischargeDetailsList = objDischargeModify.DischargeTemplateDetailsList;
            //    dgDischargeDetails.ItemsSource = null;
            //    dgDischargeDetails.ItemsSource = objDischargeDetailsList;
            //}

            //CheckCountDischarge();
            objAnimation.Invoke(RotationType.Forward);
        }

        private void CheckCountDischarge()
        {
            clsGetDischargeTemplateMasterListBizActionVO BizAction = new clsGetDischargeTemplateMasterListBizActionVO();
            BizAction.ID = objDischargeModify.ID;
            BizAction.UnitID = objDischargeModify.UnitID;
            BizAction.IsViewClick = true;

            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsGetDischargeTemplateMasterListBizActionVO)args.Result).Count > 0)
                        {
                            cmdModify.IsEnabled = false;
                        }
                    }
                };
                client.ProcessAsync(BizAction, User);//new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
            }
        }

        private void BindGridList()
        {
            clsGetDischargeTemplateMasterListBizActionVO BizAction = new clsGetDischargeTemplateMasterListBizActionVO();
            BizAction.DischargeTemplateMasterList = new List<clsDischargeTemplateMasterVO>();
            BizAction.SearchExpression = txtNameDisCharge.Text;
            BizAction.PagingEnabled = true;
            BizAction.MaximumRows = MasterList.PageSize;
            BizAction.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        BizAction.DischargeTemplateMasterList = (((clsGetDischargeTemplateMasterListBizActionVO)args.Result).DischargeTemplateMasterList);
                        MasterList.TotalItemCount = (int)(((clsGetDischargeTemplateMasterListBizActionVO)args.Result).TotalRows);
                        MasterList.Clear();

                        foreach (clsDischargeTemplateMasterVO item in BizAction.DischargeTemplateMasterList)
                        {
                            MasterList.Add(item);
                        }
                        dgDischargeTemplateDetailsList.ItemsSource = null;
                        dgDischargeTemplateDetailsList.ItemsSource = MasterList;
                        dataGridPager.Source = null;
                        dataGridPager.Source = MasterList;
                        dgDischargeTemplateDetailsList.SelectedItem = null;
                    }
                };
                client.ProcessAsync(BizAction, User);//new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            BindGridList();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            clsAddDischargeTemplateMasterBizActionVO bizActionVO = new clsAddDischargeTemplateMasterBizActionVO();
            bizActionVO.DischargeTemplateMaster = new clsDischargeTemplateMasterVO();
            bizActionVO.DischargeTemplateDetailsList = new List<clsDischargeTemplateDetailsVO>();

            if (dgDischargeTemplateDetailsList.SelectedItem != null)
            {
                try
                {
                    bizActionVO.IsCheckBox = true;
                    bizActionVO.DischargeTemplateMaster = (clsDischargeTemplateMasterVO)dgDischargeTemplateDetailsList.SelectedItem;
                    bizActionVO.DischargeTemplateMaster.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
                    bizActionVO.DischargeTemplateMaster.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    bizActionVO.DischargeTemplateMaster.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    bizActionVO.DischargeTemplateMaster.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    bizActionVO.DischargeTemplateMaster.UpdatedDateTime = System.DateTime.Now;
                    bizActionVO.DischargeTemplateMaster.UpdatedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            msgText = "Status Updated Successfully.";
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                        }
                        else
                        {
                            SetCommandButtonState("View");
                            msgText = "Error occurred while processing.";
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }

                    };
                    client.ProcessAsync(bizActionVO, new clsUserVO());
                    client.CloseAsync();
                }
                catch (Exception ex)
                {
                }
            }
        }

        //private void chkIsTextTemplate_Click(object sender, RoutedEventArgs e)
        //{
        //    if (chkIsTextTemplate.IsChecked == true)
        //    {
        //        grdRichTextBox.Visibility = Visibility.Visible;
        //        grdFieldName.Visibility = Visibility.Collapsed;
        //    }
        //    else
        //    {
        //        objDischargeDetailsList = new List<clsDischargeTemplateDetailsVO>();
        //        grdRichTextBox.Visibility = Visibility.Collapsed;
        //        grdFieldName.Visibility = Visibility.Visible;
        //    }
        //}

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private bool CodeCheckDuplicasy()
        {
            clsDischargeTemplateMasterVO codeItem = null;
            clsDischargeTemplateMasterVO descriptionItem = null;
            bool result = true;
            codeItem = ((PagedSortableCollectionView<clsDischargeTemplateMasterVO>)dgDischargeTemplateDetailsList.ItemsSource).FirstOrDefault(p => p.Code.Equals(txtCode.Text));
            descriptionItem = ((PagedSortableCollectionView<clsDischargeTemplateMasterVO>)dgDischargeTemplateDetailsList.ItemsSource).FirstOrDefault(p => p.Description.Equals(txtName.Text));
            if (codeItem != null)
            {
                msgText = "Code is already exist";
                txtCode.SetValidation(msgText);
                txtCode.RaiseValidationError();
                result = false;
            }
            else
            {
                txtCode.ClearValidationError();
            }
            if (descriptionItem != null)
            {
                msgText = "Description is already exist";
                txtName.SetValidation(msgText);
                txtName.RaiseValidationError();
                result = false;
            }
            else
            {
                txtName.ClearValidationError();
            }
            return result;

            //clsDischargeTemplateMasterVO Item = null;
            //if (!string.IsNullOrEmpty(txtCode.Text))
            //{
            //    if (IsModify == false)
            //    {
            //        Item = ((PagedSortableCollectionView<clsDischargeTemplateMasterVO>)dgDischargeTemplateDetailsList.ItemsSource).FirstOrDefault(p => p.Code.Equals(txtCode.Text));
            //    }
            //    else if (IsModify == true)
            //    {
            //        if (objDischargeModify.Code != txtCode.Text.Trim())
            //        {
            //            Item = ((PagedSortableCollectionView<clsDischargeTemplateMasterVO>)dgDischargeTemplateDetailsList.ItemsSource).FirstOrDefault(p => p.Code.Equals(txtCode.Text));
            //        }
            //    }
            //}
            //if (Item != null)
            //{
            //    return false;
            //}

            //else
            //{
            //    return true;
            //}
        }

        private bool NameCheckDuplicasy()
        {
            clsDischargeTemplateMasterVO Item = null;
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                if (IsModify == false)
                {
                    Item = ((PagedSortableCollectionView<clsDischargeTemplateMasterVO>)dgDischargeTemplateDetailsList.ItemsSource).FirstOrDefault(p => p.Description.Equals(txtName.Text));
                }
                else if (IsModify == true)
                {
                    if (objDischargeModify.Description != txtName.Text.Trim())
                    {
                        Item = ((PagedSortableCollectionView<clsDischargeTemplateMasterVO>)dgDischargeTemplateDetailsList.ItemsSource).FirstOrDefault(p => p.Description.Equals(txtName.Text));
                    }
                }
            }

            if (Item != null)
            {
                return false;
            }

            else
            {
                return true;
            }
        }

        private void txtCode_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
            //{
            //    if (!((TextBox)e.OriginalSource).Text.IsItSpecialChar())
            //    {
            //        ((TextBox)e.OriginalSource).Text = string.Empty;
            //        ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Red);
            //        string msgTitle = "";
            //        string msgText = "Special characters are not allowed.";
            //        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //        msgWindow.Show();
            //    }
            //    else
            //    {
            //        ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Gray);

            //        if (CodeCheckDuplicasy() == false)
            //        {
            //            cmdSave.IsEnabled = false;
            //            cmdModify.IsEnabled = false;
            //            msgText = "Code is already exist";
            //            txtCode.SetValidation(msgText);
            //            txtCode.RaiseValidationError();
            //        }
            //    }
            //}
        }

        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = CIMS.Comman.HandleAlphnumericAndSpecialChar(sender, e, false);
        }

        private void txtName_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
            //{
            //    if (!((TextBox)e.OriginalSource).Text.AllowSpecialChar())
            //    {
            //        ((TextBox)e.OriginalSource).Text = string.Empty;
            //        ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Red);
            //        msgText = "Only & ,.,- and space special characters are allowed.";
            //        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //        msgWindow.Show();
            //    }
            //    else
            //    {
            //        ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Gray);
            //        if (NameCheckDuplicasy() == false)
            //        {
            //            msgText = "Description is already exist";
            //            txtName.SetValidation(msgText);
            //            txtName.RaiseValidationError();
            //        }

            //    }
            //}
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = CIMS.Comman.HandleAlphnumericAndSpecialChar(sender, e, true);
        }

        private void txtNameDisCharge_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BindGridList();
                dataGridPager.PageIndex = 0;
            }
        }


    }
}
