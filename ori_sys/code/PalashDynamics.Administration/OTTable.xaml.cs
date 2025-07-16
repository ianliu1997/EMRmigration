using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using System.ComponentModel;
using PalashDynamics.Collections;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Reflection;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using CIMS;
using PalashDynamics.ValueObjects.Master;
using System.Windows.Input;



namespace PalashDynamics.Administration
{
    public partial class OTTable : UserControl, INotifyPropertyChanged
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
            bool Result = true;
            string strVal = string.Empty;
            if (string.IsNullOrEmpty(txtStoreCode.Text.Trim()))
            {
                strVal = "Please enter code !";
                //strVal = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("CodeReqd_Msg");
                txtStoreCode.SetValidation(strVal);
                txtStoreCode.RaiseValidationError();
                txtStoreCode.Focus();
                Result = false;
                //return false;
            }
            else
                txtStoreCode.ClearValidationError();

            if (string.IsNullOrEmpty(txtStoreName.Text.Trim()))
            {
                strVal = "Please enter description !";
                //strVal = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("DescReqd_Msg");
                txtStoreName.SetValidation(strVal);
                txtStoreName.RaiseValidationError();
                txtStoreName.Focus();
                Result = false;
                //return false;
            }
            else
                txtStoreName.ClearValidationError();
            //else if (!string.IsNullOrEmpty(txtStoreName.Text))
            //{
            //if (((MasterListItem)CmbOtTheatre.SelectedItem) == null || ((MasterListItem)CmbOtTheatre.SelectedItem).ID == 0)
            //{
            //    msgText = "Please select Operation Theatre.";
            //    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);                
            //    CmbOtTheatre.Focus();
            //    Result = false;
            //    //return false;
            //}

            //}
            //else
            //{
            //    txtStoreCode.ClearValidationError();
            //    txtStoreName.ClearValidationError();
            //    return true;
            //}

            if ((MasterListItem)CmbOtTheatre.SelectedItem == null)
            {
                CmbOtTheatre.TextBox.SetValidation("Please Select OT Theatre");
                CmbOtTheatre.TextBox.RaiseValidationError();
                CmbOtTheatre.Focus();
                Result = false;
            }

            else
            {
                if (CmbOtTheatre.SelectedItem != null)
                {
                    if (((MasterListItem)CmbOtTheatre.SelectedItem).ID == 0)
                    {
                        CmbOtTheatre.TextBox.SetValidation("Please Select  OT Theatre");
                        CmbOtTheatre.TextBox.RaiseValidationError();
                        CmbOtTheatre.Focus();
                        Result = false;
                    }
                }
                else
                    CmbOtTheatre.TextBox.ClearValidationError();
            }


