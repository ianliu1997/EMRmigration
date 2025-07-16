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
using PalashDynamics.Animations;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Administration.IPD;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using System.Reflection;
using CIMS;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;

namespace PalashDynamics.Administration
{
    public partial class frmWardMaster : UserControl, INotifyPropertyChanged
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
        public PagedSortableCollectionView<clsIPDWardMasterVO> MasterList { get; private set; }
        List<MasterListItem> objAminitiesList = new List<MasterListItem>();



        public bool IsCancel = true;
        public string ModuleName { get; set; }
        public string Action { get; set; }
        clsIPDWardMasterVO getstoreinfo;
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
            }
        }
        #endregion


        public frmWardMaster()
        {
            InitializeComponent();
            this.DataContext = new clsIPDWardMasterVO();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            this.Loaded += new RoutedEventHandler(Ward_Master_Loaded);
            SetCommandButtonState("Load");
            FillUnitMaster();
            FillBlockMaster();
            FillFloorMaster(0);
            FillWardTypeMaster();

            MasterList = new PagedSortableCollectionView<clsIPDWardMasterVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdWard.DataContext = MasterList;
            SetupPage();
        }

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

                    break;
                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdAdd.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    isModify = true;
                    IsCancel = false;
                    break;

                default:
                    break;
            }
        }

        void Ward_Master_Loaded(object sender, RoutedEventArgs e)
        {
            FillBlockMaster();
            FillUnitMaster();
            FillWardTypeMaster();
        }
        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            SetupPage();
        }

        /// <summary>
        /// Fills front panel grid
        /// </summary>
        public void SetupPage()
        {
            try
            {
                clsIPDGetWardMasterBizActionVO bizActionVO = new clsIPDGetWardMasterBizActionVO();
                bizActionVO.SearchExpression = txtSearch.Text;
                bizActionVO.PagingEnabled = true;
                bizActionVO.MaximumRows = MasterList.PageSize;
                bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
                getstoreinfo = new clsIPDWardMasterVO();
                bizActionVO.objWardMasterDetails = new List<clsIPDWardMasterVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.objWardMasterDetails = (((clsIPDGetWardMasterBizActionVO)args.Result).objWardMasterDetails);
                        if (bizActionVO.objWardMasterDetails.Count > 0)
                        {
                            MasterList.Clear();
                            MasterList.TotalItemCount = (int)(((clsIPDGetWardMasterBizActionVO)args.Result).TotalRows);
                            foreach (clsIPDWardMasterVO item in bizActionVO.objWardMasterDetails)
                            {
                                MasterList.Add(item);
                            }
                            grdWard.ItemsSource = null;
                            grdWard.ItemsSource = MasterList;
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

        void FillUnitMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
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
                        cmbUnit.ItemsSource = null;
                        cmbUnit.ItemsSource = objList;
                    }

                    if (this.DataContext != null)
                    {
                        cmbUnit.SelectedValue = ((clsIPDWardMasterVO)this.DataContext).UnitID;
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
        /// Fills Floor Master combo
        /// </summary>
        void FillFloorMaster(long BlockID)
        {
            try
            {
                if (BlockID > 0)
                {
                    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                    BizAction.MasterTable = MasterTableNameList.M_FloorMaster;
                    BizAction.Parent = new KeyValue();
                    BizAction.Parent.Key = "1";
                    BizAction.Parent.Value = "Status";
                    if (BlockID > 0)
                        BizAction.Parent = new KeyValue { Key = BlockID, Value = "BlockID" };
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
                            cmbFloor.ItemsSource = null;
                            cmbFloor.ItemsSource = objList;
                            cmbFloor.SelectedItem = objList[0];
                        }

                        if (grdBackPanel.DataContext != null)
                        {
                            cmbFloor.SelectedValue = ((clsIPDWardMasterVO)grdBackPanel.DataContext).FloorID;
                        }
                    };
                    client.ProcessAsync(BizAction, new clsUserVO());
                    client.CloseAsync();
                }
                else
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    cmbFloor.ItemsSource = null;
                    cmbFloor.ItemsSource = objList;
                    cmbFloor.SelectedItem = objList[0];
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        void FillWardTypeMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_WardTypeMaster;
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
                        cmbCategory.ItemsSource = null;
                        cmbCategory.ItemsSource = objList;
                    }

                    if (this.DataContext != null)
                    {
                        cmbCategory.SelectedValue = ((clsIPDWardMasterVO)this.DataContext).CategoryID;
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


        void FillBlockMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_BlockMaster;
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
                        cmbBlock.ItemsSource = null;
                        cmbBlock.ItemsSource = objList;
                        cmbBlock.SelectedItem = objList[0];
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


        public void ClearUI()
        {
            this.grdBackPanel.DataContext = new clsIPDWardMasterVO();
            cmbUnit.SelectedValue = ((clsIPDWardMasterVO)this.DataContext).UnitID;
            cmbFloor.SelectedValue = ((clsIPDWardMasterVO)this.DataContext).FloorID;
            cmbCategory.SelectedValue = ((clsIPDWardMasterVO)this.DataContext).CategoryID;
            cmbBlock.SelectedValue = ((clsIPDWardMasterVO)this.DataContext).BlockID;
            txtStoreName.Text = String.Empty;
            txtStoreCode.Text = String.Empty;
        }

        #region Validation
        public bool Validation()
        {
            if (string.IsNullOrEmpty(txtStoreCode.Text))
            {
                txtStoreCode.SetValidation("Please Enter Code");
                txtStoreCode.RaiseValidationError();
                txtStoreCode.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtStoreName.Text))
            {
                txtStoreName.SetValidation("Please Enter Description");
                txtStoreName.RaiseValidationError();
                txtStoreName.Focus();
                return false;
            }
            else if (cmbUnit.SelectedItem == null || ((MasterListItem)cmbUnit.SelectedItem).ID == 0)
            {
                cmbUnit.TextBox.SetValidation("Please select unit");
                cmbUnit.TextBox.RaiseValidationError();
                cmbUnit.TextBox.Focus();
                return false;
            }
            else if (cmbBlock.SelectedItem == null || ((MasterListItem)cmbBlock.SelectedItem).ID == 0)
            {
                cmbBlock.TextBox.SetValidation("Please select Block");
                cmbBlock.TextBox.RaiseValidationError();
                cmbBlock.TextBox.Focus();
                return false;
            }
            else if (cmbFloor.SelectedItem == null || ((MasterListItem)cmbFloor.SelectedItem).ID == 0)
            {
                cmbFloor.TextBox.SetValidation("Please select Floor");
                cmbFloor.TextBox.RaiseValidationError();
                cmbFloor.TextBox.Focus();
                return false;
            }
            else if (cmbCategory.SelectedItem == null || ((MasterListItem)cmbCategory.SelectedItem).ID == 0)
            {
                cmbCategory.TextBox.SetValidation("Please select Ward Type");
                cmbCategory.TextBox.RaiseValidationError();
                cmbCategory.TextBox.Focus();
                return false;
            }
            else
            {
                txtStoreCode.ClearValidationError();
                txtStoreName.ClearValidationError();
                cmbBlock.TextBox.ClearValidationError();
                cmbCategory.TextBox.ClearValidationError();
                cmbFloor.TextBox.ClearValidationError();
                cmbUnit.TextBox.ClearValidationError();
                return true;
            }
        }

        #endregion

        private clsIPDWardMasterVO CreateFormData()
        {
            clsIPDWardMasterVO addNewBlockVO = new clsIPDWardMasterVO();
            if (isModify == true)
            {
                addNewBlockVO.ID = OTTableID;
                addNewBlockVO = ((clsIPDWardMasterVO)grdWard.SelectedItem);
            }
            else
            {
                addNewBlockVO.ID = 0;
                addNewBlockVO = (clsIPDWardMasterVO)this.grdBackPanel.DataContext;
                addNewBlockVO.Status = true;
            }
            addNewBlockVO.UnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
            addNewBlockVO.CategoryID = ((MasterListItem)cmbCategory.SelectedItem).ID;
            addNewBlockVO.FloorID = ((MasterListItem)cmbFloor.SelectedItem).ID;
            return addNewBlockVO;
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

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            this.grdBackPanel.DataContext = new clsIPDWardMasterVO();
            ClearUI();
            isModify = false;
            Validation();
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

        void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            try
            {
                if (result == MessageBoxResult.Yes)
                {
                    clsIPDAddUpdateWardMasterBizActionVO bizActionVO = new clsIPDAddUpdateWardMasterBizActionVO();
                    clsIPDWardMasterVO addNewBlockVO = new clsIPDWardMasterVO();
                    if (isModify == true)
                    {
                        addNewBlockVO.ID = ((clsIPDWardMasterVO)grdWard.SelectedItem).ID;
                    }
                    addNewBlockVO.UnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
                    addNewBlockVO.BlockID = ((MasterListItem)cmbBlock.SelectedItem).ID;
                    addNewBlockVO.FloorID = ((MasterListItem)cmbFloor.SelectedItem).ID;
                    addNewBlockVO.CategoryID = ((MasterListItem)cmbCategory.SelectedItem).ID;
                    addNewBlockVO.Description = txtStoreName.Text;
                    addNewBlockVO.Code = txtStoreCode.Text;
                    addNewBlockVO.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewBlockVO.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewBlockVO.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewBlockVO.AddedDateTime = System.DateTime.Now;
                    addNewBlockVO.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;

                    bizActionVO.objWardMatserDetails.Add(addNewBlockVO);

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsIPDAddUpdateWardMasterBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                msgText = "Record is successfully submitted!";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                OTTableID = 0;
                                addNewBlockVO = new clsIPDWardMasterVO();
                                SetupPage();

                                objAnimation.Invoke(RotationType.Backward);
                                SetCommandButtonState("Save");
                            }
                            else if (((clsIPDAddUpdateWardMasterBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                msgText = "Record cannot be save because CODE already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsIPDAddUpdateWardMasterBizActionVO)args.Result).SuccessStatus == 3)
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

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsIPDAddUpdateWardMasterBizActionVO bizactionVO = new clsIPDAddUpdateWardMasterBizActionVO();
                clsIPDWardMasterVO addNewOtTableVO = new clsIPDWardMasterVO();
                try
                {
                    isModify = true;
                    addNewOtTableVO = CreateFormData();

                    if (isModify == true)
                    {
                        addNewOtTableVO.ID = ((clsIPDWardMasterVO)grdWard.SelectedItem).ID;
                    }
                    addNewOtTableVO.UnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
                    addNewOtTableVO.BlockID = ((MasterListItem)cmbBlock.SelectedItem).ID;
                    addNewOtTableVO.FloorID = ((MasterListItem)cmbFloor.SelectedItem).ID;
                    addNewOtTableVO.CategoryID = ((MasterListItem)cmbCategory.SelectedItem).ID;
                    addNewOtTableVO.Description = txtStoreName.Text;
                    addNewOtTableVO.Code = txtStoreCode.Text;

                    addNewOtTableVO.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewOtTableVO.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewOtTableVO.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewOtTableVO.UpdatedDateTime = System.DateTime.Now;
                    addNewOtTableVO.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.objWardMatserDetails.Add(addNewOtTableVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsIPDAddUpdateWardMasterBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                msgText = "Record is successfully submitted!";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                OTTableID = 0;
                                SetupPage();
                                objAnimation.Invoke(RotationType.Backward);
                                SetCommandButtonState("Modify");
                            }
                            else if (((clsIPDAddUpdateWardMasterBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                msgText = "Record cannot be save because CODE already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsIPDAddUpdateWardMasterBizActionVO)args.Result).SuccessStatus == 3)
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
                    mElement.Text = "Admission Configuration";

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
                MasterList = new PagedSortableCollectionView<clsIPDWardMasterVO>();
                MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                PageSize = 15;
                this.dataGrid2Pager.DataContext = MasterList;
                this.grdWard.DataContext = MasterList;
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
                cmdModify.IsEnabled = ((clsIPDWardMasterVO)grdWard.SelectedItem).Status;
                Validation();
               
                if (grdWard.SelectedItem != null)
                {
                    OTTableID = ((clsIPDWardMasterVO)grdWard.SelectedItem).ID;
                    grdBackPanel.DataContext = ((clsIPDWardMasterVO)grdWard.SelectedItem).DeepCopy();
                    cmbUnit.SelectedValue = ((clsIPDWardMasterVO)grdWard.SelectedItem).UnitID;
                    cmbBlock.SelectedValue = ((clsIPDWardMasterVO)grdWard.SelectedItem).BlockID;
                    cmbFloor.SelectedValue = ((clsIPDWardMasterVO)grdWard.SelectedItem).FloorID;
                    cmbCategory.SelectedValue = ((clsIPDWardMasterVO)grdWard.SelectedItem).CategoryID;
                    objAnimation.Invoke(RotationType.Forward);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e) 
        {
            try
            {
                clsIPDAddUpdateWardMasterBizActionVO bizactionVO = new clsIPDAddUpdateWardMasterBizActionVO();
                clsIPDWardMasterVO addNewOtTableVO = new clsIPDWardMasterVO();
                if (grdWard.SelectedItem != null)
                {
                    addNewOtTableVO.ID = ((clsIPDWardMasterVO)grdWard.SelectedItem).ID;
                    addNewOtTableVO.Code = ((clsIPDWardMasterVO)grdWard.SelectedItem).Code;
                    addNewOtTableVO.UnitName = ((clsIPDWardMasterVO)grdWard.SelectedItem).UnitName;
                    addNewOtTableVO.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
                    addNewOtTableVO.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewOtTableVO.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewOtTableVO.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewOtTableVO.UpdatedDateTime = System.DateTime.Now;
                    addNewOtTableVO.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.objWardMatserDetails.Add(addNewOtTableVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsIPDAddUpdateWardMasterBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                OTTableID = 0;
                                SetupPage();
                                msgText = "Status Updated Successfully";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
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

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsIPDUpdateWardMasterStatusBizActionVO objStatus = new clsIPDUpdateWardMasterStatusBizActionVO();
                objStatus.WardStatus = new clsIPDWardMasterVO();
                objStatus.WardStatus = (clsIPDWardMasterVO)grdWard.SelectedItem;
                objStatus.WardStatus.Status = ((clsIPDWardMasterVO)grdWard.SelectedItem).Status;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (objStatus.WardStatus.Status == false)
                        {
                            msgText = "Ward Status Deactivated Successfully.";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                        }
                        else
                        {
                            msgText = "Ward Status Activated Successfully.";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                        }

                    }
                };
                client.ProcessAsync(objStatus, new clsUserVO());
                client.CloseAsync();

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private void cmbBlock_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbBlock.SelectedItem != null)
            {
                FillFloorMaster(((MasterListItem)cmbBlock.SelectedItem).ID);
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                MasterList = new PagedSortableCollectionView<clsIPDWardMasterVO>();
                MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                PageSize = 15;
                this.dataGrid2Pager.DataContext = MasterList;
                this.grdWard.DataContext = MasterList;
                SetupPage();
            }
        }
    }
}
