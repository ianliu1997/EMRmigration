using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using CIMS;
using PalashDynamics.Collections;
using System.Reflection;
using PalashDynamics.UserControls;

namespace PalashDynamics.Administration
{
    public partial class PatientConsentMaster : UserControl
    {
        public PatientConsentMaster()
        {
            InitializeComponent();
            objWIndicator = new WaitIndicator();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            //Paging
            DataList = new PagedSortableCollectionView<clsPatientConsentVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
        }

        WaitIndicator objWIndicator = null;
        private SwivelAnimation objAnimation;
        bool IsCancel = true;

        #region Paging

        public PagedSortableCollectionView<clsPatientConsentVO> DataList { get; private set; }


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


        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();

        }
        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Load");
            FillField();
            FillDepartmentList(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId);
            this.DataContext = new clsPatientConsentVO();
            FetchData();
        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("New");
            ClearData();
            ChkValidation(false);
            txtCode.Focus();
            objAnimation.Invoke(RotationType.Forward);
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = "";

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
            objAnimation.Invoke(RotationType.Backward);
        }

        #region FillComboBox
        private void FillField()
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

                        clsPatientFieldsConfigVO obj = new clsPatientFieldsConfigVO();
                        obj.ID = 0;
                        obj.FieldName = "--Select--";
                        objList.Add(obj);
                        objList.AddRange(((clsGetPatientConfigFieldsBizActionVO)args.Result).OtPateintConfigFieldsMatserDetails);

                        cmbField.ItemsSource = null;
                        cmbField.ItemsSource = objList;
                        cmbField.SelectedItem = objList[0];
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

        private void FillDepartmentList(long iUnitId)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_DepartmentUnitView;

            if (iUnitId > 0)
                BizAction.Parent = new KeyValue { Key = iUnitId, Value = "UnitId" };
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    cmbDepartment.ItemsSource = null;
                    cmbDepartment.ItemsSource = objList;
                    cmbDepartment.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {
                        cmbDepartment.SelectedValue = ((clsPatientConsentVO)this.DataContext).DepartmentID;
                    }

                }

            };


            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        #endregion

        #region SaveData
        private void Save()
        {
            clsAddPatientConsentBizActionVO BizAction = new clsAddPatientConsentBizActionVO();

            try
            {
                BizAction.ConsentDetails = (clsPatientConsentVO)this.DataContext;
                BizAction.ConsentDetails.Template = richTextEditor.Html;

                if (cmbDepartment.SelectedItem != null)
                    BizAction.ConsentDetails.DepartmentID = ((MasterListItem)cmbDepartment.SelectedItem).ID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, args) =>
                {

                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsAddPatientConsentBizActionVO)args.Result).SuccessStatus == 1)
                        {
                            ClearData();
                            FetchData();
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Patient consent added successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            objAnimation.Invoke(RotationType.Backward);
                            SetCommandButtonState("Save");
                        }
                        else if (((clsAddPatientConsentBizActionVO)args.Result).SuccessStatus == 2)
                        {

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because CODE already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                        }
                        else if (((clsAddPatientConsentBizActionVO)args.Result).SuccessStatus == 3)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because DESCRIPTION already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                        }

                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;

            }

        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (ChkValidation(true))
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to save the patient consent ?";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);

                msgW1.Show();
            }

        }
        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Save();
        }

        #endregion

        private void lnkAddItems_Click(object sender, RoutedEventArgs e)
        {
            if (cmbField.SelectedItem != null && ((clsPatientFieldsConfigVO)cmbField.SelectedItem).ID != 0)
            {
                try
                {
                    //Point ObjPoint = richTextEditor.CusrsorPosition;
                    //richTextEditor.SelectAll();

                    string str1 = richTextEditor.SelectedText;
                    string str2 = " {" + cmbField.SelectedItem.ToString() + "}";

                    richTextEditor.SelectedText = string.Empty;

                    String html = richTextEditor.Html;
                    string s = richTextEditor.Text;
                    if (richTextEditor.Html != null)
                    {

                        if (richTextEditor.Html == "")
                            richTextEditor.Html = "<p style=\"margin:0px;\"><span class=\"Normal\">" + str2 + "</span></p>\r";
                        else
                        {
                            string find = html.Insert(html.LastIndexOf("</span>") + 7, str2);

                            richTextEditor.Html = find;
                        }

                    }

                }
                catch (Exception)
                {
                    throw;
                }
            }

        }

        #region Get Data
        private void FetchData()
        {
            clsGetPatientConsentMasterBizActionVO BizAction = new clsGetPatientConsentMasterBizActionVO();
            try
            {
                if (objWIndicator != null)
                    objWIndicator.Show();
                BizAction.ConsentMatserDetails = new List<clsPatientConsentVO>();
                BizAction.SearchExpression = txtSearch.Text;
                BizAction.PagingEnabled = true;
                BizAction.MaximumRows = DataList.PageSize;
                BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    objWIndicator.Close();
                    if (arg.Error == null)
                    {
                        if (((clsGetPatientConsentMasterBizActionVO)arg.Result).ConsentMatserDetails != null)
                        {
                            clsGetPatientConsentMasterBizActionVO result = arg.Result as clsGetPatientConsentMasterBizActionVO;

                            DataList.TotalItemCount = result.TotalRows;

                            if (result.ConsentMatserDetails != null)
                            {
                                DataList.Clear();

                                foreach (var item in result.ConsentMatserDetails)
                                {
                                    DataList.Add(item);
                                }

                                dgPatientConsent.ItemsSource = null;
                                dgPatientConsent.ItemsSource = DataList;

                                dataGrid2Pager.Source = null;
                                dataGrid2Pager.PageSize = BizAction.MaximumRows;
                                dataGrid2Pager.Source = DataList;

                            }
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }

                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                objWIndicator.Close();
                throw;
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
        }

        #endregion

        #region View data
        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
            SetCommandButtonState("View");
            if (dgPatientConsent.SelectedItem != null)
            {
                this.DataContext = dgPatientConsent.SelectedItem;
                cmbDepartment.SelectedValue = ((clsPatientConsentVO)this.DataContext).DepartmentID;
                richTextEditor.Html = ((clsPatientConsentVO)this.DataContext).Template;
            }
            objAnimation.Invoke(RotationType.Forward);
        }

        #endregion

        #region Modify
        private void Modify()
        {
            clsAddPatientConsentBizActionVO BizAction = new clsAddPatientConsentBizActionVO();

            try
            {
                BizAction.ConsentDetails = (clsPatientConsentVO)this.DataContext;
                BizAction.ConsentDetails.Template = richTextEditor.Html;

                if (cmbDepartment.SelectedItem != null)
                    BizAction.ConsentDetails.DepartmentID = ((MasterListItem)cmbDepartment.SelectedItem).ID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, args) =>
                {
                    SetCommandButtonState("Modify");
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsAddPatientConsentBizActionVO)args.Result).SuccessStatus == 1)
                        {

                            ClearData();
                            FetchData();

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Patient consent updated successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                            objAnimation.Invoke(RotationType.Backward);
                        }
                        else if (((clsAddPatientConsentBizActionVO)args.Result).SuccessStatus == 2)
                        {

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because CODE already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                        }
                        else if (((clsAddPatientConsentBizActionVO)args.Result).SuccessStatus == 3)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because DESCRIPTION already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                        }

                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;

            }
        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (ChkValidation(true))
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to update the patient consent ?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();
            }
        }
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Modify();
        }

        #endregion

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

        #region Clear Data
        private void ClearData()
        {
            this.DataContext = new clsPatientConsentVO();
            cmbDepartment.SelectedValue = (long)0;
            cmbField.SelectedValue = (long)0;
            richTextEditor.Html = "";
        }

        #endregion

        #region Validation
        private bool ChkValidation(bool IsFromSave)
        {
            bool result = true;

            if (txtCode.Text == "")
            {
                txtCode.SetValidation("Please enter Code");
                txtCode.RaiseValidationError();
                txtCode.Focus();
                result = false;
            }
            else
                txtCode.ClearValidationError();

            if (txtDescription.Text == "")
            {
                txtDescription.SetValidation("Please enter Description");
                txtDescription.RaiseValidationError();
                txtDescription.Focus();
                result = false;
            }
            else
                txtDescription.ClearValidationError();

            if ((MasterListItem)cmbDepartment.SelectedItem == null)
            {
                if (cmbDepartment.TextBox != null)
                {
                    cmbDepartment.TextBox.SetValidation("Please Select Department");
                    cmbDepartment.TextBox.RaiseValidationError();
                }
                cmbDepartment.Focus();
                result = false;

            }
            else if (((MasterListItem)cmbDepartment.SelectedItem).ID == 0)
            {
                if (cmbDepartment.TextBox != null)
                {
                    cmbDepartment.TextBox.SetValidation("Please Select Department");
                    cmbDepartment.TextBox.RaiseValidationError();
                }
                cmbDepartment.Focus();
                result = false;

            }
            else
                cmbDepartment.TextBox.ClearValidationError();

            if (IsFromSave)
            {
                if (richTextEditor.Html == "")
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Please enter Template", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                    result = false;
                    return result;
                }
            }
            return result;
        }

        #endregion

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty((e.OriginalSource as TextBox).Text))
            {
                ((TextBox)e.OriginalSource).Text = ((TextBox)e.OriginalSource).Text.ToTitleCase();
            }
            //if (!string.IsNullOrEmpty(txtCode.Text))
            //{
            //    txtCode.Text = txtCode.Text.ToTitleCase();
            //}
            //if (!string.IsNullOrEmpty(txtDescription.Text))
            //{
            //    txtDescription.Text = txtDescription.Text.ToTitleCase();
            //}
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FetchData();
                dataGrid2Pager.PageIndex = 0;
            }

        }
    }
}