            return Result;
        }


        #endregion

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
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

        #region  Variables
        private SwivelAnimation objAnimation;
        string msgTitle = ""; // "PALASHDYNAMICS";
        string msgText = "";
        long OTTableID;

        public bool isModify = false;
        public PagedSortableCollectionView<clsOTTableVO> MasterList { get; private set; }


        public bool IsCancel = true;
        public string ModuleName { get; set; }
        public string Action { get; set; }

        #endregion
        /// <summary>
        /// Constructor
        /// </summary>
        public OTTable()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            MasterList = new PagedSortableCollectionView<clsOTTableVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);

            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            grdOTTable.DataContext = MasterList;

            SetupPage();
           
            //this.CmbOtTheatre.OnApplyTemplate();
            //this.Loaded += new RoutedEventHandler(OTTableMaster_Loaded);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {   
            this.DataContext = new clsOTTableVO();
            SetCommandButtonState("Load");
           // Validation();
            FillOtTheatre();
        }


        /// <summary>
        /// Fills Operation theatre combo
        /// </summary>
        void FillOtTheatre()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_OTTheatreMaster;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {

                    if (args.Error == null && args.Result != null)
                    {

                        List<MasterListItem> objList = new List<MasterListItem>();


                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                        CmbOtTheatre.ItemsSource = null;
                        CmbOtTheatre.ItemsSource = objList;
                        //CmbOtTheatre.SelectedItem = objList[0];
                        //Validation();

                    }

                    if (this.DataContext != null)
                    {
                        CmbOtTheatre.SelectedValue = ((clsOTTableVO)this.DataContext).OTTheatreID;
                    }


                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        void OTTableMaster_Loaded(object sender, RoutedEventArgs e)
        {
            //Validation();
        }
        /// <summary>
        /// Get called when  front panel grid refreshed
        /// </summary>
        /// <param name="sender"> grid</param>
        /// <param name="e">grid refresh</param>
        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            SetupPage();
        }

        clsOTTableVO getstoreinfo;

        /// <summary>
        /// Fills front panel grid
        /// </summary>
        public void SetupPage()
        {
            try
            {
                clsGetOTTableDetailsBizActionVO bizActionVO = new clsGetOTTableDetailsBizActionVO();
                bizActionVO.SearchExpression = txtSearch.Text;
                bizActionVO.PagingEnabled = true;
                bizActionVO.MaximumRows = MasterList.PageSize;
                bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
                getstoreinfo = new clsOTTableVO();
                bizActionVO.OtTableMatserDetails = new List<clsOTTableVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.OtTableMatserDetails = (((clsGetOTTableDetailsBizActionVO)args.Result).OtTableMatserDetails);
                        if (bizActionVO.OtTableMatserDetails.Count > 0)
                        {
                            MasterList.Clear();
                            MasterList.TotalItemCount = (int)(((clsGetOTTableDetailsBizActionVO)args.Result).TotalRows);
                            foreach (clsOTTableVO item in bizActionVO.OtTableMatserDetails)
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
                throw;
            }
        }

        /// <summary>
        /// set command button set
        /// </summary>
        /// <param name="strFormMode">button content</param>
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
                    //cmdClose.Visibility = Visibility.Visible;
                    //cmdCancel.Visibility = Visibility.Collapsed;
                    break;

                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdAdd.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    //cmdClose.Visibility = Visibility.Collapsed;
                    //cmdCancel.Visibility = Visibility.Visible;
                    break;

                case "Save":
                    cmdAdd.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    //cmdClose.Visibility = Visibility.Visible;
                    //cmdCancel.Visibility = Visibility.Collapsed;
                    break;

                case "Modify":
                    cmdAdd.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    //cmdClose.Visibility = Visibility.Visible;
                    //cmdCancel.Visibility = Visibility.Collapsed; 
                    break;

                case "Cancel":
                    cmdAdd.IsEnabled = true;
                    //cmdCancel.Visibility = Visibility.Collapsed;
                    //cmdClose.Visibility = Visibility.Visible;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
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
                    IsCancel = false;
                    //cmdClose.Visibility = Visibility.Collapsed;
                    //cmdCancel.Visibility = Visibility.Visible; 
                    break;

                default:
                    break;
            }
        }
        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            this.grdBackPanel.DataContext = new clsOTTableVO();
            ClearUI();

            isModify = false;
            SetCommandButtonState("New");
            try
            {
                objAnimation.Invoke(RotationType.Forward);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// clears UI
        /// </summary>
        public void ClearUI()
        {
            //txtStoreCode.Text = "";
            //txtStoreName.Text = "";


            //CmbOtTheatre.SelectedValue = ((clsOTTableVO)this.DataContext).OTTheatreID;
            this.grdBackPanel.DataContext = new clsOTTableVO();
            CmbOtTheatre.SelectedValue = ((clsOTTableVO)this.DataContext).OTTheatreID;
        }

        /// <summary>
        /// gives confirmation message for save
        /// </summary>
        /// <param name="sender">Confirmation message ok button</param>
        /// <param name="e">Confirmation message ok button click</param>
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to save ?";
                //string msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("SaveVerification_Msg");
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);
                msgWin.Show();


            }

        }

        /// <summary>
        /// Saves OT Table
        /// </summary>
        /// <param name="result">Message  Box result</param>
        void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            try
            {
                if (result == MessageBoxResult.Yes)
                {
                    clsAddUpdateOTTableDetailsBizActionVO bizactionVO = new clsAddUpdateOTTableDetailsBizActionVO();
                    clsOTTableVO addNewOTTableVO = new clsOTTableVO();

                    isModify = false;
                    addNewOTTableVO = (clsOTTableVO)this.DataContext;
                    addNewOTTableVO = CreateFormData();

                    addNewOTTableVO.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewOTTableVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewOTTableVO.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewOTTableVO.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewOTTableVO.AddedDateTime = System.DateTime.Now;
                    addNewOTTableVO.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;

                    bizactionVO.OTTableMasterMatserDetails.Add(addNewOTTableVO);

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpdateOTTableDetailsBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                msgText = "Record Saved Successfully.";
                                //msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("RecordSaved_Msg");
                                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                OTTableID = 0;
                                SetupPage();
                                objAnimation.Invoke(RotationType.Backward);
                                SetCommandButtonState("Save");
                            }
                            else if (((clsAddUpdateOTTableDetailsBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                msgText = "Record cannot be save because CODE  already exist !";
                                //msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("CodeDuplicate_Msg");
                                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            }
                            else if (((clsAddUpdateOTTableDetailsBizActionVO)args.Result).SuccessStatus == 3)
                            {
                                msgText = "Record cannot be save because DESCRIPTION already exist !";
                                //msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("DescriptionDuplicate_Msg");
                                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            }
                        }
                    };
                    client.ProcessAsync(bizactionVO, new clsUserVO());
                    client.CloseAsync();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets ID of existing OTTable or new OT Table
        /// </summary>
        /// <returns>clsOTTableVO object</returns>
        private clsOTTableVO CreateFormData()
        {
            clsOTTableVO addNewOTTableVO = new clsOTTableVO();
            if (isModify == true)
            {
                addNewOTTableVO.ID = OTTableID;
                addNewOTTableVO = ((clsOTTableVO)grdOTTable.SelectedItem);
            }
            else
            {
                addNewOTTableVO.ID = 0;
                addNewOTTableVO = (clsOTTableVO)this.grdBackPanel.DataContext;
                addNewOTTableVO.Status = true;
            }

            addNewOTTableVO.OTTheatreID = ((MasterListItem)CmbOtTheatre.SelectedItem).ID;



            return addNewOTTableVO;
        }

        /// <summary>
        /// gives confirmation message for modify
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Validation())
                {
                    string msgTitle = "";
                    string msgText = "Are you sure you want to modify ?";
                    //string msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ModifyVerification_Msg");

                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                    msgW.Show();
                }
            }
            catch (Exception Ex)
            {
                throw;
            }

        }

        /// <summary>
        /// Modifies OT Table Details
        /// </summary>
        /// <param name="result"> MessageBoxResult</param>
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsAddUpdateOTTableDetailsBizActionVO bizactionVO = new clsAddUpdateOTTableDetailsBizActionVO();
                clsOTTableVO addNewOtTableVO = new clsOTTableVO();
                try
                {
                    isModify = true;
                    addNewOtTableVO = CreateFormData();
                    addNewOtTableVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewOtTableVO.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewOtTableVO.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewOtTableVO.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewOtTableVO.UpdatedDateTime = System.DateTime.Now;
                    addNewOtTableVO.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.OTTableMasterMatserDetails.Add(addNewOtTableVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpdateOTTableDetailsBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                msgText = "Record Saved Successfully.";
                                //msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("RecordSaved_Msg");

                                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                OTTableID = 0;
                                SetupPage();
                                objAnimation.Invoke(RotationType.Backward);
                                SetCommandButtonState("Modify");
                            }
                            else if (((clsAddUpdateOTTableDetailsBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                msgText = "Record cannot be save because CODE already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpdateOTTableDetailsBizActionVO)args.Result).SuccessStatus == 3)
                            {
                                msgText = "Record cannot be save because CODE  already exist !";
                                //msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("CodeDuplicate_Msg");
                                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
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
        /// Cancel button Click
        /// </summary>
        /// <param name="sender">Cancel button</param>
        /// <param name="e">Cancel button click</param>
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetCommandButtonState("Cancel");
                SetupPage();
                objAnimation.Invoke(RotationType.Backward);
                if (IsCancel == true)
                {
                    ModuleName = "PalashDynamics.Administration";
                    Action = "PalashDynamics.Administration.frmOTConfiguration";
                    UserControl rootPage = Application.Current.RootVisual as UserControl;

                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                    mElement.Text = "OT Configuration";

                    WebClient c = new WebClient();
                    c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
                    c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                }
                else
                {
                    IsCancel = true;
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// if cancel button clicked on front panel it goes to config screen
        /// </summary>
        /// <param name="sender">Click button</param>
        /// <param name="e">Click Button Click</param>
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

        /// <summary>
        /// Fills front panel grid as per search criteria
        /// </summary>
        /// <param name="sender">Search Button</param>
        /// <param name="e">Search Button click</param>
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MasterList = new PagedSortableCollectionView<clsOTTableVO>();
                MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                PageSize = 15;
                this.dataGrid2Pager.DataContext = MasterList;
                this.grdOTTable.DataContext = MasterList;
                SetupPage();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        /// <summary>
        /// Displays OT table details of front panel grid selected OT Table
        /// </summary>
        /// <param name="sender">View Hyperlink</param>
        /// <param name="e">View Hyperlink click</param>
        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetCommandButtonState("View");
                if (grdOTTable.SelectedItem != null)
                {
                    OTTableID = ((clsOTTableVO)grdOTTable.SelectedItem).ID;
                    grdBackPanel.DataContext = ((clsOTTableVO)grdOTTable.SelectedItem);
                    CmbOtTheatre.SelectedValue = ((clsOTTableVO)grdOTTable.SelectedItem).OTTheatreID;
                    objAnimation.Invoke(RotationType.Forward);
                }
            }
            catch (Exception ex)
            {
                throw;
            }


        }
        /// <summary>
        /// For Status updation of OT Table
        /// </summary>
        /// <param name="sender">Status Check box on front table grid</param>
        /// <param name="e">Status Check box on front table grid checked</param>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                clsAddUpdateOTTableDetailsBizActionVO bizactionVO = new clsAddUpdateOTTableDetailsBizActionVO();
                clsOTTableVO addNewOtTableVO = new clsOTTableVO();
                if (grdOTTable.SelectedItem != null)
                {
                    addNewOtTableVO.ID = ((clsOTTableVO)grdOTTable.SelectedItem).ID;
                    addNewOtTableVO.OTTheatreID = ((clsOTTableVO)grdOTTable.SelectedItem).OTTheatreID;
                    addNewOtTableVO.Code = ((clsOTTableVO)grdOTTable.SelectedItem).Code;
                    addNewOtTableVO.TheatreName = ((clsOTTableVO)grdOTTable.SelectedItem).TheatreName; 
                    addNewOtTableVO.Description = ((clsOTTableVO)grdOTTable.SelectedItem).Description;
                    addNewOtTableVO.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
                    addNewOtTableVO.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewOtTableVO.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewOtTableVO.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewOtTableVO.UpdatedDateTime = System.DateTime.Now;
                    addNewOtTableVO.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.OTTableMasterMatserDetails.Add(addNewOtTableVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpdateOTTableDetailsBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                OTTableID = 0;
                                SetupPage();
                                msgText = "Status Updated Sucessfully";
                                //msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("lblStatusUpdatedSucessfully");
                                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                objAnimation.Invoke(RotationType.Backward);
                                SetCommandButtonState("Modify");
                            }
                        }
                    };
                    client.ProcessAsync(bizactionVO, new clsUserVO());
                    client.CloseAsync();
                }
            }

            catch (Exception Ex)
            {
                throw;
            }
        }
        /// <summary>
        /// checks code emty or null
        /// </summary>
        /// <param name="sender">Code Text Box</param>
        /// <param name="e">CodeText Box lost focus</param>
        private void txtStoreCode_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
                {
                    ((TextBox)e.OriginalSource).ClearValidationError();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private void txtSearch_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MasterList = new PagedSortableCollectionView<clsOTTableVO>();
                MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                //PageSize = 15;
                dataGrid2Pager.PageIndex = 0;
                SetupPage();
                this.dataGrid2Pager.DataContext = MasterList;
                this.grdOTTable.DataContext = MasterList;
            }
        }


    }
}
