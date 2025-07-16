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
using PalashDynamics.ValueObjects;
using PalashDynamics.Collections;
using System.Reflection;
using PalashDynamics.ValueObjects.Administration.IPD;
using PalashDynamics.Service.PalashTestServiceReference;

namespace PalashDynamics.Administration
{
    public partial class frmPatientVitalSMaster : UserControl
    {
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

        #region Properties
        public PagedSortableCollectionView<clsIPDPatientVitalsMasterVO> MasterList { get; private set; }
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

        private SwivelAnimation objAnimation;
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        bool IsCancel = true;
        private long VitalID;
        string msgText = "";
        string msgTitle = "PALASHDYNAMICS";
        public bool isModify = false;
        public bool IsLoad = true;
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        public List<clsIPDPatientVitalsMasterVO> VitalsList;
        public frmPatientVitalSMaster()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(IncomCategoryListLayoutRoot, NewIncomeCategoryLayoutRoot, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            this.Loaded += new RoutedEventHandler(frmPatientVitalSMaster_Loaded);

            MasterList = new PagedSortableCollectionView<clsIPDPatientVitalsMasterVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);

            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;

            this.grdVitalsMaster.DataContext = MasterList;
            VitalsList = new List<clsIPDPatientVitalsMasterVO>();
            SetupPage();
        }

        void MasterList_OnRefresh(Object sender, RefreshEventArgs e)
        {
            SetupPage();
        }

        void frmPatientVitalSMaster_Loaded(object sender, RoutedEventArgs e)
        {
            if (grdVitalsMaster != null)
            {
                if (grdVitalsMaster.Columns.Count > 0)
                {
                    grdVitalsMaster.Columns[0].Header = "View";
                    grdVitalsMaster.Columns[1].Header = "Code";
                    grdVitalsMaster.Columns[2].Header = "Description";
                    grdVitalsMaster.Columns[3].Header = "Default Value";
                    grdVitalsMaster.Columns[4].Header = "Min Value";
                    grdVitalsMaster.Columns[5].Header = "Max Value";
                    grdVitalsMaster.Columns[6].Header = "Unit";
                    grdVitalsMaster.Columns[7].Header = "Status";
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsLoad.Equals(true))
                SetCommandButtonPatientVitalMaster("Load");
            txtSearch.Focus();
            IsLoad = false;
        }

        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonPatientVitalMaster(String strFormMode)
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

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            Validation();
            SetCommandButtonPatientVitalMaster("New");
            this.DataContext = new clsIPDPatientVitalsMasterVO();
            ClearUI();
            IsCancel = false;
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "New Patient Vital Details";
            objAnimation.Invoke(RotationType.Forward);

        }

        public void ClearUI()
        {
            txtSubCode.Text = string.Empty;
            txtSubDescription.Text = string.Empty;
            txtDefaultValue.Text = string.Empty;
            txtMaxValue.Text = string.Empty;
            txtMinValue.Text = string.Empty;
            txtUnit.Text = string.Empty;

        }

        private bool CheckDuplicasy()
        {
            var Item = from r in VitalsList.ToList()
                       where r.Code.ToLower() == txtSubCode.Text.ToLower()
                       select r;
            var Item1 = from r1 in VitalsList.ToList()
                        where r1.Description.ToLower() == txtSubDescription.Text.ToLower()
                        select r1;
            if (Item != null && Item.ToList().Count > 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because CODE already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                return false;
            }
            if (Item1 != null && Item1.ToList().Count > 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because DESCRIPTION already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                return false;
            }
            else
            {
                return true;
            }
        }

