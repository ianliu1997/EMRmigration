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
using System.Reflection;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects;
using System.ComponentModel;

namespace PalashDynamics.Administration
{
    public partial class frmPatientConfiguration : UserControl, INotifyPropertyChanged
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
            bool result = true;

            if (string.IsNullOrEmpty(txtcode.Text.Trim()))
            {
                txtcode.SetValidation("Please Enter Code");
                txtcode.RaiseValidationError();
                txtcode.Focus();
                result = false;
                

            }
            else
            {
                txtcode.ClearValidationError();

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
            if (txtcode.Text.StartsWith(" "))
            {
                txtcode.SetValidation("Code Cannot Start With A Space ");
                txtcode.RaiseValidationError();
                txtcode.Focus();
                result = false;
            }
            
            if (txtDescription.Text.StartsWith(" "))
            {
                txtcode.SetValidation("Description Cannot Start With A Space ");
                txtcode.RaiseValidationError();
                txtcode.Focus();
                result = false;
            }
            return result;

              //  if (txtcode.Text.StartsWith(" ") || txtDescription.Text.StartsWith(" "))
              //  {
              //      txtcode.SetValidation("Code Does Not Accept Spaces");
              //      txtcode.RaiseValidationError();
              //     // txtcode.Focus();
              //      txtDescription.SetValidation("Description Does Not Accept Spaces");
              //      txtDescription.RaiseValidationError();
              //    //  txtDescription.Focus();
              //      result = false;

              //  }
              // else
              //{
              //  txtcode.ClearValidationError();
              ////  result = true;
              // }               
               
           
        }
        #endregion

        #region Public Variables

        public string Action { get; set; }
        private SwivelAnimation objAnimation;
        string msgTitle = "";
        string msgText = "";
        string MasterTableName = "";
        private long Id;
        string Path = "";
        bool Ispaging = true;
        public PagedSortableCollectionView<clsMasterListVO> MasterList { get; private set; }
        public string ModuleName { get; set; }
        bool IsCancel = true;
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

        public frmPatientConfiguration()
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

