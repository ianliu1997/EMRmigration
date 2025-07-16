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
using PalashDynamics.Collections;
using System.ComponentModel;
using System.Reflection;

namespace PalashDynamics.Administration
{
    public partial class SpecializationMaster : UserControl, INotifyPropertyChanged
    {

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

        #region Validation
        public bool Validation()
        {
            bool result = true;
            if (string.IsNullOrEmpty(txtSpecializationDescription.Text.Trim()))
            {
                txtSpecializationDescription.SetValidation("Please Enter Description");
                txtSpecializationDescription.RaiseValidationError();
                txtSpecializationDescription.Focus();
                result= false;
            }

            else
            {
                txtSpecializationDescription.ClearValidationError();
               // return true;
            }

            if (string.IsNullOrEmpty(txtSpecializationCode.Text.Trim()))
            {
                txtSpecializationCode.SetValidation("Please Enter Code");
                txtSpecializationCode.RaiseValidationError();
                txtSpecializationCode.Focus();
                result = false;
            }
            else
            {
                txtSpecializationCode.ClearValidationError();
              //  return true;
            }
            return result;
        }
        #endregion

        #region  Variables
        private SwivelAnimation objAnimation;
        string msgTitle = "PALASHDYNAMICS";
        string msgText = "";
        private long SpecializationId;
        public PagedSortableCollectionView<clsSpecializationVO> MasterList { get; private set; }
        bool IsCancel = true;
        public bool StatusCheck = true;
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

        #region Constructor
        public SpecializationMaster()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            this.Loaded += new RoutedEventHandler(SpecializationMaster_Loaded);
            SetCommandButtonState("Load");
            SpecializationId = 0;
            MasterList = new PagedSortableCollectionView<clsSpecializationVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdSpecialization.DataContext = MasterList;

            SetupPage();
        }


        #endregion Constructor

