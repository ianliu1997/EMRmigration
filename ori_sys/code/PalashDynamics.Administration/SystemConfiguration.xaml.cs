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
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.Patient;
using System.Reflection;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using PalashDynamics.Collections;
using System.ComponentModel;

namespace PalashDynamics.Administration
{
    public partial class SystemConfiguration : UserControl, INotifyPropertyChanged
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
        public Boolean Validation()
        {
            if (string.IsNullOrEmpty(txtcode.Text))
            {
                txtcode.SetValidation("Please Enter Code");
                txtcode.RaiseValidationError();
                txtcode.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtDescription.Text))
            {
                txtDescription.SetValidation("Please Enter Description");
                txtDescription.RaiseValidationError();
                txtDescription.Focus();
                return false;
            }
            else
            {
                txtcode.ClearValidationError();
                txtDescription.ClearValidationError();
                return true;
            }
        }
        #endregion

        #region  Variables
        private SwivelAnimation objAnimation;
        string msgTitle = "PALASHDYNAMICS";
        string msgText = "";
        string MasterTableName = "";
        private long Id;
        string Path = "";
        bool Ispaging = true;
        public PagedSortableCollectionView<clsMasterListVO> MasterList { get; private set; }
        public enum ParentCofiguration
        {
            None=0,
            Patient=1,
            Clinic=2,
            Billing=3,
            Inventory=4

        }

        public ParentCofiguration myParent = ParentCofiguration.None;
        #endregion

        #region Properties
        public string ModuleName { get; set; }
        public string Action { get; set; }
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
        public SystemConfiguration()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
          
            TreeView1.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(TreeView1_SelectedItemChanged);
            SetCommandButtonState("Load");
            Id = 0;
            ClearUI();
            TreeView1.ExpandSelectedPath();
            TreeView1.CollapseAllButSelectedPath();
            MasterList = new PagedSortableCollectionView<clsMasterListVO>();

            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdMaster.DataContext = MasterList;
            
        }
         #endregion

        #region Refresh Event

        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            SetupPage();
        }

        #endregion

        #region Treeview Selection Changed Event

        void TreeView1_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //throw new NotImplementedException();
            TreeView1.CollapseAllButSelectedPath();
            TreeView1.ExpandSelectedPath();
          

        }

        #endregion

        #region Loaded Event
        void SystemConfiguration_Loaded(object sender, RoutedEventArgs e)
        {

            
            
        }
        #endregion

        #region Set Command Button State New/Save/Modify/Cancel
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                      cmdModify.IsEnabled = false;
                     cmdSave.IsEnabled = false;
                     cmdCancel.IsEnabled = false;
                     //cmdClose.Visibility = Visibility.Visible;
                     //cmdCancel.Visibility = Visibility.Collapsed;
                    break;

                case "New":
                        cmdModify.IsEnabled = false;
                        cmdSave.IsEnabled = true;
                        cmdAdd.IsEnabled = false;
                        cmdCancel.IsEnabled = true;
                         //cmdClose.Visibility = Visibility.Collapsed;
                         //cmdCancel.Visibility = Visibility.Visible;
                 
                    break;
         
                case "Save":
                      cmdAdd.IsEnabled = true;
                      cmdSave.IsEnabled = false;
                      cmdCancel.IsEnabled = false;
                      //cmdClose.Visibility = Visibility.Visible;
                      //cmdCancel.Visibility = Visibility.Collapsed;

                    break;

                case "Modify":
                      cmdAdd.IsEnabled = true;
                      cmdModify.IsEnabled = false;
                      cmdCancel.IsEnabled = false;
                      //cmdClose.Visibility = Visibility.Visible;
                      //cmdCancel.Visibility = Visibility.Collapsed; 
                    break;
                case "Cancel":
                    cmdAdd.IsEnabled = true;
                    //cmdCancel.Visibility = Visibility.Collapsed;
                    //cmdClose.Visibility = Visibility.Visible;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = false;
                    break;

                case "FrontPanel":

                    cmdAdd.IsEnabled = true;
                    //cmdCancel.Visibility = Visibility.Collapsed;
                    //cmdClose.Visibility = Visibility.Visible;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                   
                    break;
                case "View":
                        cmdModify.IsEnabled = true;
                        cmdSave.IsEnabled = false;
                        cmdAdd.IsEnabled = false;
                        cmdCancel.IsEnabled = true;
                        //cmdClose.Visibility = Visibility.Collapsed;
                        //cmdCancel.Visibility = Visibility.Visible; 
                        break;

                default:
                    break;
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// This Method Is Use For Two Purpose It Fill DataGrid (All Master Tables Details) and 
        /// When We click On View Hyperlink Button Then It Will Get Details of Master Tables on Which we Click  
        /// </summary>
        public void SetupPage()
        {

            clsGetMasterListDetailsBizActionVO bizActionVO = new clsGetMasterListDetailsBizActionVO();
            bizActionVO.SearchExperssion = txtSearch.Text.Trim();
            bizActionVO.PagingEnabled = true;
            bizActionVO.MasterTableName =MasterTableName;
            bizActionVO.MaximumRows = MasterList.PageSize;
            bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
            clsMasterListVO getMasterListinfo = new clsMasterListVO();
            bizActionVO.ItemMatserDetails = new List<clsMasterListVO>();

            try
            {

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {

                    if (args.Error == null && args.Result != null)
                    {


                        bizActionVO.ItemMatserDetails = (((clsGetMasterListDetailsBizActionVO)args.Result).ItemMatserDetails);
                        ///Setup Page Fill DataGrid
                        if (Id == 0 && bizActionVO.ItemMatserDetails.Count > 0)
                        {
                            MasterList.Clear();
                            MasterList.TotalItemCount = (int)(((clsGetMasterListDetailsBizActionVO)args.Result).TotalRows);
                            foreach (clsMasterListVO item in bizActionVO.ItemMatserDetails)
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
            txtcode.Text = "";
            txtDescription.Text = "";
        }
        #endregion

        #region Button Click Events

        /// <summary>
        /// This Event is Call When We click on HyperLink Button And we get Table Name
        /// Which I set in Name Of Hyperlink
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            grdMainform.Visibility = Visibility.Collapsed;
            MainDockPanel.Visibility = Visibility.Visible;

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
            
            HyperlinkButton Hb = (HyperlinkButton)e.OriginalSource;
            grpMasterDetails.Header = Hb.Content + " Details";
            ContentControl.Content = "List Of " + Hb.Content +" Details";
            MasterTableName = Hb.Name;
            mElement.Text = Hb.Content.ToString();

            SetupPage();
           
        }


        /// <summary>
        /// This Event is Call When We click on Add Button, and show Back Panel Which Have  Tariff Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("New");
            ClearUI();
            grdMasterBackPanel.DataContext = new clsMasterListVO();
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
        /// This Event is Call When We click on Save Button and Save  Master Tables Details
        /// (For Add and Modify Master Tables Details, only One VO 
        /// and Procedure Which have Both Functionality)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
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
                //clsAddUpdateMasterListBizActionVO bizactionVO = new clsAddUpdateMasterListBizActionVO();
                //clsMasterListVO addMasterListVO = new clsMasterListVO();

                //try
                //{
                //    addMasterListVO.Id = 0;
                //    addMasterListVO.Code = txtcode.Text;
                //    addMasterListVO.Description = txtDescription.Text;
                //    addMasterListVO.Status = true;
                //    addMasterListVO.MasterTableName = MasterTableName;
                //    addMasterListVO.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //    addMasterListVO.AddUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //    addMasterListVO.By = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                //    addMasterListVO.On = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                //    addMasterListVO.DateTime = System.DateTime.Now;
                //    addMasterListVO.WindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                //    bizactionVO.ItemMatserDetails.Add(addMasterListVO);
                //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //    client.ProcessCompleted += (s, args) =>
                //    {

                //        if (args.Error == null && args.Result != null)
                //        {
                //            if (((clsAddUpdateMasterListBizActionVO)args.Result).SuccessStatus == 1)
                //            {
                //                msgText = "Record is successfully submitted!";

                //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //                msgWindow.Show();

                //                //After Insertion Back to BackPanel and Setup Page
                //                objAnimation.Invoke(RotationType.Backward);
                //                Id = 0;
                //                SetupPage();
                //                SetCommandButtonState("Save");
                //            }
                //            else if (((clsAddUpdateMasterListBizActionVO)args.Result).SuccessStatus == 2)
                //            {
                //                msgText = "Record cannot be save because CODE is already exist!";
                //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //                msgWindow.Show();
                //            }
                //            else if (((clsAddUpdateMasterListBizActionVO)args.Result).SuccessStatus == 3)
                //            {
                //                msgText = "Record cannot be save because DESCRIPTION is already exist!";
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
        }

        void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsAddUpdateMasterListBizActionVO bizactionVO = new clsAddUpdateMasterListBizActionVO();
                clsMasterListVO addMasterListVO = new clsMasterListVO();

                try
                {
                    addMasterListVO.Id = 0;
                    addMasterListVO.Code = txtcode.Text;
                    addMasterListVO.Description = txtDescription.Text;
                    addMasterListVO.Status = true;
                    addMasterListVO.MasterTableName = MasterTableName;
                    addMasterListVO.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addMasterListVO.AddUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addMasterListVO.By = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addMasterListVO.On = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addMasterListVO.DateTime = System.DateTime.Now;
                    addMasterListVO.WindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.ItemMatserDetails.Add(addMasterListVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpdateMasterListBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                msgText = "Record is successfully submitted!";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();

                                //After Insertion Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                Id = 0;
                                SetupPage();
                                SetCommandButtonState("Save");
                            }
                            else if (((clsAddUpdateMasterListBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                msgText = "Record cannot be save because CODE is already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpdateMasterListBizActionVO)args.Result).SuccessStatus == 3)
                            {
                                msgText = "Record cannot be save because DESCRIPTION is already exist!";
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
        /// This Event is Call When We click on Modify Button and Update Master Tables Details
        /// (For Add and Modify Master Tables Details, only One VO 
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
                //clsAddUpdateMasterListBizActionVO bizactionVO = new clsAddUpdateMasterListBizActionVO();
                //clsMasterListVO addMasterListVO = new clsMasterListVO();
                //try
                //{
                //    addMasterListVO.Id = Id;
                //    addMasterListVO.Code = txtcode.Text;
                //    addMasterListVO.Description = txtDescription.Text;
                //    addMasterListVO.UnitId = ((clsMasterListVO)grdMaster.SelectedItem).UnitId;
                //    addMasterListVO.Status = ((clsMasterListVO)grdMaster.SelectedItem).Status;
                //    addMasterListVO.MasterTableName = MasterTableName;
                //    addMasterListVO.AddUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //    addMasterListVO.By = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                //    addMasterListVO.On = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                //    addMasterListVO.DateTime = System.DateTime.Now;
                //    addMasterListVO.WindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                //    bizactionVO.ItemMatserDetails.Add(addMasterListVO);
                //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //    client.ProcessCompleted += (s, args) =>
                //    {

                //        if (args.Error == null && args.Result != null)
                //        {

                //            if (((clsAddUpdateMasterListBizActionVO)args.Result).SuccessStatus == 1)
                //            {
                //                msgText = "Record is successfully submitted!";

                //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //                msgWindow.Show();

                //                //After Updation Back to BackPanel and Setup Page
                //                objAnimation.Invoke(RotationType.Backward);
                //                Id = 0;
                //                SetupPage();
                //                SetCommandButtonState("Modify");

                //            }
                //            else if (((clsAddUpdateMasterListBizActionVO)args.Result).SuccessStatus == 2)
                //            {
                //                msgText = "Record cannot be save because CODE is already exist!";
                //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //                msgWindow.Show();
                //            }
                //            else if (((clsAddUpdateMasterListBizActionVO)args.Result).SuccessStatus == 3)
                //            {
                //                msgText = "Record cannot be save because DESCRIPTION is already exist!";
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
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsAddUpdateMasterListBizActionVO bizactionVO = new clsAddUpdateMasterListBizActionVO();
                clsMasterListVO addMasterListVO = new clsMasterListVO();
                try
                {
                    addMasterListVO.Id = Id;
                    addMasterListVO.Code = txtcode.Text;
                    addMasterListVO.Description = txtDescription.Text;
                    addMasterListVO.UnitId = ((clsMasterListVO)grdMaster.SelectedItem).UnitId;
                    addMasterListVO.Status = ((clsMasterListVO)grdMaster.SelectedItem).Status;
                    addMasterListVO.MasterTableName = MasterTableName;
                    addMasterListVO.AddUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addMasterListVO.By = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addMasterListVO.On = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addMasterListVO.DateTime = System.DateTime.Now;
                    addMasterListVO.WindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.ItemMatserDetails.Add(addMasterListVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {

                            if (((clsAddUpdateMasterListBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                msgText = "Record is successfully submitted!";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();

                                //After Updation Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                Id = 0;
                                SetupPage();
                                SetCommandButtonState("Modify");

                            }
                            else if (((clsAddUpdateMasterListBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                msgText = "Record cannot be save because CODE is already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpdateMasterListBizActionVO)args.Result).SuccessStatus == 3)
                            {
                                msgText = "Record cannot be save because DESCRIPTION is already exist!";
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
        /// Have All Master Tables Records List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {

            txtDescription.ClearValidationError();
            txtcode.ClearValidationError();
            SetCommandButtonState("Cancel");
            Id = 0;
            objAnimation.Invoke(RotationType.Backward);
        }

        /// <summary>
        /// This Event is call When we Click On Hyperlink Button which is Present in DataGid 
        /// and Show Specific Master Tables Details Which we Select
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("View");
           
            grdMasterBackPanel.DataContext = ((clsMasterListVO)grdMaster.SelectedItem);
            Id = ((clsMasterListVO)grdMaster.SelectedItem).Id;
         
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
        /// This Event is call When we Check and Uncheck Status 
        /// then staus updated in database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (grdMaster.SelectedItem != null)
            {
                #region Commented
                //string msgTitle = "";
                //string msgText = "Are you sure you want to Update Status?";

                //MessageBoxControl.MessageBoxChildWindow msgWinStatus =
                //    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                //msgWinStatus.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWinStatus_OnMessageBoxClosed);

                //msgWinStatus.Show();
                //((clsMasterListVO)grdMaster.SelectedItem).Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
                #endregion

                try
                {


                    clsAddUpdateMasterListBizActionVO bizactionVO = new clsAddUpdateMasterListBizActionVO();
                    clsMasterListVO addMasterListVO = new clsMasterListVO();
                    addMasterListVO.Id = ((clsMasterListVO)grdMaster.SelectedItem).Id;
                    addMasterListVO.Code = ((clsMasterListVO)grdMaster.SelectedItem).Code;
                    addMasterListVO.Description = ((clsMasterListVO)grdMaster.SelectedItem).Description;
                    addMasterListVO.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
                    addMasterListVO.MasterTableName = MasterTableName;
                    addMasterListVO.UnitId = ((clsMasterListVO)grdMaster.SelectedItem).UnitId;
                    addMasterListVO.AddUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addMasterListVO.By = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addMasterListVO.On = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addMasterListVO.DateTime = System.DateTime.Now;
                    addMasterListVO.WindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.ItemMatserDetails.Add(addMasterListVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {

                            if (((clsAddUpdateMasterListBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                Id = 0;
                                SetupPage();
                                msgText = "Staus Updated Successfully ";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                //After Updation Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                //cmdAdd.IsEnabled = true;
                                //cmdModify.IsEnabled = false;
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
                    clsAddUpdateMasterListBizActionVO bizactionVO = new clsAddUpdateMasterListBizActionVO();
                    clsMasterListVO addMasterListVO = new clsMasterListVO();
                    addMasterListVO.Id = ((clsMasterListVO)grdMaster.SelectedItem).Id;
                    addMasterListVO.Code = ((clsMasterListVO)grdMaster.SelectedItem).Code;
                    addMasterListVO.Description = ((clsMasterListVO)grdMaster.SelectedItem).Description;
                    addMasterListVO.Status = ((clsMasterListVO)grdMaster.SelectedItem).Status;
                    addMasterListVO.MasterTableName = MasterTableName;
                    addMasterListVO.UnitId = ((clsMasterListVO)grdMaster.SelectedItem).UnitId;
                    addMasterListVO.AddUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addMasterListVO.By = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addMasterListVO.On = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addMasterListVO.DateTime = System.DateTime.Now;
                    addMasterListVO.WindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.ItemMatserDetails.Add(addMasterListVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {

                            if (((clsAddUpdateMasterListBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                Id = 0;
                                SetupPage();
                                msgText = "Staus Updated Successfully ";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                //After Updation Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                //cmdAdd.IsEnabled = true;
                                //cmdModify.IsEnabled = false;
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
            MasterList = new PagedSortableCollectionView<clsMasterListVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            SetupPage();
            this.grdMaster.DataContext = MasterList;
            this.dataGrid2Pager.DataContext = MasterList;

        }

        /// <summary>
        /// This Hyperlink Event is Used for Alredy Created Form But Which is Present in Administration(Same Module)
        /// In Tag I Specified the Action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void CreatedForm_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton HB = (HyperlinkButton)e.OriginalSource;
            if (HB.Tag != null)
            {
                Action = HB.Tag.ToString();
                
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = HB.Content.ToString();
                UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance(Action) as UIElement;

                ((IApplicationConfiguration)App.Current).OpenMainContent(mydata);
            }
        }

        /// <summary>
        /// This Hyperlink Event is Used for Alredy Created Form But Which is Present in Different Module InTag Name 
        /// I Specified The Action and in Target I specified the Module(Dll Name)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void CreatedformInOtherProject_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton HB = (HyperlinkButton)e.OriginalSource;
            ModuleName = HB.TargetName;
            Action = HB.Tag.ToString();
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
            mElement.Text = HB.Content.ToString();
            WebClient c = new WebClient();
            c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
            c.OpenReadAsync(new Uri(ModuleName+ ".xap", UriKind.Relative));
        }

        void c_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
                try
                {

                    UIElement myData = null;
                    string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

                    XElement deploy = XDocument.Parse(appManifest).Root;
                    List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                            where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                            select assemblyParts).ToList();
                    Assembly asm = null;
                    AssemblyPart asmPart = new AssemblyPart();
                    StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                    asm = asmPart.Load(streamInfo.Stream);

                    myData = asm.CreateInstance(Action) as UIElement;

                    ((IApplicationConfiguration)App.Current).OpenMainContent(myData);


                   

                }
                catch (Exception ex)
                {
                    throw;
                }

           
           
        }


        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            grdMainform.Visibility = Visibility.Visible;
            MainDockPanel.Visibility = Visibility.Collapsed;
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            MasterList = new PagedSortableCollectionView<clsMasterListVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            SetupPage();
            this.grdMaster.DataContext = MasterList;
            this.dataGrid2Pager.DataContext = MasterList;

        }
        #endregion Button Click Events

        #region Lost Focus

        private void txtcode_LostFocus(object sender, RoutedEventArgs e)
        {
           
            if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
            {

                ((TextBox)e.OriginalSource).ClearValidationError();
                
            }

            if (txtDescription.Text != "")
            {
                txtDescription.Text = txtDescription.Text.ToTitleCase();
            }
        }

        #endregion

       


    }
}
