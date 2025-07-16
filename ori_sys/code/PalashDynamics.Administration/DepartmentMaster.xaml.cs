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
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Administration;
using System.ComponentModel;
using PalashDynamics.Collections;
using System.Reflection;
namespace PalashDynamics.Administration
{
    public partial class DepartmentMaster : UserControl, INotifyPropertyChanged
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
            if (string.IsNullOrEmpty(txtCode.Text.Trim()))
            {
                txtCode.SetValidation("Please Enter Code");
                txtCode.RaiseValidationError();
                txtCode.Focus();
                result = false;
            }
            else
            {
                txtCode.ClearValidationError();
            }
            if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
            {
                txtDescription.SetValidation("Please Enter Description");
                txtDescription.RaiseValidationError();
                txtDescription.Focus();
                result = false;
            }
            else
            {
                txtDescription.ClearValidationError();
            }
            return result;
        }

        #endregion

        #region  Variables
        private SwivelAnimation objAnimation;
        string msgTitle = "PALASHDYNAMICS";
        string msgText = "";
        private long DepartmentId;
        int PageIndex = 0;
        public PagedSortableCollectionView<clsDepartmentVO> MasterList { get; private set; }
        bool IsCancel = true;
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

        public DepartmentMaster()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(DepartmentMaster_Loaded);
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            SetCommandButtonState("Load");
            DepartmentId = 0;
            MasterList = new PagedSortableCollectionView<clsDepartmentVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdDepartment.DataContext = MasterList;
            SetupPage();
        }

        #endregion Constructor

        #region On Refresh Event

        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            PageIndex = dataGrid2Pager.PageIndex;
            SetupPage();
        }

        #endregion On Refresh Event

        #region Load Event
        void DepartmentMaster_Loaded(object sender, RoutedEventArgs e)
        {
            FillSpecializationSubSp();
         //   Validation();
        }
        #endregion Load Event

        #region Public Methods
        /// <summary>
        /// This Method Is Use For Two Purpose It Fill DataGrid (All Department Details) and 
        /// When We click On View Hyperlink Button Then It Will Get Details of Department on Which we Click  
        /// </summary>
        public void SetupPage()
        {
            clsGetDepartmentMasterDetailsBizActionVO bizActionVO = new clsGetDepartmentMasterDetailsBizActionVO();
            bizActionVO.SearchExpression = txtSearch.Text;
            bizActionVO.PagingEnabled = true;
            bizActionVO.MaximumRows = MasterList.PageSize;
            bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
            clsDepartmentVO getDepartmentinfo = new clsDepartmentVO();
            bizActionVO.ItemMatserDetails = new List<clsDepartmentVO>();
            try
            {

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {

                    if (args.Error == null && args.Result != null)
                    {


                        bizActionVO.ItemMatserDetails = (((clsGetDepartmentMasterDetailsBizActionVO)args.Result).ItemMatserDetails);

                        ///Setup Page Fill DataGrid
                        if (DepartmentId == 0 && bizActionVO.ItemMatserDetails.Count > 0)
                        {
                            MasterList.Clear();
                            MasterList.TotalItemCount = (int)(((clsGetDepartmentMasterDetailsBizActionVO)args.Result).TotalRows);
                            foreach (clsDepartmentVO item in bizActionVO.ItemMatserDetails)
                            {
                                MasterList.Add(item);
                            }

                        }

                    }

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
            txtDescription.Text = "";
            txtCode.Text = "";
            chkIsClinical.IsChecked = false;
            FillSpecializationSubSp();
        }
        #endregion Public Methods

        #region Set Command Button State New/Save/Modify/Cancel
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

                case "FrontPanel":

                    cmdAdd.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    IsCancel = true;

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
        /// This Event is Call When We click on Add Button, and show Back Panel Which Have Department Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            Validation(); 
            grdBackPanel.DataContext = new clsDepartmentVO();
            SetCommandButtonState("New");

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

        /// <summary>
        /// This Event is Call When We click on Modify Button and Update Department Details
        /// (For Add and Modify Supplier Details, only One VO 
        /// and Procedure Which have Both Functionality)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to Update ?";
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                msgW.Show();

                #region Commented
                //clsAddUpdateDepartmentMasterBizActionVO bizactionVO = new clsAddUpdateDepartmentMasterBizActionVO();
                //clsDepartmentVO addNewSubspecializationVO = new clsDepartmentVO();
                //try
                //{
                //    addNewSubspecializationVO.Id = DepartmentId;
                //    addNewSubspecializationVO.Code = txtCode.Text;
                //    addNewSubspecializationVO.IsClinical = (bool)chkIsClinical.IsChecked;
                //    addNewSubspecializationVO.Description = txtDescription.Text;
                //    addNewSubspecializationVO.UnitId = ((clsDepartmentVO)grdDepartment.SelectedItem).UnitId;
                //    addNewSubspecializationVO.Status = ((clsDepartmentVO)grdDepartment.SelectedItem).Status;
                //    addNewSubspecializationVO.AddUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //    addNewSubspecializationVO.By = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                //    addNewSubspecializationVO.On = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                //    addNewSubspecializationVO.DateTime = System.DateTime.Now;
                //    addNewSubspecializationVO.WindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                //    bizactionVO.ItemMatserDetails.Add(addNewSubspecializationVO);
                //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //    client.ProcessCompleted += (s, args) =>
                //    {
                //        if (args.Error == null && args.Result != null)
                //        {
                //            if (((clsAddUpdateDepartmentMasterBizActionVO)args.Result).SuccessStatus == 1)
                //            {
                //                msgText = "Record is successfully submitted!";
                //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //                msgWindow.Show();
                //                //After Updation Back to BackPanel and Setup Page
                //                objAnimation.Invoke(RotationType.Backward);
                //                DepartmentId = 0;
                //                SetupPage();
                //                SetCommandButtonState("Modify");
                //            }
                //            else if (((clsAddUpdateDepartmentMasterBizActionVO)args.Result).SuccessStatus == 2)
                //            {
                //                msgText = "Record cannot be save because CODE already exist!";
                //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //                msgWindow.Show();
                //            }
                //            else if (((clsAddUpdateDepartmentMasterBizActionVO)args.Result).SuccessStatus == 3)
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
                //txtCode.Text = "";
                //txtDescription.Text = "";
                Validation(); 
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsAddUpdateDepartmentMasterBizActionVO bizactionVO = new clsAddUpdateDepartmentMasterBizActionVO();
                clsDepartmentVO addNewSubspecializationVO = new clsDepartmentVO();
                try
                {
                    addNewSubspecializationVO.Id = DepartmentId;
                    addNewSubspecializationVO.Code = txtCode.Text;
                    addNewSubspecializationVO.IsClinical = (bool)chkIsClinical.IsChecked;
                    addNewSubspecializationVO.Description = txtDescription.Text;

                    addNewSubspecializationVO.UnitId = ((clsDepartmentVO)grdDepartment.SelectedItem).UnitId;
                    addNewSubspecializationVO.Status = ((clsDepartmentVO)grdDepartment.SelectedItem).Status;
                    addNewSubspecializationVO.AddUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewSubspecializationVO.By = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewSubspecializationVO.On = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewSubspecializationVO.DateTime = System.DateTime.Now;
                    addNewSubspecializationVO.WindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.ItemMatserDetails.Add(addNewSubspecializationVO);

                    //Added by CDS
                    bizactionVO.IsUpdate = true;
                    addNewSubspecializationVO.SpecilizationList = (List<clsSubSpecializationVO>)grdSpecializationDept.ItemsSource;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpdateDepartmentMasterBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                msgText = "Record is successfully submitted!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();

                                //After Updation Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                DepartmentId = 0;
                                SetupPage();
                                SetCommandButtonState("Modify");
                            }
                            else if (((clsAddUpdateDepartmentMasterBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                msgText = "Record cannot be save because CODE already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpdateDepartmentMasterBizActionVO)args.Result).SuccessStatus == 3)
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
        /// This Event is Call When We click on Save Button and Save Department Details
        /// (For Add and Modify SubSpecialization Details, only One VO 
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
                //clsAddUpdateDepartmentMasterBizActionVO bizactionVO = new clsAddUpdateDepartmentMasterBizActionVO();
                //clsDepartmentVO addNewDepartmentVO = new clsDepartmentVO();
                //try
                //{
                //    addNewDepartmentVO.Id = 0;
                //    addNewDepartmentVO.Code = txtCode.Text;
                //    addNewDepartmentVO.IsClinical = (bool)chkIsClinical.IsChecked;
                //    addNewDepartmentVO.Description = txtDescription.Text;
                //    //addNewSubSpecializationVO.SpecializationId = Convert.ToInt64(cmbSpecialization.SelectedValue);
                //    addNewDepartmentVO.Status = true;
                //    addNewDepartmentVO.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //    addNewDepartmentVO.AddUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //    addNewDepartmentVO.By = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                //    addNewDepartmentVO.On = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                //    addNewDepartmentVO.DateTime = System.DateTime.Now;
                //    addNewDepartmentVO.WindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                //    bizactionVO.ItemMatserDetails.Add(addNewDepartmentVO);
                //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //    client.ProcessCompleted += (s, args) =>
                //    {

                //        if (args.Error == null && args.Result != null)
                //        {
                //            if (((clsAddUpdateDepartmentMasterBizActionVO)args.Result).SuccessStatus == 1)
                //            {
                //                msgText = "Record is successfully submitted!";

                //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //                msgWindow.Show();
                //                //After Insertion Back to BackPanel and Setup Page
                //                objAnimation.Invoke(RotationType.Backward);
                //                DepartmentId = 0;
                //                SetupPage();
                //                SetCommandButtonState("Save");

                //            }
                //            else if (((clsAddUpdateDepartmentMasterBizActionVO)args.Result).SuccessStatus == 2)
                //            {
                //                msgText = "Record cannot be save because CODE already exist!";
                //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //                msgWindow.Show();
                //            }
                //            else if (((clsAddUpdateDepartmentMasterBizActionVO)args.Result).SuccessStatus == 3)
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
                //txtCode.Text = "";
                //txtDescription.Text = "";
                Validation(); 
            }
        }

        void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsAddUpdateDepartmentMasterBizActionVO bizactionVO = new clsAddUpdateDepartmentMasterBizActionVO();
                clsDepartmentVO addNewDepartmentVO = new clsDepartmentVO();
                try
                {
                    addNewDepartmentVO.Id = 0;
                    addNewDepartmentVO.Code = txtCode.Text;
                    addNewDepartmentVO.IsClinical = (bool)chkIsClinical.IsChecked;
                    addNewDepartmentVO.Description = txtDescription.Text;
                    //addNewSubSpecializationVO.SpecializationId = Convert.ToInt64(cmbSpecialization.SelectedValue);
                    addNewDepartmentVO.Status = true;
                    addNewDepartmentVO.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewDepartmentVO.AddUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewDepartmentVO.By = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewDepartmentVO.On = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewDepartmentVO.DateTime = System.DateTime.Now;
                    addNewDepartmentVO.WindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;

                    addNewDepartmentVO.SpecilizationList = (List<clsSubSpecializationVO>)grdSpecializationDept.ItemsSource;
                    bizactionVO.ItemMatserDetails.Add(addNewDepartmentVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpdateDepartmentMasterBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                msgText = "Record is successfully submitted!";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                //After Insertion Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                DepartmentId = 0;
                                SetupPage();
                                SetCommandButtonState("Save");


                            }
                            else if (((clsAddUpdateDepartmentMasterBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                msgText = "Record cannot be save because CODE already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpdateDepartmentMasterBizActionVO)args.Result).SuccessStatus == 3)
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
        /// Have All Department List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearUI();
            txtCode.ClearValidationError();
            txtDescription.ClearValidationError();
            SetCommandButtonState("Cancel");
            DepartmentId = 0;
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
        /// and Show Specific Department Details Which we Select
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            Validation(); 
            SetCommandButtonState("View");
            if (((clsDepartmentVO)grdDepartment.SelectedItem).Status == false)
                cmdModify.IsEnabled = false;
            else { cmdModify.IsEnabled = true; }
            DepartmentId = ((clsDepartmentVO)grdDepartment.SelectedItem).Id;
            grdBackPanel.DataContext = ((clsDepartmentVO)grdDepartment.SelectedItem).DeepCopy();
            fillDeptSecializationList(DepartmentId);
            try
            {
                objAnimation.Invoke(RotationType.Forward);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void fillDeptSecializationList(long DepartmentId)
        {
            clsGetDepartmentMasterDetailsBizActionVO BizAction = new clsGetDepartmentMasterDetailsBizActionVO();
            BizAction.DeptSpecializationDetails.Id = DepartmentId;
            BizAction.DeptSpecializationDetails.SpecilizationList = new List<clsSubSpecializationVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null)
                {
                    BizAction.DeptSpecializationDetails.SpecilizationList = (((clsGetDepartmentMasterDetailsBizActionVO)args.Result).DeptSpecializationDetails.SpecilizationList);
                    if (BizAction.DeptSpecializationDetails.SpecilizationList != null)
                    {
                        List<clsSubSpecializationVO> lstDepartment = (List<clsSubSpecializationVO>)grdSpecializationDept.ItemsSource; //new List<clsSubSpecializationVO>();

                        foreach (var item in lstDepartment)  //foreach (var item in (List<clsSubSpecializationVO>)grdSpecializationDept.ItemsSource)
                        {
                            //if (item.Status == true)
                            //{
                                foreach (var item1 in BizAction.DeptSpecializationDetails.SpecilizationList)
                                {
                                    if (item.SubSpecializationId == item1.SubSpecializationId)
                                    {
                                        item.Status = item1.Status;
                                       // lstDepartment.Add(item);
                                    }

                                }
                           // }
                        }
                        grdSpecializationDept.ItemsSource = null;
                        grdSpecializationDept.ItemsSource = lstDepartment;//BizAction.DeptSpecializationDetails.SpecilizationList;
                    }
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();

        }

        /// <summary>
        /// This Event is Call When We checked and Unchecked status from Datagrid
        ///And Status Updated Sucessfully  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (grdDepartment.SelectedItem != null)
            {
                #region Commented

                //    string msgTitle = "";
                //    string msgText = "Are you sure you want to Update Status?";

                //    MessageBoxControl.MessageBoxChildWindow msgWinStatus =
                //        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                //    msgWinStatus.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWinStatus_OnMessageBoxClosed);

                //    msgWinStatus.Show();
                //    ((clsDepartmentVO)grdDepartment.SelectedItem).Status=Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);

                //}

                #endregion
                try
                {
                    clsAddUpdateDepartmentMasterBizActionVO bizactionVO = new clsAddUpdateDepartmentMasterBizActionVO();
                    clsDepartmentVO addNewDepartmentVO = new clsDepartmentVO();
                    addNewDepartmentVO.Id = ((clsDepartmentVO)grdDepartment.SelectedItem).Id;
                    addNewDepartmentVO.Code = ((clsDepartmentVO)grdDepartment.SelectedItem).Code;
                    addNewDepartmentVO.IsClinical = ((clsDepartmentVO)grdDepartment.SelectedItem).IsClinical;
                    addNewDepartmentVO.Description = ((clsDepartmentVO)grdDepartment.SelectedItem).Description;
                    addNewDepartmentVO.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
                    addNewDepartmentVO.UnitId = ((clsDepartmentVO)grdDepartment.SelectedItem).UnitId;
                    addNewDepartmentVO.AddUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewDepartmentVO.By = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewDepartmentVO.On = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewDepartmentVO.DateTime = System.DateTime.Now;
                    addNewDepartmentVO.WindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.ItemMatserDetails.Add(addNewDepartmentVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpdateDepartmentMasterBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                DepartmentId = 0;
                                SetupPage();
                                msgText = "Status Updated Successfully";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();

                                //After Updation Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                SetCommandButtonState("Modify");
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

        void msgWinStatus_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    clsAddUpdateDepartmentMasterBizActionVO bizactionVO = new clsAddUpdateDepartmentMasterBizActionVO();
                    clsDepartmentVO addNewDepartmentVO = new clsDepartmentVO();
                    addNewDepartmentVO.Id = ((clsDepartmentVO)grdDepartment.SelectedItem).Id;
                    addNewDepartmentVO.Code = ((clsDepartmentVO)grdDepartment.SelectedItem).Code;
                    addNewDepartmentVO.IsClinical = ((clsDepartmentVO)grdDepartment.SelectedItem).IsClinical;
                    addNewDepartmentVO.Description = ((clsDepartmentVO)grdDepartment.SelectedItem).Description;
                    addNewDepartmentVO.Status = ((clsDepartmentVO)grdDepartment.SelectedItem).Status;
                    addNewDepartmentVO.UnitId = ((clsDepartmentVO)grdDepartment.SelectedItem).UnitId;
                    addNewDepartmentVO.AddUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewDepartmentVO.By = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewDepartmentVO.On = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewDepartmentVO.DateTime = System.DateTime.Now;
                    addNewDepartmentVO.WindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.ItemMatserDetails.Add(addNewDepartmentVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpdateDepartmentMasterBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                DepartmentId = 0;
                                SetupPage();
                                msgText = "Status Updated Successfully";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();

                                //After Updation Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                SetCommandButtonState("Modify");
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
            MasterList = new PagedSortableCollectionView<clsDepartmentVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdDepartment.DataContext = MasterList;
            SetupPage();

        }
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {

            MasterList = new PagedSortableCollectionView<clsDepartmentVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdDepartment.DataContext = MasterList;

            SetupPage();
        }
        #endregion Button Click Events

        #region Lost Focus
        private void txtCode_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
            {
                ((TextBox)e.OriginalSource).ClearValidationError();
            }
        }
        #endregion

        private void FillSpecialization()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_Specialization;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    //  objList.Add(new MasterListItem(0, "-- Select --"));
                    objSpecializationList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    grdSpecializationDept.ItemsSource = null;
                    grdSpecializationDept.ItemsSource = objSpecializationList;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillSpecializationSubSp()
        {
            clsGetSubSpecializationDetailsBizActionVO BizAction = new clsGetSubSpecializationDetailsBizActionVO();
            BizAction.ItemMatserDetails = new List<clsSubSpecializationVO>();
            List<clsSubSpecializationVO> tempList = new List<clsSubSpecializationVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    grdSpecializationDept.ItemsSource = null;
                    //foreach (var item in ((clsGetSubSpecializationDetailsBizActionVO)e.Result).ItemMatserDetails)
                    //{
                    //    item.Status = false;
                    //}

                    foreach (var item in ((clsGetSubSpecializationDetailsBizActionVO)e.Result).ItemMatserDetails) //Added By Umesh to avoide deactivated records in grdSpecializationDept
                    {
                        if (item.Status == true)
                        {
                            item.Status = false;
                            tempList.Add(item);
                        }
                    }
                    grdSpecializationDept.ItemsSource = tempList;//((clsGetSubSpecializationDetailsBizActionVO)e.Result).ItemMatserDetails;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillSpecilizationGrid()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_Specialization;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    foreach (var item in objList)
                    {
                        item.Status = false;
                    }
                    grdSpecializationDept.ItemsSource = null;
                    grdSpecializationDept.ItemsSource = objList;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        List<MasterListItem> objSpecializationList = new List<MasterListItem>();
        List<clsDeptSpecilizationLinkingVO> SpecializationList = new List<clsDeptSpecilizationLinkingVO>();
        List<MasterListItem> SpecializationMAsterList = new List<MasterListItem>();
        List<MasterListItem> RemovedSpecializationMAsterList = new List<MasterListItem>();

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.IsChecked == true)
            {
                foreach (var item in objSpecializationList)
                {
                    if (item.ID == ((MasterListItem)grdSpecializationDept.SelectedItem).ID)
                    {
                        item.Status = true;
                        SpecializationMAsterList.Add(item);
                    }
                }
            }
            if (SpecializationMAsterList.Count == 0)
            {
                foreach (var item in objSpecializationList)
                {
                    if (item.Status == true)
                    {
                        SpecializationMAsterList.Add(item);
                    }
                    if (item.Status == false)
                    {
                        SpecializationMAsterList.Remove(item);
                    }
                }
            }
            RemovedSpecializationMAsterList = SpecializationMAsterList;
            if (chk.IsChecked == false)
            {

                for (int i = 0; i < SpecializationMAsterList.Count(); i++)
                {
                    if (SpecializationMAsterList[i].ID == ((MasterListItem)grdSpecializationDept.SelectedItem).ID)
                    {
                        RemovedSpecializationMAsterList.Remove(SpecializationMAsterList[i]);
                    }
                }
                SpecializationMAsterList = RemovedSpecializationMAsterList;
            }
        }

        private void txtSearchCriteria_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SetupPage();
            }
        }
    }
}