        #region On Refresh Event
        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            SetupPage();
        }
        #endregion

        #region Loaded Event
        void SpecializationMaster_Loaded(object sender, RoutedEventArgs e)
        {


        }
        #endregion Loaded Event

        #region Public Methods
        /// <summary>
        /// This Method Is Use For Two Purpose It Fill DataGrid (All Supplier Details) and 
        /// When We click On View Hyperlink Button Then It Will Get Details of Supplier on Which we Click  
        /// </summary>
        public void SetupPage()
        {

            clsGetSpecializationDetailsBizActionVO bizActionVO = new clsGetSpecializationDetailsBizActionVO();
            bizActionVO.SerachExpression = txtSearch.Text;
            bizActionVO.SpecializationId = SpecializationId;
            bizActionVO.PagingEnabled = true;
            bizActionVO.MaximumRows = MasterList.PageSize;
            bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;

            clsSpecializationVO getspecializationinfo = new clsSpecializationVO();
            bizActionVO.ItemMatserDetails = new List<clsSpecializationVO>();
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {

                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.ItemMatserDetails = (((clsGetSpecializationDetailsBizActionVO)args.Result).ItemMatserDetails);
                        ///Setup Page Fill DataGrid
                        if (SpecializationId == 0 && bizActionVO.ItemMatserDetails.Count > 0)
                        {
                            MasterList.Clear();
                            MasterList.TotalItemCount = (int)(((clsGetSpecializationDetailsBizActionVO)args.Result).TotalRows);
                            foreach (clsSpecializationVO item in bizActionVO.ItemMatserDetails)
                            {
                                MasterList.Add(item);
                            }
                            
                        }

                    }
                    txtSearch.Focus();
                };
                client.ProcessAsync(bizActionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {

            }

        }


        /// <summary>
        /// When We Click On Add Button All UI Must Empty
        /// </summary>
        public void ClearUI()
        {
            txtSpecializationCode.Text = "";
            txtSpecializationDescription.Text = "";
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

        #region Button Click Events

        /// <summary>
        /// This Event is Call When We click on Add Button, and show Back Panel Which Have  Specialization Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            Validation();
            SetCommandButtonState("New");
            ClearUI();
            grdBackPanel.DataContext = new clsSpecializationVO();
            try
            {
                objAnimation.Invoke(RotationType.Forward);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// This Event is Call When We click on Modify Button and Update  Specialization Details
        /// (For Add and Modify  Specialization Details, only One VO 
        /// and Procedure Which have Both Functionality)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

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

                #region Commented

                //try
                //{
                //    clsAddUpdateSpecializationBizActionVO bizactionVO = new clsAddUpdateSpecializationBizActionVO();
                //    clsSpecializationVO addNewspecializationVO = new clsSpecializationVO();

                //    addNewspecializationVO.SpecializationId = SpecializationId;
                //    addNewspecializationVO.Code = txtSpecializationCode.Text;
                //    addNewspecializationVO.SpecializationName = txtSpecializationDescription.Text;
                //    addNewspecializationVO.ClinicId = ((clsSpecializationVO)grdSpecialization.SelectedItem).ClinicId;
                //    addNewspecializationVO.Status = ((clsSpecializationVO)grdSpecialization.SelectedItem).Status;
                //    addNewspecializationVO.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //    addNewspecializationVO.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                //    addNewspecializationVO.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                //    addNewspecializationVO.UpdatedDateTime = System.DateTime.Now;
                //    addNewspecializationVO.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                //    bizactionVO.ItemMatserDetails.Add(addNewspecializationVO);
                //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //    client.ProcessCompleted += (s, args) =>
                //    {

                //        if (args.Error == null && args.Result != null)
                //        {

                //            if (((clsAddUpdateSpecializationBizActionVO)args.Result).SuccessStatus == 1)
                //            {
                //                msgText = "Record is successfully submitted!";

                //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //                msgWindow.Show();
                //                //After Updation Back to BackPanel and Setup Page
                //                objAnimation.Invoke(RotationType.Backward);
                //                SpecializationId = 0;
                //                SetupPage();

                //                SetCommandButtonState("Modify");

                //            }
                //            else if (((clsAddUpdateSpecializationBizActionVO)args.Result).SuccessStatus == 2)
                //            {
                //                msgText = "Record cannot be save because CODE already exist!";
                //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //                msgWindow.Show();
                //            }
                //            else if (((clsAddUpdateSpecializationBizActionVO)args.Result).SuccessStatus == 3)
                //            {
                //                msgText = "Record cannot be save because DESCRIPTION already exist!";
                //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //                msgWindow.Show();
                //            }

                //        }

                //    };
                //    client.ProcessAsync(bizactionVO, new clsUserVO());
                //    client.CloseAsync();
                //}
                //catch (Exception ex)
                //{

                //}
                #endregion
            }

            else
            {
                //txtSpecializationCode.Text = "";
                //txtSpecializationDescription.Text = "";
                Validation();
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    clsAddUpdateSpecializationBizActionVO bizactionVO = new clsAddUpdateSpecializationBizActionVO();
                    clsSpecializationVO addNewspecializationVO = new clsSpecializationVO();

                    addNewspecializationVO.SpecializationId = SpecializationId;
                    addNewspecializationVO.Code = txtSpecializationCode.Text;
                    addNewspecializationVO.SpecializationName = txtSpecializationDescription.Text;
                    addNewspecializationVO.IsGenerateToken = (bool)chkIsGenerateToken.IsChecked;
                    addNewspecializationVO.ClinicId = ((clsSpecializationVO)grdSpecialization.SelectedItem).ClinicId;
                    addNewspecializationVO.Status = ((clsSpecializationVO)grdSpecialization.SelectedItem).Status;
                    addNewspecializationVO.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewspecializationVO.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewspecializationVO.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewspecializationVO.UpdatedDateTime = System.DateTime.Now;
                    addNewspecializationVO.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.ItemMatserDetails.Add(addNewspecializationVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {

                            if (((clsAddUpdateSpecializationBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                msgText = "Record is successfully submitted!";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                //After Updation Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                SpecializationId = 0;
                                SetupPage();

                                SetCommandButtonState("Modify");

                            }
                            else if (((clsAddUpdateSpecializationBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                msgText = "Record cannot be save because CODE already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpdateSpecializationBizActionVO)args.Result).SuccessStatus == 3)
                            {
                                msgText = "Record cannot be save because DESCRIPTION already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }

                        }

                    };
                    client.ProcessAsync(bizactionVO, new clsUserVO());
                    client.CloseAsync();
                }
                catch (Exception ex)
                {

                }
            }
        }



        /// <summary>
        /// This Event is Call When We click on Save Button and Save  Specialization Details
        /// (For Add and Modify  Specialization Details, only One VO 
        /// and Procedure Which have Both Functionality)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

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

                #region Commented
                //try
                //{
                //    clsAddUpdateSpecializationBizActionVO bizactionVO = new clsAddUpdateSpecializationBizActionVO();
                //    clsSpecializationVO addNewSpecializationVO = new clsSpecializationVO();
                //    addNewSpecializationVO.SpecializationId = 0;
                //    addNewSpecializationVO.Code = txtSpecializationCode.Text;
                //    addNewSpecializationVO.SpecializationName = txtSpecializationDescription.Text;
                //    addNewSpecializationVO.Status = true;
                //    addNewSpecializationVO.ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //    addNewSpecializationVO.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //    addNewSpecializationVO.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                //    addNewSpecializationVO.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                //    addNewSpecializationVO.AddedDateTime = System.DateTime.Now;
                //    addNewSpecializationVO.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                //    bizactionVO.ItemMatserDetails.Add(addNewSpecializationVO);
                //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //    client.ProcessCompleted += (s, args) =>
                //    {

                //        if (args.Error == null && args.Result != null)
                //        {
                //            if (((clsAddUpdateSpecializationBizActionVO)args.Result).SuccessStatus == 1)
                //            {
                //                msgText = "Record is successfully submitted!";

                //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //                msgWindow.Show();
                //                //After Insertion Back to BackPanel and Setup Page
                //                objAnimation.Invoke(RotationType.Backward);
                //                SpecializationId = 0;
                //                SetupPage();
                //                SetCommandButtonState("Save");
                //            }


                //            else  if (((clsAddUpdateSpecializationBizActionVO)args.Result).SuccessStatus == 2)
                //            {
                //                msgText = "Record cannot be save because CODE already exist!";
                //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //                msgWindow.Show();
                //            }
                //            else if (((clsAddUpdateSpecializationBizActionVO)args.Result).SuccessStatus == 3)
                //            {
                //                msgText = "Record cannot be save because DESCRIPTION already exist!";
                //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //                msgWindow.Show();
                //            }
                //        }

                //    };
                //    client.ProcessAsync(bizactionVO, new clsUserVO());
                //    client.CloseAsync();
                //}
                //catch (Exception ex)
                //{

                //}
                #endregion
            }

            else
            {
                //txtSpecializationCode.Text = "";
                //txtSpecializationDescription.Text = "";
                Validation();
            }
        }

        void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    clsAddUpdateSpecializationBizActionVO bizactionVO = new clsAddUpdateSpecializationBizActionVO();
                    clsSpecializationVO addNewSpecializationVO = new clsSpecializationVO();
                    addNewSpecializationVO.SpecializationId = 0;
                    addNewSpecializationVO.Code = txtSpecializationCode.Text;
                    addNewSpecializationVO.SpecializationName = txtSpecializationDescription.Text;
                    addNewSpecializationVO.IsGenerateToken = (bool)chkIsGenerateToken.IsChecked;
                    addNewSpecializationVO.Status = true;
                    addNewSpecializationVO.ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewSpecializationVO.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewSpecializationVO.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewSpecializationVO.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewSpecializationVO.AddedDateTime = System.DateTime.Now;
                    addNewSpecializationVO.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.ItemMatserDetails.Add(addNewSpecializationVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpdateSpecializationBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                msgText = "Record is successfully submitted!";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                //After Insertion Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                SpecializationId = 0;
                                SetupPage();
                                SetCommandButtonState("Save");
                            }


                            else if (((clsAddUpdateSpecializationBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                msgText = "Record cannot be save because CODE already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpdateSpecializationBizActionVO)args.Result).SuccessStatus == 3)
                            {
                                msgText = "Record cannot be save because DESCRIPTION already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                        }

                    };
                    client.ProcessAsync(bizactionVO, new clsUserVO());
                    client.CloseAsync();
                }
                catch (Exception ex)
                {

                }
            }
        }

        /// <summary>
        /// This Event is Call When We click on Cancel Button, and show Front Panel On Which DataGrid Which
        /// Have All  Specialization List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            SpecializationId = 0;
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

        /// <summary>
        /// This Event is call When we Click On Hyperlink Button which is Present in DataGid 
        /// and Show Specific Specialization Details Which we Select
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            Validation();
            SetCommandButtonState("View");
            if (((clsSpecializationVO)grdSpecialization.SelectedItem).Status == false)
                cmdModify.IsEnabled = false;
            else
            { cmdModify.IsEnabled = true; }
            SpecializationId = ((clsSpecializationVO)grdSpecialization.SelectedItem).SpecializationId;
            grdBackPanel.DataContext = ((clsSpecializationVO)grdSpecialization.SelectedItem).DeepCopy();
            try
            {
                objAnimation.Invoke(RotationType.Forward);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            StatusCheck = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
            if (((clsSpecializationVO)grdSpecialization.SelectedItem).SubSpeID > 0 && StatusCheck == false)
            {
                long count = (((clsSpecializationVO)grdSpecialization.SelectedItem).SubSpeID);
                ((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked = true;
                msgText = "You can not Deactivate specialization because,This Specialization is linked with " + count + " subspecialization";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
            }
            else
            {

                //string msgTitle = "";
                //string msgText = "Are you sure you want to Update Status?";

                //MessageBoxControl.MessageBoxChildWindow msgWin =
                //    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                //msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWinSpec_OnMessageBoxClosed);

                //msgWin.Show();

                //  }
                // }

                //   void msgWinSpec_OnMessageBoxClosed(MessageBoxResult result)
                //   {
                //   if (result == MessageBoxResult.Yes)
                //  {
                if (grdSpecialization.SelectedItem != null)
                {
                    clsAddUpdateSpecializationBizActionVO bizactionVO = new clsAddUpdateSpecializationBizActionVO();
                    clsSpecializationVO addNewspecializationVO = new clsSpecializationVO();
                    try
                    {
                        addNewspecializationVO.SpecializationId = ((clsSpecializationVO)grdSpecialization.SelectedItem).SpecializationId;
                        addNewspecializationVO.Code = ((clsSpecializationVO)grdSpecialization.SelectedItem).Code;
                        addNewspecializationVO.SpecializationName = ((clsSpecializationVO)grdSpecialization.SelectedItem).SpecializationName;
                        addNewspecializationVO.IsGenerateToken = (bool)chkIsGenerateToken.IsChecked;

                        addNewspecializationVO.Status = StatusCheck; //Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
                        addNewspecializationVO.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        addNewspecializationVO.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        addNewspecializationVO.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                        addNewspecializationVO.UpdatedDateTime = System.DateTime.Now;
                        addNewspecializationVO.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                        bizactionVO.ItemMatserDetails.Add(addNewspecializationVO);
                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, args) =>
                        {
                            if (args.Error == null && args.Result != null)
                            {
                                if (((clsAddUpdateSpecializationBizActionVO)args.Result).SuccessStatus == 1)
                                {
                                    msgText = "Status Updated Successfully!";
                                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgWindow.Show();
                                    SetupPage();
                                }
                            }
                        };
                        client.ProcessAsync(bizactionVO, new clsUserVO());
                        client.CloseAsync();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                //   }
                else
                {
                    grdSpecialization.ItemsSource = null;
                    grdSpecialization.ItemsSource = MasterList;
                }
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            MasterList = new PagedSortableCollectionView<clsSpecializationVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdSpecialization.DataContext = MasterList;

            SetupPage();
        }

        #endregion Button Click Events

        #region Lost Focus Event
        private void txtSpecializationCode_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
            {
                ((TextBox)e.OriginalSource).ClearValidationError();
            }
        }
        #endregion

        private void txtSearchCriteria_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SetupPage();
            }
        }
    }
}
