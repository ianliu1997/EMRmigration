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
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Master;
using System.ComponentModel;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using System.Reflection;

namespace PalashDynamics.Administration
{
    public partial class FrmE2LevelOfDayMaster : UserControl
    {
        public FrmE2LevelOfDayMaster()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

          
            SetCommandButtonState("Load");
            Id = 0;
            ClearUI();
            txtSearch.Focus();
         
            MasterList = new PagedSortableCollectionView<clsMasterListVO>();

            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdMaster.DataContext = MasterList;
            SetupPage();
        }

        

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

        #region Refresh Event

        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            SetupPage();
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

        #region Validation
        public Boolean Validation()
        {  //by rohinee 10/10/2015
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
            return result;
            //
        }
        #endregion

        #region Public Variables

        public string Action { get; set; }
        private SwivelAnimation objAnimation;
        string msgTitle = "";
        string msgText = "";
        string MasterTableName = "M_IVF_ELevelOnDayMaster";
        private long Id;
        string Path = "";
        bool Ispaging = true;
        public PagedSortableCollectionView<clsMasterListVO> MasterList { get; private set; }
        public string ModuleName { get; set; }
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
        bool IsCancel = true;

        #endregion

      
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
            //by rohinee 10/10/2015
            Validation();
            //
            SetCommandButtonState("View");
            //by rohinee 10/10/2015
            grdMasterBackPanel.DataContext = ((clsMasterListVO)grdMaster.SelectedItem).DeepCopy();
            //
            Id = ((clsMasterListVO)grdMaster.SelectedItem).Id;
            //by rohinee 10/10/2015
            if (((clsMasterListVO)grdMaster.SelectedItem).Status == true)
            {
                cmdModify.IsEnabled = true;
            }
            else
            {
                cmdModify.IsEnabled = false;
            }
            //
            try
            {
                objAnimation.Invoke(RotationType.Forward);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {

            SetCommandButtonState("New");
            ClearUI();
            Validation();
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
                string msgText = "Are you sure you want to Save ?";

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
                                msgText = "Record is saved successfully !";

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
                string msgText = "Are you sure you want to Update?";

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
                                msgText = "Record is Updated successfully !";

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
                                msgText = "Record cannot be Update because CODE is already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpdateMasterListBizActionVO)args.Result).SuccessStatus == 3)
                            {
                                msgText = "Record cannot be Update because DESCRIPTION is already exist!";
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
            objAnimation.Invoke(RotationType.Backward);

            if (IsCancel == true)
            {
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "IVF Configuration";

                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmIVFConfiguration") as UIElement;
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
            if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
            {
                ((TextBox)e.OriginalSource).ClearValidationError();
            }
        }



        public void ClearUI()
        {
            txtcode.Text = "";
            txtDescription.Text = "";
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

       

       
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            MasterList = new PagedSortableCollectionView<clsMasterListVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            SetupPage();
            this.grdMaster.DataContext = MasterList;
            this.dataGrid2Pager.DataContext = MasterList;
        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtDescription_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsPositiveNumberValid() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }
    }
}