        #region Refresh Event

        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            SetupPage();
        }

        #endregion

        void TreeView1_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //throw new NotImplementedException();
            TreeView1.CollapseAllButSelectedPath();
            TreeView1.ExpandSelectedPath();
        }

        private void CreatedForm_Click(object sender, RoutedEventArgs e)
        {
            MainDockPanel.Visibility = Visibility.Visible;
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

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            grdMainform.Visibility = Visibility.Collapsed;
            MainDockPanel.Visibility = Visibility.Visible;

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");

            HyperlinkButton Hb = (HyperlinkButton)e.OriginalSource;
            //commented by rohini for remove groupbox
            //grpMasterDetails.Header = Hb.Content + " Details";
            ContentControl.Content = "List Of " + Hb.Content + " Details";
            MasterTableName = Hb.Name;
            mElement.Text = Hb.Content.ToString();

            SetupPage();
        }

        private void SpecialRegistration_Click(object sender, RoutedEventArgs e)
        {
            grdMainform.Visibility = Visibility.Collapsed;
            MainDockPanel.Visibility = Visibility.Visible;
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");

            HyperlinkButton Hb = (HyperlinkButton)e.OriginalSource;
            //commented by rohini for remove groupbox
            //grpMasterDetails.Header = Hb.Content + " Details";
            ContentControl.Content = "List Of " + Hb.Content + " Details";
            MasterTableName = Hb.Name;
            mElement.Text = Hb.Content.ToString();

            SetupPage();

        }

        private void ProfileMaster_Click(object sender, RoutedEventArgs e)
        {
           
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
                
        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            Validation();
            SetCommandButtonState("View");
            txtcode.Focus();
            if (((clsMasterListVO)grdMaster.SelectedItem).Status == false)
                cmdModify.IsEnabled = false;
            else
                cmdModify.IsEnabled = true;
            grdMasterBackPanel.DataContext = ((clsMasterListVO)grdMaster.SelectedItem).DeepCopy();
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (grdMaster.SelectedItem != null)
            {
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
                                msgText = "Status Updated Successfully ";

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

        private void txtcode_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
            //{
            //    ((TextBox)e.OriginalSource).ClearValidationError();
            //}                                                       

            //if (txtcode.Text == null || txtcode.Text == "" || txtcode.Text == " ")
            //{
            //    if (string.IsNullOrEmpty(txtcode.Text.Trim()))
            //    {
            //        txtcode.SetValidation("Please Enter Code");
            //        txtcode.RaiseValidationError();
            //        txtcode.Focus();
            //      //  result = false;
            //    }
            //}
            //else
            //{
            //    txtcode.ClearValidationError();
            //  //  result = true;
            //}
            //if (txtDescription.Text == null || txtDescription.Text == "" || txtDescription.Text == " ")
            //{
            //    if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
            //    {
            //        txtDescription.SetValidation("Please Enter Description");
            //        txtDescription.RaiseValidationError();
            //        txtDescription.Focus();
            //       // result = false;
            //    }
            //}
            //else
            //{
            //    txtDescription.ClearValidationError();
            //  //  result = true;
            //}
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

        public void SetupPage()
        {
            clsGetMasterListDetailsBizActionVO bizActionVO = new clsGetMasterListDetailsBizActionVO();
            bizActionVO.SearchExperssion = txtSearch.Text.Trim();
            bizActionVO.PagingEnabled = true;
            bizActionVO.MasterTableName = MasterTableName;
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

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            Validation();
            SetCommandButtonState("New");
            ClearUI();
            txtcode.Focus();
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

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {

                string msgTitle = "";
                string msgText = "Are You Sure You Want To Save ?";

                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);

                msgWin.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Required Fields To Save The Record", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
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
                        if (txtcode.Text != null && txtcode.Text != "")
                        {
                            addMasterListVO.Code = txtcode.Text;
                        }
                        if (txtDescription.Text != null && txtDescription.Text != "")
                        {
                            addMasterListVO.Description = txtDescription.Text;
                        }

                       
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
                                    msgText = "Record Saved Successfully.";

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
                                    msgText = "Record Cannot Be Saved Because CODE Already Exist!";
                                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    msgWindow.Show();
                                }
                                else if (((clsAddUpdateMasterListBizActionVO)args.Result).SuccessStatus == 3)
                                {
                                    msgText = "Record Cannot Be Save Because DESCRIPTION Already Exist!";
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
            bool ModifySchedule = true;
            ModifySchedule = Validation();
            if (ModifySchedule == true && Validation())
            {

                string msgTitle = "";
                string msgText = "Are You Sure You Want To Update ?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Required Fields To Update The Record", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
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
                        if (txtcode.Text != null && txtcode.Text != "")
                        {
                            addMasterListVO.Code = txtcode.Text;
                        }
                        if (txtDescription.Text != null && txtDescription.Text != "")
                        {
                            addMasterListVO.Description = txtDescription.Text;
                        }
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
                                    msgText = "Record Updated Successfully.";

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
                                    msgText = "Record Cannot Be Save Because CODE Already Exist!";
                                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    msgWindow.Show();
                                }
                                else if (((clsAddUpdateMasterListBizActionVO)args.Result).SuccessStatus == 3)
                                {
                                    msgText = "Record Cannot Be Save Because DESCRIPTION Already Exist!";
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
        /// 
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            txtDescription.ClearValidationError();
            txtcode.ClearValidationError();
            SetCommandButtonState("Cancel");
            Id = 0;
            ClearUI();
            objAnimation.Invoke(RotationType.Backward);

            if (IsCancel == true)
            {
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Patient Configuration";  

                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmPatientConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

            }
            else
            {
                IsCancel = true;
            }
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
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

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MasterList = new PagedSortableCollectionView<clsMasterListVO>();
                MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                PageSize = 15;
                SetupPage();
                this.grdMaster.DataContext = MasterList;
                this.dataGrid2Pager.DataContext = MasterList;
            }
        }

        
    }
}