        #region Validation
        private bool Validation()
        {
            bool result = true;
            if (string.IsNullOrEmpty(txtSubCode.Text))
            {
                msgText = "Please Enter Code";
                txtSubCode.SetValidation(msgText);
                txtSubCode.RaiseValidationError();
                txtSubCode.Focus();
                result = false;
            }
            else
                txtSubCode.ClearValidationError();

            if (string.IsNullOrWhiteSpace(txtSubDescription.Text))
            {
                msgText = "Please Enter Description";
                txtSubDescription.SetValidation(msgText);
                txtSubDescription.RaiseValidationError();
                txtSubDescription.Focus();
                result = false;
            }
            else
                txtSubDescription.ClearValidationError();

            if (string.IsNullOrEmpty(txtDefaultValue.Text))
            {
                msgText = "Please Enter Default Value";
                txtDefaultValue.SetValidation(msgText);
                txtDefaultValue.RaiseValidationError();
                txtDefaultValue.Focus();
                result = false;
            }
            else
                txtDefaultValue.ClearValidationError();

            if (string.IsNullOrEmpty(txtMinValue.Text))
            {
                msgText = "Please Enter MinValue";
                txtMinValue.SetValidation(msgText);
                txtMinValue.RaiseValidationError();
                txtMinValue.Focus();
                result = false;
            }
            else
                txtMinValue.ClearValidationError();

            if (string.IsNullOrEmpty(txtMaxValue.Text))
            {
                msgText = "Please Enter MaxValue";
                txtMaxValue.SetValidation(msgText);
                txtMaxValue.RaiseValidationError();
                txtMaxValue.Focus();
                result = false;
            }
            else
                txtMaxValue.ClearValidationError();
            return result;
        }

