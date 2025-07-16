using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Administration.IPD;
using PalashDynamics.Collections;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using CIMS;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Reflection;
using System.Windows.Input;

namespace PalashDynamics.Administration
{
    public partial class frmRoomMaster : UserControl, INotifyPropertyChanged
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

        #region  Variables
        private SwivelAnimation objAnimation;
        string msgTitle = ""; // "PALASHDYNAMICS";
        string msgText = "";
        long OTTableID;

        public bool isModify = false;
        public PagedSortableCollectionView<clsIPDRoomMasterVO> MasterList { get; private set; }
        List<MasterListItem> objRoomMasterList = null;
        public bool IsCancel = true;
        public string ModuleName { get; set; }
        public string Action { get; set; }
        clsIPDRoomMasterVO getstoreinfo;
        public bool Ammenities = false;
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

        #region Constructor and Loaded
        public frmRoomMaster()
        {
            InitializeComponent();
            this.DataContext = new clsIPDRoomMasterVO();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            this.Loaded += new RoutedEventHandler(Room_Master_Loaded);
            SetCommandButtonState("Load");
            FillRoomAmmenityMaster();
            //FillWardMaster();
            //FillBedCategoryMaster();
            MasterList = new PagedSortableCollectionView<clsIPDRoomMasterVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            //this.dataGrid2Pager.DataContext = MasterList;
            this.grRoom.DataContext = MasterList;
            PageSize = 15;
            dataGrid2Pager.PageSize = PageSize;
            dataGrid2Pager.Source = MasterList;
            SetupPage();
        }
        void Room_Master_Loaded(object sender, RoutedEventArgs e)
        {

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
        #endregion

        #region Button Click Events
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
                    clsIPDAddUpdateRoomMasterBizActionVO bizActionVO = new clsIPDAddUpdateRoomMasterBizActionVO();
                    clsIPDRoomMasterVO addNewBlockVO = new clsIPDRoomMasterVO();

                    isModify = false;
                    addNewBlockVO = (clsIPDRoomMasterVO)this.DataContext;
                    addNewBlockVO = CreateFormData();
                    addNewBlockVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewBlockVO.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    // addNewBlockVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewBlockVO.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewBlockVO.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewBlockVO.AddedDateTime = System.DateTime.Now;
                    addNewBlockVO.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    addNewBlockVO.IsAmmenity = Ammenities;
                    bizActionVO.objRoomMatserDetails.Add(addNewBlockVO);

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsIPDAddUpdateRoomMasterBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                msgText = "Record is successfully submitted!";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                //After Insertion Back to BackPanel and Setup Page
                                OTTableID = 0;
                                SetupPage();

                                objAnimation.Invoke(RotationType.Backward);
                                SetCommandButtonState("Save");
                            }
                            else if (((clsIPDAddUpdateRoomMasterBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                msgText = "Record cannot be save because CODE already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsIPDAddUpdateRoomMasterBizActionVO)args.Result).SuccessStatus == 3)
                            {
                                msgText = "Record cannot be save because DESCRIPTION already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                        }
                    };
                    client.ProcessAsync(bizActionVO, new clsUserVO());
                    client.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Validation())
                {
                    string msgTitle = "";
                    string msgText = "Are you sure you want to Update ?";

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
                clsIPDAddUpdateRoomMasterBizActionVO bizactionVO = new clsIPDAddUpdateRoomMasterBizActionVO();
                clsIPDRoomMasterVO addNewOtTableVO = new clsIPDRoomMasterVO();
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
                    addNewOtTableVO.IsAmmenity = Ammenities;
                    bizactionVO.objRoomMatserDetails.Add(addNewOtTableVO);

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsIPDAddUpdateRoomMasterBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                msgText = "Record is successfully submitted!";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                //After Updation Back to BackPanel and Setup Page
                                OTTableID = 0;
                                SetupPage();
                                ClearUI();
                                objAnimation.Invoke(RotationType.Backward);
                                SetCommandButtonState("Modify");
                            }
                            else if (((clsIPDAddUpdateRoomMasterBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                msgText = "Record cannot be save because CODE already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsIPDAddUpdateRoomMasterBizActionVO)args.Result).SuccessStatus == 3)
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
                    throw;
                }
            }
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            this.grdBackPanel.DataContext = new clsIPDWardMasterVO();
            ClearUI();
            Validation();
            isModify = false;
            txtStoreCode.Focus();
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

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetCommandButtonState("Cancel");
                objAnimation.Invoke(RotationType.Backward);
                if (IsCancel == true)
                {
                    ModuleName = "PalashDynamics.Administration";
                    Action = "PalashDynamics.Administration.frmIPDAdmissionConfiguration";
                    UserControl rootPage = Application.Current.RootVisual as UserControl;

                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                    mElement.Text = "IPD Configuration";

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
                throw;
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MasterList = new PagedSortableCollectionView<clsIPDRoomMasterVO>();
                MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                PageSize = 15;
                this.dataGrid2Pager.DataContext = MasterList;
                this.grRoom.DataContext = MasterList;
                SetupPage();
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
                SetCommandButtonState("View");
                Validation();

                if (grRoom.SelectedItem != null)
                {
                    cmdModify.IsEnabled = ((clsIPDRoomMasterVO)grRoom.SelectedItem).Status;
                    OTTableID = ((clsIPDRoomMasterVO)grRoom.SelectedItem).ID;
                    grdBackPanel.DataContext = ((clsIPDRoomMasterVO)grRoom.SelectedItem).DeepCopy();
                    clsIPDRoomMasterVO objRoom = ((clsIPDRoomMasterVO)grRoom.SelectedItem);
                    objRoomMasterList.ForEach(z => z.Status = false);
                    List<MasterListItem> objList = new List<MasterListItem>();
                    foreach (var item in objRoom.AmmenityDetails)
                    {
                        //objRoomMasterList.Where(z => z.ID == item.ID).ToList().ForEach(z => z.Status = item.Status);
                        foreach (var Beditem in objRoomMasterList)
                        {
                            if (Beditem.ID == item.ID)
                            {
                                Beditem.Status = item.Status;
                            }
                        }
                        dgAmmenityList.ItemsSource = null;
                        dgAmmenityList.ItemsSource = objRoomMasterList;
                    }
                    objAnimation.Invoke(RotationType.Forward);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Private Methods
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

        /// <summary>
        /// Fills Room Ammenity Master combo
        /// </summary>
        void FillRoomAmmenityMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_RoomAmmenitiesMaster;
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
                        objRoomMasterList = new List<MasterListItem>();
                        objRoomMasterList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        dgAmmenityList.ItemsSource = null;
                        dgAmmenityList.ItemsSource = objRoomMasterList;
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

        /// <summary>
        /// Fills front panel grid
        /// </summary>
        public void SetupPage()
        {
            try
            {
                clsIPDGetRoomMasterBizActionVO bizActionVO = new clsIPDGetRoomMasterBizActionVO();
                bizActionVO.SearchExpression = txtSearch.Text;
                bizActionVO.PagingEnabled = true;
                bizActionVO.MaximumRows = MasterList.PageSize;
                bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
                getstoreinfo = new clsIPDRoomMasterVO();
                bizActionVO.objRoomMasterDetails = new List<clsIPDRoomMasterVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.objRoomMasterDetails = (((clsIPDGetRoomMasterBizActionVO)args.Result).objRoomMasterDetails);
                        if (bizActionVO.objRoomMasterDetails.Count > 0)
                        {
                            MasterList.Clear();
                            MasterList.TotalItemCount = (int)(((clsIPDGetRoomMasterBizActionVO)args.Result).TotalRows);
                            foreach (clsIPDRoomMasterVO item in bizActionVO.objRoomMasterDetails)
                            {
                                MasterList.Add(item);
                            }
                            grRoom.ItemsSource = null;
                            grRoom.ItemsSource = MasterList;
                            dataGrid2Pager.Source = null;
                            dataGrid2Pager.PageSize = MasterList.PageSize;
                            dataGrid2Pager.Source = MasterList;
                            grRoom.UpdateLayout();
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
        /// clears UI
        /// </summary>
        public void ClearUI()
        {
            this.grdBackPanel.DataContext = new clsIPDRoomMasterVO();
            List<MasterListItem> lstGrd = new List<MasterListItem>();
            lstGrd = (List<MasterListItem>)dgAmmenityList.ItemsSource;
            foreach (var item in lstGrd)
            {
                item.Status = false;
            }
        }

        public bool Validation()
        {
            bool result = true;
            if (string.IsNullOrEmpty(txtStoreCode.Text))
            {
                txtStoreCode.SetValidation("Please Enter Code");
                txtStoreCode.RaiseValidationError();
                txtStoreCode.Focus();
                result = false;
            }
            else
                txtStoreCode.ClearValidationError();

            if (string.IsNullOrEmpty(txtStoreName.Text))
            {
                txtStoreName.SetValidation("Please Enter Bed No.");
                txtStoreName.RaiseValidationError();
                txtStoreName.Focus();
                result = false;
            }
            else
                txtStoreName.ClearValidationError();
            return result;
        }

        /// <summary>
        /// Gets ID of existing OTTable or new OT Table
        /// </summary>
        /// <returns>clsIPDBlockMasterVO object</returns>
        private clsIPDRoomMasterVO CreateFormData()
        {
            clsIPDRoomMasterVO addNewBedVO = new clsIPDRoomMasterVO();
            if (isModify == true)
            {
                addNewBedVO.ID = OTTableID;
                addNewBedVO = ((clsIPDRoomMasterVO)grRoom.SelectedItem);
            }
            else
            {
                addNewBedVO.ID = 0;
                addNewBedVO = (clsIPDRoomMasterVO)this.grdBackPanel.DataContext;
                addNewBedVO.Status = true;
            }
            //if (chkAllAmmenities.IsChecked==true)
            addNewBedVO.CheckedAllAmmenities = true;
            //else
            //  addNewBedVO.CheckedAllAmmenities = false;

            addNewBedVO.AmmenityDetails = (List<MasterListItem>)dgAmmenityList.ItemsSource;

            //foreach (var item in (List<MasterListItem>)dgAmmenityList.ItemsSource)
            //{
            //    addNewBedVO.AmmenityDetails[item].Status = ((List<MasterListItem>)dgAmmenityList.ItemsSource)[item].Status;
            //}

            //addNewBedVO.AmmenityDetails =(List<clsIPDRoomAmmenitiesMasterVO>)dgAmmenityList.
            // addNewBlockVO.UnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
            //addNewBedVO.WardID = ((MasterListItem)cmbWard.SelectedItem).ID;
            //addNewBedVO.RoomID = ((MasterListItem)cmbRoom.SelectedItem).ID;
            //addNewBedVO.BedCategoryID = ((MasterListItem)cmbBedCategory.SelectedItem).ID;
            return addNewBedVO;
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

        #endregion

        #region CheckBox Events
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                clsIPDAddUpdateRoomMasterBizActionVO bizactionVO = new clsIPDAddUpdateRoomMasterBizActionVO();
                clsIPDRoomMasterVO addNewOtTableVO = new clsIPDRoomMasterVO();
                if (grRoom.SelectedItem != null)
                {
                    addNewOtTableVO.ID = ((clsIPDRoomMasterVO)grRoom.SelectedItem).ID;
                    //addNewOtTableVO.OTTheatreID = ((clsIPDBlockMasterVO)grdBlock.SelectedItem).OTTheatreID;
                    addNewOtTableVO.Code = ((clsIPDRoomMasterVO)grRoom.SelectedItem).Code;
                    addNewOtTableVO.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
                    addNewOtTableVO.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewOtTableVO.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewOtTableVO.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewOtTableVO.UpdatedDateTime = System.DateTime.Now;
                    addNewOtTableVO.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.objRoomMatserDetails.Add(addNewOtTableVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsIPDAddUpdateRoomMasterBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                OTTableID = 0;
                                SetupPage();
                                msgText = "Status Updated Sucessfully";
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
            }
            catch (Exception Ex)
            {
                throw;
            }
        }

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

        private void chkStatus_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                clsIPDUpdateRoomStatusBizActionVO objStatus = new clsIPDUpdateRoomStatusBizActionVO();
                objStatus.RoomStatus = new clsIPDRoomMasterVO();
                // objStatus.RoomStatus = (clsIPDRoomMasterVO)dgAmmenityList.SelectedItem;
                objStatus.RoomStatus = (clsIPDRoomMasterVO)grRoom.SelectedItem;
                objStatus.RoomStatus.Status = ((clsIPDRoomMasterVO)grRoom.SelectedItem).Status;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (objStatus.RoomStatus.Status == false)
                        {
                            msgText = "Room Status Deactivated Successfully.";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                        }
                        else
                        {
                            msgText = "Room Status Activated Successfully.";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                        }

                    }
                    //SetCommandButtonState("View");

                };
                client.ProcessAsync(objStatus, new clsUserVO());
                client.CloseAsync();

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {

            bool isAmmenities = false;
            Ammenities = false;
            //if (((MasterListItem)dgAmmenityList.SelectedItem).Status == true)
            if (dgAmmenityList.SelectedItem != null)
            {
                MasterListItem objRoom = new MasterListItem();
                List<MasterListItem> objList = new List<MasterListItem>();

                objList = (List<MasterListItem>)dgAmmenityList.ItemsSource;
                objRoom = (MasterListItem)dgAmmenityList.SelectedItem;

                foreach (var item in objList)
                {
                    if (item == objRoom)
                    {
                        item.Status = objRoom.Status;
                    }
                }
                dgAmmenityList.ItemsSource = null;
                dgAmmenityList.ItemsSource = objList;

                foreach (var item in objList)
                {
                    if (item.Status == true)
                    {
                        Ammenities = true;
                    }
                }
            }
        }

        #endregion

        private void txtSearch_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                SetupPage();
                dataGrid2Pager.PageIndex = 0;
            }
        }

    }
}