        //Added by Ashish Z.
        private bool CheckValue()
        {
            bool result = true;
            double dValue = Convert.ToDouble(txtDefaultValue.Text);
            double minValue = Convert.ToDouble(txtMinValue.Text);
            double maxValue = Convert.ToDouble(txtMaxValue.Text);
            string msgText1 = string.Empty;
            if (dValue > 0 && minValue > 0 && maxValue > 0)
            {
                if (minValue > maxValue)
                {
                    msgText1 = "Please Enter MinValue less than MaxValue";
                    result = false;
                }
                else if (minValue == maxValue)
                {
                    msgText1 = "You cannot enter same value for MinValue and MaxValue";
                    result = false;
                }
                else if (dValue > maxValue)
                {
                    msgText1 = "Please enter Default Value in between MinValue and MaxValue";
                    result = false;
                }
                else if (dValue < minValue)
                {
                    msgText1 = "Please enter Default Value in between MinValue and MaxValue";
                    result = false;
                }
                if (!result)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", msgText1, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            }
            return result;
        }
        //
        #endregion

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                if (CheckValue())
                {
                    string msgTitle = "";
                    msgText = "Are you sure you want to Save ?";
                    MessageBoxControl.MessageBoxChildWindow msgWin =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);
                    msgWin.Show();
                }
            }
        }

        void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                try
                {
                    if (CheckDuplicasy())
                    {
                        SaveVitalsMaster();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }

        }

        private clsIPDPatientVitalsMasterVO CreateFormData()
        {
            clsIPDPatientVitalsMasterVO addPatientVitalVO = new clsIPDPatientVitalsMasterVO();
            if (isModify == true)
            {
                addPatientVitalVO.ID = VitalID;
                addPatientVitalVO = ((clsIPDPatientVitalsMasterVO)grdVitalsMaster.SelectedItem);
            }
            else
            {
                addPatientVitalVO.ID = 0;
                addPatientVitalVO = (clsIPDPatientVitalsMasterVO)this.NewIncomeCategoryLayoutRoot.DataContext;
                addPatientVitalVO.Status = true;
            }

            return addPatientVitalVO;
        }

        private void SaveVitalsMaster()
        {
            clsAddUpdateIPDPatientVitalsMasterBIzActionVO BizAction = new clsAddUpdateIPDPatientVitalsMasterBIzActionVO();

            clsIPDPatientVitalsMasterVO addSubDepartmentVO = new clsIPDPatientVitalsMasterVO();

            isModify = false;
            addSubDepartmentVO = (clsIPDPatientVitalsMasterVO)this.DataContext;
            addSubDepartmentVO = CreateFormData();
            addSubDepartmentVO.IsModify = false;
            addSubDepartmentVO.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            addSubDepartmentVO.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            addSubDepartmentVO.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
            addSubDepartmentVO.AddedDateTime = System.DateTime.Now;
            addSubDepartmentVO.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;

            BizAction.PatientVitalDetailList.Add(addSubDepartmentVO);


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if ((clsAddUpdateIPDPatientVitalsMasterBIzActionVO)arg.Result != null)
                    {
                        SetupPage();
                        ClearUI();
                        objAnimation.Invoke(RotationType.Backward);
                        msgText = "Record Saved Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        SetCommandButtonPatientVitalMaster("Save");
                    }
                }


                else
                {
                    msgText = "Error occurred while processing.";
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }


            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void SetupPage()
        {
            clsGetIPDPatientVitalsMasterBIzActionVO BizAction = new clsGetIPDPatientVitalsMasterBIzActionVO();
            BizAction.VitalDetailsList = new List<clsIPDPatientVitalsMasterVO>();
            BizAction.SearchExpression = txtSearch.Text;
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
                        BizAction.VitalDetailsList = (((clsGetIPDPatientVitalsMasterBIzActionVO)args.Result).VitalDetailsList);
                        MasterList.Clear();
                        VitalsList = new List<clsIPDPatientVitalsMasterVO>();
                        VitalsList = ((clsGetIPDPatientVitalsMasterBIzActionVO)args.Result).VitalDetailsList;
                        MasterList.TotalItemCount = (int)(((clsGetIPDPatientVitalsMasterBIzActionVO)args.Result).TotalRows);
                        foreach (clsIPDPatientVitalsMasterVO item in BizAction.VitalDetailsList)
                        {
                            MasterList.Add(item);
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

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {

            SetCommandButtonPatientVitalMaster("Cancel");
            this.DataContext = null;

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = "";

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

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            MasterList = new PagedSortableCollectionView<clsIPDPatientVitalsMasterVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdVitalsMaster.DataContext = MasterList;

            SetupPage();
        }
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                if (CheckValue())
                {
                    string msgTitle = "";
                    msgText = "Are you sure you want to Update ?";
                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                    msgW.Show();
                }
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    Modify();
                    SetCommandButtonPatientVitalMaster("Modify");
                }

                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        private void Modify()
        {
            clsAddUpdateIPDPatientVitalsMasterBIzActionVO BizAction = new clsAddUpdateIPDPatientVitalsMasterBIzActionVO();
            clsIPDPatientVitalsMasterVO addSubDepartmentVO = new clsIPDPatientVitalsMasterVO();
            try
            {
                isModify = true;
                addSubDepartmentVO = (clsIPDPatientVitalsMasterVO)this.DataContext;
                addSubDepartmentVO = CreateFormData();



                addSubDepartmentVO.IsModify = true;
                addSubDepartmentVO.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                addSubDepartmentVO.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                addSubDepartmentVO.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                addSubDepartmentVO.AddedDateTime = System.DateTime.Now;
                addSubDepartmentVO.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                BizAction.PatientVitalDetailList.Add(addSubDepartmentVO);
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        SetupPage();
                        ClearUI();
                        objAnimation.Invoke(RotationType.Backward);
                        msgText = "Record Updated Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        SetCommandButtonPatientVitalMaster("Modify");
                    }
                    else
                    {
                        msgText = "Error occurred while processing.";
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

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Validation();
                SetCommandButtonPatientVitalMaster("View");
                cmdModify.IsEnabled = ((clsIPDPatientVitalsMasterVO)grdVitalsMaster.SelectedItem).Status;
                IsCancel = false;
                if (grdVitalsMaster.SelectedItem != null)
                {
                    VitalID = ((clsIPDPatientVitalsMasterVO)grdVitalsMaster.SelectedItem).ID;
                    NewIncomeCategoryLayoutRoot.DataContext = ((clsIPDPatientVitalsMasterVO)grdVitalsMaster.SelectedItem).DeepCopy();
                    objAnimation.Invoke(RotationType.Forward);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void StatusCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (grdVitalsMaster.SelectedItem != null)
            {
                clsUpdateStatusIPDPatientVitalsMasterBIzActionVO BizAction = new clsUpdateStatusIPDPatientVitalsMasterBIzActionVO();
                BizAction.VitalDetails = new clsIPDPatientVitalsMasterVO();
                BizAction.VitalDetails = ((clsIPDPatientVitalsMasterVO)grdVitalsMaster.SelectedItem);
                BizAction.VitalDetails.Status = true;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        SetCommandButtonPatientVitalMaster("Save");
                        objAnimation.Invoke(RotationType.Backward);
                        msgText = "Status Updated Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        SetupPage();
                    }
                    else
                    {
                        SetCommandButtonPatientVitalMaster("New");
                        msgText = "Error occurred while processing.";
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }

        }

        private void StatusCheckBox_UnChecked(object sender, RoutedEventArgs e)
        {

            if (grdVitalsMaster.SelectedItem != null)
            {
                clsUpdateStatusIPDPatientVitalsMasterBIzActionVO BizAction = new clsUpdateStatusIPDPatientVitalsMasterBIzActionVO();
                BizAction.VitalDetails = new clsIPDPatientVitalsMasterVO();
                BizAction.VitalDetails = ((clsIPDPatientVitalsMasterVO)grdVitalsMaster.SelectedItem);
                BizAction.VitalDetails.Status = false;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        SetCommandButtonPatientVitalMaster("Save");
                        objAnimation.Invoke(RotationType.Backward);
                        msgText = "Status Updated Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        SetupPage();
                    }
                    else
                    {
                        SetCommandButtonPatientVitalMaster("New");
                        msgText = "Error occurred while processing.";
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }

        }

        private void txtSubCode_KeyDown(object sender, KeyEventArgs e)
        {
            //e.Handled = CIMS.Comman.HandleAlphnumericAndSpecialChar(sender, e, false);
        }

        private void txtSubCode_LostFocus(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text)) ((TextBox)e.OriginalSource).Text = ((TextBox)e.OriginalSource).Text.ToTitleCase();
            //{
            //    if (!((TextBox)e.OriginalSource).Text.IsItSpecialChar())
            //    {
            //        ((TextBox)e.OriginalSource).Text = string.Empty;
            //        ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Red);
            //        msgText = "Special characters are not allowed.";
            //        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //        msgWindow.Show();
            //    }
            //    else
            //    {
            //        ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Gray);
            //    }
            //}
        }

        private void txtSubDescription_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = CIMS.Comman.HandleAlphnumericAndSpecialChar(sender, e, true);
        }

        private void txtSubDescription_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text)) ((TextBox)e.OriginalSource).Text = ((TextBox)e.OriginalSource).Text.ToTitleCase();
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

            //    }
            //}
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                MasterList = new PagedSortableCollectionView<clsIPDPatientVitalsMasterVO>();
                MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                PageSize = 15;
                this.dataGrid2Pager.DataContext = MasterList;
                this.grdVitalsMaster.DataContext = MasterList;
                SetupPage();
            }
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            //textBefore = ((TextBox)sender).Text;
            //selectionStart = ((TextBox)sender).SelectionStart;
            //selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (!txtDefaultValue.Text.IsValueDouble())
            //{
            //    txtDefaultValue.Text = textBefore;
            //    txtDefaultValue.SelectionStart = selectionStart;
            //    txtDefaultValue.SelectionLength = selectionLength;
            //    textBefore = String.Empty;
            //    selectionStart = 0;
            //    selectionLength = 0;
            //}
        }

        private void txtMinValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (!txtMinValue.Text.IsValueDouble())
            //{
            //    txtMinValue.Text = textBefore;
            //    txtMinValue.SelectionStart = selectionStart;
            //    txtMinValue.SelectionLength = selectionLength;
            //    textBefore = String.Empty;
            //    selectionStart = 0;
            //    selectionLength = 0;
            //}
        }

        private void txtMaxValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (!txtMaxValue.Text.IsValueDouble())
            //{
            //    txtMaxValue.Text = textBefore;
            //    txtMaxValue.SelectionStart = selectionStart;
            //    txtMaxValue.SelectionLength = selectionLength;
            //    textBefore = String.Empty;
            //    selectionStart = 0;
            //    selectionLength = 0;
            //}
        }

        
    }
}

